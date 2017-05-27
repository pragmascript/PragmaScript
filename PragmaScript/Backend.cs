using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PragmaScript {
    partial class SSA {

        public enum Op {
            ConstInt, ConstReal
        }

        public enum TypeKind {
            Void,
            Half,
            Float,
            Double,
            X86_FP80,
            FP128,
            PPC_FP128,
            Label,
            Integer,
            Function,
            Struct,
            Array,
            Pointer,
            Vector,
            Metadata,
            X86_MMX,
            Token
        }

        public class Module {
        }
        public class Block {
            public List<Value> ops;
        }
        public class Type {


        }

        
        public class Value {
            public int id;
            public Op op;
            public Type type;
            public List<Value> args;
            public ulong data;
        }

    }
}
