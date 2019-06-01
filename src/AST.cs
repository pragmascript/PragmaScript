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
            scope.AddTypeAlias(FrontendType.v4, token, "v4");
            scope.AddTypeAlias(FrontendType.v8, token, "v8");
            scope.AddTypeAlias(FrontendType.v4i, token, "v4i");
            scope.AddTypeAlias(FrontendType.v8i, token, "v8i");
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
            
            {
                var name = "atomic_compare_and_swap";
                var sf = new FrontendFunctionType(name);
                sf.returnType = FrontendType.i32;
                sf.specialFun = true;
                sf.AddParam("dest", new FrontendPointerType(sf.returnType));
                sf.AddParam("target", sf.returnType);
                sf.AddParam("comperand", sf.returnType);
                scope.AddVar(name, sf, Token.Undefined, isConst: true, allowOverloading: true);
            }
            {
                var name = "atomic_compare_and_swap";
                var sf = new FrontendFunctionType(name);
                sf.returnType = FrontendType.i64;
                sf.specialFun = true;
                sf.AddParam("dest", new FrontendPointerType(sf.returnType));
                sf.AddParam("target", sf.returnType);
                sf.AddParam("comperand", sf.returnType);
                scope.AddVar(name, sf, Token.Undefined, isConst: true, allowOverloading: true);
            }
            {
                var name = "atomic_add";
                var sf = new FrontendFunctionType(name);
                sf.returnType = FrontendType.i32;
                sf.specialFun = true;
                sf.AddParam("dest", new FrontendPointerType(sf.returnType));
                sf.AddParam("value", sf.returnType);
                scope.AddVar(name, sf, Token.Undefined, isConst: true, allowOverloading: true);
            }
            {
                var name = "atomic_add";
                var sf = new FrontendFunctionType(name);
                sf.returnType = FrontendType.i64;
                sf.specialFun = true;
                sf.AddParam("dest", new FrontendPointerType(sf.returnType));
                sf.AddParam("value", sf.returnType);
                scope.AddVar(name, sf, Token.Undefined, isConst: true, allowOverloading: true);
            }
            {
                var name = "atomic_sub";
                var sf = new FrontendFunctionType(name);
                sf.returnType = FrontendType.i32;
                sf.specialFun = true;
                sf.AddParam("dest", new FrontendPointerType(sf.returnType));
                sf.AddParam("value", sf.returnType);
                scope.AddVar(name, sf, Token.Undefined, isConst: true, allowOverloading: true);
            }
            {
                var name = "atomic_sub";
                var sf = new FrontendFunctionType(name);
                sf.returnType = FrontendType.i64;
                sf.specialFun = true;
                sf.AddParam("dest", new FrontendPointerType(sf.returnType));
                sf.AddParam("value", sf.returnType);
                scope.AddVar(name, sf, Token.Undefined, isConst: true, allowOverloading: true);
            }
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
