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
            public bool isFunctionParameter;
            public int parameterIdx = -1;
            public string name;
            public FrontendType type;
        }

        public struct NamedParameter
        {
            public string name;
            public FrontendType type;
        }

        public class FunctionDefinition
        {
            public string name;
            public FrontendType returnType;
            public List<NamedParameter> parameters = new List<NamedParameter>();
            public void AddParameter(string name, FrontendType type)
            {
                parameters.Add(new NamedParameter { name = name, type = type });
            }
        }

        public FunctionDefinition function;
        public Scope parent;
        public Dictionary<string, VariableDefinition> variables = new Dictionary<string, VariableDefinition>();
        public Dictionary<string, FunctionDefinition> functions = new Dictionary<string, FunctionDefinition>();
        public Dictionary<string, FrontendType> types = new Dictionary<string, FrontendType>();

        public Scope(Scope parent, FunctionDefinition function)
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

        public void AddFunctionParameter(string name, FrontendType type, int idx)
        {
            VariableDefinition v = new VariableDefinition();
            v.name = name;
            v.type = type;
            v.isFunctionParameter = true;
            v.parameterIdx = idx;
            variables.Add(name, v);
        }

        public FunctionDefinition GetFunction(string name)
        {
            FunctionDefinition result;
            if (functions.TryGetValue(name, out result))
            {
                return result;
            }
            if (parent != null)
            {
                return parent.GetFunction(name);
            }
            else
            {
                return null;
            }
        }

        public void AddFunction(FunctionDefinition fun)
        {
            if (variables.ContainsKey(fun.name))
            {
                throw new RedefinedFunction(fun.name, Token.Undefined);
            }
            functions.Add(fun.name, fun);
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
