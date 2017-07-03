using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using static PragmaScript.SSA;
using static PragmaScript.SSA.Const;


namespace PragmaScript {
    partial class Backend {
        public Module mod;
        public Builder builder;


        public class Context {
            public class FunctionContext {
                public int nameCount = 0;
                public HashSet<string> names = new HashSet<string>();
                public Block vars;
                public Block entry;
            }

            public Dictionary<Function, FunctionContext> fcs = new Dictionary<Function, FunctionContext>();
            FunctionContext globalContext = new FunctionContext();
            public AST.Node callsite;
            System.Collections.Generic.Stack<(Block next, Block end)> loopStack = new System.Collections.Generic.Stack<(Block next, Block end)>();

            public Block currentBlock;
            public Function currentFunction {
                get { return currentBlock.function; }
            }
            public FunctionContext currentFunctionContext {
                get {
                    fcs.TryGetValue(currentFunction, out var result);
                    return result;
                }
            }


            public Context() {
            }

            // TODO(pragma): ???
            public bool isGlobal { get { return currentFunction.name == "@__init"; } }

            public FunctionContext CreateFunctionContext(Function f) {
                Debug.Assert(!fcs.ContainsKey(f));
                var result = new FunctionContext();
                fcs.Add(f, result);
                return result;
            }
            public void SetCurrentBlock(Block block) {
                currentBlock = block;
                Debug.Assert(fcs.ContainsKey(currentBlock.function));
            }
            public void SetFunctionBlocks(Function f, Block vars, Block entry) {
                var ctx = fcs[f];
                ctx.vars = vars;
                ctx.entry = entry;
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
            public string RequestLocalName_(string n, Function f = null) {
                if (n == null) {
                    return null;
                }
                if (f == null) {
                    f = currentFunction;
                }
                var fc = fcs[f];

                var prefix = "%" + n;
                var result = prefix;
                if (fc.names.Contains(result)) {
                    while (fc.names.Contains(result)) {
                        fc.nameCount++;
                        result = prefix + fc.nameCount.ToString();
                    }
                }
                fc.names.Add(result);
                return result;
            }
            public string RequestGlobalName(string n) {
                var gctx = globalContext;

                var prefix = "@" + n;
                var result = prefix;
                if (gctx.names.Contains(result)) {
                    while (gctx.names.Contains(result)) {
                        gctx.nameCount++;
                        result = prefix + "." + gctx.nameCount.ToString();
                    }
                }
                gctx.names.Add(result);
                return result;
            }
        }

        public class Builder {
            Module mod;
            public Context context;

            Value intrinsic_memcpy;
            public Builder(Module mod) {
                context = new Context();
                this.mod = mod;
                // add memcpy
                var ft = new FunctionType(Const.void_t, Const.ptr_t, Const.ptr_t, Const.i32_t, Const.i32_t, Const.bool_t);
                intrinsic_memcpy = AddFunction(ft, "llvm.memcpy.p0i8.p0i8.i32");
                var p0 = GetParam((Function)intrinsic_memcpy, 0);
                var p1 = GetParam((Function)intrinsic_memcpy, 1);
                p0.noalias = false;
                p0.nocapture = true;
                p1.noalias = false;
                p1.nocapture = true;
                p1.@readonly = true;
                var f = (Function)intrinsic_memcpy;
                f.attribs |= FunctionAttribs.argmemonly;
            }


            public void PositionAtEnd(Block block) {
                context.SetCurrentBlock(block);
            }
            public Block GetInsertBlock() {
                return context.currentBlock;
            }
            public Block AppendBasicBlock(Function f, string name) {
                name = context.RequestLocalName_(name, f);
                var result = f.AppendBasicBlock(name);
                return result;
            }
            public Block AppendBasicBlock(string name) {
                return AppendBasicBlock(context.currentFunction, name);
            }
            public void MoveBasicBlockAfter(Block block, Block targetPosition) {
                block.function.MoveBasicBlockAfter(block, targetPosition);
            }
            void AddOp(Value v, string name = null, Function f = null) {
                if (!v.isConst) {
                    if (name != null) {
                        v.name = context.RequestLocalName_(name, f);
                    }
                    context.currentBlock.args.Add(v);
                }
            }
            void AddOpGlobal(Value v, string name) {
                Debug.Assert(v is GlobalVariable || v is GlobalStringPtr || v is Function);

                if (name != null) {
                    v.name = context.RequestGlobalName(name);
                }
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
                Debug.Assert(fun.type.kind == TypeKind.Pointer);
                var ft = (fun.type as PointerType).elementType as FunctionType;
                Debug.Assert(ft != null);

                var result = new Value(Op.Call, ft, fun);
                if (args != null && args.Length > 0) {
                    result.args.AddRange(args);
                }
                result.type = ft.returnType;
                if (result.type.kind != TypeKind.Void) {
                    AddOp(result, name);
                } else {
                    AddOp(result);
                }

                return result;
            }
            public Value BuildGlobalStringPtr(string str, string name = null) {
                var gs = new GlobalStringPtr(str);
                AddOpGlobal(gs, name);
                var result = BuildBitCast(gs, ptr_t);
                Debug.Assert(result.isConst);
                return result;
            }

            public Value BuildAlloca(SSAType t, string name = null) {
                var result = new Value(Op.Alloca, new PointerType(t));
                AddOp(result, name);
                return result;
            }
            public Value BuildArrayAlloca(SSAType t, Value size, string name = null) {
                var result = new Value(Op.Alloca, new PointerType(t), size);
                Debug.Assert(size.type.kind == TypeKind.Integer);
                AddOp(result, name);
                return result;
            }

            public GlobalVariable AddGlobal(SSAType t, string name = null) {
                var result = new GlobalVariable(t);
                AddOpGlobal(result, name);
                return result;
            }
            public Value BuildBitCast(Value v, SSAType dest, string name = null) {
                var result = new Value(Op.BitCast, dest, v);
                result.isConst = v.isConst;
                AddOp(result, name);
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
                AddOp(result, name);
                return result;
            }
            public Value BuildStore(Value v, Value ptr) {
                Debug.Assert(ptr.type.kind == TypeKind.Pointer);
                var result = new Value(Op.Store, null, v, ptr);
                AddOp(result);
                return result;
            }
            public GetElementPtr BuildGEP(Value ptr, string name = null, bool inBounds = false, params Value[] indices) {
                var result = new GetElementPtr(ptr, inBounds, indices);
                result.isConst = ptr.isConst && indices.All(idx => idx.isConst);
                AddOp(result, name);
                return result;
            }
            public GetElementPtr BuildStructGEP(Value structPointer, int idx, string name = null) {
                var result = new GetElementPtr(structPointer, true, zero_i32_v, new ConstInt(i32_t, (ulong)idx));
                result.isConst = structPointer.isConst;
                AddOp(result, name);
                return result;
            }
            public Value BuildExtractValue(Value v, string name = null, params Value[] indices) {
                Debug.Assert(indices != null && indices.Length > 0);
                var result = new Value(Op.ExtractValue, null, v);
                result.args.AddRange(indices);

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
                result.isConst = v.isConst;
                AddOp(result, name);
                return result;
            }
            public Value BuildNot(Value v, string name = null) {
                var result = new Value(Op.Not, v.type, v);
                result.isConst = v.isConst;
                AddOp(result, name);
                return result;
            }
            public Value BuildAnd(Value left, Value right, string name = null) {
                var result = new Value(Op.And, left.type, left, right);
                result.isConst = left.isConst && right.isConst;
                AddOp(result, name);
                return result;
            }
            public Value BuildOr(Value left, Value right, string name = null) {
                var result = new Value(Op.Or, left.type, left, right);
                result.isConst = left.isConst && right.isConst;
                AddOp(result, name);
                return result;
            }
            public Value BuildXor(Value left, Value right, string name = null) {
                var result = new Value(Op.Xor, left.type, left, right);
                result.isConst = left.isConst && right.isConst;
                AddOp(result, name);
                return result;
            }
            public ICmp BuildICmp(Value left, Value right, IcmpType icmpType, string name = null) {
                var result = new ICmp(left, right, icmpType, name);
                result.isConst = left.isConst && right.isConst;
                AddOp(result, name);
                return result;
            }
            public Value BuildAdd(Value left, Value right, string name = null) {
                var result = new Value(Op.Add, left.type, left, right);
                result.isConst = left.isConst && right.isConst;
                AddOp(result, name);
                return result;
            }
            public Value BuildSub(Value left, Value right, string name = null) {
                var result = new Value(Op.Sub, left.type, left, right);
                result.isConst = left.isConst && right.isConst;
                AddOp(result, name);
                return result;
            }
            public Value BuildMul(Value left, Value right, string name = null) {
                var result = new Value(Op.Mul, left.type, left, right);
                result.isConst = left.isConst && right.isConst;
                AddOp(result, name);
                return result;
            }
            public Value BuildSDiv(Value left, Value right, string name = null) {
                var result = new Value(Op.SDiv, left.type, left, right);
                result.isConst = left.isConst && right.isConst;
                AddOp(result, name);
                return result;
            }
            public Value BuildUDiv(Value left, Value right, string name = null) {
                var result = new Value(Op.UDiv, left.type, left, right);
                result.isConst = left.isConst && right.isConst;
                AddOp(result, name);
                return result;
            }
            public Value BuildShl(Value left, Value right, string name = null) {
                var result = new Value(Op.Shl, left.type, left, right);
                result.isConst = left.isConst && right.isConst;
                AddOp(result, name);
                return result;
            }
            public Value BuildAShr(Value left, Value right, string name = null) {
                var result = new Value(Op.AShr, left.type, left, right);
                result.isConst = left.isConst && right.isConst;
                AddOp(result, name);
                return result;
            }
            public Value BuildLShr(Value left, Value right, string name = null) {
                var result = new Value(Op.LShr, left.type, left, right);
                result.isConst = left.isConst && right.isConst;
                AddOp(result, name);
                return result;
            }
            public Value BuildURem(Value left, Value right, string name = null) {
                var result = new Value(Op.URem, left.type, left, right);
                result.isConst = left.isConst && right.isConst;
                AddOp(result, name);
                return result;
            }
            public Value BuildSRem(Value left, Value right, string name = null) {
                var result = new Value(Op.SRem, left.type, left, right);
                result.isConst = left.isConst && right.isConst;
                AddOp(result, name);
                return result;
            }
            public Value BuildNeg(Value v, string name = null) {
                Debug.Assert(v.type.kind == TypeKind.Integer);
                return BuildSub(ConstNull(v.type), v, name);
            }
            public Value BuildFAdd(Value left, Value right, string name = null) {
                var result = new Value(Op.FAdd, left.type, left, right);
                result.isConst = left.isConst && right.isConst;
                AddOp(result, name);
                return result;
            }
            public Value BuildFSub(Value left, Value right, string name = null) {
                var result = new Value(Op.FSub, left.type, left, right);
                result.isConst = left.isConst && right.isConst;
                AddOp(result, name);
                return result;
            }
            public Value BuildFMul(Value left, Value right, string name = null) {
                var result = new Value(Op.FMul, left.type, left, right);
                result.isConst = left.isConst && right.isConst;
                AddOp(result, name);
                return result;
            }
            public Value BuildFDiv(Value left, Value right, string name = null) {
                var result = new Value(Op.FDiv, left.type, left, right);
                result.isConst = left.isConst && right.isConst;
                AddOp(result, name);
                return result;
            }
            public Value BuildFRem(Value left, Value right, string name = null) {
                var result = new Value(Op.FRem, left.type, left, right);
                result.isConst = left.isConst && right.isConst;
                AddOp(result, name);
                return result;
            }
            public Value BuildFCmp(Value left, Value right, FcmpType fcmpType, string name = null) {
                var result = new FCmp(left, right, fcmpType, name);
                result.isConst = left.isConst && right.isConst;
                AddOp(result, name);
                return result;
            }
            public Value BuildFNeg(Value v, string name = null) {
                return BuildFSub(ConstNull(v.type), v, name);
            }
            public Value BuildPtrToInt(Value v, SSAType integerType, string name = null) {
                var result = new Value(Op.PtrToInt, integerType, v);
                result.isConst = v.isConst;
                AddOp(result, name);
                return result;
            }
            public Value BuildIntToPtr(Value v, SSAType pointerType, string name = null) {
                var result = new Value(Op.IntToPtr, pointerType, v);
                result.isConst = v.isConst;
                AddOp(result, name);
                return result;
            }
            public Value BuildSizeOf(SSAType t, string name = null) {
                var np = new ConstPtr(new PointerType(t), 0);
                var size = BuildGEP(np, "size_of_trick", false, one_i32_v);
                var result = BuildPtrToInt(size, Const.mm_t, name);
                Debug.Assert(result.isConst);
                return result;
            }
            public Phi BuildPhi(SSAType t, string name = null, params (Value, Block)[] incoming) {
                var result = new Phi(t, name, incoming);
                AddOp(result, name);
                return result;
            }
            public Value BuildSExt(Value v, SSAType targetType, string name = null) {
                var result = new Value(Op.SExt, targetType, v);
                result.isConst = v.isConst;
                AddOp(result, name);
                return result;
            }
            public Value BuildZExt(Value v, SSAType targetType, string name = null) {
                var result = new Value(Op.ZExt, targetType, v);
                result.isConst = v.isConst;
                AddOp(result, name);
                return result;
            }
            public Value BuildTrunc(Value v, SSAType targetType, string name = null) {
                var result = new Value(Op.Trunc, targetType, v);
                result.isConst = v.isConst;
                AddOp(result, name);
                return result;
            }
            public Value BuildFPToSI(Value v, SSAType targetType, string name = null) {
                var result = new Value(Op.FPToSI, targetType, v);
                result.isConst = v.isConst;
                AddOp(result, name);
                return result;
            }
            public Value BuildFPToUI(Value v, SSAType targetType, string name = null) {
                var result = new Value(Op.FPToUI, targetType, v);
                result.isConst = v.isConst;
                AddOp(result, name);
                return result;
            }
            public Value BuildSIToFP(Value v, SSAType targetType, string name = null) {
                var result = new Value(Op.SIToFP, targetType, v);
                result.isConst = v.isConst;
                AddOp(result, name);
                return result;
            }
            public Value BuildUIToFP(Value v, SSAType targetType, string name = null) {
                var result = new Value(Op.UIToFP, targetType, v);
                result.isConst = v.isConst;
                AddOp(result, name);
                return result;
            }
            public Value BuildFPCast(Value v, SSAType targetType, string name = null) {
                var result = new Value(Op.FPCast, targetType, v);
                result.isConst = v.isConst;
                AddOp(result, name);
                return result;
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
                var result = new Function(ft);
                context.CreateFunctionContext(result);
                if (paramNames != null) {
                    for (int i = 0; i < paramNames.Length; ++i) {
                        paramNames[i] = context.RequestLocalName_(paramNames[i], result);
                    }
                }
                result.SetParamNames(paramNames);
                AddOpGlobal(result, name);
                return result;
            }
            public FunctionArgument GetParam(Function f, int idx) {
                return (FunctionArgument)f.args[idx];
            }
        }

    }
}
