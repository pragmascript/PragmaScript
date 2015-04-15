using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PragmaScript
{
    class Token
    {

      /*
      * primary = 0;
      * Unary = 1;
      * Multiplicative = 2;
      * Additive = 3;
      * Shift = 4;
      * Relational = 5
      * Equality = 6
      * LAND = 7
      * LXOR = 8
      * LOR = 9
      * CAND = 10
      * COR = 11
      * Conditional = 12
      * Assignment = 13
      */

        public static Token Undefined = new Token { type = TokenType.Undefined, text = "undefined" };
        public enum TokenType
        {
            WhiteSpace, Let, Var, Fun, Identifier,
            OpenBracket, CloseBracket, IntNumber, FloatNumber, Assignment, Error,
            Add, Subtract, Multiply, Divide, Modulo, Semicolon, Comma, Return,
            LeftShift, RightShift,
            ConditionalOR, ConditionalAND, LogicalOR, LogicalXOR, LogicalAND,
            Equal, NotEqual, Less, Greater, LessEqual, GreaterEqual,
            Undefined,
            LogicalNOT,
            Complement,
            Conditional
        }

        public TokenType type { get; private set; }
        public string text { get; private set; }
        public string errorMessage { get; private set; }
        public int lineNumber { get; private set; }
        public int pos { get; private set; }
        public int length { get; private set; }

        static Dictionary<string, TokenType> keywords;
        static Dictionary<string, TokenType> operators;
        static HashSet<char> operatorChars = new HashSet<char>();

        static Token()
        {
            keywords = new Dictionary<string, TokenType>();
            keywords.Add("let", TokenType.Let);
            keywords.Add("var", TokenType.Var);
            keywords.Add("return", TokenType.Return);
            operators = new Dictionary<string, TokenType>();
            operators.Add("=", TokenType.Assignment);
            operators.Add("(", TokenType.OpenBracket);
            operators.Add(")", TokenType.CloseBracket);
            operators.Add("+", TokenType.Add);
            operators.Add("-", TokenType.Subtract);
            operators.Add("*", TokenType.Multiply);
            operators.Add("/", TokenType.Divide);
            operators.Add("%", TokenType.Modulo);
            operators.Add(",", TokenType.Comma);
            operators.Add(";", TokenType.Semicolon);
            operators.Add("<<", TokenType.LeftShift);
            operators.Add(">>", TokenType.RightShift);
            operators.Add("||", TokenType.ConditionalOR);
            operators.Add("&&", TokenType.ConditionalAND);
            operators.Add("|", TokenType.LogicalOR);
            operators.Add("^", TokenType.LogicalXOR);
            operators.Add("&", TokenType.LogicalAND);
            operators.Add("==", TokenType.Equal);
            operators.Add("!=", TokenType.NotEqual);
            operators.Add(">", TokenType.Greater);
            operators.Add("<", TokenType.Less);
            operators.Add(">=", TokenType.GreaterEqual);
            operators.Add("<=", TokenType.LessEqual);
            operators.Add("!", TokenType.LogicalNOT);
            operators.Add("~", TokenType.Complement);

            foreach (var op in operators.Keys)
            {
                foreach (var oc in op)
                {
                    operatorChars.Add(oc);
                }
            }
        }

        public static bool isIdentifierChar(char c)
        {
            return char.IsLetter(c) || c == '_' || char.IsDigit(c);
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
            t.type = TokenType.Undefined;
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
                    containsDecimalSeperator |= current == '.';

                    t.length++;
                    pos++;
                    if (pos >= line.Length)
                        break;
                    current = line[pos];
                }
                t.type = containsDecimalSeperator ? TokenType.FloatNumber : TokenType.IntNumber;
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

            var operatorSB = new StringBuilder();
            TokenType op = TokenType.Undefined; 
            while (operatorChars.Contains(current))
            {
                operatorSB.Append(current);
                var ops = operatorSB.ToString();
                // check if current char is operator
                if (operators.TryGetValue(ops, out op))
                {
                    t.length = ops.Length;
                    t.type = op;
                    t.text = line.Substring(t.pos, t.length);
                }
                pos++;
                if (pos >= line.Length)
                    break;
                current = line[pos];
            }

            // actually found an operator
            if (op != TokenType.Undefined)
            {
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
