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

        
        TypeChecker typeChecker;
        Dictionary<Scope.VariableDefinition, Value> variables = new Dictionary<Scope.VariableDefinition, Value>();
        Stack<Value> valueStack = new Stack<Value>();
        Dictionary<string, Value> stringTable = new Dictionary<string, Value>();

        public Backend(TypeChecker typeChecker) {
            this.typeChecker = typeChecker;
            mod = new Module();
            builder = new Builder(mod);
            
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
                arr_elem_ptr = builder.AddGlobal(at, "str_arr");
                arr_elem_ptr = builder.BuildBitCast(arr_elem_ptr, new PointerType(elem_type), "str_ptr");
            }
            builder.BuildMemCpy(arr_elem_ptr, str_ptr, size);

            // set array length in struct
            var gep_arr_length = builder.BuildGEP(arr_struct_ptr, "gep_arr_elem_ptr", zero_i32_v, zero_i32_v);
            builder.BuildStore(ConstInt(i32_t, str_length), gep_arr_length);

            // set array elem pointer in struct
            var gep_arr_elem_ptr = builder.BuildGEP(arr_struct_ptr, "gep_arr_elem_ptr", zero_i32_v, one_i32_v);
            builder.BuildStore(arr_elem_ptr, gep_arr_elem_ptr);

            var arr_struct = builder.BuildLoad(arr_struct_ptr, "arr_struct_load");
            valueStack.Push(arr_struct);

            builder.PositionAtEnd(insert);
        }

        public void Visit(AST.BinOp node) {
            if (node.type == AST.BinOp.BinOpType.ConditionalOR) {
                visitConditionalOR(node);
                return;
            }
            if (node.type == AST.BinOp.BinOpType.ConditionaAND) {
                visitConditionalAND(node);
                return;
            }

            Visit(node.left);
            var left = valueStack.Pop();
            Visit(node.right);
            var right = valueStack.Pop();

            var leftFrontendType = typeChecker.GetNodeType(node.left);
            var rightFrontendType = typeChecker.GetNodeType(node.right);

            var leftType = left.type;
            var rightType = right.type;


            Value result;
            if (leftFrontendType.Equals(FrontendType.bool_)) {
                switch (node.type) {
                    case AST.BinOp.BinOpType.LogicalAND:
                        result = builder.BuildAnd(left, right, "and_tmp");
                        break;
                    case AST.BinOp.BinOpType.LogicalOR:
                        result = builder.BuildOr(left, right, "or_tmp");
                        break;
                    case AST.BinOp.BinOpType.LogicalXOR:
                        result = builder.BuildXor(left, right, "xor_tmp");
                        break;
                    case AST.BinOp.BinOpType.Equal:
                        result = builder.BuildIcmp(left, right, IcmpType.eq, "icmp_tmp");
                        break;
                    case AST.BinOp.BinOpType.NotEqual:
                        result = builder.BuildIcmp(left, right, IcmpType.ne, "icmp_tmp");
                        break;
                    default:
                        throw new InvalidCodePath();
                }
            } else {
                switch (leftType.kind) {
                    case TypeKind.Integer:
                        switch (node.type) {
                            case AST.BinOp.BinOpType.Add:
                                result = builder.BuildAdd(left, right, "add_tmp");
                                break;
                            case AST.BinOp.BinOpType.Subract:
                                result = builder.BuildSub(left, right, "sub_tmp");
                                break;
                            case AST.BinOp.BinOpType.Multiply:
                                result = builder.BuildMul(left, right, "mul_tmp");
                                break;
                            case AST.BinOp.BinOpType.Divide:
                                result = builder.BuildSDiv(left, right, "div_tmp");
                                break;
                            case AST.BinOp.BinOpType.DivideUnsigned:
                                result = builder.BuildUDiv(left, right, "div_tmp");
                                break;
                            case AST.BinOp.BinOpType.LeftShift:
                                result = builder.BuildShl(left, right, "shl_tmp");
                                break;
                            case AST.BinOp.BinOpType.RightShift:
                                result = builder.BuildAShr(left, right, "shr_tmp");
                                break;
                            case AST.BinOp.BinOpType.RightShiftUnsigned:
                                result = builder.BuildLShr(left, right, "shr_tmp");
                                break;
                            case AST.BinOp.BinOpType.Remainder:
                                result = builder.BuildURem(left, right, "urem_tmp");
                                break;
                            case AST.BinOp.BinOpType.Equal:
                                result = builder.BuildIcmp(left, right, IcmpType.eq, "icmp_tmp");
                                break;
                            case AST.BinOp.BinOpType.NotEqual:
                                result = builder.BuildIcmp(left, right, IcmpType.ne, "icmp_tmp");
                                break;
                            case AST.BinOp.BinOpType.Greater:
                                result = builder.BuildIcmp(left, right, IcmpType.sgt, "icmp_tmp");
                                break;
                            case AST.BinOp.BinOpType.GreaterEqual:
                                result = builder.BuildIcmp(left, right, IcmpType.sge, "icmp_tmp");
                                break;
                            case AST.BinOp.BinOpType.Less:
                                result = builder.BuildIcmp(left, right, IcmpType.slt, "icmp_tmp");
                                break;
                            case AST.BinOp.BinOpType.LessEqual:
                                result = builder.BuildIcmp(left, right, IcmpType.sle, "icmp_tmp");
                                break;
                            case AST.BinOp.BinOpType.GreaterUnsigned:
                                result = builder.BuildIcmp(left, right, IcmpType.ugt, "icmp_tmp");
                                break;
                            case AST.BinOp.BinOpType.GreaterEqualUnsigned:
                                result = builder.BuildIcmp(left, right, IcmpType.uge, "icmp_tmp");
                                break;
                            case AST.BinOp.BinOpType.LessUnsigned:
                                result = builder.BuildIcmp(left, right, IcmpType.ult, "icmp_tmp");
                                break;
                            case AST.BinOp.BinOpType.LessEqualUnsigned:
                                result = builder.BuildIcmp(left, right, IcmpType.ule, "icmp_tmp");
                                break;
                            case AST.BinOp.BinOpType.LogicalAND:
                                result = builder.BuildAnd(left, right, "and_tmp");
                                break;
                            case AST.BinOp.BinOpType.LogicalOR:
                                result = builder.BuildOr(left, right, "or_tmp");
                                break;
                            case AST.BinOp.BinOpType.LogicalXOR:
                                result = builder.BuildXor(left, right, "xor_tmp");
                                break;
                            default:
                                throw new InvalidCodePath();
                        }
                        break;
                    case TypeKind.Double:
                    case TypeKind.Float:
                    case TypeKind.Half:
                        switch (node.type) {
                            case AST.BinOp.BinOpType.Add:
                                result = LLVM.BuildFAdd(builder, left, right, "fadd_tmp");
                                break;
                            case AST.BinOp.BinOpType.Subract:
                                result = LLVM.BuildFSub(builder, left, right, "fsub_tmp");
                                break;
                            case AST.BinOp.BinOpType.Multiply:
                                result = LLVM.BuildFMul(builder, left, right, "fmul_tmp");
                                break;
                            case AST.BinOp.BinOpType.Divide:
                                result = LLVM.BuildFDiv(builder, left, right, "fdiv_tmp");
                                break;
                            case AST.BinOp.BinOpType.Remainder:
                                result = LLVM.BuildFRem(builder, left, right, "frem_tmp");
                                break;
                            case AST.BinOp.BinOpType.Equal:
                                result = LLVM.BuildFCmp(builder, LLVMRealPredicate.LLVMRealOEQ, left, right, "fcmp_tmp");
                                break;
                            case AST.BinOp.BinOpType.NotEqual:
                                result = LLVM.BuildFCmp(builder, LLVMRealPredicate.LLVMRealONE, left, right, "fcmp_tmp");
                                break;
                            case AST.BinOp.BinOpType.Greater:
                                result = LLVM.BuildFCmp(builder, LLVMRealPredicate.LLVMRealOGT, left, right, "fcmp_tmp");
                                break;
                            case AST.BinOp.BinOpType.GreaterEqual:
                                result = LLVM.BuildFCmp(builder, LLVMRealPredicate.LLVMRealOGE, left, right, "fcmp_tmp");
                                break;
                            case AST.BinOp.BinOpType.Less:
                                result = LLVM.BuildFCmp(builder, LLVMRealPredicate.LLVMRealOLT, left, right, "fcmp_tmp");
                                break;
                            case AST.BinOp.BinOpType.LessEqual:
                                result = LLVM.BuildFCmp(builder, LLVMRealPredicate.LLVMRealOLE, left, right, "fcmp_tmp");
                                break;
                            default:
                                throw new InvalidCodePath();
                        }
                        break;
                    case LLVMTypeKind.LLVMPointerTypeKind: {
                            if (LLVM.GetTypeKind(rightType) == LLVMTypeKind.LLVMIntegerTypeKind) {
                                switch (node.type) {

                                    case AST.BinOp.BinOpType.Add: {
                                            var indices = new LLVMValueRef[] { right };
                                            result = LLVM.BuildGEP(builder, left, out indices[0], 1, "ptr_add");
                                        }
                                        break;
                                    case AST.BinOp.BinOpType.Subract: {
                                            var n_right = LLVM.BuildNeg(builder, right, "ptr_add_neg");
                                            var indices = new LLVMValueRef[] { n_right };
                                            result = LLVM.BuildGEP(builder, left, out indices[0], 1, "ptr_add");
                                        }
                                        break;
                                    default:
                                        throw new InvalidCodePath();
                                }
                                break;
                            } else if (LLVM.GetTypeKind(rightType) == LLVMTypeKind.LLVMPointerTypeKind) {
                                switch (node.type) {
                                    case AST.BinOp.BinOpType.Subract: {
                                            var li = LLVM.BuildPtrToInt(builder, left, Const.mm, "ptr_to_int");
                                            var ri = LLVM.BuildPtrToInt(builder, right, Const.mm, "ptr_to_int");
                                            var sub = LLVM.BuildSub(builder, li, ri, "sub");

                                            result = LLVM.BuildSDiv(builder, sub, LLVM.SizeOf(LLVM.GetElementType(leftType)), "div");
                                        }
                                        break;
                                    case AST.BinOp.BinOpType.GreaterUnsigned:
                                        result = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntUGT, left, right, "icmp_tmp");
                                        break;
                                    case AST.BinOp.BinOpType.GreaterEqualUnsigned:
                                        result = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntUGE, left, right, "icmp_tmp");
                                        break;
                                    case AST.BinOp.BinOpType.LessUnsigned:
                                        result = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntULT, left, right, "icmp_tmp");
                                        break;
                                    case AST.BinOp.BinOpType.LessEqualUnsigned:
                                        result = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntULE, left, right, "icmp_tmp");
                                        break;
                                    case AST.BinOp.BinOpType.Equal:
                                        result = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntEQ, left, right, "icmp_tmp");
                                        break;
                                    case AST.BinOp.BinOpType.NotEqual:
                                        result = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntNE, left, right, "icmp_tmp");
                                        break;
                                    default:
                                        throw new InvalidCodePath();
                                }
                            } else
                                throw new InvalidCodePath();
                        }
                        break;
                    default:
                        throw new InvalidCodePath();
                }
            }
            valueStack.Push(result);
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
