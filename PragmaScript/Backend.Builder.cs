using System.Diagnostics;
using static PragmaScript.SSA;
using static PragmaScript.SSA.Const;

namespace PragmaScript {
    partial class Backend {
        public Module mod;
        public Builder builder;

        public class Context {

            public Function currentFunction;
            public Block vars;
            public Block entry;
            public AST.Node callsite;

            System.Collections.Generic.Stack<(Block next, Block end)> loopStack = new System.Collections.Generic.Stack<(Block next, Block end)>();

            // TODO(pragma): ???
            public bool isGlobal { get { return currentFunction.name == "__init"; } }

            public void EnterFunction(Function function) {
                currentFunction = function;
                vars = function.blocks["vars"];
                entry = function.blocks["entry"];
            }

            public void SetCallsite(AST.Node callsite) {
                this.callsite = callsite;
            }

            public void PushLoop(Block loopNext, Block loopEnd) {
                loopStack.Push((loopNext, loopEnd));
            }
            public void PopLoop() {
            }
            public bool IsLoop() {
                return loopStack.Count > 0;
            }
            public (Block next, Block end) PeekLoop() {
                return loopStack.Peek();
            }

        }

        public class Builder {
            Module mod;
            public Context context;
            public Block currentBlock;
            Value intrinsic_memcpy;
            public Builder(Module mod) {
                context = new Context();
                this.mod = mod;
                // add memcpy
                var ft = new FunctionType(Const.void_t, Const.ptr_t, Const.ptr_t, Const.i32_t, Const.bool_t);
                intrinsic_memcpy = AddFunction(ft, "llvm.memcpy.p0i8.p0i8.i32");
            }
            public void EnterFunction(Function function) {
                context.EnterFunction(function);
                currentBlock = context.entry;
            }
            public void PositionAtEnd(Block block) {
                currentBlock = block;
            }
            public Block PositionAtEnd(string name) {
                var block = context.currentFunction.blocks[name];
                PositionAtEnd(block);
                return block;
            }
            public Block GetInsertBlock() {
                return currentBlock;
            }
            public Block AppendBasicBlock(string name) {
                var f = context.currentFunction;
                var result = f.AppendBasicBlock(name);
                return result;
            }
            public void MoveBasicBlockAfter(Block block, Block targetPosition) {
                block.function.MoveBasicBlockAfter(block, targetPosition);
            }
            void AddOp(Value v) {
                currentBlock.args.Add(v);
            }
            void AddOpGlobal(Value v) {
                mod.globals.args.Add(v);
            }

            public Value BuildRet(Value ret) {
                var result = new Value(Op.Ret, ret.type, ret);
                AddOp(result);
                return result;
            }
            public Value BuildRetVoid() {
                var result = new Value(Op.Ret, Const.void_t);
                AddOp(result);
                return result;
            }

            public Value BuildBr(Block block) {
                var result = new Value(Op.Br, null, block);
                AddOp(result);
                return result;
            }
            public Value BuildCondBr(Value cond, Block ifTrue, Block ifFalse) {
                Debug.Assert(SSAType.IsBoolType(cond.type));
                var result = new Value(Op.Br, null, cond, ifTrue, ifFalse);
                AddOp(result);
                return result;
            }
            public Value BuildCall(Value fun, string name = null, params Value[] args) {
                Debug.Assert(fun.type.kind == TypeKind.Function);
                var ft = (FunctionType)fun.type;
                var result = new Value(Op.Call, ft, fun);
                if (args != null && args.Length > 0) {
                    result.args.AddRange(args);
                }
                result.type = ft.returnType;

                AddOp(result);
                return result;
            }
            public GlobalStringPtr BuildGlobalStringPtr(string str, string name = null) {
                var result = new GlobalStringPtr(str, name);
                result.name = name;
                AddOpGlobal(result);
                return result;
            }

            public Value BuildAlloca(SSAType t, string name = null) {
                var result = new Value(Op.Alloca, new PointerType(t));
                result.name = name;
                AddOp(result);
                return result;
            }
            public Value BuildArrayAlloca(SSAType t, Value size, string name = null) {
                var result = new Value(Op.Alloca, new PointerType(t), size);
                Debug.Assert(size.type.kind == TypeKind.Integer);
                result.name = name;
                AddOp(result);
                return result;
            }

            public GlobalVariable AddGlobal(SSAType t, string name = null) {
                var result = new GlobalVariable(t, name);
                AddOpGlobal(result);
                return result;
            }

            public Value BuildBitCast(Value v, SSAType dest, string name = null) {
                var result = new Value(Op.BitCast, v.type, v);
                result.name = name;
                AddOp(result);
                return result;
            }
            public Value BuildMemCpy(Value destPtr, Value srcPtr, Value count, bool isVolatile = false) {
                Value isVolatile_v;
                if (isVolatile) {
                    isVolatile_v = true_v;
                } else {
                    isVolatile_v = false_v;
                }
                var result = BuildCall(intrinsic_memcpy, "memcpy", destPtr, srcPtr, count, zero_i32_v, isVolatile_v);
                return result;
            }
            public Value BuildLoad(Value ptr, string name = null) {
                Debug.Assert(ptr.type.kind == TypeKind.Pointer);
                var pt = (PointerType)ptr.type;
                var result = new Value(Op.Load, pt.elementType, ptr);
                result.name = name;
                AddOp(result);
                return result;
            }
            public Value BuildStore(Value v, Value ptr) {
                Debug.Assert(ptr.type.kind == TypeKind.Pointer);
                var result = new Value(Op.Store, null, v, ptr);
                return result;
            }
            public GetElementPtr BuildGEP(Value ptr, string name = null, bool inBounds = false, params Value[] indices) {
                var result = new GetElementPtr(ptr, name, inBounds, indices);
                AddOp(result);
                return result;
            }
            public Value BuildExtractValue(Value v, string name = null, params Value[] indices) {
                Debug.Assert(indices != null && indices.Length > 0);
                var result = new Value(Op.ExtractValue, null, indices);

                Debug.Assert(indices != null && indices.Length > 0);
                Debug.Assert(indices[0].type.kind == TypeKind.Integer);
                SSAType resultType = v.type;
                for (int i = 0; i < indices.Length; ++i) {
                    var idx = indices[i];
                    Debug.Assert(idx.type.kind == TypeKind.Integer);
                    if (resultType.kind == TypeKind.Array) {
                        resultType = ((ArrayType)resultType).elementType;
                    } else if (resultType.kind == TypeKind.Struct) {
                        Debug.Assert(idx.isConst);
                        var elementIdx = (int)(idx as ConstInt).data;
                        var st = (StructType)resultType;
                        resultType = st.elementTypes[elementIdx];
                    }
                }
                result.type = resultType;
                return result;
            }
            public Value BuildNot(Value v, string name = null) {
                var result = new Value(Op.Not, v.type, v);
                result.name = name;
                AddOp(result);
                return result;
            }
            public Value BuildAnd(Value left, Value right, string name = null) {
                var result = new Value(Op.Or, left.type, left, right);
                result.name = name;
                AddOp(result);
                return result;
            }
            public Value BuildOr(Value left, Value right, string name = null) {
                var result = new Value(Op.Or, left.type, left, right);
                result.name = name;
                AddOp(result);
                return result;
            }
            public Value BuildXor(Value left, Value right, string name = null) {
                var result = new Value(Op.Xor, left.type, left, right);
                result.name = name;
                AddOp(result);
                return result;
            }
            public ICmp BuildICmp(Value left, Value right, IcmpType icmpType, string name = null) {
                var result = new ICmp(left, right, icmpType, name);
                AddOp(result);
                return result;
            }
            public Value BuildAdd(Value left, Value right, string name = null) {
                var result = new Value(Op.Add, left.type, left, right);
                result.name = name;
                AddOp(result);
                return result;
            }
            public Value BuildSub(Value left, Value right, string name = null) {
                var result = new Value(Op.Sub, left.type, left, right);
                result.name = name;
                AddOp(result);
                return result;
            }
            public Value BuildMul(Value left, Value right, string name = null) {
                var result = new Value(Op.Mul, left.type, left, right);
                result.name = name;
                AddOp(result);
                return result;
            }
            public Value BuildSDiv(Value left, Value right, string name = null) {
                var result = new Value(Op.SDiv, left.type, left, right);
                result.name = name;
                AddOp(result);
                return result;
            }
            public Value BuildUDiv(Value left, Value right, string name = null) {
                var result = new Value(Op.URem, left.type, left, right);
                result.name = name;
                AddOp(result);
                return result;
            }
            public Value BuildShl(Value left, Value right, string name = null) {
                var result = new Value(Op.Shl, left.type, left, right);
                result.name = name;
                AddOp(result);
                return result;
            }
            public Value BuildAShr(Value left, Value right, string name = null) {
                var result = new Value(Op.AShr, left.type, left, right);
                result.name = name;
                AddOp(result);
                return result;
            }
            public Value BuildLShr(Value left, Value right, string name = null) {
                var result = new Value(Op.URem, left.type, left, right);
                result.name = name;
                AddOp(result);
                return result;
            }
            public Value BuildURem(Value left, Value right, string name = null) {
                var result = new Value(Op.URem, left.type, left, right);
                result.name = name;
                AddOp(result);
                return result;
            }
            public Value BuildNeg(Value v, string name = null) {
                var result = new Value(Op.Neg, v.type, v);
                result.name = name;
                AddOp(result);
                return result;
            }
            public Value BuildFAdd(Value left, Value right, string name = null) {
                var result = new Value(Op.FAdd, left.type, left, right);
                result.name = name;
                AddOp(result);
                return result;
            }
            public Value BuildFSub(Value left, Value right, string name = null) {
                var result = new Value(Op.FSub, left.type, left, right);
                result.name = name;
                AddOp(result);
                return result;
            }
            public Value BuildFMul(Value left, Value right, string name = null) {
                var result = new Value(Op.FMul, left.type, left, right);
                result.name = name;
                AddOp(result);
                return result;
            }
            public Value BuildFDiv(Value left, Value right, string name = null) {
                var result = new Value(Op.FDiv, left.type, left, right);
                result.name = name;
                AddOp(result);
                return result;
            }
            public Value BuildFRem(Value left, Value right, string name = null) {
                var result = new Value(Op.FRem, left.type, left, right);
                result.name = name;
                AddOp(result);
                return result;
            }
            public Value BuildFCmp(Value left, Value right, FcmpType fcmpType, string name = null) {
                var result = new FCmp(left, right, fcmpType, name);
                AddOp(result);
                return result;
            }
            public Value BuildFNeg(Value v, string name = null) {
                var result = new Value(Op.FNeg, v.type, v);
                result.name = name;
                AddOp(result);
                return result;
            }
            public Value BuildPtrToInt(Value v, SSAType integerType, string name = null) {
                var result = new Value(Op.PtrToInt, integerType, v);
                result.name = name;
                AddOp(result);
                return result;
            }
            public Value BuildIntToPtr(Value v, SSAType pointerType, string name = null) {
                var result = new Value(Op.IntToPtr, pointerType, v);
                result.name = name;
                AddOp(result);
                return result;
            }
            public Value BuildSizeOf(SSAType t, string name = null) {
                var np = new ConstPtr(new PointerType(t), 0);
                var size = BuildGEP(np, "size_of_trick", false, one_i32_v);
                var result = BuildPtrToInt(size, Const.mm_t, name);
                return result;
            }
            public Phi BuildPhi(SSAType t, string name = null, params (Value, Block)[] incoming) {
                var result = new Phi(t, name, incoming);
                AddOp(result);
                return result;
            }
            public Value BuildSExt(Value v, SSAType targetType, string name = null) {
                var result = new Value(Op.SExt, targetType, v);
                result.name = name;
                return result;
            }
            public Value BuildZExt(Value v, SSAType targetType, string name = null) {
                var result = new Value(Op.ZExt, targetType, v);
                result.name = name;
                return result;
            }
            public Value BuildTrunc(Value v, SSAType targetType, string name = null) {
                var result = new Value(Op.Trunc, targetType, v);
                result.name = name;
                return result;
            }
            public Value BuildFPToSI(Value v, SSAType targetType, string name = null) {
                var result = new Value(Op.FPToSI, targetType, v);
                result.name = name;
                return result;
            }
            public Value BuildFPToUI(Value v, SSAType targetType, string name = null) {
                var result = new Value(Op.FPToUI, targetType, v);
                result.name = name;
                return result;
            }
            public Value BuildSIToFP(Value v, SSAType targetType, string name = null) {
                var result = new Value(Op.SIToFP, targetType, v);
                result.name = name;
                return result;
            }
            public Value BuildUIToFP(Value v, SSAType targetType, string name = null) {
                var result = new Value(Op.UIToFP, targetType, v);
                result.name = name;
                return result;
            }
            public Value BuildFPCast(Value v, SSAType targetType, string name = null) {
                var result = new Value(Op.FPCast, targetType, v);
                result.name = name;
                return result;
            }
            public GetElementPtr BuildStructGEP(Value structPointer, int idx, string name = null) {
                return new GetElementPtr(structPointer, name, true, new ConstInt(i32_t, (ulong)idx));
            }
            public Value ConstNull(SSAType t) {
                
                switch (t.kind) {
                    case TypeKind.Integer:
                        return new ConstInt(t, 0);
                    case TypeKind.Float:
                        return new ConstReal(t, 0.0);
                    case TypeKind.Pointer:
                        return new ConstPtr(t, 0);
                    case TypeKind.Struct:
                    case TypeKind.Array:
                        // store <{ float, float, float }> zeroinitializer, <{ float, float, float }> * % struct_arg_1
                        var result = new Value(Op.ConstAggregateZero, t);
                        result.isConst = true;
                        return result;
                    default:
                        throw new System.NotImplementedException();
                }
            }

            public Function AddFunction(FunctionType ft, string name, string[] paramNames = null) {
                var result = new Function(ft, name, paramNames);
                mod.functions.Add(name, result);
                return result;
            }
            public Value GetParam(Function f, int idx) {
                return f.args[idx];
            }


        }

    }
}
