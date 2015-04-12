using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PragmaScript
{
    class AST
    {
        public abstract class Node
        {
            public virtual IEnumerable<Node> GetChilds()
            {
                yield break;
            }
        }

        public class Block : Node
        {
            public List<Node> statements = new List<Node>();
            public override IEnumerable<Node> GetChilds()
            {
                foreach (var s in statements)
                    yield return s;
            }
            public override string ToString()
            {
                return "Block";
            }
        }

        public class VariableDeclaration : Node
        {
            public string variableName;
            public Node expression;

            public override IEnumerable<Node> GetChilds()
            {
                yield return expression;
            }

            public override string ToString()
            {
                return "var " + variableName + " = ";
            }
        }

        public class FunctionCall : Node
        {
            public string functionName;
            public List<Node> argumentList = new List<Node>();

            public override IEnumerable<Node> GetChilds()
            {
                foreach (var exp in argumentList)
                {
                    yield return exp;
                }
            }
            public override string ToString()
            {
                return functionName + "()";
            }
        }

        public class VariableLookup : Node
        {
            public string variableName;

            public override string ToString()
            {
                return variableName;
            }
        }

        public class Assignment : Node
        {
            public string variableName;
            public Node expression;

            public override IEnumerable<Node> GetChilds()
            {
                yield return expression;
            }

            public override string ToString()
            {
                return variableName + " = ";
            }
        }

        public class ConstInt : Node
        {
            public int number;

            public override string ToString()
            {
                return number.ToString();
            }
        }

        public class BinOp : Node
        {
            public enum BinOpType { Add, Subract, Multiply, Divide }
            public BinOpType type;

            public Node left;
            public Node right;

            public void SetTypeFromToken(Token next)
            {
                switch (next.type)
                {
                    case Token.TokenType.Add:
                        type = BinOpType.Add;
                        break;
                    case Token.TokenType.Subtract:
                        type = BinOpType.Subract;
                        break;
                    case Token.TokenType.Multiply:
                        type = BinOpType.Multiply;
                        break;
                    case Token.TokenType.Divide:
                        type = BinOpType.Divide;
                        break;
                    default:
                        throw new CompilerError("Invalid token type for binary operation", next);
                }
            }

            public override IEnumerable<Node> GetChilds()
            {
                yield return left;
                yield return right;
            }

            public override string ToString()
            {
                switch (type)
                {
                    case BinOpType.Add:
                        return "+";
                    case BinOpType.Subract:
                        return "-";
                    case BinOpType.Multiply:
                        return "*";
                    case BinOpType.Divide:
                        return "/";
                    default:
                        throw new InvalidCodePath();
                }
            }
        }

        public class UnaryOp : Node
        {
            public enum UnaryOpType { Add, Subract }
            public UnaryOpType type;

            public Node expression;

            public void SetTypeFromToken(Token next)
            {
                switch (next.type)
                {
                    case Token.TokenType.Add:
                        type = UnaryOpType.Add;
                        break;
                    case Token.TokenType.Subtract:
                        type = UnaryOpType.Subract;
                        break;
                    default:
                        throw new CompilerError("Invalid token type for unary operator", next);
                }
            }

            public override IEnumerable<Node> GetChilds()
            {
                yield return expression;
            }

            public override string ToString()
            {
                switch (type)
                {
                    case UnaryOpType.Add:
                        return "unary +";
                    case UnaryOpType.Subract:
                        return "unary -";
                
                    default:
                        throw new InvalidCodePath();
                }
            }
        }

        public static int skipWhitespace(IList<Token> tokens, int pos, bool requireOneWS = false)
        {
            bool foundWS = false;
            while (tokens[pos].type == Token.TokenType.WhiteSpace)
            {
                foundWS = true;
                pos++;
                if (pos >= tokens.Count)
                {
                    break;
                }
            }

            if (requireOneWS && !foundWS)
            {
                throw new CompilerError("Expected Whitespace", tokens[pos]);
            }
            return pos;
        }


        static Node parseStatement(IList<Token> tokens, ref int pos)
        {
            pos = skipWhitespace(tokens, pos);
            var result = default(Node);

            var current = tokens[pos];
            if (current.type == Token.TokenType.Var)
            {
                result = parseVariableDeclaration(tokens, ref pos);
            }
            else if (current.type == Token.TokenType.Identifier)
            {
                var next = peekToken(tokens, pos, tokenMustExist: true, skipWS: true);

                // could be either a function call or an assignment
                expectTokenType(next, Token.TokenType.OpenBracket, Token.TokenType.Assignment);

                if (next.type == Token.TokenType.OpenBracket)
                {
                    result = parseFunctionCall(tokens, ref pos);
                }
                else if (next.type == Token.TokenType.Assignment)
                {
                    result = parseAssignment(tokens, ref pos);
                }
                else
                    throw new InvalidCodePath();
            }

            var endOfStatement = nextToken(tokens, ref pos, true);
            expectTokenType(endOfStatement, Token.TokenType.Semicolon);

            return result;
        }

        static Node parseAssignment(IList<Token> tokens, ref int pos)
        {
            var current = tokens[pos];
            expectTokenType(current, Token.TokenType.Identifier);

            var assign = nextToken(tokens, ref pos, skipWS: true);
            expectTokenType(assign, Token.TokenType.Assignment);

            var firstExpressionToken = nextToken(tokens, ref pos, skipWS: true);

            var result = new Assignment();
            result.variableName = current.text;
            result.expression = parseExpression(tokens, ref pos);
            return result;
        }

        static Node parseVariableDeclaration(IList<Token> tokens, ref int pos)
        {
            var current = tokens[pos];
            expectTokenType(current, Token.TokenType.Var);

            var ident = nextToken(tokens, ref pos, skipWS: true);
            expectTokenType(ident, Token.TokenType.Identifier);

            var assign = nextToken(tokens, ref pos, skipWS: true);
            expectTokenType(assign, Token.TokenType.Assignment);

            var firstExpressionToken = nextToken(tokens, ref pos, skipWS: true);

            var result = new VariableDeclaration();
            result.variableName = ident.text;
            result.expression = parseExpression(tokens, ref pos);
            return result;
        }

        private static Node parseExpression(IList<Token> tokens, ref int pos)
        {
            var result = parseTerm(tokens, ref pos);
            var otherTerm = default(Node);
            var next = peekToken(tokens, pos, tokenMustExist: false, skipWS: true);


            // is operator precedence 4
            while (next.type == Token.TokenType.Add || next.type == Token.TokenType.Subtract)
            {
                // continue to the next token after the add or subtract
                nextToken(tokens, ref pos, true);
                nextToken(tokens, ref pos, true);

                otherTerm = parseTerm(tokens, ref pos);
                var bo = new BinOp();
                bo.left = result;
                bo.right = otherTerm;
                bo.SetTypeFromToken(next);
                result = bo;
                next = peekToken(tokens, pos, tokenMustExist: false, skipWS: true);
            }

            return result;
        }

        private static Node parseTerm(IList<Token> tokens, ref int pos)
        {
            var result = parseUnary(tokens, ref pos);
            var otherFactor = default(Node);
            var next = peekToken(tokens, pos, tokenMustExist: false, skipWS: true);


            // is operator precedence 3
            while (next.type == Token.TokenType.Multiply || next.type == Token.TokenType.Divide)
            {
                // continue to the next token after the add or subtract
                nextToken(tokens, ref pos, true);
                nextToken(tokens, ref pos, true);

                otherFactor = parseUnary(tokens, ref pos);
                var bo = new BinOp();
                bo.left = result;
                bo.right = otherFactor;
                bo.SetTypeFromToken(next);
                result = bo;
                next = peekToken(tokens, pos, tokenMustExist: false, skipWS: true);
            }

            return result;
        }

        private static Node parseUnary(IList<Token> tokens, ref int pos)
        {
            var current = tokens[pos];

            // is operator precedence 2
            if (current.type == Token.TokenType.Add || current.type == Token.TokenType.Subtract)
            {
                var result = new UnaryOp();
                result.SetTypeFromToken(current);
                nextToken(tokens, ref pos);
                result.expression = parseFactor(tokens, ref pos);
                return result;
            }

            return parseFactor(tokens, ref pos);
        }

        private static Node parseFactor(IList<Token> tokens, ref int pos)
        {
            var current = tokens[pos];
            expectTokenType(current, Token.TokenType.Number, Token.TokenType.Identifier, Token.TokenType.OpenBracket);


            if (current.type == Token.TokenType.Number)
            {
                var result = new ConstInt();
                result.number = int.Parse(current.text);
                return result;
            }

            // '(' expr ')'
            if (current.type == Token.TokenType.OpenBracket)
            {
                var exprStart = nextToken(tokens, ref pos);
                var result = parseExpression(tokens, ref pos);
                var cBracket = nextToken(tokens, ref pos, skipWS: true);
                expectTokenType(cBracket, Token.TokenType.CloseBracket);
                return result;
            }

            // either function call or variable
            if (current.type == Token.TokenType.Identifier)
            {
                var peek = peekToken(tokens, pos, skipWS: true);
                if (peek.type == Token.TokenType.OpenBracket)
                {
                    var result = parseFunctionCall(tokens, ref pos);
                    return result;
                }
                var varLookup = new VariableLookup();
                varLookup.variableName = current.text;
                return varLookup;
            }

            throw new InvalidCodePath();
        }

        static void expectTokenType(Token t, Token.TokenType type)
        {
            if (t.type != type)
                throw new CompilerErrorExpected(type.ToString(), t.type.ToString(), t);
        }

        static void expectTokenType(Token token, params Token.TokenType[] types)
        {
            var found = false;
            foreach (var tt in types)
            {
                if (token.type == tt)
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                string exp = "either ( " + string.Join(" | ", types.Select(tt => tt.ToString())) + " )";
                throw new CompilerErrorExpected(exp, token.ToString(), token);
            }
        }

        static Node parseFunctionCall(IList<Token> tokens, ref int pos)
        {
            var result = new FunctionCall();

            var current = tokens[pos];
            expectTokenType(current, Token.TokenType.Identifier);

            result.functionName = current.text;

            var ob = nextToken(tokens, ref pos, skipWS: true);
            expectTokenType(ob, Token.TokenType.OpenBracket);

            
            var next = peekToken(tokens, pos, skipWS: true);
            if (next.type != Token.TokenType.CloseBracket)
            {
                while (true)
                {
                    nextToken(tokens, ref pos);
                    var exp = parseExpression(tokens, ref pos);
                    result.argumentList.Add(exp);
                    next = peekToken(tokens, pos);
                    if (next.type != Token.TokenType.Comma)
                    {
                        break;
                    }
                    else
                    {
                        // skip comma
                        nextToken(tokens, ref pos);
                    }
                }
            }

            var cb = nextToken(tokens, ref pos, skipWS: true);
            expectTokenType(cb, Token.TokenType.CloseBracket);

            return result;
        }

        static Token peekToken(IList<Token> tokens, int pos, bool tokenMustExist = false, bool skipWS = true)
        {
            pos++;
            if (skipWS)
            {
                pos = skipWhitespace(tokens, pos);
            }

            if (pos >= tokens.Count)
            {
                if (tokenMustExist)
                {
                    throw new CompilerError("Missing next token", tokens[pos - 1]);
                }
                else
                {
                    return null;
                }
            }
            return tokens[pos];
        }

        static Token nextToken(IList<Token> tokens, ref int pos, bool skipWS = true)
        {
            pos++;
            if (skipWS)
            {
                pos = skipWhitespace(tokens, pos);
            }

            if (pos >= tokens.Count)
            {
                throw new CompilerError("Missing next token", tokens[pos]);
            }
            else
            {
                return tokens[pos];
            }
        }

        public static Node Parse(IList<Token> tokens)
        {
            var result = new Block();
            int pos = 0;
            while (pos < tokens.Count)
            {
                var s = parseStatement(tokens, ref pos);
                result.statements.Add(s);
                pos++;
            }
            return result;
        }
    }

}
