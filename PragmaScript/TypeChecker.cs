using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PragmaScript
{

    class UnresolvedType
    {
        public List<UnresolvedType> waitingFor;
        public List<UnresolvedType> blocking;
        public AST.Node node;


        public UnresolvedType(AST.Node node)
        {
            this.node = node;
            waitingFor = new List<UnresolvedType>();
            blocking = new List<UnresolvedType>();
        }
    }

    class TypeChecker
    {
        Dictionary<AST.Node, FrontendType> knownTypes;
        Dictionary<AST.Node, UnresolvedType> unresolved;
        Dictionary<AST.Node, FrontendType> pre_resolved;

        public TypeChecker()
        {
            unresolved = new Dictionary<AST.Node, UnresolvedType>();
            knownTypes = new Dictionary<AST.Node, FrontendType>();
            pre_resolved = new Dictionary<AST.Node, FrontendType>();
        }


        public void CheckTypes(AST.ProgramRoot root)
        {
            foreach (var fr in root.files)
            {
                checkTypeDynamic(fr);
            }
            foreach (var u in unresolved.Values)
            {
                Console.WriteLine(u.node.ToString());
            }
            if (unresolved.Count > 0)
            {
                throw new ParserError($"Cannot resolve type: {unresolved.First().Key}", unresolved.First().Key.token);
            }

            // TODO: cycle detection!
            //    HashSet<AST.Node> blocker = new HashSet<AST.Node>();
            //foreach (var v in unresolved.Values)
            //{
            //    getRootBlocker(v, blocker);
            //}
            //foreach (var n in blocker)
            //{
            //    throw new ParserError($"Cannot resolve type of \"{n}\"", n.token);
            //}
        }

        public FrontendType GetNodeType(AST.Node node)
        {
            return knownTypes[node];
        }

        FrontendType getType(AST.Node node)
        {
            FrontendType result;
            if (knownTypes.TryGetValue(node, out result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        void addUnresolved(AST.Node from, AST.Node to)
        {
            Debug.Assert(from != null);
            Debug.Assert(to != null);
            Debug.Assert(from != to);

            UnresolvedType u;
            if (!unresolved.TryGetValue(from, out u))
            {
                u = new UnresolvedType(from);
                unresolved.Add(from, u);
            }

            UnresolvedType cu;
            if (!unresolved.TryGetValue(to, out cu))
            {
                cu = new UnresolvedType(to);
                unresolved.Add(to, cu);
            }
            u.waitingFor.Add(cu);
            cu.blocking.Add(u);
        }

        void resolve(AST.Node node, FrontendType type)
        {
            Debug.Assert(node != null);
            Debug.Assert(type != null);

            knownTypes.Add(node, type);
            UnresolvedType u;
            if (unresolved.TryGetValue(node, out u))
            {
                unresolved.Remove(node);
                if (u.blocking.Count > 0)
                {
                    foreach (var b in u.blocking)
                    {
                        b.waitingFor.Remove(u);
                        if (b.waitingFor.Count == 0)
                        {
                            checkTypeDynamic(b.node);
                        }
                    }
                }
            }

            if (pre_resolved.ContainsKey(node))
            {
                pre_resolved.Remove(node);
            }
        }

        void pre_resolve(AST.Node node, FrontendType type)
        {
            Debug.Assert(node != null);
            Debug.Assert(type != null);
            pre_resolved.Add(node, type);
        }

        void getRootBlocker(UnresolvedType t, HashSet<AST.Node> blocker)
        {
            if (t.waitingFor.Count > 0)
            {
                foreach (var wt in t.waitingFor)
                {
                    if (!blocker.Contains(wt.node))
                    {
                        getRootBlocker(wt, blocker);
                    }
                }
            }
            else
            {
                blocker.Add(t.node);
            }
        }

        void checkType(AST.FileRoot node)
        {
            foreach (var n in node.declarations)
            {
                checkTypeDynamic(n);
            }
            resolve(node, FrontendType.none);
        }

        void checkTypeDynamic(AST.Node node)
        {
            if (knownTypes.ContainsKey(node))
            {
                return;
            }
            dynamic dn = node;
            checkType(dn);
        }
        
        void checkType(AST.Block node)
        {
            foreach (var n in node.statements)
            {
                checkTypeDynamic(n);
            }
            resolve(node, FrontendType.none);
        }



        void checkType(AST.Elif node)
        {
            checkTypeDynamic(node.condition);
            var ct = getType(node.condition);
            if (ct != null)
            {
                if (!ct.Equals(FrontendType.bool_))
                    throw new ParserExpectedType(FrontendType.bool_, ct, node.condition.token);
                resolve(node, FrontendType.none);
            }
            else
            {
                var u = new UnresolvedType(node);
                var cu = unresolved[node.condition];
                u.waitingFor.Add(cu);
                cu.blocking.Add(u);
            }

            checkTypeDynamic(node.thenBlock);

        }

        void checkType(AST.IfCondition node)
        {
            checkTypeDynamic(node.condition);
            var ct = getType(node.condition);
            if (ct != null)
            {
                if (!ct.Equals(FrontendType.bool_))
                    throw new ParserExpectedType(FrontendType.bool_, ct, node.condition.token);
                resolve(node, FrontendType.none);
            }
            else
            {
                addUnresolved(node, node.condition);
            }

            checkTypeDynamic(node.thenBlock);

            foreach (var elif in node.elifs)
            {
                checkTypeDynamic(elif);
            }

            if (node.elseBlock != null)
            {
                checkTypeDynamic(node.elseBlock);
            }
        }

        void checkType(AST.ForLoop node)
        {
            foreach (var init in node.initializer)
            {
                checkTypeDynamic(init);
            }
            checkTypeDynamic(node.condition);
            var ct = getType(node.condition);
            if (ct != null)
            {
                if (!ct.Equals(FrontendType.bool_))
                    throw new ParserExpectedType(FrontendType.bool_, ct, node.condition.token);
                resolve(node, FrontendType.none);
            }
            else
            {
                addUnresolved(node, node.condition);
            }
            foreach (var iter in node.iterator)
            {
                checkTypeDynamic(iter);
            }
            checkTypeDynamic(node.loopBody);
        }

        void checkType(AST.WhileLoop node)
        {
            checkTypeDynamic(node.condition);
            var ct = getType(node.condition);
            if (ct != null)
            {
                if (!ct.Equals(FrontendType.bool_))
                    throw new ParserExpectedType(FrontendType.bool_, ct, node.condition.token);
                resolve(node, FrontendType.none);
            }
            else
            {
                addUnresolved(node, node.condition);
            }
            checkTypeDynamic(node.loopBody);
        }

        void checkType(AST.VariableDefinition node)
        {
            Debug.Assert(node.typeString != null || node.expression != null);

            FrontendType tt = null;
            if (node.typeString != null)
            {
                checkTypeDynamic(node.typeString);
                tt = getType(node.typeString);
                if (tt == null)
                {
                    addUnresolved(node, node.typeString);
                }
            }
            FrontendType et = null;
            if (node.expression != null)
            {
                checkTypeDynamic(node.expression);
                et = getType(node.expression);
                if (et == null)
                {
                    addUnresolved(node, node.expression);
                }
            }

            if (et != null || tt != null)
            {
                if (node.typeString != null && node.expression == null && tt != null)
                {
                    resolve(node, tt);
                    return;
                }
                if (node.typeString != null && node.expression != null && et != null && tt != null)
                {
                    if (!et.Equals(tt))
                    {
                        throw new ParserTypeMismatch(tt, et, node.token);
                    }
                    resolve(node, et);
                    return;
                }
                if (node.typeString == null && et != null)
                {
                    Debug.Assert(node.expression != null);
                    Debug.Assert(tt == null);
                    resolve(node, et);
                    return;
                }
            }
        }

        void checkType(AST.FunctionDefinition node)
        {
            FrontendType tt = null;
            if (node.typeString != null)
            {
                checkTypeDynamic(node.typeString);
                tt = getType(node.typeString);
                if (tt == null)
                {
                    addUnresolved(node, node.typeString);
                }
            }
            if (tt != null)
            {
                resolve(node, tt);
            }

            // TODO: find out why this has to come last
            // weird errors occur if this is further up!
            if (node.body != null)
            {
                checkTypeDynamic(node.body);
            }
        }

        void checkType(AST.StructConstructor node)
        {
            var td = node.scope.GetType(node.structName);

            if (td == null)
            {
                throw new ParserError($"Unknown type: \"{node.structName}\"", node.token);
            }

            FrontendStructType structType = null;
            if (td.type == null)
            {
                var t = getType(td.node);
                if (t == null)
                {
                    addUnresolved(node, td.node);
                }
                else
                {
                    if (!(t is FrontendStructType))
                    {
                        throw new ParserErrorExpected("struct type", t.name, node.token);
                    }
                    structType = t as FrontendStructType;
                }
            }
            else
            {
                Debug.Assert(td.type is FrontendStructType);
                structType = td.type as FrontendStructType;
            }

            List<FrontendType> argTypes = new List<FrontendType>();
            foreach (var arg in node.argumentList)
            {
                checkTypeDynamic(arg);
                var argt = getType(arg);
                if (argt != null)
                {
                    argTypes.Add(argt);
                }
                else
                {
                    addUnresolved(node, arg);
                }
            }

            if (structType != null && argTypes.Count == node.argumentList.Count)
            {
                for (int i = 0; i < argTypes.Count; ++i)
                {
                    if (!argTypes[i].Equals(structType.fields[i].type))
                    {
                        throw new ParserExpectedArgumentType(structType.fields[i].type, argTypes[i], i + 1, node.argumentList[i].token);
                    }
                }
                resolve(node, structType);
            }
        }

        void checkType(AST.StructDeclaration node)
        {

            FrontendStructType result;
            if (!pre_resolved.ContainsKey(node))
            {
                result = new FrontendStructType();
                result.name = node.name;
                pre_resolve(node, result);
            }
            else
            {
                result = pre_resolved[node] as FrontendStructType;
                Debug.Assert(result != null);
            }
            
            List<FrontendType> fieldTypes = new List<FrontendType>();
            foreach (var p in node.fields)
            {
                checkTypeDynamic(p.typeString);
                var pt = getType(p.typeString);
                if (pt != null)
                {
                    fieldTypes.Add(pt);
                }
                else
                {
                    addUnresolved(node, p.typeString);
                }
            }

            bool all_fs = fieldTypes.Count == node.fields.Count;
            if (all_fs)
            {
                
                for (int idx = 0; idx < node.fields.Count; ++idx)
                {
                    result.AddField(node.fields[idx].name, fieldTypes[idx]);
                }
                result.name = node.name;
                resolve(node, result);
            }
        }

        void checkType(AST.FunctionCall node)
        {
            var fun_vd = node.scope.GetVar(node.functionName);
            if (fun_vd == null)
            {
                throw new ParserError($"Unknown function of name: \"{node.functionName}\"", node.token);
            }

            FrontendFunctionType f_type = null;
            if (fun_vd.type != null)
            {
                Debug.Assert(fun_vd.type is FrontendFunctionType);
                f_type = fun_vd.type as FrontendFunctionType;
            }
            else
            {
                var t = getType(fun_vd.node);
                if (t == null)
                {
                    addUnresolved(node, fun_vd.node);
                }
                else
                {
                    if (!(t is FrontendFunctionType))
                    {
                        throw new ParserErrorExpected("funciton type", t.name, node.token);
                    }
                    f_type = t as FrontendFunctionType;
                }
            }
            List<FrontendType> argumentTypes = new List<FrontendType>();
            foreach (var arg in node.argumentList)
            {
                checkTypeDynamic(arg);
                var pt = getType(arg);
                if (pt != null)
                {
                    argumentTypes.Add(pt);
                }
                else
                {
                    addUnresolved(node, arg);
                }
            }
            if (f_type != null)
            {
                if (node.argumentList.Count != f_type.parameters.Count)
                {
                    throw new ParserError($"Function argument count mismatch! Got {node.argumentList.Count} expected {f_type.parameters.Count}.", node.token);
                }
                if (argumentTypes.Count == f_type.parameters.Count)
                {
                    for (int idx = 0; idx < argumentTypes.Count; ++idx)
                    {
                        var arg = argumentTypes[idx];
                        if (!arg.Equals(f_type.parameters[idx].type))
                        {
                            throw new ParserExpectedArgumentType(f_type.parameters[idx].type, arg, idx + 1, node.argumentList[idx].token);
                        }
                    }
                    node.callThroughPointer = !fun_vd.isConstant;
                    resolve(node, f_type.returnType);
                }
            }
        }

        void checkType(AST.VariableReference node)
        {
            Scope.VariableDefinition vd;
            if (node.vd == null)
            {
                Debug.Assert(node.variableName != null);
                vd = node.scope.GetVar(node.variableName);
                if (vd == null)
                {
                    throw new ParserError($"Unknown variable \"{node.variableName}\"", node.token);
                }
                if (!vd.isConstant)
                {
                    throw new ParserError("Non constant variables can't be accessesd prior to declaration", node.token);
                }
                node.vd = vd;
            }
            else
            {
                vd = node.vd;
            }

            Debug.Assert(vd != null);

            if (vd.type != null)
            {
                resolve(node, vd.type);
            }
            else
            {
                var vt = getType(vd.node);
                if (vt != null)
                {
                    if (vd.isFunctionParameter)
                    {
                        var ft = vt as FrontendFunctionType;
                        var pt = ft.parameters[vd.parameterIdx].type;
                        resolve(node, pt);
                    }
                    else
                    {
                        resolve(node, vt);
                    }
                }
                else
                {
                    addUnresolved(node, vd.node);
                }
            }
        }

        void checkType(AST.Assignment node)
        {
            checkTypeDynamic(node.left);
            var tt = getType(node.left);
            if (tt == null)
            {
                addUnresolved(node, node.left);
            }

            checkTypeDynamic(node.right);
            var et = getType(node.right);
            if (et == null)
            {
                addUnresolved(node, node.right);
            }

            if (tt != null && et != null)
            {
                if (!et.Equals(tt))
                {
                    throw new ParserVariableTypeMismatch(tt, et, node.token);
                }
                resolve(node, tt);
            }
        }

        void checkType(AST.ConstInt node)
        {
            resolve(node, FrontendType.i32);
        }

        void checkType(AST.ConstFloat node)
        {
            resolve(node, FrontendType.f32);
        }

        void checkType(AST.ConstBool node)
        {
            resolve(node, FrontendType.bool_);
        }

        void checkType(AST.ConstString node)
        {
            resolve(node, FrontendType.string_);
        }

        void checkType(AST.ArrayConstructor node)
        {
            if (node.elements.Count == 0)
            {
                throw new ParserError("zero sized array detected", node.token);
            }
            List<FrontendType> elementTypes = new List<FrontendType>();
            foreach (var e in node.elements)
            {
                checkTypeDynamic(e);
                var et = getType(e);
                if (et != null)
                {
                    elementTypes.Add(et);
                }
                else
                {
                    addUnresolved(node, e);
                }
            }
            if (elementTypes.Count == node.elements.Count)
            {
                bool same = true;
                var first = elementTypes.First();
                for (int i = 1; i < elementTypes.Count; ++i)
                {
                    if (!first.Equals(elementTypes[i]))
                    {
                        same = false;
                        break;
                    }
                }
                if (!same)
                {
                    throw new ParserError("all elements in an array must have the same type", node.token);
                }
                resolve(node, first);
            }
        }

        //void checkType(AST.UninitializedArray node)
        //{
        //    throw new NotImplementedException();
        //}

        void checkType(AST.StructFieldAccess node)
        {
            checkTypeDynamic(node.left);
            var lt = getType(node.left);
            if (lt == null)
            {
                addUnresolved(node, node.left);
            }
            else
            {
                var st = lt as FrontendStructType;
                if (st == null)
                {
                    if (lt is FrontendPointerType)
                    {
                        st = (lt as FrontendPointerType).elementType as FrontendStructType;
                        if (st != null)
                        {
                            node.IsArrow = true;
                        }
                    }
                }
                if (st == null)
                {
                    throw new ParserError("left side is not a struct type", node.token);
                }

                var field = st.GetField(node.fieldName);
                if (field == null)
                {
                    throw new ParserError($"struct does not contain field \"{node.fieldName}\"", node.token);
                }
                resolve(node, field);
            }
        }

        void checkType(AST.ArrayElementAccess node)
        {
            checkTypeDynamic(node.left);
            var lt = getType(node.left);
            FrontendArrayType at;
            if (lt == null)
            {
                addUnresolved(node, node.left);
                at = null;
            }
            else
            {
                at = lt as FrontendArrayType;
                if (at == null)
                {
                    throw new ParserError("left side is not a struct type", node.token);
                }
            }
            checkTypeDynamic(node.index);
            var idx_t = getType(node.index);
            if (idx_t == null)
            {
                addUnresolved(node, node.index);
            }
            else
            {
                if (!idx_t.Equals(FrontendType.i32))
                {
                    throw new ParserExpectedType(FrontendType.i32, idx_t, node.index.token);
                }
            }
            if (at != null && idx_t != null)
            {
                resolve(node, at.elementType);
            }
        }

        void checkType(AST.BreakLoop node)
        {
            resolve(node, FrontendType.none);
        }

        void checkType(AST.ContinueLoop node)
        {
            resolve(node, FrontendType.none);
        }

        void checkType(AST.ReturnFunction node)
        {
            FrontendType returnType = null;
            if (node.expression != null)
            {
                checkTypeDynamic(node.expression);
                var rt = getType(node.expression);
                if (rt != null)
                {
                    returnType = rt;
                    // TODO check if right type? get from scope?
                }
                else
                {
                    addUnresolved(node, node.expression);
                }
            }
            else
            {
                returnType = FrontendType.void_;
            }
            if (returnType != null)
            {
                resolve(node, returnType);
            }
        }

        void checkType(AST.BinOp node)
        {
            checkTypeDynamic(node.left);
            var lt = getType(node.left);
            if (lt == null)
            {
                addUnresolved(node, node.left);
            }
            checkTypeDynamic(node.right);
            var rt = getType(node.right);
            if (rt == null)
            {
                addUnresolved(node, node.right);
            }
            if (lt != null && rt != null)
            {
                if (node.isEither(AST.BinOp.BinOpType.LeftShift, AST.BinOp.BinOpType.RightShift))
                {
                    // TODO: suppport all integer types here.
                    if (!(FrontendType.IsIntegerType(lt) && FrontendType.IsIntegerType(rt)))
                    {
                        throw new ParserErrorExpected("two integer types", string.Format("{0} and {1}", lt, rt), node.token);
                    }
                }
                else if (lt is FrontendPointerType)
                {
                    if (!node.isEither(AST.BinOp.BinOpType.Add, AST.BinOp.BinOpType.Subract))
                    {
                        throw new ParserError("Only add and subtract are valid pointer arithmetic operations.", node.token);
                    }

                    // TODO: rather use umm and smm???
                    if (!FrontendType.IsIntegerType(rt))
                    {
                        throw new ParserError($"Right side of pointer arithmetic operation must be of integer type not \"{rt}\".", node.right.token);
                    }
                }
                else if (!lt.Equals(rt))
                {
                    throw new ParserTypeMismatch(lt, rt, node.token);
                }
                if (node.isEither(AST.BinOp.BinOpType.Less, AST.BinOp.BinOpType.LessEqual,
                    AST.BinOp.BinOpType.Greater, AST.BinOp.BinOpType.GreaterEqual,
                    AST.BinOp.BinOpType.Equal, AST.BinOp.BinOpType.NotEqual))
                {
                    resolve(node, FrontendType.bool_);
                }
                else
                {
                    resolve(node, lt);
                }
            }
        }

        void checkType(AST.UnaryOp node)
        {
            checkTypeDynamic(node.expression);
            var et = getType(node.expression);
            if (et == null)
            {
                addUnresolved(node, node.expression);
            }
            else
            {
                switch (node.type)
                {
                    case AST.UnaryOp.UnaryOpType.AddressOf:
                        // TODO can i take the address of everything?
                        resolve(node, new FrontendPointerType(et));
                        break;
                    case AST.UnaryOp.UnaryOpType.Dereference:
                        var pet = et as FrontendPointerType;
                        if (pet == null)
                        {
                            throw new ParserErrorExpected("Pointer type", et.ToString(), node.token);
                        }
                        resolve(node, pet.elementType);
                        break;
                    case AST.UnaryOp.UnaryOpType.SizeOf:
                        resolve(node, FrontendType.umm);
                        break;
                    default:
                        resolve(node, et);
                        break;
                }
            }
        }

        void checkType(AST.TypeCastOp node)
        {
            checkTypeDynamic(node.expression);
            var et = getType(node.expression);
            if (et == null)
            {
                addUnresolved(node, node.expression);
            }

            checkTypeDynamic(node.typeString);
            var tt = getType(node.typeString);
            if (tt == null)
            {
                addUnresolved(node, node.typeString);
            }

            if (et != null && tt != null)
            {
                // TODO: cast allowed?
                resolve(node, tt);
            }
        }

        void checkType(AST.TypeString node)
        {
            switch (node.kind)
            {
                case AST.TypeString.TypeKind.Other:
                {
                    var base_t_def = node.scope.GetType(node.typeName);
                    if (base_t_def == null)
                    {
                        throw new ParserError($"Unknown type: \"{node.typeName}\"", node.token);
                    }

                    FrontendType base_t = null;
                    if (base_t_def.type != null)
                    {
                        base_t = base_t_def.type;
                    }
                    else
                    {
                        if (pre_resolved.ContainsKey(base_t_def.node) && node.isPointerType)
                        {
                            base_t = pre_resolved[base_t_def.node];
                        }
                        var nt = getType(base_t_def.node);
                        if (nt == null)
                        {
                            addUnresolved(node, base_t_def.node);
                        }
                        else
                        {
                            base_t = nt;
                        }
                    }
                    if (base_t != null)
                    {
                        FrontendType result = base_t;
                        if (node.isArrayType)
                        {
                            Debug.Assert(!node.isPointerType);
                            result = new FrontendArrayType(base_t);
                        }
                        else if (node.isPointerType)
                        {
                            result = base_t;
                            for (int i = 0; i < node.pointerLevel; ++i)
                            {
                                result = new FrontendPointerType(result);
                            }
                        }
                        resolve(node, result);
                    }
                }
                break;
                case AST.TypeString.TypeKind.Function:
                {
                    var fts = node.functionTypeString;
                    List<FrontendType> parameterTypes = new List<FrontendType>();
                    foreach (var p in fts.parameters)
                    {
                        checkTypeDynamic(p.typeString);
                        var pt = getType(p.typeString);
                        if (pt != null)
                        {
                            parameterTypes.Add(pt);
                        }
                        else
                        {
                            addUnresolved(node, p.typeString);
                        }
                    }
                    bool all_ps = parameterTypes.Count == fts.parameters.Count;
                    checkTypeDynamic(fts.returnType);
                    var returnType = getType(fts.returnType);
                    if (returnType == null)
                    {
                        addUnresolved(node, fts.returnType);
                    }
                    if (returnType != null && all_ps)
                    {
                        var result = new FrontendFunctionType();
                        for (int idx = 0; idx < fts.parameters.Count; ++idx)
                        {
                            result.AddParam(fts.parameters[idx].name, parameterTypes[idx]);
                        }
                        result.returnType = returnType;
                        resolve(node, result);
                    }
                }
                break;
                case AST.TypeString.TypeKind.Struct:
                    throw new NotImplementedException();

            }
        }

        void checkType(AST.Node node)
        {
            throw new InvalidCodePath();
        }
    }
}

