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
                Value result = new Value(Op.Load, ptr);
                result.name = name;
                AddOp(result);
                return result;
            }
            public Value BuildStore(Value v, Value ptr) {
                Value result = new Value(Op.Store, v, ptr);
                return result;
            }

            public Value BuildGEP(Value ptr, string name = null, params Value[] indices) {
                Value result = new Value(Op.GEP, ptr);
                result.args.AddRange(indices);
                result.name = name;
                AddOp(result);
                return result;
            }
        }

    }
}
