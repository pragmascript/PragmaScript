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
        readonly LLVMBool llvmTrue = new LLVMBool(1);
        readonly LLVMBool llvmFalse = new LLVMBool(0);

        Stack<LLVMValueRef> valueStack = new Stack<LLVMValueRef>();
        Dictionary<string, LLVMValueRef> variables = new Dictionary<string, LLVMValueRef>();

        LLVMBuilderRef builder;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int Add(int a, int b);
        public static void Test()
        {

            LLVMBool False = new LLVMBool(0);
            LLVMModuleRef mod = LLVM.ModuleCreateWithName("LLVMSharpIntro");

            LLVMTypeRef[] param_types = { LLVM.Int32Type(), LLVM.Int32Type() };
            LLVMTypeRef ret_type = LLVM.FunctionType(LLVM.Int32Type(), out param_types[0], 2, False);
            LLVMValueRef sum = LLVM.AddFunction(mod, "sum", ret_type);

            LLVMBasicBlockRef entry = LLVM.AppendBasicBlock(sum, "entry");

            LLVMBuilderRef builder = LLVM.CreateBuilder();
            LLVM.PositionBuilderAtEnd(builder, entry);

            LLVM.BuildAlloca(builder, LLVM.Int32Type(), "x");
            
            LLVMValueRef tmp = LLVM.BuildAdd(builder, LLVM.GetParam(sum, 0), LLVM.GetParam(sum, 1), "tmp");
            LLVM.BuildRet(builder, tmp);

            IntPtr error;
            LLVM.VerifyModule(mod, LLVMVerifierFailureAction.LLVMAbortProcessAction, out error);
            LLVM.DisposeMessage(error);

            LLVMExecutionEngineRef engine;

            LLVM.LinkInMCJIT();
            LLVM.InitializeX86Target();
            LLVM.InitializeX86TargetInfo();
            LLVM.InitializeX86TargetMC();
            LLVM.InitializeX86AsmPrinter();

            var platform = Environment.OSVersion.Platform;
            if (platform == PlatformID.Win32NT) // On Windows, LLVM currently (3.6) does not support PE/COFF
            {
                LLVM.SetTarget(mod, Marshal.PtrToStringAnsi(LLVM.GetDefaultTargetTriple()) + "-elf");
            }

            var options = new LLVMMCJITCompilerOptions();
            var optionsSize = (4 * sizeof(int)) + IntPtr.Size; // LLVMMCJITCompilerOptions has 4 ints and a pointer

            LLVM.InitializeMCJITCompilerOptions(out options, optionsSize);
            LLVM.CreateMCJITCompilerForModule(out engine, mod, out options, optionsSize, out error);

            var addMethod = (Add)Marshal.GetDelegateForFunctionPointer(LLVM.GetPointerToGlobal(engine, sum), typeof(Add));
            int result = addMethod(10, 15);

            Console.WriteLine("Result of sum is: " + result);

            if (LLVM.WriteBitcodeToFile(mod, "sum.bc") != 0)
            {
                Console.WriteLine("error writing bitcode to file, skipping");
            }

            LLVM.DumpModule(mod);

            LLVM.DisposeBuilder(builder);
            LLVM.DisposeExecutionEngine(engine);
            Console.ReadKey();
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int llvm_main();
        public void EmitAndRun(AST.Node root)
        {
            LLVMBool False = new LLVMBool(0);
            LLVMModuleRef mod = LLVM.ModuleCreateWithName("WhatIsThisIDontEven");

            var passManager = LLVM.CreatePassManager();

            LLVMTypeRef[] param_types = { LLVM.Int32Type(), LLVM.Int32Type() };
            LLVMTypeRef ret_type = LLVM.FunctionType(LLVM.Int32Type(), out param_types[0], 0, False);
            LLVMValueRef main = LLVM.AddFunction(mod, "main", ret_type);

            LLVMBasicBlockRef entry = LLVM.AppendBasicBlock(main, "entry");
            
            builder = LLVM.CreateBuilder();
            LLVM.PositionBuilderAtEnd(builder, entry);
            
            Visit(root);

            LLVMValueRef tmp = LLVM.BuildLoad(builder, variables["x"], "result");
            LLVM.BuildRet(builder, tmp);

            IntPtr error;
            LLVM.VerifyModule(mod, LLVMVerifierFailureAction.LLVMAbortProcessAction, out error);
            LLVM.DisposeMessage(error);
            
            LLVMExecutionEngineRef engine;

            LLVM.LinkInMCJIT();
            LLVM.InitializeX86Target();
            LLVM.InitializeX86TargetInfo();
            LLVM.InitializeX86TargetMC();
            LLVM.InitializeX86AsmPrinter();

            var platform = Environment.OSVersion.Platform;
            if (platform == PlatformID.Win32NT) // On Windows, LLVM currently (3.6) does not support PE/COFF
            {
                LLVM.SetTarget(mod, Marshal.PtrToStringAnsi(LLVM.GetDefaultTargetTriple()) + "-elf");
            }

            var options = new LLVMMCJITCompilerOptions();
            var optionsSize = (4 * sizeof(int)) + IntPtr.Size; // LLVMMCJITCompilerOptions has 4 ints and a pointer

            options.OptLevel = 3;
            LLVM.InitializeMCJITCompilerOptions(out options, optionsSize);
            LLVM.CreateMCJITCompilerForModule(out engine, mod, out options, optionsSize, out error);
            

            var mainMethod = (llvm_main)Marshal.GetDelegateForFunctionPointer(LLVM.GetPointerToGlobal(engine, main), typeof(llvm_main));

            var answer = mainMethod(); 

            Console.WriteLine();
            Console.WriteLine("THE ANSWER IS: " + answer);
            Console.WriteLine();

            //int result = addMethod(10, 15);

            //Console.WriteLine("Result of sum is: " + result);

            //if (LLVM.WriteBitcodeToFile(mod, "sum.bc") != 0)
            //{
            //    Console.WriteLine("error writing bitcode to file, skipping");
            //}

            LLVM.DumpModule(mod);

            LLVM.DisposeBuilder(builder);
            LLVM.DisposeExecutionEngine(engine);
            Console.ReadKey();
        }

        public void Visit(AST.ConstInt node)
        {
            valueStack.Push(LLVM.ConstInt(LLVM.Int32Type(), (ulong)node.number, llvmTrue));
        }

        public void Visit(AST.BinOp node)
        {
            Visit(node.left);
            Visit(node.right);
            
            var right = valueStack.Pop();
            var left = valueStack.Pop();

            LLVMValueRef result;
            switch (node.type)
            {
                case AST.BinOp.BinOpType.Add:
                    result = LLVM.BuildAdd(builder, left, right, "add_tmp");
                    break;
                case AST.BinOp.BinOpType.Subract:
                    result = LLVM.BuildSub(builder, left, right, "sub_tmp");
                    break;
                case AST.BinOp.BinOpType.Multiply:
                    result = LLVM.BuildMul(builder, left, right, "mul_tmp");
                    break;
                case AST.BinOp.BinOpType.Divide:
                    result = LLVM.BuildSDiv(builder, left, right, "div_tmp");
                    break;
                default:
                    throw new InvalidCodePath();
            }

            valueStack.Push(result);
        }

        public void Visit(AST.VariableDeclaration node)
        {
            var v = LLVM.BuildAlloca(builder, LLVM.Int32Type(), node.variableName);
            variables[node.variableName] = v;

            Visit(node.expression);

            var result = valueStack.Pop();
            LLVM.BuildStore(builder, result, v);
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
            var l = LLVM.BuildLoad(builder, variables[node.variableName], node.variableName);
            valueStack.Push(l);
        }

        public void Visit(AST.Node node)
        {
            if (node is AST.ConstInt)
            {
                Visit(node as AST.ConstInt);
            }
            else if (node is AST.BinOp)
            {
                Visit(node as AST.BinOp);
            }
            else if (node is AST.VariableDeclaration)
            {
                Visit(node as AST.VariableDeclaration);
            }
            else if (node is AST.Block)
            {
                Visit(node as AST.Block);
            }
            else if (node is AST.VariableLookup)
            {
                Visit(node as AST.VariableLookup);
            }
        }



    }

}
