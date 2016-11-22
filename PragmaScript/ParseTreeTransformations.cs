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

        public static void Desugar(List<VariableReference> embeddings, TypeChecker tc)
        {
            foreach (var e in embeddings)
            {
                var p = e.scope.function.typeString.functionTypeString.parameters[e.vd.parameterIdx];
                var pt = tc.GetNodeType(p.typeString);
              
                var vr = new AST.VariableReference(e.token, e.scope);
                vr.variableName = p.name;
                vr.vd = new Scope.VariableDefinition();
                vr.vd.isFunctionParameter = true;
                vr.vd.parameterIdx = e.vd.parameterIdx;
                vr.vd.name = p.name;
                tc.ResolveNode(vr, tc.GetNodeType(p.typeString));

                var f = new AST.StructFieldAccess(e.token, e.scope);
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
                tc.ResolveNode(f, st.fields[e.vd.embeddingIdx].type);
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
