using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PragmaScript
{


    partial class AST
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

            public abstract Task<FrontendType> CheckType(Scope scope);
        }

        public class AnnotatedNode : Node
        {
            Node node;
            public string annotation;
            public AnnotatedNode(Node n, string annotation)
                : base(n.token)
            {
                node = n;
                this.annotation = annotation;
            }

            public override IEnumerable<Node> GetChilds()
            {
                foreach (var n in node.GetChilds())
                {
                    yield return n;
                }
            }

            public override async Task<FrontendType> CheckType(Scope scope)
            {
                return await node.CheckType(scope);
            }

            public override string ToString()
            {
                return node.ToString();
            }
        }



        public class FrontendType
        {
            public static readonly FrontendType void_ = new FrontendType { name = "void" };
            public static readonly FrontendType int32 = new FrontendType { name = "int32" };
            public static readonly FrontendType float32 = new FrontendType { name = "float32" };
            public static readonly FrontendType bool_ = new FrontendType { name = "bool" };
            public static readonly FrontendType string_ = new FrontendType { name = "string" };
            string name;

            public FrontendType(string name)
            {
                this.name = name;
            }

            protected FrontendType()
            {

            }

            public override int GetHashCode()
            {
                return ToString().GetHashCode();
            }

            public override string ToString()
            {
                return name;
            }

            public override bool Equals(object obj)
            {
                return ToString() == obj.ToString();
            }
        }

        public class FrontendArrayType : FrontendStructType
        {
            public FrontendType elementType;
            string name;
            public FrontendArrayType(FrontendType elementType)
            {
                this.elementType = elementType;
                name = "[" + elementType + "]";
                AddField("length", FrontendType.int32);
            }

            public override string ToString()
            {
                return name;
            }
        }

        public class FrontendStructType : FrontendType
        {
            Dictionary<string, FrontendType> fields = new Dictionary<string, FrontendType>();


            string name = "";
            public FrontendStructType()
            {
            }

            public void AddField(string name, FrontendType type)
            {
                fields.Add(name, type);
            }

            public FrontendType GetField(string name)
            {
                var result = default(FrontendType);
                if (fields.TryGetValue(name, out result))
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }

            void calcTypeName()
            {
                name = "{" + string.Join(",", fields.Values) + "}";
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
                parameters.Add(new NamedParameter { name = name, type = type });
            }
        }

        public class Scope
        {
            public FunctionDefinition function;
            public Scope parent;

            public Dictionary<string, VariableDefinition> variables = new Dictionary<string, VariableDefinition>();
            public Dictionary<string, FunctionDefinition> functions = new Dictionary<string, FunctionDefinition>();
            public Dictionary<string, FrontendType> types = new Dictionary<string, FrontendType>();


            public Scope(Scope parent, FunctionDefinition function)
            {
                this.parent = parent;
                this.function = function;
            }

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
                FrontendType type = new FrontendType(name);
                if (types.ContainsKey(name))
                {
                    throw new RedefinedType(name, t);
                }
                types.Add(name, type);
            }

            public void AddType(FrontendType t, Token token)
            {
                if (types.ContainsKey(t.ToString()))
                {
                    throw new RedefinedType(t.ToString(), token);
                }
                types.Add(t.ToString(), t);
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
            public override async Task<FrontendType> CheckType(Scope scope)
            {

                var types = await Task.WhenAll(statements.Select(s => s.CheckType(this.scope)));
                return null;
            }

            public override string ToString()
            {
                return "Block";
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
                if (!next.isAssignmentOperator() && !(next.type == Token.TokenType.OpenBracket)
                    && !((next.type == Token.TokenType.Increment || next.type == Token.TokenType.Decrement)))
                {
                    throw new ParserErrorExpected("assignment operator, function call, or increment/decrement", next.type.ToString(), next);
                }

                if (next.type == Token.TokenType.OpenBracket)
                {
                    result = parseFunctionCall(tokens, ref pos, scope);
                }
                else if (next.isAssignmentOperator())
                {
                    result = parseAssignment(tokens, ref pos, scope);
                }
                else if (next.type == Token.TokenType.Increment || next.type == Token.TokenType.Decrement)
                {
                    result = parseVariableLookup(tokens, ref pos, scope);
                }
                else
                {
                    throw new InvalidCodePath();
                }
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

            if (current.type == Token.TokenType.While)
            {
                result = parseWhileLoop(tokens, ref pos, scope);
                ignoreSemicolon = true;
            }

            if (current.type == Token.TokenType.Let)
            {
                // TODO: distinguish between constant variables and function definition
                result = parseFunctionDefinition(tokens, ref pos, scope);
                ignoreSemicolon = true;
            }


            // TODO: check if inside loop
            if (current.type == Token.TokenType.Continue)
            {
                result = new ContinueLoop(current);
            }

            // TODO: check if inside loop
            if (current.type == Token.TokenType.Break)
            {
                result = new BreakLoop(current);
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


        static Node parseWhileLoop(IList<Token> tokens, ref int pos, Scope scope)
        {

            // while
            var current = tokens[pos];
            expectTokenType(current, Token.TokenType.While);

            // while (
            var ob = nextToken(tokens, ref pos);
            expectTokenType(ob, Token.TokenType.OpenBracket);

            var result = new WhileLoop(current);
            var loopBodyScope = new Scope(scope, scope.function);


            // while (i < 10
            nextToken(tokens, ref pos);
            result.condition = parseBinOp(tokens, ref pos, loopBodyScope);

            // while (i < 10)
            var cb = nextToken(tokens, ref pos);
            expectTokenType(cb, Token.TokenType.CloseBracket);

            // while (i < 10) { ... }
            nextToken(tokens, ref pos);
            result.loopBody = parseBlock(tokens, ref pos, scope, newScope: loopBodyScope);

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
            var loopBodyScope = new Scope(scope, scope.function);

            // for(int i = 0
            var next = peekToken(tokens, pos);
            if (next.type != Token.TokenType.Semicolon)
            {
                nextToken(tokens, ref pos);
                result.initializer = parseForInitializer(tokens, ref pos, loopBodyScope);
            }
            else
            {
                result.initializer = new List<Node>();
            }

            // for(int i = 0;
            nextToken(tokens, ref pos);
            expectTokenType(tokens[pos], Token.TokenType.Semicolon);

            next = peekToken(tokens, pos);
            if (next.type != Token.TokenType.Semicolon)
            {
                // for(int i = 0; i < 10
                nextToken(tokens, ref pos);
                result.condition = parseBinOp(tokens, ref pos, loopBodyScope);
            }
            else
            {
                result.condition = new ConstBool(next, true);
            }

            // for(int i = 0; i < 10;
            nextToken(tokens, ref pos);
            expectTokenType(tokens[pos], Token.TokenType.Semicolon);

            next = peekToken(tokens, pos);
            if (next.type != Token.TokenType.CloseBracket)
            {
                // for(int i = 0; i < 10; i = i + 1
                nextToken(tokens, ref pos);
                result.iterator = parseForIterator(tokens, ref pos, loopBodyScope);
            }
            else
            {
                result.iterator = new List<Node>();
            }

            // for(int i = 0; i < 10; i = i + 1)
            var cb = nextToken(tokens, ref pos);
            expectTokenType(cb, Token.TokenType.CloseBracket);

            // for(int i = 0; i < 10; i = i + 1) { ... }
            nextToken(tokens, ref pos);
            result.loopBody = parseBlock(tokens, ref pos, scope, newScope: loopBodyScope);

            return result;
        }

        static List<Node> parseForInitializer(IList<Token> tokens, ref int pos, Scope scope)
        {
            return parseForStatements(tokens, ref pos, scope, declaration: true);
        }
        static List<Node> parseForIterator(IList<Token> tokens, ref int pos, Scope scope)
        {
            return parseForStatements(tokens, ref pos, scope, declaration: false);
        }

        static List<Node> parseForStatements(IList<Token> tokens, ref int pos, Scope scope, bool declaration)
        {
            var current = tokens[pos];
            var next = peekToken(tokens, pos);
            var result = new List<Node>();

            while (true)
            {
                if (current.type == Token.TokenType.Var && declaration)
                {
                    var vd = parseVariableDeclaration(tokens, ref pos, scope);
                    result.Add(vd);
                }
                else if (current.type == Token.TokenType.Identifier)
                {
                    if (next.type == Token.TokenType.CloseBracket
                        || next.type == Token.TokenType.Increment || next.type == Token.TokenType.Decrement)
                    {
                        var variable = parseVariableLookup(tokens, ref pos, scope);
                        result.Add(variable);
                    }
                    else if (next.type == Token.TokenType.OpenBracket)
                    {
                        nextToken(tokens, ref pos);
                        var call = parseFunctionCall(tokens, ref pos, scope);
                        result.Add(call); ;
                    }
                    else if (next.isAssignmentOperator())
                    {
                        var assignment = parseAssignment(tokens, ref pos, scope);
                        result.Add(assignment);
                    }
                    else
                    {
                        throw new ParserError("Invalid statement in for " + (declaration ? "initializer" : "iterator"), current);
                    }
                }
                else if (current.type == Token.TokenType.Increment || current.type == Token.TokenType.Decrement)
                {
                    var variable = parseVariableLookup(tokens, ref pos, scope);
                    result.Add(variable);
                }
                else
                {
                    if (declaration)
                    {
                        expectTokenType(current, Token.TokenType.Comma, Token.TokenType.Semicolon);
                    }
                    else
                    {
                        expectTokenType(current, Token.TokenType.Comma, Token.TokenType.CloseBracket);
                    }
                }

                next = peekToken(tokens, pos);
                if (next.type != Token.TokenType.Comma)
                {
                    break;
                }
                else
                {
                    nextToken(tokens, ref pos);
                    current = nextToken(tokens, ref pos);
                }
            }

            return result;
        }

        static Node parseIf(IList<Token> tokens, ref int pos, Scope scope)
        {
            // if
            var current = tokens[pos];
            expectTokenType(current, Token.TokenType.If);

            // if (
            var ob = nextToken(tokens, ref pos);
            expectTokenType(ob, Token.TokenType.OpenBracket);

            var result = new IfCondition(current);

            // if(i < 10
            nextToken(tokens, ref pos);
            result.condition = parseBinOp(tokens, ref pos, scope);

            // if(i < 10)
            var cb = nextToken(tokens, ref pos);
            expectTokenType(cb, Token.TokenType.CloseBracket);

            // if(i < 10) {
            nextToken(tokens, ref pos);
            result.thenBlock = parseBlock(tokens, ref pos, scope);

            // if(i < 10) { ... } elif
            var next = peekToken(tokens, pos);
            while (next.type == Token.TokenType.Elif)
            {
                nextToken(tokens, ref pos);
                var elif = parseElif(tokens, ref pos, scope);
                result.elifs.Add(elif);
                next = peekToken(tokens, pos);
            }

            if (next.type == Token.TokenType.Else)
            {
                nextToken(tokens, ref pos);
                nextToken(tokens, ref pos);
                result.elseBlock = parseBlock(tokens, ref pos, scope);
            }

            return result;
        }

        static Node parseElif(IList<Token> tokens, ref int pos, Scope scope)
        {
            // elif
            var current = tokens[pos];
            expectTokenType(current, Token.TokenType.Elif);

            // elif (
            var ob = nextToken(tokens, ref pos);
            expectTokenType(ob, Token.TokenType.OpenBracket);

            var result = new Elif(current);

            // elif(i < 10
            nextToken(tokens, ref pos);
            result.condition = parseBinOp(tokens, ref pos, scope);

            // elif(i < 10)
            var cb = nextToken(tokens, ref pos);
            expectTokenType(cb, Token.TokenType.CloseBracket);

            // elif(i < 10) {
            nextToken(tokens, ref pos);
            result.thenBlock = parseBlock(tokens, ref pos, scope);

            return result;
        }

        static Node parseReturn(IList<Token> tokens, ref int pos, Scope scope)
        {
            var current = tokens[pos];
            expectTokenType(current, Token.TokenType.Return);

            var next = peekToken(tokens, pos, true, true);
            if (next.type == Token.TokenType.Semicolon)
            {
                var result = new ReturnFunction(current);
                return result;
            }
            else
            {
                var result = new ReturnFunction(current);
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
            // expectTokenType(assign, Token.TokenType.Assignment);
            if (!assign.isAssignmentOperator())
            {
                throw new ParserErrorExpected("assignment operator", assign.type.ToString(), assign);
            }

            var firstExpressionToken = nextToken(tokens, ref pos, skipWS: true);

            var result = new Assignment(current);
            var variable = scope.GetVar(current.text);
            if (variable == null)
            {
                throw new UndefinedVariable(current.text, current);
            }
            result.variable = variable;

            var exp = parseBinOp(tokens, ref pos, scope);

            var compound = new BinOp(assign);
            var v = new VariableLookup(current);
            v.variableName = current.text;
            compound.left = v;
            compound.right = exp;

            result.expression = compound;

            switch (assign.type)
            {
                case Token.TokenType.Assignment:
                    result.expression = exp;
                    break;
                case Token.TokenType.PlusEquals:
                    compound.type = BinOp.BinOpType.Add;
                    break;
                case Token.TokenType.RightShiftEquals:
                    compound.type = BinOp.BinOpType.RightShift;
                    break;
                case Token.TokenType.LeftShiftEquals:
                    compound.type = BinOp.BinOpType.LeftShift;
                    break;
                case Token.TokenType.XOREquals:
                    compound.type = BinOp.BinOpType.LogicalXOR;
                    break;
                case Token.TokenType.OREquals:
                    compound.type = BinOp.BinOpType.LogicalOR;
                    break;
                case Token.TokenType.DivideEquals:
                    compound.type = BinOp.BinOpType.Divide;
                    break;
                case Token.TokenType.ANDEquals:
                    compound.type = BinOp.BinOpType.LogicalAND;
                    break;
                case Token.TokenType.RemainderEquals:
                    compound.type = BinOp.BinOpType.Remainder;
                    break;
                case Token.TokenType.MultiplyEquals:
                    compound.type = BinOp.BinOpType.Multiply;
                    break;
                case Token.TokenType.MinusEquals:
                    compound.type = BinOp.BinOpType.Subract;
                    break;
                default:
                    throw new InvalidCodePath();
            }



            return result;
        }


        static Node parseArray(IList<Token> tokens, ref int pos, Scope scope)
        {
            var current = tokens[pos];
            expectTokenType(current, Token.TokenType.OpenSquareBracket);

            var next = peekToken(tokens, pos);

            var result = new ConstArray(current);

            while (next.type != Token.TokenType.CloseSquareBracket)
            {
                current = nextToken(tokens, ref pos);
                var elem = parseBinOp(tokens, ref pos, scope);
                result.elements.Add(elem);
                next = peekToken(tokens, pos);
                if (next.type == Token.TokenType.Comma)
                {
                    nextToken(tokens, ref pos);
                }
                else
                {
                    expectTokenType(next, Token.TokenType.CloseSquareBracket);
                }
            }

            nextToken(tokens, ref pos);
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

            if (firstExpressionToken.type == Token.TokenType.OpenSquareBracket)
            {
                result.expression = parseArray(tokens, ref pos, scope);
            }
            else
            {
                result.expression = parseBinOp(tokens, ref pos, scope);
            }


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

            // either function call variable or struct field access
            if (current.type == Token.TokenType.Identifier)
            {
                var peek = peekToken(tokens, pos, skipWS: true);
                if (peek.type == Token.TokenType.OpenBracket)
                {
                    var result = parseFunctionCall(tokens, ref pos, scope);
                    return result;
                }
                if (peek.type == Token.TokenType.Dot)
                {
                    return parseStructFieldAccess(tokens, ref pos, scope);
                }
                return parseVariableLookup(tokens, ref pos, scope);
            }

            if (current.type == Token.TokenType.Increment || current.type == Token.TokenType.Decrement)
            {
                return parseVariableLookup(tokens, ref pos, scope);
            }
            throw new ParserError("Unexpected token type: " + current.type, current);
        }


        static Node parseStructFieldAccess(IList<Token> tokens, ref int pos, Scope scope)
        {
            var current = tokens[pos];
            expectTokenType(current, Token.TokenType.Identifier);

            var dot = nextToken(tokens, ref pos);
            expectTokenType(dot, Token.TokenType.Dot);

            var fieldName = nextToken(tokens, ref pos);
            expectTokenType(fieldName, Token.TokenType.Identifier);

            

            if (scope.GetVar(current.text) == null)
            {
                throw new UndefinedVariable(current.text, current);
            }
            var result = new StructFieldAccess(current);
            result.structName = current.text;
            result.fieldName = fieldName.text;

            // TODO: what happens if the field is an array? [ ]
            return result;
        }

        static Node parseVariableLookup(IList<Token> tokens, ref int pos, Scope scope)
        {
            var current = tokens[pos];
            expectTokenType(current, Token.TokenType.Identifier, Token.TokenType.Increment, Token.TokenType.Decrement);

            VariableLookup result = new VariableLookup(current);
            result.inc = VariableLookup.Incrementor.None;

            if (current.type == Token.TokenType.Increment || current.type == Token.TokenType.Decrement)
            {
                result.inc = current.type == Token.TokenType.Increment ?
                    VariableLookup.Incrementor.preIncrement : VariableLookup.Incrementor.preDecrement;
                var id = nextToken(tokens, ref pos);
                expectTokenType(id, Token.TokenType.Identifier);
                result.token = id;
                current = id;
            }

            result.variableName = current.text;

            if (scope.GetVar(current.text) == null)
            {
                throw new UndefinedVariable(current.text, current);
            }

            var peek = peekToken(tokens, pos);
            if (peek.type == Token.TokenType.Increment || peek.type == Token.TokenType.Decrement)
            {
                if (result.inc != VariableLookup.Incrementor.None)
                {
                    throw new ParserError("You can't use both pre-increment and post-increment", peek);
                }
                nextToken(tokens, ref pos);
                result.inc = peek.type == Token.TokenType.Increment ?
                    VariableLookup.Incrementor.postIncrement : VariableLookup.Incrementor.postDecrement;
            }

            // TODO: handle multi dimensional arrays
            if (peek.type == Token.TokenType.OpenSquareBracket)
            {
                // skip [ token
                nextToken(tokens, ref pos);
                nextToken(tokens, ref pos);

                var arrayElem = new ArrayElementAccess(peek);
                arrayElem.variableName = current.text;

                arrayElem.index = parseBinOp(tokens, ref pos, scope);
                var cb = nextToken(tokens, ref pos);
                expectTokenType(cb, Token.TokenType.CloseSquareBracket);

                return arrayElem;
            }
            return result;
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
                newScope = new Scope(parentScope, parentScope.function);
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
                if (s is ReturnFunction)
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
                if (s is ReturnFunction)
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

            if (next.type != Token.TokenType.CloseBracket)
            {
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
            }
            else
            {
                nextToken(tokens, ref pos);
                nextToken(tokens, ref pos);
                // let foo = ( ) =>
            }

            // let foo = (x: int32) =>
            var arrow = tokens[pos];
            expectTokenType(arrow, Token.TokenType.FatArrow);

            nextToken(tokens, ref pos);
            // let foo = (x: int32) => { ... }
            scope.AddFunction(fun);
            var result = new FunctionDeclaration(current);
            result.fun = fun;
            var funScope = new Scope(scope, fun);

            var idx = 0;
            foreach (var pd in fun.parameters)
            {
                funScope.AddFunctionParameter(pd.name, pd.type, idx);
                idx++;
            }

            result.body = parseBlock(tokens, ref pos, null, funScope);

            var block = result.body as Block;
            //if (!(block.statements.Last() is Return))
            //{
            //    // TODO: do proper checking if all code path have a return statement
            //    var rtn = new Return(result.body.token);


            //    // TODO: have a default operator to return a default value for a given type
            //    if (fun.returnType == FrontendType.int32)
            //    {
            //        var i = new ConstInt32(Token.Undefined);
            //        i.number = 0;
            //        rtn.expression = i;
            //    }
            //    if (fun.returnType == FrontendType.float32)
            //    {
            //        var f = new ConstFloat32(Token.Undefined);
            //        f.number = 0.0f;
            //        rtn.expression = f;
            //    }

            //    block.statements.Add(rtn);
            //}

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


        static async Task<FrontendType> performTypeChecking(Node main, Scope root)
        {
            try
            {
                var result = await main.CheckType(root);
                return result;
            }
            catch (Exception e)
            {
                throw e;
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
                var main = new FunctionDefinition { name = "main", returnType = FrontendType.int32 };

                var rootScope = new Scope(null, main);
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

                var read = new FunctionDefinition { name = "read", returnType = FrontendType.string_ };
                rootScope.AddFunction(read);

                var cat = new FunctionDefinition { name = "cat", returnType = FrontendType.void_ };
                rootScope.AddFunction(cat);


                rootScope.AddFunction(main);
                rootScope.function = main;
                // perform AST generation pass
                pos = skipWhitespace(tokens, pos);
                block = parseMainBlock(tokens, ref pos, rootScope);

                // perform type checking pass
                try
                {
                    performTypeChecking(block, rootScope).Wait();
                }
                catch (System.AggregateException e)
                {
                    throw e.InnerException;
                }
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
