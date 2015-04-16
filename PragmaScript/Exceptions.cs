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

    class ParserError : Exception
    {
        public ParserError(string message, Token t)
            : base(String.Format("error: {0} at {1}", message, t))
        {
        }
    }

    class ParserErrorExpected : ParserError
    {
        public ParserErrorExpected(string expected, string got, Token t)
            : base(string.Format("expected \"{0}\", but got \"{1}\"", expected, got), t)
        {

        }
    }
    class ParserExpectedType : ParserError
    {
        public ParserExpectedType(AST.VariableType expected, AST.VariableType got, Token t)
            : base(string.Format("expected type \"{0}\", but got type \"{1}\"", expected.name, got.name), t)
        {

        }
    }



    class ParserTypeMismatch : ParserError
    {
        public ParserTypeMismatch(AST.VariableType type1, AST.VariableType type2, Token t)
            : base(string.Format("Type mismatch: type {0} is not equal to {1}", type1.name, type2.name), t)
        {

        }
    }

    class ParserVariableTypeMismatch : ParserError
    {
        public ParserVariableTypeMismatch(AST.VariableType varType, AST.VariableType otherType, Token t)
            : base(string.Format("Type of expression does not match variable type: variable type {0} != expression type {1}", varType.name, otherType.name), t)
        {

        }
    }

    class UndefinedVariable : ParserError
    {
        public UndefinedVariable(string variableName, Token t)
            : base(string.Format("undefined varialbe \"{0}\"", variableName), t)
        {

        }

    }

    class RedefinedVariable : ParserError
    {
        public RedefinedVariable(string variableName, Token t)
            : base(string.Format("varialbe \"{0}\" already defined", variableName), t)
        {

        }

    }

    class RedefinedType : ParserError
    {
        public RedefinedType(string typeName, Token t)
            : base(string.Format("type \"{0}\" already defined", typeName), t)
        {

        }

    }

    class BackendTypeMismatchException : Exception
    {
        public BackendTypeMismatchException(Backend.BackendType type1, Backend.BackendType type2)
            : base(string.Format("Type mismatch: type {0} != type {1}", type1, type2))
        {

        }
    }
}
