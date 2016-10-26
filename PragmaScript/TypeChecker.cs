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
        public Scope scope;


        public UnresolvedType(AST.Node node, Scope scope)
        {
            this.node = node;
            this.scope = scope;
            waitingFor = new List<UnresolvedType>();
            blocking = new List<UnresolvedType>();
        }
    }

    class TypeChecker
    {
        Dictionary<AST.Node, FrontendType> knownTypes;
        Dictionary<AST.Node, UnresolvedType> unresolved;

        public TypeChecker()
        {
            unresolved = new Dictionary<AST.Node, UnresolvedType>();
            knownTypes = new Dictionary<AST.Node, FrontendType>();
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

        void addUnresolved(AST.Node from, AST.Node to, Scope fromScope)
        {
            Debug.Assert(from != null);
            Debug.Assert(to != null);
            Debug.Assert(fromScope != null);

            UnresolvedType u;
            if (!unresolved.TryGetValue(from, out u))
            {
                u = new UnresolvedType(from, fromScope);
                unresolved.Add(from, u);
            }
            var cu = unresolved[to];
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
                            checkTypeDynamic(b.node, b.scope);
                        }
                    }
                }
            }
        }

        public void CheckTypes(AST.Root root)
        {
            foreach (var n in root.declarations)
            {
                checkTypeDynamic(n, root.scope);
            }
        }

        void checkType(AST.Block node, Scope scope)
        {
            foreach (var n in node.statements)
            {
                checkTypeDynamic(n, scope);
            }
        }

        void checkTypeDynamic(AST.Node node, Scope scope)
        {
            if (knownTypes.ContainsKey(node))
            {
                return;
            }
            dynamic dn = node;
            checkType(dn, scope);
        }

        void checkType(AST.Elif node, Scope scope)
        {
            checkTypeDynamic(node.condition, scope);
            var ct = getType(node.condition);
            if (ct != null)
            {
                if (!ct.Equals(FrontendType.bool_))
                    throw new ParserExpectedType(FrontendType.bool_, ct, node.condition.token);
            }
            else
            {
                var u = new UnresolvedType(node, scope);
                var cu = unresolved[node.condition];
                u.waitingFor.Add(cu);
                cu.blocking.Add(u);
            }

            checkTypeDynamic(node.thenBlock, scope);
        }

        void checkType(AST.IfCondition node, Scope scope)
        {
            checkTypeDynamic(node.condition, scope);
            var ct = getType(node.condition);
            if (ct != null)
            {
                if (!ct.Equals(FrontendType.bool_))
                    throw new ParserExpectedType(FrontendType.bool_, ct, node.condition.token);
            }
            else
            {
                addUnresolved(node, node.condition, scope);
            }

            checkTypeDynamic(node.thenBlock, scope);

            foreach (var elif in node.elifs)
            {
                checkTypeDynamic(elif, scope);
            }

            if (node.elseBlock != null)
            {
                checkTypeDynamic(node.elseBlock, scope);
            }
        }

        void checkType(AST.ForLoop node, Scope scope)
        {
            var loopBodyScope = (node.loopBody as AST.Block).scope;
            foreach (var init in node.initializer)
            {
                checkTypeDynamic(init, loopBodyScope);
            }
            checkTypeDynamic(node.condition, loopBodyScope);
            var ct = getType(node.condition);
            if (ct != null)
            {
                if (!ct.Equals(FrontendType.bool_))
                    throw new ParserExpectedType(FrontendType.bool_, ct, node.condition.token);
            }
            else
            {
                addUnresolved(node, node.condition, scope);
            }
            foreach (var iter in node.iterator)
            {
                checkTypeDynamic(iter, loopBodyScope);
            }
            checkTypeDynamic(node.loopBody, scope);
        }

        void checkType(AST.WhileLoop node, Scope scope)
        {
            var loopBodyScope = (node.loopBody as AST.Block).scope;
            checkTypeDynamic(node.condition, loopBodyScope);
            var ct = getType(node.condition);
            if (ct != null)
            {
                if (!ct.Equals(FrontendType.bool_))
                    throw new ParserExpectedType(FrontendType.bool_, ct, node.condition.token);
            }
            else
            {
                addUnresolved(node, node.condition, scope);
            }
            checkTypeDynamic(node.loopBody, scope);
        }

        void checkType(AST.VariableDefinition node, Scope scope)
        {
            checkTypeDynamic(node.expression, scope);
            var et = getType(node.expression);
            if (et != null)
            {
                resolve(node, et);
            }
            else
            {
                addUnresolved(node, node.expression, scope);
            }
        }

        void checkType(AST.FunctionDefinition node, Scope scope)
        {
            List<FrontendType> parameterTypes = new List<FrontendType>();
            foreach (var p in node.parameters)
            {
                checkTypeDynamic(p.typeString, scope);
                var pt = getType(p.typeString);
                if (pt != null)
                {
                    parameterTypes.Add(pt);
                }
                else
                {
                    addUnresolved(node, p.typeString, scope);
                }
            }

            bool all_ps = parameterTypes.Count == node.parameters.Count;

            checkTypeDynamic(node.returnType, scope);
            var returnType = getType(node.returnType);
            if (returnType == null)
            {
                addUnresolved(node, node.returnType, scope);
            }

            if (!node.external)
            {
                checkTypeDynamic(node.body, scope);
            }

            if (returnType != null && all_ps)
            {
                var result = new FrontendFunctionType();
                for (int idx = 0; idx < node.parameters.Count; ++idx)
                {
                    result.AddParam(node.parameters[idx].name, parameterTypes[idx]);
                }
                result.returnType = returnType;
                result.name = node.funName;
                resolve(node, result);
            }
        }

        void checkType(AST.StructConstructor node, Scope scope)
        {
            var td = scope.GetType(node.structName);

            FrontendStructType structType;
            if (td.type == null)
            {
                var t = getType(td.node);
                if (!(t is FrontendStructType))
                {
                    throw new ParserErrorExpected("struct type", t.name, node.token);
                }
                structType = t as FrontendStructType;
                if (t == null)
                {
                    addUnresolved(node, td.node, scope);
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
                checkTypeDynamic(arg, scope);
                var argt = getType(arg);
                if (argt != null)
                {
                    argTypes.Add(argt);
                }
                else
                {
                    addUnresolved(node, arg, scope);
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

        void checkType(AST.StructDefinition node, Scope scope)
        {
            List<FrontendType> fieldTypes = new List<FrontendType>();
            foreach (var p in node.fields)
            {
                checkTypeDynamic(p.typeString, scope);
                var pt = getType(p.typeString);
                if (pt != null)
                {
                    fieldTypes.Add(pt);
                }
                else
                {
                    addUnresolved(node, p.typeString, scope);
                }
            }

            bool all_fs = fieldTypes.Count == node.fields.Count;

            if (all_fs)
            {
                var result = new FrontendStructType();
                for (int idx = 0; idx < node.fields.Count; ++idx)
                {
                    result.AddField(node.fields[idx].name, fieldTypes[idx]);
                }
                result.name = node.name;
                resolve(node, result);
            }
        }

        void checkType(AST.FunctionCall node, Scope scope)
        {
            var fun_vd = scope.GetVar(node.functionName);
            if (fun_vd == null)
            {
                throw new ParserError($"Unknown function of name: \"{node.functionName}\"", node.token);
            }

            FrontendFunctionType f_type;
            if (fun_vd.type != null)
            {
                Debug.Assert(fun_vd.type is FrontendFunctionType);
                f_type = fun_vd.type as FrontendFunctionType;
            }
            else
            {
                var t = getType(fun_vd.node);
                if (!(t is FrontendFunctionType))
                {
                    throw new ParserErrorExpected("funciton type", t.name, node.token);
                }
                f_type = t as FrontendFunctionType;
                if (t == null)
                {
                    addUnresolved(node, fun_vd.node, scope);
                }
            }

            if (node.argumentList.Count != f_type.parameters.Count)
            {
                throw new ParserError($"Function argument count mismatch! Got {node.argumentList.Count} expected {f_type.parameters.Count}.", node.token);
            }

            List<FrontendType> argumentTypes = new List<FrontendType>();
            foreach (var arg in node.argumentList)
            {
                checkTypeDynamic(arg, scope);
                var pt = getType(arg);
                if (pt != null)
                {
                    argumentTypes.Add(pt);
                }
                else
                {
                    addUnresolved(node, arg, scope);
                }
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
                resolve(node, f_type.returnType);
            }
        }

        void checkType(AST.VariableReference node, Scope scope)
        {
            var vd = scope.GetVar(node.variableName);

            if (node.variableName == "value")
            {
                int breakHere = 42;
            }
                

            Debug.Assert(vd != null);

            if (vd.type != null)
            {
                node.vd = vd;
                resolve(node, vd.type);
            }
            else
            {
                var vt = getType(vd.node);
                if (vt != null)
                {
                    resolve(node, vt);
                }
                else
                {
                    addUnresolved(node, vd.node, scope);
                }
            }
        }

        void checkType(AST.Assignment node, Scope scope)
        {
            checkTypeDynamic(node.target, scope);
            var tt = getType(node.target);
            if (tt == null)
            {
                addUnresolved(node, node.target, scope);
            }

            checkTypeDynamic(node.expression, scope);
            var et = getType(node.expression);
            if (et == null)
            {
                addUnresolved(node, node.expression, scope);
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

        void checkType(AST.ConstInt node, Scope scope)
        {
            resolve(node, FrontendType.i32);
        }

        void checkType(AST.ConstFloat node, Scope scope)
        {
            resolve(node, FrontendType.f32);
        }

        void checkType(AST.ConstBool node, Scope scope)
        {
            resolve(node, FrontendType.bool_);
        }

        void checkType(AST.ConstString node, Scope scope)
        {
            resolve(node, FrontendType.string_);
        }

        void checkType(AST.ArrayConstructor node, Scope scope)
        {
            if (node.elements.Count == 0)
            {
                throw new ParserError("zero sized array detected", node.token);
            }
            List<FrontendType> elementTypes = new List<FrontendType>();
            foreach (var e in node.elements)
            {
                checkTypeDynamic(e, scope);
                var et = getType(e);
                if (et != null)
                {
                    elementTypes.Add(et);
                }
                else
                {
                    addUnresolved(node, e, scope);
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

        void checkType(AST.UninitializedArray node, Scope scope)
        {
            throw new NotImplementedException();
        }

        void checkType(AST.StructFieldAccess node, Scope scope)
        {
            checkTypeDynamic(node.left, scope);
            var lt = getType(node.left);
            if (lt == null)
            {
                addUnresolved(node, node.left, scope);
            }
            else
            {
                var st = lt as FrontendStructType;
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

        void checkType(AST.ArrayElementAccess node, Scope scope)
        {
            checkTypeDynamic(node.left, scope);
            var lt = getType(node.left);
            FrontendArrayType at;
            if (lt == null)
            {
                addUnresolved(node, node.left, scope);
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

            checkTypeDynamic(node.index, scope);
            var idx_t = getType(node.index);
            if (idx_t == null)
            {
                addUnresolved(node, node.index, scope);
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

        void checkType(AST.BreakLoop node, Scope scope)
        {
        }

        void checkType(AST.ContinueLoop node, Scope scope)
        {
        }

        void checkType(AST.ReturnFunction node, Scope scope)
        {
            FrontendType returnType = null;
            if (node.expression != null)
            {
                checkTypeDynamic(node.expression, scope);
                var rt = getType(node.expression);
                if (rt != null)
                {
                    returnType = rt;
                    // TODO check if right type? get from scope?

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

        void checkType(AST.BinOp node, Scope scope)
        {
            checkTypeDynamic(node.left, scope);
            var lt = getType(node.left);
            if (lt == null)
            {
                addUnresolved(node, node.left, scope);
            }
            checkTypeDynamic(node.right, scope);
            var rt = getType(node.right);
            if (rt == null)
            {
                addUnresolved(node, node.right, scope);
            }
            if (lt != null && rt != null)
            {
                if (node.isEither(AST.BinOp.BinOpType.LeftShift, AST.BinOp.BinOpType.RightShift))
                {
                    // TODO: suppport all integer types here.
                    if (!lt.Equals(FrontendType.i32) || !rt.Equals(FrontendType.i32))
                    {
                        throw new ParserErrorExpected("two integer types", string.Format("{0} and {1}", lt, rt), node.token);
                    }
                }
                if (lt is FrontendPointerType)
                {
                    if (!node.isEither(AST.BinOp.BinOpType.Add, AST.BinOp.BinOpType.Subract))
                    {
                        throw new ParserError("Only add and subtract are valid pointer arithmetic operations.", node.token);
                    }

                    // TODO: rather use umm and smm???
                    if (!(rt.Equals(FrontendType.i32) || rt.Equals(FrontendType.i64) || rt.Equals(FrontendType.i8)))
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

        void checkType(AST.UnaryOp node, Scope scope)
        {
            checkTypeDynamic(node.expression, scope);
            var et = getType(node.expression);
            if (et == null)
            {
                addUnresolved(node, node.expression, scope);
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
                    default:
                        resolve(node, et);
                        break;
                }
            }
        }

        void checkType(AST.TypeCastOp node, Scope scope)
        {
            checkTypeDynamic(node.expression, scope);
            var et = getType(node.expression);
            if (et == null)
            {
                addUnresolved(node, node.expression, scope);
            }

            checkTypeDynamic(node.typeString, scope);
            var tt = getType(node.typeString);
            if (tt == null)
            {
                addUnresolved(node, node.typeString, scope);
            }

            if (et != null && tt != null)
            {
                // TODO: cast allowed?
                resolve(node, tt);
            }
        }

        void checkType(AST.TypeString node, Scope scope)
        {
            var base_t_def = scope.GetType(node.typeString);
            FrontendType base_t = null;
            if (base_t_def.type != null)
            {
                base_t = base_t_def.type;
            }
            else
            {
                var nt = getType(base_t_def.node);
                if (nt == null)
                {
                    addUnresolved(node, base_t_def.node, scope);
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

        void checkType(AST.Node node, Scope scope)
        {
            throw new InvalidCodePath();
        }
    }
}

