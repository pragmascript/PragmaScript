using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static PragmaScript.SSA;
using static PragmaScript.SSA.Const;

namespace PragmaScript {
    partial class Backend {
        public class Cursor {
            public Function currentFunction;
            public Block currentBlock;

            public Block vars;
            public Block entry;
            
            public void EnterFunction(Function function) {
                currentFunction = function;
                vars = function.blocks["vars"];
                entry = function.blocks["entry"];
                currentBlock = entry;
            }
        }

        public Module mod;
        public Cursor cursor;
        Value intrinsic_memcpy;

        public Backend() {
            mod = new Module();
            // add memcpy
            var ft = new FunctionType(Const.void_t, Const.ptr_t, Const.ptr_t, Const.i32_t, Const.bool_t);
            intrinsic_memcpy = mod.AddFunction("llvm.memcpy.p0i8.p0i8.i32", ft).value;
        }

        static bool isConstVariableDefinition(AST.Node node) {
            if (node is AST.VariableDefinition vd) {
                return vd.variable.isConstant;
            }
            return false;
        }

        static bool isGlobalVariableDefinition(AST.Node node) {
            if (node is AST.VariableDefinition vd) {
                return vd.variable.isGlobal;
            }
            return false;
        }

        public Function CreateAndEnterFunction(string name, FunctionType ft) {
            var function = mod.AddFunction(name, ft);
            var vars = function.AppendBasicBlock("vars");
            var entry = function.AppendBasicBlock("entry");
            cursor.EnterFunction(function);
            return function;
        }

        public void Visit(AST.ProgramRoot node, AST.FunctionDefinition main) {
            // HACK:
            AST.FileRoot merge = new AST.FileRoot(Token.Undefined, node.scope);
            foreach (var fr in node.files) {
                foreach (var decl in fr.declarations) {
                    merge.declarations.Add(decl);
                }
            }
            Visit(merge, main);
        }

        public void Visit(AST.FileRoot node, AST.FunctionDefinition main) {
            var constVariables = new List<AST.Node>();
            var functionDefinitions = new List<AST.Node>();
            var globalVariables = new List<AST.Node>();
            var other = new List<AST.Node>();

            // visit function definitions make prototypes
            foreach (var decl in node.declarations) {
                if (isConstVariableDefinition(decl)) {
                    constVariables.Add(decl);
                } else if (isGlobalVariableDefinition(decl)) {
                    globalVariables.Add(decl);
                } else if (decl is AST.FunctionDefinition) {
                    functionDefinitions.Add(decl);
                    if (!(decl as AST.FunctionDefinition).external) {
                        other.Add(decl);
                    }
                } else {
                    other.Add(decl);
                }
            }

            FunctionType ft;
            if (CompilerOptions.dll) {
                ft = new FunctionType(i32_t, mm_t, i32_t, ptr_t);
            } else {
                ft = new FunctionType(void_t);
            }
            // var blockTemp = LLVM.GetInsertBlock(builder);

            //CreateAndEnterFunction("__init", ft);

            //foreach (var decl in functionDefinitions) {
            //    Visit(decl as AST.FunctionDefinition, proto: true);
            //}
            //foreach (var decl in constVariables) {
            //    Visit(decl as AST.VariableDefinition);
            //}
            //foreach (var decl in globalVariables) {
            //    Visit(decl as AST.VariableDefinition);
            //}
            //foreach (var decl in other) {
            //    Visit(decl);
            //}

            //LLVM.PositionBuilderAtEnd(builder, entry);

            //if (main != null) {
            //    var mf = variables[main.scope.GetVar(main.funName, main.token)];
            //    var par = new LLVMValueRef[1];
            //    LLVM.BuildCall(builder, mf, out par[0], 0, "");
            //}

            //if (CompilerOptions.dll) {
            //    LLVM.BuildRet(builder, Const.OneInt32);
            //} else {
            //    LLVM.BuildRetVoid(builder);
            //}

            //LLVM.PositionBuilderAtEnd(builder, vars);
            //LLVM.BuildBr(builder, entry);

            //LLVM.PositionBuilderAtEnd(builder, blockTemp);


            //LLVM.VerifyFunction(function, LLVMVerifierFailureAction.LLVMPrintMessageAction);

            //ctx.Pop();
        }
    }
}
