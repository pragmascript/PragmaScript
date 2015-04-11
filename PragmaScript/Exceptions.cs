using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PragmaScript
{
    class InvalidCodePath : Exception
    {
        public InvalidCodePath()
            : base("Program should never get here!")
        { }
    }

    class CompilerError : Exception
    {
        public CompilerError(string message, Token t)
            : base(String.Format("error: {0}! at {1}", message, t))
        {
        }
    }

    class CompilerErrorExpected : CompilerError
    {
        public CompilerErrorExpected(string expected, string got, Token t)
            : base(string.Format("expected \"{0}\", but got \"{1}\"", expected, got), t)
        {

        }
    }
}
