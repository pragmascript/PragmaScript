using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using static PragmaScript.SSA;
using static PragmaScript.SSA.Const;


namespace PragmaScript
{
    partial class Backend
    {
        public Module mod;
        public Builder builder;


        public class Context
        {
            public class FunctionContext
            {
                public int nameCount = 0;
                public HashSet<string> names = new HashSet<string>();
                public Block vars;
                public Block entry;
                public Block @return;
                public List<(Value, Block)> returnEdges = new List<(Value, Block)>();

                public void RegisterReturnEdge(Value returnValue, Block from)
                {
                    returnEdges.Add((returnValue, from));
                }
            }

            public Dictionary<Function, FunctionContext> fcs = new Dictionary<Function, FunctionContext>();
            public AST.Node callsite;

            FunctionContext globalContext = new FunctionContext();
            System.Collections.Generic.Stack<(Block next, Block end)> loopStack = new System.Collections.Generic.Stack<(Block next, Block end)>();

            public Block currentBlock;
            public Function currentFunction
            {
                get { return currentBlock.function; }
            }
            public FunctionContext currentFunctionContext
            {
                get
                {
                    fcs.TryGetValue(currentFunction, out var result);
                    return result;
                }
            }

            // public AST.Node debugContextNode;


            public Context()
            {
            }

            // TODO(pragma): ???
            public bool isGlobal { get { return currentFunction.name == "@__init"; } }

            public FunctionContext CreateFunctionContext(Function f)
            {
                Debug.Assert(!fcs.ContainsKey(f));
                var result = new FunctionContext();
                fcs.Add(f, result);
                return result;
            }
            public void SetCurrentBlock(Block block)
            {
                currentBlock = block;
                Debug.Assert(fcs.ContainsKey(currentBlock.function));
            }

            public void SetFunctionBlocks(Function f, Block vars, Block entry, Block @return)
            {
                var ctx = fcs[f];
                ctx.vars = vars;
                ctx.entry = entry;
                ctx.@return = @return;
            }

            public void SetCallsite(AST.Node callsite)
            {
                this.callsite = callsite;
            }

            public void PushLoop(Block loopNext, Block loopEnd)
            {
                loopStack.Push((loopNext, loopEnd));
            }
            public void PopLoop()
            {
                loopStack.Pop();
            }
            public bool IsLoop()
            {
                return loopStack.Count > 0;
            }
            public (Block next, Block end) PeekLoop()
            {
                return loopStack.Peek();
            }
            public string RequestLocalName_(string n, Function f = null)
            {
                if (n == null)
                {
                    return null;
                }
                if (f == null)
                {
                    f = currentFunction;
                }
                var fc = fcs[f];

                var prefix = "%" + n;
                var result = prefix;
                if (fc.names.Contains(result))
                {
                    while (fc.names.Contains(result))
                    {
                        fc.nameCount++;
                        result = prefix + fc.nameCount.ToString();
                    }
                }
                fc.names.Add(result);
                return result;
            }
            public string RequestGlobalName(string n)
            {
                var gctx = globalContext;

                var prefix = "@" + n;
                var result = prefix;
                if (gctx.names.Contains(result))
                {
                    while (gctx.names.Contains(result))
                    {
                        gctx.nameCount++;
                        result = prefix + "." + gctx.nameCount.ToString();
                    }
                }
                gctx.names.Add(result);
                return result;
            }
        }

        public class Builder
        {
            Module mod;
            public Context context;

            Value intrinsic_memcpy;
            public Builder(Module mod)
            {
                context = new Context();
                this.mod = mod;
                // add memcpy
                var ft = new FunctionType(Const.void_t, Const.ptr_t, Const.ptr_t, Const.i32_t, Const.i32_t, Const.bool_t);
                intrinsic_memcpy = AddFunction(ft, null, "llvm.memcpy.p0i8.p0i8.i32");
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


            public void PositionAtEnd(Block block)
            {
                context.SetCurrentBlock(block);
            }
            public Block GetInsertBlock()
            {
                return context.currentBlock;
            }
            public Block AppendBasicBlock(Function f, string name)
            {
                name = context.RequestLocalName_(name, f);
                var result = f.AppendBasicBlock(name);
                return result;
            }
            public void RemoveBasicBlock(Block block)
            {
                var f = context.currentFunction;
                f.blocks.Remove(block);
            }
            public Block AppendBasicBlock(string name)
            {
                return AppendBasicBlock(context.currentFunction, name);
            }
            public void MoveBasicBlockAfter(Block block, Block targetPosition)
            {
                block.function.MoveBasicBlockAfter(block, targetPosition);
            }
            void AddOp(Value v, string name = null, Function f = null, AST.Node contextNode = null)
            {
                if (!v.isConst)
                {
                    if (name != null)
                    {
                        v.name = context.RequestLocalName_(name, f);
                    }
                    context.currentBlock.args.Add(v);
                }
                if (context.callsite == null)
                {
                    v.debugContextNode = contextNode;
                }
                else
                {
                    v.debugContextNode = context.callsite;
                }

            }
            void AddOpGlobal(Value v, string name, AST.Node contextNode = null)
            {
                Debug.Assert(v is GlobalVariable || v is GlobalStringPtr || v is Function);

                if (name != null)
                {
                    v.name = context.RequestGlobalName(name);
                }
                mod.globals.args.Add(v);
                v.debugContextNode = contextNode;
            }
            public Value BuildRet(Value ret, AST.Node contextNode)
            {
                var result = new Value(Op.Ret, ret.type, ret);
                AddOp(result, contextNode: contextNode);
                return result;
            }
            public Value BuildRetVoid(AST.Node contextNode)
            {
                var result = new Value(Op.Ret, Const.void_t);
                AddOp(result, contextNode: contextNode);
                return result;
            }

            public Value BuildBr(Block block, AST.Node contextNode)
            {
                var result = new Value(Op.Br, null, block);
                AddOp(result, contextNode: contextNode);
                return result;
            }
            public Value BuildCondBr(Value cond, Block ifTrue, Block ifFalse, AST.Node contextNode)
            {
                Debug.Assert(SSAType.IsBoolType(cond.type));
                var result = new Value(Op.Br, null, cond, ifTrue, ifFalse);
                AddOp(result, contextNode: contextNode);
                return result;
            }
            public Value BuildCall(Value fun, AST.Node contextNode, string name = null, params Value[] args)
            {
                Debug.Assert(fun.type.kind == TypeKind.Pointer);
                var ft = (fun.type as PointerType).elementType as FunctionType;
                Debug.Assert(ft != null);

                var result = new Value(Op.Call, ft, fun);
                if (args != null && args.Length > 0)
                {
                    result.args.AddRange(args);
                }
                result.type = ft.returnType;
                if (result.type.kind != TypeKind.Void)
                {
                    AddOp(result, name, contextNode: contextNode);
                }
                else
                {
                    AddOp(result, contextNode: contextNode);
                }

                return result;
            }
            public Value BuildGlobalStringPtr(string str, AST.Node contextNode, string name = null)
            {
                var gs = new GlobalStringPtr(str);
                AddOpGlobal(gs, name, contextNode: contextNode);
                var result = BuildBitCast(gs, ptr_t, contextNode);
                Debug.Assert(result.isConst);
                return result;
            }
            public Value BuildAlloca(SSAType t, AST.Node contextNode, string name = null, int align = 0)
            {
                if (align > 16)
                {
                    align = 16;
                }
                var result = new Value(Op.Alloca, new PointerType(t));
                result.alignment = align;
                AddOp(result, name, contextNode: contextNode);
                return result;
            }
            public Value BuildArrayAlloca(SSAType t, Value size, AST.Node contextNode, string name = null)
            {
                var result = new Value(Op.Alloca, new PointerType(t), size);
                result.alignment = 16;
                Debug.Assert(size.type.kind == TypeKind.Integer);
                AddOp(result, name, contextNode: contextNode);
                return result;
            }

            public GlobalVariable AddGlobal(SSAType t, AST.Node contextNode, string name = null, bool isConst = false, int align = 0)
            {
                if (align > 16)
                {
                    align = 16;
                }
                var result = new GlobalVariable(t, isConst);
                result.alignment = align;

                AddOpGlobal(result, name, contextNode: contextNode);
                return result;
            }
            public Value BuildBitCast(Value v, SSAType dest, AST.Node contextNode, string name = null)
            {
                var result = new Value(Op.BitCast, dest, v);
                result.isConst = v.isConst;
                AddOp(result, name, contextNode: contextNode);
                return result;
            }
            public Value BuildMemCpy(Value destPtr, Value srcPtr, Value count, AST.Node contextNode, bool isVolatile = false)
            {
                Value isVolatile_v;
                if (isVolatile)
                {
                    isVolatile_v = true_v;
                }
                else
                {
                    isVolatile_v = false_v;
                }
                var result = BuildCall(intrinsic_memcpy, contextNode, "memcpy", destPtr, srcPtr, count, zero_i32_v, isVolatile_v);
                return result;
            }
            public Value BuildLoad(Value ptr, AST.Node contextNode, string name = null, bool isVolatile = false, int align = 0)
            {
                if (align > 16)
                {
                    align = 16;
                }
                Debug.Assert(ptr.type.kind == TypeKind.Pointer);
                var pt = (PointerType)ptr.type;
                var result = new Value(Op.Load, pt.elementType, ptr);
                result.alignment = align;
                if (isVolatile)
                {
                    result.flags |= SSAFlags.@volatile;
                }
                AddOp(result, name, contextNode: contextNode);
                return result;
            }
            public Value BuildStore(Value v, Value ptr, AST.Node contextNode, bool isVolatile = false, int align = 0)
            {
                if (align > 16)
                {
                    align = 16;
                }
                Debug.Assert(ptr.type.kind == TypeKind.Pointer);
                var result = new Value(Op.Store, null, v, ptr);
                result.alignment = align;
                if (isVolatile)
                {
                    result.flags |= SSAFlags.@volatile;
                }
                AddOp(result, contextNode: contextNode);
                return result;
            }
            public GetElementPtr BuildGEP(Value ptr, AST.Node contextNode, string name = null, bool inBounds = false, params Value[] indices)
            {
                var result = new GetElementPtr(ptr, inBounds, indices);
                result.isConst = ptr.isConst && indices.All(idx => idx.isConst);
                AddOp(result, name, contextNode: contextNode);
                return result;
            }
            public GetElementPtr BuildStructGEP(Value structPointer, int idx, AST.Node contextNode, string name = null)
            {
                var result = new GetElementPtr(structPointer, true, zero_i32_v, new ConstInt(i32_t, (ulong)idx));
                result.isConst = structPointer.isConst;
                AddOp(result, name, contextNode: contextNode);
                return result;
            }
            public Value BuildExtractValue(Value v, AST.Node contextNode, string name = null, params Value[] indices)
            {
                Debug.Assert(indices != null && indices.Length > 0);
                var result = new Value(Op.ExtractValue, null, v);
                result.args.AddRange(indices);

                Debug.Assert(indices != null && indices.Length > 0);
                Debug.Assert(indices[0].type.kind == TypeKind.Integer);
                SSAType resultType = v.type;
                for (int i = 0; i < indices.Length; ++i)
                {
                    var idx = indices[i];
                    Debug.Assert(idx.type.kind == TypeKind.Integer);
                    if (resultType.kind == TypeKind.Array)
                    {
                        resultType = ((ArrayType)resultType).elementType;
                    }
                    else if (resultType.kind == TypeKind.Struct)
                    {
                        Debug.Assert(idx.isConst);
                        var elementIdx = (int)(idx as ConstInt).data;
                        var st = (StructType)resultType;
                        resultType = st.elementTypes[elementIdx];
                    }
                }
                result.type = resultType;
                result.isConst = v.isConst;
                AddOp(result, name, contextNode: contextNode);
                return result;
            }
            public Value BuildNot(Value v, AST.Node contextNode, string name = null)
            {
                var result = new Value(Op.Not, v.type, v);
                result.isConst = v.isConst;
                AddOp(result, name, contextNode: contextNode);
                return result;
            }
            public Value BuildAnd(Value left, Value right, AST.Node contextNode, string name = null)
            {
                var result = new Value(Op.And, left.type, left, right);
                result.isConst = left.isConst && right.isConst;
                AddOp(result, name, contextNode: contextNode);
                return result;
            }
            public Value BuildOr(Value left, Value right, AST.Node contextNode, string name = null)
            {
                var result = new Value(Op.Or, left.type, left, right);
                result.isConst = left.isConst && right.isConst;
                AddOp(result, name, contextNode: contextNode);
                return result;
            }
            public Value BuildXor(Value left, Value right, AST.Node contextNode, string name = null)
            {
                var result = new Value(Op.Xor, left.type, left, right);
                result.isConst = left.isConst && right.isConst;
                AddOp(result, name, contextNode: contextNode);
                return result;
            }
            public ICmp BuildICmp(Value left, Value right, IcmpType icmpType, AST.Node contextNode, string name = null)
            {
                var result = new ICmp(left, right, icmpType, name);
                result.isConst = left.isConst && right.isConst;
                AddOp(result, name, contextNode: contextNode);
                return result;
            }
            public Value BuildAdd(Value left, Value right, AST.Node contextNode, string name = null)
            {
                var result = new Value(Op.Add, left.type, left, right);
                result.isConst = left.isConst && right.isConst;
                AddOp(result, name, contextNode: contextNode);
                return result;
            }
            public Value BuildSub(Value left, Value right, AST.Node contextNode, string name = null)
            {
                var result = new Value(Op.Sub, left.type, left, right);
                result.isConst = left.isConst && right.isConst;
                AddOp(result, name, contextNode: contextNode);
                return result;
            }
            public Value BuildMul(Value left, Value right, AST.Node contextNode, string name = null)
            {
                var result = new Value(Op.Mul, left.type, left, right);
                result.isConst = left.isConst && right.isConst;
                AddOp(result, name, contextNode: contextNode);
                return result;
            }
            public Value BuildSDiv(Value left, Value right, AST.Node contextNode, string name = null)
            {
                var result = new Value(Op.SDiv, left.type, left, right);
                result.isConst = left.isConst && right.isConst;
                AddOp(result, name, contextNode: contextNode);
                return result;
            }
            public Value BuildUDiv(Value left, Value right, AST.Node contextNode, string name = null)
            {
                var result = new Value(Op.UDiv, left.type, left, right);
                result.isConst = left.isConst && right.isConst;
                AddOp(result, name, contextNode: contextNode);
                return result;
            }
            public Value BuildShl(Value left, Value right, AST.Node contextNode, string name = null)
            {
                var result = new Value(Op.Shl, left.type, left, right);
                result.isConst = left.isConst && right.isConst;
                AddOp(result, name, contextNode: contextNode);
                return result;
            }
            public Value BuildAShr(Value left, Value right, AST.Node contextNode, string name = null)
            {
                var result = new Value(Op.AShr, left.type, left, right);
                result.isConst = left.isConst && right.isConst;
                AddOp(result, name, contextNode: contextNode);
                return result;
            }
            public Value BuildLShr(Value left, Value right, AST.Node contextNode, string name = null)
            {
                var result = new Value(Op.LShr, left.type, left, right);
                result.isConst = left.isConst && right.isConst;
                AddOp(result, name, contextNode: contextNode);
                return result;
            }
            public Value BuildURem(Value left, Value right, AST.Node contextNode, string name = null)
            {
                var result = new Value(Op.URem, left.type, left, right);
                result.isConst = left.isConst && right.isConst;
                AddOp(result, name, contextNode: contextNode);
                return result;
            }
            public Value BuildSRem(Value left, Value right, AST.Node contextNode, string name = null)
            {
                var result = new Value(Op.SRem, left.type, left, right);
                result.isConst = left.isConst && right.isConst;
                AddOp(result, name, contextNode: contextNode);
                return result;
            }
            public Value BuildNeg(Value v, AST.Node contextNode, string name = null)
            {
                Debug.Assert(v.type.kind == TypeKind.Integer);
                return BuildSub(ConstNull(v.type), v, contextNode, name);
            }
            public Value BuildFAdd(Value left, Value right, AST.Node contextNode, string name = null)
            {
                var result = new Value(Op.FAdd, left.type, left, right);
                result.isConst = left.isConst && right.isConst;
                AddOp(result, name, contextNode: contextNode);
                return result;
            }
            public Value BuildFSub(Value left, Value right, AST.Node contextNode, string name = null)
            {
                var result = new Value(Op.FSub, left.type, left, right);
                result.isConst = left.isConst && right.isConst;
                AddOp(result, name, contextNode: contextNode);
                return result;
            }
            public Value BuildFMul(Value left, Value right, AST.Node contextNode, string name = null)
            {
                var result = new Value(Op.FMul, left.type, left, right);
                result.isConst = left.isConst && right.isConst;
                AddOp(result, name, contextNode: contextNode);
                return result;
            }
            public Value BuildFDiv(Value left, Value right, AST.Node contextNode, string name = null)
            {
                var result = new Value(Op.FDiv, left.type, left, right);
                result.isConst = left.isConst && right.isConst;
                AddOp(result, name, contextNode: contextNode);
                return result;
            }
            public Value BuildFRem(Value left, Value right, AST.Node contextNode, string name = null)
            {
                var result = new Value(Op.FRem, left.type, left, right);
                result.isConst = left.isConst && right.isConst;
                AddOp(result, name, contextNode: contextNode);
                return result;
            }
            public Value BuildFCmp(Value left, Value right, FcmpType fcmpType, AST.Node contextNode, string name = null)
            {
                var result = new FCmp(left, right, fcmpType, name);
                result.isConst = left.isConst && right.isConst;
                AddOp(result, name, contextNode: contextNode);
                return result;
            }
            public Value BuildFNeg(Value v, AST.Node contextNode, string name = null)
            {
                return BuildFSub(ConstNull(v.type), v, contextNode, name);
            }
            public Value BuildPtrToInt(Value v, SSAType integerType, AST.Node contextNode, string name = null)
            {
                var result = new Value(Op.PtrToInt, integerType, v);
                result.isConst = v.isConst;
                AddOp(result, name, contextNode: contextNode);
                return result;
            }
            public Value BuildIntToPtr(Value v, SSAType pointerType, AST.Node contextNode, string name = null)
            {
                var result = new Value(Op.IntToPtr, pointerType, v);
                result.isConst = v.isConst;
                AddOp(result, name, contextNode: contextNode);
                return result;
            }
            public Value BuildSizeOf(SSAType t, AST.Node contextNode, string name = null)
            {
                var np = new ConstPtr(new PointerType(t), 0);
                var size = BuildGEP(np, contextNode, "size_of_trick", false, one_i32_v);
                var result = BuildPtrToInt(size, Const.mm_t, contextNode, name);
                Debug.Assert(result.isConst);
                return result;
            }
            public Phi BuildPhi(SSAType t, AST.Node contextNode, string name = null, params (Value, Block)[] incoming)
            {
                var result = new Phi(t, name, incoming);
                AddOp(result, name, contextNode: contextNode);
                return result;
            }


            public Value BuildSExt(Value v, SSAType targetType, AST.Node contextNode, string name = null)
            {
                var result = new Value(Op.SExt, targetType, v);
                result.isConst = v.isConst;
                AddOp(result, name, contextNode: contextNode);
                return result;
            }
            public Value BuildZExt(Value v, SSAType targetType, AST.Node contextNode, string name = null)
            {
                var result = new Value(Op.ZExt, targetType, v);
                result.isConst = v.isConst;
                AddOp(result, name, contextNode: contextNode);
                return result;
            }
            public Value BuildTrunc(Value v, SSAType targetType, AST.Node contextNode, string name = null)
            {
                var result = new Value(Op.Trunc, targetType, v);
                result.isConst = v.isConst;
                AddOp(result, name, contextNode: contextNode);
                return result;
            }
            public Value BuildFPToSI(Value v, SSAType targetType, AST.Node contextNode, string name = null)
            {
                var result = new Value(Op.FPToSI, targetType, v);
                result.isConst = v.isConst;
                AddOp(result, name, contextNode: contextNode);
                return result;
            }
            public Value BuildFPToUI(Value v, SSAType targetType, AST.Node contextNode, string name = null)
            {
                var result = new Value(Op.FPToUI, targetType, v);
                result.isConst = v.isConst;
                AddOp(result, name, contextNode: contextNode);
                return result;
            }
            public Value BuildSIToFP(Value v, SSAType targetType, AST.Node contextNode, string name = null)
            {
                var result = new Value(Op.SIToFP, targetType, v);
                result.isConst = v.isConst;
                AddOp(result, name, contextNode: contextNode);
                return result;
            }
            public Value BuildUIToFP(Value v, SSAType targetType, AST.Node contextNode, string name = null)
            {
                var result = new Value(Op.UIToFP, targetType, v);
                result.isConst = v.isConst;
                AddOp(result, name, contextNode: contextNode);
                return result;
            }
            public Value BuildFPCast(Value v, SSAType targetType, AST.Node contextNode, string name = null)
            {
                var result = new Value(Op.FPCast, targetType, v);
                result.isConst = v.isConst;
                AddOp(result, name, contextNode: contextNode);
                return result;
            }
            public void BuildComment(string comment, AST.Node contextNode, string name = null)
            {
                if (CompilerOptions._i.debug && CompilerOptions._i.ll)
                {
                    var result = new Emit($"; {comment}");
                    AddOp(result, name, contextNode: contextNode);
                }
            }
            public Value BuildEmit(string instr, AST.Node contextNode, string name = null)
            {
                var result = new Emit(instr);
                AddOp(result, name, contextNode: contextNode);
                return result;
            }
            public Value BuildCmpxchg(Value dest, Value comperand, Value target, AST.Node contextNode, string name = null)
            {
                Debug.Assert(dest.type.kind == TypeKind.Pointer &&
                    ((PointerType)dest.type).elementType.EqualType(target.type) &&
                    ((PointerType)dest.type).elementType.EqualType(comperand.type));

                var resultType = new StructType(false);
                resultType.elementTypes.Add(target.type);
                resultType.elementTypes.Add(new IntegerType(1));
                var result = new Value(Op.Cmpxchg, resultType, dest, comperand, target);
                AddOp(result, name, contextNode: contextNode);
                return result;
            }
            public Value BuildAtomicRMW(Value ptr, Value value, AtomicRMWType rmwType, AST.Node contextNode, string name = null)
            {
                Debug.Assert(ptr.type.kind == TypeKind.Pointer &&
                    ((PointerType)ptr.type).elementType.EqualType(value.type));
                var result = new AtomicRMW(ptr, value, rmwType);
                AddOp(result, name, contextNode: contextNode);
                return result;
            }
            public Value BuildExtractElement(Value vec, Value idx, AST.Node contextNode, string name = null)
            {
                Debug.Assert(vec.type.kind == TypeKind.Vector);
                var vt = (VectorType)vec.type;
                var result = new Value(Op.ExtractElement, vt.elementType, vec, idx);
                AddOp(result, name, contextNode: contextNode);
                return result;
            }
            public Value BuildInsertElement(Value vec, Value element, Value idx, AST.Node contextNode, string name = null)
            {
                Debug.Assert(vec.type.kind == TypeKind.Vector);
                var vt = (VectorType)vec.type;
                Debug.Assert(vt.elementType.EqualType(element.type));
                var result = new Value(Op.InsertElement, vt, vec, element, idx);
                AddOp(result, name, contextNode: contextNode);
                return result;
            }
            public Value BuildShuffleVector(Value vecA, Value vecB, Value mask, AST.Node contextNode, string name = null)
            {
                Debug.Assert(vecA.type.kind == TypeKind.Vector);
                Debug.Assert(vecB.type.kind == TypeKind.Vector);
                Debug.Assert(mask.type.kind == TypeKind.Vector);
                Debug.Assert(mask.isConst);
                Debug.Assert(vecA.type.EqualType(vecB.type));
                Debug.Assert(((VectorType)mask.type).elementType.EqualType(Const.i32_t
                ));
                var vt = vecA.type;
                var result = new Value(Op.ShuffleVector, vt, vecA, vecB, mask);
                AddOp(result, name, contextNode: contextNode);
                return result;
            }
            public Value ConstNull(SSAType t)
            {
                switch (t.kind)
                {
                    case TypeKind.Integer:
                        return new ConstInt(t, 0);
                    case TypeKind.Float:
                        return new ConstReal(t, 0.0);
                    case TypeKind.Pointer:
                        return new ConstPtr(t, 0);
                    case TypeKind.Struct:
                    case TypeKind.Array:
                    case TypeKind.Vector:
                        // store <{ float, float, float }> zeroinitializer, <{ float, float, float }> * % struct_arg_1
                        var result = new Value(Op.ConstAggregateZero, t);
                        result.isConst = true;
                        return result;
                    default:
                        throw new System.NotImplementedException();
                }
            }

            public Function AddFunction(FunctionType ft, AST.Node contextNode, string name, string[] paramNames = null)
            {
                var result = new Function(ft);
                context.CreateFunctionContext(result);
                if (paramNames != null)
                {
                    for (int i = 0; i < paramNames.Length; ++i)
                    {
                        paramNames[i] = context.RequestLocalName_(paramNames[i], result);
                    }
                }
                result.SetParamNames(paramNames);
                AddOpGlobal(result, name, contextNode: contextNode);
                return result;
            }
            public FunctionArgument GetParam(Function f, int idx)
            {
                return (FunctionArgument)f.args[idx];
            }
        }

    }
}
