using System;

namespace PragmaScript
{
    class InvalidCodePath : Exception
    {
        public InvalidCodePath()
            : base("Program should never get here!")
        { }
    }

    public class CompilerError : Exception
    {
        public string message;
        public Token token;
        public CompilerError(string message, Token t)
            : base(String.Format("error: {0} at {1}", message, t))
        {
            this.message = message;
            this.token = t;
        }
    }

    public class LexerError : CompilerError
    {
        public LexerError(string message, Token t)
            : base(message, t)
        {
        }
    }

    public class ParserErrorExpected : CompilerError
    {
        public ParserErrorExpected(string expected, string got, Token t)
            : base(string.Format("expected \"{0}\", but got \"{1}\"", expected, got), t)
        {
        }
    }
    public class ParserExpectedType : CompilerError
    {
        public ParserExpectedType(FrontendType expected, FrontendType got, Token t)
            : base(string.Format("expected type \"{0}\", but got type \"{1}\"", expected.ToString(), got.ToString()), t)
        {
        }
    }

    public class ParserExpectedArgumentType : CompilerError
    {
        public ParserExpectedArgumentType(FrontendType expected, FrontendType got, int idx, Token t)
            : base(string.Format("function argument {2} has type \"{0}\", but got type \"{1}\"", expected.ToString(), got.ToString(), idx), t)
        {
        }
    }
    public class ParserTypeMismatch : CompilerError
    {
        public ParserTypeMismatch(FrontendType type1, FrontendType type2, Token t)
            : base(string.Format("Type mismatch: type {0} is not equal to {1}", type1.ToString(), type2.ToString()), t)
        {
        }
    }
    public class ParserVariableTypeMismatch : CompilerError
    {
        public ParserVariableTypeMismatch(FrontendType varType, FrontendType otherType, Token t)
            : base(string.Format("Type of expression does not match variable type: variable type {0} != expression type {1}", varType.ToString(), otherType.ToString()), t)
        {
        }
    }

    public class UndefinedVariable : CompilerError
    {
        public UndefinedVariable(string variableName, Token t)
            : base(string.Format("undefined variable \"{0}\"", variableName), t)
        {
        }
    }

    public class UndefinedType : CompilerError
    {
        public UndefinedType(string typeName, Token t)
            : base(string.Format("undefined type \"{0}\"", typeName), t)
        {
        }
    }

    public class RedefinedVariable : CompilerError
    {
        public RedefinedVariable(string variableName, Token t)
            : base(string.Format("variable \"{0}\" already defined", variableName), t)
        {
        }

    }
    public class RedefinedFunction : CompilerError
    {
        public RedefinedFunction(string functionName, Token t)
            : base(string.Format("function \"{0}\" already defined", functionName), t)
        {
        }

    }
    public class RedefinedType : CompilerError
    {
        public RedefinedType(string typeName, Token t)
            : base(string.Format("type \"{0}\" already defined", typeName), t)
        {
        }
    }
}
