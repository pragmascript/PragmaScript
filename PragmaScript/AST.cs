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

        static void addBasicFunctions(Scope scope)
        {

            //var write_file = new FrontendFunctionType();
            //write_file.returnType = FrontendType.bool_;
            //write_file.AddParam("hFile", FrontendType.i64);
            //write_file.AddParam("lpBuffer", new FrontendPointerType(FrontendType.i8));
            //write_file.AddParam("nNumberOfBytesToWrite", FrontendType.i32);
            //write_file.AddParam("lpNumberOfBytesWritten", new FrontendPointerType(FrontendType.i8));
            //write_file.AddParam("lpOverlapped", new FrontendPointerType(FrontendType.i8));
            //scope.AddVar("WriteFile", write_file, Token.Undefined, isConst: true);


            //var read_file = new FrontendFunctionType();
            //read_file.returnType = FrontendType.bool_;
            //read_file.AddParam("hFile", FrontendType.i64);
            //read_file.AddParam("lpBuffer", new FrontendPointerType(FrontendType.i8));
            //read_file.AddParam("nNumberOfBytesToRead", FrontendType.i32);
            //read_file.AddParam("lpNumberOfBytesRead", new FrontendPointerType(FrontendType.i8));
            //read_file.AddParam("lpOverlapped", new FrontendPointerType(FrontendType.i8));
            //scope.AddVar("ReadFile", read_file, Token.Undefined, isConst: true);


            addIntrinsics(scope);
        }

        static void addIntrinsics(Scope scope)
        {
            var cos = new FrontendFunctionType("cos");
            cos.returnType = FrontendType.f32;
            cos.AddParam("x", FrontendType.f32);
            scope.AddVar("cos", cos, Token.Undefined, isConst: true);

            var sin = new FrontendFunctionType("sin");
            sin.returnType = FrontendType.f32;
            sin.AddParam("x", FrontendType.f32);
            scope.AddVar("sin", sin, Token.Undefined, isConst: true);

            var abs = new FrontendFunctionType("abs");
            abs.returnType = FrontendType.f32;
            abs.AddParam("x", FrontendType.f32);
            scope.AddVar("abs", abs, Token.Undefined, isConst: true);

            var sqrt = new FrontendFunctionType("sqrt");
            sqrt.returnType = FrontendType.f32;
            sqrt.AddParam("x", FrontendType.f32);
            scope.AddVar("sqrt", sqrt, Token.Undefined, isConst: true);

            var floor = new FrontendFunctionType("floor");
            floor.returnType = FrontendType.f32;
            floor.AddParam("x", FrontendType.f32);
            scope.AddVar("floor", floor, Token.Undefined, isConst: true);

            var trunc = new FrontendFunctionType("trunc");
            trunc.returnType = FrontendType.f32;
            trunc.AddParam("x", FrontendType.f32);
            scope.AddVar("trunc", trunc, Token.Undefined, isConst: true);

            var ceil = new FrontendFunctionType("ceil");
            ceil.returnType = FrontendType.f32;
            ceil.AddParam("x", FrontendType.f32);
            scope.AddVar("ceil", ceil, Token.Undefined, isConst: true);

            var round = new FrontendFunctionType("round");
            round.returnType = FrontendType.f32;
            round.AddParam("x", FrontendType.f32);
            scope.AddVar("round", round, Token.Undefined, isConst: true);
        }

        static void addSpecialFunctions(Scope scope)
        {
            var file_pos = new FrontendFunctionType("__file_pos__");
            file_pos.returnType = FrontendType.string_;
            file_pos.specialFun = true;
            scope.AddVar("__file_pos__", file_pos, Token.Undefined, isConst: true);
            
        }

        static void addBasicConstants(Scope scope, Token token)
        {
            var nullptr = scope.AddVar("nullptr", FrontendType.ptr, token, isConst: true);
        }

        public static Scope MakeRootScope()
        {

            var rootScope = new Scope(null);
            addBasicTypes(rootScope, Token.Undefined);
            addBasicConstants(rootScope, Token.Undefined);
            addBasicFunctions(rootScope);
            addSpecialFunctions(rootScope);

            //var main = new Scope.FunctionDefinition { name = "main", returnType = FrontendType.int32 };
            //rootScope.AddFunction(main);
            //rootScope.function = main;
            return rootScope;
        }

    }
}
