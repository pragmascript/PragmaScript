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

            var function = builder.AddFunction(ft, "__init");
            var vars = builder.AppendBasicBlock(function, "vars");
            var entry = builder.AppendBasicBlock(function, "entry");
            builder.context.SetFunctionBlocks(function, vars, entry);

            // var blockTemp = builder.GetInsertBlock();
            builder.PositionAtEnd(entry);

            
            
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

            builder.PositionAtEnd(entry);

            if (main != null) {
                var mf = variables[main.scope.GetVar(main.funName, main.token)];
                builder.BuildCall(mf);
            }

            if (CompilerOptions.dll) {
                builder.BuildRet(one_i32_v);
            } else {
                builder.BuildRet(void_v);
            }

            builder.PositionAtEnd(vars);
            builder.BuildBr(entry);

            // builder.PositionAtEnd(blockTemp);
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
                result = new ConstReal(ct, node.number);
            } else {
                Debug.Assert(ct.kind == TypeKind.Integer);
                result = new ConstInt(ct, (ulong)node.number);
            }
            valueStack.Push(result);
        }

        public void Visit(AST.ConstFloat node) {
            var ct = GetTypeRef(typeChecker.GetNodeType(node));
            var result = new ConstReal(ct, node.number);
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

            builder.PositionAtEnd(builder.context.currentFunctionContext.vars);

            var arr_struct_ptr = builder.BuildAlloca(arr_struct_type, "arr_struct_alloca");
            var str_length = (uint)str.Length;
            var elem_type = GetTypeRef(type.elementType);

            var size = new ConstInt(i32_t, str_length);

            Value arr_elem_ptr;

            if (node.scope.function != null) {
                arr_elem_ptr = builder.BuildArrayAlloca(elem_type, size, "arr_elem_alloca");
            } else {
                var at = new ArrayType(elem_type, str_length);
                arr_elem_ptr = builder.AddGlobal(at, "str_arr");
                // if we are in a "global" scope dont allocate on the stack
                ((GlobalVariable)arr_elem_ptr).SetInitializer(builder.ConstNull(at));
                arr_elem_ptr = builder.BuildBitCast(arr_elem_ptr, new PointerType(elem_type), "str_ptr");
            }
            builder.BuildMemCpy(arr_elem_ptr, str_ptr, size);

            // set array length in struct
            var gep_arr_length = builder.BuildGEP(arr_struct_ptr, "gep_arr_elem_ptr", false, zero_i32_v, zero_i32_v);
            builder.BuildStore(new ConstInt(i32_t, str_length), gep_arr_length);

            // set array elem pointer in struct
            var gep_arr_elem_ptr = builder.BuildGEP(arr_struct_ptr, "gep_arr_elem_ptr", false, zero_i32_v, one_i32_v);
            builder.BuildStore(arr_elem_ptr, gep_arr_elem_ptr);

            var arr_struct = builder.BuildLoad(arr_struct_ptr, "arr_struct_load");
            valueStack.Push(arr_struct);

            builder.PositionAtEnd(insert);
        }

        public void Visit(AST.BinOp node) {
            if (node.type == AST.BinOp.BinOpType.ConditionalOR) {
                VisitConditionalOR(node);
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
                        result = builder.BuildICmp(left, right, IcmpType.eq, "icmp_tmp");
                        break;
                    case AST.BinOp.BinOpType.NotEqual:
                        result = builder.BuildICmp(left, right, IcmpType.ne, "icmp_tmp");
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
                                result = builder.BuildICmp(left, right, IcmpType.eq, "icmp_tmp");
                                break;
                            case AST.BinOp.BinOpType.NotEqual:
                                result = builder.BuildICmp(left, right, IcmpType.ne, "icmp_tmp");
                                break;
                            case AST.BinOp.BinOpType.Greater:
                                result = builder.BuildICmp(left, right, IcmpType.sgt, "icmp_tmp");
                                break;
                            case AST.BinOp.BinOpType.GreaterEqual:
                                result = builder.BuildICmp(left, right, IcmpType.sge, "icmp_tmp");
                                break;
                            case AST.BinOp.BinOpType.Less:
                                result = builder.BuildICmp(left, right, IcmpType.slt, "icmp_tmp");
                                break;
                            case AST.BinOp.BinOpType.LessEqual:
                                result = builder.BuildICmp(left, right, IcmpType.sle, "icmp_tmp");
                                break;
                            case AST.BinOp.BinOpType.GreaterUnsigned:
                                result = builder.BuildICmp(left, right, IcmpType.ugt, "icmp_tmp");
                                break;
                            case AST.BinOp.BinOpType.GreaterEqualUnsigned:
                                result = builder.BuildICmp(left, right, IcmpType.uge, "icmp_tmp");
                                break;
                            case AST.BinOp.BinOpType.LessUnsigned:
                                result = builder.BuildICmp(left, right, IcmpType.ult, "icmp_tmp");
                                break;
                            case AST.BinOp.BinOpType.LessEqualUnsigned:
                                result = builder.BuildICmp(left, right, IcmpType.ule, "icmp_tmp");
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
                                result = builder.BuildFAdd(left, right, "fadd_tmp");
                                break;
                            case AST.BinOp.BinOpType.Subract:
                                result = builder.BuildFSub(left, right, "fsub_tmp");
                                break;
                            case AST.BinOp.BinOpType.Multiply:
                                result = builder.BuildFMul(left, right, "fmul_tmp");
                                break;
                            case AST.BinOp.BinOpType.Divide:
                                result = builder.BuildFDiv(left, right, "fdiv_tmp");
                                break;
                            case AST.BinOp.BinOpType.Remainder:
                                result = builder.BuildFRem(left, right, "frem_tmp");
                                break;
                            case AST.BinOp.BinOpType.Equal:
                                result = builder.BuildFCmp(left, right, FcmpType.oeq, "fcmp_tmp");
                                break;
                            case AST.BinOp.BinOpType.NotEqual:
                                result = builder.BuildFCmp(left, right, FcmpType.one, "fcmp_tmp");
                                break;
                            case AST.BinOp.BinOpType.Greater:
                                result = builder.BuildFCmp(left, right, FcmpType.ogt, "fcmp_tmp");
                                break;
                            case AST.BinOp.BinOpType.GreaterEqual:
                                result = builder.BuildFCmp(left, right, FcmpType.oge, "fcmp_tmp");
                                break;
                            case AST.BinOp.BinOpType.Less:
                                result = builder.BuildFCmp(left, right, FcmpType.olt, "fcmp_tmp");
                                break;
                            case AST.BinOp.BinOpType.LessEqual:
                                result = builder.BuildFCmp(left, right, FcmpType.ole, "fcmp_tmp");
                                break;
                            default:
                                throw new InvalidCodePath();
                        }
                        break;
                    case TypeKind.Pointer: {
                            if (rightType.kind == TypeKind.Integer) {
                                switch (node.type) {
                                    case AST.BinOp.BinOpType.Add: {
                                            result = builder.BuildGEP(left, "ptr_add", false, right);
                                        }
                                        break;
                                    case AST.BinOp.BinOpType.Subract: {
                                            var n_right = builder.BuildNeg(right, "ptr_add_neg");
                                            result = builder.BuildGEP(left, "ptr_add", false, n_right);
                                        }
                                        break;
                                    default:
                                        throw new InvalidCodePath();
                                }
                                break;
                            } else if (rightType.kind == TypeKind.Pointer) {
                                switch (node.type) {
                                    case AST.BinOp.BinOpType.Subract: {
                                            var li = builder.BuildPtrToInt(left, mm_t, "ptr_to_int");
                                            var ri = builder.BuildPtrToInt(right, mm_t, "ptr_to_int");
                                            var sub = builder.BuildSub(li, ri, "sub");
                                            var lpt = ((PointerType)leftType).elementType;
                                            var size_of = builder.BuildSizeOf(leftType);
                                            result = builder.BuildSDiv(sub, size_of, "div");
                                        }
                                        break;
                                    case AST.BinOp.BinOpType.GreaterUnsigned:
                                        result = builder.BuildICmp(left, right, IcmpType.ugt, "icmp_tmp");
                                        break;
                                    case AST.BinOp.BinOpType.GreaterEqualUnsigned:
                                        result = builder.BuildICmp(left, right, IcmpType.uge, "icmp_tmp");
                                        break;
                                    case AST.BinOp.BinOpType.LessUnsigned:
                                        result = builder.BuildICmp(left, right, IcmpType.ult, "icmp_tmp");
                                        break;
                                    case AST.BinOp.BinOpType.LessEqualUnsigned:
                                        result = builder.BuildICmp(left, right, IcmpType.ule, "icmp_tmp");
                                        break;
                                    case AST.BinOp.BinOpType.Equal:
                                        result = builder.BuildICmp(left, right, IcmpType.eq, "icmp_tmp");
                                        break;
                                    case AST.BinOp.BinOpType.NotEqual:
                                        result = builder.BuildICmp(left, right, IcmpType.ne, "icmp_tmp");
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

        void VisitConditionalOR(AST.BinOp op) {
            Visit(op.left);
            var cmp = valueStack.Pop();
            Debug.Assert(SSAType.IsBoolType(cmp.type));
            var block = builder.GetInsertBlock();

            var cor_rhs = builder.AppendBasicBlock("cor.rhs");

            // TODO(pragma): why do i need this?
            builder.MoveBasicBlockAfter(cor_rhs, block);

            var cor_end = builder.AppendBasicBlock("cor.end");

            // TODO(pragma): why do i need this?
            builder.MoveBasicBlockAfter(cor_end, cor_rhs);

            builder.BuildCondBr(cmp, cor_end, cor_rhs);

            // cor.rhs: 
            builder.PositionAtEnd(cor_rhs);
            Visit(op.right);

            var cor_rhs_tv = valueStack.Pop();
            Debug.Assert(SSAType.IsBoolType(cor_rhs_tv.type));
            builder.BuildBr(cor_end);

            cor_rhs = builder.GetInsertBlock();

            // cor.end:
            builder.PositionAtEnd(cor_end);

            var phi = builder.BuildPhi(bool_t, "corphi", (true_v, block), (cor_rhs_tv, cor_rhs));

            valueStack.Push(phi);
        }

        void visitConditionalAND(AST.BinOp op) {
            Visit(op.left);
            var cmp = valueStack.Pop();
            Debug.Assert(SSAType.IsBoolType(cmp.type));
            var block = builder.GetInsertBlock();

            var cand_rhs = builder.AppendBasicBlock("cand.rhs");
            builder.MoveBasicBlockAfter(cand_rhs, block);

            var cand_end = builder.AppendBasicBlock("cand.end");
            builder.MoveBasicBlockAfter(cand_end, cand_rhs);

            builder.BuildCondBr(cmp, cand_rhs, cand_end);

            // cor.rhs: 
            builder.PositionAtEnd(cand_rhs);
            Visit(op.right);
            var cand_rhs_tv = valueStack.Pop();
            Debug.Assert(SSAType.IsBoolType(cand_rhs_tv.type));

            builder.BuildBr(cand_end);
            cand_rhs = builder.GetInsertBlock();

            // cor.end:
            builder.PositionAtEnd(cand_end);
            var phi = builder.BuildPhi(bool_t, "candphi", (false_v, block), (cand_rhs_tv, cand_rhs));

            valueStack.Push(phi);
        }

        public void Visit(AST.UnaryOp node) {
            if (node.type == AST.UnaryOp.UnaryOpType.SizeOf) {
                var fet = typeChecker.GetNodeType(node.expression);
                var et = GetTypeRef(fet);

                valueStack.Push(builder.BuildSizeOf(et));
                return;
            }

            Visit(node.expression);

            var v = valueStack.Pop();
            var vtype = v.type;
            Value result;

            switch (node.type) {
                case AST.UnaryOp.UnaryOpType.Add:
                    result = v;
                    break;
                case AST.UnaryOp.UnaryOpType.Subract:
                    switch (vtype.kind) {
                        case TypeKind.Half:
                        case TypeKind.Float:
                        case TypeKind.Double:
                            result = builder.BuildFNeg(v, "fneg_tmp");
                            break;
                        case TypeKind.Integer:
                            result = builder.BuildNeg(v, "neg_tmp");
                            break;
                        default:
                            throw new InvalidCodePath();
                    }
                    break;
                case AST.UnaryOp.UnaryOpType.LogicalNot:
                    Debug.Assert(SSAType.IsBoolType(vtype));
                    result = builder.BuildNot(v, "not_tmp");
                    break;
                case AST.UnaryOp.UnaryOpType.Complement:
                    result = builder.BuildXor(v, new ConstInt(vtype, unchecked((ulong)-1)), "complement_tmp");
                    break;
                case AST.UnaryOp.UnaryOpType.AddressOf:
                    // HACK: for NOW this happens via returnPointer nonsense
                    result = v;
                    if (v.type is FunctionType) {
                        builder.BuildBitCast(v, ptr_t, "func_pointer");
                    }
                    break;
                case AST.UnaryOp.UnaryOpType.Dereference:
                    result = v;
                    if (!node.returnPointer) {
                        result = builder.BuildLoad(result, "deref");
                    }

                    break;
                case AST.UnaryOp.UnaryOpType.PreInc: {
                        result = builder.BuildLoad(v, "preinc_load");
                        Debug.Assert(vtype is PointerType);
                        var vet = (vtype as PointerType).elementType;
                        var vet_kind = vet.kind;
                        switch (vet_kind) {
                            case TypeKind.Integer:
                                result = builder.BuildAdd(result, new ConstInt(vet, 1), "preinc");
                                break;
                            case TypeKind.Half:
                            case TypeKind.Float:
                            case TypeKind.Double:
                                result = builder.BuildFAdd(result, new ConstReal(vet, 1.0), "preinc");
                                break;
                            case TypeKind.Pointer:
                                result = builder.BuildGEP(result, "ptr_pre_inc", false, one_i32_v);
                                break;
                            default:
                                throw new InvalidCodePath();
                        }
                        builder.BuildStore(result, v);
                    }
                    break;
                case AST.UnaryOp.UnaryOpType.PreDec: {
                        result = builder.BuildLoad(v, "predec_load");
                        Debug.Assert(vtype is PointerType);
                        var vet = (vtype as PointerType).elementType;
                        var vet_kind = vet.kind;
                        switch (vet_kind) {
                            case TypeKind.Integer:
                                result = builder.BuildSub(result, new ConstInt(vet, 1), "predec");
                                break;
                            case TypeKind.Half:
                            case TypeKind.Float:
                            case TypeKind.Double:
                                result = builder.BuildFSub(result, new ConstReal(vet, 1.0), "predec");
                                break;
                            case TypeKind.Pointer:
                                result = builder.BuildGEP(result, "ptr_pre_dec", false, neg_1_i32_v);
                                break;
                            default:
                                throw new InvalidCodePath();
                        }
                        builder.BuildStore(result, v);
                    }
                    break;
                case AST.UnaryOp.UnaryOpType.PostInc: {
                        result = builder.BuildLoad(v, "postinc_load");
                        Debug.Assert(vtype is PointerType);
                        var vet = (vtype as PointerType).elementType;
                        var vet_kind = vet.kind;
                        switch (vet_kind) {
                            case TypeKind.Integer: {
                                    var inc = builder.BuildAdd(result, new ConstInt(vet, 1), "postinc");
                                    builder.BuildStore(inc, v);
                                }
                                break;
                            case TypeKind.Half:
                            case TypeKind.Float:
                            case TypeKind.Double: {
                                    var inc = builder.BuildFAdd(result, new ConstReal(vet, 1.0), "postinc");
                                    builder.BuildStore(inc, v);
                                }
                                break;
                            case TypeKind.Pointer: {
                                    var inc = builder.BuildGEP(result, "ptr_post_inc", false, one_i32_v);
                                    builder.BuildStore(inc, v);
                                }
                                break;
                            default:
                                throw new InvalidCodePath();
                        }
                    }
                    break;
                case AST.UnaryOp.UnaryOpType.PostDec: {
                        result = builder.BuildLoad(v, "postdec_load");
                        Debug.Assert(vtype is PointerType);
                        var vet = (vtype as PointerType).elementType;
                        var vet_kind = vet.kind;
                        switch (vet_kind) {
                            case TypeKind.Integer: {
                                    var inc = builder.BuildSub(result, new ConstInt(vet, 1), "postdec");
                                    builder.BuildStore(inc, v);
                                }
                                break;
                            case TypeKind.Half:
                            case TypeKind.Float:
                            case TypeKind.Double: {
                                    var inc = builder.BuildFSub(result, new ConstReal(vet, 1.0), "postdec");
                                    builder.BuildStore(inc, v);
                                }
                                break;
                            case TypeKind.Pointer: {
                                    var inc = builder.BuildGEP(result, "ptr_post_dec", false, neg_1_i32_v);
                                    builder.BuildStore(inc, v);
                                }
                                break;
                            default:
                                throw new InvalidCodePath();
                        }
                    }
                    break;
                default:
                    throw new InvalidCodePath();
            }
            valueStack.Push(result);
        }

        public void Visit(AST.TypeCastOp node) {
            Visit(node.expression);

            var v = valueStack.Pop();
            var vtype = v.type;

            var typeName = node.typeString.ToString(); // node.type.ToString();

            Value result = null;
            var targetType = GetTypeRef(typeChecker.GetNodeType(node));

            if (targetType.EqualType(vtype)) {
                result = v;
                valueStack.Push(result);
                return;
            }

            //var ttk = targetType.kind;
            //var vtk = vtype.kind;
            switch (targetType) {
                case IntegerType t_it:
                    switch (vtype) {
                        case IntegerType v_it:
                            if (t_it.bitWidth > v_it.bitWidth) {
                                if (!node.unsigned) {
                                    result = builder.BuildSExt(v, targetType, "int_cast");
                                } else {
                                    result = builder.BuildZExt(v, targetType, "int_cast");
                                }
                            } else if (t_it.bitWidth < v_it.bitWidth) {
                                result = builder.BuildTrunc(v, targetType, "int_trunc");
                            } else if (t_it.bitWidth == v_it.bitWidth) {
                                result = builder.BuildBitCast(v, targetType, "int_bitcast");
                            }
                            break;
                        case FloatType v_ft:
                            if (!node.unsigned) {
                                result = builder.BuildFPToSI(v, targetType, "int_cast");
                            } else {
                                result = builder.BuildFPToUI(v, targetType, "int_cast");
                            }
                            break;
                        case PointerType v_pt:
                            result = builder.BuildPtrToInt(v, targetType, "int_cast");
                            break;
                        default:
                            throw new InvalidCodePath();
                    }
                    break;
                case FloatType t_ft:
                    switch (vtype) {
                        case IntegerType v_it:
                            if (!node.unsigned) {
                                result = builder.BuildSIToFP(v, targetType, "int_to_float_cast");
                            } else {
                                result = builder.BuildUIToFP(v, targetType, "int_to_float_cast");
                            }
                            break;
                        case FloatType v_fp:
                            result = builder.BuildFPCast(v, targetType, "fp_to_fp_cast");
                            break;
                        default:
                            throw new InvalidCodePath();
                    }
                    break;
                case PointerType t_pt:
                    switch (vtype) {
                        case IntegerType v_it:
                            result = builder.BuildIntToPtr(v, targetType, "int_to_ptr");
                            break;
                        case PointerType v_pt:
                            result = builder.BuildBitCast(v, targetType, "pointer_bit_cast");
                            break;
                        default:
                            throw new InvalidCodePath();
                    }
                    break;
                default:
                    throw new InvalidCodePath();

            }

            Debug.Assert(result != null);
            valueStack.Push(result);
        }


        public void Visit(AST.StructConstructor node) {
            var sc = node;
            var sft = typeChecker.GetNodeType(node) as FrontendStructType;
            var structType = GetTypeRef(sft);

            var insert = builder.GetInsertBlock();
            builder.PositionAtEnd(builder.context.currentFunctionContext.vars);
            var struct_ptr = builder.BuildAlloca(structType, "struct_alloca");
            builder.PositionAtEnd(insert);

            for (int i = 0; i < sft.fields.Count; ++i) {
                if (i < node.argumentList.Count) {
                    Visit(sc.argumentList[i]);
                    var arg = valueStack.Pop();
                    var arg_ptr = builder.BuildStructGEP(struct_ptr, i, "struct_arg_" + i);
                    builder.BuildStore(arg, arg_ptr);
                } else {
                    var arg_ptr = builder.BuildStructGEP(struct_ptr, i, "struct_arg_" + i);
                    var pt = (arg_ptr.type as PointerType).elementType;
                    builder.BuildStore(builder.ConstNull(pt), arg_ptr);
                }
            }
            valueStack.Push(struct_ptr);
        }

        public void Visit(AST.ArrayConstructor node) {
            var ac = node;
            var ac_type = typeChecker.GetNodeType(node) as FrontendArrayType;

            var arr_struct_type = GetTypeRef(ac_type);

            var insert = builder.GetInsertBlock();
            builder.PositionAtEnd(builder.context.currentFunctionContext.vars);
            var arr_struct_ptr = builder.BuildAlloca(arr_struct_type, "arr_struct_alloca");
            var elem_type = GetTypeRef(ac_type.elementType);
            var size = new ConstInt(i32_t, (ulong)ac.elements.Count);
            var arr_elem_ptr = builder.BuildArrayAlloca(elem_type, size, "arr_elem_alloca");
            builder.PositionAtEnd(insert);

            // set array length in struct
            // TODO(pragma): this should be StructGEP?
            var gep_arr_length = builder.BuildGEP(arr_struct_ptr, "gep_arr_elem_ptr", false, zero_i32_v, zero_i32_v);
            builder.BuildStore(new ConstInt(i32_t, (ulong)ac.elements.Count), gep_arr_length);

            // set array elem pointer in struct
            var gep_arr_elem_ptr = builder.BuildGEP(arr_struct_ptr, "gep_arr_elem_ptr", false, zero_i32_v, one_i32_v);
            builder.BuildStore(arr_elem_ptr, gep_arr_elem_ptr);

            for (int i = 0; i < ac.elements.Count; ++i) {
                var elem = ac.elements[i];
                Visit(elem);
                var arg = valueStack.Pop();
                // var arg_type_string = typeToString(LLVM.TypeOf(arg));
                var gep_idx = new ConstInt(i32_t, (ulong)i);
                var gep = builder.BuildGEP(arr_elem_ptr, "array_elem_" + i, false, gep_idx);

                builder.BuildStore(arg, gep);
            }
            valueStack.Push(arr_struct_ptr);
        }

        public void Visit(AST.VariableDefinition node) {
            if (node.variable.isConstant) {
                Visit(node.expression);
                var v = valueStack.Pop();
                // Debug.Assert(LLVM.IsConstant(v));
                variables[node.variable] = v;
                return;
            }

            if (!builder.context.isGlobal) {
                Debug.Assert(node.expression != null || node.typeString != null);

                SSAType vType;
                Value v;
                if (node.expression != null) {
                    Visit(node.expression);
                    v = valueStack.Pop();
                    vType = v.type;
                } else {
                    v = null;
                    vType = GetTypeRef(typeChecker.GetNodeType(node.typeString));
                }

                Value result;
                if (node.expression != null && node.expression is AST.StructConstructor) {
                    result = v;
                } else if (node.expression != null && node.expression is AST.ArrayConstructor) {
                    result = v;
                } else {
                    var insert = builder.GetInsertBlock();
                    builder.PositionAtEnd(builder.context.currentFunctionContext.vars);
                    result = builder.BuildAlloca(vType, node.variable.name);
                    variables[node.variable] = result;
                    builder.PositionAtEnd(insert);
                    if (v != null) {
                        builder.BuildStore(v, result);
                    }
                }
                if (node.typeString != null && node.typeString.allocationCount > 0) {
                    var insert = builder.GetInsertBlock();
                    builder.PositionAtEnd(builder.context.currentFunctionContext.vars);
                    Debug.Assert(node.expression == null);

                    var ac = new ConstInt(i32_t, (ulong)node.typeString.allocationCount);
                    var et = (vType as PointerType).elementType;

                    var alloc = builder.BuildArrayAlloca(et, ac, "alloca");
                    builder.BuildStore(alloc, result);
                    builder.PositionAtEnd(insert);
                }
                variables[node.variable] = result;
            } else // is global
              {
                if (node.expression != null && node.expression is AST.StructConstructor) {
                    var sc = node.expression as AST.StructConstructor;
                    var structType = GetTypeRef(typeChecker.GetNodeType(sc));

                    var v = builder.AddGlobal(structType, node.variable.name);
                    // LLVM.SetLinkage(v, LLVMLinkage.LLVMInternalLinkage);
                    variables[node.variable] = v;
                    v.SetInitializer(builder.ConstNull(structType));

                    for (int i = 0; i < sc.argumentList.Count; ++i) {
                        Visit(sc.argumentList[i]);
                        var arg = valueStack.Pop();
                        var arg_ptr = builder.BuildStructGEP(v, i, "struct_arg_" + i);
                        builder.BuildStore(arg, arg_ptr);
                    }
                } else if (node.expression is AST.ArrayConstructor) {
                    throw new System.NotImplementedException();
                } else {
                    if (node.expression != null) {
                        Visit(node.expression);
                        var result = valueStack.Pop();
                        var resultType = result.type;
                        var v = builder.AddGlobal(resultType, node.variable.name);
                        variables[node.variable] = v;
                        // LVM.SetLinkage(v, LLVMLinkage.LLVMInternalLinkage);
                        if (result.isConst) {
                            v.SetInitializer(result);
                        } else {

                            v.SetInitializer(builder.ConstNull(resultType));
                            builder.BuildStore(result, v);
                        }
                    } else {
                        var vType = GetTypeRef(typeChecker.GetNodeType(node.typeString));
                        var v = builder.AddGlobal(vType, node.variable.name);
                        variables[node.variable] = v;
                        // LLVM.SetLinkage(v, LLVMLinkage.LLVMInternalLinkage);
                        v.SetInitializer(builder.ConstNull(vType));
                    }
                }
            }
        }

        public void Visit(AST.Assignment node) {
            Visit(node.left);
            var target = valueStack.Pop();
            var targetType = target.type;
            // var targetTypeName = typeToString(targetType);

            Visit(node.right);
            var result = valueStack.Pop();
            var resultType = result.type;
            // var resultTypeName = typeToString(resultType);

            var et = (targetType as PointerType).elementType;
            if (!et.EqualType(resultType)) {
                result = builder.BuildBitCast(result, et, "hmpf");
            }
            builder.BuildStore(result, target);
            valueStack.Push(result);
        }

        public void Visit(AST.Block node) {
            // HACK: DO a prepass with sorting
            foreach (var s in node.statements) {
                if (isConstVariableDefinition(s)) {
                    Visit(s);
                    valueStack.Clear();
                }
            }
            foreach (var s in node.statements) {
                if (!isConstVariableDefinition(s)) {
                    Visit(s);
                    valueStack.Clear();
                }
            }
        }

        public void Visit(AST.VariableReference node) {
            var vd = node.scope.GetVar(node.variableName, node.token);
            // if variable is function paramter just return it immediately
            if (vd.isFunctionParameter) {

                var f = builder.context.currentFunction;
                var pr = builder.GetParam(f, vd.parameterIdx);
                valueStack.Push(pr);
                return;
            }
            var nt = typeChecker.GetNodeType(node);
            var v = variables[vd];
            Value result;
            if (vd.isConstant) {
                result = v;
                // Debug.Assert(LLVM.IsConstant(v));
            } else {
                result = v;
                if (!node.returnPointer) {
                    result = builder.BuildLoad(v, vd.name);
                }
            }
            valueStack.Push(result);
        }

        public void VisitSpecialFunction(AST.FunctionCall node, FrontendFunctionType feft) {
            switch (feft.funName) {
                case "__file_pos__": {
                        var callsite = builder.context.callsite;
                        var s = new AST.ConstString(node.left.token, callsite.scope);
                        var fp = node.left.token.FilePosBackendString();
                        if (callsite != null) {
                            fp = callsite.token.FilePosBackendString();
                        }

                        s.s = fp;
                        Visit(s, false);
                    }
                    break;
            }
        }

        public void Visit(AST.FunctionCall node) {
            var feft = typeChecker.GetNodeType(node.left) as FrontendFunctionType;
            
            if (feft.specialFun) {
                VisitSpecialFunction(node, feft);
                return;
            }
            if (feft.inactiveConditional) {
                return;
            }

            Visit(node.left);
            var f = valueStack.Pop();

            if (!(f is Function)) {
                f = builder.BuildLoad(f, "fun_ptr_load");
            }

            var cnt = feft.parameters.Count; // System.Math.Max(1, feft.parameters.Count);
            Value[] parameters = new Value[cnt];

            var ft = (f.type as PointerType).elementType as FunctionType;
            var rt = ft.returnType;
            var ps = ft.argumentTypes;
            for (int i = 0; i < node.argumentList.Count; ++i) {
                Visit(node.argumentList[i]);
                parameters[i] = valueStack.Pop();

                // HACK: RETHINK THIS NONSENSE SOON
                if (!parameters[i].type.EqualType(ps[i])) {
                    parameters[i] = builder.BuildBitCast(parameters[i], ps[i], "fun_param_hack");
                }
            }



            if (node.argumentList.Count < feft.parameters.Count) {
                var fd = typeChecker.GetFunctionDefinition(feft);
                var fts = fd.typeString.functionTypeString;
                for (int idx = node.argumentList.Count; idx < feft.parameters.Count; ++idx) {
                    builder.context.SetCallsite(node);
                    Visit(fts.parameters[idx].defaultValueExpression);
                    builder.context.SetCallsite(null);
                    parameters[idx] = valueStack.Pop();
                }
            }

            // http://lists.cs.uiuc.edu/pipermail/llvmdev/2008-May/014844.html
            if (rt.kind == TypeKind.Void) {
                builder.BuildCall(f, null, parameters);
            } else {
                var v = builder.BuildCall(f, "fun_call", parameters);
                valueStack.Push(v);
            }
        }

        public void Visit(AST.ReturnFunction node) {
            if (node.expression != null) {
                Visit(node.expression);
                var v = valueStack.Pop();
                builder.BuildRet(v);
            } else {
                builder.BuildRetVoid();
            }
        }

        public void Visit(AST.IfCondition node) {
            Visit(node.condition);
            var condition = valueStack.Pop();
            Debug.Assert(SSAType.IsBoolType(condition.type));

            var insert = builder.GetInsertBlock();

            var thenBlock = builder.AppendBasicBlock("then");
            builder.MoveBasicBlockAfter(thenBlock, insert);

            var lastBlock = thenBlock;
            var elifBlocks = new List<Block>();
            var idx = 0;
            foreach (var elif in node.elifs) {
                var elifBlock = builder.AppendBasicBlock("elif_" + (idx++));
                builder.MoveBasicBlockAfter(elifBlock, lastBlock);
                lastBlock = elifBlock;
                elifBlocks.Add(elifBlock);
            }

            Block elseBlock = null;
            Block endIfBlock = null;
            if (node.elseBlock != null) {
                elseBlock = builder.AppendBasicBlock("else");
                builder.MoveBasicBlockAfter(elseBlock, lastBlock);
                lastBlock = elseBlock;
            }

            endIfBlock = builder.AppendBasicBlock("endif");
            builder.MoveBasicBlockAfter(endIfBlock, lastBlock);
            lastBlock = endIfBlock;

            var nextFail = endIfBlock;
            if (elifBlocks.Count > 0) {
                nextFail = elifBlocks.First();
            } else if (node.elseBlock != null) {
                nextFail = elseBlock;
            }

            builder.BuildCondBr(condition, thenBlock, nextFail);

            builder.PositionAtEnd(thenBlock);
            Visit(node.thenBlock);

            if (!builder.GetInsertBlock().HasTerminator()) {
                builder.BuildBr(endIfBlock);
            }

            for (int i = 0; i < elifBlocks.Count; ++i) {
                var elif = elifBlocks[i];

                var elifThen = builder.AppendBasicBlock("elif_" + i + "_then");
                builder.MoveBasicBlockAfter(elifThen, elif);

                builder.PositionAtEnd(elif);
                var elifNode = node.elifs[i] as AST.Elif;
                Visit(elifNode.condition);
                var elifCond = valueStack.Pop();


                var nextBlock = endIfBlock;
                if (node.elseBlock != null) {
                    nextBlock = elseBlock;
                }
                if (i < elifBlocks.Count - 1) {
                    nextBlock = elifBlocks[i + 1];
                }
                builder.BuildCondBr(elifCond, elifThen, nextBlock);

                builder.PositionAtEnd(elifThen);
                Visit(elifNode.thenBlock);


                if (!builder.GetInsertBlock().HasTerminator()) {
                    builder.BuildBr(endIfBlock);
                }
            }

            if (node.elseBlock != null) {
                builder.PositionAtEnd(elseBlock);
                Visit(node.elseBlock);
                if (!builder.GetInsertBlock().HasTerminator()) {
                    builder.BuildBr(endIfBlock);
                }
            }

            builder.PositionAtEnd(endIfBlock);
        }

        public void Visit(AST.ForLoop node) {
            var insert = builder.GetInsertBlock();

            var loopPre = builder.AppendBasicBlock("for_cond");
            builder.MoveBasicBlockAfter(loopPre, insert);
            var loopBody = builder.AppendBasicBlock("for");
            builder.MoveBasicBlockAfter(loopBody, loopPre);
            var loopIter = builder.AppendBasicBlock("for_iter");
            builder.MoveBasicBlockAfter(loopIter, loopBody);
            var endFor = builder.AppendBasicBlock("end_for");
            builder.MoveBasicBlockAfter(endFor, loopIter);

            foreach (var n in node.initializer) {
                Visit(n);
            }
            builder.BuildBr(loopPre);

            builder.PositionAtEnd(loopPre);
            Visit(node.condition);

            var condition = valueStack.Pop();
            Debug.Assert(SSAType.IsBoolType(condition.type));
            builder.BuildCondBr(condition, loopBody, endFor);
            builder.PositionAtEnd(loopBody);

            builder.context.PushLoop(loopIter, endFor);
            Visit(node.loopBody);
            if (!builder.GetInsertBlock().HasTerminator()) {
                builder.BuildBr(loopIter);
            }
            builder.context.PopLoop();

            builder.PositionAtEnd(loopIter);
            foreach (var n in node.iterator) {
                Visit(n);
            }

            builder.BuildBr(loopPre);
            builder.PositionAtEnd(endFor);
        }

        public void Visit(AST.WhileLoop node) {
            var insert = builder.GetInsertBlock();

            var loopPre = builder.AppendBasicBlock("while_cond");
            builder.MoveBasicBlockAfter(loopPre, insert);
            var loopBody = builder.AppendBasicBlock("while");
            builder.MoveBasicBlockAfter(loopBody, loopPre);
            var loopEnd = builder.AppendBasicBlock("while_end");
            builder.MoveBasicBlockAfter(loopEnd, loopBody);

            builder.BuildBr(loopPre);

            builder.PositionAtEnd(loopPre);
            Visit(node.condition);

            var condition = valueStack.Pop();
            Debug.Assert(SSAType.IsBoolType(condition.type));
            builder.BuildCondBr(condition, loopBody, loopEnd);
            builder.PositionAtEnd(loopBody);

            builder.context.PushLoop(loopPre, loopEnd);
            Visit(node.loopBody);
            builder.context.PopLoop();

            if (!builder.GetInsertBlock().HasTerminator()) {
                builder.BuildBr(loopPre);
            }
            builder.PositionAtEnd(loopEnd);
        }

        void insertMissingReturn(SSAType returnType) {
            if (!builder.GetInsertBlock().HasTerminator()) {
                if (returnType.kind == TypeKind.Void) {
                    builder.BuildRetVoid();
                } else {
                    var dummy = builder.BuildBitCast(zero_i32_v, returnType, "dummy");
                    builder.BuildRet(dummy);
                }
            }
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
                var function = builder.AddFunction(funType, functionName, fun.parameters.Select(p => p.name).ToArray());
                variables.Add(node.variableDefinition, function);
            } else {
                if (node.external || node.body == null) {
                    return;
                }
                //var functionName = node.externalFunctionName != null ? node.externalFunctionName : node.funName;
                //var function = mod.functions[functionName];
                var function = variables[node.variableDefinition] as Function;

                if (node.HasAttribute("DLL.EXPORT")) {
                    function.ExportDLL = true;
                }

                var vars = builder.AppendBasicBlock(function, "vars");
                var entry = builder.AppendBasicBlock(function, "entry");
                builder.context.SetFunctionBlocks(function, vars, entry);
                
                var blockTemp = builder.GetInsertBlock();
                builder.PositionAtEnd(entry);

                if (node.body != null) {
                    Visit(node.body);
                }

                var returnType = GetTypeRef(fun.returnType);
                insertMissingReturn(returnType);

                builder.PositionAtEnd(vars);
                builder.BuildBr(entry);

                builder.PositionAtEnd(blockTemp);
            }
        }


        public void Visit(AST.BreakLoop node) {
            Debug.Assert(builder.context.IsLoop());
            builder.BuildBr(builder.context.PeekLoop().end);
        }

        public void Visit(AST.ContinueLoop node) {
            Debug.Assert(builder.context.IsLoop());
            builder.BuildBr(builder.context.PeekLoop().next);
        }


        public void Visit(AST.ArrayElementAccess node) {
            Visit(node.left);
            var arr = valueStack.Pop();

            Visit(node.index);
            var idx = valueStack.Pop();

            Value arr_elem_ptr;

            // is not function argument?
            if (arr.op != Op.FunctionArgument) {
                var gep_arr_elem_ptr = builder.BuildGEP(arr, "gep_arr_elem_ptr", false, zero_i32_v, one_i32_v);
                arr_elem_ptr = builder.BuildLoad(gep_arr_elem_ptr, "arr_elem_ptr");
            } else {
                arr_elem_ptr = builder.BuildExtractValue(arr, "gep_arr_elem_ptr", one_i32_v);
            }

            var gep_arr_elem = builder.BuildGEP(arr_elem_ptr, "gep_arr_elem", false, idx);
            Value result = gep_arr_elem;

            if (!node.returnPointer) {
                result = builder.BuildLoad(gep_arr_elem, "arr_elem");
            }
            valueStack.Push(result);
        }



        public void Visit(AST.FieldAccess node) {
            Visit(node.left);

            var v = valueStack.Pop();

            FrontendStructType s;
            if (node.IsArrow) {
                s = (typeChecker.GetNodeType(node.left) as FrontendPointerType).elementType
                    as FrontendStructType;
            } else {
                s = typeChecker.GetNodeType(node.left) as FrontendStructType;
            }
            var idx = s.GetFieldIndex(node.fieldName);
            Value gep;

            // is not function argument?
            // assume that when its _NOT_ a pointer then it will be a function argument
            if (!(v.op == Op.FunctionArgument) && v.type.kind == TypeKind.Pointer) {
                if (node.IsArrow) {
                    v = builder.BuildLoad(v, "struct_arrow_load");
                }

                // HACK: we hit limit of recursive type so just perform bitcast
                if ((v.type as PointerType).elementType.kind != TypeKind.Struct) { 
                    var sp = new PointerType(GetTypeRef(s));
                    v = builder.BuildBitCast(v, sp, "hack_bitcast");
                }

                Value result;

                result = builder.BuildGEP(v, "struct_field_ptr", true, zero_i32_v, new ConstInt(i32_t, (ulong)idx));

                var fe_nt = typeChecker.GetNodeType(node);
                var be_nt = new PointerType(GetTypeRef(fe_nt));

                if (!be_nt.EqualType(result.type)) {
                    result = builder.BuildBitCast(result, be_nt, "hack_cast");
                }
                if (!node.returnPointer) {
                    result = builder.BuildLoad(result, "struct_field");
                }
                valueStack.Push(result);

                return;
            } else {
                Value result;
                if (node.IsArrow) {
                    result = builder.BuildGEP(v, "struct_field_ptr", true, zero_i32_v, new ConstInt(i32_t, (ulong)idx));

                    var fe_nt = typeChecker.GetNodeType(node);
                    var be_nt = new PointerType(GetTypeRef(fe_nt));

                    if (!be_nt.EqualType(result.type)) {
                        result = builder.BuildBitCast(result, be_nt, "hack_cast");
                    }
                    if (!node.returnPointer) {
                        result = builder.BuildLoad(result, "struct_arrow");
                    }
                } else {
                    uint[] uindices = { (uint)idx };
                    result = builder.BuildExtractValue(v, "struct_field_extract", new ConstInt(i32_t, (ulong)idx));
                }

                valueStack.Push(result);
                return;
            }
        }

        public void Visit(AST.StructDeclaration node) {
        }


        // TODO(pragma): generate this code
        public void Visit(AST.Node node) {
            switch (node) {
                case AST.ProgramRoot n:
                    Visit(n);
                    break;
                case AST.FileRoot n:
                    Visit(n);
                    break;
                case AST.Namespace n:
                    Visit(n);
                    break;
                case AST.Block n:
                    Visit(n);
                    break;
                case AST.Elif n:
                    Visit(n);
                    break;
                case AST.IfCondition n:
                    Visit(n);
                    break;
                case AST.ForLoop n:
                    Visit(n);
                    break;
                case AST.WhileLoop n:
                    Visit(n);
                    break;
                case AST.VariableDefinition n:
                    Visit(n);
                    break;
                case AST.FunctionDefinition n:
                    Visit(n);
                    break;
                case AST.StructConstructor n:
                    Visit(n);
                    break;
                case AST.StructDeclaration n:
                    Visit(n);
                    break;
                case AST.FunctionCall n:
                    Visit(n);
                    break;
                case AST.VariableReference n:
                    Visit(n);
                    break;
                case AST.Assignment n:
                    Visit(n);
                    break;
                case AST.ConstInt n:
                    Visit(n);
                    break;
                case AST.ConstFloat n:
                    Visit(n);
                    break;
                case AST.ConstBool n:
                    Visit(n);
                    break;
                case AST.ConstString n:
                    Visit(n);
                    break;
                case AST.ArrayConstructor n:
                    Visit(n);
                    break;
                case AST.FieldAccess n:
                    Visit(n);
                    break;
                case AST.ArrayElementAccess n:
                    Visit(n);
                    break;
                case AST.BreakLoop n:
                    Visit(n);
                    break;
                case AST.ContinueLoop n:
                    Visit(n);
                    break;
                case AST.ReturnFunction n:
                    Visit(n);
                    break;
                case AST.BinOp n:
                    Visit(n);
                    break;
                case AST.UnaryOp n:
                    Visit(n);
                    break;
                case AST.TypeCastOp n:
                    Visit(n);
                    break;
                case AST.TypeString n:
                    Visit(n);
                    break;
                default:
                    throw new System.NotImplementedException();
            }
        }

    }
}
