using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PragmaScript
{
    partial class AST
    {
        public abstract class Node
        {
            public Token token;

            public Node(Token t)
            {
                token = t;
            }

            public virtual IEnumerable<Node> GetChilds()
            {
                yield break;
            }

            public abstract Task<FrontendType> CheckType(Scope scope);
        }

        public class AnnotatedNode : Node
        {
            Node node;
            public string annotation;
            public AnnotatedNode(Node n, string annotation)
                : base(n.token)
            {
                node = n;
                this.annotation = annotation;
            }

            public override IEnumerable<Node> GetChilds()
            {
                foreach (var n in node.GetChilds())
                {
                    yield return n;
                }
            }

            public override async Task<FrontendType> CheckType(Scope scope)
            {
                return await node.CheckType(scope);
            }

            public override string ToString()
            {
                return node.ToString();
            }
        }

        public class FrontendType
        {
            public static readonly FrontendType void_ = new FrontendType("void");
            public static readonly FrontendType int32 = new FrontendType("int32");
            public static readonly FrontendType int64 = new FrontendType("int64");
            public static readonly FrontendType int8 = new FrontendType("int8");
            public static readonly FrontendType float32 = new FrontendType("float32");
            public static readonly FrontendType bool_ = new FrontendType("bool");
            public static readonly FrontendArrayType string_ = new FrontendArrayType(int8);
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
                AddField("length", FrontendType.int32);
                AddField("ptr", new FrontendPointerType(elementType));
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

        static void expectTokenType(Token t, Token.TokenType type)
        {
            if (t.type != type)
                throw new ParserErrorExpected(type.ToString(), t.type.ToString(), t);
        }

        static void expectTokenType(Token token, params Token.TokenType[] types)
        {
            var found = false;
            foreach (var tt in types)
            {
                if (token.type == tt)
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                string exp = "either ( " + string.Join(" | ", types.Select(tt => tt.ToString())) + " )";
                throw new ParserErrorExpected(exp, token.ToString(), token);
            }
        }

        static int skipWhitespace(IList<Token> tokens, int pos, bool requireOneWS = false)
        {
            bool foundWS = false;

            while (pos < tokens.Count && (tokens[pos].type == Token.TokenType.WhiteSpace || tokens[pos].type == Token.TokenType.Comment))
            {
                foundWS = true;
                pos++;
                if (pos >= tokens.Count)
                {
                    break;
                }
            }

            if (requireOneWS && !foundWS)
            {
                throw new ParserError("Expected Whitespace", tokens[pos]);
            }
            return pos;
        }

        static Token peekToken(IList<Token> tokens, int pos, bool tokenMustExist = false, bool skipWS = true)
        {
            pos++;
            if (skipWS)
            {
                pos = skipWhitespace(tokens, pos);
            }

            if (pos >= tokens.Count)
            {
                if (tokenMustExist)
                {
                    throw new ParserError("Missing next token", tokens[pos - 1]);
                }
                else
                {
                    return null;
                }
            }
            return tokens[pos];
        }

        static Token peekTokenUpdatePos(IList<Token> tokens, ref int pos, bool tokenMustExist = false, bool skipWS = true)
        {
            pos++;
            if (skipWS)
            {
                pos = skipWhitespace(tokens, pos);
            }

            if (pos >= tokens.Count)
            {
                if (tokenMustExist)
                {
                    throw new ParserError("Missing next token", tokens[pos - 1]);
                }
                else
                {
                    return null;
                }
            }
            return tokens[pos];
        }

        static Token expectCurrent(IList<Token> tokens, int pos, Token.TokenType tt)
        {
            var t = tokens[pos];
            expectTokenType(t, tt);
            return t;
        }

        static Token expectNext(IList<Token> tokens, ref int pos, Token.TokenType tt)
        {
            var t = nextToken(tokens, ref pos);
            expectTokenType(t, tt);
            return t;
        }

        static Token nextToken(IList<Token> tokens, ref int pos, bool skipWS = true)
        {
            pos++;
            if (skipWS)
            {
                pos = skipWhitespace(tokens, pos);
            }

            if (pos >= tokens.Count)
            {
                throw new ParserError("Missing next token", tokens[pos]);
            }
            else
            {
                return tokens[pos];
            }
        }

        static async Task<FrontendType> performTypeChecking(Node main, Scope root)
        {
            try
            {
                var result = await main.CheckType(root);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        static void addBasicTypes(Scope scope, Token token)
        {
            scope.AddType(FrontendType.float32, token);
            scope.AddType(FrontendType.int32, token);
            scope.AddType(FrontendType.bool_, token);
            scope.AddTypeAlias(FrontendType.string_, token, "string");
        }


        
        static void addBasicFunctions(Scope scope)
        {
            var print_i32 = new Scope.FunctionDefinition { name = "print_i32", returnType = FrontendType.void_ };
            print_i32.AddParameter("x", FrontendType.int32);
            scope.AddFunction(print_i32);

            var print_i8 = new Scope.FunctionDefinition { name = "print_i8", returnType = FrontendType.void_ };
            print_i8.AddParameter("x", FrontendType.int8);
            scope.AddFunction(print_i8);

            var print_f32 = new Scope.FunctionDefinition { name = "print_f32", returnType = FrontendType.void_ };
            print_f32.AddParameter("x", FrontendType.float32);
            scope.AddFunction(print_f32);

            var print = new Scope.FunctionDefinition { name = "print", returnType = FrontendType.void_ };
            print.AddParameter("length", FrontendType.int32);
            print.AddParameter("ptr", new FrontendPointerType(FrontendType.int8));
            scope.AddFunction(print);

            var read = new Scope.FunctionDefinition { name = "read", returnType = FrontendType.string_ };
            scope.AddFunction(read);

            var cat = new Scope.FunctionDefinition { name = "cat", returnType = FrontendType.void_ };
            scope.AddFunction(cat);

            // TODO: avoid having to do this twice here and in the backend?
            var get_std_handle = new Scope.FunctionDefinition { name = "GetStdHandle", returnType = FrontendType.int64 };
            get_std_handle.AddParameter("nStdHandle", FrontendType.int32);
            scope.AddFunction(get_std_handle);

            var write_file = new Scope.FunctionDefinition { name = "WriteFile", returnType = FrontendType.bool_ };
            write_file.AddParameter("hFile", FrontendType.int64);
            write_file.AddParameter("lpBuffer", new FrontendPointerType(FrontendType.int8));
            write_file.AddParameter("nNumberOfBytesToWrite", FrontendType.int32);
            write_file.AddParameter("lpNumberOfBytesWritten", new FrontendPointerType(FrontendType.int8));
            write_file.AddParameter("lpOverlapped", new FrontendPointerType(FrontendType.int8));
            scope.AddFunction(write_file);

        }

        
        static void addBasicConstants(Scope scope, Token token)
        {
            // TODO make those ACTUAL constants
            var nullptr = scope.AddVar("nullptr", token);
            nullptr.type = new FrontendPointerType(FrontendType.int8);
        }


        public static Node Parse(IList<Token> tokens)
        {
            int pos = 0;
            var current = tokens[pos];

            Node block = null;
#if !DEBUG
            try
#endif
            {
                var main = new Scope.FunctionDefinition { name = "main", returnType = FrontendType.int32 };
                var rootScope = new Scope(null, main);
                addBasicTypes(rootScope, current);
                addBasicConstants(rootScope, current);
                addBasicFunctions(rootScope);
                

                rootScope.AddFunction(main);
                rootScope.function = main;

                // perform AST generation pass
                pos = skipWhitespace(tokens, pos);
                block = parseMainBlock(tokens, ref pos, rootScope);

                // perform type checking pass
                try
                {
                    performTypeChecking(block, rootScope).Wait();
                }
                catch (System.AggregateException e)
                {
                    throw e.InnerException;
                }
            }
#if !DEBUG
            catch (ParserError error)
            {
                Console.WriteLine();
                Console.Error.WriteLine(error.Message);
                Console.WriteLine();
                return null;
            }
#endif

            return block;
        }
    }
}
