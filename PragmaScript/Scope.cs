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
        public class Namespace
        {
            public string name;
            public Scope scope;

            public Namespace(string name, Scope parentScope)
            {
                this.name = name;
                Debug.Assert(parentScope.function == null);
                scope = new Scope(parentScope, null);
                scope.namesp = this;
            }
        }

        public class VariableDefinition
        {
            public bool isGlobal = false;
            public bool isConstant = false;
            public bool isFunctionParameter;
            public int parameterIdx = -1;
            public bool isEmbedded;
            public int embeddingIdx = -1;
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

        public class FullyQualifiedName
        {

            internal List<string> path;
            // internal string name;

            public FullyQualifiedName()
            {
                path = new List<string>();
            }

            public string GetName()
            {
                return path.LastOrDefault();
            }

            public override string ToString()
            {
                // return $"{string.Join(".", path)}.{name}";
                return $"{string.Join(".", path)}";
            }
        }


        // TODO: make this non static
        public static Dictionary<string, Namespace> namespaces = new Dictionary<string, Namespace>();

        public Scope parent;
        public AST.FunctionDefinition function;
        public Namespace namesp;
        public Dictionary<string, VariableDefinition> variables = new Dictionary<string, VariableDefinition>();
        public Dictionary<string, TypeDefinition> types = new Dictionary<string, TypeDefinition>();

        public Scope(Scope parent, AST.FunctionDefinition function)
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

        public VariableDefinition AddVar(string name, AST.Node node, Token t, bool isConst = false)
        {
            Debug.Assert(node != null);
            VariableDefinition v = new VariableDefinition();
            v.isGlobal = parent == null;
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

        public VariableDefinition AddVar(string name, FrontendType @type, Token t, bool isConst = false, bool isGlobal = true)
        {
            Debug.Assert(@type != null);
            VariableDefinition v = new VariableDefinition();
            v.isGlobal = parent == null;
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


        // TODO: implement
        public TypeDefinition GetType(FullyQualifiedName typeName)
        {
            var result = GetType(typeName.GetName());
            return result;
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

        public Namespace AddNamespace(string name)
        {
            if (namespaces.ContainsKey(name))
            {
                return namespaces[name];
            }
            else
            {

            }
            var result = new Namespace(name, this);
            namespaces.Add(name, result);
            return result;
        }

        public Namespace GetNamespace(string name)
        {
            namespaces.TryGetValue(name, out Namespace result);
            return result;
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
