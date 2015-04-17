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
            Float32, Int32, Bool, Void
        }

        public class ExecutionContext
        {
            public LLVMValueRef function;
        }

        struct TypedValue
        {
            public static BackendType MapType(PragmaScript.AST.FrontendType ftype)
            {
                BackendType type;
                if (ftype == PragmaScript.AST.FrontendType.bool_)
                {
                    type = BackendType.Bool;
                }
                else if (ftype == PragmaScript.AST.FrontendType.int32)
                {
                    type = BackendType.Int32;
                }
                else if (ftype == PragmaScript.AST.FrontendType.float32)
                {
                    type = BackendType.Float32;
                }
                else if (ftype == PragmaScript.AST.FrontendType.void_)
                {
                    type = BackendType.Void;
                }
                else
                    throw new InvalidCodePath();

                return type;
            }
           
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
            public static readonly LLVMTypeRef VoidType = LLVM.VoidType();
            
            public static LLVMTypeRef GetTypeRef(BackendType type)
            {
                switch (type)
                {
                    case BackendType.Float32:
                        return Float32Type;
                    case BackendType.Int32:
                        return Int32Type;
                    case BackendType.Bool:
                        return BoolType;
                    case BackendType.Void:

                        break;
                    default:
                        throw new InvalidCodePath();
                }
                throw new InvalidCodePath();
            }
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

        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //public delegate void void_del();
        //public static void_del print;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void void_int_del(int x);
        public static void_int_del print_i32;
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void void_float_del(float x);
        public static void_float_del print_f32;

        public Backend()
        {
            int i = 0;
            //print += () =>
            //{
            //    Console.WriteLine("Hello world: " + i++);
            //};

            //foo += () =>
            //{
            //    Console.WriteLine("foo called!");
            //    return 3;
            //};

            print_i32 += (x) =>
            {
                Console.WriteLine(x);
            };

            print_f32 += (x) =>
            {
                Console.WriteLine(x);
            };

            //LLVMTypeRef[] print_int_param_types = { LLVM.Int32Type() };
            //var print_int_fun_type = LLVM.FunctionType(LLVM.VoidType(), out print_int_param_types[0], 0, Const.FalseBool);
            //IntPtr printPtr = Marshal.GetFunctionPointerForDelegate(print);
            //var printFuncConst = LLVM.ConstIntToPtr(LLVM.ConstInt(LLVM.Int64Type(), (ulong)printPtr, Const.FalseBool), LLVM.PointerType(print_int_fun_type, 0));
            //functions.Add("print", printFuncConst);

            //LLVMTypeRef[] foo_int_param_types = { LLVM.Int32Type() };
            //var foo_int_fun_type = LLVM.FunctionType(LLVM.Int32Type(), out print_int_param_types[0], 0, Const.FalseBool);
            //IntPtr fooPtr = Marshal.GetFunctionPointerForDelegate(foo);
            //var fooFuncConst = LLVM.ConstIntToPtr(LLVM.ConstInt(LLVM.Int64Type(), (ulong)fooPtr, Const.FalseBool), LLVM.PointerType(foo_int_fun_type, 0));
            //functions.Add("foo", fooFuncConst);

            LLVMTypeRef[] printi_param_types = { Const.Int32Type };
            var printi_fun_type = LLVM.FunctionType(Const.VoidType, out printi_param_types[0], 1, Const.FalseBool);
            IntPtr printiPtr = Marshal.GetFunctionPointerForDelegate(print_i32);
            var printiFuncConst = LLVM.ConstIntToPtr(LLVM.ConstInt(LLVM.Int64Type(), (ulong)printiPtr, Const.FalseBool), LLVM.PointerType(printi_fun_type, 0));
            functions.Add("print_i32", printiFuncConst);

            LLVMTypeRef[] printf_param_types = { Const.Float32Type };
            var printf_fun_type = LLVM.FunctionType(Const.VoidType, out printf_param_types[0], 1, Const.FalseBool);
            IntPtr printfPtr = Marshal.GetFunctionPointerForDelegate(print_f32);
            var printfFuncConst = LLVM.ConstIntToPtr(LLVM.ConstInt(LLVM.Int64Type(), (ulong)printfPtr, Const.FalseBool), LLVM.PointerType(printf_fun_type, 0));
            functions.Add("print_f32", printfFuncConst);
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

            var answer = mainFunctionDelegate();

            // *********************************************************************** 


            //if (LLVM.WriteBitcodeToFile(mod, "main.bc") != 0)
            //{
            //    Console.WriteLine("error writing bitcode to file, skipping");
            //}
#if DEBUG
            LLVM.DumpModule(mod);
#endif
            LLVM.DisposeBuilder(builder);
            LLVM.DisposeExecutionEngine(engine);

#if DEBUG
            Console.WriteLine();
            Console.WriteLine("THE ANSWER IS: " + answer);
            Console.WriteLine();
#endif
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
            if (cmp.type != BackendType.Bool)
                throw new BackendTypeMismatchException(BackendType.Bool, cmp.type);

            var cor_rhs = LLVM.AppendBasicBlock(ctx.function, "cor.rhs");
            var cor_end = LLVM.AppendBasicBlock(ctx.function, "cor.end");
            var block = LLVM.GetInsertBlock(builder);
            LLVM.BuildCondBr(builder, cmp.value, cor_end, cor_rhs);

            // cor.rhs: 
            LLVM.PositionBuilderAtEnd(builder, cor_rhs);
            Visit(op.right);
            var cor_rhs_tv = valueStack.Pop();
            if (cmp.type != BackendType.Bool)
                throw new BackendTypeMismatchException(BackendType.Bool, cor_rhs_tv.type);

            LLVM.BuildBr(builder, cor_end);

            // cor.end:
            LLVM.PositionBuilderAtEnd(builder, cor_end);
            var phi = LLVM.BuildPhi(builder, Const.BoolType, "corphi");

            LLVMBasicBlockRef[] incomingBlocks = new LLVMBasicBlockRef[2] { block, cor_rhs };
            LLVMValueRef[] incomingValues = new LLVMValueRef[2] { Const.True, cor_rhs_tv.value };

            LLVM.AddIncoming(phi, out incomingValues[0], out incomingBlocks[0], 2);

            valueStack.Push(new TypedValue(phi, BackendType.Bool));
        }

        void visitConditionalAND(AST.BinOp op)
        {
            Visit(op.left);
            var cmp = valueStack.Pop();
            if (cmp.type != BackendType.Bool)
                throw new BackendTypeMismatchException(BackendType.Bool, cmp.type);

            var cand_rhs = LLVM.AppendBasicBlock(ctx.function, "cand.rhs");
            var cand_end = LLVM.AppendBasicBlock(ctx.function, "cand.end");
            var block = LLVM.GetInsertBlock(builder);

            LLVM.BuildCondBr(builder, cmp.value, cand_rhs, cand_end);

            // cor.rhs: 
            LLVM.PositionBuilderAtEnd(builder, cand_rhs);
            Visit(op.right);
            var cor_rhs_tv = valueStack.Pop();
            if (cor_rhs_tv.type != BackendType.Bool)
                throw new BackendTypeMismatchException(BackendType.Bool, cor_rhs_tv.type);
            LLVM.BuildBr(builder, cand_end);

            // cor.end:
            LLVM.PositionBuilderAtEnd(builder, cand_end);
            var phi = LLVM.BuildPhi(builder, Const.BoolType, "corphi");

            LLVMBasicBlockRef[] incomingBlocks = new LLVMBasicBlockRef[2] { block, cand_rhs };
            LLVMValueRef[] incomingValues = new LLVMValueRef[2] { Const.False, cor_rhs_tv.value };

            LLVM.AddIncoming(phi, out incomingValues[0], out incomingBlocks[0], 2);

            valueStack.Push(new TypedValue(phi, BackendType.Bool));
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
            TypedValue result = new TypedValue();

            // TODO: check if integral type
            // TODO: handle non integral types
            result.type = TypedValue.MapType(node.type);
            switch (result.type)
            {
                case BackendType.Float32:
                    result.value = LLVM.BuildSIToFP(builder, v.value, Const.Float32Type, "float32_cast");
                    break;
                case BackendType.Int32:
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
                    result.type = BackendType.Int32;
                    break;
                case BackendType.Bool:
                    // TODO insert conversion to bool
                    throw new NotImplementedException();
                case BackendType.Void:
                    throw new InvalidCodePath();
                default:
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
            if (node.varDefinition.isFunctionParameter)
            {

                var pr = new TypedValue(LLVM.GetParam(ctx.function, (uint)node.varDefinition.parameterIdx),
                    TypedValue.MapType(node.varDefinition.type)); ;
                valueStack.Push(pr);
                return;
            }

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
            var cnt = node.argumentList.Count;
            LLVMValueRef[] parameters = new LLVMValueRef[Math.Max(1, cnt)];

            for (int i = 0; i < node.argumentList.Count; ++i)
            {
                Visit(node.argumentList[i]);
                parameters[i] = valueStack.Pop().value;
            }

            // TODO: use proper function declarations
            var t = default(BackendType);
            if (node.returnType == AST.FrontendType.int32)
            {
                t = BackendType.Int32;
            }
            else if (node.returnType == AST.FrontendType.float32)
            {
                t = BackendType.Float32;
            }
            else if (node.returnType == AST.FrontendType.bool_)
            {
                t = BackendType.Bool;
            }

            if (node.returnType == AST.FrontendType.void_)
            {
                LLVM.BuildCall(builder, f, out parameters[0], (uint)cnt, "");
            }
            else
            {
                var v = LLVM.BuildCall(builder, f, out parameters[0], (uint)cnt, node.functionName);
                valueStack.Push(new TypedValue(v, t));
            }
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

            var elseBlock = default(LLVMBasicBlockRef);
            if (node.elseBlock != null)
            {
                elseBlock = LLVM.AppendBasicBlock(ctx.function, "else");
            }
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

        public void Visit(AST.FunctionDeclaration node)
        {
            var fun = node.fun;

            if (functions.ContainsKey(fun.name))
                throw new Exception("function redefinition");

            var cnt = Math.Max(1, fun.parameters.Count);
            var par = new LLVMTypeRef[cnt];

            for (int i = 0; i < fun.parameters.Count; ++i)
            {
                par[i] = Const.GetTypeRef(TypedValue.MapType(fun.parameters[i].type));
            }

            var returnType = Const.GetTypeRef(TypedValue.MapType(fun.returnType));

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
