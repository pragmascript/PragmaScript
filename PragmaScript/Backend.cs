using LLVMSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PragmaScript
{


    partial class Backend
    {

        public class ExecutionContext
        {
            public LLVMValueRef function;
            public LLVMBasicBlockRef entry;
            public LLVMBasicBlockRef vars;

            public bool loop;
            public LLVMBasicBlockRef loopNext;
            public LLVMBasicBlockRef loopEnd;

            public ExecutionContext(LLVMValueRef function, LLVMBasicBlockRef entry, LLVMBasicBlockRef vars)
            {
                this.function = function;
                this.entry = entry;
                this.vars = vars;
                loop = false;
                loopNext = default(LLVMBasicBlockRef);
                loopEnd = default(LLVMBasicBlockRef);
            }

            public ExecutionContext(ExecutionContext other)
            {
                function = other.function;
                entry = other.entry;
                vars = other.vars;
                loop = other.loop;
                loopNext = other.loopNext;
                loopEnd = other.loopEnd;
            }

        }


        public static class Const
        {
            public static readonly LLVMBool TrueBool = new LLVMBool(1);
            public static readonly LLVMBool FalseBool = new LLVMBool(0);
            public static readonly LLVMValueRef NegativeOneInt32 = LLVM.ConstInt(LLVM.Int32Type(), unchecked((ulong)-1), new LLVMBool(1));
            public static readonly LLVMValueRef ZeroInt32 = LLVM.ConstInt(LLVM.Int32Type(), 0, new LLVMBool(1));
            public static readonly LLVMValueRef OneInt32 = LLVM.ConstInt(LLVM.Int32Type(), 1, new LLVMBool(1));
            public static readonly LLVMValueRef OneFloat32 = LLVM.ConstReal(LLVM.FloatType(), 1.0);
            public static readonly LLVMValueRef True = LLVM.ConstInt(LLVM.Int1Type(), (ulong)1, new LLVMBool(0));
            public static readonly LLVMValueRef False = LLVM.ConstInt(LLVM.Int1Type(), (ulong)0, new LLVMBool(0));

            public static readonly LLVMTypeRef Float32Type = LLVM.FloatType();
            public static readonly LLVMTypeRef Int32Type = LLVM.Int32Type();
            public static readonly LLVMTypeRef Int32ArrayType = LLVM.ArrayType(LLVM.Int32Type(), 0);
            public static readonly LLVMTypeRef Int8ArrayType = LLVM.ArrayType(LLVM.Int8Type(), 0);
            public static readonly LLVMTypeRef Int8PointerType = LLVM.PointerType(LLVM.Int8Type(), 0);
            public static readonly LLVMTypeRef BoolType = LLVM.Int1Type();
            public static readonly LLVMTypeRef VoidType = LLVM.VoidType();
            public static readonly LLVMTypeRef VoidStarType = LLVM.PointerType(VoidType, 0);
        }



        Stack<LLVMValueRef> valueStack = new Stack<LLVMValueRef>();
        Dictionary<string, LLVMValueRef> variables = new Dictionary<string, LLVMValueRef>();
        Dictionary<string, LLVMValueRef> functions = new Dictionary<string, LLVMValueRef>();

        public Stack<ExecutionContext> ctx = new Stack<ExecutionContext>();

        LLVMModuleRef mod;
        LLVMBuilderRef builder;
        LLVMValueRef mainFunction;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int llvm_main();

        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //public delegate void void_del();
        //public static void_del print;

        List<Delegate> functionDelegates = new List<Delegate>();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void void_int_del(int x);
        public static void_int_del print_i32;

        // http://stackoverflow.com/questions/14106619/passing-delegate-to-a-unmanaged-method-which-expects-a-delegate-method-with-an-i
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void void_float_del(float x);
        public static void_float_del print_f32;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void void_string_del(IntPtr ptr);
        public static void_string_del print_string;

        public Backend()
        {
            print_i32 += (x) =>
            {
                Console.Write(x);
            };

            print_f32 += (x) =>
            {
                Console.Write(x);
            };

            print_string += (x) =>
            {
                Console.Write(Marshal.PtrToStringAnsi(x));
            };


            addDelegate(print_i32, "print_i32");
            addDelegate(print_f32, "print_f32");
            addDelegate(print_string, "print");
        }

        static LLVMTypeRef getTypeRef(PragmaScript.AST.FrontendType t)
        {
            if (t == PragmaScript.AST.FrontendType.int32)
            {
                return Const.Int32Type;
            }
            if (t == PragmaScript.AST.FrontendType.float32)
            {
                return Const.Float32Type;
            }
            if (t == PragmaScript.AST.FrontendType.bool_)
            {
                return Const.BoolType;
            }
            if (t == PragmaScript.AST.FrontendType.void_)
            {
                return Const.VoidType;
            }
            if (t == PragmaScript.AST.FrontendType.string_)
            {
                return Const.Int8PointerType;
            }
            else
            {
                throw new InvalidCodePath();
            }
        }

        static LLVMTypeRef getTypeRef(Type t)
        {
            if (t == typeof(Int32))
            {
                return Const.Int32Type;
            }
            else if (t == typeof(float))
            {
                return Const.Float32Type;
            }
            else if (t == typeof(void))
            {
                return Const.VoidType;
            }
            else if (t == typeof(IntPtr))
            {
                return Const.Int8PointerType;
            }
            else if (t == typeof(int[]))
            {
                return Const.Int32ArrayType;
            }
            // TODO how to handle string types properly
            else if (t == typeof(byte[]))
            {
                return Const.Int8PointerType;
            }

            else
            {
                throw new BackendException("No LLVM type for " + t.Name);
            }
        }

        public static bool isEqualType(LLVMTypeRef a, LLVMTypeRef b)
        {
            return a.Pointer == b.Pointer;
        }

        public static string typeToString(LLVMTypeRef t)
        {
            return Marshal.PtrToStringAnsi(LLVM.PrintTypeToString(t));
        }

        void addDelegate<T>(T del, string name) where T : class
        {
            if (!typeof(T).IsSubclassOf(typeof(Delegate)))
            {
                throw new InvalidOperationException(typeof(T).Name + " is not a delegate type");
            }
            var info = typeof(T).GetMethod("Invoke");
            var parameters = info.GetParameters();


            LLVMTypeRef[] param_types = new LLVMTypeRef[Math.Max(parameters.Length, 1)];

            for (int i = 0; i < parameters.Length; ++i)
            {
                var p = parameters[i];
                var pt = p.ParameterType;
                param_types[i] = getTypeRef(pt);
            }

            var returnTypeRef = getTypeRef(info.ReturnType);

            var fun_type = LLVM.FunctionType(Const.VoidType, out param_types[0], (uint)parameters.Length, Const.FalseBool);
            IntPtr functionPtr = Marshal.GetFunctionPointerForDelegate(del as Delegate);
            var llvmFuncPtr = LLVM.ConstIntToPtr(LLVM.ConstInt(LLVM.Int64Type(), (ulong)functionPtr, Const.FalseBool), LLVM.PointerType(fun_type, 0));
            functions.Add(name, llvmFuncPtr);
        }



        void prepareModule()
        {
            mod = LLVM.ModuleCreateWithName("WhatIsThisIDontEven");

            LLVMTypeRef[] main_param_types = { LLVM.Int32Type(), LLVM.Int32Type() };
            LLVMTypeRef main_fun_type = LLVM.FunctionType(LLVM.Int32Type(), out main_param_types[0], 0, Const.FalseBool);
            mainFunction = LLVM.AddFunction(mod, "main", main_fun_type);
            LLVMBasicBlockRef vars = LLVM.AppendBasicBlock(mainFunction, "vars");
            LLVMBasicBlockRef entry = LLVM.AppendBasicBlock(mainFunction, "entry");

            var c = new ExecutionContext(mainFunction, entry, vars);
            ctx.Push(c);
            


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
                Console.WriteLine();
                LLVM.DumpModule(mod);
                return;
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
                LLVM.DumpModule(mod);
                return;
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
                LLVM.AddFunctionInliningPass(pass);
                LLVM.AddBBVectorizePass(pass);
                LLVM.AddLoopVectorizePass(pass);
                LLVM.AddSLPVectorizePass(pass);
                LLVM.RunPassManager(pass, mod);
            }
            else
            {
                LLVM.AddVerifierPass(pass);
                LLVM.RunPassManager(pass, mod);
            }

            var mainFunctionDelegate = (llvm_main)Marshal.GetDelegateForFunctionPointer(LLVM.GetPointerToGlobal(engine, mainFunction), typeof(llvm_main));

            // **************************** RUN THE THING **************************** 
            if (CompilerOptions.debug)
            {
                Console.WriteLine();
                Console.WriteLine("PROGRAM OUTPUT: ");
                Console.WriteLine("****************************");
                Console.WriteLine();
            }

            var answer = mainFunctionDelegate();
            
            if (CompilerOptions.debug)
            {
                Console.WriteLine();
                Console.WriteLine("****************************");
                Console.WriteLine();
            }

            // *********************************************************************** 


            //if (LLVM.WriteBitcodeToFile(mod, "main.bc") != 0)
            //{
            //    Console.WriteLine("error writing bitcode to file, skipping");
            //}
            if (CompilerOptions.debug)
            {
                LLVM.DumpModule(mod);
            }
            LLVM.DisposeBuilder(builder);
            LLVM.DisposeExecutionEngine(engine);

            if (CompilerOptions.debug)
            {
                Console.WriteLine();
                Console.WriteLine("THE ANSWER IS: " + answer);
                Console.WriteLine();
            }
        }
        public void EmitAndRun(AST.Node root, bool useOptimizations)
        {
            prepareModule();
            Visit(root);
            InsertMissingReturn(Const.Int32Type);
            LLVM.PositionBuilderAtEnd(builder, ctx.Peek().vars);
            LLVM.BuildBr(builder, ctx.Peek().entry);
            executeModule(useOptimizations);
        }

        public void InsertMissingReturn(LLVMTypeRef returnType)
        {
            var term = LLVM.GetBasicBlockTerminator(LLVM.GetInsertBlock(builder));
            if (term.Pointer == IntPtr.Zero)
            {
                if (isEqualType(returnType, Const.VoidType))
                {
                    LLVM.BuildRetVoid(builder);
                }
                else
                {
                    var dummy = LLVM.BuildBitCast(builder, Const.ZeroInt32, returnType, "dummy");
                    LLVM.BuildRet(builder, dummy);
                }
            }
        }
    }

}
