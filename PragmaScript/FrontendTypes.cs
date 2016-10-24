using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PragmaScript
{
    public class FrontendType
    {
        public static readonly FrontendType void_ = new FrontendType("void");
        public static readonly FrontendType i8 = new FrontendType("i8");
        public static readonly FrontendType i16 = new FrontendType("i16");
        public static readonly FrontendType i32 = new FrontendType("i32");
        public static readonly FrontendType i64 = new FrontendType("i64");
        public static readonly FrontendType u8 = new FrontendType("u8");
        public static readonly FrontendType u16 = new FrontendType("u16");
        public static readonly FrontendType u32 = new FrontendType("u32");
        public static readonly FrontendType u64 = new FrontendType("u64");
        
        public static readonly FrontendType f32 = new FrontendType("f32");
        public static readonly FrontendType f64 = new FrontendType("f64");
        public static readonly FrontendType bool_ = new FrontendType("bool");
        public static readonly FrontendType umm = new FrontendType("umm");

        public static readonly FrontendArrayType string_ = new FrontendArrayType(i8);


        public string name;

        protected FrontendType()
        {
        }

        public FrontendType(string name)
        {
            this.name = name;
        }
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
        public override string ToString()
        {
            return name;
        }
        public override bool Equals(object obj)
        {
            return ToString() == obj.ToString();
        }

        // TODO: remove
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

    }

    public class FrontendArrayType : FrontendStructType
    {
        public FrontendType elementType;
        public FrontendArrayType(FrontendType elementType)
        {
            this.elementType = elementType;
            name = "[" + elementType + "]";
            AddField("length", FrontendType.i32);
            AddField("data", new FrontendPointerType(elementType));
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

        public void AddField(string name, FrontendType type)
        {
            fields.Add(new Field { name = name, type = type });
        }

        public FrontendType GetField(string name)
        {
            var f = fields.Where(x => x.name == name).FirstOrDefault();
            return f != null ? f.type : null;
        }

        public int GetFieldIndex(string name)
        {
            int idx = 0;
            foreach (var f in fields)
            {
                if (f.name == name)
                {
                    return idx;
                }
                idx++;
            }
            throw new InvalidCodePath();
        }

        void calcTypeName()
        {
            name = "{" + string.Join(",", fields) + "}";
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

    public class FrontendFunctionType: FrontendType
    {
        public class Param
        {
            public string name;
            public FrontendType type;
            public override string ToString()
            {
                return type.ToString();
            }
        }
        public List<Param> parameters = new List<Param>();
        public FrontendType returnType;

        public void AddParam(string name, FrontendType type)
        {
            parameters.Add(new Param { name = name, type = type });
        }

        public FrontendType GetParam(string name)
        {
            var f = parameters.Where(x => x.name == name).FirstOrDefault();
            return f != null ? f.type : null;
        }

        public int GetParamIndex(string name)
        {
            int idx = 0;
            foreach (var f in parameters)
            {
                if (f.name == name)
                {
                    return idx;
                }
                idx++;
            }
            throw new InvalidCodePath();
        }

        void calcTypeName()
        {
            name = $"({string.Join(",", parameters)}) => {returnType}";
        }
    }

}
