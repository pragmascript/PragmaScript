using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PragmaScript {
    class SSA {

        public enum Op {
            ConstInt, ConstReal, ConstPtr, ConstVoid, GlobalStringPtr, GlobalVariable, Function,
            Label, Ret, Br, Call,
            Alloca,
            BitCast,
            Store,
            GEP,
            Load,
            And,
            Or,
            Xor,
            ICmp,
            Add,
            Sub,
            Mul,
            SDiv,
            UDiv,
            URem,
            SRem,
            Shl,
            AShr,
            LShr,
            FAdd,
            FSub,
            FMul,
            FDiv,
            FRem,
            FCmp,
            PtrToInt,
            Phi,
            Not,
            SExt,
            ZExt,
            Trunc,
            FPToSI,
            FPToUI,
            SIToFP,
            UIToFP,
            FPCast,
            IntToPtr,
            ConstAggregateZero,
            FunctionArgument,
            ExtractValue,
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
            public Block globals;
            public Module() {
                globals = new Block(null, "globals");
            }
        }

        public abstract class SSAType {
            public TypeKind kind;
            public SSAType(TypeKind kind) {
                this.kind = kind;
            }

            public static bool IsBoolType(SSAType t) {
                var it = t as IntegerType;
                if (it != null) {
                    if (it.bitWidth == 1) {
                        return true;
                    }
                }
                return false;
            }

            public abstract bool EqualType(SSAType other);
        }

        public class VoidType : SSAType {
            public VoidType()
                : base(TypeKind.Void) {
            }
            public override bool EqualType(SSAType other) {
                return other is VoidType;
            }
        }

        public class FunctionType : SSAType {
            public FunctionType(SSAType returnType, params SSAType[] argumentTypes)
                : base(TypeKind.Function) {
                this.returnType = returnType;
                this.argumentTypes.AddRange(argumentTypes);
            }
            public SSAType returnType;
            public List<SSAType> argumentTypes = new List<SSAType>();
            public override bool EqualType(SSAType other) {
                if (other is FunctionType ft) {
                    if (!returnType.EqualType(ft.returnType)) {
                        return false;
                    }
                    if (argumentTypes.Count != ft.argumentTypes.Count) {
                        return false;
                    }
                    for (int i = 0; i < argumentTypes.Count; ++i) {
                        if (!argumentTypes[i].EqualType(ft.argumentTypes[i])) {
                            return false;
                        }
                    }
                    return true;
                } else {
                    return false;
                }
            }
        }

        public class IntegerType : SSAType {
            public IntegerType(int bitWidth)
                : base(TypeKind.Integer) {
                this.bitWidth = bitWidth;
            }
            public int bitWidth;
            public override bool EqualType(SSAType other) {
                if (other is IntegerType it) {
                    return bitWidth == it.bitWidth;
                } else {
                    return false;
                }
            }
        }

        public class FloatType : SSAType {
            public enum FloatWidths {
                fp16, fp32, fp64
            }
            public FloatWidths width;
            public int BitWidth {
                get {
                    switch (width) {
                        case FloatWidths.fp16:
                            return 16;
                        case FloatWidths.fp32:
                            return 32;
                        case FloatWidths.fp64:
                            return 64;
                        default:
                            throw new InvalidCodePath();
                    }
                }
            }
            public FloatType(FloatWidths width)
                : base(TypeKind.Float) {
                this.width = width;
            }
            public override bool EqualType(SSAType other) {
                if (other is FloatType ft) {
                    return width == ft.width;
                } else {
                    return false;
                }
            }
        }

        public class PointerType : SSAType {
            public SSAType elementType;
            public PointerType(SSAType elementType)
                : base(TypeKind.Pointer) {
                this.elementType = elementType;
            }
            public override bool EqualType(SSAType other) {
                if (other is PointerType pt) {
                    return elementType.EqualType(pt.elementType);
                } else {
                    return false;
                }
            }
        }

        public class VectorType : SSAType {
            public int elementCount;
            public SSAType elementType;
            public VectorType(int elementCount, SSAType elementType)
                : base(TypeKind.Vector) {
                this.elementCount = elementCount;
                this.elementType = elementType;
            }
            public override bool EqualType(SSAType other) {
                if (other is VectorType vt) {
                    if (elementCount != vt.elementCount) {
                        return false;
                    }
                    if (!elementType.EqualType(vt.elementType)) {
                        return false;
                    }
                    return true;
                } else {
                    return false;
                }
            }
        }

        public class LabelType : SSAType {
            public LabelType()
                : base(TypeKind.Label) {
            }
            public override bool EqualType(SSAType other) {
                throw new NotImplementedException();
            }
        }

        public class MetadataType : SSAType {
            public MetadataType()
                : base(TypeKind.Metadata) {
            }
            public override bool EqualType(SSAType other) {
                throw new NotImplementedException();
            }
        }

        public class ArrayType : SSAType {
            public uint elementCount;
            public SSAType elementType;
            public ArrayType(SSAType elementType, uint elementCount)
                : base(TypeKind.Array) {
                this.elementCount = elementCount;
                this.elementType = elementType;
            }
            public override bool EqualType(SSAType other) {
                if (other is ArrayType at) {
                    if (elementCount != at.elementCount) {
                        return false;
                    }
                    if (!elementType.EqualType(at.elementType)) {
                        return false;
                    }
                    return true;
                } else {
                    return false;
                }
            }
        }

        public class StructType : SSAType {
            public StructType(bool packed)
                : base(TypeKind.Struct) {
                this.packed = packed;
            }
            public bool packed;
            public List<SSAType> elementTypes = new List<SSAType>();
            public override bool EqualType(SSAType other) {
                if (other is StructType st) {
                    if (elementTypes.Count != st.elementTypes.Count) {
                        return false;
                    }
                    for (int i = 0; i < elementTypes.Count; ++i) {
                        if (!elementTypes[i].EqualType(st.elementTypes[i])) {
                            return false;
                        }
                    }
                    return true;
                } else {
                    return false;
                }
            }
        }

        public enum IcmpType {
            eq, ne, ugt, uge, ult, ule, sgt, sge, slt, sle
        }
        public enum FcmpType {
            @false, oeq, ogt, oge, olt, ole, one, ord, ueq, ugt, uge, ult, ule, une, uno, @true
        }

        public class Value {
            public Op op;
            public SSAType type;
            public List<Value> args;
            public string name;
            public bool isConst = false;
            public Value(Op op, SSAType t, params Value[] args) {
                this.op = op;
                type = t;
                if (args != null && args.Length > 0) {
                    this.args = new List<Value>();
                    this.args.AddRange(args);
                }
            }
            public bool IsTerminator() {
                switch (op) {
                    case Op.Ret:
                    case Op.Br:
                        return true;
                    default:
                        return false;
                }
            }

        }

        public class FunctionArgument : Value {
            public bool noalias = false;
            public bool nocapture = false;
            public bool @readonly = false;
            public FunctionArgument(SSAType type)
                : base(Op.FunctionArgument, type) {
                if (type.kind == TypeKind.Pointer) {
                    noalias = true;
                }
            }
        }
        [Flags]
        public enum FunctionAttribs {
            nounwind=1, readnone=2, argmemonly=4,
        }
        public class Function : Value {
            public List<Block> blocks;
            public bool exportDLL = false;
            public bool internalLinkage = true;
            public bool isStub = false;
            
            public FunctionAttribs attribs = FunctionAttribs.nounwind;

            public Function(FunctionType ft)
                : base(Op.Function, new PointerType(ft)) {
                args = new List<Value>();
                blocks = null;
            }
            public void SetParamNames(string[] paramNames) {
                var ft = (FunctionType)((PointerType)type).elementType;
                Debug.Assert(paramNames == null || paramNames.Length == ft.argumentTypes.Count);
                for (int idx = 0; idx < ft.argumentTypes.Count; ++idx) {
                    var arg = new FunctionArgument(ft.argumentTypes[idx]);
                    if (paramNames != null) {
                        arg.name = paramNames[idx];
                    }
                    args.Add(arg);
                }
            }
            public Block AppendBasicBlock(string name) {
                if (blocks == null) {
                    blocks = new List<Block>();
                }
                var b = new Block(this, name);
                blocks.Add(b);
                return b;
            }
            public void MoveBasicBlockAfter(Block b, Block targetBlock) {
                Debug.Assert(b.function == this && targetBlock.function == this);
                blocks.Remove(b);
                var idx = blocks.IndexOf(targetBlock);
                blocks.Insert(idx + 1, b);
            }
            public void MoveBasicBlockBefore(Block b, Block targetBlock) {
                Debug.Assert(b.function == this && targetBlock.function == this);
                blocks.Remove(b);
                var idx = blocks.IndexOf(targetBlock);
                blocks.Insert(idx, b);
            }

        }

        public class Block : Value {
            public Function function;
            public Block(Function f, string name)
            : base(Op.Label, Const.label_t) {
                this.name = name;
                this.function = f;
                args = new List<Value>();
            }
            public bool HasTerminator() {
                if (args.Count == 0) {
                    return false;
                }
                var last = args.Last();
                return last.IsTerminator();
            }

            public Value GetTerminator() {
                if (args.Count == 0) {
                    return null;
                }
                var last = args.Last();
                if (last.IsTerminator()) {
                    return last;
                } else {
                    return null;
                }
            }
        }

        public class ConstInt : Value {
            public ulong data;
            public ConstInt(SSAType t, ulong data)
                : base(Op.ConstInt, t) {
                isConst = true;
                Debug.Assert(t.kind == TypeKind.Integer);
                this.data = data;
            }
        }
        public class ConstPtr : Value {
            public ulong data;
            public ConstPtr(SSAType pointerType, ulong data)
                : base(Op.ConstPtr, pointerType) {
                Debug.Assert(pointerType.kind == TypeKind.Pointer);
                isConst = true;
                this.data = data;
            }
        }
        public class ConstReal : Value {
            public double data;
            public ConstReal(SSAType t, double data)
                : base(Op.ConstReal, t) {
                isConst = true;
                Debug.Assert(t.kind == TypeKind.Half ||
                             t.kind == TypeKind.Float ||
                             t.kind == TypeKind.Double);
                this.data = data;
            }
        }

        public class GlobalStringPtr : Value {
            public string data;
            public GlobalStringPtr(string data)
                : base(Op.GlobalStringPtr, new PointerType(new ArrayType(Const.i8_t, (uint)data.Length + 1))) {
                isConst = true;
                this.data = data;
            }
        }

        public class GlobalVariable : Value {
            
            public Value initializer = null;
            public bool isConstantVariable = false;
            public GlobalVariable(SSAType t)
                : base(Op.GlobalVariable, new PointerType(t)) {
                isConst = true;
            }
            public void SetInitializer(Value v) {
                initializer = v;
            }
        }

        public class ICmp : Value {
            public IcmpType icmpType;
            public ICmp(Value left, Value right, IcmpType icmpType, string name)
                : base(Op.ICmp, Const.bool_t, left, right) {
                this.name = name;
                this.icmpType = icmpType;
            }
        }

        public class FCmp : Value {
            public FcmpType fcmpType;
            public FCmp(Value left, Value right, FcmpType fcmpType, string name)
                : base(Op.FCmp, Const.bool_t, left, right) {
                this.name = name;
                this.fcmpType = fcmpType;
            }
        }

        public class Phi : Value {
            public List<(Value v, Block b)> incoming;
            public Phi(SSAType t, string name = null, params (Value, Block)[] incoming)
                : base(Op.Phi, t) {
                Debug.Assert(incoming != null && incoming.Length > 0);
                this.incoming = new List<(Value v, Block b)>();
#if DEBUG
                for (int idx = 0; idx < incoming.Length; ++idx) {
                    Debug.Assert(t == incoming[idx].Item1.type);
                }
#endif
                this.incoming.AddRange(incoming);

            }
        }

        public class GetElementPtr : Value {
            public bool inBounds;
            public SSAType baseType;
            public GetElementPtr(Value ptr, bool inBounds = false, params Value[] indices)
                : base(Op.GEP, null, ptr) {
                args.AddRange(indices);

                Debug.Assert(ptr.type.kind == TypeKind.Pointer);
                Debug.Assert(indices != null && indices.Length > 0);
                Debug.Assert(indices[0].type.kind == TypeKind.Integer);

                var pt = (PointerType)ptr.type;
                baseType = pt.elementType;
                var resultType = pt.elementType;
                for (int i = 1; i < indices.Length; ++i) {
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

                this.type = new PointerType(resultType);
                this.inBounds = inBounds;
            }
        }


        public class Const {
            const int NATIVE_POINTER_WIDTH = 64;

            public static readonly LabelType label_t = new LabelType();

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

            public static readonly Value void_v = new Value(Op.ConstVoid, void_t);
            public static readonly ConstInt true_v = new ConstInt(bool_t, 1) { isConst = true };
            public static readonly Value false_v = new ConstInt(bool_t, 0);
            public static readonly Value zero_i32_v = new ConstInt(i32_t, 0);
            public static readonly Value one_i32_v = new ConstInt(i32_t, 1);
            public static readonly Value neg_1_i32_v = new ConstInt(i32_t, unchecked((ulong)-1));
            public static readonly Value zero_i64_v = new ConstInt(i32_t, 0);
            public static readonly Value null_ptr_v = new ConstPtr(ptr_t, 0);

            public static SSAType GetTypeRef(FrontendType t) {
                return getTypeRef(t, 0);
            }
            static SSAType getTypeRef(FrontendType t, int depth) {
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
            static SSAType getTypeRef(FrontendStructType t, int depth) {
                // TODO(pragma): don't assume PACKED!
                bool packed = true;
                var result = new StructType(packed);
                foreach (var f in t.fields) {
                    result.elementTypes.Add(getTypeRef(f.type, depth + 1));
                }
                return result;
            }
            static SSAType getTypeRef(FrontendArrayType t, int depth) {
                // TODO(pragma): don't assume PACKED!
                var result = new StructType(true);
                result.elementTypes.Add(Const.i32_t);
                result.elementTypes.Add(new PointerType(getTypeRef(t.elementType, depth)));
                return result;
            }
            static SSAType getTypeRef(FrontendPointerType t, int depth) {
                if (depth > 0 && t.elementType is FrontendStructType) {
                    return Const.ptr_t;
                } else {
                    var et = getTypeRef(t.elementType, depth);
                    return new PointerType(et);
                }
            }
            static SSAType getTypeRef(FrontendFunctionType t, int depth) {
                var ft = new FunctionType(getTypeRef(t.returnType, depth));
                foreach (var p in t.parameters) {
                    ft.argumentTypes.Add(getTypeRef(p.type, depth));
                }
                return new PointerType(ft);
            }
        }
    }
}
