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

            public void EnterFunction(Function function) {
                currentFunction = function;
                vars = function.blocks["vars"];
                entry = function.blocks["entry"];
            }
        }

        public class Builder {
            Module mod;
            public Context context;
            public Block currentBlock;
            Value intrinsic_memcpy;
            public Builder(Module mod) {
                this.mod = mod;
                // add memcpy
                var ft = new FunctionType(Const.void_t, Const.ptr_t, Const.ptr_t, Const.i32_t, Const.bool_t);
                intrinsic_memcpy = mod.AddFunction("llvm.memcpy.p0i8.p0i8.i32", ft).value;
            }
            public Function CreateAndEnterFunction(string name, FunctionType ft) {
                var function = mod.AddFunction(name, ft);
                EnterFunction(function);
                return function;
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
            void AddOp(Value v) {
                currentBlock.ops.Add(v);
            }
            void AddOpGlobal(Value v) {
                mod.globals.ops.Add(v);
            }

            public Value BuildRet(Value ret) {
                var result = new Value(Op.Ret, ret);
                AddOp(result);
                return result;
            }
            public Value BuildBr(Block block) {
                var result = new Value(Op.Br, block.value);
                AddOp(result);
                return result;
            }
            public Value BuildCall(Value fun, params Value[] args) {
                var result = new Value(Op.Call, fun);
                if (args != null && args.Length > 0) {
                    result.args.AddRange(args);
                }
                Debug.Assert(fun.type.kind == TypeKind.Function);
                var ft = (FunctionType)fun.type;
                result.type = ft.returnType;

                AddOp(result);
                return result;
            }
            public Value BuildGlobalStringPtr(string str, string name = null) {
                var result = new Value(Op.GlobalStringPtr, ptr_t, str);
                result.name = name;
                AddOpGlobal(result);
                return result;
            }

            public Value BuildAlloca(Type t, string name = null) {
                var result = new Value(Op.Alloca, new PointerType(t));
                result.name = name;
                AddOp(result);
                return result;
            }
            public Value BuildArrayAlloca(Type t, Value size, string name = null) {
                var result = new Value(Op.Alloca, new PointerType(t), size);
                Debug.Assert(size.type.kind == TypeKind.Integer);
                result.name = name;
                AddOp(result);
                return result;
            }

            public Value AddGlobal(Type t, string name = null) {
                var result = new Value(Op.GlobalVariable, new PointerType(t));
                result.name = name;
                AddOpGlobal(result);
                return result;
            }

            public Value BuildBitCast(Value v, Type dest, string name = null) {
                var result = new Value(Op.BitCast, v);
                result.name = name;
                AddOp(result);
                return result;
            }
            public Value BuildMemCpy(Value destPtr, Value srcPtr, Value count, bool isVolatile = false) {
                Value result;
                Value isVolatile_v;

                if (isVolatile) {
                    isVolatile_v = true_v;
                } else {
                    isVolatile_v = false_v;
                }
                result = BuildCall(intrinsic_memcpy, destPtr, srcPtr, count, zero_i32_v, isVolatile_v);
                return result;
            }
            public Value BuildLoad(Value ptr, string name = null) {
                Debug.Assert(ptr.type.kind == TypeKind.Pointer);
                var pt = (PointerType)ptr.type;
                Value result = new Value(Op.Load, pt.elementType, ptr);
                result.name = name;
                AddOp(result);
                return result;
            }
            public Value BuildStore(Value v, Value ptr) {
                Debug.Assert(ptr.type.kind == TypeKind.Pointer);
                Value result = new Value(Op.Store, v, ptr);
                return result;
            }
            public Value BuildGEP(Value ptr, string name = null, params Value[] indices) {
                Value result = new Value(Op.GEP, ptr);
                result.args.AddRange(indices);
                result.name = name;

                Debug.Assert(ptr.type.kind == TypeKind.Pointer);
                Debug.Assert(indices != null && indices.Length > 0);
                Debug.Assert(indices[0].type.kind == TypeKind.Integer);

                var pt = (PointerType)ptr.type;
                var resultType = pt.elementType;
                for (int i = 1; i < indices.Length; ++i) {
                    var idx = indices[i];
                    Debug.Assert(idx.type.kind == TypeKind.Integer);
                    if (resultType.kind == TypeKind.Array) {
                        resultType = ((ArrayType)resultType).elementType;
                    } else if (resultType.kind == TypeKind.Struct) {
                        Debug.Assert(idx.isConst);
                        var elementIdx = (int)idx.dataInt;
                        var st = (StructType)resultType;
                        resultType = st.elementTypes[elementIdx];
                    }
                }
                result.type = resultType;
                AddOp(result);
                return result;
            }
            public Value BuildAnd(Value left, Value right, string name = null) {
                Value result = new Value(Op.Or, left, right);
                result.name = name;
                result.type = left.type;
                AddOp(result);
                return result;
            }
            public Value BuildOr(Value left, Value right, string name = null) {
                Value result = new Value(Op.Or, left, right);
                result.type = left.type;
                result.name = name;
                AddOp(result);
                return result;
            }
            public Value BuildXor(Value left, Value right, string name = null) {
                Value result = new Value(Op.Xor, left, right);
                result.type = left.type;
                result.name = name;
                AddOp(result);
                return result;
            }
            public Value BuildIcmp(Value left, Value right, IcmpType icmpType, string name = null) {
                Value result = new Value(Op.Icmp, left, right);
                result.type = bool_t;
                result.dataInt = (uint)icmpType;
                result.name = name;
                AddOp(result);
                return result;
            }
            public Value BuildAdd(Value left, Value right, string name = null) {
                Value result = new Value(Op.Add, left, right);
                result.type = left.type;
                result.name = name;
                AddOp(result);
                return result;
            }
            public Value BuildSub(Value left, Value right, string name = null) {
                Value result = new Value(Op.Sub, left, right);
                result.type = left.type;
                result.name = name;
                AddOp(result);
                return result;
            }
            public Value BuildMul(Value left, Value right, string name = null) {
                Value result = new Value(Op.Mul, left, right);
                result.type = left.type;
                result.name = name;
                AddOp(result);
                return result;
            }
            public Value BuildSDiv(Value left, Value right, string name = null) {
                Value result = new Value(Op.SDiv, left, right);
                result.type = left.type;
                result.name = name;
                AddOp(result);
                return result;
            }
            public Value BuildUDiv(Value left, Value right, string name = null) {
                Value result = new Value(Op.URem, left, right);
                result.type = left.type;
                result.name = name;
                AddOp(result);
                return result;
            }
            public Value BuildShl(Value left, Value right, string name = null) {
                Value result = new Value(Op.Shl, left, right);
                result.type = left.type;
                result.name = name;
                AddOp(result);
                return result;
            }
            public Value BuildAShr(Value left, Value right, string name = null) {
                Value result = new Value(Op.AShr, left, right);
                result.type = left.type;
                result.name = name;
                AddOp(result);
                return result;
            }
            public Value BuildLShr(Value left, Value right, string name = null) {
                Value result = new Value(Op.URem, left, right);
                result.type = left.type;
                result.name = name;
                AddOp(result);
                return result;
            }
            public Value BuildURem(Value left, Value right, string name = null) {
                Value result = new Value(Op.URem, left, right);
                result.type = left.type;
                result.name = name;
                AddOp(result);
                return result;
            }
            public Value BuildNegt(Value v, string name = null) {
                Value result = new Value(Op.Neg, v);
                result.type = v.type;
                result.name = name;
                AddOp(result);
                return result;
            }
            public Value BuildFAdd(Value left, Value right, string name = null) {
                Value result = new Value(Op.FAdd, left, right);
                result.type = left.type;
                result.name = name;
                AddOp(result);
                return result;
            }
            public Value BuildFSub(Value left, Value right, string name = null) {
                Value result = new Value(Op.FSub, left, right);
                result.type = left.type;
                result.name = name;
                AddOp(result);
                return result;
            }
            public Value BuildFMul(Value left, Value right, string name = null) {
                Value result = new Value(Op.FMul, left, right);
                result.type = left.type;
                result.name = name;
                AddOp(result);
                return result;
            }
            public Value BuildFDiv(Value left, Value right, string name = null) {
                Value result = new Value(Op.FDiv, left, right);
                result.type = left.type;
                result.name = name;
                AddOp(result);
                return result;
            }
            public Value BuildFRem(Value left, Value right, string name = null) {
                Value result = new Value(Op.FRem, left, right);
                result.type = left.type;
                result.name = name;
                AddOp(result);
                return result;
            }
            public Value BuildFCmp(Value left, Value right, FcmpType fcmpType, string name = null) {
                Value result = new Value(Op.Icmp, left, right);
                result.type = bool_t;
                result.dataInt = (uint)fcmpType;
                result.name = name;
                AddOp(result);
                return result;
            }
            public Value BuildPtrToInt(Value v, Type integerType, string name = null) {
                var result = new Value(Op.PtrToInt, v);
                result.type = integerType;
                result.name = name;
                AddOp(result);
                return result;
            }
            public Value BuildSizeOf(Type t, string name = null) {
                var np = new Value(Op.ConstPtr, new PointerType(t), 0, true);
                var size = BuildGEP(np, "size_of_trick", one_i32_v);
                var result = BuildPtrToInt(size, Const.mm_t, name);
                return result;
            }
        }

    }
}
