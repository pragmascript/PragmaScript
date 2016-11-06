using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PragmaScript
{

    class Scope
    {
        public class VariableDefinition
        {
            public bool isConstant = false;
            public bool isFunctionParameter;
            public int parameterIdx = -1;
            public string name;
            public AST.Node node;
            public FrontendType type;
        }

        public class TypeDefinition
        {
            public string name;
            public AST.Node node;
            public FrontendType type;
        }

        public Scope parent;
        public Dictionary<string, VariableDefinition> variables = new Dictionary<string, VariableDefinition>();
        public Dictionary<string, TypeDefinition> types = new Dictionary<string, TypeDefinition>();

        public Scope(Scope parent)
        {
            this.parent = parent;
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

        public VariableDefinition AddVar(string name, AST.Node node, Token t, bool isConst = false)
        {
            Debug.Assert(node != null);
            VariableDefinition v = new VariableDefinition();
            v.name = name;
            v.node = node;
            v.isConstant = isConst;
            if (variables.ContainsKey(name))
            {
                throw new RedefinedVariable(name, t);
            }
            variables.Add(name, v);
            return v;
        }

        public VariableDefinition AddVar(string name, FrontendType @type, Token t, bool isConst = false)
        {
            Debug.Assert(@type != null);
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

        public void AddFunctionParameter(string name, AST.FunctionDefinition node, int idx)
        {
            VariableDefinition v = new VariableDefinition();
            v.name = name;
            v.node = node;
            v.isFunctionParameter = true;
            v.parameterIdx = idx;
            variables.Add(name, v);
        }

        public TypeDefinition GetType(string typeName)
        {
            var result = this.getTypeRec(typeName);
            return result;
        }

        TypeDefinition getTypeRec(string typeName)
        {
            TypeDefinition result;

            if (types.TryGetValue(typeName, out result))
            {
                return result;
            }

            if (parent != null)
            {
                return parent.getTypeRec(typeName);
            }
            else
            {
                return null;
            }
        }

        public void AddType(string name, AST.Node node, Token t)
        {
            Debug.Assert(node != null);
            if (types.ContainsKey(name))
            {
                throw new RedefinedType(name, t);
            }
            var td = new TypeDefinition();
            td.name = name;
            td.node = node;
            types.Add(td.name, td);
        }

        public void AddTypeAlias(FrontendType t, Token token, string alias)
        {
            if (types.ContainsKey(alias))
            {
                throw new RedefinedType(alias, token);
            }
            var td = new TypeDefinition();
            td.name = alias;
            td.type = t;
            td.node = null;

            types.Add(td.name, td);
        }

        public void AddType(FrontendType t, Token token)
        {
            if (types.ContainsKey(t.ToString()))
            {
                throw new RedefinedType(t.ToString(), token);
            }
            var td = new TypeDefinition();
            td.name = t.ToString();
            td.type = t;
            td.node = null;
            types.Add(td.name, td);
        }

    }
}
