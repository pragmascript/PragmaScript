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

            public string GetPath()
            {
                List<Namespace> ns = new List<Namespace>();

                var current = this;
                while (true)
                {
                    ns.Add(current);
                    Debug.Assert(current.scope.parent != null);
                    current = current.scope.parent.namesp;
                    if (current == null)
                    {
                        break;
                    }
                }

                ns.Reverse();
                return string.Join(".", ns.Select(nns => nns.name));
            }

            public override string ToString()
            {
                return GetPath();
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


        public TypeDefinition GetType(FullyQualifiedName typeName)
        {
            if (typeName.path.Count == 1)
            {
                var result = GetType(typeName.GetName());
                return result;
            }
            else
            {
                var ns_name = string.Join(".", typeName.path.Take(typeName.path.Count - 1));
                var ns = GetNamespace(ns_name);
                if (ns == null && namesp != null)
                {
                    var path = $"{namesp.GetPath()}.{ns_name}";
                    ns = GetNamespace(path);
                }
                if (ns != null)
                {
                    return ns.scope.GetType(typeName.GetName());
                }
                else
                {

                    return null;
                }

            }
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

        public Namespace AddNamespace(List<string> path)
        {
            Debug.Assert(path.Count > 0);

            var path_string = "";
            if (namesp != null)
            {
                path_string = namesp.GetPath();
            }
            Scope parentScope = this;
            Namespace lastNamespace = namesp;

            foreach (var p in path)
            {
                Debug.Assert(!string.IsNullOrWhiteSpace(p));
                if (string.IsNullOrWhiteSpace(path_string))
                {
                    path_string = p;
                }
                else
                {
                    path_string = $"{path_string}.{p}";
                }
                if (!namespaces.ContainsKey(path_string))
                {
                    var n = new Namespace(p, parentScope);
                    namespaces.Add(path_string, n);
                    parentScope = n.scope;
                    lastNamespace = n;
                }
                else
                {
                    lastNamespace = namespaces[path_string];
                    parentScope = lastNamespace.scope;
                }
            }
            return lastNamespace;
        }

        public Namespace GetNamespace(List<string> path)
        {
            namespaces.TryGetValue(string.Join(".", path), out Namespace result);
            return result;
        }

        public Namespace GetNamespace(string path)
        {
            namespaces.TryGetValue(string.Join(".", path), out Namespace result);
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
