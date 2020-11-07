using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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

    public class TypeChecker
    {
        Dictionary<FrontendType, AST.Node> typeRoots;
        Dictionary<AST.Node, FrontendType> knownTypes;
        Dictionary<AST.Node, UnresolvedType> unresolved;
        Dictionary<AST.Node, FrontendType> pre_resolved;

        public List<AST.VariableReference> embeddings;
        public List<(AST.FieldAccess fa, Scope.Module ns)> namespaceAccesses;

        public TypeChecker()
        {
            typeRoots = new Dictionary<FrontendType, AST.Node>();
            unresolved = new Dictionary<AST.Node, UnresolvedType>();
            knownTypes = new Dictionary<AST.Node, FrontendType>();
            pre_resolved = new Dictionary<AST.Node, FrontendType>();
            embeddings = new List<AST.VariableReference>();
            namespaceAccesses = new List<(AST.FieldAccess fa, Scope.Module ns)>();
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
                throw new CompilerError($"Cannot resolve type: {unresolved.First().Key}", unresolved.First().Key.token);
            }

        }

        public FrontendType GetNodeType(AST.Node node)
        {
            return getType(node, mustBeBound: true);
        }

        public AST.Node GetTypeRoot(FrontendType t)
        {
            typeRoots.TryGetValue(t, out var result);
            return result;
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
            if (knownTypes.TryGetValue(node, out result))
            {
                if (result is FrontendNumberType)
                {
                    var fnt = result as FrontendNumberType;
                    if (fnt.boundType != null)
                    {
                        return fnt.boundType;
                    }
                    else
                    {
                        if (mustBeBound)
                        {
                            return fnt.Default();
                        }
                    }

                }
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
            Debug.Assert(!knownTypes.ContainsKey(from));
            Debug.Assert(!knownTypes.ContainsKey(to));

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
            type.preResolved = false;
            // TODO(pragma): Sometimes a type will get resolved more than once. When does this happen?
            // can this be avoided?
            if (!knownTypes.ContainsKey(node))
            {
                knownTypes.Add(node, type);
            }
            else
            {
                Debug.Assert(knownTypes[node].Equals(type));
            }
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
            if (unresolved.TryGetValue(node, out var u))
            {
                if (u.blocking.Count > 0)
                {
                    var temp = new List<AST.Node>();
                    foreach (var b in u.blocking)
                    {
                        // NOTE(pragma): if the function is not satisfied with the pre-resolved type
                        // it will add it to the waitingFor list again so we can safely remove it here.
                        b.waitingFor.Remove(u);
                        if (b.waitingFor.Count == 0)
                        {
                            temp.Add(b.node);
                        }
                    }
                    foreach (var n in temp)
                    {
                        checkTypeDynamic(n);
                    }
                }
            }
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

        void checkType(AST.Module node)
        {
            foreach (var n in node.declarations)
            {
                checkTypeDynamic(n);
            }
            resolve(node, FrontendType.none);
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
                addUnresolved(node, node.condition);
            }
            checkTypeDynamic(node.thenBlock);
        }

        void checkType(AST.IfCondition node)
        {
            int elifsResolved = 0;
            foreach (var elif in node.elifs)
            {
                checkTypeDynamic(elif);
                elifsResolved++;
            }

            checkTypeDynamic(node.condition);
            var ct = getType(node.condition);
            if (ct != null && elifsResolved == node.elifs.Count)
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
                    if (!FrontendType.CompatibleAndLateBind(et, tt))
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
                    if (et is FrontendNumberType)
                    {
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
            if (node.typeString != null)
            {
                // node.typeString.fullyQualifiedName.name = node.funName;
                checkTypeDynamic(node.typeString);
                tt = getType(node.typeString);
                if (tt == null)
                {
                    addUnresolved(node, node.typeString);
                }
            }
            if (tt != null)
            {
                var ft = tt as FrontendFunctionType;
                Debug.Assert(ft != null);
                // TODO: put this somewhere it makes more sense.
                var cond = node.GetAttribute("CONDITIONAL");
                if (cond != null)
                {
                    if (CompilerOptions._i.debug)
                    {
                        if (cond != "DEBUG")
                        {
                            (tt as FrontendFunctionType).inactiveConditional = true;
                        }
                    }
                    else
                    {
                        if (cond != "RELEASE")
                        {
                            (tt as FrontendFunctionType).inactiveConditional = true;
                        }
                    }
                }

                List<(int i, int j, FrontendStructType.Field field)> embeddingFields = new List<(int, int, FrontendStructType.Field)>();

                bool waitForStructResolve = false;
                for (int i = 0; i < ft.parameters.Count; ++i)
                {
                    var p = ft.parameters[i];
                    if (p.embed)
                    {
                        var stn = p.type;
                        Debug.Assert(stn != null);
                        if (stn is FrontendPointerType pt)
                        {
                            stn = pt.elementType;
                        }
                        Debug.Assert(stn is FrontendStructType);
                        var st = stn as FrontendStructType;
                        if (st.preResolved)
                        {
                            addUnresolved(node, typeRoots[st]);
                            waitForStructResolve = true;
                            break;
                        }
                        else
                        {
                            for (int j = 0; j < st.fields.Count; ++j)
                            {
                                embeddingFields.Add((i, j, st.fields[j]));
                            }
                        }
                        if (waitForStructResolve)
                        {
                            break;
                        }
                    }
                }
                if (!waitForStructResolve)
                {
                    if (node.body != null)
                    {
                        var idx = 0;
                        foreach (var p in ft.parameters)
                        {
                            var vd = node.body.scope.AddVar(p.name, p.type, node.typeString.token);
                            vd.isFunctionParameter = true;
                            vd.parameterIdx = idx++;
                        }
                        foreach (var ef in embeddingFields)
                        {
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
                    result.calcTypeName();


                    if (!typeRoots.ContainsKey(tt))
                    {
                        typeRoots.Add(result, node);
                    }
                    resolve(node, result);
                }
                // TODO: find out why this has to come last
                // weird errors occur if this is further up!
                if (node.body != null)
                {
                    checkTypeDynamic(node.body);
                }
            }
        }

        void checkType(AST.CompoundLiteral node)
        {
            //var td = node.scope.GetType(node.structName);
            //if (td == null)
            //{
            //    throw new ParserError($"Unknown type: \"{node.structName}\"", node.token);
            //}

            FrontendStructType structType = null;
            FrontendVectorType vecType = null;
            checkTypeDynamic(node.typeString);
            var ts = getType(node.typeString);

            if (ts == null)
            {
                addUnresolved(node, node.typeString);
            }
            else
            {
                if (!(ts is FrontendStructType) && !(ts is FrontendVectorType))
                {
                    throw new ParserErrorExpected("struct type or vector type", ts.name, node.token);
                }
                structType = ts as FrontendStructType;
                vecType = ts as FrontendVectorType;
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
                if (node.argumentList.Count > structType.fields.Count)
                {
                    throw new CompilerError("The number of arguments in a compound literal can not exceed the number of fields in the struct.", node.argumentList[structType.fields.Count].token);
                }
                for (int i = 0; i < argTypes.Count; ++i)
                {
                    if (!FrontendType.CompatibleAndLateBind(argTypes[i], structType.fields[i].type))
                    {
                        throw new ParserExpectedArgumentType(structType.fields[i].type, argTypes[i], i + 1, node.argumentList[i].token);
                    }
                }
                resolve(node, structType);
            }
            if (vecType != null && argTypes.Count == node.argumentList.Count)
            {
                if (node.argumentList.Count != 0 && node.argumentList.Count != vecType.length)
                {
                    var token = node.argumentList.Count < vecType.length ? node.argumentList.Last().token : node.argumentList[vecType.length].token;
                    throw new CompilerError("The number of arguments in a compound literal must match the length of the vector type.", node.argumentList[structType.fields.Count].token);
                }

                for (int i = 0; i < argTypes.Count; ++i)
                {
                    if (!FrontendType.CompatibleAndLateBind(argTypes[i], vecType.elementType))
                    {
                        throw new ParserExpectedArgumentType(vecType.elementType, argTypes[i], i + 1, node.argumentList[i].token);
                    }
                }
                resolve(node, vecType);
            }
        }

        void checkType(AST.StructDeclaration node)
        {
            FrontendStructType result;

            if (!pre_resolved.ContainsKey(node))
            {
                var fqn = node.scope.GetFullyQualifiedName(node.name);
                result = new FrontendStructType(fqn);
                result.packed = node.packed;
                result.preResolved = true;
                if (!typeRoots.ContainsKey(result))
                {
                    typeRoots.Add(result, node);
                }
                pre_resolve(node, result);
            }
            else
            {
                result = pre_resolved[node] as FrontendStructType;
                if (!typeRoots.ContainsKey(result))
                {
                    typeRoots.Add(result, node);
                }
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
                result.name = node.scope.GetFullyQualifiedName(node.name);
                resolve(node, result);
            }
        }

        void checkType(AST.EnumDeclaration node)
        {
            FrontendEnumType result = new FrontendEnumType(node.name);
            // TODO(pragma): other integer types
            result.integerType = FrontendType.i32;
            if (!typeRoots.ContainsKey(result))
            {
                typeRoots.Add(result, node);
            }
            resolve(node, result);
        }

        void checkType(AST.FunctionCall node)
        {
            FrontendFunctionType f_type = null;

            checkTypeDynamic(node.left);
            var lt = getType(node.left);
            if (lt == null)
            {
                addUnresolved(node, node.left);
            }
            else
            {
                f_type = lt as FrontendFunctionType;
                if (f_type == null && !(lt is FrontendSumType))
                {
                    throw new CompilerError($"Variable is not a function.", node.token);
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

            if (lt is FrontendSumType st)
            {
                if (argumentTypes.Count < node.argumentList.Count)
                {
                    return;
                }
                Debug.Assert(argumentTypes.Count == node.argumentList.Count);
                var validTypes = new List<(int, FrontendFunctionType)>();
                for (int i = 0; i < st.types.Count; ++i)
                {
                    f_type = st.types[i] as FrontendFunctionType;
                    if (f_type != null)
                    {
                        if (node.argumentList.Count > f_type.parameters.Count)
                        {
                            continue;
                        }
                        bool validArguments = true;
                        for (int idx = 0; idx < f_type.parameters.Count; ++idx)
                        {
                            if (idx >= argumentTypes.Count)
                            {
                                if (!f_type.parameters[idx].optional)
                                {
                                    // too few arguments
                                    validArguments = false;
                                    break;
                                }
                            }
                            else
                            {
                                var arg = argumentTypes[idx];
                                if (!FrontendType.CompatibleAndLateBind(arg, f_type.parameters[idx].type))
                                {
                                    validArguments = false;
                                    break;
                                }
                            }
                        }
                        if (!validArguments)
                        {
                            continue;
                        }
                        else
                        {
                            validTypes.Add((i, f_type));
                        }
                    }
                }
                if (validTypes.Count == 0)
                {
                    throw new CompilerError($"Could not find matching overload.", node.token);
                }
                if (validTypes.Count > 1)
                {
                    // var ambigousLocations = string.Join(",", validTypes.Select(vt => (GetTypeRoot(st.types[vt.Item1]).token, vt.Item2)));
                    var ambigousLocations = string.Join(", ", validTypes.Select(vt => vt.Item2));
                    throw new CompilerError($"Overload is ambigous between {ambigousLocations}", node.token);
                }
                f_type = validTypes[0].Item2;

                // HACK: TODO(pragma): remove !!!
                var vr = (AST.VariableReference)node.left;
                vr.overloadedIdx = validTypes[0].Item1;
                // resolve(node.left, f_type);

                resolve(node, f_type.returnType);
                return;
            }


            if (f_type != null)
            {
                // TODO(pragma): ugly hack
                if (f_type.specialFun && f_type.funName == "len")
                {
                    if (argumentTypes.Count == node.argumentList.Count)
                    {
                        var arg_t = argumentTypes[0] as FrontendArrayType;
                        if (arg_t == null)
                        {
                            throw new CompilerError($"Argument type to \"len\" fuction must be array", node.argumentList[0].token);
                        }
                        if (arg_t.dims.Count > 1)
                        {
                            if (!(node.argumentList.Count == 2 || node.argumentList.Count == 1))
                            {
                                throw new CompilerError($"Function argument count mismatch! Got {node.argumentList.Count} expected 2 or 1 arguments.", node.token);
                            }
                            if (node.argumentList.Count == 2)
                            {
                                if (!FrontendType.CompatibleAndLateBind(argumentTypes[1], FrontendType.i32))
                                {
                                    throw new ParserExpectedArgumentType(FrontendType.i32, argumentTypes[1], 2, node.argumentList[1].token);
                                }
                            }
                        }
                        else
                        {
                            if (node.argumentList.Count != 1)
                            {
                                throw new CompilerError($"Function argument count mismatch! Got {node.argumentList.Count} expected {1}.", node.token);
                            }
                        }
                        resolve(node, f_type.returnType);
                    }
                }
                else
                {
                    if (node.argumentList.Count > f_type.parameters.Count)
                    {
                        throw new CompilerError($"Function argument count mismatch! Got {node.argumentList.Count} expected {f_type.parameters.Count}.", node.token);
                    }
                    if (argumentTypes.Count == node.argumentList.Count)
                    {
                        for (int idx = 0; idx < f_type.parameters.Count; ++idx)
                        {
                            if (idx >= argumentTypes.Count)
                            {
                                if (!f_type.parameters[idx].optional)
                                {
                                    throw new CompilerError($"Function argument count mismatch! Got {node.argumentList.Count} expected {f_type.parameters.Count}.", node.token);
                                }
                                // NOTE(pragma): all remaining parameters must be optional so so we just break out of the loop
                                break;
                            }
                            var arg = argumentTypes[idx];
                            if (!FrontendType.CompatibleAndLateBind(arg, f_type.parameters[idx].type))
                            {
                                throw new ParserExpectedArgumentType(f_type.parameters[idx].type, arg, idx + 1, node.argumentList[idx].token);
                            }
                        }
                        resolve(node, f_type.returnType);
                    }
                }
            }
        }

        void checkType(AST.VariableReference node)
        {
            Scope.OverloadedVariableDefinition ov = null;
            bool functionResolved = false;
            if (node.scope.function != null)
            {
                Debug.Assert(node.variableName != null);
                var fun = node.scope.function;
                var ftn = getType(fun);
                if (ftn == null)
                {
                    // in order to resolve @ embeddings we need to resolve the function first
                    addUnresolved(node, fun);
                }
                else
                {
                    functionResolved = true;
                }
            }
            if (node.scope.function == null || functionResolved)
            {
                if (node.modulePath != null)
                {
                    var ns = node.scope.GetModule(node.modulePath);
                    if (ns == null)
                    {
                        throw new CompilerError("Could not resolve module path", node.token);
                    }
                    node.scope = ns.scope;
                }

                ov = node.scope.GetVar(node.variableName, node.token);

                if (ov == null)
                {
                    throw new CompilerError($"Unknown variable \"{node.variableName}\"", node.token);
                }
                if (!ov.IsOverloaded)
                {
                    var vd = ov.First;
                    if (ov != null && vd.isEmbedded)
                    {
                        embeddings.Add(node);
                    }
                    else
                    {
                        var isLocal = (vd != null) && !vd.isGlobal && !vd.isConstant && !vd.isFunctionParameter && !vd.isNamespace;
                        if (isLocal && Token.IsBefore(node.token, vd.node.token))
                        {
                            throw new CompilerError("Variable can't be accessesd prior to declaration", node.token);
                        }
                    }
                }
                else
                {
                    // TODO(pragma): check for "Variable can't be accessesd prior to declaration"
                    // probably in resolution code?
                    // maybe have a flag that indicated whether a certain type is valid
                }
            }
            if (ov != null)
            {
                if (!ov.IsOverloaded)
                {
                    var vd = ov.First;
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
                else
                {
                    var types = new List<FrontendType>();
                    int resolvedTypes = 0;
                    for (int i = 0; i < ov.variables.Count; ++i)
                    {
                        types.Add(null);
                        var vd = ov.variables[i];
                        if (vd.type != null)
                        {
                            types[i] = vd.type;
                            resolvedTypes++;
                        }
                    }
                    if (resolvedTypes < ov.variables.Count)
                    {
                        for (int i = 0; i < types.Count; ++i)
                        {
                            var t = types[i];
                            if (t == null)
                            {
                                t = getType(ov.variables[i].node);
                                if (t == null)
                                {
                                    addUnresolved(node, ov.variables[i].node);
                                }
                                else
                                {
                                    types[i] = t;
                                    resolvedTypes++;
                                }
                            }
                        }
                    }
                    if (resolvedTypes == ov.variables.Count)
                    {
                        Debug.Assert(types.All(t => t != null));
                        var st = new FrontendSumType(types);

                        resolve(node, st);
                    }
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
                if (!FrontendType.CompatibleAndLateBind(et, tt))
                {
                    throw new ParserVariableTypeMismatch(tt, et, node.token);
                }
                // TODO(pragma): this should be in desugar?
                if (node.left is AST.IndexedElementAccess iea)
                {
                    if (GetNodeType(iea.left) is FrontendVectorType)
                    {
                        Debug.Assert(iea.indices.Count == 1);
                        node.type = AST.Assignment.AssignmentType.Vector;
                    }
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
            if (node.elements.Count == 0)
            {
                throw new CompilerError("zero sized array detected", node.token);
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
                if (first is FrontendNumberType)
                {
                    var nt = first as FrontendNumberType;
                    Debug.Assert(nt.boundType == null);
                    first = nt.Default();
                }
                for (int i = 1; i < elementTypes.Count; ++i)
                {
                    var et = elementTypes[i];
                    if (!FrontendType.CompatibleAndLateBind(first, et))
                    {
                        same = false;
                        break;
                    }
                }
                if (!same)
                {
                    throw new CompilerError("all elements in an array must be of the same type", node.token);
                }
                var at = new FrontendArrayType(first, node.dims);
                resolve(node, at);
            }
        }


        void checkType(AST.FieldAccess node)
        {
            checkTypeDynamic(node.left);
            var lt = getType(node.left);
            if (lt == null)
            {
                addUnresolved(node, node.left);
            }
            else
            {
                FrontendStructType st = lt as FrontendStructType;
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
                    throw new CompilerError("left side is not a struct type", node.token);
                }

                node.kind = AST.FieldAccess.AccessKind.Struct;
                if (st.preResolved)
                {
                    addUnresolved(node, typeRoots[st]);
                }
                else
                {
                    if (typeRoots.TryGetValue(st, out var tr))
                    {
                        // TODO(pragma): speed up linear search of field names
                        var np = ((AST.StructDeclaration)tr).GetField(node.fieldName);
                        if (np != null)
                        {
                            node.IsVolatile = np.isVolatile;
                        }
                    }
                    // TODO(pragma): speed up linear search of field names
                    var field = st.GetField(node.fieldName);
                    if (field == null)
                    {
                        throw new CompilerError($"struct does not contain field \"{node.fieldName}\"", node.token);
                    }
                    resolve(node, field);
                }
            }
        }

        void checkType(AST.IndexedElementAccess node)
        {
            checkTypeDynamic(node.left);
            var lt = getType(node.left);
            FrontendType et;
            if (lt == null)
            {
                addUnresolved(node, node.left);
                et = null;
            }
            else
            {
                // TODO(pragma): handle slice (and pointer
                switch (lt)
                {
                    case FrontendArrayType at:
                        {
                            et = at.elementType;
                            if (node.indices.Count > 1)
                            {
                                if (node.indices.Count != at.dims.Count)
                                {
                                    throw new CompilerError("Index count must be 1 or match dimension of array", node.token);
                                }
                            }
                        }
                        break;
                    case FrontendSliceType st:
                        et = st.elementType;
                        break;
                    case FrontendVectorType vt:
                        et = vt.elementType;
                        if (node.indices.Count != 1)
                        {
                            throw new CompilerError("When indexing into a vector you can only have one index", node.token);
                        }
                        break;
                    default:
                        throw new CompilerError("left side is not an array, vector or slice type", node.token);
                }
            }

            List<FrontendType> indexTypes = new List<FrontendType>();
            foreach (var idx in node.indices)
            {
                checkTypeDynamic(idx);
                var idx_t = getType(idx);
                if (idx_t != null)
                {
                    indexTypes.Add(idx_t);
                }
                else
                {
                    addUnresolved(node, idx);
                }
            }
            if (indexTypes.Count == node.indices.Count)
            {
                for (int i = 0; i < indexTypes.Count; ++i)
                {
                    var idx_t = indexTypes[i];
                    if (!FrontendType.CompatibleAndLateBind(idx_t, FrontendType.i32))
                    {
                        throw new ParserExpectedType(FrontendType.i32, idx_t, node.indices[i].token);
                    }
                }
            }

            if (et != null && indexTypes.Count == node.indices.Count)
            {
                resolve(node, et);
            }
        }

        void checkType(AST.SliceOp node)
        {
            checkTypeDynamic(node.left);
            var lt = getType(node.left);
            FrontendType st;

            if (lt == null)
            {
                addUnresolved(node, node.left);
                st = null;
            }
            else
            {
                // TODO(pragma): handle slice (and pointer
                switch (lt)
                {
                    case FrontendArrayType at:
                        st = new FrontendSliceType(at.elementType);

                        // TODO(pragma): HACK remove
                        if (!AST.ActivateReturnPointer(node.left))
                        {
                            throw new CompilerError("Cannot slice constant array", node.token);
                        }
                        if (node.to == null)
                        {
                            // TODO(pragma): move this to desugar????
                            var length = new AST.ConstInt(node.token, node.scope);
                            length.number = (ulong)at.Length;
                            node.to = length;
                        }
                        if (node.from == null)
                        {
                            // TODO(pragma): move this to desugar????
                            var from = new AST.ConstInt(node.token, node.scope);
                            from.number = (ulong)0;
                            node.from = from;
                        }
                        node.capacity = new AST.ConstInt(node.token, node.scope)
                        {
                            number = (ulong)at.Length
                        };
                        break;
                    case FrontendSliceType other_st:
                        st = other_st;
                        var otherSlice = node.left;

                        // TODO(pragma): move this to desugar????
                        var fa = new AST.FieldAccess(node.token, node.scope);
                        fa.fieldName = "data";
                        fa.left = otherSlice;
                        node.left = fa;
                        checkTypeDynamic(node.left);
                        Debug.Assert(getType(node.left) != null);

                        if (node.to == null)
                        {
                            var fa_to = new AST.FieldAccess(node.token, node.scope);
                            fa_to.fieldName = "length";
                            fa_to.left = otherSlice;
                            node.to = fa_to;
                            checkTypeDynamic(node.to);
                            var ntt = getType(node.to);
                            Debug.Assert(ntt != null);
                            Debug.Assert(FrontendType.CompatibleAndLateBind(ntt, FrontendType.i32));
                        }
                        if (node.from == null)
                        {
                            var from = new AST.ConstInt(node.token, node.scope);
                            from.number = (ulong)0;
                            node.from = from;
                        }
                        node.capacity = new AST.FieldAccess(node.token, node.scope)
                        {
                            fieldName = "length",
                            left = otherSlice,
                        };
                        break;
                    case FrontendPointerType pt:
                        st = new FrontendSliceType(pt.elementType);
                        if (node.to == null)
                        {
                            throw new CompilerError("Slice operator on a pointer type requires a \"to\" index.", node.token);
                        }
                        if (node.from == null)
                        {
                            var from = new AST.ConstInt(node.token, node.scope);
                            from.number = (ulong)0;
                            node.from = from;
                        }
                        node.capacity = new AST.ConstInt(node.token, node.scope)
                        {
                            number = (ulong)0
                        };
                        break;
                    default:
                        throw new CompilerError("left side is not an array, slice or pointer type", node.token);
                }
            }

            FrontendType from_t = null;
            if (node.from != null)
            {
                checkTypeDynamic(node.from);
                from_t = getType(node.from);
                if (from_t == null)
                {
                    addUnresolved(node, node.from);
                }
                else
                {
                    if (!FrontendType.CompatibleAndLateBind(from_t, FrontendType.i32))
                    {
                        throw new ParserExpectedType(FrontendType.i32, from_t, node.from.token);
                    }
                }
            }
            FrontendType to_t = null;
            if (node.to != null)
            {
                checkTypeDynamic(node.to);
                to_t = getType(node.to);
                if (to_t == null)
                {
                    addUnresolved(node, node.to);
                }
                else
                {
                    if (!FrontendType.CompatibleAndLateBind(to_t, FrontendType.i32))
                    {
                        throw new ParserExpectedType(FrontendType.i32, to_t, node.to.token);
                    }
                }
            }
            FrontendType c_t = null;
            if (node.capacity != null)
            {
                checkTypeDynamic(node.capacity);
                c_t = getType(node.capacity);
                if (c_t == null)
                {
                    addUnresolved(node, node.capacity);
                }
                else
                {
                    if (!FrontendType.CompatibleAndLateBind(c_t, FrontendType.i32))
                    {
                        throw new ParserExpectedType(FrontendType.i32, c_t, node.capacity.token);
                    }
                }
            }

            bool canResolve = true;
            if (c_t == null)
            {
                canResolve = false;
            }
            if (st == null)
            {
                canResolve = false;
            }
            if (node.from != null && from_t == null)
            {
                canResolve = false;
            }
            if (node.to != null && to_t == null)
            {
                canResolve = false;
            }
            if (canResolve)
            {
                resolve(node, st);
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
                if (rt == null)
                {
                    addUnresolved(node, node.expression);
                }
                returnType = rt;
            }
            else
            {
                returnType = FrontendType.void_;
            }

            var ft = getType(node.scope.function) as FrontendFunctionType;
            if (ft == null)
            {
                addUnresolved(node, node.scope.function);
            }

            if (returnType != null && ft != null)
            {
                if (FrontendType.CompatibleAndLateBind(returnType, ft.returnType))
                {
                    resolve(node, returnType);
                }
                else
                {
                    throw new ParserTypeMismatch(returnType, ft.returnType, node.token);
                }
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
                    if (lt is FrontendVectorType vt_left)
                    {
                        if (!(rt is FrontendVectorType vt_right))
                        {
                            throw new ParserTypeMismatch(lt, rt, node.token);
                        }
                        if (!FrontendType.IntegersOrLateBind(vt_left.elementType, vt_right.elementType))
                        {
                            throw new ParserErrorExpected("two integer types", string.Format("{0} and {1}", lt, rt), node.token);
                        }
                    }
                    else
                    {
                        if (lt is FrontendNumberType)
                        {
                            var lnt = lt as FrontendNumberType;
                            lt = lnt.Default();
                            lnt.Bind(lt);
                        }
                        if (rt is FrontendNumberType)
                        {
                            var rnt = rt as FrontendNumberType;
                            rt = rnt.Default();
                            rnt.Bind(rt);
                        }
                        if (!FrontendType.IntegersOrLateBind(lt, rt))
                        {
                            throw new ParserErrorExpected("two integer types", string.Format("{0} and {1}", lt, rt), node.token);
                        }
                    }
                }
                else if (lt is FrontendPointerType)
                {
                    if (!node.isEither(AST.BinOp.BinOpType.Add, AST.BinOp.BinOpType.Subract,
                                       AST.BinOp.BinOpType.Equal, AST.BinOp.BinOpType.NotEqual,
                                       AST.BinOp.BinOpType.GreaterUnsigned, AST.BinOp.BinOpType.GreaterEqualUnsigned,
                                       AST.BinOp.BinOpType.LessUnsigned, AST.BinOp.BinOpType.LessEqualUnsigned))
                    {
                        throw new CompilerError("Only add, subtract and unsigned comparisons are valid pointer operations.", node.token);
                    }

                    bool correctType = false;
                    correctType = correctType || FrontendType.IsIntegerOrLateBind(rt);
                    if (rt is FrontendPointerType)
                    {
                        var ltp = lt as FrontendPointerType;
                        var rtp = rt as FrontendPointerType;
                        if (ltp.elementType.Equals(rtp.elementType))
                        {
                            correctType = true;
                        }
                    }
                    // TODO: rather use umm and smm???
                    if (!correctType)
                    {
                        throw new CompilerError($"Right side of pointer arithmetic operation is not of supported type.", node.right.token);
                    }
                }
                else if (!FrontendType.CompatibleAndLateBind(lt, rt))
                {
                    throw new ParserTypeMismatch(lt, rt, node.token);
                }

                if (node.isEither(AST.BinOp.BinOpType.LessUnsigned, AST.BinOp.BinOpType.LessEqualUnsigned,
                    AST.BinOp.BinOpType.GreaterUnsigned, AST.BinOp.BinOpType.GreaterEqualUnsigned))
                {
                    if (!(lt is FrontendPointerType))
                    {
                        if (!FrontendType.IntegersOrLateBind(lt, rt))
                        {
                            throw new CompilerError($"Unsigned comparison operators are only valid for integer or pointer types not \"{lt}\".", node.right.token);
                        }
                    }
                }

                if (node.isEither(AST.BinOp.BinOpType.Less, AST.BinOp.BinOpType.LessEqual,
                    AST.BinOp.BinOpType.Greater, AST.BinOp.BinOpType.GreaterEqual,
                    AST.BinOp.BinOpType.LessUnsigned, AST.BinOp.BinOpType.LessEqualUnsigned,
                    AST.BinOp.BinOpType.GreaterUnsigned, AST.BinOp.BinOpType.GreaterEqualUnsigned,
                    AST.BinOp.BinOpType.Equal, AST.BinOp.BinOpType.NotEqual))
                {
                    resolve(node, FrontendType.bool_);
                }
                else
                {
                    if (node.type == AST.BinOp.BinOpType.DivideUnsigned)
                    {
                        if (!FrontendType.IntegersOrLateBind(lt, rt))
                        {
                            throw new CompilerError($"Unsigned division operator is only valid for integer types not \"{lt}\".", node.right.token);
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
                        if (et is FrontendArrayType at)
                        {
                            resolve(node, new FrontendPointerType(at.elementType));
                        }
                        else
                        {
                            resolve(node, new FrontendPointerType(et));
                        }
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
                if (FrontendType.AllowedTypeCastAndLateBind(et, tt))
                {
                    resolve(node, tt);
                }
                else
                {
                    throw new CompilerError("Cast not allowed for types.", node.token);
                }
            }
        }

        void checkType(AST.TypeString node)
        {
            switch (node.kind)
            {
                case AST.TypeString.TypeKind.Other:
                    {
                        var base_t_def = node.scope.GetType(node.fullyQualifiedName, node.token);
                        if (base_t_def == null)
                        {
                            throw new CompilerError($"Unknown type: \"{node.fullyQualifiedName}\"", node.token);
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
                                Debug.Assert(!node.isSliceType);
                                result = new FrontendArrayType(base_t, node.arrayDims);
                            }
                            else
                            if (node.isSliceType)
                            {
                                Debug.Assert(!node.isPointerType);
                                Debug.Assert(!node.isArrayType);
                                result = new FrontendSliceType(base_t);
                            }
                            else
                            if (node.isPointerType)
                            {
                                Debug.Assert(!node.isSliceType);
                                Debug.Assert(!node.isArrayType);
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
                        List<FrontendType> optionalExpressionTypes = new List<FrontendType>();
                        int optionalCount = 0;
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
                                if (p.typeString.isPointerType)
                                {
                                    var base_t_def = node.scope.GetType(p.typeString.fullyQualifiedName, p.typeString.token);
                                    if (pre_resolved.ContainsKey(base_t_def.node))
                                    {
                                        parameterTypes.Add(pre_resolved[base_t_def.node]);
                                    }
                                    else
                                    {
                                        addUnresolved(node, p.typeString);
                                    }
                                }
                                else
                                {
                                    addUnresolved(node, p.typeString);
                                }
                            }
                            if (p.defaultValueExpression != null)
                            {
                                optionalCount++;
                                checkTypeDynamic(p.defaultValueExpression);
                                var et = getType(p.defaultValueExpression);
                                if (et != null)
                                {
                                    optionalExpressionTypes.Add(et);
                                }
                                else
                                {
                                    addUnresolved(node, p.defaultValueExpression);
                                }
                            }
                        }
                        bool all_ps = parameterTypes.Count == fts.parameters.Count;

                        if (all_ps && optionalCount == optionalExpressionTypes.Count)
                        {
                            int o_idx = 0;
                            for (int i = 0; i < parameterTypes.Count; ++i)
                            {
                                if (fts.parameters[i].defaultValueExpression != null)
                                {
                                    var pt = parameterTypes[i];
                                    var opt = optionalExpressionTypes[o_idx++];
                                    if (!FrontendType.CompatibleAndLateBind(opt, pt))
                                    {
                                        throw new ParserExpectedArgumentType(pt, opt, i + 1, fts.parameters[i].defaultValueExpression.token);
                                    }
                                }
                            }
                        }

                        checkTypeDynamic(fts.returnType);
                        var returnType = getType(fts.returnType);
                        if (returnType == null)
                        {
                            addUnresolved(node, fts.returnType);
                        }
                        if (returnType != null && all_ps && optionalCount == optionalExpressionTypes.Count)
                        {
                            var result = new FrontendFunctionType(null);
                            result.returnType = returnType;
                            for (int idx = 0; idx < fts.parameters.Count; ++idx)
                            {
                                var p = fts.parameters[idx];
                                result.AddParam(p.name, parameterTypes[idx], p.isOptional(), p.embed);
                            }
                            result.calcTypeName();
                            resolve(node, result);
                        }
                    }
                    break;
                case AST.TypeString.TypeKind.Struct:
                    throw new NotImplementedException();

            }
        }

        void checkTypeDynamic(AST.Node node)
        {
            if (knownTypes.ContainsKey(node))
            {
                return;
            }
            switch (node)
            {
                case AST.FileRoot n:
                    checkType(n);
                    break;
                case AST.Module n:
                    checkType(n);
                    break;
                case AST.Block n:
                    checkType(n);
                    break;
                case AST.Elif n:
                    checkType(n);
                    break;
                case AST.IfCondition n:
                    checkType(n);
                    break;
                case AST.ForLoop n:
                    checkType(n);
                    break;
                case AST.WhileLoop n:
                    checkType(n);
                    break;
                case AST.VariableDefinition n:
                    checkType(n);
                    break;
                case AST.FunctionDefinition n:
                    checkType(n);
                    break;
                case AST.CompoundLiteral n:
                    checkType(n);
                    break;
                case AST.StructDeclaration n:
                    checkType(n);
                    break;
                case AST.EnumDeclaration n:
                    checkType(n);
                    break;
                case AST.FunctionCall n:
                    checkType(n);
                    break;
                case AST.VariableReference n:
                    checkType(n);
                    break;
                case AST.Assignment n:
                    checkType(n);
                    break;
                case AST.ConstInt n:
                    checkType(n);
                    break;
                case AST.ConstFloat n:
                    checkType(n);
                    break;
                case AST.ConstBool n:
                    checkType(n);
                    break;
                case AST.ConstString n:
                    checkType(n);
                    break;
                case AST.ArrayConstructor n:
                    checkType(n);
                    break;
                case AST.FieldAccess n:
                    checkType(n);
                    break;
                case AST.IndexedElementAccess n:
                    checkType(n);
                    break;
                case AST.BreakLoop n:
                    checkType(n);
                    break;
                case AST.ContinueLoop n:
                    checkType(n);
                    break;
                case AST.ReturnFunction n:
                    checkType(n);
                    break;
                case AST.BinOp n:
                    checkType(n);
                    break;
                case AST.UnaryOp n:
                    checkType(n);
                    break;
                case AST.TypeCastOp n:
                    checkType(n);
                    break;
                case AST.TypeString n:
                    checkType(n);
                    break;
                case AST.SliceOp n:
                    checkType(n);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}

