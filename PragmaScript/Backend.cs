using LLVMSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PragmaScript
{


    class Backend
    {
        public enum BackendType
        {
            Float32, Int32
        }

        struct TypedValue
        {
            public BackendType type;
            public LLVMValueRef value;
            public TypedValue(LLVMValueRef value, BackendType type)
            {
                this.value = value;
                this.type = type;
            }
        }
        readonly LLVMBool llvmTrue = new LLVMBool(1);
        readonly LLVMBool llvmFalse = new LLVMBool(0);

        Stack<TypedValue> valueStack = new Stack<TypedValue>();
        Dictionary<string, TypedValue> variables = new Dictionary<string, TypedValue>();

        LLVMModuleRef mod;
        LLVMBuilderRef builder;
        LLVMValueRef mainFunction;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int llvm_main();

        void prepareModule()
        {
            mod = LLVM.ModuleCreateWithName("WhatIsThisIDontEven");

            LLVMTypeRef[] param_types = { LLVM.Int32Type(), LLVM.Int32Type() };
            LLVMTypeRef ret_type = LLVM.FunctionType(LLVM.Int32Type(), out param_types[0], 0, llvmFalse);
            mainFunction = LLVM.AddFunction(mod, "main", ret_type);

            LLVMBasicBlockRef entry = LLVM.AppendBasicBlock(mainFunction, "entry");

            builder = LLVM.CreateBuilder();
            LLVM.PositionBuilderAtEnd(builder, entry);
        }

        void executeModule(bool useOptimizationPasses = true)
        {
            IntPtr error;

            var verifyFunction = LLVM.VerifyFunction(mainFunction, LLVMVerifierFailureAction.LLVMReturnStatusAction);
            if (verifyFunction.Value != 0)
            {
                Console.WriteLine("VerifyFunction error!");
            }

            var verifyModule = LLVM.VerifyModule(mod, LLVMVerifierFailureAction.LLVMReturnStatusAction, out error);
            if (verifyModule.Value != 0)
            {
                var s = Marshal.PtrToStringAnsi(error);
                Console.WriteLine("VerifyModule error: " + s);
            }
            LLVM.DisposeMessage(error);
            
            LLVMExecutionEngineRef engine;

            LLVM.LinkInMCJIT();
            LLVM.InitializeX86Target();
            LLVM.InitializeX86TargetInfo();
            LLVM.InitializeX86TargetMC();
            LLVM.InitializeX86AsmPrinter();
            LLVM.InitializeX86Disassembler();

            var platform = Environment.OSVersion.Platform;
            if (platform == PlatformID.Win32NT) // On Windows, LLVM currently (3.6) does not support PE/COFF
            {
                LLVM.SetTarget(mod, Marshal.PtrToStringAnsi(LLVM.GetDefaultTargetTriple()) + "-elf");
            }

            var options = new LLVMMCJITCompilerOptions();
            var optionsSize = (4 * sizeof(int)) + IntPtr.Size; // LLVMMCJITCompilerOptions has 4 ints and a pointer
            options.OptLevel = 3;
            LLVM.InitializeMCJITCompilerOptions(out options, optionsSize);

            var compileError = LLVM.CreateMCJITCompilerForModule(out engine, mod, out options, optionsSize, out error);
            if (compileError.Value != 0)
            {
                var s = Marshal.PtrToStringAnsi(error);
                Console.WriteLine();
                Console.WriteLine("error: " + s);
                Console.WriteLine();
            }

            LLVMPassManagerRef pass = LLVM.CreatePassManager();
            LLVM.AddTargetData(LLVM.GetExecutionEngineTargetData(engine), pass);
            if (useOptimizationPasses)
            {
                LLVM.AddConstantPropagationPass(pass);
                LLVM.AddInstructionCombiningPass(pass);
                LLVM.AddPromoteMemoryToRegisterPass(pass);
                LLVM.AddGVNPass(pass);
                LLVM.AddCFGSimplificationPass(pass);
                LLVM.AddGlobalOptimizerPass(pass);
                LLVM.AddVerifierPass(pass);
                LLVM.RunPassManager(pass, mod);
            }
            else
            {
                LLVM.AddVerifierPass(pass);
                LLVM.RunPassManager(pass, mod);
            }

            var mainFunctionDelegate = (llvm_main)Marshal.GetDelegateForFunctionPointer(LLVM.GetPointerToGlobal(engine, mainFunction), typeof(llvm_main));
            var answer = mainFunctionDelegate();
            //if (LLVM.WriteBitcodeToFile(mod, "main.bc") != 0)
            //{
            //    Console.WriteLine("error writing bitcode to file, skipping");
            //}

            LLVM.DumpModule(mod);

            LLVM.DisposeBuilder(builder);
            LLVM.DisposeExecutionEngine(engine);


            Console.WriteLine();
            Console.WriteLine("THE ANSWER IS: " + answer);
            Console.WriteLine();
        }
        public void EmitAndRun(AST.Node root, bool useOptimizations)
        {
            prepareModule();
            Visit(root);
            executeModule(useOptimizations);
        }

        public void Visit(AST.ConstInt32 node)
        {
            var tv = new TypedValue(LLVM.ConstInt(LLVM.Int32Type(), (ulong)node.number, llvmTrue), BackendType.Int32);
            valueStack.Push(tv);
        }

        public void Visit(AST.ConstFloat32 node)
        {
            var tv = new TypedValue(LLVM.ConstReal(LLVM.FloatType(), node.number), BackendType.Float32);
            valueStack.Push(tv);
        }

        public void Visit(AST.BinOp node)
        {
            Visit(node.left);
            Visit(node.right);

            var right = valueStack.Pop();
            var left = valueStack.Pop();

            if (left.type != right.type)
            {
                throw new BackendTypeMismatchException(left.type, right.type);
            }

            LLVMValueRef result;
            switch (left.type)
            {
                case BackendType.Int32:
                    switch (node.type)
                    {
                        case AST.BinOp.BinOpType.Add:
                            result = LLVM.BuildAdd(builder, left.value, right.value, "add_tmp");
                            break;
                        case AST.BinOp.BinOpType.Subract:
                            result = LLVM.BuildSub(builder, left.value, right.value, "sub_tmp");
                            break;
                        case AST.BinOp.BinOpType.Multiply:
                            result = LLVM.BuildMul(builder, left.value, right.value, "mul_tmp");
                            break;
                        case AST.BinOp.BinOpType.Divide:
                            result = LLVM.BuildSDiv(builder, left.value, right.value, "div_tmp");
                            break;
                        default:
                            throw new InvalidCodePath();
                    }
                    break;
                case BackendType.Float32:
                    switch (node.type)
                    {
                        case AST.BinOp.BinOpType.Add:
                            result = LLVM.BuildFAdd(builder, left.value, right.value, "fadd_tmp");
                            break;
                        case AST.BinOp.BinOpType.Subract:
                            result = LLVM.BuildFSub(builder, left.value, right.value, "fsub_tmp");
                            break;
                        case AST.BinOp.BinOpType.Multiply:
                            result = LLVM.BuildFMul(builder, left.value, right.value, "fmul_tmp");
                            break;
                        case AST.BinOp.BinOpType.Divide:
                            result = LLVM.BuildFDiv(builder, left.value, right.value, "fdiv_tmp");
                            break;
                        default:
                            throw new InvalidCodePath();
                    }
                    break;
                default:
                    throw new InvalidCodePath();
            }

            valueStack.Push(new TypedValue(result, left.type));
        }

        public void Visit(AST.UnaryOp node)
        {
            Visit(node.expression);

            var v = valueStack.Pop();

            TypedValue result = v;
            switch (node.type)
            {
                case AST.UnaryOp.UnaryOpType.Add:
                    result = v;
                    break;
                case AST.UnaryOp.UnaryOpType.Subract:
                    result.value = LLVM.BuildNeg(builder, v.value, "neg_tmp");
                    break;
                default:
                    throw new InvalidCodePath();
            }
            valueStack.Push(result);
        }

        public void Visit(AST.TypeCastOp node)
        {
            Visit(node.expression);

            var v = valueStack.Pop();

            var typeName = node.type.name;
            TypedValue result;
            

            // TODO: check if integral type
            // TODO: handle non integral types
            // TODO: do i have to handle constant values differently?
            if (typeName == "float32")
            {
                result.type = BackendType.Float32;
                result.value = LLVM.BuildSIToFP(builder, v.value, LLVM.FloatType(), "float_cast");
                // result.value = LLVM.ConstFPCast(v.value, LLVM.FloatType());
            }
            else if (typeName == "int32")
            {
                result.type = BackendType.Int32;
                result.value = LLVM.BuildFPToSI(builder, v.value, LLVM.Int32Type(), "int_cast");
            }
            else
            {
                throw new InvalidCodePath();
            }
            valueStack.Push(result);
        }

        public void Visit(AST.VariableDeclaration node)
        {
            Visit(node.expression);

            var result = valueStack.Pop();

            LLVMValueRef v;
            switch (result.type)
            {
                case BackendType.Float32:
                    v = LLVM.BuildAlloca(builder, LLVM.FloatType(), node.variable.name);
                    break;
                case BackendType.Int32:
                    v = LLVM.BuildAlloca(builder, LLVM.Int32Type(), node.variable.name);
                    break;
                default:
                    throw new InvalidCodePath();
            }
            variables[node.variable.name] = new TypedValue(v, result.type);
            LLVM.BuildStore(builder, result.value, v);
        }

        public void Visit(AST.Assignment node)
        {
            Visit(node.expression);

            var result = valueStack.Pop();
            var v = variables[node.variable.name];

            if (v.type != result.type)
            {
                throw new BackendTypeMismatchException(result.type, v.type);
            }

            LLVM.BuildStore(builder, result.value, v.value);
        }

        public void Visit(AST.Block node)
        {
            foreach (var s in node.statements)
            {
                Visit(s);
            }
        }

        public void Visit(AST.VariableLookup node)
        {
            var v = variables[node.variableName];
            var l = LLVM.BuildLoad(builder, v.value, node.variableName);
            var value = new TypedValue(l, v.type);
            valueStack.Push(value);
        }

        public void Visit(AST.Return node)
        {
            if (node.expression != null)
            {
                Visit(node.expression);
                var v = valueStack.Pop();
                LLVM.BuildRet(builder, v.value);
            }
            else
            {
                LLVM.BuildRetVoid(builder);
            }
        }

        public void Visit(AST.Node node)
        {
            if (node is AST.ConstInt32)
            {
                Visit(node as AST.ConstInt32);
            }
            else if (node is AST.ConstFloat32)
            {
                Visit(node as AST.ConstFloat32);
            }
            else if (node is AST.BinOp)
            {
                Visit(node as AST.BinOp);
            }
            else if (node is AST.UnaryOp)
            {
                Visit(node as AST.UnaryOp);
            }
            else if (node is AST.TypeCastOp)
            {
                Visit(node as AST.TypeCastOp);
            }
            else if (node is AST.VariableDeclaration)
            {
                Visit(node as AST.VariableDeclaration);
            }
            else if (node is AST.Assignment)
            {
                Visit(node as AST.Assignment);
            }
            else if (node is AST.Block)
            {
                Visit(node as AST.Block);
            }
            else if (node is AST.VariableLookup)
            {
                Visit(node as AST.VariableLookup);
            }
            else if (node is AST.Return)
            {
                Visit(node as AST.Return);
            }
        }
    }

}
