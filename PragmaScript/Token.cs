using System;
using System.Collections.Generic;
using System.Text;

namespace PragmaScript
{
    public class Token
    {

        public Token(string filename)
        {
            this.filename = filename;
        }
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

        public static readonly Token Undefined = new Token("undefined") { type = TokenType.Undefined, text = "undefined" };
        public static Token NewLine(int pos, int line, string filename)
        {
            return new Token(filename) { type = TokenType.WhiteSpace, text = Environment.NewLine, length = 1, pos = pos, lineNumber = line };
        }
        // public static readonly Token EOF = new Token { type = TokenType.EOF };
        public enum TokenType
        {
            WhiteSpace, Let, Var, Fun, Identifier,
            OpenBracket, CloseBracket, IntNumber, FloatNumber, Assignment, Error,
            Add, Subtract, Multiply, Divide, Remainder, Semicolon, Comma, Return,
            LeftShift, RightShift,
            ConditionalOR, ConditionalAND, LogicalOR, LogicalXOR, LogicalAND,
            Equal, NotEquals, Less, Greater, LessEqual, GreaterEqual,
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
            EOP,
            Elif,
            PlusEquals,
            RightShiftEquals,
            LeftShiftEquals,
            OrEquals,
            DivideEquals,
            AndEquals,
            RemainderEquals,
            MultiplyEquals,
            MinusEquals,
            XorEquals,
            Continue,
            Break,
            While,
            OpenSquareBracket,
            CloseSquareBracket,
            Dot,
            Struct,
            ArrayTypeBrackets,
            Alias,
            SizeOf,
            Extern,
            Import,
            Unsigned,
            GreaterUnsigned,
            LessUnsigned,
            GreaterEqualUnsigned,
            LessEqualUnsigned,
            DivideEqualsUnsigned,
            DivideUnsigned
        }

        public TokenType type { get; private set; }
        public string text { get; private set; }
        public string errorMessage { get; private set; }
        public int lineNumber { get; private set; }
        public int pos { get; private set; }
        public int length { get; private set; }
        public string filename { get; private set; }

        static Dictionary<string, TokenType> keywords;
        static Dictionary<string, TokenType> operators;
        static HashSet<char> operatorChars = new HashSet<char>();

        static Token()
        {
            keywords = new Dictionary<string, TokenType>();
            keywords.Add("let", TokenType.Let);
            keywords.Add("var", TokenType.Var);
            keywords.Add("struct", TokenType.Struct);
            keywords.Add("fun", TokenType.Fun);
            keywords.Add("alias", TokenType.Alias);
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
            keywords.Add("sizeof", TokenType.SizeOf);
            keywords.Add("extern", TokenType.Extern);
            keywords.Add("import", TokenType.Import);

            operators = new Dictionary<string, TokenType>();
            operators.Add("=", TokenType.Assignment);
            operators.Add("(", TokenType.OpenBracket);
            operators.Add(")", TokenType.CloseBracket);
            operators.Add("[", TokenType.OpenSquareBracket);
            operators.Add("]", TokenType.CloseSquareBracket);
            operators.Add("{", TokenType.OpenCurly);
            operators.Add("}", TokenType.CloseCurly);
            operators.Add("+", TokenType.Add);
            operators.Add("-", TokenType.Subtract);
            operators.Add("*", TokenType.Multiply);
            operators.Add("/", TokenType.Divide);
            operators.Add("/\\", TokenType.DivideUnsigned);
            operators.Add("%", TokenType.Remainder);
            operators.Add(",", TokenType.Comma);
            operators.Add(";", TokenType.Semicolon);
            operators.Add(":", TokenType.Colon);
            operators.Add("<<", TokenType.LeftShift);
            operators.Add(">>", TokenType.RightShift);
            operators.Add("||", TokenType.ConditionalOR);
            operators.Add("&&", TokenType.ConditionalAND);
            operators.Add("|", TokenType.LogicalOR);
            operators.Add("^", TokenType.LogicalXOR);
            operators.Add("&", TokenType.LogicalAND);
            operators.Add("==", TokenType.Equal);
            operators.Add("!=", TokenType.NotEquals);
            operators.Add(">", TokenType.Greater);
            operators.Add("<", TokenType.Less);
            operators.Add(">=", TokenType.GreaterEqual);
            operators.Add("<=", TokenType.LessEqual);
            operators.Add(">\\", TokenType.GreaterUnsigned);
            operators.Add("<\\", TokenType.LessUnsigned);
            operators.Add(">=\\", TokenType.GreaterEqualUnsigned);
            operators.Add("<=\\", TokenType.LessEqualUnsigned);
            operators.Add("\\", TokenType.Unsigned);
            operators.Add("!", TokenType.LogicalNOT);
            operators.Add("~", TokenType.Complement);
            operators.Add("++", TokenType.Increment);
            operators.Add("--", TokenType.Decrement);
            operators.Add("=>", TokenType.FatArrow);
            operators.Add(".", TokenType.Dot);
            operators.Add("+=", TokenType.PlusEquals);
            operators.Add("-=", TokenType.MinusEquals);
            operators.Add("*=", TokenType.MultiplyEquals);
            operators.Add("/=", TokenType.DivideEquals);
            operators.Add("/\\=", TokenType.DivideEqualsUnsigned);
            operators.Add("%=", TokenType.RemainderEquals);
            operators.Add("&=", TokenType.AndEquals);
            operators.Add("|=", TokenType.OrEquals);
            operators.Add("^=", TokenType.XorEquals);
            operators.Add("<<=", TokenType.LeftShiftEquals);
            operators.Add(">>=", TokenType.RightShiftEquals);
            operators.Add("[]", TokenType.ArrayTypeBrackets);

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
                case TokenType.XorEquals:
                case TokenType.OrEquals:
                case TokenType.DivideEquals:
                case TokenType.DivideEqualsUnsigned:
                case TokenType.AndEquals:
                case TokenType.RemainderEquals:
                case TokenType.MultiplyEquals:
                case TokenType.MinusEquals:
                    return true;
                default:
                    return false;
            }
        }

        public static Token Parse(string line, int pos, int lineNumber, string filename)
        {
            var t = new Token(filename);
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
                bool isHexadecimal = false;
                if (pos + 1 < line.Length)
                {
                    if (line[pos + 1] == 'x')
                    {
                        isHexadecimal = true;
                        pos += 2;
                        t.length += 2;
                    }
                }
                if (pos >= line.Length)
                {
                    current = '\0';
                }
                else
                {
                    current = line[pos];
                }


                while (char.IsDigit(current) || current == '.'
                    || (isHexadecimal && (current >= 'A' && current <= 'F')))
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
            if (char.IsLetter(current) || current == '_')
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
            throw new LexerError("Syntax error!", t);
        }

        public override string ToString()
        {
            if (type != TokenType.Error)
            {
                return string.Format("({0}, file \"{1}\", line {2}, pos {3}, \"{4}\")", type.ToString(), filename, lineNumber + 1, pos + 1, text);
            }
            else
            {
                return string.Format("({0}, file {1}, line {2}, pos {3}, \"{4}\")", "error: " + errorMessage, filename, lineNumber + 1, pos + 1, text);
            }

        }

        public static void Tokenize(List<Token> result, string text, string filename)
        {
            var lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            for (int i = 0; i < lines.Length; ++i)
            {
                var line = lines[i];
                var pos = 0;
                while (pos < line.Length)
                {
                    var t = Token.Parse(line, pos, i, filename);
                    t.filename = filename;
                    result.Add(t);
                    pos += t.length;
                }
                var tnl = Token.NewLine(pos, i, filename);
                tnl.filename = filename;
                result.Add(tnl);
            }

            var teof = new Token(filename);
            teof.type = TokenType.EOF;
            teof.lineNumber = lines.Length;
            result.Add(teof);
        }
    }

}
