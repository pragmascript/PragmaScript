using System.Collections.Generic;
using System.Globalization;

namespace PragmaScript
{
    partial class AST
    {
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

            // TODO: make this LET thing work for variables as well
            if (current.type == Token.TokenType.Let)
            {
                var letPos = pos;

                // TODO: do i really need 3 lookahead?
                expectNext(tokens, ref pos, Token.TokenType.Identifier);
                expectNext(tokens, ref pos, Token.TokenType.Assignment);
                var next = peekTokenUpdatePos(tokens, ref pos);

                pos = letPos;

                // TODO: distinguish between constant variables and function definition
                if (next.type == Token.TokenType.Struct)
                {
                    result = parseStructDefinition(tokens, ref pos, scope);
                    ignoreSemicolon = true;
                }
                else
                {
                    result = parseFunctionDefinition(tokens, ref pos, scope);
                    ignoreSemicolon = true;
                }
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
            var current = expectCurrent(tokens, pos, Token.TokenType.While);

            // while (
            expectNext(tokens, ref pos, Token.TokenType.OpenBracket);

            var result = new WhileLoop(current);
            var loopBodyScope = new Scope(scope, scope.function);

            // while (i < 10
            nextToken(tokens, ref pos);
            result.condition = parseBinOp(tokens, ref pos, loopBodyScope);

            // while (i < 10)
            expectNext(tokens, ref pos, Token.TokenType.CloseBracket);

            // while (i < 10) { ... }
            nextToken(tokens, ref pos);
            result.loopBody = parseBlock(tokens, ref pos, scope, newScope: loopBodyScope);

            return result;
        }

        static Node parseForLoop(IList<Token> tokens, ref int pos, Scope scope)
        {

            // for
            var current = expectCurrent(tokens, pos, Token.TokenType.For);

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
            expectNext(tokens, ref pos, Token.TokenType.Semicolon);

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
            expectNext(tokens, ref pos, Token.TokenType.Semicolon);

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
            expectNext(tokens, ref pos, Token.TokenType.CloseBracket);

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
            var current = expectCurrent(tokens, pos, Token.TokenType.If);

            // if (
            expectNext(tokens, ref pos, Token.TokenType.OpenBracket);

            var result = new IfCondition(current);

            // if(i < 10
            nextToken(tokens, ref pos);
            result.condition = parseBinOp(tokens, ref pos, scope);

            // if(i < 10)
            expectNext(tokens, ref pos, Token.TokenType.CloseBracket);

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
            var current = expectCurrent(tokens, pos, Token.TokenType.Elif);

            // elif (
            expectNext(tokens, ref pos, Token.TokenType.OpenBracket);

            var result = new Elif(current);

            // elif(i < 10
            nextToken(tokens, ref pos);
            result.condition = parseBinOp(tokens, ref pos, scope);

            // elif(i < 10)
            expectNext(tokens, ref pos, Token.TokenType.CloseBracket);

            // elif(i < 10) {
            nextToken(tokens, ref pos);
            result.thenBlock = parseBlock(tokens, ref pos, scope);

            return result;
        }

        static Node parseReturn(IList<Token> tokens, ref int pos, Scope scope)
        {
            var current = expectCurrent(tokens, pos, Token.TokenType.Return);

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

        static Node parseUninitializedArray(IList<Token> tokens, ref int pos, Scope scope)
        {
            // var x = int32
            var current = expectCurrent(tokens, pos, Token.TokenType.Identifier);

            var result = new UninitializedArray(current);
            result.elementTypeName = current.text;

            // var x = int32[]
            expectNext(tokens, ref pos, Token.TokenType.ArrayTypeBrackets);

            // var x = int32[] { 
            expectNext(tokens, ref pos, Token.TokenType.OpenCurly);

            // var x = int32[] { 12
            var cnt = nextToken(tokens, ref pos);
            try
            {
                result.length = int.Parse(cnt.text);
            }
            catch
            {
                throw new ParserErrorExpected("compile time constant integer array length", cnt.text, cnt);
            }

            var cc = nextToken(tokens, ref pos);
            expectTokenType(cc, Token.TokenType.CloseCurly);

            return result;
        }

        static Node parseArray(IList<Token> tokens, ref int pos, Scope scope)
        {
            var current = expectCurrent(tokens, pos, Token.TokenType.OpenSquareBracket);
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
            var current = expectCurrent(tokens, pos, Token.TokenType.Var);
            var ident = expectNext(tokens, ref pos, Token.TokenType.Identifier);
            var assign = expectNext(tokens, ref pos, Token.TokenType.Assignment);
            var firstExpressionToken = nextToken(tokens, ref pos, skipWS: true);
            var result = new VariableDefinition(current);

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
            var next = peekTokenUpdatePos(tokens, ref nextIdx);

            // TODO: DO I REALLY NEED 2 LOOKAHEAD HERE?
            var nextNext = peekToken(tokens, nextIdx);

            // handle type cast operator (T)x
            if (current.type == Token.TokenType.OpenBracket
                && next.type == Token.TokenType.Identifier
                && nextNext.type == Token.TokenType.CloseBracket)
            {
                var typeNameToken = expectNext(tokens, ref pos, Token.TokenType.Identifier);
                expectNext(tokens, ref pos, Token.TokenType.CloseBracket);

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
                expectNext(tokens, ref pos, Token.TokenType.CloseBracket);
                return result;
            }
            if (current.type == Token.TokenType.Identifier)
            {
                var peek = peekToken(tokens, pos, skipWS: true);

                // function call
                if (peek.type == Token.TokenType.OpenBracket)
                {
                    var result = parseFunctionCall(tokens, ref pos, scope);
                    return result;
                }

                // struct field access
                if (peek.type == Token.TokenType.Dot)
                {
                    return parseStructFieldAccess(tokens, ref pos, scope);
                }

                // struct defintion
                if (peek.type == Token.TokenType.OpenCurly)
                {
                    return parseStructConstructor(tokens, ref pos, scope);
                }

                if (peek.type == Token.TokenType.ArrayTypeBrackets)
                {
                    return parseUninitializedArray(tokens, ref pos, scope);
                }

                return parseVariableLookup(tokens, ref pos, scope);
            }

            if (current.type == Token.TokenType.Increment || current.type == Token.TokenType.Decrement)
            {
                return parseVariableLookup(tokens, ref pos, scope);
            }
            throw new ParserError("Unexpected token type: " + current.type, current);
        }

        static Node parseStructConstructor(IList<Token> tokens, ref int pos, Scope scope)
        {

            // var x = vec3_i32
            var current = expectCurrent(tokens, pos, Token.TokenType.Identifier);

            var result = new StructConstructor(current);
            result.structName = current.text;

            // var x = vec3_i32 {
            var oc = nextToken(tokens, ref pos);
            expectTokenType(oc, Token.TokenType.OpenCurly);

            var next = peekToken(tokens, pos, skipWS: true);
            if (next.type != Token.TokenType.CloseCurly)
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

            expectNext(tokens, ref pos, Token.TokenType.CloseCurly);
            return result;
        }

        static Node parseStructFieldAccess(IList<Token> tokens, ref int pos, Scope scope)
        {
            var current = expectCurrent(tokens, pos, Token.TokenType.Identifier);
            expectNext(tokens, ref pos, Token.TokenType.Dot);

            var fieldName = nextToken(tokens, ref pos);
            expectTokenType(fieldName, Token.TokenType.Identifier);

            if (scope.GetVar(current.text) == null)
            {
                throw new UndefinedVariable(current.text, current);
            }
            var result = new StructFieldAccess(current);
            result.structName = current.text;
            result.fieldName = fieldName.text;

            // TODO: what happens if the field is an array? [ ] no idea
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

                expectNext(tokens, ref pos, Token.TokenType.CloseSquareBracket);
                return arrayElem;
            }
            return result;
        }

        static Node parseFunctionCall(IList<Token> tokens, ref int pos, Scope scope)
        {
            var current = expectCurrent(tokens, pos, Token.TokenType.Identifier);

            var result = new FunctionCall(current);
            result.functionName = current.text;

            if (scope.GetFunction(result.functionName) == null)
            {
                throw new ParserError(string.Format("Undefined function \"{0}\"", result.functionName), current);
            }

            expectNext(tokens, ref pos, Token.TokenType.OpenBracket);

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

            expectNext(tokens, ref pos, Token.TokenType.CloseBracket);
            return result;
        }

        public static Node parseBlock(IList<Token> tokens, ref int pos, Scope parentScope,
            Scope newScope = null)
        {
            var current = expectCurrent(tokens, pos, Token.TokenType.OpenCurly);

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

        public static Node parseStructDefinition(IList<Token> tokens, ref int pos, Scope scope)
        {
            // let
            var current = expectCurrent(tokens, pos, Token.TokenType.Let);

            // let foo
            var id = expectNext(tokens, ref pos, Token.TokenType.Identifier);

            // let foo = 
            expectNext(tokens, ref pos, Token.TokenType.Assignment);

            // let foo = struct
            expectNext(tokens, ref pos, Token.TokenType.Struct);

            // let foo = stuct { 
            expectNext(tokens, ref pos, Token.TokenType.OpenCurly);

            var result = new StructDefinition(current);
            result.type = new FrontendStructType();
            result.type.name = id.text;

            // add struct type to scope here to allow recursive structs
            scope.AddType(result.type.name, result.type, current);

            var next = peekToken(tokens, pos);
            while (next.type != Token.TokenType.CloseCurly)
            {
                // let foo = struct { x 
                var ident = expectNext(tokens, ref pos, Token.TokenType.Identifier);

                // let foo = struct { x: 
                expectNext(tokens, ref pos, Token.TokenType.Colon);

                // let foo = struct { x: int32
                var typ = expectNext(tokens, ref pos, Token.TokenType.Identifier);

                // TODO: what if type cannot be resolved here?
                // insert type proxy to be resolved later?
                result.type.AddField(ident.text, scope.GetType(typ.text));

                // let foo = struct { x: int32, ... 
                expectNext(tokens, ref pos, Token.TokenType.Semicolon);
                next = peekToken(tokens, pos);
            }

            nextToken(tokens, ref pos);
            return result;
        }

        public static Node parseFunctionDefinition(IList<Token> tokens, ref int pos, Scope scope)
        {

            // let
            var current = expectCurrent(tokens, pos, Token.TokenType.Let);

            if (scope.parent != null)
                throw new ParserError("functions can only be defined in the primary block for now!", current);

            var fun = new Scope.FunctionDefinition();

            // let foo
            var id = expectNext(tokens, ref pos, Token.TokenType.Identifier);
            fun.name = id.text;

            // let foo = 
            expectNext(tokens, ref pos, Token.TokenType.Assignment);

            // let foo = (
            expectNext(tokens, ref pos, Token.TokenType.OpenBracket);

            var next = peekToken(tokens, pos);
            if (next.type != Token.TokenType.CloseBracket)
            {
                while (true)
                {
                    // let foo = (x
                    var pid = expectNext(tokens, ref pos, Token.TokenType.Identifier);

                    // let foo = (x: 
                    expectNext(tokens, ref pos, Token.TokenType.Colon);

                    // let foo = (x: int32
                    var ptyp = expectNext(tokens, ref pos, Token.TokenType.Identifier);

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
            expectCurrent(tokens, pos, Token.TokenType.FatArrow);

            nextToken(tokens, ref pos);
            // let foo = (x: int32) => { ... }
            scope.AddFunction(fun);
            var result = new FunctionDefinition(current);
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
            return result;
        }
    }
}
