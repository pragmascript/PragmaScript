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
            public Builder(Module mod) {
                this.mod = mod;
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
            public Value BuildCall(Value fun) {
                var result = new Value(Op.Call, fun);
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

        }

    }
}
