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

        public static readonly Token Undefined = new Token { type = TokenType.Undefined, text = "undefined" };
        public static Token NewLine(int pos, int line)
        {
            return new Token { type = TokenType.WhiteSpace, text = Environment.NewLine, length = 1, pos = pos, lineNumber = line };
        }
        public static readonly Token EOF = new Token { type = TokenType.EOF };
        public enum TokenType
        {
            WhiteSpace, Let, Var, Fun, Identifier,
            OpenBracket, CloseBracket, IntNumber, FloatNumber, Assignment, Error,
            Add, Subtract, Multiply, Divide, Remainder, Semicolon, Comma, Return,
            LeftShift, RightShift,
            ConditionalOR, ConditionalAND, LogicalOR, LogicalXOR, LogicalAND,
            Equal, NotEqual, Less, Greater, LessEqual, GreaterEqual,
            Undefined,
            LogicalNOT,
            Complement,
            Conditional,
            True,
            False,
            If,
            Else,
            OpenCurly,
            CloseCurly,
            For,
            Increment,
            Decrement,
            FatArrow,
            Colon,
            String,
            Comment,
            EOF,
            Elif,
            PlusEquals,
            RightShiftEquals,
            LeftShiftEquals,
            OREquals,
            DivideEquals,
            ANDEquals,
            RemainderEquals,
            MultiplyEquals,
            MinusEquals,
            XOREquals,
            Continue,
            Break,
            While
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
            keywords.Add("true", TokenType.True);
            keywords.Add("false", TokenType.False);
            keywords.Add("if", TokenType.If);
            keywords.Add("elif", TokenType.Elif);
            keywords.Add("else", TokenType.Else);
            keywords.Add("for", TokenType.For);
            keywords.Add("while", TokenType.While);
            keywords.Add("break", TokenType.Break);
            keywords.Add("continue", TokenType.Continue);


            operators = new Dictionary<string, TokenType>();
            operators.Add("=", TokenType.Assignment);
            operators.Add("(", TokenType.OpenBracket);
            operators.Add(")", TokenType.CloseBracket);
            operators.Add("{", TokenType.OpenCurly);
            operators.Add("}", TokenType.CloseCurly);
            operators.Add("+", TokenType.Add);
            operators.Add("-", TokenType.Subtract);
            operators.Add("*", TokenType.Multiply);
            operators.Add("/", TokenType.Divide);
            operators.Add("%", TokenType.Remainder);
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
            operators.Add("++", TokenType.Increment);
            operators.Add("--", TokenType.Decrement);
            operators.Add("=>", TokenType.FatArrow);

            operators.Add("+=", TokenType.PlusEquals);
            operators.Add("-=", TokenType.MinusEquals);
            operators.Add("*=", TokenType.MultiplyEquals);
            operators.Add("/=", TokenType.DivideEquals);
            operators.Add("%=", TokenType.RemainderEquals);
            operators.Add("&=", TokenType.ANDEquals);
            operators.Add("|=", TokenType.OREquals);
            operators.Add("^=", TokenType.XOREquals);
            operators.Add("<<=", TokenType.LeftShiftEquals);
            operators.Add(">>=", TokenType.RightShiftEquals);

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

        public bool isAssignmentOperator()
        {
            switch (type)
            {
                case TokenType.Assignment:
                case TokenType.PlusEquals:
                case TokenType.RightShiftEquals:
                case TokenType.LeftShiftEquals:
                case TokenType.XOREquals:
                case TokenType.OREquals:
                case TokenType.DivideEquals:
                case TokenType.ANDEquals:
                case TokenType.RemainderEquals:
                case TokenType.MultiplyEquals:
                case TokenType.MinusEquals:
                    return true;
                default:
                    return false;
            }
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

            if (current == '/')
            {
                if (pos + 1 < line.Length)
                {
                    if (line[pos + 1] == '/')
                    {
                        t.type = TokenType.Comment;
                        t.text = line.Substring(t.pos, line.Length - t.pos);
                        t.length = t.text.Length;
                        return t;
                    }
                }
            }

            if (current == '"')
            {
                t.type = TokenType.String;
                char last = current;
                do
                {
                    pos++;
                    t.length++;
                    if (pos >= line.Length)
                    {
                        t.text = line.Substring(t.pos, t.length - 1);
                        throw new LexerError("String constant exceeds line!", t);
                    }
                    last = current;
                    current = line[pos];
                } while (current != '"' || last == '\\');
                pos++;
                t.length++;
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
                        throw new LexerError("Only one decimal seperator is allowed!", t);
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
            throw new LexerError("Syntax error!");
        }

        public override string ToString()
        {
            if (type != TokenType.Error)
            {
                return string.Format("({0}, line {1}, pos {2}, \"{3}\")", type.ToString(), lineNumber + 1, pos + 1, text);
            }
            else
            {
                return string.Format("({0}, line {1}, pos {2}, \"{3}\")", "error: " + errorMessage, lineNumber + 1, pos + 1, text);
            }

        }
    }

}
