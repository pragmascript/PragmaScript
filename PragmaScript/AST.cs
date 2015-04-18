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
        Scope rootScope;
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

            public abstract FrontendType CheckType(Scope scope);
        }

        public class FrontendType
        {
            public static readonly FrontendType void_ = new FrontendType { name = "void" };
            public static readonly FrontendType int32 = new FrontendType { name = "int32" };
            public static readonly FrontendType float32 = new FrontendType { name = "float32" };
            public static readonly FrontendType bool_ = new FrontendType { name = "bool" };
            public static readonly FrontendType string_ = new FrontendType { name = "string" };

            public string name;

            public override int GetHashCode()
            {
                return name.GetHashCode();
            }

            public override string ToString()
            {
                return name;
            }
        }

        public class VariableDefinition
        {
            public bool isFunctionParameter;
            public int parameterIdx = -1;
            public string name;
            public FrontendType type;
        }

        public struct NamedParameter
        {
            public string name;
            public FrontendType type;
        }

        public class FunctionDefinition
        {
            public string name;
            public FrontendType returnType;
            public List<NamedParameter> parameters = new List<NamedParameter>();
            public void AddParameter(string name, FrontendType type)
            {
                parameters.Add(new NamedParameter{name = name, type = type});
            }
        }

        public class Scope
        {
            public FunctionDefinition function;
            public Scope parent;

            public Dictionary<string, VariableDefinition> variables = new Dictionary<string, VariableDefinition>();
            public Dictionary<string, FunctionDefinition> functions = new Dictionary<string, FunctionDefinition>();
            public Dictionary<string, FrontendType> types = new Dictionary<string, FrontendType>();

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

            // TODO: handle function parameters different?
            public void AddFunctionParameter(string name, FrontendType type, int idx)
            {
                VariableDefinition v = new VariableDefinition();
                v.name = name;
                v.type = type;
                v.isFunctionParameter = true;
                v.parameterIdx = idx;
                variables.Add(name, v);
            }

            public FunctionDefinition GetFunction(string name)
            {
                FunctionDefinition result;

                if (functions.TryGetValue(name, out result))
                {
                    return result;
                }

                if (parent != null)
                {
                    return parent.GetFunction(name);
                }
                else
                {
                    return null;
                }
            }

            public void AddFunction(FunctionDefinition fun)
            {
                if (variables.ContainsKey(fun.name))
                {
                    throw new RedefinedFunction(fun.name, Token.Undefined);
                }
                functions.Add(fun.name, fun);
            }

            public FrontendType GetType(string typeName)
            {
                FrontendType result;

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
                FrontendType type = new FrontendType();
                type.name = name;
                if (types.ContainsKey(name))
                {
                    throw new RedefinedType(name, t);
                }
                types.Add(name, type);
            }

            public void AddType(FrontendType t, Token token)
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
            public override FrontendType CheckType(Scope scope)
            {
                foreach (var s in statements)
                {
                    var rt = s.CheckType(this.scope);
                }
                return null;
            }
            public override string ToString()
            {
                return "Block";
            }
        }

        public class IfCondition : Node
        {
            public Node condition;
            public Node thenBlock;
            public Node elseBlock;
            public IfCondition(Token t)
                : base(t)
            {
            }

            public override IEnumerable<Node> GetChilds()
            {
                yield return condition;
                yield return thenBlock;
                if (elseBlock != null)
                {
                    yield return elseBlock;
                }
            }
            public override FrontendType CheckType(Scope scope)
            {
                var ct = condition.CheckType(scope);
                if (ct != FrontendType.bool_)
                    throw new ParserExpectedType(FrontendType.bool_, ct, condition.token);

                thenBlock.CheckType(scope);
                if (elseBlock != null)
                    elseBlock.CheckType(scope);

                return null;
            }

            public override string ToString()
            {
                return "if";
            }
        }

        public class ForLoop : Node
        {
            public Node initializer;
            public Node condition;
            public Node iterator;

            public Node loopBody;

            public ForLoop(Token t)
                : base(t)
            {
            }
            public override IEnumerable<Node> GetChilds()
            {
                yield return initializer;
                yield return condition;
                yield return iterator;
                yield return loopBody;
            }
            public override FrontendType CheckType(Scope scope)
            {
                var loopBodyScope = (loopBody as Block).scope;
                initializer.CheckType(loopBodyScope);
                var ct = condition.CheckType(loopBodyScope);
                if (ct != FrontendType.bool_)
                    throw new ParserExpectedType(FrontendType.bool_, ct, condition.token);
                iterator.CheckType(loopBodyScope);
                loopBody.CheckType(scope);
                return null;
            }
            public override string ToString()
            {
                return "for";
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
            public override FrontendType CheckType(Scope scope)
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

        public class FunctionDeclaration : Node
        {
            public FunctionDefinition fun;
            public Node body;

            public FunctionDeclaration(Token t)
                : base(t)
            {
            }

            public override IEnumerable<Node> GetChilds()
            {
                yield return body;
            }
            public override FrontendType CheckType(Scope scope)
            {
                body.CheckType(scope);
                return null;
            }
            public override string ToString()
            {
                string result = fun.name + "(";
                for (int i = 0; i < fun.parameters.Count; ++i)
                {
                    var p = fun.parameters[i];
                    result += p.name + ": " + p.type;
                    if (i != fun.parameters.Count - 1)
                        result += ", ";
                }
                return result + ")";
            }
        }

        public class FunctionCall : Node
        {
            public string functionName;
            public List<Node> argumentList = new List<Node>();
            public FrontendType returnType;

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
            public override FrontendType CheckType(Scope scope)
            {
                var fun = scope.GetFunction(functionName);
                int idx = 0;
                foreach (var arg in argumentList)
                {
                    var targ = arg.CheckType(scope);
                    if (targ != fun.parameters[idx].type)
                    {
                        throw new ParserExpectedArgumentType(fun.parameters[idx].type, targ, idx + 1, token);
                    }
                }
                returnType = fun.returnType;
                return returnType;
            }
            public override string ToString()
            {
                return functionName + "()";
            }
        }

        public class VariableLookup : Node
        {
            public enum Incrementor { None, preIncrement, preDecrement, postIncrement, postDecrement }
            public Incrementor inc;
            public string variableName;
            public VariableDefinition varDefinition;
            public VariableLookup(Token t)
                : base(t)
            {
            }
            public override FrontendType CheckType(Scope scope)
            {
                var v = scope.GetVar(variableName);
                varDefinition = v;
                return v.type;
            }
            public override string ToString()
            {
                switch (inc)
                {
                    case Incrementor.None:
                        return variableName;
                    case Incrementor.preIncrement:
                        return "++" + variableName;
                    case Incrementor.preDecrement:
                        return "--" + variableName;
                    case Incrementor.postIncrement:
                        return variableName + "++";
                    case Incrementor.postDecrement:
                        return variableName + "--";
                    default:
                        throw new InvalidCodePath();
                }
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
            public override FrontendType CheckType(Scope scope)
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
            public override FrontendType CheckType(Scope scope)
            {
                return FrontendType.int32;
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
            public override FrontendType CheckType(Scope scope)
            {
                return FrontendType.float32;
            }
            public override string ToString()
            {
                return number.ToString("F2", CultureInfo.InvariantCulture);
            }
        }

        public class ConstBool : Node
        {
            public bool value;
            public ConstBool(Token t)
                : base(t)
            {
            }
            public override FrontendType CheckType(Scope scope)
            {
                return FrontendType.bool_;
            }
            public override string ToString()
            {
                return value.ToString();
            }
        }

        public class ConstString : Node
        {
            public string s;

            public ConstString(Token t)
                : base(t)
            {
            }
            public override FrontendType CheckType(Scope scope)
            {
                return FrontendType.string_;
            }
            public override string ToString()
            {
                return s;
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
                if (expression != null)
                {
                    yield return expression;
                }
            }
            public override FrontendType CheckType(Scope scope)
            {
                var result = default(FrontendType);
                if (expression != null)
                {
                    result = expression.CheckType(scope);
                }
                else
                {
                    result = FrontendType.void_;
                }
                if (scope.function.returnType != null)
                {
                    if (result != scope.function.returnType)
                    {
                        throw new ParserError("return statement returns different types in one block", token);
                    }
                }
                else
                {
                    scope.function.returnType = result;
                }
                return result;
            }
            public override string ToString()
            {
                return "return";
            }

        }

        public class BinOp : Node
        {
            public enum BinOpType { Add, Subract, Multiply, Divide, ConditionalOR, ConditionaAND, LogicalOR, LogicalXOR, LogicalAND, Equal, NotEqual, Greater, Less, GreaterEqual, LessEqual, LeftShift, RightShift, Remainder }
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
                    case Token.TokenType.Remainder:
                        type = BinOpType.Remainder;
                        break;
                    case Token.TokenType.LeftShift:
                        type = BinOpType.LeftShift;
                        break;
                    case Token.TokenType.RightShift:
                        type = BinOpType.RightShift;
                        break;
                    case Token.TokenType.ConditionalOR:
                        type = BinOpType.ConditionalOR;
                        break;
                    case Token.TokenType.ConditionalAND:
                        type = BinOpType.ConditionaAND;
                        break;
                    case Token.TokenType.LogicalOR:
                        type = BinOpType.LogicalOR;
                        break;
                    case Token.TokenType.LogicalXOR:
                        type = BinOpType.LogicalXOR;
                        break;
                    case Token.TokenType.LogicalAND:
                        type = BinOpType.LogicalAND;
                        break;
                    case Token.TokenType.Equal:
                        type = BinOpType.Equal;
                        break;
                    case Token.TokenType.NotEqual:
                        type = BinOpType.NotEqual;
                        break;
                    case Token.TokenType.Greater:
                        type = BinOpType.Greater;
                        break;
                    case Token.TokenType.Less:
                        type = BinOpType.Less;
                        break;
                    case Token.TokenType.GreaterEqual:
                        type = BinOpType.GreaterEqual;
                        break;
                    case Token.TokenType.LessEqual:
                        type = BinOpType.LessEqual;
                        break;
                    default:
                        throw new ParserError("Invalid token type for binary operation", next);
                }
            }

            bool isEither(params BinOpType[] types)
            {
                for (int i = 0; i < types.Length; ++i)
                {
                    if (type == types[i])
                        return true;
                }

                return false;
            }

            public override FrontendType CheckType(Scope scope)
            {
                var lType = left.CheckType(scope);
                var rType = right.CheckType(scope);

                if (isEither(BinOpType.LeftShift, BinOpType.RightShift))
                {
                    // TODO: suppport all integer types here.
                    if (lType != FrontendType.int32 || rType != FrontendType.int32)
                    {
                        throw new ParserErrorExpected("two integer types", string.Format("{0} and {1}", lType, rType), token);
                    }
                }

                if (lType != rType)
                {
                    throw new ParserTypeMismatch(lType, rType, token);
                }

                if (isEither(BinOpType.Less, BinOpType.LessEqual, BinOpType.Greater, BinOpType.GreaterEqual,
                    BinOpType.Equal, BinOpType.NotEqual))
                {
                    return FrontendType.bool_;
                }
                else
                {
                    return lType;
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
                    case BinOpType.ConditionalOR:
                        return "||";
                    case BinOpType.ConditionaAND:
                        return "&&";
                    case BinOpType.LogicalOR:
                        return "|";
                    case BinOpType.LogicalXOR:
                        return "^";
                    case BinOpType.LogicalAND:
                        return "&";
                    case BinOpType.Equal:
                        return "==";
                    case BinOpType.NotEqual:
                        return "!=";
                    case BinOpType.Greater:
                        return ">";
                    case BinOpType.Less:
                        return "<";
                    case BinOpType.GreaterEqual:
                        return ">=";
                    case BinOpType.LessEqual:
                        return "<=";
                    case BinOpType.LeftShift:
                        return "<<";
                    case BinOpType.RightShift:
                        return ">>";
                    case BinOpType.Remainder:
                        return "%";
                    default:
                        throw new InvalidCodePath();
                }


            }
        }

        public class UnaryOp : Node
        {
            public enum UnaryOpType { Add, Subract, LogicalNOT, Complement }
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
                    case Token.TokenType.LogicalNOT:
                        type = UnaryOpType.LogicalNOT;
                        break;
                    case Token.TokenType.Complement:
                        type = UnaryOpType.Complement;
                        break;
                    default:
                        throw new ParserError("Invalid token type for unary operator", next);
                }
            }

            public override FrontendType CheckType(Scope scope)
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
                    case UnaryOpType.LogicalNOT:
                        return "!";
                    case UnaryOpType.Complement:
                        return "~";
                    default:
                        throw new InvalidCodePath();
                }
            }
        }

        public class TypeCastOp : Node
        {
            public Node expression;
            public FrontendType type;

            public TypeCastOp(Token t)
                : base(t)
            {
            }

            public override IEnumerable<Node> GetChilds()
            {
                yield return expression;
            }
            public override FrontendType CheckType(Scope scope)
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

            while (pos < tokens.Count && (tokens[pos].type == Token.TokenType.WhiteSpace || tokens[pos].type == Token.TokenType.Comment))
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
            if (current.type == Token.TokenType.Identifier)
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

            bool ignoreSemicolon = false;
            if (current.type == Token.TokenType.If)
            {
                result = parseIf(tokens, ref pos, scope);
                ignoreSemicolon = true;
            }

            if (current.type == Token.TokenType.For)
            {
                result = parseForLoop(tokens, ref pos, scope);
                ignoreSemicolon = true;
            }

            if (current.type == Token.TokenType.Let)
            {
                // TODO: distinguish between constant variables and function definition
                result = parseFunctionDefinition(tokens, ref pos, scope);
                ignoreSemicolon = true;
            }

            if (!ignoreSemicolon)
            {
                var endOfStatement = nextToken(tokens, ref pos, true);
                expectTokenType(endOfStatement, Token.TokenType.Semicolon);
            }

            if (result == null)
            {
                throw new ParserError(string.Format("Unexpected token type: \"{0}\"", current.type), current);
            }

            return result;
        }

        static Node parseForLoop(IList<Token> tokens, ref int pos, Scope scope)
        {

            // for
            var current = tokens[pos];
            expectTokenType(current, Token.TokenType.For);

            // for(
            var ob = nextToken(tokens, ref pos);
            expectTokenType(ob, Token.TokenType.OpenBracket);

            var result = new ForLoop(current);
            var loopBodyScope = new Scope();
            loopBodyScope.parent = scope;

            // for(int i = 0
            nextToken(tokens, ref pos);
            result.initializer = parseVariableDeclaration(tokens, ref pos, loopBodyScope);

            // for(int i = 0;
            nextToken(tokens, ref pos);
            expectTokenType(tokens[pos], Token.TokenType.Semicolon);

            // for(int i = 0; i < 10
            nextToken(tokens, ref pos);
            result.condition = parseBinOp(tokens, ref pos, loopBodyScope);

            // for(int i = 0; i < 10;
            nextToken(tokens, ref pos);
            expectTokenType(tokens[pos], Token.TokenType.Semicolon);

            // for(int i = 0; i < 10; i = i + 1
            nextToken(tokens, ref pos);
            result.iterator = parseForIterator(tokens, ref pos, loopBodyScope);

            // for(int i = 0; i < 10; i = i + 1)
            var cb = nextToken(tokens, ref pos);
            expectTokenType(cb, Token.TokenType.CloseBracket);

            // for(int i = 0; i < 10; i = i + 1) { ... }
            nextToken(tokens, ref pos);
            result.loopBody = parseBlock(tokens, ref pos, scope, newScope: loopBodyScope);

            return result;
        }

        static Node parseForIterator(IList<Token> tokens, ref int pos, Scope scope)
        {
            var current = tokens[pos];
            var next = peekToken(tokens, pos);
            if (current.type == Token.TokenType.Identifier)
            {
                if (next.type == Token.TokenType.CloseBracket
                    || next.type == Token.TokenType.Increment || next.type == Token.TokenType.Decrement)
                {
                    var variable = parseVariableLookup(tokens, ref pos, scope);
                    return variable;
                }

                if (next.type == Token.TokenType.OpenBracket)
                {
                    nextToken(tokens, ref pos);
                    var result = parseFunctionCall(tokens, ref pos, scope);
                    return result;
                }
            }

            if (current.type == Token.TokenType.Increment || current.type == Token.TokenType.Decrement)
            {
                var variable = parseVariableLookup(tokens, ref pos, scope);
                return variable;
            }

            throw new InvalidCodePath();
        }

        static Node parseIf(IList<Token> tokens, ref int pos, Scope scope)
        {
            var current = tokens[pos];
            expectTokenType(current, Token.TokenType.If);

            var ob = nextToken(tokens, ref pos);
            expectTokenType(ob, Token.TokenType.OpenBracket);

            var result = new IfCondition(current);
            nextToken(tokens, ref pos);
            result.condition = parseBinOp(tokens, ref pos, scope);

            var cb = nextToken(tokens, ref pos);
            expectTokenType(cb, Token.TokenType.CloseBracket);

            nextToken(tokens, ref pos);

            result.thenBlock = parseBlock(tokens, ref pos, scope);

            var next = peekToken(tokens, pos);
            if (next.type == Token.TokenType.Else)
            {
                nextToken(tokens, ref pos);
                nextToken(tokens, ref pos);
                result.elseBlock = parseBlock(tokens, ref pos, scope);
            }

            return result;
        }

        static Node parseReturn(IList<Token> tokens, ref int pos, Scope scope)
        {
            var current = tokens[pos];
            expectTokenType(current, Token.TokenType.Return);

            var next = peekToken(tokens, pos, true, true);
            if (next.type == Token.TokenType.Semicolon)
            {
                var result = new Return(current);
                return result;
            }
            else
            {
                var result = new Return(current);
                nextToken(tokens, ref pos);
                result.expression = parseBinOp(tokens, ref pos, scope);
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
            result.expression = parseBinOp(tokens, ref pos, scope);

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
            result.expression = parseBinOp(tokens, ref pos, scope);

            return result;
        }


        static bool isBinOp(Token t, int precedence)
        {
            var tt = t.type;
            switch (precedence)
            {
                case 2:
                    return tt == Token.TokenType.Multiply
                        || tt == Token.TokenType.Divide
                        || tt == Token.TokenType.Remainder;
                case 3:
                    return tt == Token.TokenType.Add
                        || tt == Token.TokenType.Subtract;
                case 4:
                    return tt == Token.TokenType.LeftShift
                        || tt == Token.TokenType.RightShift;
                case 5:
                    return tt == Token.TokenType.Less
                        || tt == Token.TokenType.Greater
                        || tt == Token.TokenType.LessEqual
                        || tt == Token.TokenType.GreaterEqual;
                case 6:
                    return tt == Token.TokenType.Equal
                        || tt == Token.TokenType.NotEqual;
                case 7:
                    return tt == Token.TokenType.LogicalAND;
                case 8:
                    return tt == Token.TokenType.LogicalXOR;
                case 9:
                    return tt == Token.TokenType.LogicalOR;
                case 10:
                    return tt == Token.TokenType.ConditionalAND;
                case 11:
                    return tt == Token.TokenType.ConditionalOR;
                default:
                    throw new InvalidCodePath();

            }
        }
        private static Node parseBinOp(IList<Token> tokens, ref int pos, Scope scope)
        {
            return parseBinOp(tokens, ref pos, scope, 11);
        }
        private static Node parseBinOp(IList<Token> tokens, ref int pos, Scope scope, int precedence)
        {
            if (precedence == 1)
            {
                return parseUnary(tokens, ref pos, scope);
            }
            var result = parseBinOp(tokens, ref pos, scope, precedence - 1);
            var otherFactor = default(Node);
            var next = peekToken(tokens, pos, tokenMustExist: false, skipWS: true);

            while (isBinOp(next, precedence))
            {
                // continue to the next token after the add or subtract
                nextToken(tokens, ref pos, true);
                nextToken(tokens, ref pos, true);

                otherFactor = parseBinOp(tokens, ref pos, scope, precedence - 1);
                var bo = new BinOp(next);
                bo.left = result;
                bo.right = otherFactor;
                bo.SetTypeFromToken(next);
                result = bo;
                next = peekToken(tokens, pos, tokenMustExist: false, skipWS: true);
            }

            return result;
        }

        // operator precedence 1
        private static Node parseUnary(IList<Token> tokens, ref int pos, Scope scope)
        {
            var current = tokens[pos];


            // handle unary plus and minus, ! and ~
            if (current.type == Token.TokenType.Add || current.type == Token.TokenType.Subtract
                || current.type == Token.TokenType.LogicalNOT || current.type == Token.TokenType.Complement)
            {
                var result = new UnaryOp(current);
                result.SetTypeFromToken(current);
                nextToken(tokens, ref pos);
                result.expression = parsePrimary(tokens, ref pos, scope);
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
                var exp = parsePrimary(tokens, ref pos, scope);

                var result = new TypeCastOp(current);

                // TODO: check if valid type (in type check phase?)
                result.type = scope.GetType(typeNameToken.text);
                result.expression = exp;
                return result;
            }

            return parsePrimary(tokens, ref pos, scope);
        }

        // operator precedence 0
        private static Node parsePrimary(IList<Token> tokens, ref int pos, Scope scope)
        {
            var current = tokens[pos];
            expectTokenType(current, Token.TokenType.IntNumber, Token.TokenType.FloatNumber, Token.TokenType.Identifier, Token.TokenType.OpenBracket,
                Token.TokenType.True, Token.TokenType.False, Token.TokenType.Increment, Token.TokenType.Decrement, Token.TokenType.String);

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
            if (current.type == Token.TokenType.True || current.type == Token.TokenType.False)
            {
                var result = new ConstBool(current);
                result.value = current.type == Token.TokenType.True;
                return result;
            }
            if (current.type == Token.TokenType.String)
            {
                var result = new ConstString(current);
                result.s = current.text;
                return result;
            }
            // '(' expr ')'
            if (current.type == Token.TokenType.OpenBracket)
            {
                var exprStart = nextToken(tokens, ref pos);
                var result = parseBinOp(tokens, ref pos, scope);
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
                    // TODO: check if function exists!
                    var result = parseFunctionCall(tokens, ref pos, scope);
                    return result;
                }

                return parseVariableLookup(tokens, ref pos, scope);
            }

            if (current.type == Token.TokenType.Increment || current.type == Token.TokenType.Decrement)
            {
                return parseVariableLookup(tokens, ref pos, scope);
            }
            throw new ParserError("Unexpected token type: " + current.type, current);
        }

        static Node parseVariableLookup(IList<Token> tokens, ref int pos, Scope scope)
        {
            var current = tokens[pos];
            expectTokenType(current, Token.TokenType.Identifier, Token.TokenType.Increment, Token.TokenType.Decrement);

            var varLookup = new VariableLookup(current);
            varLookup.inc = VariableLookup.Incrementor.None;

            if (current.type == Token.TokenType.Increment || current.type == Token.TokenType.Decrement)
            {
                varLookup.inc = current.type == Token.TokenType.Increment ?
                    VariableLookup.Incrementor.preIncrement : VariableLookup.Incrementor.preDecrement;
                var id = nextToken(tokens, ref pos);
                expectTokenType(id, Token.TokenType.Identifier);
                varLookup.token = id;
                current = id;
            }

            varLookup.variableName = current.text;

            if (scope.GetVar(current.text) == null)
            {
                throw new UndefinedVariable(current.text, current);
            }

            var peek = peekToken(tokens, pos);
            if (peek.type == Token.TokenType.Increment || peek.type == Token.TokenType.Decrement)
            {
                if (varLookup.inc != VariableLookup.Incrementor.None)
                {
                    throw new ParserError("You can't use both pre-increment and post-increment", peek);
                }
                nextToken(tokens, ref pos);
                varLookup.inc = peek.type == Token.TokenType.Increment ?
                    VariableLookup.Incrementor.postIncrement : VariableLookup.Incrementor.postDecrement;
            }

            return varLookup;
        }

        static Node parseFunctionCall(IList<Token> tokens, ref int pos, Scope scope)
        {
            var current = tokens[pos];
            expectTokenType(current, Token.TokenType.Identifier);

            var result = new FunctionCall(current);
            result.functionName = current.text;

            if (scope.GetFunction(result.functionName) == null)
            {
                throw new ParserError(string.Format("Undefined function \"{0}\"", result.functionName), current);
            }
            
            var ob = nextToken(tokens, ref pos, skipWS: true);
            expectTokenType(ob, Token.TokenType.OpenBracket);
            
            var next = peekToken(tokens, pos, skipWS: true);
            if (next.type != Token.TokenType.CloseBracket)
            {
                while (true)
                {
                    nextToken(tokens, ref pos);
                    var exp = parseBinOp(tokens, ref pos, scope);
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


        public static Node parseBlock(IList<Token> tokens, ref int pos, Scope parentScope, 
            Scope newScope = null)
        {
            var current = tokens[pos];
            expectTokenType(current, Token.TokenType.OpenCurly);

            var result = new Block(current);
            if (newScope == null)
            {
                newScope = new Scope();
                newScope.parent = parentScope;
                newScope.function = parentScope.function;
            }
            result.scope = newScope;
            
            var next = peekToken(tokens, pos);

            bool foundReturn = false;
            while (next.type != Token.TokenType.CloseCurly)
            {
                nextToken(tokens, ref pos);
                var s = parseStatement(tokens, ref pos, result.scope);
                // ignore statements after the return so that return is the last statement in the block
                if (!foundReturn)
                {
                    result.statements.Add(s);
                }
                if (s is Return)
                {
                    foundReturn = true;
                }
                next = peekToken(tokens, pos);
                if (pos >= tokens.Count || next == null)
                {
                    throw new ParserError("No matching \"}\" found", current);
                }

            }

            nextToken(tokens, ref pos);
            return result;
        }


        public static Node parseMainBlock(IList<Token> tokens, ref int pos, Scope rootScope)
        {
            var current = tokens[pos];
            var result = new Block(current);
            result.scope = rootScope;

            var next = current;

            bool foundReturn = false;
            while (next.type != Token.TokenType.EOF)
            {
                
                var s = parseStatement(tokens, ref pos, result.scope);
                // ignore statements after the return so that return is the last statement in the block
                if (!foundReturn)
                {
                    result.statements.Add(s);
                }
                if (s is Return)
                {
                    foundReturn = true;
                }

                next = nextToken(tokens, ref pos); ;
            }
            return result;
        }

        // TODO: make this LET thing work for variables as well
        public static Node parseFunctionDefinition(IList<Token> tokens, ref int pos, Scope scope)
        {

            // let
            var current = tokens[pos];
            expectTokenType(current, Token.TokenType.Let);

            if (scope.parent != null)
                throw new ParserError("functions can only be defined in the primary block for now!", current);

            var fun = new FunctionDefinition();

            // let foo
            var id = nextToken(tokens, ref pos);
            expectTokenType(id, Token.TokenType.Identifier);
            fun.name = id.text;

            // let foo = 
            var ass = nextToken(tokens, ref pos);
            expectTokenType(ass, Token.TokenType.Assignment);

            // let foo = (
            var ob = nextToken(tokens, ref pos);
            expectTokenType(ob, Token.TokenType.OpenBracket);

            var next = peekToken(tokens, pos);
            while (true)
            {
                // let foo = (x
                var pid = nextToken(tokens, ref pos);
                expectTokenType(pid, Token.TokenType.Identifier);

                // let foo = (x: 
                expectTokenType(nextToken(tokens, ref pos), Token.TokenType.Colon);

                // let foo = (x: int32
                var ptyp = nextToken(tokens, ref pos);
                expectTokenType(ptyp, Token.TokenType.Identifier);
                var type = scope.GetType(ptyp.text);
                if (type == null)
                {
                    throw new ParserError(string.Format("Could not resolve type in function parameter list: {0}", type), ptyp);
                }
                fun.AddParameter(pid.text, type);

                // let foo = (x: int32
                next = nextToken(tokens, ref pos);
                if (next == null)
                {
                    throw new ParserError("Missing \")\" in function definition", ptyp);
                }
                if (next.type != Token.TokenType.CloseBracket)
                {
                    // let foo = (x: int32,
                    expectTokenType(next, Token.TokenType.Comma);
                    continue;
                }
                else
                {
                    // let foo = (x: int32)
                    nextToken(tokens, ref pos);
                    break;
                }
            }

            // let foo = (x: int32) =>
            var arrow = tokens[pos];
            expectTokenType(arrow, Token.TokenType.FatArrow);

            nextToken(tokens, ref pos);
            // let foo = (x: int32) => { ... }
            scope.AddFunction(fun);
            var result = new FunctionDeclaration(current);
            result.fun = fun;
            var funScope = new Scope();
            funScope.parent = scope;
            funScope.function = fun;

            var idx = 0;
            foreach (var pd in fun.parameters)
            {
                funScope.AddFunctionParameter(pd.name, pd.type, idx);
                idx++;
            }

            result.body = parseBlock(tokens, ref pos, null, funScope);

            var block = result.body as Block;
            if (!(block.statements.Last() is Return))
            {
                var error = string.Format("Last statement of function \"{0}\" must be a \"return\" (for now)", result.fun.name);
                throw new ParserError(error, block.statements.Last().token);
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
#if !DEBUG
            try
#endif
            {
                var rootScope = new Scope();
                rootScope.AddType(FrontendType.float32, current);
                rootScope.AddType(FrontendType.int32, current);
                rootScope.AddType(FrontendType.bool_, current);
                
                var print_i32 = new FunctionDefinition { name = "print_i32", returnType = FrontendType.void_ };
                print_i32.AddParameter("x", FrontendType.int32);
                rootScope.AddFunction(print_i32);

                var print_f32 = new FunctionDefinition { name = "print_f32", returnType = FrontendType.void_ };
                print_f32.AddParameter("x", FrontendType.float32);
                rootScope.AddFunction(print_f32);

                var print = new FunctionDefinition { name = "print", returnType = FrontendType.void_ };
                print.AddParameter("s", FrontendType.string_);
                rootScope.AddFunction(print);

                var main = new FunctionDefinition { name = "main", returnType = FrontendType.int32 };
                rootScope.AddFunction(main);
                rootScope.function = main;
                // perform AST generation pass
                pos = skipWhitespace(tokens, pos);
                block = parseMainBlock(tokens, ref pos, rootScope);

                // perform type checking pass
                block.CheckType(rootScope);
            }
#if !DEBUG
            catch (ParserError error)
            {
                Console.WriteLine();
                Console.Error.WriteLine(error.Message);
                Console.WriteLine();
                return null;
            }
#endif

            return block;
        }

    }

}
