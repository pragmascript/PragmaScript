using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PragmaScript
{
    partial class AST
    {

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
            scope.AddType(FrontendType.int8, token);
            scope.AddType(FrontendType.bool_, token);
            scope.AddTypeAlias(FrontendType.string_, token, "string");
        }

        static void addBasicFunctions(Scope scope)
        {

            //var cat = new Scope.FunctionDefinition { name = "cat", returnType = FrontendType.void_ };
            //scope.AddFunction(cat);

            // TODO: avoid having to do this twice here and in the backend?
            var get_std_handle = new Scope.FunctionDefinition { external = true, name = "GetStdHandle", returnType = FrontendType.int64 };
            get_std_handle.AddParameter("nStdHandle", FrontendType.int32);
            scope.AddFunction(get_std_handle);

            var write_file = new Scope.FunctionDefinition { external = true, name = "WriteFile", returnType = FrontendType.bool_ };
            write_file.AddParameter("hFile", FrontendType.int64);
            write_file.AddParameter("lpBuffer", new FrontendPointerType(FrontendType.int8));
            write_file.AddParameter("nNumberOfBytesToWrite", FrontendType.int32);
            write_file.AddParameter("lpNumberOfBytesWritten", new FrontendPointerType(FrontendType.int8));
            write_file.AddParameter("lpOverlapped", new FrontendPointerType(FrontendType.int8));
            scope.AddFunction(write_file);

            var read_file = new Scope.FunctionDefinition { external = true, name = "ReadFile", returnType = FrontendType.bool_ };
            read_file.AddParameter("hFile", FrontendType.int64);
            read_file.AddParameter("lpBuffer", new FrontendPointerType(FrontendType.int8));
            read_file.AddParameter("nNumberOfBytesToRead", FrontendType.int32);
            read_file.AddParameter("lpNumberOfBytesRead", new FrontendPointerType(FrontendType.int8));
            read_file.AddParameter("lpOverlapped", new FrontendPointerType(FrontendType.int8));
            scope.AddFunction(read_file);

        }

        static void addBasicConstants(Scope scope, Token token)
        {
            // TODO make those ACTUAL constants
            var nullptr = scope.AddVar("nullptr", token);
            nullptr.type = new FrontendPointerType(FrontendType.int8);
        }

        public static void TypeCheck(Node node, Scope scope)
        {
            try
            {
                performTypeChecking(node, scope).Wait();
            }
            catch (System.AggregateException e)
            {
                throw e.InnerException;
            }
        }

        public static Scope MakeRootScope()
        {
            
            var rootScope = new Scope(null, null);
            addBasicTypes(rootScope, Token.Undefined);
            addBasicConstants(rootScope, Token.Undefined);
            addBasicFunctions(rootScope);


            //var main = new Scope.FunctionDefinition { name = "main", returnType = FrontendType.int32 };
            //rootScope.AddFunction(main);
            //rootScope.function = main;

            return rootScope;
        }

        public static Node Parse(Token[] tokens, Scope scope)
        {
            int pos = 0;
            var current = tokens[pos];

            Node root = null;
#if !DEBUG
            try
#endif
            {
                ParseState ps;
                ps.pos = 0;
                ps.tokens = tokens;
                // perform AST generation pass
                ps.SkipWhitespace();
                root = parseRoot(ref ps, scope);
             
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

            return root;
        }
    }
}
