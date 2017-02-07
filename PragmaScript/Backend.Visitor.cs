using LLVMSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PragmaScript
{
    partial class Backend
    {

        static bool isConstVariableDefinition(AST.Node node)
        {

            if (node is AST.VariableDefinition) {
                if ((node as AST.VariableDefinition).variable.isConstant) {
                    return true;
                }
            }
            return false;
        }

        static bool isGlobalVariableDefinition(AST.Node node)
        {

            if (node is AST.VariableDefinition) {
                if ((node as AST.VariableDefinition).variable.isGlobal) {
                    return true;
                }
            }
            return false;
        }


        public void Visit(AST.ProgramRoot node, AST.FunctionDefinition main)
        {
            // HACK:
            AST.FileRoot merge = new AST.FileRoot(Token.Undefined, node.scope);
            foreach (var fr in node.files) {
                foreach (var decl in fr.declarations) {
                    merge.declarations.Add(decl);
                }
            }
            Visit(merge, main);
        }

        public void Visit(AST.FileRoot node, AST.FunctionDefinition main)
        {


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


            var par_t = new LLVMTypeRef[1];
            var returnType = LLVM.VoidType();
            var funType = LLVM.FunctionType(returnType, out par_t[0], (uint)0, Const.FalseBool);

            if (CompilerOptions.dll) {
                par_t = new LLVMTypeRef[3] { Const.mm, Const.Int32Type, Const.Int8PointerType };
                returnType = LLVM.Int32Type();
                funType = LLVM.FunctionType(returnType, out par_t[0], (uint)3, Const.FalseBool);
            }

            var function = LLVM.AddFunction(mod, "__init", funType);

            var vars = LLVM.AppendBasicBlock(function, "vars");
            var entry = LLVM.AppendBasicBlock(function, "entry");

            var blockTemp = LLVM.GetInsertBlock(builder);

            LLVM.PositionBuilderAtEnd(builder, entry);

            ctx.Push(new ExecutionContext(function, "__init", entry, vars, global: true));


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

            LLVM.PositionBuilderAtEnd(builder, entry);

            if (main != null) {
                var mf = variables[main.funName];
                var par = new LLVMValueRef[1];
                LLVM.BuildCall(builder, mf, out par[0], 0, "");
            }

            if (CompilerOptions.dll) {
                LLVM.BuildRet(builder, Const.OneInt32);
            } else {
                LLVM.BuildRetVoid(builder);
            }

            LLVM.PositionBuilderAtEnd(builder, vars);
            LLVM.BuildBr(builder, entry);

            LLVM.PositionBuilderAtEnd(builder, blockTemp);


            LLVM.VerifyFunction(function, LLVMVerifierFailureAction.LLVMPrintMessageAction);

            ctx.Pop();
        }

        public void Visit(AST.Namespace node)
        {
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

        public void Visit(AST.ConstInt node)
        {
            var ct = GetTypeRef(typeChecker.GetNodeType(node));
            LLVMValueRef result;
            if (LLVM.GetTypeKind(ct) == LLVMTypeKind.LLVMFloatTypeKind
                || LLVM.GetTypeKind(ct) == LLVMTypeKind.LLVMDoubleTypeKind) {
                result = LLVM.ConstReal(ct, node.number);
            } else {
                result = LLVM.ConstInt(ct, (ulong)node.number, Const.TrueBool);
            }

            valueStack.Push(result);
        }

        public void Visit(AST.ConstFloat node)
        {
            var ct = GetTypeRef(typeChecker.GetNodeType(node));
            var result = LLVM.ConstReal(ct, node.number);
            valueStack.Push(result);
        }

        public void Visit(AST.ConstBool node)
        {
            var result = node.value ? Const.True : Const.False;
            valueStack.Push(result);
        }

        Dictionary<string, LLVMValueRef> stringTable = new Dictionary<string, LLVMValueRef>();

        public void Visit(AST.ConstString node, bool needsConversion = true)
        {
            var str = node.s;
            if (needsConversion) {
                str = node.ConvertString();
            }

            LLVMValueRef str_ptr = new LLVMValueRef();
            // if (node.scope.function != null)
            {
                if (!stringTable.TryGetValue(str, out str_ptr)) {
                    str_ptr = LLVM.BuildGlobalStringPtr(builder, str, "str");
                    stringTable.Add(str, str_ptr);
                }
            }

            var type = FrontendType.string_;
            var arr_struct_type = GetTypeRef(type);
            var insert = LLVM.GetInsertBlock(builder);
            LLVM.PositionBuilderAtEnd(builder, ctx.Peek().vars);


            var arr_struct_ptr = LLVM.BuildAlloca(builder, arr_struct_type, "arr_struct_alloca");
            var str_length = (uint)str.Length;
            var elem_type = GetTypeRef(type.elementType);
            var size = LLVM.ConstInt(Const.Int32Type, str_length, Const.FalseBool);

            LLVMValueRef arr_elem_ptr = new LLVMValueRef();
            // arr_elem_ptr = LLVM.BuildArrayAlloca(builder, elem_type, size, "arr_elem_alloca");
            if (node.scope.function != null) {
                arr_elem_ptr = LLVM.BuildArrayAlloca(builder, elem_type, size, "arr_elem_alloca");
            } else {
                var at = LLVM.ArrayType(elem_type, str_length);
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

        //public void Visit(AST.UninitializedArray node)
        //{
        //    throw new NotImplementedException();

        //    //var l = node.length;

        //    //var values = new LLVMValueRef[l];
        //    //var et = GetTypeRef(node.elementType);


        //    //for (int i = 0; i < values.Length; ++i)
        //    //{
        //    //    values[i] = LLVM.ConstNull(et);
        //    //}

        //    //var size = LLVM.ConstInt(Const.Int32Type, (ulong)l, Const.FalseBool);
        //    //var arr = LLVM.ConstArray(GetTypeRef(node.elementType), out values[0], (uint)values.Length);


        //    //var sp = new LLVMValueRef[] { size, arr };
        //    //// TODO: does this need to be packed?
        //    //var structure = LLVM.ConstStruct(out sp[0], 2, Const.FalseBool);
        //    //valueStack.Push(structure);
        //}

        public void Visit(AST.BinOp node)
        {
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


            var leftType = LLVM.TypeOf(left);
            var rightType = LLVM.TypeOf(right);


            LLVMValueRef result;
            if (isEqualType(leftType, Const.BoolType)) {
                switch (node.type) {
                    case AST.BinOp.BinOpType.LogicalAND:
                        result = LLVM.BuildAnd(builder, left, right, "and_tmp");
                        break;
                    case AST.BinOp.BinOpType.LogicalOR:
                        result = LLVM.BuildOr(builder, left, right, "or_tmp");
                        break;
                    case AST.BinOp.BinOpType.LogicalXOR:
                        result = LLVM.BuildXor(builder, left, right, "or_tmp");
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
            } else {
                switch (LLVM.GetTypeKind(leftType)) {
                    case LLVMTypeKind.LLVMIntegerTypeKind:
                        switch (node.type) {
                            case AST.BinOp.BinOpType.Add:
                                result = LLVM.BuildAdd(builder, left, right, "add_tmp");
                                break;
                            case AST.BinOp.BinOpType.Subract:
                                result = LLVM.BuildSub(builder, left, right, "sub_tmp");
                                break;
                            case AST.BinOp.BinOpType.Multiply:
                                result = LLVM.BuildMul(builder, left, right, "mul_tmp");
                                break;
                            case AST.BinOp.BinOpType.Divide:
                                result = LLVM.BuildSDiv(builder, left, right, "div_tmp");
                                break;
                            case AST.BinOp.BinOpType.DivideUnsigned:
                                result = LLVM.BuildUDiv(builder, left, right, "div_tmp");
                                break;
                            case AST.BinOp.BinOpType.LeftShift:
                                result = LLVM.BuildShl(builder, left, right, "shl_tmp");
                                break;
                            case AST.BinOp.BinOpType.RightShift:
                                result = LLVM.BuildAShr(builder, left, right, "shr_tmp");
                                break;
                            case AST.BinOp.BinOpType.Remainder:
                                result = LLVM.BuildURem(builder, left, right, "urem_tmp");
                                break;
                            case AST.BinOp.BinOpType.Equal:
                                result = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntEQ, left, right, "icmp_tmp");
                                break;
                            case AST.BinOp.BinOpType.NotEqual:
                                result = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntNE, left, right, "icmp_tmp");
                                break;
                            case AST.BinOp.BinOpType.Greater:
                                result = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntSGT, left, right, "icmp_tmp");
                                break;
                            case AST.BinOp.BinOpType.GreaterEqual:
                                result = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntSGE, left, right, "icmp_tmp");
                                break;
                            case AST.BinOp.BinOpType.Less:
                                result = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntSLT, left, right, "icmp_tmp");
                                break;
                            case AST.BinOp.BinOpType.LessEqual:
                                result = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntSLE, left, right, "icmp_tmp");
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
                            case AST.BinOp.BinOpType.LogicalAND:
                                result = LLVM.BuildAnd(builder, left, right, "and_tmp");
                                break;
                            case AST.BinOp.BinOpType.LogicalOR:
                                result = LLVM.BuildOr(builder, left, right, "or_tmp");
                                break;
                            case AST.BinOp.BinOpType.LogicalXOR:
                                result = LLVM.BuildXor(builder, left, right, "xor_tmp");
                                break;
                            default:
                                throw new InvalidCodePath();
                        }
                        break;
                    case LLVMTypeKind.LLVMDoubleTypeKind:
                    case LLVMTypeKind.LLVMFloatTypeKind:
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

        void visitConditionalOR(AST.BinOp op)
        {
            Visit(op.left);
            var cmp = valueStack.Pop();
            if (!isEqualType(LLVM.TypeOf(cmp), Const.BoolType))
                throw new BackendTypeMismatchException(Const.BoolType, LLVM.TypeOf(cmp));

            var block = LLVM.GetInsertBlock(builder);

            var cor_rhs = LLVM.AppendBasicBlock(ctx.Peek().function, "cor.rhs");
            LLVM.MoveBasicBlockAfter(cor_rhs, block);

            var cor_end = LLVM.AppendBasicBlock(ctx.Peek().function, "cor.end");
            LLVM.MoveBasicBlockAfter(cor_end, cor_rhs);

            LLVM.BuildCondBr(builder, cmp, cor_end, cor_rhs);

            // cor.rhs: 
            LLVM.PositionBuilderAtEnd(builder, cor_rhs);
            Visit(op.right);
            var cor_rhs_tv = valueStack.Pop();
            if (!isEqualType(LLVM.TypeOf(cor_rhs_tv), Const.BoolType))
                throw new BackendTypeMismatchException(Const.BoolType, LLVM.TypeOf(cor_rhs_tv));

            LLVM.BuildBr(builder, cor_end);
            cor_rhs = LLVM.GetInsertBlock(builder);

            // cor.end:
            LLVM.PositionBuilderAtEnd(builder, cor_end);
            var phi = LLVM.BuildPhi(builder, Const.BoolType, "corphi");

            LLVMBasicBlockRef[] incomingBlocks = new LLVMBasicBlockRef[2] { block, cor_rhs };
            LLVMValueRef[] incomingValues = new LLVMValueRef[2] { Const.True, cor_rhs_tv };

            LLVM.AddIncoming(phi, out incomingValues[0], out incomingBlocks[0], 2);

            valueStack.Push(phi);
        }

        void visitConditionalAND(AST.BinOp op)
        {
            Visit(op.left);
            var cmp = valueStack.Pop();
            if (!isEqualType(LLVM.TypeOf(cmp), Const.BoolType))
                throw new BackendTypeMismatchException(Const.BoolType, LLVM.TypeOf(cmp));

            var block = LLVM.GetInsertBlock(builder);

            var cand_rhs = LLVM.AppendBasicBlock(ctx.Peek().function, "cand.rhs");
            LLVM.MoveBasicBlockAfter(cand_rhs, block);

            var cand_end = LLVM.AppendBasicBlock(ctx.Peek().function, "cand.end");
            LLVM.MoveBasicBlockAfter(cand_end, cand_rhs);


            LLVM.BuildCondBr(builder, cmp, cand_rhs, cand_end);

            // cor.rhs: 
            LLVM.PositionBuilderAtEnd(builder, cand_rhs);
            Visit(op.right);
            var cand_rhs_tv = valueStack.Pop();
            if (!isEqualType(LLVM.TypeOf(cand_rhs_tv), Const.BoolType))
                throw new BackendTypeMismatchException(Const.BoolType, LLVM.TypeOf(cand_rhs_tv));

            LLVM.BuildBr(builder, cand_end);
            cand_rhs = LLVM.GetInsertBlock(builder);

            // cor.end:
            LLVM.PositionBuilderAtEnd(builder, cand_end);
            var phi = LLVM.BuildPhi(builder, Const.BoolType, "candphi");

            //var ic = LLVM.CountIncoming(phi);
            //for (int i = 0; i < ic; ++i)
            //{

            //}

            LLVMBasicBlockRef[] incomingBlocks = new LLVMBasicBlockRef[2] { block, cand_rhs };
            LLVMValueRef[] incomingValues = new LLVMValueRef[2] { Const.False, cand_rhs_tv };

            LLVM.AddIncoming(phi, out incomingValues[0], out incomingBlocks[0], 2);

            valueStack.Push(phi);
        }

        public void Visit(AST.UnaryOp node)
        {
            http://stackoverflow.com/questions/14608250/how-can-i-find-the-size-of-a-type
            if (node.type == AST.UnaryOp.UnaryOpType.SizeOf) {
                var fet = typeChecker.GetNodeType(node.expression);
                var et = GetTypeRef(fet);
                //var indices = new LLVMValueRef[] { Const.OneInt32 };
                //var size = LLVM.BuildGEP(builder, LLVM.ConstPointerNull(LLVM.PointerType(et, 0)), out indices[0], 1, "size_of_trick");
                //var size_of = LLVM.BuildPtrToInt(builder, size, Const.mm, "size_of_int");

                valueStack.Push(LLVM.SizeOf(et));
                return;
            }


            Visit(node.expression);

            var v = valueStack.Pop();
            var vtype = LLVM.TypeOf(v);
            var result = default(LLVMValueRef);
            switch (node.type) {
                case AST.UnaryOp.UnaryOpType.Add:
                    result = v;
                    break;
                case AST.UnaryOp.UnaryOpType.Subract:
                    switch (LLVM.GetTypeKind(vtype)) {
                        case LLVMTypeKind.LLVMDoubleTypeKind:
                        case LLVMTypeKind.LLVMFloatTypeKind:
                            result = LLVM.BuildFNeg(builder, v, "fneg_tmp");
                            break;
                        case LLVMTypeKind.LLVMIntegerTypeKind:
                            result = LLVM.BuildNeg(builder, v, "neg_tmp");
                            break;
                        default:
                            throw new InvalidCodePath();
                    }
                    break;
                case AST.UnaryOp.UnaryOpType.LogicalNot:
                    if (!isEqualType(vtype, Const.BoolType)) {
                        throw new BackendTypeMismatchException(vtype, Const.BoolType);
                    }
                    result = LLVM.BuildNot(builder, v, "not_tmp");
                    break;
                case AST.UnaryOp.UnaryOpType.Complement:
                    result = LLVM.BuildXor(builder, v, Const.NegativeOneInt32, "complement_tmp");
                    break;
                case AST.UnaryOp.UnaryOpType.AddressOf:
                    // HACK: for NOW this happens via returnPointer nonsense
                    result = v;
                    if (LLVM.IsAFunction(v).Pointer != IntPtr.Zero) {
                        LLVM.BuildBitCast(builder, v, Const.Int8PointerType, "func_pointer");
                    }
                    break;
                case AST.UnaryOp.UnaryOpType.Dereference:
                    result = v;
                    if (!node.returnPointer) {
                        result = LLVM.BuildLoad(builder, result, "deref");
                    }

                    break;
                case AST.UnaryOp.UnaryOpType.PreInc: {
                        result = LLVM.BuildLoad(builder, v, "preinc_load");
                        var vet = LLVM.GetElementType(vtype);
                        var vet_kind = LLVM.GetTypeKind(vet);
                        switch (vet_kind) {
                            case LLVMTypeKind.LLVMIntegerTypeKind:
                                result = LLVM.BuildAdd(builder, result, LLVM.ConstInt(vet, 1, Const.FalseBool), "preinc");
                                break;
                            case LLVMTypeKind.LLVMDoubleTypeKind:
                            case LLVMTypeKind.LLVMFloatTypeKind:
                                result = LLVM.BuildFAdd(builder, result, LLVM.ConstReal(vet, 1.0), "preinc");
                                break;
                            case LLVMTypeKind.LLVMPointerTypeKind:
                                var indices = new LLVMValueRef[] { Const.OneInt32 };
                                result = LLVM.BuildGEP(builder, result, out indices[0], 1, "ptr_pre_inc");
                                break;
                            default:
                                throw new InvalidCodePath();
                        }
                        LLVM.BuildStore(builder, result, v);
                    }
                    break;
                case AST.UnaryOp.UnaryOpType.PreDec: {
                        result = LLVM.BuildLoad(builder, v, "predec_load");
                        var vet = LLVM.GetElementType(vtype);
                        var vet_kind = LLVM.GetTypeKind(vet);
                        switch (vet_kind) {
                            case LLVMTypeKind.LLVMIntegerTypeKind:
                                result = LLVM.BuildSub(builder, result, LLVM.ConstInt(vet, 1, Const.FalseBool), "predec");
                                break;
                            case LLVMTypeKind.LLVMDoubleTypeKind:
                            case LLVMTypeKind.LLVMFloatTypeKind:
                                result = LLVM.BuildFSub(builder, result, LLVM.ConstReal(vet, 1.0), "predec");
                                break;
                            case LLVMTypeKind.LLVMPointerTypeKind:
                                var indices = new LLVMValueRef[] { Const.NegativeOneInt32 };
                                result = LLVM.BuildGEP(builder, result, out indices[0], 1, "ptr_pre_dec");
                                break;
                            default:
                                throw new InvalidCodePath();
                        }
                        LLVM.BuildStore(builder, result, v);
                    }
                    break;
                case AST.UnaryOp.UnaryOpType.PostInc: {
                        result = LLVM.BuildLoad(builder, v, "postinc_load");
                        var vet = LLVM.GetElementType(vtype);
                        var vet_kind = LLVM.GetTypeKind(vet);
                        switch (vet_kind) {
                            case LLVMTypeKind.LLVMIntegerTypeKind: {
                                    var inc = LLVM.BuildAdd(builder, result, LLVM.ConstInt(vet, 1, Const.FalseBool), "postinc");
                                    LLVM.BuildStore(builder, inc, v);
                                }
                                break;
                            case LLVMTypeKind.LLVMDoubleTypeKind:
                            case LLVMTypeKind.LLVMFloatTypeKind: {
                                    var inc = LLVM.BuildFAdd(builder, result, LLVM.ConstReal(vet, 1.0), "postinc");
                                    LLVM.BuildStore(builder, inc, v);
                                }
                                break;
                            case LLVMTypeKind.LLVMPointerTypeKind: {
                                    var indices = new LLVMValueRef[] { Const.OneInt32 };
                                    var inc = LLVM.BuildGEP(builder, result, out indices[0], 1, "ptr_post_inc");
                                    LLVM.BuildStore(builder, inc, v);
                                }
                                break;
                            default:
                                throw new InvalidCodePath();
                        }
                    }
                    break;
                case AST.UnaryOp.UnaryOpType.PostDec: {
                        result = LLVM.BuildLoad(builder, v, "postdec_load");
                        var vet = LLVM.GetElementType(vtype);
                        var vet_kind = LLVM.GetTypeKind(vet);
                        switch (vet_kind) {
                            case LLVMTypeKind.LLVMIntegerTypeKind: {
                                    var inc = LLVM.BuildSub(builder, result, LLVM.ConstInt(vet, 1, Const.FalseBool), "postdec");
                                    LLVM.BuildStore(builder, inc, v);
                                }
                                break;
                            case LLVMTypeKind.LLVMDoubleTypeKind:
                            case LLVMTypeKind.LLVMFloatTypeKind: {
                                    var inc = LLVM.BuildFSub(builder, result, LLVM.ConstReal(vet, 1.0), "postdec");
                                    LLVM.BuildStore(builder, inc, v);
                                }
                                break;
                            case LLVMTypeKind.LLVMPointerTypeKind: {
                                    var indices = new LLVMValueRef[] { Const.NegativeOneInt32 };
                                    var inc = LLVM.BuildGEP(builder, result, out indices[0], 1, "ptr_post_dec");
                                    LLVM.BuildStore(builder, inc, v);
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

        public void Visit(AST.TypeCastOp node)
        {
            Visit(node.expression);

            var v = valueStack.Pop();
            var vtype = LLVM.TypeOf(v);

            var typeName = node.typeString.ToString(); // node.type.ToString();

            var result = default(LLVMValueRef);
            var targetType = GetTypeRef(typeChecker.GetNodeType(node));

            if (isEqualType(targetType, vtype)) {
                result = v;
                valueStack.Push(result);
                return;
            }

            var ttk = LLVM.GetTypeKind(targetType);
            var vtk = LLVM.GetTypeKind(vtype);
            switch (ttk) {
                case LLVMTypeKind.LLVMIntegerTypeKind:
                    switch (vtk) {
                        case LLVMTypeKind.LLVMIntegerTypeKind:
                            if (LLVM.GetIntTypeWidth(targetType) > LLVM.GetIntTypeWidth(vtype)) {
                                if (!node.unsigned) {
                                    result = LLVM.BuildSExt(builder, v, targetType, "int_cast");
                                } else {
                                    result = LLVM.BuildZExt(builder, v, targetType, "int_cast");
                                }
                            } else if (LLVM.GetIntTypeWidth(targetType) < LLVM.GetIntTypeWidth(vtype)) {
                                result = LLVM.BuildTrunc(builder, v, targetType, "int_trunc");
                            } else if (LLVM.GetIntTypeWidth(targetType) == LLVM.GetIntTypeWidth(vtype)) {
                                result = LLVM.BuildBitCast(builder, v, targetType, "int_bitcast");
                            }
                            break;
                        case LLVMTypeKind.LLVMDoubleTypeKind:
                        case LLVMTypeKind.LLVMFloatTypeKind:
                            if (!node.unsigned) {
                                result = LLVM.BuildFPToSI(builder, v, targetType, "int_cast");
                            } else {
                                result = LLVM.BuildFPToUI(builder, v, targetType, "int_cast");
                            }
                            break;
                        case LLVMTypeKind.LLVMPointerTypeKind:
                            result = LLVM.BuildPtrToInt(builder, v, targetType, "int_cast");
                            break;
                        default:
                            throw new InvalidCodePath();
                    }
                    break;
                case LLVMTypeKind.LLVMDoubleTypeKind:
                case LLVMTypeKind.LLVMFloatTypeKind:
                    switch (vtk) {
                        case LLVMTypeKind.LLVMIntegerTypeKind:
                            if (!node.unsigned) {
                                result = LLVM.BuildSIToFP(builder, v, targetType, "int_to_float_cast");
                            } else {
                                result = LLVM.BuildUIToFP(builder, v, targetType, "int_to_float_cast");
                            }
                            break;
                        case LLVMTypeKind.LLVMDoubleTypeKind:
                        case LLVMTypeKind.LLVMFloatTypeKind:
                            result = LLVM.BuildFPCast(builder, v, targetType, "fp_to_fp_cast");
                            break;
                        default:
                            throw new InvalidCodePath();
                    }
                    break;
                case LLVMTypeKind.LLVMPointerTypeKind:
                    switch (vtk) {
                        case LLVMTypeKind.LLVMIntegerTypeKind:
                            result = LLVM.BuildIntToPtr(builder, v, targetType, "int_to_ptr");
                            break;
                        case LLVMTypeKind.LLVMPointerTypeKind:
                            result = LLVM.BuildBitCast(builder, v, targetType, "pointer_bit_cast");
                            break;
                        default:
                            throw new InvalidCodePath();
                    }
                    break;
                default:
                    throw new InvalidCodePath();

            }
            valueStack.Push(result);
        }



        public void Visit(AST.StructConstructor node)
        {
            var sc = node;
            var sft = typeChecker.GetNodeType(node) as FrontendStructType;
            var structType = GetTypeRef(sft);

            var insert = LLVM.GetInsertBlock(builder);
            LLVM.PositionBuilderAtEnd(builder, ctx.Peek().vars);
            var struct_ptr = LLVM.BuildAlloca(builder, structType, "struct_alloca");
            LLVM.PositionBuilderAtEnd(builder, insert);

            for (int i = 0; i < sft.fields.Count; ++i) {
                if (i < node.argumentList.Count) {
                    Visit(sc.argumentList[i]);
                    var arg = valueStack.Pop();
                    var arg_ptr = LLVM.BuildStructGEP(builder, struct_ptr, (uint)i, "struct_arg_" + i);
                    LLVM.BuildStore(builder, arg, arg_ptr);
                } else {
                    var arg_ptr = LLVM.BuildStructGEP(builder, struct_ptr, (uint)i, "struct_arg_" + i);
                    var pt = LLVM.GetElementType(LLVM.TypeOf(arg_ptr));
                    LLVM.BuildStore(builder, LLVM.ConstNull(pt), arg_ptr);
                }
            }
            valueStack.Push(struct_ptr);
        }

        public void Visit(AST.ArrayConstructor node)
        {
            var ac = node;
            var ac_type = typeChecker.GetNodeType(node) as FrontendArrayType;

            var arr_struct_type = GetTypeRef(ac_type);

            var insert = LLVM.GetInsertBlock(builder);
            LLVM.PositionBuilderAtEnd(builder, ctx.Peek().vars);
            var arr_struct_ptr = LLVM.BuildAlloca(builder, arr_struct_type, "arr_struct_alloca");
            var elem_type = GetTypeRef(ac_type.elementType);
            var size = LLVM.ConstInt(Const.Int32Type, (ulong)ac.elements.Count, Const.FalseBool);
            var arr_elem_ptr = LLVM.BuildArrayAlloca(builder, elem_type, size, "arr_elem_alloca");
            LLVM.PositionBuilderAtEnd(builder, insert);


            // set array length in struct

            var gep_idx_0 = new LLVMValueRef[] { Const.ZeroInt32, Const.ZeroInt32 };
            var gep_arr_length = LLVM.BuildGEP(builder, arr_struct_ptr, out gep_idx_0[0], 2, "gep_arr_elem_ptr");
            LLVM.BuildStore(builder, LLVM.ConstInt(Const.Int32Type, (ulong)ac.elements.Count, true), gep_arr_length);

            // set array elem pointer in struct
            var gep_idx_1 = new LLVMValueRef[] { Const.ZeroInt32, Const.OneInt32 };
            var gep_arr_elem_ptr = LLVM.BuildGEP(builder, arr_struct_ptr, out gep_idx_1[0], 2, "gep_arr_elem_ptr");
            LLVM.BuildStore(builder, arr_elem_ptr, gep_arr_elem_ptr);

            for (int i = 0; i < ac.elements.Count; ++i) {
                var elem = ac.elements[i];
                Visit(elem);
                var arg = valueStack.Pop();
                var arg_type_string = typeToString(LLVM.TypeOf(arg));
                var gep_idx = new LLVMValueRef[] { LLVM.ConstInt(Const.Int32Type, (ulong)i, Const.FalseBool) };
                var gep = LLVM.BuildGEP(builder, arr_elem_ptr, out gep_idx[0], 1, "array_elem_" + i);

                LLVM.BuildStore(builder, arg, gep);
            }

            valueStack.Push(arr_struct_ptr);
        }


        public void Visit(AST.VariableDefinition node)
        {
            if (node.variable.isConstant) {

                Visit(node.expression);
                var v = valueStack.Pop();
                // Debug.Assert(LLVM.IsConstant(v));
                variables[node.variable.name] = v;
                return;
            }

            if (!ctx.Peek().global) {
                Debug.Assert(node.expression != null || node.typeString != null);

                LLVMTypeRef vType;
                LLVMValueRef v;
                if (node.expression != null) {
                    Visit(node.expression);
                    v = valueStack.Pop();
                    vType = LLVM.TypeOf(v);

                } else {
                    v = new LLVMValueRef(IntPtr.Zero);
                    vType = GetTypeRef(typeChecker.GetNodeType(node.typeString));
                }

                LLVMValueRef result;
                if (node.expression != null && node.expression is AST.StructConstructor) {
                    result = v;
                } else if (node.expression != null && node.expression is AST.ArrayConstructor) {
                    result = v;
                } else {
                    var insert = LLVM.GetInsertBlock(builder);
                    LLVM.PositionBuilderAtEnd(builder, ctx.Peek().vars);
                    result = LLVM.BuildAlloca(builder, vType, node.variable.name);
                    variables[node.variable.name] = result;
                    LLVM.PositionBuilderAtEnd(builder, insert);
                    if (v.Pointer != IntPtr.Zero) {
                        LLVM.BuildStore(builder, v, result);
                    }
                }
                if (node.typeString != null && node.typeString.allocationCount > 0) {
                    var insert = LLVM.GetInsertBlock(builder);
                    LLVM.PositionBuilderAtEnd(builder, ctx.Peek().vars);
                    Debug.Assert(node.expression == null);

                    var ac = LLVM.ConstInt(LLVM.Int32Type(), (ulong)node.typeString.allocationCount, false);
                    var et = LLVM.GetElementType(vType);

                    var alloc = LLVM.BuildArrayAlloca(builder, et, ac, "alloca");
                    LLVM.BuildStore(builder, alloc, result);
                    LLVM.PositionBuilderAtEnd(builder, insert);
                }
                variables[node.variable.name] = result;
            } else // is global
              {
                if (node.expression != null && node.expression is AST.StructConstructor) {
                    var sc = node.expression as AST.StructConstructor;
                    var structType = GetTypeRef(typeChecker.GetNodeType(sc));

                    var v = LLVM.AddGlobal(mod, structType, node.variable.name);
                    LLVM.SetLinkage(v, LLVMLinkage.LLVMInternalLinkage);
                    variables[node.variable.name] = v;
                    LLVM.SetInitializer(v, LLVM.ConstNull(structType));

                    for (int i = 0; i < sc.argumentList.Count; ++i) {
                        Visit(sc.argumentList[i]);
                        var arg = valueStack.Pop();
                        var arg_ptr = LLVM.BuildStructGEP(builder, v, (uint)i, "struct_arg_" + i);
                        LLVM.BuildStore(builder, arg, arg_ptr);
                    }
                } else if (node.expression is AST.ArrayConstructor) {
                    throw new NotImplementedException();
                } else {
                    if (node.expression != null) {
                        Visit(node.expression);
                        var result = valueStack.Pop();
                        var resultType = LLVM.TypeOf(result);
                        var v = LLVM.AddGlobal(mod, resultType, node.variable.name);
                        variables[node.variable.name] = v;
                        LLVM.SetLinkage(v, LLVMLinkage.LLVMInternalLinkage);
                        if (LLVM.IsConstant(result)) {
                            LLVM.SetInitializer(v, result);
                        } else {

                            LLVM.SetInitializer(v, LLVM.ConstNull(resultType));
                            LLVM.BuildStore(builder, result, v);
                        }
                    } else {
                        var vType = GetTypeRef(typeChecker.GetNodeType(node.typeString));
                        var v = LLVM.AddGlobal(mod, vType, node.variable.name);
                        variables[node.variable.name] = v;
                        LLVM.SetLinkage(v, LLVMLinkage.LLVMInternalLinkage);
                        LLVM.SetInitializer(v, LLVM.ConstNull(vType));
                    }
                }
            }
        }

        public void Visit(AST.Assignment node)
        {
            Visit(node.left);
            var target = valueStack.Pop();
            var targetType = LLVM.TypeOf(target);
            var targetTypeName = typeToString(targetType);

            Visit(node.right);
            var result = valueStack.Pop();
            var resultType = LLVM.TypeOf(result);
            var resultTypeName = typeToString(resultType);

            var et = LLVM.GetElementType(targetType);
            if (et.Pointer != resultType.Pointer) {
                result = LLVM.BuildBitCast(builder, result, LLVM.GetElementType(targetType), "hmpf");
            }
            LLVM.BuildStore(builder, result, target);

            valueStack.Push(result);
        }


        public void Visit(AST.Block node)
        {
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


        public void Visit(AST.VariableReference node)
        {

            var vd = node.scope.GetVar(node.variableName, node.token);
            // if variable is function paramter just return it immediately
            if (vd.isFunctionParameter) {
                var pr = LLVM.GetParam(ctx.Peek().function, (uint)vd.parameterIdx);
                valueStack.Push(pr);
                return;
            }
            LLVMValueRef v;
            var nt = typeChecker.GetNodeType(node);
            v = variables[node.variableName];
            var v_type = typeToString(GetTypeRef(nt));
            LLVMValueRef result;
            bool is_global = LLVM.IsAGlobalVariable(v).Pointer != IntPtr.Zero;

            if (vd.isConstant) {
                result = v;
                // Debug.Assert(LLVM.IsConstant(v));
            } else {
                result = v;
                if (!node.returnPointer) {
                    result = LLVM.BuildLoad(builder, v, vd.name);
                }
            }
            var ltype = LLVM.TypeOf(result);
            var ltype_string = typeToString(ltype);
            valueStack.Push(result);
        }

        public void VisitSpecialFunction(AST.FunctionCall node, FrontendFunctionType feft)
        {
            switch (feft.funName) {
                case "__file_pos__": {
                        var callsite = ctx.Peek().defaultParameterCallsite;
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

        public void Visit(AST.FunctionCall node)
        {
            if (node.token.Line == 55) {
                int breakHere = 42;
            }
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

            if (LLVM.IsAFunction(f).Pointer == IntPtr.Zero) {

                f = LLVM.BuildLoad(builder, f, "fun_ptr_load");

            }
            var cnt = feft.parameters.Count;
            LLVMValueRef[] parameters = new LLVMValueRef[Math.Max(1, cnt)];


            var ft = LLVM.TypeOf(f);
            var rt = LLVM.GetReturnType(LLVM.GetElementType(ft));
            var ps = LLVM.GetElementType(ft).GetParamTypes();
            for (int i = 0; i < node.argumentList.Count; ++i) {
                Visit(node.argumentList[i]);
                parameters[i] = valueStack.Pop();
                // HACK: RETHINK THIS NONSENSE SOON
                if (LLVM.TypeOf(parameters[i]).Pointer != ps[i].Pointer) {
                    parameters[i] = LLVM.BuildBitCast(builder, parameters[i], ps[i], "fun_param_hack");
                }
            }

            if (node.argumentList.Count < feft.parameters.Count) {
                var fd = typeChecker.GetFunctionDefinition(feft);
                var fts = fd.typeString.functionTypeString;
                for (int idx = node.argumentList.Count; idx < feft.parameters.Count; ++idx) {
                    ctx.Peek().defaultParameterCallsite = node;
                    Visit(fts.parameters[idx].defaultValueExpression);
                    ctx.Peek().defaultParameterCallsite = null;
                    parameters[idx] = valueStack.Pop();
                }
            }


            // http://lists.cs.uiuc.edu/pipermail/llvmdev/2008-May/014844.html
            if (isEqualType(rt, Const.VoidType)) {
                LLVM.BuildCall(builder, f, out parameters[0], (uint)cnt, "");
            } else {
                var v = LLVM.BuildCall(builder, f, out parameters[0], (uint)cnt, "fun_call");
                valueStack.Push(v);
            }
        }

        public void Visit(AST.ReturnFunction node)
        {
            if (node.expression != null) {
                Visit(node.expression);
                var v = valueStack.Pop();
                LLVM.BuildRet(builder, v);
            } else {
                LLVM.BuildRetVoid(builder);
            }
        }

        public void Visit(AST.IfCondition node)
        {
            Visit(node.condition);
            var condition = valueStack.Pop();
            if (!isEqualType(LLVM.TypeOf(condition), Const.BoolType)) {
                throw new BackendTypeMismatchException(LLVM.TypeOf(condition), Const.BoolType);
            }

            var insert = LLVM.GetInsertBlock(builder);

            var thenBlock = LLVM.AppendBasicBlock(ctx.Peek().function, "then");
            LLVM.MoveBasicBlockAfter(thenBlock, insert);


            var lastBlock = thenBlock;
            List<LLVMBasicBlockRef> elifBlocks = new List<LLVMBasicBlockRef>();
            var idx = 0;
            foreach (var elif in node.elifs) {
                var elifBlock = LLVM.AppendBasicBlock(ctx.Peek().function, "elif_" + (idx++));
                LLVM.MoveBasicBlockAfter(elifBlock, lastBlock);
                lastBlock = elifBlock;
                elifBlocks.Add(elifBlock);
            }

            var elseBlock = default(LLVMBasicBlockRef);
            var endIfBlock = default(LLVMBasicBlockRef);
            if (node.elseBlock != null) {
                elseBlock = LLVM.AppendBasicBlock(ctx.Peek().function, "else");
                LLVM.MoveBasicBlockAfter(elseBlock, lastBlock);
                lastBlock = elseBlock;
            }

            endIfBlock = LLVM.AppendBasicBlock(ctx.Peek().function, "endif");
            LLVM.MoveBasicBlockAfter(endIfBlock, lastBlock);
            lastBlock = endIfBlock;

            var nextFail = endIfBlock;
            if (elifBlocks.Count > 0) {
                nextFail = elifBlocks.First();
            } else if (node.elseBlock != null) {
                nextFail = elseBlock;
            }

            LLVM.BuildCondBr(builder, condition, thenBlock, nextFail);

            LLVM.PositionBuilderAtEnd(builder, thenBlock);
            Visit(node.thenBlock);

            var term = LLVM.GetBasicBlockTerminator(LLVM.GetInsertBlock(builder));
            if (term.Pointer == IntPtr.Zero) {
                LLVM.BuildBr(builder, endIfBlock);
            }


            for (int i = 0; i < elifBlocks.Count; ++i) {
                var elif = elifBlocks[i];

                var elifThen = LLVM.AppendBasicBlock(ctx.Peek().function, "elif_" + i + "_then");
                LLVM.MoveBasicBlockAfter(elifThen, elif);

                LLVM.PositionBuilderAtEnd(builder, elif);
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
                LLVM.BuildCondBr(builder, elifCond, elifThen, nextBlock);

                LLVM.PositionBuilderAtEnd(builder, elifThen);
                Visit(elifNode.thenBlock);

                term = LLVM.GetBasicBlockTerminator(LLVM.GetInsertBlock(builder));
                if (term.Pointer == IntPtr.Zero) {
                    LLVM.BuildBr(builder, endIfBlock);
                }
            }

            if (node.elseBlock != null) {
                LLVM.PositionBuilderAtEnd(builder, elseBlock);
                Visit(node.elseBlock);
                term = LLVM.GetBasicBlockTerminator(LLVM.GetInsertBlock(builder));
                if (term.Pointer == IntPtr.Zero)
                    LLVM.BuildBr(builder, endIfBlock);
            }

            LLVM.PositionBuilderAtEnd(builder, endIfBlock);
        }

        public void Visit(AST.ForLoop node)
        {
            var insert = LLVM.GetInsertBlock(builder);

            var loopPre = LLVM.AppendBasicBlock(ctx.Peek().function, "for_cond");
            LLVM.MoveBasicBlockAfter(loopPre, insert);
            var loopBody = LLVM.AppendBasicBlock(ctx.Peek().function, "for");
            LLVM.MoveBasicBlockAfter(loopBody, loopPre);
            var loopIter = LLVM.AppendBasicBlock(ctx.Peek().function, "for_iter");
            LLVM.MoveBasicBlockAfter(loopIter, loopBody);
            var endFor = LLVM.AppendBasicBlock(ctx.Peek().function, "end_for");
            LLVM.MoveBasicBlockAfter(endFor, loopIter);

            foreach (var n in node.initializer) {
                Visit(n);
            }
            LLVM.BuildBr(builder, loopPre);

            LLVM.PositionBuilderAtEnd(builder, loopPre);
            Visit(node.condition);

            var condition = valueStack.Pop();
            if (!isEqualType(LLVM.TypeOf(condition), Const.BoolType)) {
                throw new BackendTypeMismatchException(LLVM.TypeOf(condition), Const.BoolType);
            }
            LLVM.BuildCondBr(builder, condition, loopBody, endFor);
            LLVM.PositionBuilderAtEnd(builder, loopBody);

            ctx.Push(new ExecutionContext(ctx.Peek()) { loop = true, loopNext = loopIter, loopEnd = endFor });

            Visit(node.loopBody);
            var term = LLVM.GetBasicBlockTerminator(LLVM.GetInsertBlock(builder));
            if (term.Pointer == IntPtr.Zero)
                LLVM.BuildBr(builder, loopIter);

            ctx.Pop();

            LLVM.PositionBuilderAtEnd(builder, loopIter);
            foreach (var n in node.iterator) {
                Visit(n);
            }

            LLVM.BuildBr(builder, loopPre);

            LLVM.PositionBuilderAtEnd(builder, endFor);
        }

        public void Visit(AST.WhileLoop node)
        {
            var insert = LLVM.GetInsertBlock(builder);

            var loopPre = LLVM.AppendBasicBlock(ctx.Peek().function, "while_cond");
            LLVM.MoveBasicBlockAfter(loopPre, insert);
            var loopBody = LLVM.AppendBasicBlock(ctx.Peek().function, "while");
            LLVM.MoveBasicBlockAfter(loopBody, loopPre);
            var loopEnd = LLVM.AppendBasicBlock(ctx.Peek().function, "while_end");
            LLVM.MoveBasicBlockAfter(loopEnd, loopBody);

            LLVM.BuildBr(builder, loopPre);

            LLVM.PositionBuilderAtEnd(builder, loopPre);
            Visit(node.condition);

            var condition = valueStack.Pop();
            if (!isEqualType(LLVM.TypeOf(condition), Const.BoolType)) {
                throw new BackendTypeMismatchException(LLVM.TypeOf(condition), Const.BoolType);
            }
            LLVM.BuildCondBr(builder, condition, loopBody, loopEnd);
            LLVM.PositionBuilderAtEnd(builder, loopBody);

            ctx.Push(new ExecutionContext(ctx.Peek()) { loop = true, loopNext = loopPre, loopEnd = loopEnd });

            Visit(node.loopBody);

            ctx.Pop();

            var term = LLVM.GetBasicBlockTerminator(LLVM.GetInsertBlock(builder));
            if (term.Pointer == IntPtr.Zero)
                LLVM.BuildBr(builder, loopPre);

            LLVM.PositionBuilderAtEnd(builder, loopEnd);
        }

        public void Visit(AST.FunctionDefinition node, bool proto = false)
        {
            var fun = typeChecker.GetNodeType(node) as FrontendFunctionType;
            if (fun.inactiveConditional) {
                return;
            }
            if (proto) {
                if (node.external && variables.ContainsKey(node.funName) || node.isFunctionTypeDeclaration()) {
                    return;
                }
                var funType = LLVM.GetElementType(GetTypeRef(fun));

                Debug.Assert(!variables.ContainsKey(node.funName));
                var function = LLVM.AddFunction(mod, node.funName, funType);



                LLVM.AddFunctionAttr(function, LLVMAttribute.LLVMNoUnwindAttribute);
                for (int i = 0; i < fun.parameters.Count; ++i) {
                    LLVMValueRef param = LLVM.GetParam(function, (uint)i);
                    LLVM.SetValueName(param, fun.parameters[i].name);
                    //var p = node.typeString.functionTypeString.parameters[i].typeString;
                    //if (p.HasAttribute("LLVM.NOCAPTURE"))
                    //{
                    //    LLVM.AddAttribute(param, LLVMAttribute.LLVMNoCaptureAttribute);
                    //}
                    // variables.Add(fun.parameters[i].name, new TypedValue(param, TypedValue.MapType(fun.parameters[i].type)));
                }
                variables.Add(node.funName, function);
            } else {
                if (node.external || node.body == null) {
                    return;
                }
                var function = variables[node.funName];


                if (node.HasAttribute("DLL.EXPORT")) {
                    LLVM.SetDLLStorageClass(function, LLVMDLLStorageClass.LLVMDLLExportStorageClass);
                } else {
                    LLVM.SetLinkage(function, LLVMLinkage.LLVMInternalLinkage);
                }

                var vars = LLVM.AppendBasicBlock(function, "vars");
                var entry = LLVM.AppendBasicBlock(function, "entry");

                var blockTemp = LLVM.GetInsertBlock(builder);

                LLVM.PositionBuilderAtEnd(builder, entry);
                ctx.Push(new ExecutionContext(function, node.funName, entry, vars));

                if (node.body != null) {
                    Visit(node.body);
                }

                var returnType = GetTypeRef(fun.returnType);
                insertMissingReturn(returnType);

                LLVM.PositionBuilderAtEnd(builder, vars);
                LLVM.BuildBr(builder, entry);

                LLVM.PositionBuilderAtEnd(builder, blockTemp);

                // var dib = LLVM.NewDIBuilder(mod);
                //var metas = new LLVMValueRef[] { LLVM.MDString("file.prag", (uint)"file.prag".Length), LLVM.MDString("file.prag", (uint)"file.prag".Length) };
                //var mdnode = LLVM.MDNodeInContext(LLVM.GetModuleContext(mod), out metas[0], 2);
                //LLVM.SetMetadata(function, 0, mdnode);

                // LLVM.VerifyFunction(function, LLVMVerifierFailureAction.LLVMPrintMessageAction);

                ctx.Pop();
            }
        }

        public void Visit(AST.BreakLoop node)
        {
            if (!ctx.Peek().loop) {
                throw new BackendException("break statement outside of loop not allowed");
            }
            LLVM.BuildBr(builder, ctx.Peek().loopEnd);
        }

        public void Visit(AST.ContinueLoop node)
        {
            if (!ctx.Peek().loop) {
                throw new BackendException("break statement outside of loop not allowed");
            }
            LLVM.BuildBr(builder, ctx.Peek().loopNext);
        }

        public void Visit(AST.ArrayElementAccess node)
        {
            Visit(node.left);
            var arr = valueStack.Pop();
            var arr_type = typeToString(LLVM.TypeOf(arr));

            Visit(node.index);
            var idx = valueStack.Pop();

            LLVMValueRef arr_elem_ptr;

            // is not function argument?
            if (LLVM.IsAArgument(arr).Pointer == IntPtr.Zero) {
                var gep_idx_0 = new LLVMValueRef[] { Const.ZeroInt32, Const.OneInt32 };
                var gep_arr_elem_ptr = LLVM.BuildGEP(builder, arr, out gep_idx_0[0], 2, "gep_arr_elem_ptr");
                arr_elem_ptr = LLVM.BuildLoad(builder, gep_arr_elem_ptr, "arr_elem_ptr");
            } else {
                arr_elem_ptr = LLVM.BuildExtractValue(builder, arr, (uint)1, "gep_arr_elem_ptr");
            }

            var gep_idx_1 = new LLVMValueRef[] { idx };
            var gep_arr_elem = LLVM.BuildGEP(builder, arr_elem_ptr, out gep_idx_1[0], 1, "gep_arr_elem");


            var result = gep_arr_elem;
            if (!node.returnPointer) {
                result = LLVM.BuildLoad(builder, gep_arr_elem, "arr_elem");
            }
            valueStack.Push(result);
        }

        public void Visit(AST.FieldAccess node)
        {

            Visit(node.left);

            var v = valueStack.Pop();
            var v_type = typeToString(LLVM.TypeOf(v));

            FrontendStructType s;
            if (node.IsArrow) {
                s = (typeChecker.GetNodeType(node.left) as FrontendPointerType).elementType
                    as FrontendStructType;
            } else {
                s = typeChecker.GetNodeType(node.left) as FrontendStructType;
            }
            var idx = s.GetFieldIndex(node.fieldName);
            LLVMValueRef gep;



            // is not function argument?
            // assume that when its _NOT_ a pointer then it will be a function argument
            if (LLVM.IsAArgument(v).Pointer == IntPtr.Zero && LLVM.GetTypeKind(LLVM.TypeOf(v)) == LLVMTypeKind.LLVMPointerTypeKind) {
                if (node.IsArrow) {
                    v = LLVM.BuildLoad(builder, v, "struct_arrow_load");
                }

                // HACK: we hit limit of recursive type so just perform bitcast
                if (LLVM.GetTypeKind(LLVM.GetElementType(LLVM.TypeOf(v))) != LLVMTypeKind.LLVMStructTypeKind) {
                    var sp = LLVM.PointerType(GetTypeRef(s), 0);
                    v = LLVM.BuildBitCast(builder, v, sp, "hack_bitcast");
                }


                LLVMValueRef result;
                var indices = new LLVMValueRef[] { Const.ZeroInt32, LLVM.ConstInt(Const.Int32Type, (ulong)idx, Const.FalseBool) };
                gep = LLVM.BuildInBoundsGEP(builder, v, out indices[0], 2, "struct_field_ptr");

                result = gep;
                if (!node.returnPointer) {
                    result = LLVM.BuildLoad(builder, gep, "struct_field");
                }
                valueStack.Push(result);

                return;
            } else {
                LLVMValueRef result;
                if (node.IsArrow) {
                    var indices = new LLVMValueRef[] { Const.ZeroInt32, LLVM.ConstInt(Const.Int32Type, (ulong)idx, Const.FalseBool) };
                    result = LLVM.BuildInBoundsGEP(builder, v, out indices[0], 2, "struct_field_ptr");
                    if (!node.returnPointer) {
                        result = LLVM.BuildLoad(builder, result, "struct_arrow");
                    }
                    var result_type_name = typeToString(LLVM.TypeOf(result));
                } else {
                    uint[] uindices = { (uint)idx };
                    result = LLVM.BuildExtractValue(builder, v, (uint)idx, "struct_field_extract");
                }

                valueStack.Push(result);
                return;
            }
        }


        public void Visit(AST.StructDeclaration node)
        {
        }


        public void Visit(AST.Node node)
        {
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
                    throw new NotImplementedException();
            }
        }
    }
}
