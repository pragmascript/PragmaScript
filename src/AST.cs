using System.Collections.Generic;
using System.Linq;

namespace PragmaScript {
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
            foreach (var tt in types) {
                if (token.type == tt) {
                    found = true;
                    break;
                }
            }
            if (!found) {
                string exp = "either ( " + string.Join(" | ", types.Select(tt => tt.ToString())) + " )";
                throw new ParserErrorExpected(exp, token.ToString(), token);
            }
        }


        static void addBasicTypes(Scope scope, Token token)
        {
            scope.AddType(FrontendType.f32, token);
            scope.AddType(FrontendType.f64, token);

            scope.AddType(FrontendType.i16, token);
            scope.AddType(FrontendType.i32, token);
            scope.AddType(FrontendType.i64, token);
            scope.AddType(FrontendType.i8, token);

            scope.AddType(FrontendType.mm, token);


            scope.AddType(FrontendType.bool_, token);
            scope.AddTypeAlias(FrontendType.string_, token, "string");
            scope.AddTypeAlias(FrontendType.ptr, token, "ptr");
            scope.AddType(FrontendType.void_, token);
        }

        
        static void addSpecialFunctions(Scope scope)
        {
            var file_pos = new FrontendFunctionType("__file_pos__");
            file_pos.returnType = FrontendType.string_;
            file_pos.specialFun = true;
            scope.AddVar("__file_pos__", file_pos, Token.Undefined, isConst: true);

            var len = new FrontendFunctionType("len");
            len.returnType = FrontendType.mm;
            len.specialFun = true;
            len.AddParam("x", new FrontendArrayType(null, new List<int>()));
            scope.AddVar("len", len, Token.Undefined, isConst: true);
            
            var emit = new FrontendFunctionType("__emit__");
            emit.returnType = FrontendType.void_;
            emit.specialFun = true;
            emit.AddParam("instr", FrontendType.string_);
            scope.AddVar("__emit__", emit, Token.Undefined, isConst: true);
        }

        public static Scope MakeRootScope()
        {
            var rootScope = new Scope(null, null);
            addBasicTypes(rootScope, Token.Undefined);
            addSpecialFunctions(rootScope);
            return rootScope;
        }
    }
}
