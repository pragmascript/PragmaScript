﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public static Token UndefinedRoot(string fn) => new Token("undefined") { type = TokenType.Undefined, text = "undefined", filename = fn };
        public static readonly Token Undefined = new Token("undefined") { type = TokenType.Undefined, text = "undefined" };
        public static Token NewLine(int pos, int line, string filename)
        {
            return new Token(filename) { type = TokenType.WhiteSpace, text = Environment.NewLine, length = 1, pos_idx = pos, line_idx = line };
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
            SliceBrackets,
            Dot,
            Struct,
            Enum,
            // ArrayTypeBrackets,
            SizeOf,
            Extern,
            Import,
            GreaterUnsigned,
            LessUnsigned,
            GreaterEqualUnsigned,
            LessEqualUnsigned,
            DivideEqualsUnsigned,
            DivideUnsigned,
            At,
            Module,
            With,
            Reserve,
            RightShiftUnsigned,
            RightShiftEqualsUnsigned,
            UnsignedCast,
            ModuleOp
        }

        public TokenType type { get; private set; }
        public string text { get; private set; }
        public string errorMessage { get; private set; }
        private int line_idx;
        private int pos_idx;
        public int Line { get { return line_idx + 1; } }
        public int Pos { get { return pos_idx + 1; } }
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
            keywords.Add("enum", TokenType.Enum);
            keywords.Add("fun", TokenType.Fun);
            // keywords.Add("alias", TokenType.Alias);
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
            keywords.Add("size_of", TokenType.SizeOf);
            keywords.Add("extern", TokenType.Extern);
            keywords.Add("import", TokenType.Import);
            keywords.Add("mod", TokenType.Module);
            keywords.Add("with", TokenType.With);

            operators = new Dictionary<string, TokenType>();
            operators.Add("=", TokenType.Assignment);
            operators.Add("(", TokenType.OpenBracket);
            operators.Add(")", TokenType.CloseBracket);
            operators.Add("[", TokenType.OpenSquareBracket);
            operators.Add("]", TokenType.CloseSquareBracket);
            operators.Add("[]", TokenType.SliceBrackets);
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
            operators.Add("::", TokenType.ModuleOp);
            operators.Add("<<", TokenType.LeftShift);
            operators.Add(">>", TokenType.RightShift);
            operators.Add(">>\\", TokenType.RightShiftUnsigned);
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
            operators.Add("/=\\", TokenType.DivideEqualsUnsigned);
            operators.Add("%=", TokenType.RemainderEquals);
            operators.Add("&=", TokenType.AndEquals);
            operators.Add("|=", TokenType.OrEquals);
            operators.Add("^=", TokenType.XorEquals);
            operators.Add("<<=", TokenType.LeftShiftEquals);
            operators.Add(">>=", TokenType.RightShiftEquals);
            operators.Add(">>=\\", TokenType.RightShiftEqualsUnsigned);
            // operators.Add("[]", TokenType.ArrayTypeBrackets);

            operators.Add("@", TokenType.At);
            operators.Add("@\\", TokenType.UnsignedCast);


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
                case TokenType.LeftShiftEquals:
                case TokenType.RightShiftEquals:
                case TokenType.RightShiftEqualsUnsigned:
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

        public static Token NextToken(string[] lines, ref int pos, ref int lineIdx, string filename)
        {
            var t = new Token(filename);
            t.type = TokenType.Undefined;
            t.pos_idx = pos;
            t.line_idx = lineIdx;
            t.length = 0;

            var line = lines[lineIdx];
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
                t.text = line.Substring(t.pos_idx, t.length);
                return t;
            }

            if (current == '/')
            {
                if (pos + 1 < line.Length)
                {
                    if (line[pos + 1] == '/')
                    {
                        t.type = TokenType.Comment;
                        t.text = line.Substring(t.pos_idx, line.Length - t.pos_idx);
                        t.length = t.text.Length;
                        pos += t.length;
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
                        t.text = line.Substring(t.pos_idx, t.length - 1);
                        throw new LexerError("String constant exceeds line!", t);
                    }
                    last = current;
                    current = line[pos];
                } while (current != '"' || last == '\\');
                pos++;
                t.length++;
                t.text = line.Substring(t.pos_idx, t.length);
                return t;
            }

            // verbatim strings
            if (current == '@')
            {
                if (pos + 1 < line.Length)
                {
                    if (line[pos + 1] == '"')
                    {

                        t.type = TokenType.String;
                        var sb = new StringBuilder();
                        pos++;
                        sb.Append('"');
                        while (true)
                        {
                            pos++;
                            if (pos >= line.Length)
                            {
                                if (lineIdx >= lines.Length)
                                {
                                    t.text = sb.ToString();
                                    throw new LexerError("Verbatim string constant exceeds file!", t);
                                }
                                lineIdx++;
                                pos = 0;
                                line = lines[lineIdx];
                                sb.Append(Environment.NewLine);
                            }
                            current = line[pos];
                            if (current == '"')
                            {
                                if (pos < line.Length - 1)
                                {
                                    if (line[pos + 1] == '"')
                                    {
                                        pos++;
                                        sb.Append('"');
                                    }
                                    else
                                    {
                                        pos++;
                                        sb.Append('"');
                                        break;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                sb.Append(current);
                            }
                        }
                        t.text = sb.ToString();
                        t.length = t.text.Length;
                        return t;
                    }
                }

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
                        t.text = line.Substring(t.pos_idx, t.length);
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
                t.text = line.Substring(t.pos_idx, t.length);
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

                var identifier = line.Substring(t.pos_idx, t.length);
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
            bool foundOperator = false;


            int tempPos = pos;
            while (operatorChars.Contains(current))
            {
                operatorSB.Append(current);
                var ops = operatorSB.ToString();
                // check if current char is operator
                if (operators.TryGetValue(ops, out op))
                {
                    t.length = ops.Length;
                    t.type = op;
                    t.text = line.Substring(t.pos_idx, t.length);
                    foundOperator = true;
                }
                tempPos++;
                if (tempPos >= line.Length)
                {
                    break;
                }
                current = line[tempPos];
            }

            // actually found an operator
            if (foundOperator)
            {
                pos += t.length;
                return t;
            }

            t.length = 1;
            t.text = line.Substring(t.pos_idx, t.length);
            throw new LexerError("Syntax error!", t);
        }

        public override string ToString()
        {
            if (type != TokenType.Error)
            {
                return string.Format("({0}, file \"{1}\", line {2}, pos {3}, \"{4}\")", type.ToString(), filename, Line, Pos, text);
            }
            else
            {
                return string.Format("({0}, file {1}, line {2}, pos {3}, \"{4}\")", "error: " + errorMessage, filename, Line, Pos, text);
            }

        }

        public string FilePosBackendString()
        {
            return $"(file \"{filename}\", line {Line}, pos {Pos})";
        }

        public static void Tokenize(List<Token> result, string text, string filename)
        {
            var lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            int lineIdx = 0;
            while (lineIdx < lines.Length)
            {
                var pos = 0;
                while (pos < lines[lineIdx].Length)
                {
                    var t = Token.NextToken(lines, ref pos, ref lineIdx, filename);
                    t.filename = filename;
                    result.Add(t);
                }

                var tnl = Token.NewLine(pos, lineIdx, filename);
                tnl.filename = filename;
                result.Add(tnl);
                lineIdx++;
            }

            var teof = new Token(filename);
            teof.type = TokenType.EOF;
            teof.line_idx = lines.Length;
            result.Add(teof);
        }

        public static bool IsBefore(Token a, Token b)
        {
            Debug.Assert(a.filename == b.filename);
            if (a.Line == b.Line)
            {
                Debug.Assert(a.Pos != b.Pos);
                return a.Pos < b.Pos;
            }
            return a.Line < b.Line;
        }

        public static bool IsAfter(Token a, Token b)
        {
            return IsBefore(b, a);
        }

    }

}
