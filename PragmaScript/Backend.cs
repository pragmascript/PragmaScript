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

        Value intrinsic_memcpy;
        TypeChecker typeChecker;
        Dictionary<Scope.VariableDefinition, Value> variables = new Dictionary<Scope.VariableDefinition, Value>();
        Stack<Value> valueStack = new Stack<Value>();
        Dictionary<string, Value> stringTable = new Dictionary<string, Value>();

        public Backend(TypeChecker typeChecker) {
            this.typeChecker = typeChecker;
            mod = new Module();
            builder = new Builder(mod);
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

            builder.CreateAndEnterFunction("__init", ft);

            foreach (var decl in functionDefinitions) {
                Visit(decl as AST.FunctionDefinition, proto: true);
            }
            foreach (var decl in constVariables) {
                Visit(decl as AST.VariableDefinition);
            }
            foreach (var decl in globalVariables) {
                Visit(decl as AST.VariableDefinition);
            }
            foreach (var decl in other) {
                Visit(decl);
            }


            var entry = builder.PositionAtEnd("entry");

            if (main != null) {
                var mf = variables[main.scope.GetVar(main.funName, main.token)];
                builder.BuildCall(mf);
            }

            if (CompilerOptions.dll) {
                builder.BuildRet(one_i32_v);
            } else {
                builder.BuildRet(void_v);
            }

            builder.PositionAtEnd("vars");
            builder.BuildBr(entry);
        }

        public void Visit(AST.Namespace node) {
            var functionDefinitions = new List<AST.Node>();
            var constVariables = new List<AST.Node>();
            var variables = new List<AST.Node>();
            var other = new List<AST.Node>();

            // visit function definitions make prototypes
            foreach (var decl in node.declarations) {
                if (decl is AST.VariableDefinition vd) {
                    if (vd.variable.isConstant) {
                        constVariables.Add(decl);
                    } else {
                        variables.Add(decl);
                    }
                } else if (decl is AST.FunctionDefinition) {
                    functionDefinitions.Add(decl);
                    if (!(decl as AST.FunctionDefinition).external) {
                        other.Add(decl);
                    }
                } else {
                    other.Add(decl);
                }
            }
            foreach (var decl in functionDefinitions) {
                Visit(decl as AST.FunctionDefinition, proto: true);
            }
            foreach (var decl in constVariables) {
                Visit(decl as AST.VariableDefinition);
            }
            foreach (var decl in variables) {
                Visit(decl as AST.VariableDefinition);
            }
            foreach (var decl in other) {
                Visit(decl);
            }
        }

        public void Visit(AST.ConstInt node) {
            var nt = typeChecker.GetNodeType(node);
            var ct = GetTypeRef(nt);
            Value result;
            if (ct.kind == TypeKind.Half || ct.kind == TypeKind.Float || ct.kind == TypeKind.Double) {
                result = ConstReal(ct, node.number);
            } else {
                Debug.Assert(ct.kind == TypeKind.Integer);
                result = ConstInt(ct, (ulong)node.number);
            }
            valueStack.Push(result);
        }

        public void Visit(AST.ConstFloat node) {
            var ct = GetTypeRef(typeChecker.GetNodeType(node));
            var result = ConstReal(ct, node.number);
            valueStack.Push(result);
        }

        public void Visit(AST.ConstBool node) {
            var result = node.value ? true_v : false_v;
            valueStack.Push(result);
        }

        public void Visit(AST.ConstString node, bool needsConversion = true) {
            var str = node.s;
            if (needsConversion) {
                str = node.ConvertString();
            }

            Value str_ptr;

            if (!stringTable.TryGetValue(str, out str_ptr)) {
                str_ptr = builder.BuildGlobalStringPtr(str, "str");
                stringTable.Add(str, str_ptr);
            }

            var type = FrontendType.string_;
            var arr_struct_type = GetTypeRef(type);
            var insert = builder.GetInsertBlock();

            builder.PositionAtEnd(builder.context.vars);

            var arr_struct_ptr = builder.BuildAlloca(arr_struct_type, "arr_struct_alloca");
            var str_length = (uint)str.Length;
            var elem_type = GetTypeRef(type.elementType);

            var size = ConstInt(i32_t, str_length);

            Value arr_elem_ptr;

            
            if (node.scope.function != null) {
                arr_elem_ptr = builder.BuildArrayAlloca(elem_type, size, "arr_elem_alloca");
            } else {

                var at = new ArrayType(elem_type, str_length);

                arr_elem_ptr = builder.AddGlobal(at, "str_arr");

                // if we are in a "global" scope dont allocate on the stack

                arr_elem_ptr = LLVM.AddGlobal(mod, LLVM.ArrayType(elem_type, str_length), "str_arr");
                LLVM.SetLinkage(arr_elem_ptr, LLVMLinkage.LLVMInternalLinkage);

                //LLVMValueRef[] bytes = new LLVMValueRef[str.Length];
                //if (str.Length == 0)
                //{
                //    bytes = new LLVMValueRef[1];
                //}
                //for (int i = 0; i < str.Length; ++i)
                //{
                //    bytes[i] = LLVM.ConstInt(elem_type, (ulong)str[i], false);
                //}
                // LLVM.SetInitializer(arr_elem_ptr, LLVM.ConstArray(elem_type, out bytes[0], (uint)str.Length));
                LLVM.SetInitializer(arr_elem_ptr, LLVM.ConstNull(at));
                arr_elem_ptr = LLVM.BuildBitCast(builder, arr_elem_ptr, LLVM.PointerType(elem_type, 0), "str_ptr");
            }
            BuildMemCpy(arr_elem_ptr, str_ptr, size);

            // LLVM.SetAlignment(arr_elem_ptr, 4);

            // set array length in struct
            var gep_idx_0 = new LLVMValueRef[] { Const.ZeroInt32, Const.ZeroInt32 };
            var gep_arr_length = LLVM.BuildGEP(builder, arr_struct_ptr, out gep_idx_0[0], 2, "gep_arr_elem_ptr");
            LLVM.BuildStore(builder, LLVM.ConstInt(Const.Int32Type, str_length, true), gep_arr_length);

            // set array elem pointer in struct
            var gep_idx_1 = new LLVMValueRef[] { Const.ZeroInt32, Const.OneInt32 };
            var gep_arr_elem_ptr = LLVM.BuildGEP(builder, arr_struct_ptr, out gep_idx_1[0], 2, "gep_arr_elem_ptr");
            LLVM.BuildStore(builder, arr_elem_ptr, gep_arr_elem_ptr);



            //for (int i = 0; i < bytes.Length; ++i)
            //{
            //    var c = bytes[i];
            //    var gep_idx = new LLVMValueRef[] { LLVM.ConstInt(Const.Int32Type, (ulong)i, Const.FalseBool) };
            //    var gep = LLVM.BuildGEP(builder, arr_elem_ptr, out gep_idx[0], 1, "array_elem_" + i);
            //    var store = LLVM.BuildStore(builder, LLVM.ConstInt(Const.Int8Type, (ulong)c, true), gep);
            //    // LLVM.SetAlignment(store, 4);
            //}

            var arr_struct = LLVM.BuildLoad(builder, arr_struct_ptr, "arr_struct_load");
            valueStack.Push(arr_struct);

            LLVM.PositionBuilderAtEnd(builder, insert);

        }

        public void Visit(AST.FunctionDefinition node, bool proto = false) {
            var fun = typeChecker.GetNodeType(node) as FrontendFunctionType;
            if (fun.inactiveConditional) {
                return;
            }
            if (proto) {
                if (node.isFunctionTypeDeclaration()) {
                    return;
                }
                var funPointer = GetTypeRef(fun) as PointerType;
                var funType = funPointer.elementType as FunctionType;
                Debug.Assert(funPointer != null);
                Debug.Assert(funType != null);
                // TODO(pragma): 
                // if (node.HasAttribute("STUB")) {
                var functionName = node.externalFunctionName != null ? node.externalFunctionName : node.funName;
                var function = mod.AddFunction(functionName, funType);
                variables.Add(node.variableDefinition, function.value);
            } else {
                if (node.external || node.body == null) {
                    return;
                }
                var functionName = node.externalFunctionName != null ? node.externalFunctionName : node.funName;
                var function = mod.functions[functionName];


                if (node.HasAttribute("DLL.EXPORT")) {
                    function.ExportDLL = true;
                }

                var vars = function.AppendBasicBlock("vars");
                var entry = function.AppendBasicBlock("entry");
                builder.EnterFunction(function);

                if (node.body != null) {
                    Visit(node.body);
                }

                var returnType = GetTypeRef(fun.returnType);
                // insertMissingReturn(returnType);

                builder.PositionAtEnd(vars);
                builder.BuildBr(entry);
            }
        }




    }
}
