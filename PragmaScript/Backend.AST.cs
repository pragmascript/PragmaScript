using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PragmaScript
{
    partial class Backend
    {

        public void TransformAST(AST.FileRoot root)
        {
            transform(root);
        }

        void transform(AST.FileRoot node)
        {
            foreach (var decl in node.declarations)
            {
                if (decl is AST.FunctionDefinition)
                {
                    transform(decl as AST.FunctionDefinition);
                }
            }
        }


        void transform(AST.FunctionDefinition node)
        {

        }


    }
}
