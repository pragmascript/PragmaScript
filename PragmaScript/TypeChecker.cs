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
        Dictionary<FrontendType, AST.Node> typeRoots;
        Dictionary<AST.Node, FrontendType> knownTypes;
        Dictionary<AST.Node, UnresolvedType> unresolved;
        Dictionary<AST.Node, FrontendType> pre_resolved;

        public List<AST.VariableReference> embeddings;
        public List<(AST.FieldAccess fa, Scope.Namespace ns)> namespaceAccesses;

        public TypeChecker()
        {
            typeRoots = new Dictionary<FrontendType, AST.Node>();
            unresolved = new Dictionary<AST.Node, UnresolvedType>();
            knownTypes = new Dictionary<AST.Node, FrontendType>();
            pre_resolved = new Dictionary<AST.Node, FrontendType>();
            embeddings = new List<AST.VariableReference>();
            namespaceAccesses = new List<(AST.FieldAccess fa, Scope.Namespace ns)>();
        }


        public void CheckTypes(AST.ProgramRoot root)
        {
            foreach (var fr in root.files) {
                checkTypeDynamic(fr);
            }
            foreach (var u in unresolved.Values) {
                Console.WriteLine(u.node.ToString());
            }
            if (unresolved.Count > 0) {
                throw new ParserError($"Cannot resolve type: {unresolved.First().Key}", unresolved.First().Key.token);
            }

        }

        public FrontendType GetNodeType(AST.Node node)
        {
            return getType(node, mustBeBound: true);
        }

        public void ResolveNode(AST.Node node, FrontendType ft)
        {
            resolve(node, ft);
        }

        public AST.FunctionDefinition GetFunctionDefinition(FrontendFunctionType fft)
        {
            var result = typeRoots[fft] as AST.FunctionDefinition;
            Debug.Assert(result != null);
            return result;
        }

        FrontendType getType(AST.Node node, bool mustBeBound = false)
        {
            FrontendType result;
            if (knownTypes.TryGetValue(node, out result)) {
                if (result is FrontendNumberType) {
                    var fnt = result as FrontendNumberType;
                    if (fnt.boundType != null) {
                        return fnt.boundType;
                    } else {
                        if (mustBeBound) {
                            return fnt.Default();
                        }
                    }

                }
                return result;
            } else {
                return null;
            }
        }

        void addUnresolved(AST.Node from, AST.Node to)
        {
            Debug.Assert(from != null);
            Debug.Assert(to != null);
            Debug.Assert(from != to);
            Debug.Assert(!knownTypes.ContainsKey(from));
            Debug.Assert(!knownTypes.ContainsKey(to));

            UnresolvedType u;
            if (!unresolved.TryGetValue(from, out u)) {
                u = new UnresolvedType(from);
                unresolved.Add(from, u);
            }

            UnresolvedType cu;
            if (!unresolved.TryGetValue(to, out cu)) {
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

            type.preResolved = false;
            knownTypes.Add(node, type);
            UnresolvedType u;
            if (unresolved.TryGetValue(node, out u)) {
                unresolved.Remove(node);
                if (u.blocking.Count > 0) {
                    foreach (var b in u.blocking) {
                        b.waitingFor.Remove(u);
                        if (b.waitingFor.Count == 0) {
                            checkTypeDynamic(b.node);
                        }
                    }
                }
            }


            if (pre_resolved.ContainsKey(node)) {
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
            if (t.waitingFor.Count > 0) {
                foreach (var wt in t.waitingFor) {
                    if (!blocker.Contains(wt.node)) {
                        getRootBlocker(wt, blocker);
                    }
                }
            } else {
                blocker.Add(t.node);
            }
        }

        void checkType(AST.FileRoot node)
        {
            foreach (var n in node.declarations) {
                checkTypeDynamic(n);
            }
            resolve(node, FrontendType.none);
        }

        void checkType(AST.Namespace node)
        {
            foreach (var n in node.declarations) {
                checkTypeDynamic(n);
            }
            resolve(node, FrontendType.none);
        }


        void checkTypeDynamic(AST.Node node)
        {
            if (knownTypes.ContainsKey(node)) {
                return;
            }
            dynamic dn = node;
            checkType(dn);
        }

        void checkType(AST.Block node)
        {
            foreach (var n in node.statements) {
                checkTypeDynamic(n);
            }
            resolve(node, FrontendType.none);
        }

        void checkType(AST.Elif node)
        {
            checkTypeDynamic(node.condition);
            var ct = getType(node.condition);

            if (ct != null) {
                if (!ct.Equals(FrontendType.bool_))
                    throw new ParserExpectedType(FrontendType.bool_, ct, node.condition.token);
                resolve(node, FrontendType.none);
            } else {
                addUnresolved(node, node.condition);
            }
            checkTypeDynamic(node.thenBlock);
        }

        void checkType(AST.IfCondition node)
        {

            int elifsResolved = 0;
            foreach (var elif in node.elifs) {
                checkTypeDynamic(elif);
                elifsResolved++;
            }

            checkTypeDynamic(node.condition);
            var ct = getType(node.condition);
            if (ct != null && elifsResolved == node.elifs.Count) {
                if (!ct.Equals(FrontendType.bool_))
                    throw new ParserExpectedType(FrontendType.bool_, ct, node.condition.token);
                resolve(node, FrontendType.none);
            } else {
                addUnresolved(node, node.condition);
            }

            checkTypeDynamic(node.thenBlock);


            if (node.elseBlock != null) {
                checkTypeDynamic(node.elseBlock);
            }
        }

        void checkType(AST.ForLoop node)
        {
            foreach (var init in node.initializer) {
                checkTypeDynamic(init);
            }
            checkTypeDynamic(node.condition);
            var ct = getType(node.condition);
            if (ct != null) {
                if (!ct.Equals(FrontendType.bool_))
                    throw new ParserExpectedType(FrontendType.bool_, ct, node.condition.token);
                resolve(node, FrontendType.none);
            } else {
                addUnresolved(node, node.condition);
            }
            foreach (var iter in node.iterator) {
                checkTypeDynamic(iter);
            }
            checkTypeDynamic(node.loopBody);
        }

        void checkType(AST.WhileLoop node)
        {
            checkTypeDynamic(node.condition);
            var ct = getType(node.condition);
            if (ct != null) {
                if (!ct.Equals(FrontendType.bool_))
                    throw new ParserExpectedType(FrontendType.bool_, ct, node.condition.token);
                resolve(node, FrontendType.none);
            } else {
                addUnresolved(node, node.condition);
            }
            checkTypeDynamic(node.loopBody);
        }

        void checkType(AST.VariableDefinition node)
        {
            Debug.Assert(node.typeString != null || node.expression != null);

            FrontendType tt = null;
            if (node.typeString != null) {
                checkTypeDynamic(node.typeString);
                tt = getType(node.typeString);
                if (tt == null) {
                    addUnresolved(node, node.typeString);
                }
            }
            FrontendType et = null;
            if (node.expression != null) {
                checkTypeDynamic(node.expression);
                et = getType(node.expression);
                if (et == null) {
                    addUnresolved(node, node.expression);
                }
            }

            if (et != null || tt != null) {
                if (node.typeString != null && node.expression == null && tt != null) {
                    resolve(node, tt);
                    return;
                }
                if (node.typeString != null && node.expression != null && et != null && tt != null) {
                    if (!FrontendType.CompatibleAndLateBind(et, tt)) {
                        throw new ParserTypeMismatch(tt, et, node.token);
                    }
                    resolve(node, et);
                    return;
                }
                if (node.typeString == null && et != null) {
                    Debug.Assert(node.expression != null);
                    Debug.Assert(tt == null);
                    if (et is FrontendNumberType) {
                        et = (et as FrontendNumberType).Default();
                    }
                    resolve(node, et);
                    return;
                }
            }
        }

        void checkType(AST.FunctionDefinition node)
        {


            FrontendType tt = null;
            if (node.typeString != null) {
                // node.typeString.fullyQualifiedName.name = node.funName;
                checkTypeDynamic(node.typeString);
                tt = getType(node.typeString);
                if (tt == null) {
                    addUnresolved(node, node.typeString);
                }
            }
            if (tt != null) {
                var ft = tt as FrontendFunctionType;
                Debug.Assert(ft != null);
                // TODO: put this somewhere it makes more sense.
                var cond = node.GetAttribute("CONDITIONAL");
                if (cond != null) {
                    if (CompilerOptions.debug) {
                        if (cond != "DEBUG") {
                            (tt as FrontendFunctionType).inactiveConditional = true;
                        }
                    } else {
                        if (cond != "RELEASE") {
                            (tt as FrontendFunctionType).inactiveConditional = true;
                        }
                    }
                }

                List<(int i, int j, FrontendStructType.Field field)> embeddingFields = new List<(int, int, FrontendStructType.Field)>();

                bool waitForStructResolve = false;
                for (int i = 0; i < ft.parameters.Count; ++i) {
                    var p = ft.parameters[i];
                    if (p.embed) {
                        var stn = p.type;
                        Debug.Assert(stn != null);
                        if (stn is FrontendPointerType pt) {
                            stn = pt.elementType;
                        }
                        Debug.Assert(stn is FrontendStructType);
                        var st = stn as FrontendStructType;
                        if (st.preResolved) {
                            addUnresolved(node, typeRoots[st]);
                            waitForStructResolve = true;
                            break;
                        } else {
                            for (int j = 0; j < st.fields.Count; ++j) {
                                embeddingFields.Add((i, j, st.fields[j]));
                            }
                        }
                        if (waitForStructResolve) {
                            break;
                        }
                    }
                }
                if (!waitForStructResolve) {
                    if (node.body != null) {
                        var idx = 0;
                        foreach (var p in ft.parameters) {
                            var vd = node.body.scope.AddVar(p.name, p.type, node.typeString.token);
                            vd.isFunctionParameter = true;
                            vd.parameterIdx = idx++;
                        }
                        foreach (var ef in embeddingFields) {
                            var field = ef.field;
                            var vd = node.body.scope.AddVar(field.name, field.type, node.typeString.token);
                            vd.isFunctionParameter = true;
                            vd.parameterIdx = ef.i;
                            vd.isEmbedded = true;
                            vd.embeddingIdx = ef.j;
                        }
                    }


                    var result = new FrontendFunctionType(node.funName);
                    result.inactiveConditional = ft.inactiveConditional;
                    result.parameters = ft.parameters;
                    result.returnType = ft.returnType;
                    result.specialFun = ft.specialFun;
                    result.preResolved = ft.preResolved;


                    if (!typeRoots.ContainsKey(tt)) {
                        typeRoots.Add(result, node);
                    }
                    resolve(node, result);
                }
                // TODO: find out why this has to come last
                // weird errors occur if this is further up!
                if (node.body != null) {
                    checkTypeDynamic(node.body);
                }
            }
        }

        void checkType(AST.StructConstructor node)
        {
            //var td = node.scope.GetType(node.structName);
            //if (td == null)
            //{
            //    throw new ParserError($"Unknown type: \"{node.structName}\"", node.token);
            //}

            FrontendStructType structType = null;
            checkTypeDynamic(node.typeString);
            var ts = getType(node.typeString);

            if (ts == null) {
                addUnresolved(node, node.typeString);
            } else {
                if (!(ts is FrontendStructType)) {
                    throw new ParserErrorExpected("struct type", ts.name, node.token);
                }
                structType = ts as FrontendStructType;
            }


            List<FrontendType> argTypes = new List<FrontendType>();
            foreach (var arg in node.argumentList) {
                checkTypeDynamic(arg);
                var argt = getType(arg);
                if (argt != null) {
                    argTypes.Add(argt);
                } else {
                    addUnresolved(node, arg);
                }
            }

            if (structType != null && argTypes.Count == node.argumentList.Count) {
                for (int i = 0; i < argTypes.Count; ++i) {
                    if (!FrontendType.CompatibleAndLateBind(argTypes[i], structType.fields[i].type)) {
                        throw new ParserExpectedArgumentType(structType.fields[i].type, argTypes[i], i + 1, node.argumentList[i].token);
                    }
                }
                resolve(node, structType);
            }
        }

        void checkType(AST.StructDeclaration node)
        {

            FrontendStructType result;
            if (!pre_resolved.ContainsKey(node)) {
                result = new FrontendStructType(node.name);
                result.preResolved = true;
                pre_resolve(node, result);
            } else {
                result = pre_resolved[node] as FrontendStructType;
            }
            if (!typeRoots.ContainsKey(result)) {
                typeRoots.Add(result, node);
            }

            List<FrontendType> fieldTypes = new List<FrontendType>();
            foreach (var p in node.fields) {
                checkTypeDynamic(p.typeString);
                var pt = getType(p.typeString);
                if (pt != null) {
                    fieldTypes.Add(pt);
                } else {
                    addUnresolved(node, p.typeString);
                }
            }

            bool all_fs = fieldTypes.Count == node.fields.Count;
            if (all_fs) {
                for (int idx = 0; idx < node.fields.Count; ++idx) {
                    result.AddField(node.fields[idx].name, fieldTypes[idx]);
                }
                result.name = node.name;
                resolve(node, result);
            }
        }

        void checkType(AST.FunctionCall node)
        {
            FrontendFunctionType f_type = null;

            checkTypeDynamic(node.left);
            var lt = getType(node.left);
            if (lt == null) {
                addUnresolved(node, node.left);
            } else {
                f_type = lt as FrontendFunctionType;
                Debug.Assert(f_type != null);
            }

            List<FrontendType> argumentTypes = new List<FrontendType>();
            foreach (var arg in node.argumentList) {
                checkTypeDynamic(arg);
                var pt = getType(arg);
                if (pt != null) {
                    argumentTypes.Add(pt);
                } else {
                    addUnresolved(node, arg);
                }
            }
            if (f_type != null) {
                if (node.argumentList.Count > f_type.parameters.Count) {
                    throw new ParserError($"Function argument count mismatch! Got {node.argumentList.Count} expected {f_type.parameters.Count}.", node.token);
                }
                if (argumentTypes.Count == node.argumentList.Count) {
                    for (int idx = 0; idx < f_type.parameters.Count; ++idx) {
                        if (idx >= argumentTypes.Count) {
                            if (!f_type.parameters[idx].optional) {
                                throw new ParserError($"Function argument count mismatch! Got {node.argumentList.Count} expected {f_type.parameters.Count}.", node.token);
                            }
                            break;
                        }
                        var arg = argumentTypes[idx];
                        if (!FrontendType.CompatibleAndLateBind(arg, f_type.parameters[idx].type)) {
                            throw new ParserExpectedArgumentType(f_type.parameters[idx].type, arg, idx + 1, node.argumentList[idx].token);
                        }
                    }
                }

                resolve(node, f_type.returnType);
            }
        }

        void checkType(AST.VariableReference node)
        {
            Scope.VariableDefinition vd = null;
            Scope.Namespace ns = null;
            bool functionResolved = false;
            if (node.scope.function != null) {
                Debug.Assert(node.variableName != null);
                var fun = node.scope.function;
                var ftn = getType(fun);
                if (ftn == null) {
                    // in order to resolve @ embeddings we need to resolve the function first
                    addUnresolved(node, fun);
                } else {
                    functionResolved = true;
                }
            }
            if (node.scope.function == null || functionResolved) {
                vd = node.scope.GetVar(node.variableName, node.token);
                ns = node.scope.GetNamespace(node.variableName);
                if (vd == null && ns == null) {
                    throw new ParserError($"Unknown variable \"{node.variableName}\"", node.token);
                }
                if (vd != null && vd.isEmbedded) {
                    embeddings.Add(node);
                } else
                // TODO: How to do this???
                if (false) {
                    throw new ParserError("Variable can't be accessesd prior to declaration", node.token);
                }
            }

            if (vd != null) {
                if (vd.type != null) {
                    resolve(node, vd.type);
                } else {
                    var vt = getType(vd.node);
                    if (vt != null) {
                        if (vd.isFunctionParameter) {
                            var ft = vt as FrontendFunctionType;
                            var pt = ft.parameters[vd.parameterIdx].type;
                            resolve(node, pt);
                        } else {
                            resolve(node, vt);
                        }
                    } else {
                        addUnresolved(node, vd.node);
                    }
                }
            } else if (ns != null) {
                resolve(node, FrontendType.none);
            }
        }

        void checkType(AST.Assignment node)
        {
            checkTypeDynamic(node.left);
            var tt = getType(node.left);
            if (tt == null) {
                addUnresolved(node, node.left);
            }

            checkTypeDynamic(node.right);
            var et = getType(node.right);
            if (et == null) {
                addUnresolved(node, node.right);
            }

            if (tt != null && et != null) {
                if (!FrontendType.CompatibleAndLateBind(et, tt)) {
                    throw new ParserVariableTypeMismatch(tt, et, node.token);
                }
                resolve(node, tt);
            }
        }

        void checkType(AST.ConstInt node)
        {
            resolve(node, new FrontendNumberType(false));
        }

        void checkType(AST.ConstFloat node)
        {
            resolve(node, new FrontendNumberType(true));
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
            if (node.elements.Count == 0) {
                throw new ParserError("zero sized array detected", node.token);
            }
            List<FrontendType> elementTypes = new List<FrontendType>();
            foreach (var e in node.elements) {
                checkTypeDynamic(e);
                var et = getType(e);
                if (et != null) {
                    elementTypes.Add(et);
                } else {
                    addUnresolved(node, e);
                }
            }
            if (elementTypes.Count == node.elements.Count) {
                bool same = true;
                var first = elementTypes.First();
                if (first is FrontendNumberType) {
                    var nt = first as FrontendNumberType;
                    Debug.Assert(nt.boundType == null);
                    first = nt.Default();
                }
                for (int i = 1; i < elementTypes.Count; ++i) {
                    if (!FrontendType.CompatibleAndLateBind(first, elementTypes[i])) {
                        same = false;
                        break;
                    }
                }
                if (!same) {
                    throw new ParserError("all elements in an array must have the same type", node.token);
                }
                resolve(node, first);
            }
        }


        void checkType(AST.FieldAccess node)
        {
            checkTypeDynamic(node.left);
            var lt = getType(node.left);
            if (lt == null) {
                addUnresolved(node, node.left);
            } else {
                FrontendStructType st = lt as FrontendStructType;
                if (st == null) {
                    if (lt is FrontendPointerType) {
                        st = (lt as FrontendPointerType).elementType as FrontendStructType;
                        if (st != null) {
                            node.IsArrow = true;
                        }
                    }
                }

                bool isNamespace = false;
                if (st != null) {
                    node.kind = AST.FieldAccess.AccessKind.Struct;
                    if (st.preResolved) {
                        addUnresolved(node, typeRoots[st]);
                    } else {
                        var field = st.GetField(node.fieldName);
                        if (field == null) {
                            throw new ParserError($"struct does not contain field \"{node.fieldName}\"", node.token);
                        }
                        resolve(node, field);
                    }

                } else {
                    // assume its a namespace
                    if (lt.Equals(FrontendType.none)) {
                        var ns = node.scope.GetNamespace(node.left.token.text);
                        if (ns != null) {
                            isNamespace = true;
                            node.kind = AST.FieldAccess.AccessKind.Namespace;
                            var vd = ns.scope.GetVar(node.fieldName, node.token, recurse: false);

                            if (vd == null) {
                                new ParserError($"Unknown variable \"{node.fieldName}\" in namespace \"{ns}\"", node.token);
                            }

                            if (vd.type != null) {
                                resolve(node, vd.type);
                            } else {
                                var vt = getType(vd.node);
                                if (vt != null) {
                                    namespaceAccesses.Add((node, ns));
                                    Debug.Assert(vd.isFunctionParameter == false);
                                    resolve(node, vt);
                                } else {
                                    addUnresolved(node, vd.node);
                                }
                            }
                        }
                    }
                }
                if (st == null && !isNamespace) {
                    throw new ParserError("left side is not a struct type or namespace", node.token);
                }

            }
        }

        void checkType(AST.ArrayElementAccess node)
        {
            checkTypeDynamic(node.left);
            var lt = getType(node.left);
            FrontendArrayType at;
            if (lt == null) {
                addUnresolved(node, node.left);
                at = null;
            } else {
                at = lt as FrontendArrayType;
                if (at == null) {
                    throw new ParserError("left side is not a struct type", node.token);
                }
            }
            checkTypeDynamic(node.index);
            var idx_t = getType(node.index);
            if (idx_t == null) {
                addUnresolved(node, node.index);
            } else {
                if (!FrontendType.CompatibleAndLateBind(idx_t, FrontendType.i32)) {
                    throw new ParserExpectedType(FrontendType.i32, idx_t, node.index.token);
                }
            }
            if (at != null && idx_t != null) {
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
            if (node.expression != null) {
                checkTypeDynamic(node.expression);
                var rt = getType(node.expression);
                if (rt == null) {
                    addUnresolved(node, node.expression);
                }
                returnType = rt;
            } else {
                returnType = FrontendType.void_;
            }

            var ft = getType(node.scope.function) as FrontendFunctionType;
            if (ft == null) {
                addUnresolved(node, node.scope.function);
            }

            if (returnType != null && ft != null) {
                if (FrontendType.CompatibleAndLateBind(returnType, ft.returnType)) {
                    resolve(node, returnType);
                } else {
                    throw new ParserTypeMismatch(returnType, ft.returnType, node.token);
                }
            }
        }

        void checkType(AST.BinOp node)
        {
            checkTypeDynamic(node.left);
            var lt = getType(node.left);
            if (lt == null) {
                addUnresolved(node, node.left);
            }
            checkTypeDynamic(node.right);
            var rt = getType(node.right);
            if (rt == null) {
                addUnresolved(node, node.right);
            }
            if (lt != null && rt != null) {
                if (node.isEither(AST.BinOp.BinOpType.LeftShift, AST.BinOp.BinOpType.RightShift)) {

                    if (lt is FrontendNumberType) {
                        var lnt = lt as FrontendNumberType;
                        lt = lnt.Default();
                        lnt.Bind(lt);
                    }
                    if (rt is FrontendNumberType) {
                        var rnt = rt as FrontendNumberType;
                        rt = rnt.Default();
                        rnt.Bind(rt);
                    }
                    if (!FrontendType.IntegersOrLateBind(lt, rt)) {
                        throw new ParserErrorExpected("two integer types", string.Format("{0} and {1}", lt, rt), node.token);
                    }
                } else if (lt is FrontendPointerType) {
                    if (!node.isEither(AST.BinOp.BinOpType.Add, AST.BinOp.BinOpType.Subract,
                                       AST.BinOp.BinOpType.Equal, AST.BinOp.BinOpType.NotEqual,
                                       AST.BinOp.BinOpType.GreaterUnsigned, AST.BinOp.BinOpType.GreaterEqualUnsigned,
                                       AST.BinOp.BinOpType.LessUnsigned, AST.BinOp.BinOpType.LessEqualUnsigned)) {
                        throw new ParserError("Only add, subtract and unsigned comparisons are valid pointer operations.", node.token);
                    }

                    bool correctType = false;
                    correctType = correctType || FrontendType.IsIntegerOrLateBind(rt);
                    if (rt is FrontendPointerType) {
                        var ltp = lt as FrontendPointerType;
                        var rtp = rt as FrontendPointerType;
                        if (ltp.elementType.Equals(rtp.elementType)) {
                            correctType = true;
                        }
                    }
                    // TODO: rather use umm and smm???
                    if (!correctType) {
                        throw new ParserError($"Right side of pointer arithmetic operation is not of supported type.", node.right.token);
                    }
                } else if (!FrontendType.CompatibleAndLateBind(lt, rt)) {
                    throw new ParserTypeMismatch(lt, rt, node.token);
                }

                if (node.isEither(AST.BinOp.BinOpType.LessUnsigned, AST.BinOp.BinOpType.LessEqualUnsigned,
                    AST.BinOp.BinOpType.GreaterUnsigned, AST.BinOp.BinOpType.GreaterEqualUnsigned)) {
                    if (!(lt is FrontendPointerType)) {
                        if (!FrontendType.IntegersOrLateBind(lt, rt)) {
                            throw new ParserError($"Unsigned comparison operators are only valid for integer or pointer types not \"{lt}\".", node.right.token);
                        }
                    }
                }

                if (node.isEither(AST.BinOp.BinOpType.Less, AST.BinOp.BinOpType.LessEqual,
                    AST.BinOp.BinOpType.Greater, AST.BinOp.BinOpType.GreaterEqual,
                    AST.BinOp.BinOpType.LessUnsigned, AST.BinOp.BinOpType.LessEqualUnsigned,
                    AST.BinOp.BinOpType.GreaterUnsigned, AST.BinOp.BinOpType.GreaterEqualUnsigned,
                    AST.BinOp.BinOpType.Equal, AST.BinOp.BinOpType.NotEqual)) {
                    resolve(node, FrontendType.bool_);
                } else {
                    if (node.type == AST.BinOp.BinOpType.DivideUnsigned) {
                        if (!FrontendType.IntegersOrLateBind(lt, rt)) {
                            throw new ParserError($"Unsigned division operator is only valid for integer types not \"{lt}\".", node.right.token);
                        }
                    }

                    resolve(node, lt);
                }
            }
        }

        void checkType(AST.UnaryOp node)
        {
            checkTypeDynamic(node.expression);
            var et = getType(node.expression);
            if (et == null) {
                addUnresolved(node, node.expression);
            } else {
                switch (node.type) {
                    case AST.UnaryOp.UnaryOpType.AddressOf:
                        // TODO can i take the address of everything?
                        resolve(node, new FrontendPointerType(et));
                        break;
                    case AST.UnaryOp.UnaryOpType.Dereference:
                        var pet = et as FrontendPointerType;
                        if (pet == null) {
                            throw new ParserErrorExpected("Pointer type", et.ToString(), node.token);
                        }
                        resolve(node, pet.elementType);
                        break;
                    case AST.UnaryOp.UnaryOpType.SizeOf:
                        resolve(node, FrontendType.mm);
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
            if (et == null) {
                addUnresolved(node, node.expression);
            }

            checkTypeDynamic(node.typeString);
            var tt = getType(node.typeString);
            if (tt == null) {
                addUnresolved(node, node.typeString);
            }

            if (et != null && tt != null) {
                if (FrontendType.AllowedTypeCastAndLateBind(et, tt)) {
                    resolve(node, tt);
                } else {
                    throw new ParserError("Cast not allowed for types.", node.token);
                }
            }
        }

        void checkType(AST.TypeString node)
        {

            switch (node.kind) {
                case AST.TypeString.TypeKind.Other: {
                        var base_t_def = node.scope.GetType(node.fullyQualifiedName, node.token);
                        if (base_t_def == null) {
                            throw new ParserError($"Unknown type: \"{node.fullyQualifiedName}\"", node.token);
                        }
                        FrontendType base_t = null;
                        if (base_t_def.type != null) {
                            base_t = base_t_def.type;
                        } else {
                            if (pre_resolved.ContainsKey(base_t_def.node) && node.isPointerType) {
                                base_t = pre_resolved[base_t_def.node];
                            }
                            var nt = getType(base_t_def.node);
                            if (nt == null) {
                                addUnresolved(node, base_t_def.node);
                            } else {
                                base_t = nt;
                            }
                        }
                        if (base_t != null) {
                            FrontendType result = base_t;
                            if (node.isArrayType) {
                                Debug.Assert(!node.isPointerType);
                                result = new FrontendArrayType(base_t);
                            } else if (node.isPointerType) {
                                result = base_t;
                                for (int i = 0; i < node.pointerLevel; ++i) {
                                    result = new FrontendPointerType(result);
                                }
                            }
                            resolve(node, result);
                        }
                    }
                    break;
                case AST.TypeString.TypeKind.Function: {
                        var fts = node.functionTypeString;
                        List<FrontendType> parameterTypes = new List<FrontendType>();
                        List<FrontendType> optionalExpressionTypes = new List<FrontendType>();
                        int optionalCount = 0;
                        foreach (var p in fts.parameters) {
                            checkTypeDynamic(p.typeString);
                            var pt = getType(p.typeString);
                            if (pt != null) {
                                parameterTypes.Add(pt);
                            } else {
                                addUnresolved(node, p.typeString);
                            }
                            if (p.defaultValueExpression != null) {
                                optionalCount++;
                                checkTypeDynamic(p.defaultValueExpression);
                                var et = getType(p.defaultValueExpression);
                                if (et != null) {
                                    optionalExpressionTypes.Add(et);
                                } else {
                                    addUnresolved(node, p.defaultValueExpression);
                                }
                            }
                        }
                        bool all_ps = parameterTypes.Count == fts.parameters.Count;

                        if (all_ps && optionalCount == optionalExpressionTypes.Count) {
                            int o_idx = 0;
                            for (int i = 0; i < parameterTypes.Count; ++i) {
                                if (fts.parameters[i].defaultValueExpression != null) {
                                    var pt = parameterTypes[i];
                                    var opt = optionalExpressionTypes[o_idx++];
                                    if (!FrontendType.CompatibleAndLateBind(opt, pt)) {
                                        throw new ParserExpectedArgumentType(pt, opt, i + 1, fts.parameters[i].defaultValueExpression.token);
                                    }
                                }
                            }
                        }

                        checkTypeDynamic(fts.returnType);
                        var returnType = getType(fts.returnType);
                        if (returnType == null) {
                            addUnresolved(node, fts.returnType);
                        }
                        if (returnType != null && all_ps && optionalCount == optionalExpressionTypes.Count) {
                            var result = new FrontendFunctionType(null);
                            for (int idx = 0; idx < fts.parameters.Count; ++idx) {
                                var p = fts.parameters[idx];
                                result.AddParam(p.name, parameterTypes[idx], p.isOptional(), p.embed);
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

