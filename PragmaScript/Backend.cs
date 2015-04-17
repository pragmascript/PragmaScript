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

        public class ExecutionContext
        {
            public LLVMValueRef function;
        }

        //struct TypedValue
        //{
        //    public static LLVMTypeRef MapType(PragmaScript.AST.FrontendType ftype)
        //    {
        //        LLVMTypeRef type;
        //        if (ftype == PragmaScript.AST.FrontendType.bool_)
        //        {
        //            type = Const.BoolType;
        //        }
        //        else if (ftype == PragmaScript.AST.FrontendType.int32)
        //        {
        //            type = Const.Int32Type;
        //        }
        //        else if (ftype == PragmaScript.AST.FrontendType.float32)
        //        {
        //            type = Const.Float32Type;
        //        }
        //        else if (ftype == PragmaScript.AST.FrontendType.void_)
        //        {
        //            type = Const.VoidType;
        //        }
        //        else if (ftype == PragmaScript.AST.FrontendType.string_)
        //        {
        //            type = Const.Int8PointerType;
        //        }
        //        else
        //            throw new InvalidCodePath();

        //        return type;
        //    }

        //    public LLVMTypeRef type;
        //    public LLVMValueRef value;
        //    public TypedValue(LLVMValueRef value, LLVMTypeRef type)
        //    {
        //        this.value = value;
        //        this.type = type;
        //    }
        //}

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

        public ExecutionContext ctx = new ExecutionContext();

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
            executeModule(useOptimizations);
        }

        public void Visit(AST.ConstInt32 node)
        {
            var result = LLVM.ConstInt(Const.Int32Type, (ulong)node.number, Const.TrueBool);
            valueStack.Push(result);
        }

        public void Visit(AST.ConstFloat32 node)
        {
            var result = LLVM.ConstReal(Const.Float32Type, node.number);
            valueStack.Push(result);
        }

        public void Visit(AST.ConstBool node)
        {
            var result = node.value ? Const.True : Const.False;
            valueStack.Push(result);
        }

        public static string ParseString(string txt)
        {
            var provider = new Microsoft.CSharp.CSharpCodeProvider();
            var prms = new System.CodeDom.Compiler.CompilerParameters();
            prms.GenerateExecutable = false;
            prms.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(prms, @"
namespace tmp
{
    public class tmpClass
    {
        public static string GetValue()
        {
             return " + "\"" + txt + "\"" + @";
        }
    }
}");
            System.Reflection.Assembly ass = results.CompiledAssembly;
            var method = ass.GetType("tmp.tmpClass").GetMethod("GetValue");
            return method.Invoke(null, null) as string;
        }

        string convertString(string s)
        {
            var tmp = s.Substring(1, s.Length - 2);
            return ParseString(tmp);
        }
        public void Visit(AST.ConstString node)
        {
            var str = convertString(node.s);
            var constString = LLVM.BuildGlobalStringPtr(builder, str, "str");
            var result = LLVM.BuildBitCast(builder, constString, Const.Int8PointerType, "str_ptr");
            valueStack.Push(result);
        }

        public void Visit(AST.BinOp node)
        {
            if (node.type == AST.BinOp.BinOpType.ConditionalOR)
            {
                visitConditionalOR(node);
                return;
            }
            if (node.type == AST.BinOp.BinOpType.ConditionaAND)
            {
                visitConditionalAND(node);
                return;
            }

            Visit(node.left);
            Visit(node.right);

            var right = valueStack.Pop();
            var left = valueStack.Pop();

            var leftType = LLVM.TypeOf(left);
            var rightType = LLVM.TypeOf(right);


            if (!isEqualType(leftType, rightType))
            {
                throw new BackendTypeMismatchException(leftType, rightType);
            }

            LLVMValueRef result;
            if (isEqualType(leftType, Const.BoolType))
            {
                switch (node.type)
                {
                    case AST.BinOp.BinOpType.LogicalAND:
                        result = LLVM.BuildAnd(builder, left, right, "and_tmp");
                        break;
                    case AST.BinOp.BinOpType.LogicalOR:
                        result = LLVM.BuildOr(builder, left, right, "or_tmp");
                        break;
                    case AST.BinOp.BinOpType.LogicalXOR:
                        result = LLVM.BuildXor(builder, left, right, "or_tmp");
                        break;

                    default:
                        throw new InvalidCodePath();
                }
            }
            else if (isEqualType(leftType, Const.Int32Type))
            {
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
                    case AST.BinOp.BinOpType.LeftShift:
                        result = LLVM.BuildShl(builder, left, right, "shl_tmp");
                        break;
                    case AST.BinOp.BinOpType.RightShift:
                        result = LLVM.BuildAShr(builder, left, right, "shr_tmp");
                        break;
                    case AST.BinOp.BinOpType.Remainder:
                        result = LLVM.BuildSRem(builder, left, right, "srem_tmp");
                        break;
                    case AST.BinOp.BinOpType.Equal:
                        result = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntEQ, left, right, "icmp_tmp");
                        break;
                    case AST.BinOp.BinOpType.NotEqual:
                        result = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntNE, left, right, "icmp_tmp");
                        break;
                    case AST.BinOp.BinOpType.Greater:
                        result = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntSGT, left, right, "icmp_tmp");
                        break;
                    case AST.BinOp.BinOpType.GreaterEqual:
                        result = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntSGE, left, right, "icmp_tmp");
                        break;
                    case AST.BinOp.BinOpType.Less:
                        result = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntSLT, left, right, "icmp_tmp");
                        break;
                    case AST.BinOp.BinOpType.LessEqual:
                        result = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntSLE, left, right, "icmp_tmp");
                        break;
                    default:
                        throw new InvalidCodePath();
                }
            }
            else if (isEqualType(leftType, Const.Float32Type))
            {
                switch (node.type)
                {
                    case AST.BinOp.BinOpType.Add:
                        result = LLVM.BuildFAdd(builder, left, right, "fadd_tmp");
                        break;
                    case AST.BinOp.BinOpType.Subract:
                        result = LLVM.BuildFSub(builder, left, right, "fsub_tmp");
                        break;
                    case AST.BinOp.BinOpType.Multiply:
                        result = LLVM.BuildFMul(builder, left, right, "fmul_tmp");
                        break;
                    case AST.BinOp.BinOpType.Divide:
                        result = LLVM.BuildFDiv(builder, left, right, "fdiv_tmp");
                        break;
                    case AST.BinOp.BinOpType.Remainder:
                        result = LLVM.BuildFRem(builder, left, right, "frem_tmp");
                        break;
                    case AST.BinOp.BinOpType.Equal:
                        result = LLVM.BuildFCmp(builder, LLVMRealPredicate.LLVMRealOEQ, left, right, "fcmp_tmp");
                        break;
                    case AST.BinOp.BinOpType.NotEqual:
                        result = LLVM.BuildFCmp(builder, LLVMRealPredicate.LLVMRealONE, left, right, "fcmp_tmp");
                        break;
                    case AST.BinOp.BinOpType.Greater:
                        result = LLVM.BuildFCmp(builder, LLVMRealPredicate.LLVMRealOGT, left, right, "fcmp_tmp");
                        break;
                    case AST.BinOp.BinOpType.GreaterEqual:
                        result = LLVM.BuildFCmp(builder, LLVMRealPredicate.LLVMRealOGE, left, right, "fcmp_tmp");
                        break;
                    case AST.BinOp.BinOpType.Less:
                        result = LLVM.BuildFCmp(builder, LLVMRealPredicate.LLVMRealOLT, left, right, "fcmp_tmp");
                        break;
                    case AST.BinOp.BinOpType.LessEqual:
                        result = LLVM.BuildFCmp(builder, LLVMRealPredicate.LLVMRealOLE, left, right, "fcmp_tmp");
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

        void visitConditionalOR(AST.BinOp op)
        {
            //List<AST.BinOp> ors = new List<AST.BinOp>();
            //ors.Add(op);
            //// capture all consecutive ors
            //while (op.left is AST.BinOp && (op.left as AST.BinOp).type == AST.BinOp.BinOpType.ConditionalOR)
            //{
            //    op = op.left as AST.BinOp;
            //    ors.Add(op);
            //}

            Visit(op.left);
            var cmp = valueStack.Pop();
            if (!isEqualType(LLVM.TypeOf(cmp), Const.BoolType))
                throw new BackendTypeMismatchException(Const.BoolType, LLVM.TypeOf(cmp));

            var cor_rhs = LLVM.AppendBasicBlock(ctx.function, "cor.rhs");
            var cor_end = LLVM.AppendBasicBlock(ctx.function, "cor.end");
            var block = LLVM.GetInsertBlock(builder);
            LLVM.BuildCondBr(builder, cmp, cor_end, cor_rhs);

            // cor.rhs: 
            LLVM.PositionBuilderAtEnd(builder, cor_rhs);
            Visit(op.right);
            var cor_rhs_tv = valueStack.Pop();
            if (!isEqualType(LLVM.TypeOf(cor_rhs_tv), Const.BoolType))
                throw new BackendTypeMismatchException(Const.BoolType, LLVM.TypeOf(cor_rhs_tv));

            LLVM.BuildBr(builder, cor_end);

            // cor.end:
            LLVM.PositionBuilderAtEnd(builder, cor_end);
            var phi = LLVM.BuildPhi(builder, Const.BoolType, "corphi");

            LLVMBasicBlockRef[] incomingBlocks = new LLVMBasicBlockRef[2] { block, cor_rhs };
            LLVMValueRef[] incomingValues = new LLVMValueRef[2] { Const.True, cor_rhs_tv };

            LLVM.AddIncoming(phi, out incomingValues[0], out incomingBlocks[0], 2);

            valueStack.Push(phi);
        }

        void visitConditionalAND(AST.BinOp op)
        {
            Visit(op.left);
            var cmp = valueStack.Pop();
            if (!isEqualType(LLVM.TypeOf(cmp), Const.BoolType))
                throw new BackendTypeMismatchException(Const.BoolType, LLVM.TypeOf(cmp));

            var cand_rhs = LLVM.AppendBasicBlock(ctx.function, "cand.rhs");
            var cand_end = LLVM.AppendBasicBlock(ctx.function, "cand.end");
            var block = LLVM.GetInsertBlock(builder);

            LLVM.BuildCondBr(builder, cmp, cand_rhs, cand_end);

            // cor.rhs: 
            LLVM.PositionBuilderAtEnd(builder, cand_rhs);
            Visit(op.right);
            var cand_rhs_tv = valueStack.Pop();
            if (!isEqualType(LLVM.TypeOf(cand_rhs_tv), Const.BoolType))
                throw new BackendTypeMismatchException(Const.BoolType, LLVM.TypeOf(cand_rhs_tv));
            LLVM.BuildBr(builder, cand_end);

            // cor.end:
            LLVM.PositionBuilderAtEnd(builder, cand_end);
            var phi = LLVM.BuildPhi(builder, Const.BoolType, "candphi");

            LLVMBasicBlockRef[] incomingBlocks = new LLVMBasicBlockRef[2] { block, cand_rhs };
            LLVMValueRef[] incomingValues = new LLVMValueRef[2] { Const.False, cand_rhs_tv };

            LLVM.AddIncoming(phi, out incomingValues[0], out incomingBlocks[0], 2);

            valueStack.Push(phi);
        }

        public void Visit(AST.UnaryOp node)
        {
            Visit(node.expression);

            var v = valueStack.Pop();
            var vtype = LLVM.TypeOf(v);
            var result = default(LLVMValueRef);
            switch (node.type)
            {
                case AST.UnaryOp.UnaryOpType.Add:
                    result = v;
                    break;
                case AST.UnaryOp.UnaryOpType.Subract:
                    if (isEqualType(vtype, Const.Float32Type))
                    {
                        result = LLVM.BuildFNeg(builder, v, "fneg_tmp");
                    }
                    else if (isEqualType(vtype, Const.Int32Type))
                    {
                        result = LLVM.BuildNeg(builder, v, "neg_tmp");
                    }
                    else
                    {
                        throw new BackendException("unary subtract is not defined on type " + typeToString(vtype));
                    }
                    break;
                case AST.UnaryOp.UnaryOpType.LogicalNOT:
                    if (!isEqualType(vtype, Const.BoolType))
                    {
                        throw new BackendTypeMismatchException(vtype, Const.BoolType);
                    }
                    result = LLVM.BuildNot(builder, v, "not_tmp");
                    break;
                case AST.UnaryOp.UnaryOpType.Complement:
                    result = LLVM.BuildXor(builder, v, Const.NegativeOneInt32, "complement_tmp");
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
            var vtype = LLVM.TypeOf(v);

            var typeName = node.type.name;

            var result = default(LLVMValueRef);
            var resultType = getTypeRef(node.type);

            // TODO: check if integral type
            // TODO: handle non integral types


            if (isEqualType(resultType, Const.Float32Type))
            {
                result = LLVM.BuildSIToFP(builder, v, Const.Float32Type, "float32_cast");
            }
            else if (isEqualType(resultType, Const.Int32Type))
            {
                if (isEqualType(vtype, Const.Float32Type))
                {
                    result = LLVM.BuildFPToSI(builder, v, Const.Int32Type, "int32_cast");
                }
                else if (isEqualType(vtype, Const.Int32Type))
                {
                    result = v;
                }
                else if (isEqualType(vtype, Const.BoolType))
                {
                    result = LLVM.BuildZExt(builder, v, Const.Int32Type, "int32_cast");
                }
                else
                {
                    throw new InvalidCodePath();
                }
            }
            else
            {
                throw new NotImplementedException();
            }
            valueStack.Push(result);
        }

        public void Visit(AST.VariableDeclaration node)
        {
            Visit(node.expression);

            var result = valueStack.Pop();
            var resultType = LLVM.TypeOf(result);

            LLVMValueRef v;
            v = LLVM.BuildAlloca(builder, resultType, node.variable.name);
            variables[node.variable.name] = v;
            LLVM.BuildStore(builder, result, v);
        }

        public void Visit(AST.Assignment node)
        {
            Visit(node.expression);

            var result = valueStack.Pop();
            var resultType = LLVM.TypeOf(result);
            var v = variables[node.variable.name];
            var vtype = LLVM.TypeOf(v);

            if (!isEqualType(vtype, resultType))
            {
                throw new BackendTypeMismatchException(resultType, vtype);
            }

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
            if (node.varDefinition.isFunctionParameter)
            {
                var pr = LLVM.GetParam(ctx.function, (uint)node.varDefinition.parameterIdx);
                valueStack.Push(pr);
                return;
            }

            var v = variables[node.variableName];
            
            var l = LLVM.BuildLoad(builder, v, node.variableName);
            var ltype = LLVM.TypeOf(l);
            var result = l;
            switch (node.inc)
            {
                case AST.VariableLookup.Incrementor.None:
                    break;

                case AST.VariableLookup.Incrementor.preIncrement:
                    if (isEqualType(ltype, Const.Int32Type))
                    {
                        result = LLVM.BuildAdd(builder, l, Const.OneInt32, "preinc");
                    }
                    else if (isEqualType(ltype, Const.Float32Type))
                    {
                        result = LLVM.BuildFAdd(builder, l, Const.OneFloat32, "preinc");
                    }
                    LLVM.BuildStore(builder, result, v);
                    break;
                case AST.VariableLookup.Incrementor.preDecrement:
                    if (isEqualType(ltype, Const.Int32Type))
                    {
                        result = LLVM.BuildSub(builder, l, Const.OneInt32, "predec");
                    }
                    else if (isEqualType(ltype, Const.Float32Type))
                    {
                        result = LLVM.BuildFSub(builder, l, Const.OneFloat32, "predec");
                    }
                    LLVM.BuildStore(builder, result, v);
                    break;
                case AST.VariableLookup.Incrementor.postIncrement:
                    var postinc = default(LLVMValueRef);
                    if (isEqualType(ltype, Const.Int32Type))
                    {
                        postinc = LLVM.BuildAdd(builder, l, Const.OneInt32, "postinc");
                    }
                    else if (isEqualType(ltype, Const.Float32Type))
                    {
                        postinc = LLVM.BuildFAdd(builder, l, Const.OneFloat32, "postinc");
                    }
                    LLVM.BuildStore(builder, postinc, v);
                    break;
                case AST.VariableLookup.Incrementor.postDecrement:
                    var postdec = default(LLVMValueRef);
                    if (isEqualType(ltype, Const.Int32Type))
                    {
                        postdec = LLVM.BuildSub(builder, l, Const.OneInt32, "postdec");
                    }
                    else if (isEqualType(ltype, Const.Float32Type))
                    {
                        postdec = LLVM.BuildFSub(builder, l, Const.OneFloat32, "postdec");
                    }
                    LLVM.BuildStore(builder, postdec, v);
                    break;
                default:
                    break;
            }
            valueStack.Push(result);

        }

        public void Visit(AST.FunctionCall node)
        {
            var f = functions[node.functionName];
            var cnt = node.argumentList.Count;
            LLVMValueRef[] parameters = new LLVMValueRef[Math.Max(1, cnt)];

            for (int i = 0; i < node.argumentList.Count; ++i)
            {
                Visit(node.argumentList[i]);
                parameters[i] = valueStack.Pop();
            }

            // TODO: use proper function declarations
            var ft = LLVM.TypeOf(f);
            // http://lists.cs.uiuc.edu/pipermail/llvmdev/2008-May/014844.html
            var rt = LLVM.GetReturnType(LLVM.GetElementType(ft));
            var rt_string = typeToString(rt);
            if (isEqualType(rt, Const.VoidType))
            {
                LLVM.BuildCall(builder, f, out parameters[0], (uint)cnt, "");
            }
            else
            {
                var v = LLVM.BuildCall(builder, f, out parameters[0], (uint)cnt, node.functionName);
                valueStack.Push(v);
            }
        }

        public void Visit(AST.Return node)
        {
            if (node.expression != null)
            {
                Visit(node.expression);
                var v = valueStack.Pop();
                LLVM.BuildRet(builder, v);
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
            if (!isEqualType(LLVM.TypeOf(condition), Const.BoolType))
            {
                throw new BackendTypeMismatchException(LLVM.TypeOf(condition), Const.BoolType);
            }

            var thenBlock = LLVM.AppendBasicBlock(ctx.function, "then");

            var elseBlock = default(LLVMBasicBlockRef);
            if (node.elseBlock != null)
            {
                elseBlock = LLVM.AppendBasicBlock(ctx.function, "else");
            }
            var endIfBlock = LLVM.AppendBasicBlock(ctx.function, "endif");

            if (node.elseBlock != null)
            {
                LLVM.BuildCondBr(builder, condition, thenBlock, elseBlock);
            }
            else
            {
                LLVM.BuildCondBr(builder, condition, thenBlock, endIfBlock);
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
            if (!isEqualType(LLVM.TypeOf(condition), Const.BoolType))
            {
                throw new BackendTypeMismatchException(LLVM.TypeOf(condition), Const.BoolType);
            }
            LLVM.BuildCondBr(builder, condition, loopBody, endFor);

            LLVM.PositionBuilderAtEnd(builder, loopBody);
            Visit(node.loopBody);
            Visit(node.iterator);

            LLVM.BuildBr(builder, loopPre);

            LLVM.PositionBuilderAtEnd(builder, endFor);
        }

        public void Visit(AST.FunctionDeclaration node)
        {
            var fun = node.fun;

            if (functions.ContainsKey(fun.name))
                throw new Exception("function redefinition");

            var cnt = Math.Max(1, fun.parameters.Count);
            var par = new LLVMTypeRef[cnt];

            for (int i = 0; i < fun.parameters.Count; ++i)
            {
                par[i] = getTypeRef(fun.parameters[i].type);
            }

            var returnType = getTypeRef((fun.returnType));

            var funType = LLVM.FunctionType(returnType, out par[0], (uint)fun.parameters.Count, Const.FalseBool);
            var function = LLVM.AddFunction(mod, fun.name, funType);

            // TODO: insert function type into dictionary as well?
            functions.Add(fun.name, function);

            for (int i = 0; i < fun.parameters.Count; ++i)
            {
                LLVMValueRef param = LLVM.GetParam(function, (uint)i);
                LLVM.SetValueName(param, fun.parameters[i].name);
                // variables.Add(fun.parameters[i].name, new TypedValue(param, TypedValue.MapType(fun.parameters[i].type)));
            }

            var entry = LLVM.AppendBasicBlock(function, "entry");

            var blockTemp = LLVM.GetInsertBlock(builder);

            LLVM.PositionBuilderAtEnd(builder, entry);
            var funTemp = ctx.function;
            ctx.function = function;
            Visit(node.body);
            ctx.function = funTemp;
            LLVM.PositionBuilderAtEnd(builder, blockTemp);

            LLVM.VerifyFunction(function, LLVMVerifierFailureAction.LLVMPrintMessageAction);
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
            else if (node is AST.ConstString)
            {
                Visit(node as AST.ConstString);
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
            else if (node is AST.FunctionDeclaration)
            {
                Visit(node as AST.FunctionDeclaration);
            }
        }
    }

}
