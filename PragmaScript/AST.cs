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

#if false
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

#endif

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

            var print = new Scope.FunctionDefinition { name = "print_string", returnType = FrontendType.void_ };
            print.AddParameter("length", FrontendType.int32);
            print.AddParameter("str", new FrontendPointerType(FrontendType.int8));
            // print.AddParameter("str", FrontendType.string_);
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


        public static Node Parse(Token[] tokens)
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

                ParseState ps;
                ps.pos = 0;
                ps.tokens = tokens;

                // perform AST generation pass
                ps.SkipWhitespace();
                block = parseMainBlock(ref ps, rootScope);

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
