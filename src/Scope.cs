using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PragmaScript
{

    public class Scope
    {
        public class Module
        {
            public string name;
            public Scope scope;

            public Module(string name, Scope parentScope)
            {
                this.name = name;
                Debug.Assert(parentScope.function == null);
                scope = new Scope(parentScope, null);
                scope.module = this;
            }

            public string GetPath()
            {
                List<Module> mod = new List<Module>();

                var current = this;
                while (true)
                {
                    mod.Add(current);
                    Debug.Assert(current.scope.parent != null);
                    current = current.scope.parent.module;
                    if (current == null)
                    {
                        break;
                    }
                }

                mod.Reverse();
                return string.Join("::", mod.Select(nns => nns.name));
            }

            public override string ToString()
            {
                return GetPath();
            }
        }



        public class VariableDefinition
        {
            public bool isGlobal = false;
            public bool isNamespace = false;
            public bool isConstant = false;
            public bool isFunctionParameter;
            public int parameterIdx = -1;
            public bool isEmbedded;
            public int embeddingIdx = -1;
            public string name;
            public AST.Node node;
            public FrontendType type;
        }

        public class OverloadedVariableDefinition
        {
            public bool allowOverloading;
            public List<VariableDefinition> variables;
            public OverloadedVariableDefinition()
            {
                variables = new List<VariableDefinition>();
            }
            public OverloadedVariableDefinition(VariableDefinition vd)
            {
                variables = new List<VariableDefinition>(1);
                variables.Add(vd);
            }
            public bool IsOverloaded { get { return variables.Count > 1; } }
            public VariableDefinition First { get { return variables[0]; } }
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
                return $"{string.Join("::", path)}";
            }
        }


        public Dictionary<string, Module> modules;
        public Scope parent;
        public AST.FunctionDefinition function;
        public Module module;
        public List<Module> importedModules = new List<Module>();

        public Dictionary<string, OverloadedVariableDefinition> variables = new Dictionary<string, OverloadedVariableDefinition>();
        public Dictionary<string, TypeDefinition> types = new Dictionary<string, TypeDefinition>();

        public AST.Node owner;
        public List<Scope> childs = new List<Scope>();

        public List<(Token start, Token end)> tokenRanges = new List<(Token start, Token end)>();

        public Scope(Scope parent, AST.FunctionDefinition function)
        {
            if (parent == null)
            {
                modules = new Dictionary<string, Module>();
            }
            else
            {
                modules = parent.modules;
                parent.childs.Add(this);
                Debug.Assert(parent.modules != null);
            }
            this.parent = parent;
            this.function = function;
        }

        public void AddTokenRangeForModule(Token start, Token end)
        {
            Debug.Assert(module != null);
            tokenRanges.Add((start, end));
            if (parent != null && parent.module != null)
            {
                parent.AddTokenRangeForModule(start, end);
            }
        }


        public OverloadedVariableDefinition GetVar(string name, Token from, bool recurse = true)
        {
            OverloadedVariableDefinition result = new OverloadedVariableDefinition();

            if (variables.TryGetValue(name, out var ovd))
            {
                result.variables.AddRange(ovd.variables);
                result.allowOverloading = ovd.allowOverloading;
            }

            foreach (var mod in importedModules)
            {
                if (mod.scope.variables.TryGetValue(name, out var mvd))
                {
                    if (result.variables.Count > 0 && !result.allowOverloading)
                    {
                        Debug.Assert(result.variables.Count == 1);
                        var p0 = $"\"{result.First.node.scope.module.GetPath()}::{name}\"";
                        var p1 = $"\"{mod.GetPath()}::{name}\"";
                        throw new CompilerError($"Access to variable is ambigious between {p0} and {p1}.", from);
                    }
                    else
                    {
                        if (result.variables.Count > 0)
                        {
                            Debug.Assert(result.allowOverloading);
                            Debug.Assert(mvd.allowOverloading);
                        }
                        result.variables.AddRange(mvd.variables);
                    }
                }
            }
            if (result.variables.Count > 0)
            {
                return result;
            }

            if (recurse && parent != null)
            {
                return parent.GetVar(name, from);
            }
            else
            {
                return null;
            }
        }

        public VariableDefinition AddVar(string name, AST.Node node, Token t, bool isConst = false, bool allowOverloading = false)
        {
            Debug.Assert(node != null);

            bool variablePresent = variables.ContainsKey(name);
            if (!allowOverloading && variablePresent)
            {
                throw new RedefinedVariable(name, t);
            }
            var v = new VariableDefinition();
            v.isGlobal = parent == null;
            v.isNamespace = this.module != null;
            v.name = name;
            v.node = node;
            v.isConstant = isConst;
            OverloadedVariableDefinition ov;
            if (variablePresent)
            {
                ov = variables[name];
                Debug.Assert(ov.allowOverloading);
                ov.variables.Add(v);
            }
            else
            {
                ov = new OverloadedVariableDefinition(v);
                variables.Add(name, ov);
                ov.allowOverloading = allowOverloading;
            }
            return v;
        }

        public VariableDefinition AddVar(string name, FrontendType @type, Token t, bool isConst = false, bool allowOverloading = false)
        {
            Debug.Assert(@type != null);

            bool variablePresent = variables.ContainsKey(name);
            if (!allowOverloading && variables.ContainsKey(name))
            {
                throw new RedefinedVariable(name, t);
            }

            VariableDefinition v = new VariableDefinition();
            v.isGlobal = parent == null;
            v.isNamespace = this.module != null;
            v.name = name;
            v.type = @type;
            v.isConstant = isConst;

            OverloadedVariableDefinition ov;
            if (variablePresent)
            {
                ov = variables[name];
                Debug.Assert(ov.allowOverloading);
                ov.variables.Add(v);
            }
            else
            {
                ov = new OverloadedVariableDefinition(v);
                variables.Add(name, ov);
                ov.allowOverloading = allowOverloading;
            }
            return v;
        }


        public TypeDefinition GetType(FullyQualifiedName typeName, Token from)
        {
            if (typeName.path.Count == 1)
            {
                var name = typeName.GetName();
                var result = getType(name, true, from);
                return result;
            }
            else
            {
                var mod_name = string.Join("::", typeName.path.Take(typeName.path.Count - 1));
                var mod = GetModule(mod_name);
                if (mod == null && module != null)
                {
                    var path = $"{module.GetPath()}::{mod_name}";
                    mod = GetModule(path);
                }
                if (mod != null)
                {
                    return mod.scope.getType(typeName.GetName(), false, from);
                }
                else
                {
                    return null;
                }
            }
        }

        public string GetFullyQualifiedName(string name)
        {
            string result = name;
            if (module != null)
            {
                result = $"{module.GetPath()}::{name}";
            }
            return result;
        }


        TypeDefinition getType(string typeName, bool recursive, Token from)
        {
            TypeDefinition result;
            if (recursive)
            {
                result = this.getTypeRec(typeName, from);
            }
            else
            {
                types.TryGetValue(typeName, out result);

            }
            return result;
        }

        TypeDefinition getTypeRec(string typeName, Token from)
        {
            TypeDefinition result;

            if (types.TryGetValue(typeName, out result))
            {
                return result;
            }

            if (result == null)
            {
                Module result_mod = null;
                foreach (var mod in importedModules)
                {
                    if (mod.scope.types.TryGetValue(typeName, out var td))
                    {
                        if (result != null)
                        {
                            var p0 = $"\"{result_mod.GetPath()}.{typeName}\"";
                            var p1 = $"\"{mod.GetPath()}.{typeName}\"";
                            throw new CompilerError($"Access to type is ambigious between {p0} and {p1}.", from);
                        }
                        else
                        {
                            result = td;
                            result_mod = mod;
                        }
                    }
                }
                if (result != null)
                {
                    return result;
                }
            }
            if (parent != null)
            {
                return parent.getTypeRec(typeName, from);
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

        public Scope GetRootScope()
        {
            Scope result = this;
            while (result.parent != null)
            {
                result = result.parent;
            }
            return result;
        }

        public Module AddModule(List<string> path, bool root = false)
        {
            Debug.Assert(path.Count > 0);

            string path_string = null;
            Scope root_scope;
            if (root)
            {
                root_scope = GetRootScope();
                path_string = "";
            }
            else
            {
                root_scope = this;
                if (module != null)
                {
                    path_string = module.GetPath();
                }
            }

            Scope parentScope = root_scope;
            Module lastModule = module;

            foreach (var p in path)
            {
                Debug.Assert(!string.IsNullOrWhiteSpace(p));
                if (string.IsNullOrWhiteSpace(path_string))
                {
                    path_string = p;
                }
                else
                {
                    path_string = $"{path_string}::{p}";
                }
                if (!modules.ContainsKey(path_string))
                {
                    var n = new Module(p, parentScope);
                    modules.Add(path_string, n);
                    parentScope = n.scope;
                    lastModule = n;
                }
                else
                {
                    lastModule = modules[path_string];
                    parentScope = lastModule.scope;
                }
            }
            return lastModule;
        }

        public Module GetModule(List<string> path)
        {
            modules.TryGetValue(string.Join("::", path), out Module result);
            return result;
        }

        public Module GetModule(string path)
        {
            modules.TryGetValue(string.Join("::", path), out Module result);
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
