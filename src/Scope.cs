using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PragmaScript {

    class Scope
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
                while (true) {
                    mod.Add(current);
                    Debug.Assert(current.scope.parent != null);
                    current = current.scope.parent.module;
                    if (current == null) {
                        break;
                    }
                }

                mod.Reverse();
                return string.Join(".", mod.Select(nns => nns.name));
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

        public class OverloadedFunctionDefinition: VariableDefinition
        {
            public List<AST.Node> nodes = new List<AST.Node>();
            public List<FrontendType> types = new List<FrontendType>();
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
        public static Dictionary<string, Module> modules = new Dictionary<string, Module>();

        public Scope parent;
        public AST.FunctionDefinition function;
        public Module module;
        public List<Module> importedModules = new List<Module>();

        public Dictionary<string, VariableDefinition> variables = new Dictionary<string, VariableDefinition>();
        public Dictionary<string, TypeDefinition> types = new Dictionary<string, TypeDefinition>();

        public AST.Node owner;
        public Scope(Scope parent, AST.FunctionDefinition function)
        {
            this.parent = parent;
            this.function = function;
        }

        public VariableDefinition GetVar(string name, Token from, bool recurse = true)
        {
            VariableDefinition result;

            if (variables.TryGetValue(name, out result)) {
                return result;
            }
            Module result_ns = null;
            foreach (var ns in importedModules) {

                if (ns.scope.variables.TryGetValue(name, out var vd)) {
                    if (result != null) {
                        var p0 = $"\"{result_ns.GetPath()}.{name}\"";
                        var p1 = $"\"{ns.GetPath()}.{name}\"";
                        throw new ParserError($"Access to variable is ambigious between {p0} and {p1}.", from);
                    } else {
                        result = vd;
                        result_ns = ns;
                    }
                }
            }
            if (result != null) {
                return result;
            }

            if (recurse && parent != null) {
                return parent.GetVar(name, from);
            } else {
                return null;
            }
        }

        public VariableDefinition AddVar(string name, AST.Node node, Token t, bool isConst = false)
        {
            Debug.Assert(node != null);
            if (variables.ContainsKey(name)) {
                throw new RedefinedVariable(name, t);
            }
            var v = new VariableDefinition();
            v.isGlobal = parent == null;
            v.isNamespace = this.module != null;
            v.name = name;
            v.node = node;
            v.isConstant = isConst;
            variables.Add(name, v);
            return v;
        }

        public VariableDefinition AddVar(string name, FrontendType @type, Token t, bool isConst = false, bool isGlobal = true)
        {
            Debug.Assert(@type != null);
            VariableDefinition v = new VariableDefinition();
            v.isGlobal = parent == null;
            v.isNamespace = this.module != null;
            v.name = name;
            v.type = @type;
            v.isConstant = isConst;
            if (variables.ContainsKey(name)) {
                throw new RedefinedVariable(name, t);
            }
            variables.Add(name, v);
            return v;
        }


        public TypeDefinition GetType(FullyQualifiedName typeName, Token from)
        {
            if (typeName.path.Count == 1) {
                var name = typeName.GetName();
                var result = getType(name, true, from);
                return result;
            } else {
                var mod_name = string.Join("::", typeName.path.Take(typeName.path.Count - 1));
                var mod = GetModule(mod_name);
                if (mod == null && module != null) {
                    var path = $"{module.GetPath()}::{mod_name}";
                    mod = GetModule(path);
                }
                if (mod != null) {
                    return mod.scope.getType(typeName.GetName(), false, from);
                } else {
                    return null;
                }
            }
        }

        TypeDefinition getType(string typeName, bool recursive, Token from)
        {
            TypeDefinition result;
            if (recursive) {
                result = this.getTypeRec(typeName, from);
            } else {
                types.TryGetValue(typeName, out result);

            }
            return result;
        }

        TypeDefinition getTypeRec(string typeName, Token from)
        {
            TypeDefinition result;

            if (types.TryGetValue(typeName, out result)) {
                return result;
            }

            if (result == null) {
                Module result_mod = null;
                foreach (var mod in importedModules) {
                    if (mod.scope.types.TryGetValue(typeName, out var td)) {
                        if (result != null) {
                            var p0 = $"\"{result_mod.GetPath()}.{typeName}\"";
                            var p1 = $"\"{mod.GetPath()}.{typeName}\"";
                            throw new ParserError($"Access to variable is ambigious between {p0} and {p1}.", from);
                        } else {
                            result = td;
                            result_mod = mod;
                        }
                    }
                }
                if (result != null) {
                    return result;
                }
            }
            if (parent != null) {
                return parent.getTypeRec(typeName, from);
            } else {
                return null;
            }
        }

        public void AddType(string name, AST.Node node, Token t)
        {
            Debug.Assert(node != null);
            if (types.ContainsKey(name)) {
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
            while (result.parent != null) {
                result = result.parent;
            }
            return result;
        }

        public Module AddModule(List<string> path, bool root = false)
        {
            Debug.Assert(path.Count > 0);

            string path_string = null;
            Scope root_scope;
            if (root) {
                root_scope = GetRootScope();
                path_string = "";
            } else {
                root_scope = this;
                if (module != null) {
                    path_string = module.GetPath();
                }
            }

            Scope parentScope = root_scope;
            Module lastModule = module;

            foreach (var p in path) {
                Debug.Assert(!string.IsNullOrWhiteSpace(p));
                if (string.IsNullOrWhiteSpace(path_string)) {
                    path_string = p;
                } else {
                    path_string = $"{path_string}::{p}";
                }
                if (!modules.ContainsKey(path_string)) {
                    var n = new Module(p, parentScope);
                    modules.Add(path_string, n);
                    parentScope = n.scope;
                    lastModule = n;
                } else {
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
            if (types.ContainsKey(alias)) {
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
            if (types.ContainsKey(t.ToString())) {
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
