using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PragmaScript
{
    class Token
    {
        public enum TokenType
        {
            WhiteSpace, Let, Var, Fun, Identifier, 
            OpenBracket, CloseBracket, Number, Assignment, Error, 
            Add, Subtract, Multiply, Divide, Semicolon, Comma
        }

        public TokenType type { get; private set; }
        public string text { get; private set; }
        public string errorMessage { get; private set; }
        public int lineNumber { get; private set; }
        public int pos { get; private set; }
        public int length { get; private set; }

        static Dictionary<string, TokenType> keywords;
        static Dictionary<string, TokenType> operators;


        static Token()
        {
            keywords = new Dictionary<string, TokenType>();
            keywords.Add("let", TokenType.Let);
            keywords.Add("var", TokenType.Var);

            operators = new Dictionary<string, TokenType>();
            operators.Add("=", TokenType.Assignment);
            operators.Add("(", TokenType.OpenBracket);
            operators.Add(")", TokenType.CloseBracket);
            operators.Add("+", TokenType.Add);
            operators.Add("-", TokenType.Subtract);
            operators.Add("*", TokenType.Multiply);
            operators.Add("/", TokenType.Divide);
            operators.Add(",", TokenType.Comma);
            operators.Add(";", TokenType.Semicolon);
        }

        public static bool isIdentifierChar(char c)
        {
            return char.IsLetter(c) || c == '_';
        }

        public static bool isKeyword(string identifier)
        {
            return keywords.ContainsKey(identifier);
        }

        public static bool isOperator(char c)
        {
            return operators.ContainsKey(c.ToString());
        }

        public static Token EmitError(Token t, string errorMessage)
        {
            t.errorMessage = errorMessage;
            t.type = TokenType.Error;
            return t;
        }
        public static Token Parse(string line, int pos, int lineNumber)
        {
            var t = new Token();
            t.pos = pos;
            t.lineNumber = lineNumber;
            t.length = 0;

            char current = line[pos];

            // first test if char is whitespace
            if (char.IsWhiteSpace(current))
            {
                while (char.IsWhiteSpace(current))
                {
                    t.length++;
                    pos++;
                    if (pos >= line.Length)
                        break;
                    current = line[pos];
                }

                t.type = TokenType.WhiteSpace;
                t.text = line.Substring(t.pos, t.length);
                return t;
            }


            // test if first char is a radix 10 digit
            if (char.IsDigit(current))
            {
                bool containsDecimalSeperator = false;
                while (char.IsDigit(current) || current == '.')
                {
                    // only one decimal seperator is allowed
                    if (current == '.' && containsDecimalSeperator)
                    {
                        t.text = line.Substring(t.pos, t.length);
                        return EmitError(t, "Only one decimal seperator is allowed!");
                    }
                    containsDecimalSeperator = current == '.';

                    t.length++;
                    pos++;
                    if (pos >= line.Length)
                        break;
                    current = line[pos];
                }
                t.type = TokenType.Number;
                t.text = line.Substring(t.pos, t.length);
                return t;
            }

            // if a token starts with a leter its either a keyword or an identifier
            if (char.IsLetter(current))
            {
                while (isIdentifierChar(current))
                {
                    t.length++;
                    pos++;
                    if (pos >= line.Length)
                        break;
                    current = line[pos];
                }

                var identifier = line.Substring(t.pos, t.length);
                t.text = identifier;

                // check if current identifier is a reserved keyword
                if (isKeyword(identifier))
                {
                    t.type = keywords[identifier];
                    return t;
                }
                else
                {
                    t.type = TokenType.Identifier;
                    return t;
                }
            }

            // check if current char is operator
            if (isOperator(current))
            {
                // TOOD: support combined operators of multipel chars ( e.g. +=, == )
                t.length = 1;
                t.type = operators[current.ToString()];
                t.text = line.Substring(t.pos, t.length);
                return t;
            }

            t.length = 1;
            t.text = line.Substring(t.pos, t.length);
            return EmitError(t, "Syntax error!");
        }

        public override string ToString()
        {
            if (type != TokenType.Error)
            {
                return string.Format("({0}, {1}, {2}, \"{3}\")", type.ToString(), lineNumber, pos, text);
            }
            else
            {
                return string.Format("({0}, {1}, {2}, \"{3}\")", "error: " + errorMessage, lineNumber, pos, text);
            }

        }
    }

}
