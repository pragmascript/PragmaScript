using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PragmaScript
{



    class AST
    {
        public abstract class Node
        {
            public Token token;

            public Node(Token t)
            {
                token = t;
            }
            public virtual IEnumerable<Node> GetChilds()
            {
                yield break;
            }

            public abstract VariableType CheckType(Scope scope);
        }

        public class VariableType
        {
            public static VariableType void_ = new VariableType { name = "void" };
            public static VariableType int32 = new VariableType { name = "int32" };
            public static VariableType float32 = new VariableType { name = "float32" };
            public string name;

            public override int GetHashCode()
            {
                return name.GetHashCode();
            }
        }

        public class VariableDefinition
        {
            public string name;
            public VariableType type;
        }

        public class Scope
        {
            public Scope parent;

            public Dictionary<string, VariableDefinition> variables = new Dictionary<string, VariableDefinition>();
            public Dictionary<string, VariableType> types = new Dictionary<string, VariableType>();

            public VariableDefinition GetVar(string name)
            {
                VariableDefinition result;

                if (variables.TryGetValue(name, out result))
                {
                    return result;
                }

                if (parent != null)
                {
                    return parent.GetVar(name);
                }
                else
                {
                    return null;
                }
            }

            public VariableDefinition AddVar(string name, Token t)
            {
                VariableDefinition v = new VariableDefinition();
                v.name = name;
                if (variables.ContainsKey(name))
                {
                    throw new RedefinedVariable(name, t);
                }
                variables.Add(name, v);
                return v;
            }

            public VariableType GetType(string typeName)
            {
                VariableType result;

                if (types.TryGetValue(typeName, out result))
                {
                    return result;
                }

                if (parent != null)
                {
                    return parent.GetType(typeName);
                }
                else
                {
                    return null;
                }
            }

            public void AddType(string name, Token t)
            {
                VariableType type = new VariableType();
                type.name = name;
                if (types.ContainsKey(name))
                {
                    throw new RedefinedType(name, t);
                }
                types.Add(name, type);
            }

            public void AddType(VariableType t, Token token)
            {
                if (types.ContainsKey(t.name))
                {
                    throw new RedefinedType(t.name, token);
                }
                types.Add(t.name, t);
            }
        }



        public class Block : Node
        {
            public Scope scope;
            public List<Node> statements = new List<Node>();
            public Block(Token t)
                : base(t)
            {
            }
            public override IEnumerable<Node> GetChilds()
            {
                foreach (var s in statements)
                    yield return s;
            }
            public override VariableType CheckType(Scope scope)
            {
                foreach (var s in statements)
                {
                    s.CheckType(this.scope);
                }
                return null;
            }
            public override string ToString()
            {
                return "Block";
            }
        }


        public class VariableDeclaration : Node
        {
            public VariableDefinition variable;
            public Node expression;

            public VariableDeclaration(Token t)
                : base(t)
            {
            }

            public override IEnumerable<Node> GetChilds()
            {
                yield return expression;
            }
            public override VariableType CheckType(Scope scope)
            {
                var type = expression.CheckType(scope);
                variable.type = type;
                return type; 
            }
            public override string ToString()
            {
                return "var " + variable.name + " = ";
            }

        }

        public class FunctionCall : Node
        {
            public string functionName;
            public List<Node> argumentList = new List<Node>();
            public VariableType returnType;

            public FunctionCall(Token t)
                : base(t)
            {
            }
            public override IEnumerable<Node> GetChilds()
            {
                foreach (var exp in argumentList)
                {
                    yield return exp;
                }
            }
            public override VariableType CheckType(Scope scope)
            {
                return returnType;
            }
            public override string ToString()
            {
                return functionName + "()";
            }
        }

        public class VariableLookup : Node
        {
            public string variableName;

            public VariableLookup(Token t)
                : base(t)
            {
            }

            public override VariableType CheckType(Scope scope)
            {
                return scope.GetVar(variableName).type;
            }
            public override string ToString()
            {
                return variableName;
            }
        }

        public class Assignment : Node
        {
            public VariableDefinition variable;
            public Node expression;

            public Assignment(Token t)
                : base(t)
            {
            }

            public override IEnumerable<Node> GetChilds()
            {
                yield return expression;
            }
            public override VariableType CheckType(Scope scope)
            {
                var et = expression.CheckType(scope);
                if (et != variable.type)
                {
                    throw new ParserVariableTypeMismatch(variable.type, et, token);
                }
                return variable.type;
            }
            public override string ToString()
            {
                return variable.name + " = ";
            }
        }

        public class ConstInt32 : Node
        {
            public int number;

            public ConstInt32(Token t)
                : base(t)
            {
            }

            public override VariableType CheckType(Scope scope)
            {
                return VariableType.int32;
            }
            public override string ToString()
            {
                return number.ToString();
            }
        }

        public class ConstFloat32 : Node
        {
            public double number;

            public ConstFloat32(Token t)
                : base(t)
            {
            }


            public override VariableType CheckType(Scope scope)
            {
                return VariableType.float32;
            }
            public override string ToString()
            {
                return number.ToString();
            }
        }

        public class Return : Node
        {
            public Node expression;

            public Return(Token t)
                : base(t)
            {
            }


            public override IEnumerable<Node> GetChilds()
            {
                yield return expression;
            }
            public override VariableType CheckType(Scope scope)
            {
                var result = expression.CheckType(scope);
                return result;
            }
            public override string ToString()
            {
                return "return";
            }

        }

        public class BinOp : Node
        {
            public enum BinOpType { Add, Subract, Multiply, Divide }
            public BinOpType type;

            public Node left;
            public Node right;

            public BinOp(Token t)
                : base(t)
            {
            }

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
                        throw new ParserError("Invalid token type for binary operation", next);
                }
            }
            public override VariableType CheckType(Scope scope)
            {
                var lType = left.CheckType(scope);
                var rType = right.CheckType(scope);
                if (lType != rType)
                {
                    throw new ParserTypeMismatch(lType, rType, token);
                }
                return lType;
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

            public UnaryOp(Token t)
                : base(t)
            {
            }


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
                        throw new ParserError("Invalid token type for unary operator", next);
                }
            }

            public override VariableType CheckType(Scope scope)
            {
                return expression.CheckType(scope);
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

        public class TypeCastOp : Node
        {
            public Node expression;
            public VariableType type;

            public TypeCastOp(Token t)
                : base(t)
            {
            }

            public override IEnumerable<Node> GetChilds()
            {
                yield return expression;
            }
            public override VariableType CheckType(Scope scope)
            {
                return type;
            }
            public override string ToString()
            {
                return "(" + type.name + ")";
            }
        }

        public static int skipWhitespace(IList<Token> tokens, int pos, bool requireOneWS = false)
        {
            bool foundWS = false;

            while (pos < tokens.Count && tokens[pos].type == Token.TokenType.WhiteSpace)
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
                throw new ParserError("Expected Whitespace", tokens[pos]);
            }
            return pos;
        }


        static Node parseStatement(IList<Token> tokens, ref int pos, Scope scope)
        {
            pos = skipWhitespace(tokens, pos);
            var result = default(Node);

            var current = tokens[pos];
            if (current.type == Token.TokenType.Var)
            {
                result = parseVariableDeclaration(tokens, ref pos, scope);
            }
            if (current.type == Token.TokenType.Return)
            {
                result = parseReturn(tokens, ref pos, scope);
            }
            else if (current.type == Token.TokenType.Identifier)
            {
                var next = peekToken(tokens, pos, tokenMustExist: true, skipWS: true);

                // could be either a function call or an assignment
                expectTokenType(next, Token.TokenType.OpenBracket, Token.TokenType.Assignment);

                if (next.type == Token.TokenType.OpenBracket)
                {
                    result = parseFunctionCall(tokens, ref pos, scope);
                }
                else if (next.type == Token.TokenType.Assignment)
                {
                    result = parseAssignment(tokens, ref pos, scope);
                }
                else
                    throw new InvalidCodePath();
            }

            var endOfStatement = nextToken(tokens, ref pos, true);
            expectTokenType(endOfStatement, Token.TokenType.Semicolon);

            return result;
        }

        static Node parseReturn(IList<Token> tokens, ref int pos, Scope scope)
        {
            var current = tokens[pos];
            expectTokenType(current, Token.TokenType.Return);

            var next = peekToken(tokens, pos, true, true);
            if (next.type == Token.TokenType.Semicolon)
            {
                return new Return(current);
            }
            else
            {
                var result = new Return(current);
                nextToken(tokens, ref pos);
                result.expression = parseExpression(tokens, ref pos, scope);
                return result;
            }
        }

        static Node parseAssignment(IList<Token> tokens, ref int pos, Scope scope)
        {
            var current = tokens[pos];
            expectTokenType(current, Token.TokenType.Identifier);

            var assign = nextToken(tokens, ref pos, skipWS: true);
            expectTokenType(assign, Token.TokenType.Assignment);

            var firstExpressionToken = nextToken(tokens, ref pos, skipWS: true);

            var result = new Assignment(current);
            var variable = scope.GetVar(current.text);
            if (variable == null)
            {
                throw new UndefinedVariable(current.text, current);
            }
            result.variable = variable;
            result.expression = parseExpression(tokens, ref pos, scope);

            return result;
        }

        static Node parseVariableDeclaration(IList<Token> tokens, ref int pos, Scope scope)
        {
            var current = tokens[pos];
            expectTokenType(current, Token.TokenType.Var);

            var ident = nextToken(tokens, ref pos, skipWS: true);
            expectTokenType(ident, Token.TokenType.Identifier);

            var assign = nextToken(tokens, ref pos, skipWS: true);
            expectTokenType(assign, Token.TokenType.Assignment);

            var firstExpressionToken = nextToken(tokens, ref pos, skipWS: true);

            var result = new VariableDeclaration(current);
            
            // add variable to scope
            result.variable = scope.AddVar(ident.text, current); 
            result.expression = parseExpression(tokens, ref pos, scope);

            return result;
        }

        private static Node parseExpression(IList<Token> tokens, ref int pos, Scope scope)
        {
            var result = parseTerm(tokens, ref pos, scope);
            var otherTerm = default(Node);
            var next = peekToken(tokens, pos, tokenMustExist: false, skipWS: true);


            // is operator precedence 4
            while (next.type == Token.TokenType.Add || next.type == Token.TokenType.Subtract)
            {
                // continue to the next token after the add or subtract
                nextToken(tokens, ref pos, true);
                nextToken(tokens, ref pos, true);

                otherTerm = parseTerm(tokens, ref pos, scope);
                var bo = new BinOp(next);
                bo.left = result;
                bo.right = otherTerm;
                bo.SetTypeFromToken(next);
                result = bo;
                next = peekToken(tokens, pos, tokenMustExist: false, skipWS: true);
            }
            return result;
        }

        private static Node parseTerm(IList<Token> tokens, ref int pos, Scope scope)
        {
            var result = parseUnary(tokens, ref pos, scope);
            var otherFactor = default(Node);
            var next = peekToken(tokens, pos, tokenMustExist: false, skipWS: true);


            // is operator precedence 3
            while (next.type == Token.TokenType.Multiply || next.type == Token.TokenType.Divide)
            {
                // continue to the next token after the add or subtract
                nextToken(tokens, ref pos, true);
                nextToken(tokens, ref pos, true);

                otherFactor = parseUnary(tokens, ref pos, scope);
                var bo = new BinOp(next);
                bo.left = result;
                bo.right = otherFactor;
                bo.SetTypeFromToken(next);
                result = bo;
                next = peekToken(tokens, pos, tokenMustExist: false, skipWS: true);
            }

            return result;
        }

        private static Node parseUnary(IList<Token> tokens, ref int pos, Scope scope)
        {
            var current = tokens[pos];

            // is operator precedence 2

            // handle unary plus and minus
            if (current.type == Token.TokenType.Add || current.type == Token.TokenType.Subtract)
            {
                var result = new UnaryOp(current);
                result.SetTypeFromToken(current);
                nextToken(tokens, ref pos);
                result.expression = parseFactor(tokens, ref pos, scope);
                return result;
            }

            // check if next token is an identifier
            var nextIdx = pos;
            var next = peekToken(tokens, ref nextIdx);
            // check if next next token is close bracket
            // TODO: DO I REALLY NEED 2 LOOKAHEAD HERE?
            var nextNext = peekToken(tokens, nextIdx);
            
            // handle type cast operator (T)x
            if (current.type == Token.TokenType.OpenBracket 
                && next.type == Token.TokenType.Identifier 
                && nextNext.type == Token.TokenType.CloseBracket)
            {
                var typeNameToken = nextToken(tokens, ref pos);
                expectTokenType(typeNameToken, Token.TokenType.Identifier);
                var closeBrackeToken = nextToken(tokens, ref pos);
                expectTokenType(closeBrackeToken, Token.TokenType.CloseBracket);

                nextToken(tokens, ref pos);
                var exp = parseFactor(tokens, ref pos, scope);

                var result = new TypeCastOp(current);

                // TODO: check if valid type (in type check phase?)
                result.type = scope.GetType(typeNameToken.text);
                result.expression = exp;
                return result;
            }

            return parseFactor(tokens, ref pos, scope);
        }

        private static Node parseFactor(IList<Token> tokens, ref int pos, Scope scope)
        {
            var current = tokens[pos];
            expectTokenType(current, Token.TokenType.IntNumber, Token.TokenType.FloatNumber, Token.TokenType.Identifier, Token.TokenType.OpenBracket);

            if (current.type == Token.TokenType.IntNumber)
            {
                var result = new ConstInt32(current);
                result.number = int.Parse(current.text);
                return result;
            }
            if (current.type == Token.TokenType.FloatNumber)
            {
                var result = new ConstFloat32(current);
                result.number = double.Parse(current.text, CultureInfo.InvariantCulture);
                return result;
            }

            // '(' expr ')'
            if (current.type == Token.TokenType.OpenBracket)
            {
                var exprStart = nextToken(tokens, ref pos);
                var result = parseExpression(tokens, ref pos, scope);
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
                    var result = parseFunctionCall(tokens, ref pos, scope);
                    return result;
                }

                if (scope.GetVar(current.text) == null)
                {
                    throw new UndefinedVariable(current.text, current);
                }
                var varLookup = new VariableLookup(current);
                varLookup.variableName = current.text;
                return varLookup;
            }

            throw new InvalidCodePath();
        }

        static Node parseFunctionCall(IList<Token> tokens, ref int pos, Scope scope)
        {
            

            var current = tokens[pos];
            expectTokenType(current, Token.TokenType.Identifier);

            var result = new FunctionCall(current);
            result.functionName = current.text;

            var ob = nextToken(tokens, ref pos, skipWS: true);
            expectTokenType(ob, Token.TokenType.OpenBracket);


            var next = peekToken(tokens, pos, skipWS: true);
            if (next.type != Token.TokenType.CloseBracket)
            {
                while (true)
                {
                    nextToken(tokens, ref pos);
                    var exp = parseExpression(tokens, ref pos, scope);
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


        public static Node parseBlock(IList<Token> tokens, ref int pos, Scope parentScope)
        {
            var current = tokens[pos];
            var result = new Block(current);
            result.scope = new Scope();
            result.scope.parent = parentScope;
            while (pos < tokens.Count)
            {
                var s = parseStatement(tokens, ref pos, result.scope);
                result.statements.Add(s);
                pos++;
            }
            return result;
        }

        static void expectTokenType(Token t, Token.TokenType type)
        {
            if (t.type != type)
                throw new ParserErrorExpected(type.ToString(), t.type.ToString(), t);
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
                throw new ParserErrorExpected(exp, token.ToString(), token);
            }
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
                    throw new ParserError("Missing next token", tokens[pos - 1]);
                }
                else
                {
                    return null;
                }
            }
            return tokens[pos];
        }

        static Token peekToken(IList<Token> tokens, ref int pos, bool tokenMustExist = false, bool skipWS = true)
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
                    throw new ParserError("Missing next token", tokens[pos - 1]);
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
                throw new ParserError("Missing next token", tokens[pos]);
            }
            else
            {
                return tokens[pos];
            }
        }


        public static Node Parse(IList<Token> tokens)
        {
            int pos = 0;
            var current = tokens[pos];

            Node block = null;
            try
            {
                var rootScope = new Scope();
                rootScope.AddType(VariableType.float32, current);
                rootScope.AddType(VariableType.int32, current);

                // perform AST generation pass
                block = parseBlock(tokens, ref pos, rootScope);

                // perform type checking pass
                block.CheckType(rootScope);
            }
            catch (ParserError error)
            {
                Console.Error.WriteLine(error.Message);
                return null;
            }

            return block;
        }

        public static void TypeCheck(Node root)
        {

        }
    }

}
