﻿using System;

namespace PragmaScript
{
    class InvalidCodePath : Exception
    {
        public InvalidCodePath()
            : base("Program should never get here!")
        { }
    }

    class LexerError : Exception
    {
        public LexerError(string message, Token t)
            : base(String.Format("error: {0} at {1}", message, t))
        {

        }

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
        public ParserExpectedType(FrontendType expected, FrontendType got, Token t)
            : base(string.Format("expected type \"{0}\", but got type \"{1}\"", expected.ToString(), got.ToString()), t)
        {

        }
    }

    class ParserExpectedArgumentType : ParserError
    {
        public ParserExpectedArgumentType(FrontendType expected, FrontendType got, int idx, Token t)
            : base(string.Format("function argument {2} has type \"{0}\", but got type \"{1}\"", expected.ToString(), got.ToString(), idx), t)
        {

        }
    }
    class ParserTypeMismatch : ParserError
    {
        public ParserTypeMismatch(FrontendType type1, FrontendType type2, Token t)
            : base(string.Format("Type mismatch: type {0} is not equal to {1}", type1.ToString(), type2.ToString()), t)
        {

        }
    }
    class ParserVariableTypeMismatch : ParserError
    {
        public ParserVariableTypeMismatch(FrontendType varType, FrontendType otherType, Token t)
            : base(string.Format("Type of expression does not match variable type: variable type {0} != expression type {1}", varType.ToString(), otherType.ToString()), t)
        {

        }
    }

    class UndefinedVariable : ParserError
    {
        public UndefinedVariable(string variableName, Token t)
            : base(string.Format("undefined variable \"{0}\"", variableName), t)
        {
        }
    }

    class UndefinedType : ParserError
    {
        public UndefinedType(string typeName, Token t)
            : base(string.Format("undefined type \"{0}\"", typeName), t)
        {
        }
    }

    class RedefinedVariable : ParserError
    {
        public RedefinedVariable(string variableName, Token t)
            : base(string.Format("variable \"{0}\" already defined", variableName), t)
        {

        }

    }
    class RedefinedFunction : ParserError
    {
        public RedefinedFunction(string functionName, Token t)
            : base(string.Format("function \"{0}\" already defined", functionName), t)
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

    class BackendException : Exception
    {
        public BackendException(string s)
            : base(s)
        {

        }
    }

}