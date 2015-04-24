using LLVMSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PragmaScript
{
    partial class Backend
    {
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

        public static string ParseString(string txt)
        {
            var provider = new Microsoft.CSharp.CSharpCodeProvider();
            var prms = new System.CodeDom.Compiler.CompilerParameters();
            prms.GenerateExecutable = false;
            prms.GenerateInMemory = true;
            var results = provider.CompileAssemblyFromSource(prms, @"
namespace tmp
{
    public class tmpClass
    {
        public static string GetValue()
        {
             return " + "\"" + txt + "\"" + @";
        }
    }
}");
            System.Reflection.Assembly ass = results.CompiledAssembly;
            var method = ass.GetType("tmp.tmpClass").GetMethod("GetValue");
            return method.Invoke(null, null) as string;
        }

        string convertString(string s)
        {
            var tmp = s.Substring(1, s.Length - 2);
            return ParseString(tmp);
        }
        public void Visit(AST.ConstString node)
        {
            var str = convertString(node.s);
            var constString = LLVM.BuildGlobalStringPtr(builder, str, "str");
            var result = LLVM.BuildBitCast(builder, constString, Const.Int8PointerType, "str_ptr");
            valueStack.Push(result);
        }

        public void Visit(AST.ConstArray node)
        {

            LLVMValueRef[] values = new LLVMValueRef[node.elements.Count];
            
            var idx = 0;
            foreach (var elem in node.elements)
            {
                Visit(elem);
                values[idx++] = valueStack.Pop();
            }

            var size = LLVM.ConstInt(Const.Int32Type, (ulong)node.elements.Count, Const.FalseBool);
            var arr = LLVM.ConstArray(getTypeRef(node.elementType), out values[0], (uint)values.Length);

            var sp = new LLVMValueRef[] { size, arr };
            // TODO: does this need to be packed?
            var structure = LLVM.ConstStruct(out sp[0], 2, Const.FalseBool);

            //var global = LLVM.AddGlobal(mod, LLVM.TypeOf(arr), ctx.Peek().functionName + "." + "const_array");
            //LLVM.SetInitializer(global, arr);
            //LLVM.SetGlobalConstant(global, Const.TrueBool);
            //LLVM.SetLinkage(global, LLVMLinkage.LLVMPrivateLinkage);
            //LLVM.SetUnnamedAddr(global, Const.TrueBool);
            //var result = LLVM.BuildLoad(builder, global, "array_load");
            //valueStack.Push(result);
            

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
            Visit(node.right);

            var right = valueStack.Pop();
            var left = valueStack.Pop();

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
            var resultType = getTypeRef(node.type);

            // TODO: check if integral type
            // TODO: handle non integral types
            if (isEqualType(resultType, Const.Float32Type))
            {
                result = LLVM.BuildSIToFP(builder, v, Const.Float32Type, "float32_cast");
            }
            else if (isEqualType(resultType, Const.Int32Type))
            {
                if (isEqualType(vtype, Const.Float32Type))
                {
                    result = LLVM.BuildFPToSI(builder, v, Const.Int32Type, "int32_cast");
                }
                else if (isEqualType(vtype, Const.Int32Type))
                {
                    result = v;
                }
                else if (isEqualType(vtype, Const.BoolType))
                {
                    result = LLVM.BuildZExt(builder, v, Const.Int32Type, "int32_cast");
                }
                else
                {
                    throw new InvalidCodePath();
                }
            }
            else
            {
                throw new NotImplementedException();
            }
            valueStack.Push(result);
        }

        public void Visit(AST.VariableDeclaration node)
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

        public void Visit(AST.Assignment node)
        {
            Visit(node.expression);

            var result = valueStack.Pop();
            var resultType = LLVM.TypeOf(result);
            var v = variables[node.variable.name];
            var vtype = LLVM.TypeOf(v);



            if (!isEqualType(LLVM.GetElementType(vtype), resultType))
            {
                throw new BackendTypeMismatchException(resultType, LLVM.GetElementType(vtype));
            }

            LLVM.BuildStore(builder, result, v);
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
            if (node.varDefinition.isFunctionParameter)
            {
                var pr = LLVM.GetParam(ctx.Peek().function, (uint)node.varDefinition.parameterIdx);
                valueStack.Push(pr);
                return;
            }

            var v = variables[node.variableName];

            var l = LLVM.BuildLoad(builder, v, node.variableName);
            var ltype = LLVM.TypeOf(l);
            var result = l;
            switch (node.inc)
            {
                case AST.VariableLookup.Incrementor.None:
                    break;

                case AST.VariableLookup.Incrementor.preIncrement:
                    if (isEqualType(ltype, Const.Int32Type))
                    {
                        result = LLVM.BuildAdd(builder, l, Const.OneInt32, "preinc");
                    }
                    else if (isEqualType(ltype, Const.Float32Type))
                    {
                        result = LLVM.BuildFAdd(builder, l, Const.OneFloat32, "preinc");
                    }
                    LLVM.BuildStore(builder, result, v);
                    break;
                case AST.VariableLookup.Incrementor.preDecrement:
                    if (isEqualType(ltype, Const.Int32Type))
                    {
                        result = LLVM.BuildSub(builder, l, Const.OneInt32, "predec");
                    }
                    else if (isEqualType(ltype, Const.Float32Type))
                    {
                        result = LLVM.BuildFSub(builder, l, Const.OneFloat32, "predec");
                    }
                    LLVM.BuildStore(builder, result, v);
                    break;
                case AST.VariableLookup.Incrementor.postIncrement:
                    var postinc = default(LLVMValueRef);
                    if (isEqualType(ltype, Const.Int32Type))
                    {
                        postinc = LLVM.BuildAdd(builder, l, Const.OneInt32, "postinc");
                    }
                    else if (isEqualType(ltype, Const.Float32Type))
                    {
                        postinc = LLVM.BuildFAdd(builder, l, Const.OneFloat32, "postinc");
                    }
                    LLVM.BuildStore(builder, postinc, v);
                    break;
                case AST.VariableLookup.Incrementor.postDecrement:
                    var postdec = default(LLVMValueRef);
                    if (isEqualType(ltype, Const.Int32Type))
                    {
                        postdec = LLVM.BuildSub(builder, l, Const.OneInt32, "postdec");
                    }
                    else if (isEqualType(ltype, Const.Float32Type))
                    {
                        postdec = LLVM.BuildFSub(builder, l, Const.OneFloat32, "postdec");
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
            }

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

        public void Visit(AST.FunctionDeclaration node)
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

            ctx.Push(new ExecutionContext(function, fun.name, vars, entry));

            Visit(node.body);



            InsertMissingReturn(returnType);

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
            var arr = variables[node.variableName];

            Visit(node.index);
            var idx = valueStack.Pop();

            var gep_idx = new LLVMValueRef[]{ Const.ZeroInt32, Const.OneInt32, idx };
            var gep = LLVM.BuildInBoundsGEP(builder, arr, out gep_idx[0], 3, "array_elem_ptr");
            var load = LLVM.BuildLoad(builder, gep, "array_elem");

            valueStack.Push(load);
        }

        public void Visit(AST.StructFieldAccess node)
        {
            var v = variables[node.structName];
            var s = node.structure.type as AST.FrontendStructType;
            var idx = s.GetFieldIndex(node.fieldName);
            var indices = new LLVMValueRef[] { Const.ZeroInt32, LLVM.ConstInt(Const.Int32Type, (ulong)idx, Const.FalseBool) };
            var gep = LLVM.BuildInBoundsGEP(builder, v, out indices[0], 2, "struct_field_ptr");
            var load = LLVM.BuildLoad(builder, gep, "struct_field");

            valueStack.Push(load);
        }

        public void Visit(AST.Node node)
        {
            if (node is AST.ConstInt32)
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
            else if (node is AST.ConstArray)
            {
                Visit(node as AST.ConstArray);
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
            else if (node is AST.VariableDeclaration)
            {
                Visit(node as AST.VariableDeclaration);
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
            else if (node is AST.FunctionDeclaration)
            {
                Visit(node as AST.FunctionDeclaration);
            }
        }
    }
}
