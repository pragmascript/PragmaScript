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
            Float32, Int32, Bool
        }

        public class ExecutionContext
        {
            public LLVMValueRef function;
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

        public static class Const
        {
            public static readonly LLVMBool TrueBool = new LLVMBool(1);
            public static readonly LLVMBool FalseBool = new LLVMBool(0);
            public static readonly LLVMValueRef NegativeOneInt32 = LLVM.ConstInt(LLVM.Int32Type(), unchecked((ulong)-1), TrueBool);
            public static readonly LLVMValueRef OneInt32 = LLVM.ConstInt(LLVM.Int32Type(), 1, TrueBool);
            public static readonly LLVMValueRef OneFloat32 = LLVM.ConstReal(LLVM.FloatType(), 1.0);
            public static readonly LLVMValueRef True = LLVM.ConstInt(LLVM.Int1Type(), (ulong)1, FalseBool);
            public static readonly LLVMValueRef False = LLVM.ConstInt(LLVM.Int1Type(), (ulong)0, FalseBool);

            public static readonly LLVMTypeRef Float32Type = LLVM.FloatType();
            public static readonly LLVMTypeRef Int32Type = LLVM.Int32Type();
            public static readonly LLVMTypeRef BoolType = LLVM.Int1Type();

        }


        Stack<TypedValue> valueStack = new Stack<TypedValue>();
        Dictionary<string, TypedValue> variables = new Dictionary<string, TypedValue>();
        Dictionary<string, LLVMValueRef> functions = new Dictionary<string, LLVMValueRef>();

        public ExecutionContext ctx = new ExecutionContext();

        LLVMModuleRef mod;
        LLVMBuilderRef builder;
        LLVMValueRef mainFunction;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int llvm_main();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void void_del();
        public static void_del print;


        public Backend()
        {
            int i = 0;
            print += () =>
            {
                Console.WriteLine("Hello world: " + i++);
            };

            LLVMTypeRef[] print_int_param_types = { LLVM.Int32Type() };
            var print_int_fun_type = LLVM.FunctionType(LLVM.VoidType(), out print_int_param_types[0], 0, Const.FalseBool);
            IntPtr printPtr = Marshal.GetFunctionPointerForDelegate(print);
            var printFuncConst = LLVM.ConstIntToPtr(LLVM.ConstInt(LLVM.Int64Type(), (ulong)printPtr, Const.FalseBool), LLVM.PointerType(print_int_fun_type, 0));
            functions.Add("print", printFuncConst);
        }

        void prepareModule()
        {
            mod = LLVM.ModuleCreateWithName("WhatIsThisIDontEven");

            LLVMTypeRef[] main_param_types = { LLVM.Int32Type(), LLVM.Int32Type() };
            LLVMTypeRef main_fun_type = LLVM.FunctionType(LLVM.Int32Type(), out main_param_types[0], 0, Const.FalseBool);
            mainFunction = LLVM.AddFunction(mod, "main", main_fun_type);
            ctx.function = mainFunction;

            LLVMBasicBlockRef entry = LLVM.AppendBasicBlock(mainFunction, "entry");


            builder = LLVM.CreateBuilder();
            LLVM.PositionBuilderAtEnd(builder, entry);

            // LLVM.BuildCall(builder, printFuncConst, out args[0], 0, "");
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
            var tv = new TypedValue(LLVM.ConstInt(Const.Int32Type, (ulong)node.number, Const.TrueBool), BackendType.Int32);
            valueStack.Push(tv);
        }

        public void Visit(AST.ConstFloat32 node)
        {
            var tv = new TypedValue(LLVM.ConstReal(Const.Float32Type, node.number), BackendType.Float32);
            valueStack.Push(tv);
        }

        public void Visit(AST.ConstBool node)
        {
            var tv = new TypedValue();
            tv.type = BackendType.Bool;
            tv.value = node.value ? Const.True : Const.False;
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

            var resultType = left.type;
            LLVMValueRef result;
            switch (left.type)
            {
                case BackendType.Bool:
                    switch (node.type)
                    {
                        case AST.BinOp.BinOpType.LogicalAND:
                            result = LLVM.BuildAnd(builder, left.value, right.value, "and_tmp");
                            break;
                        case AST.BinOp.BinOpType.LogicalOR:
                            result = LLVM.BuildOr(builder, left.value, right.value, "or_tmp");
                            break;
                        case AST.BinOp.BinOpType.LogicalXOR:
                            result = LLVM.BuildXor(builder, left.value, right.value, "or_tmp");
                            break;
                        default:
                            throw new InvalidCodePath();
                    }
                    break;
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
                        case AST.BinOp.BinOpType.LeftShift:
                            result = LLVM.BuildShl(builder, left.value, right.value, "shl_tmp");
                            break;
                        case AST.BinOp.BinOpType.RightShift:
                            result = LLVM.BuildAShr(builder, left.value, right.value, "shr_tmp");
                            break;
                        case AST.BinOp.BinOpType.Remainder:
                            result = LLVM.BuildSRem(builder, left.value, right.value, "srem_tmp");
                            break;
                        case AST.BinOp.BinOpType.Equal:
                            result = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntEQ, left.value, right.value, "icmp_tmp");
                            resultType = BackendType.Bool;
                            break;
                        case AST.BinOp.BinOpType.NotEqual:
                            result = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntNE, left.value, right.value, "icmp_tmp");
                            resultType = BackendType.Bool;
                            break;
                        case AST.BinOp.BinOpType.Greater:
                            result = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntSGT, left.value, right.value, "icmp_tmp");
                            resultType = BackendType.Bool;
                            break;
                        case AST.BinOp.BinOpType.GreaterEqual:
                            result = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntSGE, left.value, right.value, "icmp_tmp");
                            resultType = BackendType.Bool;
                            break;
                        case AST.BinOp.BinOpType.Less:
                            result = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntSLT, left.value, right.value, "icmp_tmp");
                            resultType = BackendType.Bool;
                            break;
                        case AST.BinOp.BinOpType.LessEqual:
                            result = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntSLE, left.value, right.value, "icmp_tmp");
                            resultType = BackendType.Bool;
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
                        case AST.BinOp.BinOpType.Remainder:
                            result = LLVM.BuildFRem(builder, left.value, right.value, "frem_tmp");
                            break;
                        case AST.BinOp.BinOpType.Equal:
                            result = LLVM.BuildFCmp(builder, LLVMRealPredicate.LLVMRealOEQ, left.value, right.value, "fcmp_tmp");
                            resultType = BackendType.Bool;
                            break;
                        case AST.BinOp.BinOpType.NotEqual:
                            result = LLVM.BuildFCmp(builder, LLVMRealPredicate.LLVMRealONE, left.value, right.value, "fcmp_tmp");
                            resultType = BackendType.Bool;
                            break;
                        case AST.BinOp.BinOpType.Greater:
                            result = LLVM.BuildFCmp(builder, LLVMRealPredicate.LLVMRealOGT, left.value, right.value, "fcmp_tmp");
                            resultType = BackendType.Bool;
                            break;
                        case AST.BinOp.BinOpType.GreaterEqual:
                            result = LLVM.BuildFCmp(builder, LLVMRealPredicate.LLVMRealOGE, left.value, right.value, "fcmp_tmp");
                            resultType = BackendType.Bool;
                            break;
                        case AST.BinOp.BinOpType.Less:
                            result = LLVM.BuildFCmp(builder, LLVMRealPredicate.LLVMRealOLT, left.value, right.value, "fcmp_tmp");
                            resultType = BackendType.Bool;
                            break;
                        case AST.BinOp.BinOpType.LessEqual:
                            result = LLVM.BuildFCmp(builder, LLVMRealPredicate.LLVMRealOLE, left.value, right.value, "fcmp_tmp");
                            resultType = BackendType.Bool;
                            break;
                        default:
                            throw new InvalidCodePath();
                    }
                    break;
                default:
                    throw new InvalidCodePath();
            }

            valueStack.Push(new TypedValue(result, resultType));
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
                    switch (v.type)
                    {
                        case BackendType.Float32:
                            result.value = LLVM.BuildFNeg(builder, v.value, "fneg_tmp");
                            break;
                        case BackendType.Int32:
                            result.value = LLVM.BuildNeg(builder, v.value, "neg_tmp");
                            break;
                        case BackendType.Bool:
                            break;
                        default:
                            break;
                    }

                    break;
                case AST.UnaryOp.UnaryOpType.LogicalNOT:
                    if (v.type != BackendType.Bool)
                    {
                        throw new BackendTypeMismatchException(v.type, BackendType.Bool);
                    }
                    result.value = LLVM.BuildNot(builder, v.value, "not_tmp");
                    break;
                case AST.UnaryOp.UnaryOpType.Complement:
                    result.value = LLVM.BuildXor(builder, v.value, Const.NegativeOneInt32, "complement_tmp");
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
                result.value = LLVM.BuildSIToFP(builder, v.value, Const.Float32Type, "float32_cast");
                // result.value = LLVM.ConstFPCast(v.value, LLVM.FloatType());
            }
            else if (typeName == "int32")
            {
                result.type = BackendType.Int32;
                switch (v.type)
                {
                    case BackendType.Float32:
                        result.value = LLVM.BuildFPToSI(builder, v.value, Const.Int32Type, "int32_cast");
                        break;
                    case BackendType.Int32:
                        result = v;
                        break;
                    case BackendType.Bool:
                        result.value = LLVM.BuildZExt(builder, v.value, Const.Int32Type, "int32_cast");
                        break;
                    default:
                        throw new InvalidCodePath();
                }

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
                case BackendType.Bool:
                    v = LLVM.BuildAlloca(builder, LLVM.Int1Type(), node.variable.name);
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
            var result = new TypedValue(l, v.type);
            switch (node.inc)
            {
                case AST.VariableLookup.Incrementor.None:
                    break;
                
                case AST.VariableLookup.Incrementor.preIncrement:
                    if (v.type == BackendType.Int32)
                    {
                        result.value = LLVM.BuildAdd(builder, l, Const.OneInt32, "preinc");
                    }
                    if (v.type == BackendType.Float32)
                    {
                        result.value = LLVM.BuildFAdd(builder, l, Const.OneFloat32, "preinc");
                    }
                    LLVM.BuildStore(builder, result.value, v.value);
                    break;
                case AST.VariableLookup.Incrementor.preDecrement:
                    if (v.type == BackendType.Int32)
                    {
                        result.value = LLVM.BuildSub(builder, l, Const.OneInt32, "predec");
                    }
                    if (v.type == BackendType.Float32)
                    {
                        result.value = LLVM.BuildFSub(builder, l, Const.OneFloat32, "predec");
                    }
                    LLVM.BuildStore(builder, result.value, v.value);
                    break;
                case AST.VariableLookup.Incrementor.postIncrement:
                    var postinc = default(LLVMValueRef);
                    if (v.type == BackendType.Int32)
                    {
                        postinc = LLVM.BuildAdd(builder, l, Const.OneInt32, "postinc");
                    }
                    if (v.type == BackendType.Float32)
                    {
                        postinc = LLVM.BuildFAdd(builder, l, Const.OneFloat32, "postinc");
                    }
                    LLVM.BuildStore(builder, postinc, v.value);
                    break;
                case AST.VariableLookup.Incrementor.postDecrement:
                    var postdec = default(LLVMValueRef);
                    if (v.type == BackendType.Int32)
                    {
                        postdec = LLVM.BuildSub(builder, l, Const.OneInt32, "postdec");
                    }
                    if (v.type == BackendType.Float32)
                    {
                        postdec = LLVM.BuildFSub(builder, l, Const.OneFloat32, "postdec");
                    }
                    LLVM.BuildStore(builder, postdec, v.value);
                    break;
                default:
                    break;
            }
            valueStack.Push(result);
            
        }

        public void Visit(AST.FunctionCall node)
        {
            var f = functions[node.functionName];
            LLVMValueRef[] parameters = new LLVMValueRef[1];
            var v = LLVM.BuildCall(builder, f, out parameters[0], 0, "");
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

        public void Visit(AST.IfCondition node)
        {
            Visit(node.condition);
            var condition = valueStack.Pop();
            if (condition.type != BackendType.Bool)
            {
                throw new BackendTypeMismatchException(condition.type, BackendType.Bool);
            }

            var thenBlock = LLVM.AppendBasicBlock(ctx.function, "then");
            var elseBlock = LLVM.AppendBasicBlock(ctx.function, "else");
            var endIfBlock = LLVM.AppendBasicBlock(ctx.function, "endif");

            if (node.elseBlock != null)
            {
                LLVM.BuildCondBr(builder, condition.value, thenBlock, elseBlock);
            }
            else
            {
                LLVM.BuildCondBr(builder, condition.value, thenBlock, endIfBlock);
            }

            LLVM.PositionBuilderAtEnd(builder, thenBlock);
            Visit(node.thenBlock);
            var term = LLVM.GetBasicBlockTerminator(thenBlock);
            if (term.Pointer == IntPtr.Zero)
                LLVM.BuildBr(builder, endIfBlock);
            if (node.elseBlock != null)
            {
                LLVM.PositionBuilderAtEnd(builder, elseBlock);
                Visit(node.elseBlock);
                term = LLVM.GetBasicBlockTerminator(elseBlock);
                if (term.Pointer == IntPtr.Zero)
                    LLVM.BuildBr(builder, endIfBlock);
            }

            LLVM.PositionBuilderAtEnd(builder, endIfBlock);
        }

        public void Visit(AST.ForLoop node)
        {
            var loopPre = LLVM.AppendBasicBlock(ctx.function, "for_cond");
            var loopBody = LLVM.AppendBasicBlock(ctx.function, "for");
            var endFor = LLVM.AppendBasicBlock(ctx.function, "end_for");

            Visit(node.initializer);
            LLVM.BuildBr(builder, loopPre);

            LLVM.PositionBuilderAtEnd(builder, loopPre);
            Visit(node.condition);

            var condition = valueStack.Pop();
            if (condition.type != BackendType.Bool)
            {
                throw new BackendTypeMismatchException(condition.type, BackendType.Bool);
            }
            LLVM.BuildCondBr(builder, condition.value, loopBody, endFor);

            LLVM.PositionBuilderAtEnd(builder, loopBody);
            Visit(node.loopBody);
            Visit(node.iterator);

            LLVM.BuildBr(builder, loopPre);

            LLVM.PositionBuilderAtEnd(builder, endFor);
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
            else if (node is AST.ConstBool)
            {
                Visit(node as AST.ConstBool);
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
            else if (node is AST.FunctionCall)
            {
                Visit(node as AST.FunctionCall);
            }
            else if (node is AST.IfCondition)
            {
                Visit(node as AST.IfCondition);
            }
            else if (node is AST.ForLoop)
            {
                Visit(node as AST.ForLoop);
            }
            else if (node is AST.Return)
            {
                Visit(node as AST.Return);
            }
        }
    }

}
