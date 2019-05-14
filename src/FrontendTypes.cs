using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace PragmaScript {
    public class FrontendType
    {
        public static readonly FrontendType none = new FrontendType("$$__none__$$");
        public static readonly FrontendType void_ = new FrontendType("void");
        public static readonly FrontendType i8 = new FrontendType("i8");
        public static readonly FrontendType i16 = new FrontendType("i16");
        public static readonly FrontendType i32 = new FrontendType("i32");
        public static readonly FrontendType i64 = new FrontendType("i64");
        public static readonly FrontendType f32 = new FrontendType("f32");
        public static readonly FrontendType f64 = new FrontendType("f64");
        public static readonly FrontendType bool_ = new FrontendType("bool");
        public static readonly FrontendType mm = new FrontendType("mm");

        public static readonly FrontendSliceType string_ = new FrontendSliceType(i8, "string");
        public static readonly FrontendPointerType ptr = new FrontendPointerType(i8);


        public string name;
        internal bool preResolved = false;

        protected FrontendType()
        {
        }

        public FrontendType(string name)
        {
            this.name = name;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString()
        {
            return name;
        }
        public override bool Equals(object obj)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(ToString()));
            Debug.Assert(!string.IsNullOrWhiteSpace(obj.ToString()));

            return ToString() == obj.ToString();
        }



        public static bool operator ==(FrontendType t1, FrontendType t2)
        {
            // only compare against null not other type
            if (!ReferenceEquals(t1, null) && !ReferenceEquals(t2, null))
                throw new InvalidCodePath();

            return ReferenceEquals(t1, t2);
        }

        public static bool operator !=(FrontendType t1, FrontendType t2)
        {
            return !(t1 == t2);
        }

        public static bool IsIntegerType(FrontendType t)
        {
            bool result = false;
            result |= t.Equals(i8);
            result |= t.Equals(i16);
            result |= t.Equals(i32);
            result |= t.Equals(i64);
            result |= t.Equals(mm);
            return result;
        }

        public static bool IsBoolType(FrontendType t) {
            return t.Equals(bool_);
        }

        public static bool IsVoidType(FrontendType t) {
            return t.Equals(void_);
        }

        public static bool IsIntegerOrLateBind(FrontendType t)
        {
            if (IsIntegerType(t)) {
                return true;
            }
            if (t is FrontendNumberType) {
                var tn = t as FrontendNumberType;
                if (tn.floatType) {
                    return false;
                }
                tn.Bind(tn.Default());
                return true;
            }
            return false;
        }

        public static bool IntegersOrLateBind(FrontendType a, FrontendType b)
        {
            // TODO: resolve to same bit width
            return IsIntegerOrLateBind(a) && IsIntegerOrLateBind(b);
        }

        public static bool IsFloatType(FrontendType t)
        {
            bool result = false;
            result |= t.Equals(f32);
            result |= t.Equals(f64);
            return result;
        }
        public static bool AllowedTypeCastAndLateBind(FrontendType a, FrontendType b)
        {
            var a_is_number = a is FrontendNumberType;
            var b_is_number = b is FrontendNumberType;
            if (a_is_number || b_is_number) {
                if (a_is_number && b_is_number) {
                    var an = a as FrontendNumberType;
                    var bn = b as FrontendNumberType;
                    an.others.Add(bn);
                    bn.others.Add(an);
                    if (an.floatType || bn.floatType) {
                        an.floatType = true;
                        bn.floatType = true;
                    }
                    return true;
                }

                if (a_is_number) {
                    if (IsIntegerType(b)) {
                        var an = a as FrontendNumberType;
                        if (!an.floatType) {
                            an.Bind(b);
                        }
                    } else if (IsFloatType(b)) {
                        (a as FrontendNumberType).Bind(b);
                    }
                }
                if (b_is_number) {
                    if (IsIntegerType(a)) {
                        var bn = b as FrontendNumberType;
                        if (!bn.floatType) {
                            bn.Bind(a);
                        }
                    } else if (IsFloatType(a)) {
                        (b as FrontendNumberType).Bind(a);
                    }
                }
            }
            // TODO: actually check if cast is allowed here!
            return true;
        }
        public static bool CompatibleAndLateBind(FrontendType a, FrontendType b)
        {
            var a_is_number = a is FrontendNumberType;
            var b_is_number = b is FrontendNumberType;
            if (a_is_number || b_is_number) {
                if (a_is_number && b_is_number) {
                    var an = a as FrontendNumberType;
                    var bn = b as FrontendNumberType;
                    an.others.Add(bn);
                    bn.others.Add(an);
                    if (an.floatType || bn.floatType) {
                        an.floatType = true;
                        bn.floatType = true;
                    }

                    return true;
                }

                if (a_is_number) {
                    if (IsIntegerType(b)) {
                        var an = a as FrontendNumberType;
                        if (an.floatType) {
                            return false;
                        } else {
                            an.Bind(b);
                            return true;
                        }
                    } else if (IsFloatType(b)) {
                        (a as FrontendNumberType).Bind(b);
                        return true;
                    } else {
                        return false;
                    }
                }
                if (b_is_number) {
                    if (IsIntegerType(a)) {
                        var bn = b as FrontendNumberType;
                        if (bn.floatType) {
                            return false;
                        } else {
                            bn.Bind(a);
                            return true;
                        }
                    } else if (IsFloatType(a)) {
                        (b as FrontendNumberType).Bind(a);
                        return true;
                    } else {
                        return false;
                    }
                }
            }
            if (a.Equals(b)) {
                return true;
            } else {
                return false;
            }
        }
    }

    public class FrontendNumberType : FrontendType
    {
        public FrontendType boundType;
        public List<FrontendNumberType> others;
        public bool floatType;

        public FrontendNumberType(bool floatLiteral)
        {
            name = "$$__number__$$";
            others = new List<FrontendNumberType>();
            boundType = null;
            this.floatType = floatLiteral;
        }
        public void Bind(FrontendType type)
        {
            HashSet<FrontendNumberType> visited = new HashSet<FrontendNumberType>();
            bind(type, visited);
        }

        void bind(FrontendType type, HashSet<FrontendNumberType> visited)
        {
            visited.Add(this);
            boundType = type;
            foreach (var other in others) {
                if (!visited.Contains(other)) {
                    other.bind(type, visited);
                }
            }
        }
        public FrontendType Default()
        {
            if (floatType) {
                return FrontendType.f32;
            } else {
                return FrontendType.i32;
            }
        }
        public override string ToString()
        {
            if (boundType != null) {
                return boundType.ToString();
            }
            if (floatType) {
                return "float literal";
            } else {
                return "integer literal";
            }

        }
    }

    public class FrontendEnumType: FrontendType
    {
        public string enumName;
        public FrontendType integerType;
        public class Entry {
            public string name;
            public ulong value;
        }
        List<Entry> entries;
        public FrontendEnumType(string name) 
        {
            this.enumName = name;
            this.name = name;
            entries = new List<Entry>();
        }
        public void AddEntry(string name) 
        {
            entries.Add(new Entry { name=name, value=(ulong)entries.Count });
        }
    }


    public class FrontendSliceType : FrontendStructType
    {
        public FrontendType elementType;
        public FrontendSliceType(FrontendType elementType, string structName = "")
            : base(structName)
        {
            this.elementType = elementType;
            AddField("length", FrontendType.i32);
            AddField("data", new FrontendPointerType(elementType));
            if (string.IsNullOrEmpty(structName)) {
                name = "[" + elementType + "]";
            }
        }
    }

    public class FrontendArrayType : FrontendType
    {
        public FrontendType elementType;
        public List<int> dims;
        public int Length {
             get {
                 int result = 1;
                 foreach (var d in dims) {
                     result *= d;
                 }
                return result;
            }
        }
        public FrontendArrayType(FrontendType elementType, List<int> dim)
            : base("")
        {
            this.elementType = elementType;
            this.dims = new List<int>();
            this.dims.AddRange(dim);
            name = $"[({string.Join(", ", dim)}) x {elementType}]";
        }
    }

    public class FrontendStructType : FrontendType
    {
        public class Field
        {
            public string name;
            public FrontendType type;
            public override string ToString()
            {
                return type.ToString();
            }
        }
        public List<Field> fields = new List<Field>();
        public string structName;
        public bool packed;


        public FrontendStructType(string structName)
        {
            this.structName = structName;
            calcTypeName();
        }

        public void AddField(string name, FrontendType type)
        {
            fields.Add(new Field { name = name, type = type });
            calcTypeName();
        }

        public FrontendType GetField(string name)
        {
            var f = fields.Where(x => x.name == name).FirstOrDefault();
            return f != null ? f.type : null;
        }

        public int GetFieldIndex(string name)
        {
            int idx = 0;
            foreach (var f in fields) {
                if (f.name == name) {
                    return idx;
                }
                idx++;
            }
            throw new InvalidCodePath();
        }

        void calcTypeName()
        {
            if (structName != null) {
                name = structName;
            } else {
                name = "{" + string.Join(",", fields) + "}";
            }
        }
    }

    public class FrontendPointerType : FrontendType
    {
        public FrontendType elementType;
        public FrontendPointerType(FrontendType elementType)
        {
            this.elementType = elementType;
            name = elementType + "*";
        }
    }

    public class FrontendSumType : FrontendType {
        public List<FrontendType> types;
        public FrontendSumType(params FrontendType[] types) {
            this.types = types.ToList();
            name = $"{{{string.Join(" + ", this.types)}}}";
        }
        public FrontendSumType(List<FrontendType> types) {
            this.types = types;
            name = $"{{{string.Join(" + ", this.types)}}}";
        }
    }

    public class FrontendFunctionType : FrontendType
    {
        public class Param
        {
            public bool optional;
            public bool embed;
            public string name;
            public FrontendType type;
            public override string ToString()
            {
                return type.ToString();
            }
        }
        public List<Param> parameters = new List<Param>();
        public FrontendType returnType;
        public string funName;
        public bool specialFun;
        public bool inactiveConditional;

        public FrontendFunctionType(string funName)
        {
            if (funName != null) {
                this.funName = funName;
            } else {
                this.funName = "";
            }
            calcTypeName();
        }

        public void AddParam(string name, FrontendType type, bool optional = false, bool embed = false)
        {
            parameters.Add(new Param { name = name, type = type, optional = optional, embed = embed });
            calcTypeName();
        }

        public Param GetParam(string name)
        {
            var f = parameters.Where(x => x.name == name).FirstOrDefault();
            return f;
        }

        public int GetParamIndex(string name)
        {
            int idx = 0;
            foreach (var f in parameters) {
                if (f.name == name) {
                    return idx;
                }
                idx++;
            }
            throw new InvalidCodePath();
        }

        internal void calcTypeName()
        {
            name = $"{funName}({string.Join(",", parameters)}) => {returnType}";
        }
    }
}
