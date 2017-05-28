using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PragmaScript {
    class SSA {

        public enum Op {
            ConstInt, ConstReal, ConstPtr, FunctionDeclaration
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


        public class Function {
            public Value value;
            public List<Block> blocks;
            public Function(Value value) {
                this.value = value;
                blocks = null;
            }
        }

        public class Module {
            Dictionary<string, Function> functions = new Dictionary<string, Function>();
            public Value AddFunctionDecl(string name, FunctionType ft) {
                var result = new Value(Op.FunctionDeclaration, ft);
                var f = new Function(result);
                functions.Add(name, f);
                return result;
            }
        }
        public class Block {
            public List<Value> ops;
        }
        public class Type {
            public TypeKind kind;
            public Type(TypeKind kind) {
                this.kind = kind;
            }
        }

        public class VoidType : Type {
            public VoidType()
                : base(TypeKind.Void) {
            }
        }

        public class FunctionType : Type {
            public FunctionType(Type returnType, params Type[] argumentTypes)
                : base(TypeKind.Function) {
                this.returnType = returnType;
                this.argumentTypes.AddRange(argumentTypes);
            }
            public Type returnType;
            public List<Type> argumentTypes = new List<Type>();
        }

        public class IntegerType : Type {
            public IntegerType(int bitWidth)
                : base(TypeKind.Integer) {
                this.bitWidth = bitWidth;
            }
            public int bitWidth;
        }

        public class FloatType : Type {
            public enum FloatWidths {
                fp16, fp32, fp64
            }
            public FloatWidths width;
            public FloatType(FloatWidths width)
                : base(TypeKind.Float) {
                this.width = width;
            }
        }

        public class PointerType : Type {
            public Type elementType;
            public PointerType(Type elementType)
                : base(TypeKind.Pointer) {
                this.elementType = elementType;
            }
        }

        public class VectorType : Type {
            public int elementCount;
            public Type elementType;
            public VectorType(int elementCount, Type elementType)
                : base(TypeKind.Vector) {
                this.elementCount = elementCount;
                this.elementType = elementType;
            }
        }

        public class LabelType : Type {
            public LabelType()
                : base(TypeKind.Label) {
            }
        }

        public class MetadataType : Type {
            public MetadataType()
                : base(TypeKind.Metadata) {
            }
        }

        public class ArrayType : Type {
            public int elementCount;
            public Type elementType;
            public ArrayType(int elementCount, Type elementType)
                : base(TypeKind.Array) {
                this.elementCount = elementCount;
                this.elementType = elementType;
            }
        }

        public class StructType : Type {
            public StructType()
                : base(TypeKind.Struct) {
            }
            public List<Type> elementTypes = new List<Type>();
        }

        public class Value {
            public Op op;
            public Type type;
            public List<Value> args;
            public ulong data;
            public Value(Op op, Type t, ulong data = 0) {
                this.op = op;
                this.type = t;
                this.data = data;
            }
        }

        public class Const {
            const int NATIVE_POINTER_WIDTH = 64;

            public static readonly IntegerType bool_t = new IntegerType(1);
            public static readonly IntegerType i8_t = new IntegerType(8);
            public static readonly IntegerType i16_t = new IntegerType(16);
            public static readonly IntegerType i32_t = new IntegerType(32);
            public static readonly IntegerType i64_t = new IntegerType(64);

            // TODO(pragma): make this compilation platform dependent
            public static readonly IntegerType mm_t = new IntegerType(NATIVE_POINTER_WIDTH);

            public static readonly FloatType f16_t = new FloatType(FloatType.FloatWidths.fp16);
            public static readonly FloatType f32_t = new FloatType(FloatType.FloatWidths.fp32);
            public static readonly FloatType f64_t = new FloatType(FloatType.FloatWidths.fp64);
            public static readonly PointerType ptr_t = new PointerType(i8_t);
            public static readonly VoidType void_t = new VoidType();

            public static readonly Value true_v = new Value(Op.ConstInt, bool_t, 1);
            public static readonly Value false_v = new Value(Op.ConstInt, bool_t, 0);
            public static readonly Value zero_i32_v = new Value(Op.ConstInt, i32_t, 0);
            public static readonly Value one_i32_v = new Value(Op.ConstInt, i32_t, 1);
            public static readonly Value neg_1_i32_v = new Value(Op.ConstInt, i32_t, unchecked((ulong)-1));
            public static readonly Value zero_i64_v = new Value(Op.ConstInt, i64_t, 0);
            public static readonly Value null_ptr_v = new Value(Op.ConstPtr, ptr_t, 0);

        }

        
        static Type getTypeRef(FrontendType t, int depth = 0) {
            if (t.Equals(FrontendType.i8)) {
                return Const.i8_t;
            }
            if (t.Equals(FrontendType.i16)) {
                return Const.i16_t;
            }
            if (t.Equals(FrontendType.i32)) {
                return Const.i32_t;
            }
            if (t.Equals(FrontendType.i64)) {
                return Const.i64_t;
            }
            if (t.Equals(FrontendType.mm)) {
                return Const.mm_t;
            }
            if (t.Equals(FrontendType.f32)) {
                return Const.f32_t;
            }
            if (t.Equals(FrontendType.f64)) {
                return Const.f64_t;
            }
            if (t.Equals(FrontendType.bool_)) {
                return Const.bool_t;
            }
            if (t.Equals(FrontendType.void_)) {
                return Const.void_t;
            }
            if (t.Equals(FrontendType.string_)) {
                return getTypeRef(t as FrontendArrayType, depth);
            }
            switch (t) {
                case FrontendArrayType ta:
                    return getTypeRef(ta, depth);
                case FrontendStructType ts:
                    return getTypeRef(ts, depth);
                case FrontendPointerType tp:
                    return getTypeRef(tp, depth);
                case FrontendFunctionType tf:
                    return getTypeRef(tf, depth);
            }
            throw new InvalidCodePath();
        }
        static Type getTypeRef(FrontendStructType t, int depth) {
            var result = new StructType();
            foreach (var f in t.fields) {
                result.elementTypes.Add(getTypeRef(f.type));
            }
            return result;
        }
        static Type getTypeRef(FrontendArrayType t, int depth) {
            var result = new StructType();
            result.elementTypes.Add(Const.i32_t);
            result.elementTypes.Add(new PointerType(getTypeRef(t.elementType)));
            return result;
        }
        static Type getTypeRef(FrontendPointerType t, int depth) {
            if (depth > 0 && t.elementType is FrontendStructType) {
                return Const.ptr_t;
            } else {
                var et = getTypeRef(t.elementType, depth);
                return new PointerType(et);
            }
        }
        static Type getTypeRef(FrontendFunctionType t, int depth) {
            var ft = new FunctionType(getTypeRef(t.returnType));
            foreach (var p in t.parameters) {
                ft.argumentTypes.Add(getTypeRef(p.type));
            }
            return new PointerType(ft);
        }



   }
}
