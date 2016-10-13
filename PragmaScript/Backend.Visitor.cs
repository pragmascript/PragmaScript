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

        public void Visit(AST.Root node)
        {

            var par_t = new LLVMTypeRef[1];
            var returnType = LLVM.VoidType();

            var funType = LLVM.FunctionType(returnType, out par_t[0], (uint)0, Const.FalseBool);
            var function = LLVM.AddFunction(mod, "__init", funType);

            var vars = LLVM.AppendBasicBlock(function, "vars");
            var entry = LLVM.AppendBasicBlock(function, "entry");

            var blockTemp = LLVM.GetInsertBlock(builder);

            LLVM.PositionBuilderAtEnd(builder, entry);

            ctx.Push(new ExecutionContext(function, "__init", entry, vars, global: true));

            // TODO: call main:
            foreach (var decl in node.declarations)
            {
                Visit(decl);
            }



            LLVM.PositionBuilderAtEnd(builder, entry);
            Debug.Assert(functions.ContainsKey("main"));
            var mf = functions["main"];
            var par = new LLVMValueRef[1];
            LLVM.BuildCall(builder, mf, out par[0], 0, "");

            insertMissingReturn(returnType);

            LLVM.PositionBuilderAtEnd(builder, vars);
            LLVM.BuildBr(builder, entry);

            LLVM.PositionBuilderAtEnd(builder, blockTemp);
            LLVM.VerifyFunction(function, LLVMVerifierFailureAction.LLVMPrintMessageAction);

            ctx.Pop();
        }

        public void Visit(AST.ConstInt32 node)
        {
            var result = LLVM.ConstInt(Const.Int32Type, (ulong)node.number, Const.TrueBool);
            valueStack.Push(result);
        }

        public void Visit(AST.ConstFloat32 node)
        {
            var result = LLVM.ConstReal(Const.Float32Type, node.number);
            valueStack.Push(result);
        }

        public void Visit(AST.ConstBool node)
        {
            var result = node.value ? Const.True : Const.False;
            valueStack.Push(result);
        }



        public static string ParseString(string txt, Token t)
        {
            StringBuilder result = new StringBuilder(txt.Length);
            int idx = 0;
            while (idx < txt.Length)
            {
                if (txt[idx] != '\\')
                {
                    result.Append(txt[idx]);
                }
                else
                {
                    idx++;
                    Debug.Assert(idx < txt.Length);
                    // TODO: finish escape sequences
                    // https://msdn.microsoft.com/en-us/library/h21280bw.aspx
                    switch (txt[idx])
                    {
                        case '\\':
                            result.Append('\\');
                            break;
                        case 'n':
                            result.Append('\n');
                            break;
                        case 't':
                            result.Append('\t');
                            break;
                        case '"':
                            result.Append('"');
                            break;
                        case '0':
                            result.Append('\0');
                            break;
                    }
                }
                idx++;
            }
            return result.ToString();
        }

        string convertString(string s, Token t)
        {
            var tmp = s.Substring(1, s.Length - 2);
            return ParseString(tmp, t);
        }

        //public void Visit(AST.ConstString node)
        //{
        //    var str = convertString(node.s);
        //    var constString = LLVM.BuildGlobalStringPtr(builder, str, "str");
        //    var result = LLVM.BuildBitCast(builder, constString, Const.Int8PointerType, "str_ptr");
        //    valueStack.Push(result);
        //}

        public void Visit(AST.ConstString node)
        {
            var str = convertString(node.s, node.token);
            var bytes = System.Text.ASCIIEncoding.ASCII.GetBytes(str);

            var type = FrontendType.string_;

            var arr_struct_type = getTypeRef(type);

            var insert = LLVM.GetInsertBlock(builder);
            LLVM.PositionBuilderAtEnd(builder, ctx.Peek().vars);

            var arr_struct_ptr = LLVM.BuildAlloca(builder, arr_struct_type, "arr_struct_alloca");
            var elem_type = getTypeRef(type.elementType);

            var size = LLVM.ConstInt(Const.Int32Type, (ulong)bytes.Length, Const.FalseBool);
            var arr_elem_ptr = LLVM.BuildArrayAlloca(builder, elem_type, size, "arr_elem_alloca");


            LLVM.PositionBuilderAtEnd(builder, insert);

            // set array length in struct
            var gep_idx_0 = new LLVMValueRef[] { Const.ZeroInt32, Const.ZeroInt32 };
            var gep_arr_length = LLVM.BuildGEP(builder, arr_struct_ptr, out gep_idx_0[0], 2, "gep_arr_elem_ptr");
            LLVM.BuildStore(builder, LLVM.ConstInt(Const.Int32Type, (ulong)bytes.Length, true), gep_arr_length);

            // set array elem pointer in struct
            var gep_idx_1 = new LLVMValueRef[] { Const.ZeroInt32, Const.OneInt32 };
            var gep_arr_elem_ptr = LLVM.BuildGEP(builder, arr_struct_ptr, out gep_idx_1[0], 2, "gep_arr_elem_ptr");
            LLVM.BuildStore(builder, arr_elem_ptr, gep_arr_elem_ptr);


            for (int i = 0; i < bytes.Length; ++i)
            {
                var c = bytes[i];
                var gep_idx = new LLVMValueRef[] { LLVM.ConstInt(Const.Int32Type, (ulong)i, Const.FalseBool) };
                var gep = LLVM.BuildGEP(builder, arr_elem_ptr, out gep_idx[0], 1, "array_elem_" + i);

                LLVM.BuildStore(builder, LLVM.ConstInt(Const.Int8Type, (ulong)c, true), gep);
            }

            var arr_struct = LLVM.BuildLoad(builder, arr_struct_ptr, "arr_struct_load");

            valueStack.Push(arr_struct);
            // TODO: use memcopy intrinsic here use
            // http://stackoverflow.com/questions/27681500/generate-call-to-intrinsic-using-llvm-c-api
            // with
            // http://llvm.org/docs/LangRef.html#standard-c-library-intrinsics

        }

        public void Visit(AST.UninitializedArray node)
        {
            var l = node.length;

            var values = new LLVMValueRef[l];
            var et = getTypeRef(node.elementType);


            for (int i = 0; i < values.Length; ++i)
            {
                values[i] = LLVM.ConstNull(et);
            }

            var size = LLVM.ConstInt(Const.Int32Type, (ulong)l, Const.FalseBool);
            var arr = LLVM.ConstArray(getTypeRef(node.elementType), out values[0], (uint)values.Length);


            var sp = new LLVMValueRef[] { size, arr };
            // TODO: does this need to be packed?
            var structure = LLVM.ConstStruct(out sp[0], 2, Const.FalseBool);
            valueStack.Push(structure);
        }

        public void Visit(AST.BinOp node)
        {
            if (node.type == AST.BinOp.BinOpType.ConditionalOR)
            {
                visitConditionalOR(node);
                return;
            }
            if (node.type == AST.BinOp.BinOpType.ConditionaAND)
            {
                visitConditionalAND(node);
                return;
            }

            Visit(node.left);
            var left = valueStack.Pop();
            Visit(node.right);
            var right = valueStack.Pop();


            var leftType = LLVM.TypeOf(left);
            var rightType = LLVM.TypeOf(right);


            if (!isEqualType(leftType, rightType))
            {
                throw new BackendTypeMismatchException(leftType, rightType);
            }

            LLVMValueRef result;
            if (isEqualType(leftType, Const.BoolType))
            {
                switch (node.type)
                {
                    case AST.BinOp.BinOpType.LogicalAND:
                        result = LLVM.BuildAnd(builder, left, right, "and_tmp");
                        break;
                    case AST.BinOp.BinOpType.LogicalOR:
                        result = LLVM.BuildOr(builder, left, right, "or_tmp");
                        break;
                    case AST.BinOp.BinOpType.LogicalXOR:
                        result = LLVM.BuildXor(builder, left, right, "or_tmp");
                        break;

                    default:
                        throw new InvalidCodePath();
                }
            }
            else if (isEqualType(leftType, Const.Int32Type))
            {
                switch (node.type)
                {
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
                    case AST.BinOp.BinOpType.LeftShift:
                        result = LLVM.BuildShl(builder, left, right, "shl_tmp");
                        break;
                    case AST.BinOp.BinOpType.RightShift:
                        result = LLVM.BuildAShr(builder, left, right, "shr_tmp");
                        break;
                    case AST.BinOp.BinOpType.Remainder:
                        result = LLVM.BuildSRem(builder, left, right, "srem_tmp");
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
                    default:
                        throw new InvalidCodePath();
                }
            }
            else if (isEqualType(leftType, Const.Float32Type))
            {
                switch (node.type)
                {
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
            }
            else
            {
                throw new InvalidCodePath();
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

            // cor.end:
            LLVM.PositionBuilderAtEnd(builder, cand_end);
            var phi = LLVM.BuildPhi(builder, Const.BoolType, "candphi");

            LLVMBasicBlockRef[] incomingBlocks = new LLVMBasicBlockRef[2] { block, cand_rhs };
            LLVMValueRef[] incomingValues = new LLVMValueRef[2] { Const.False, cand_rhs_tv };

            LLVM.AddIncoming(phi, out incomingValues[0], out incomingBlocks[0], 2);

            valueStack.Push(phi);
        }

        public void Visit(AST.UnaryOp node)
        {
            Visit(node.expression);

            var v = valueStack.Pop();
            var vtype = LLVM.TypeOf(v);
            var result = default(LLVMValueRef);
            switch (node.type)
            {
                case AST.UnaryOp.UnaryOpType.Add:
                    result = v;
                    break;
                case AST.UnaryOp.UnaryOpType.Subract:
                    if (isEqualType(vtype, Const.Float32Type))
                    {
                        result = LLVM.BuildFNeg(builder, v, "fneg_tmp");
                    }
                    else if (isEqualType(vtype, Const.Int32Type))
                    {
                        result = LLVM.BuildNeg(builder, v, "neg_tmp");
                    }
                    else
                    {
                        throw new BackendException("unary subtract is not defined on type " + typeToString(vtype));
                    }
                    break;
                case AST.UnaryOp.UnaryOpType.LogicalNOT:
                    if (!isEqualType(vtype, Const.BoolType))
                    {
                        throw new BackendTypeMismatchException(vtype, Const.BoolType);
                    }
                    result = LLVM.BuildNot(builder, v, "not_tmp");
                    break;
                case AST.UnaryOp.UnaryOpType.Complement:
                    result = LLVM.BuildXor(builder, v, Const.NegativeOneInt32, "complement_tmp");
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

            var typeName = node.type.ToString();

            var result = default(LLVMValueRef);
            var targetType = getTypeRef(node.type);

            if (isEqualType(targetType, vtype))
            {
                result = v;
                valueStack.Push(result);
                return;
            }

            // TODO: check if integral type
            // TODO: handle non integral types
            if (LLVM.GetTypeKind(targetType) == LLVMTypeKind.LLVMIntegerTypeKind)
            {
                if (LLVM.GetTypeKind(vtype) == LLVMTypeKind.LLVMIntegerTypeKind)
                {
                    if (LLVM.GetIntTypeWidth(targetType) > LLVM.GetIntTypeWidth(vtype))
                    {
                        result = LLVM.BuildZExt(builder, v, targetType, "int_cast");
                    }
                    else if (LLVM.GetIntTypeWidth(targetType) < LLVM.GetIntTypeWidth(vtype))
                    {
                        result = LLVM.BuildTrunc(builder, v, targetType, "int_trunc");
                    }
                    else if (LLVM.GetIntTypeWidth(targetType) == LLVM.GetIntTypeWidth(vtype))
                    {
                        result = LLVM.BuildBitCast(builder, v, targetType, "int_bitcast");
                    }
                }
                // TODO: support different float widths
                else if (isEqualType(vtype, Const.Float32Type))
                {
                    result = LLVM.BuildFPToSI(builder, v, Const.Int32Type, "int_cast");
                }
                else if (isEqualType(vtype, Const.BoolType))
                {
                    result = LLVM.BuildZExt(builder, v, targetType, "int_cast");
                }
                else
                {
                    throw new InvalidCodePath();
                }
            }
            else if (LLVM.GetTypeKind(targetType) == LLVMTypeKind.LLVMFloatTypeKind)
            {
                if (LLVM.GetTypeKind(vtype) == LLVMTypeKind.LLVMIntegerTypeKind)
                {
                    result = LLVM.BuildSIToFP(builder, v, targetType, "int_to_float_cast");
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                throw new InvalidCodePath();
            }
            valueStack.Push(result);

            //if (isEqualType(resultType, Const.Float32Type))
            //{
            //    result = LLVM.BuildSIToFP(builder, v, Const.Float32Type, "float32_cast");
            //}
            //else if (isEqualType(resultType, Const.Int32Type))
            //{
            //    if (isEqualType(vtype, Const.Float32Type))
            //    {
            //        result = LLVM.BuildFPToSI(builder, v, Const.Int32Type, "int32_cast");
            //    }
            //    else if (isEqualType(vtype, Const.Int32Type))
            //    {
            //        result = v;
            //    }
            //    else if (isEqualType(vtype, Const.BoolType))
            //    {
            //        result = LLVM.BuildZExt(builder, v, Const.Int32Type, "int32_cast");
            //    }
            //    else
            //    {
            //        throw new InvalidCodePath();
            //    }
            //}
            //else
            //{
            //    throw new NotImplementedException();
            //}

        }

        public void Visit(AST.VariableDefinition node)
        {
            if (!ctx.Peek().global)
            {
                // TODO simplify this so you dont have to dynamicly dispatch here
                if (node.expression is AST.StructConstructor)
                {
                    var sc = node.expression as AST.StructConstructor;

                    var structType = getTypeRef(sc.structType);

                    var insert = LLVM.GetInsertBlock(builder);
                    LLVM.PositionBuilderAtEnd(builder, ctx.Peek().vars);
                    var struct_ptr = LLVM.BuildAlloca(builder, structType, node.variable.name);
                    variables[node.variable.name] = struct_ptr;
                    LLVM.PositionBuilderAtEnd(builder, insert);

                    for (int i = 0; i < sc.argumentList.Count; ++i)
                    {
                        Visit(sc.argumentList[i]);
                        var arg = valueStack.Pop();
                        var arg_ptr = LLVM.BuildStructGEP(builder, struct_ptr, (uint)i, "struct_arg_" + i);
                        LLVM.BuildStore(builder, arg, arg_ptr);
                    }
                }
                // TODO simplify this so you dont have to dynamicly dispatch here
                else if (node.expression is AST.ArrayConstructor)
                {
                    var ac = node.expression as AST.ArrayConstructor;

                    var arr_struct_type = getTypeRef(ac.type);

                    var insert = LLVM.GetInsertBlock(builder);
                    LLVM.PositionBuilderAtEnd(builder, ctx.Peek().vars);
                    var arr_struct_ptr = LLVM.BuildAlloca(builder, arr_struct_type, "arr_struct_alloca");
                    var elem_type = getTypeRef(ac.type.elementType);
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

                    for (int i = 0; i < ac.elements.Count; ++i)
                    {
                        var elem = ac.elements[i];
                        Visit(elem);
                        var arg = valueStack.Pop();
                        var gep_idx = new LLVMValueRef[] { LLVM.ConstInt(Const.Int32Type, (ulong)i, Const.FalseBool) };
                        var gep = LLVM.BuildGEP(builder, arr_elem_ptr, out gep_idx[0], 1, "array_elem_" + i);

                        LLVM.BuildStore(builder, arg, gep);
                    }
                    variables[node.variable.name] = arr_struct_ptr;
                }
                else
                {
                    Visit(node.expression);
                    var result = valueStack.Pop();
                    var resultType = LLVM.TypeOf(result);
                    LLVMValueRef v;

                    var insert = LLVM.GetInsertBlock(builder);
                    LLVM.PositionBuilderAtEnd(builder, ctx.Peek().vars);
                    v = LLVM.BuildAlloca(builder, resultType, node.variable.name);
                    variables[node.variable.name] = v;
                    LLVM.PositionBuilderAtEnd(builder, insert);
                    LLVM.BuildStore(builder, result, v);


                }
            }
            else
            {
                if (node.expression is AST.StructConstructor)
                {
                }
                else if (node.expression is AST.ArrayConstructor)
                {
                }
                else
                {
                    Visit(node.expression);
                    var result = valueStack.Pop();
                    var resultType = LLVM.TypeOf(result);
                    var v = LLVM.AddGlobal(mod, resultType, node.variable.name);

                    if (LLVM.IsConstant(result))
                    {
                        LLVM.SetInitializer(v, result);
                    }
                    else
                    {
                        LLVM.SetInitializer(v, LLVM.ConstNull(resultType));
                        // var insert = LLVM.GetInsertBlock(builder);
                        // LLVM.PositionBuilderAtEnd(builder, ctx.Peek().vars);
                        LLVM.BuildStore(builder, result, v);
                        // LLVM.PositionBuilderAtEnd(builder, insert);
                    }
                    variables[node.variable.name] = v;
                }
            }
        }

        public void Visit(AST.Assignment node)
        {
            Visit(node.expression);
            var result = valueStack.Pop();
            var resultType = LLVM.TypeOf(result);
            var resultTypeName = typeToString(resultType);

            Visit(node.target);
            var target = valueStack.Pop();
            var targetType = LLVM.TypeOf(target);
            var targetTypeName = typeToString(targetType);


            //var v = variables[node.variable.name];


            //if (node.isArrayAssignment)
            //{
            //    Visit(node.index);
            //    var idx = valueStack.Pop();
            //    var arr = v;
            //    var arr_type_name = typeToString(LLVM.TypeOf(arr));

            //    LLVMValueRef arr_elem_ptr;
            //    var gep_idx_0 = new LLVMValueRef[] { Const.ZeroInt32, Const.OneInt32 };
            //    var gep_arr_elem_ptr = LLVM.BuildGEP(builder, arr, out gep_idx_0[0], 2, "gep_arr_elem_ptr");
            //    arr_elem_ptr = LLVM.BuildLoad(builder, gep_arr_elem_ptr, "arr_elem_ptr");

            //    var gep_idx_1 = new LLVMValueRef[] { idx };
            //    var gep_arr_elem = LLVM.BuildGEP(builder, arr_elem_ptr, out gep_idx_1[0], 1, "gep_arr_elem");

            //    LLVM.BuildStore(builder, result, gep_arr_elem);
            //}
            //else
            {
                if (!isEqualType(LLVM.GetElementType(targetType), resultType))
                {
                    throw new BackendTypeMismatchException(resultType, LLVM.GetElementType(targetType));
                }
                LLVM.BuildStore(builder, result, target);
            }
        }

        public void Visit(AST.Block node)
        {
            foreach (var s in node.statements)
            {
                Visit(s);
            }
        }


        public void Visit(AST.VariableLookup node)
        {
            var vd = node.varDefinition;
            // if variable is function paramter just return it immediately
            if (vd.isFunctionParameter)
            {
                var pr = LLVM.GetParam(ctx.Peek().function, (uint)vd.parameterIdx);
                valueStack.Push(pr);
                return;
            }
            var v = variables[vd.name];


            LLVMValueRef result;

            bool is_global = LLVM.IsAGlobalVariable(v).Pointer != IntPtr.Zero;

            // HACK:
            if (LLVM.IsConstant(v) && !is_global)
            {
                result = v;
            }
            else
            {
                var load = LLVM.BuildLoad(builder, v, vd.name);
                result = load;
                if (node.returnPointer)
                {
                    result = v;
                    Debug.Assert(node.inc == AST.VariableLookup.Incrementor.None);
                }
            }
            var ltype = LLVM.TypeOf(result);


            switch (node.inc)
            {
                case AST.VariableLookup.Incrementor.None:
                    break;

                case AST.VariableLookup.Incrementor.preIncrement:
                    if (isEqualType(ltype, Const.Int32Type))
                    {
                        result = LLVM.BuildAdd(builder, result, Const.OneInt32, "preinc");
                    }
                    else if (isEqualType(ltype, Const.Float32Type))
                    {
                        result = LLVM.BuildFAdd(builder, result, Const.OneFloat32, "preinc");
                    }
                    LLVM.BuildStore(builder, result, v);
                    break;
                case AST.VariableLookup.Incrementor.preDecrement:
                    if (isEqualType(ltype, Const.Int32Type))
                    {
                        result = LLVM.BuildSub(builder, result, Const.OneInt32, "predec");
                    }
                    else if (isEqualType(ltype, Const.Float32Type))
                    {
                        result = LLVM.BuildFSub(builder, result, Const.OneFloat32, "predec");
                    }
                    LLVM.BuildStore(builder, result, v);
                    break;
                case AST.VariableLookup.Incrementor.postIncrement:
                    var postinc = default(LLVMValueRef);
                    if (isEqualType(ltype, Const.Int32Type))
                    {
                        postinc = LLVM.BuildAdd(builder, result, Const.OneInt32, "postinc");
                    }
                    else if (isEqualType(ltype, Const.Float32Type))
                    {
                        postinc = LLVM.BuildFAdd(builder, result, Const.OneFloat32, "postinc");
                    }
                    LLVM.BuildStore(builder, postinc, v);
                    break;
                case AST.VariableLookup.Incrementor.postDecrement:
                    var postdec = default(LLVMValueRef);
                    if (isEqualType(ltype, Const.Int32Type))
                    {
                        postdec = LLVM.BuildSub(builder, result, Const.OneInt32, "postdec");
                    }
                    else if (isEqualType(ltype, Const.Float32Type))
                    {
                        postdec = LLVM.BuildFSub(builder, result, Const.OneFloat32, "postdec");
                    }
                    LLVM.BuildStore(builder, postdec, v);
                    break;
                default:
                    break;
            }
            valueStack.Push(result);

        }

        public void Visit(AST.FunctionCall node)
        {
            var f = functions[node.functionName];
            var cnt = node.argumentList.Count;
            LLVMValueRef[] parameters = new LLVMValueRef[Math.Max(1, cnt)];

            for (int i = 0; i < node.argumentList.Count; ++i)
            {
                Visit(node.argumentList[i]);
                parameters[i] = valueStack.Pop();
                var pn = parameters[i].GetTypeString();
                Console.WriteLine(pn);
            }

            var ftn = f.GetTypeString();

            var ft = LLVM.TypeOf(f);
            // http://lists.cs.uiuc.edu/pipermail/llvmdev/2008-May/014844.html
            var rt = LLVM.GetReturnType(LLVM.GetElementType(ft));
            if (isEqualType(rt, Const.VoidType))
            {

                LLVM.BuildCall(builder, f, out parameters[0], (uint)cnt, "");
            }
            else
            {
                var v = LLVM.BuildCall(builder, f, out parameters[0], (uint)cnt, node.functionName);
                valueStack.Push(v);
            }
        }

        public void Visit(AST.ReturnFunction node)
        {
            if (node.expression != null)
            {
                Visit(node.expression);
                var v = valueStack.Pop();
                LLVM.BuildRet(builder, v);
            }
            else
            {
                LLVM.BuildRetVoid(builder);
            }
        }

        public void Visit(AST.IfCondition node)
        {
            Visit(node.condition);
            var condition = valueStack.Pop();
            if (!isEqualType(LLVM.TypeOf(condition), Const.BoolType))
            {
                throw new BackendTypeMismatchException(LLVM.TypeOf(condition), Const.BoolType);
            }

            var insert = LLVM.GetInsertBlock(builder);

            var thenBlock = LLVM.AppendBasicBlock(ctx.Peek().function, "then");
            LLVM.MoveBasicBlockAfter(thenBlock, insert);


            var lastBlock = thenBlock;
            List<LLVMBasicBlockRef> elifBlocks = new List<LLVMBasicBlockRef>();
            var idx = 0;
            foreach (var elif in node.elifs)
            {
                var elifBlock = LLVM.AppendBasicBlock(ctx.Peek().function, "elif_" + (idx++));
                LLVM.MoveBasicBlockAfter(elifBlock, lastBlock);
                lastBlock = elifBlock;
                elifBlocks.Add(elifBlock);
            }

            var elseBlock = default(LLVMBasicBlockRef);
            var endIfBlock = default(LLVMBasicBlockRef);
            if (node.elseBlock != null)
            {
                elseBlock = LLVM.AppendBasicBlock(ctx.Peek().function, "else");
                LLVM.MoveBasicBlockAfter(elseBlock, lastBlock);
                lastBlock = elseBlock;
            }

            endIfBlock = LLVM.AppendBasicBlock(ctx.Peek().function, "endif");
            LLVM.MoveBasicBlockAfter(endIfBlock, lastBlock);
            lastBlock = endIfBlock;

            var nextFail = endIfBlock;
            if (elifBlocks.Count > 0)
            {
                nextFail = elifBlocks.First();
            }
            else if (node.elseBlock != null)
            {
                nextFail = elseBlock;
            }

            LLVM.BuildCondBr(builder, condition, thenBlock, nextFail);

            LLVM.PositionBuilderAtEnd(builder, thenBlock);
            Visit(node.thenBlock);

            var term = LLVM.GetBasicBlockTerminator(LLVM.GetInsertBlock(builder));
            if (term.Pointer == IntPtr.Zero)
            {
                LLVM.BuildBr(builder, endIfBlock);
            }


            for (int i = 0; i < elifBlocks.Count; ++i)
            {
                var elif = elifBlocks[i];

                var elifThen = LLVM.AppendBasicBlock(ctx.Peek().function, "elif_" + i + "_then");
                LLVM.MoveBasicBlockAfter(elifThen, elif);

                LLVM.PositionBuilderAtEnd(builder, elif);
                var elifNode = node.elifs[i] as AST.Elif;
                Visit(elifNode.condition);
                var elifCond = valueStack.Pop();


                var nextBlock = endIfBlock;
                if (node.elseBlock != null)
                {
                    nextBlock = elseBlock;
                }
                if (i < elifBlocks.Count - 1)
                {
                    nextBlock = elifBlocks[i + 1];
                }
                LLVM.BuildCondBr(builder, elifCond, elifThen, nextBlock);

                LLVM.PositionBuilderAtEnd(builder, elifThen);
                Visit(elifNode.thenBlock);

                term = LLVM.GetBasicBlockTerminator(LLVM.GetInsertBlock(builder));
                if (term.Pointer == IntPtr.Zero)
                {
                    LLVM.BuildBr(builder, endIfBlock);
                }
            }

            if (node.elseBlock != null)
            {
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

            foreach (var n in node.initializer)
            {
                Visit(n);
            }
            LLVM.BuildBr(builder, loopPre);

            LLVM.PositionBuilderAtEnd(builder, loopPre);
            Visit(node.condition);

            var condition = valueStack.Pop();
            if (!isEqualType(LLVM.TypeOf(condition), Const.BoolType))
            {
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
            foreach (var n in node.iterator)
            {
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
            if (!isEqualType(LLVM.TypeOf(condition), Const.BoolType))
            {
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

        public void Visit(AST.FunctionDefinition node)
        {
            var fun = node.fun;

            if (functions.ContainsKey(fun.name))
                throw new Exception("function redefinition");

            var cnt = Math.Max(1, fun.parameters.Count);
            var par = new LLVMTypeRef[cnt];

            for (int i = 0; i < fun.parameters.Count; ++i)
            {
                par[i] = getTypeRef(fun.parameters[i].type);
            }

            var returnType = getTypeRef((fun.returnType));

            var funType = LLVM.FunctionType(returnType, out par[0], (uint)fun.parameters.Count, Const.FalseBool);
            var function = LLVM.AddFunction(mod, fun.name, funType);

            functions.Add(fun.name, function);

            for (int i = 0; i < fun.parameters.Count; ++i)
            {
                LLVMValueRef param = LLVM.GetParam(function, (uint)i);
                LLVM.SetValueName(param, fun.parameters[i].name);
                // variables.Add(fun.parameters[i].name, new TypedValue(param, TypedValue.MapType(fun.parameters[i].type)));
            }

            var vars = LLVM.AppendBasicBlock(function, "vars");
            var entry = LLVM.AppendBasicBlock(function, "entry");

            var blockTemp = LLVM.GetInsertBlock(builder);

            LLVM.PositionBuilderAtEnd(builder, entry);

            ctx.Push(new ExecutionContext(function, fun.name, entry, vars));

            Visit(node.body);

            insertMissingReturn(returnType);

            LLVM.PositionBuilderAtEnd(builder, vars);
            LLVM.BuildBr(builder, entry);

            LLVM.PositionBuilderAtEnd(builder, blockTemp);

            LLVM.VerifyFunction(function, LLVMVerifierFailureAction.LLVMPrintMessageAction);

            ctx.Pop();

        }

        public void Visit(AST.BreakLoop node)
        {
            if (!ctx.Peek().loop)
            {
                throw new BackendException("break statement outside of loop not allowed");
            }
            LLVM.BuildBr(builder, ctx.Peek().loopEnd);
        }

        public void Visit(AST.ContinueLoop node)
        {
            if (!ctx.Peek().loop)
            {
                throw new BackendException("break statement outside of loop not allowed");
            }
            LLVM.BuildBr(builder, ctx.Peek().loopNext);
        }

        public void Visit(AST.ArrayElementAccess node)
        {
            var vd = node.varDefinition;
            var arr = default(LLVMValueRef);
            if (vd.isFunctionParameter)
            {
                arr = LLVM.GetParam(ctx.Peek().function, (uint)vd.parameterIdx);
            }
            else
            {
                arr = variables[vd.name];
            }

            // var arr_type_name = typeToString(LLVM.TypeOf(arr));

            Visit(node.index);
            var idx = valueStack.Pop();


            LLVMValueRef arr_elem_ptr;
            if (!vd.isFunctionParameter)
            {
                var gep_idx_0 = new LLVMValueRef[] { Const.ZeroInt32, Const.OneInt32 };
                var gep_arr_elem_ptr = LLVM.BuildGEP(builder, arr, out gep_idx_0[0], 2, "gep_arr_elem_ptr");
                arr_elem_ptr = LLVM.BuildLoad(builder, gep_arr_elem_ptr, "arr_elem_ptr");
            }
            else
            {
                arr_elem_ptr = LLVM.BuildExtractValue(builder, arr, (uint)1, "gep_arr_elem_ptr");
            }

            var gep_idx_1 = new LLVMValueRef[] { idx };
            var gep_arr_elem = LLVM.BuildGEP(builder, arr_elem_ptr, out gep_idx_1[0], 1, "gep_arr_elem");
            var load = LLVM.BuildLoad(builder, gep_arr_elem, "arr_elem");

            var result = load;
            if (node.returnPointer)
            {
                result = gep_arr_elem;
            }
            valueStack.Push(result);
        }

        public void Visit(AST.StructFieldAccess node)
        {
            //var vd = node.structure;

            //LLVMValueRef v;
            //if (vd.isFunctionParameter)
            //{
            //    v = LLVM.GetParam(ctx.Peek().function, (uint)vd.parameterIdx);
            //}
            //else
            //{
            //    v = variables[vd.name];
            //}

            //var ts = typeToString(LLVM.TypeOf(v));
            //Console.WriteLine(ts);


            Visit(node.left);
            var v = valueStack.Pop();

            var v_type = typeToString(LLVM.TypeOf(v));

            var s = node.structType;
            var idx = s.GetFieldIndex(node.fieldName);
            var indices = new LLVMValueRef[] { Const.ZeroInt32, LLVM.ConstInt(Const.Int32Type, (ulong)idx, Const.FalseBool) };

            LLVMValueRef gep;

            // is not function argument?
            if (LLVM.IsAArgument(v).Pointer == IntPtr.Zero)
            {
                gep = LLVM.BuildInBoundsGEP(builder, v, out indices[0], 2, "struct_field_ptr");
                var load = LLVM.BuildLoad(builder, gep, "struct_field");
                var result = load;
                if (node.returnPointer)
                {
                    result = gep;
                }
                valueStack.Push(result);
                return;
            }
            else
            {
                uint[] uindices = { (uint)idx };
                var load = LLVM.BuildExtractValue(builder, v, (uint)idx, "struct_field_extract");
                var result = load;
                if (node.returnPointer)
                {
                    throw new NotImplementedException();
                }
                valueStack.Push(result);
                return;
            }


        }

        public void Visit(AST.StructConstructor node)
        {
            // throw new NotImplementedException();
        }

        public void Visit(AST.ArrayConstructor node)
        {

            throw new NotImplementedException();

            LLVMValueRef[] values = new LLVMValueRef[node.elements.Count];

            var idx = 0;
            foreach (var elem in node.elements)
            {
                Visit(elem);
                values[idx++] = valueStack.Pop();
            }


            var elem_type = getTypeRef(node.type.elementType);
            var size = LLVM.ConstInt(Const.Int32Type, (ulong)node.elements.Count, Const.FalseBool);
            var const_arr = LLVM.ConstArray(elem_type, out values[0], (uint)values.Length);

            var alloc_arr = LLVM.BuildArrayAlloca(builder, elem_type, size, "arr_alloca");
            // var alloc_arr_typ = typeToString(LLVM.TypeOf(alloc_arr));

            var arr_ptr = LLVM.BuildBitCast(builder, alloc_arr, LLVM.PointerType(LLVM.TypeOf(const_arr), 0), "arr_ptr");
            var store_arr = LLVM.BuildStore(builder, const_arr, arr_ptr);

            //var global = LLVM.AddGlobal(mod, LLVM.TypeOf(arr), ctx.Peek().functionName + "." + "const_array");
            //LLVM.SetInitializer(global, arr);
            //LLVM.SetGlobalConstant(global, Const.TrueBool);
            //LLVM.SetLinkage(global, LLVMLinkage.LLVMPrivateLinkage);
            //LLVM.SetUnnamedAddr(global, Const.TrueBool);
            //var result = LLVM.BuildLoad(builder, global, "array_load");
            //valueStack.Push(result);

            //  valueStack.Push(structure);
        }

        public void Visit(AST.StructDefinition node)
        {
            // throw new NotImplementedException();
        }


        public void Visit(AST.Node node)
        {
            if (node is AST.Root)
            {
                Visit(node as AST.Root);
            }
            else if (node is AST.ConstInt32)
            {
                Visit(node as AST.ConstInt32);
            }
            else if (node is AST.ConstFloat32)
            {
                Visit(node as AST.ConstFloat32);
            }
            else if (node is AST.ConstBool)
            {
                Visit(node as AST.ConstBool);
            }
            else if (node is AST.ConstString)
            {
                Visit(node as AST.ConstString);
            }
            else if (node is AST.ArrayConstructor)
            {
                Visit(node as AST.ArrayConstructor);
            }
            else if (node is AST.BinOp)
            {
                Visit(node as AST.BinOp);
            }
            else if (node is AST.UnaryOp)
            {
                Visit(node as AST.UnaryOp);
            }
            else if (node is AST.TypeCastOp)
            {
                Visit(node as AST.TypeCastOp);
            }
            else if (node is AST.VariableDefinition)
            {
                Visit(node as AST.VariableDefinition);
            }
            else if (node is AST.Assignment)
            {
                Visit(node as AST.Assignment);
            }
            else if (node is AST.Block)
            {
                Visit(node as AST.Block);
            }
            else if (node is AST.VariableLookup)
            {
                Visit(node as AST.VariableLookup);
            }
            else if (node is AST.ArrayElementAccess)
            {
                Visit(node as AST.ArrayElementAccess);
            }
            else if (node is AST.StructFieldAccess)
            {
                Visit(node as AST.StructFieldAccess);
            }
            else if (node is AST.StructConstructor)
            {
                Visit(node as AST.StructConstructor);
            }
            else if (node is AST.UninitializedArray)
            {
                Visit(node as AST.UninitializedArray);
            }
            else if (node is AST.FunctionCall)
            {
                Visit(node as AST.FunctionCall);
            }
            else if (node is AST.IfCondition)
            {
                Visit(node as AST.IfCondition);
            }
            else if (node is AST.ForLoop)
            {
                Visit(node as AST.ForLoop);
            }
            else if (node is AST.WhileLoop)
            {
                Visit(node as AST.WhileLoop);
            }
            else if (node is AST.BreakLoop)
            {
                Visit(node as AST.BreakLoop);
            }
            else if (node is AST.ContinueLoop)
            {
                Visit(node as AST.ContinueLoop);
            }
            else if (node is AST.ReturnFunction)
            {
                Visit(node as AST.ReturnFunction);
            }
            else if (node is AST.FunctionDefinition)
            {
                Visit(node as AST.FunctionDefinition);
            }
            else if (node is AST.StructDefinition)
            {
                Visit(node as AST.StructDefinition);
            }
            else
            {
                throw new InvalidCodePath();
            }
        }
    }
}
