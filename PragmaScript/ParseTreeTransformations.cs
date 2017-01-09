using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static PragmaScript.AST;

namespace PragmaScript
{
    class ParseTreeTransformations
    {

        public static void Init(AST.ProgramRoot root)
        {
            fixupParents(null, root);
        }

        public static void Desugar(List<(AST.FieldAccess fa, Scope.Namespace ns)> namespaceAccesses, TypeChecker tc)
        {
            foreach (var na in namespaceAccesses)
            {
                Debug.Assert(na.fa.kind == FieldAccess.AccessKind.Namespace);
                var result = new VariableReference(na.fa.token, na.ns.scope);
                result.variableName = na.fa.fieldName;
                tc.ResolveNode(result, tc.GetNodeType(na.fa));
                na.fa.parent.Replace(na.fa, result);
            }
        }

        public static void Desugar(List<VariableReference> embeddings, TypeChecker tc)
        {
            foreach (var e in embeddings)
            {
                var ft = tc.GetNodeType(e.scope.function) as FrontendFunctionType;
                Debug.Assert(ft != null);

                var vd = e.scope.GetVar(e.variableName, e.token);

                var p = ft.parameters[vd.parameterIdx];
                var pt = p.type;
                // var pt = tc.GetNodeType(p.typeString);

                var vr = new AST.VariableReference(e.token, e.scope);
                vr.variableName = p.name;
                tc.ResolveNode(vr, pt);

                var f = new AST.FieldAccess(e.token, e.scope);
                f.fieldName = e.variableName;
                f.left = vr;
                f.parent = e.parent;
                f.IsArrow = pt is FrontendPointerType;
                f.returnPointer = e.returnPointer;

                FrontendStructType st;
                if (pt is FrontendPointerType fpt)
                {
                    st = fpt.elementType as FrontendStructType;
                }
                else
                {
                    st = pt as FrontendStructType;
                }
                Debug.Assert(st != null);
                tc.ResolveNode(f, st.fields[vd.embeddingIdx].type);
                vr.parent = f;


                e.parent.Replace(e, f);
            }
        }

        static void fixupParents(Node parent, Node node)
        {
            node.parent = parent;
            foreach (var c in node.GetChilds())
            {
                fixupParents(node, c);
            }
        }

    }
}
