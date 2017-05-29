using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static PragmaScript.SSA;

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

            void AddOp(Value v) {
                currentBlock.ops.Add(v);
            }
            public void BuildRet(Value ret) {
                AddOp(new Value(Op.Ret, ret));
            }
            public void BuildBr(Block block) {
                AddOp(new Value(Op.Br, block.value));
            }
            public void BuildCall(Value fun) {
                AddOp(new Value(Op.Call, fun));
            }

        }

    }
}
