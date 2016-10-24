using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PragmaScript
{

    public class Scope
    {
        public class VariableDefinition
        {
            public bool isConstant = false;
            public bool isFunctionParameter;
            public int parameterIdx = -1;
            public string name;
            public FrontendType type;
        }

        public Scope parent;
        public FrontendFunctionType function;
        public Dictionary<string, VariableDefinition> variables = new Dictionary<string, VariableDefinition>();
        public Dictionary<string, FrontendType> types = new Dictionary<string, FrontendType>();

        public Scope(Scope parent, FrontendFunctionType function)
        {
            this.parent = parent;
            this.function = function;
        }

        public VariableDefinition GetVar(string name)
        {
            VariableDefinition result;

            if (variables.TryGetValue(name, out result))
            {
                return result;
            }

            if (parent != null)
            {
                return parent.GetVar(name);
            }
            else
            {
                return null;
            }
        }

        public VariableDefinition AddVar(string name, Token t)
        {
            VariableDefinition v = new VariableDefinition();
            v.name = name;
            if (variables.ContainsKey(name))
            {
                throw new RedefinedVariable(name, t);
            }
            variables.Add(name, v);
            return v;
        }

        public VariableDefinition AddVar(string name, FrontendType @type, Token t, bool isConst = false)
        {
            VariableDefinition v = new VariableDefinition();
            v.name = name;
            v.type = @type;
            v.isConstant = isConst;
            if (variables.ContainsKey(name))
            {
                throw new RedefinedVariable(name, t);
            }
            variables.Add(name, v);
            return v;
        }


        public void AddFunctionParameter(string name, FrontendType type, int idx)
        {
            VariableDefinition v = new VariableDefinition();
            v.name = name;
            v.type = type;
            v.isFunctionParameter = true;
            v.parameterIdx = idx;
            variables.Add(name, v);
        }

        public FrontendType GetType(string typeName)
        {
            FrontendType result;

            if (types.TryGetValue(typeName, out result))
            {
                return result;
            }

            if (parent != null)
            {
                return parent.GetType(typeName);
            }
            else
            {
                return null;
            }
        }

        public FrontendType GetArrayType(string elementType)
        {
            var et = GetType(elementType);
            return new FrontendArrayType(et);
        }

        public void AddType(string name, FrontendType typ, Token t)
        {
            if (types.ContainsKey(name))
            {
                throw new RedefinedType(name, t);
            }
            types.Add(name, typ);
        }

        public void AddTypeAlias(FrontendType t, Token token, string alias)
        {
            if (types.ContainsKey(alias))
            {
                throw new RedefinedType(alias, token);
            }
            types.Add(alias, t);
        }

        public void AddType(FrontendType t, Token token)
        {
            if (types.ContainsKey(t.ToString()))
            {
                throw new RedefinedType(t.ToString(), token);
            }
            types.Add(t.ToString(), t);
        }

    }
}
