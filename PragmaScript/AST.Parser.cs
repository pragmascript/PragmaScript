﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace PragmaScript
{
    partial class AST
    {

        public struct ParseState
        {
            public Token[] tokens;
            public int pos;

            public Token CurrentToken()
            {
                return tokens[pos];
            }

            public Token PeekToken(bool tokenMustExist = false, bool skipWS = true)
            {
                int tempPos = pos;
                pos++;
                if (skipWS)
                {
                    SkipWhitespace();
                }

                if (pos >= tokens.Length)
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
                Token result = tokens[pos];
                pos = tempPos;
                return result;
            }


            public Token NextToken(bool skipWS = true, bool tokenMustExist = true)
            {
                pos++;
                if (skipWS)
                {
                    SkipWhitespace();
                }

                if (pos >= tokens.Length)
                {
                    if (tokenMustExist)
                    {
                        throw new ParserError("Missing next token", tokens[pos]);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return tokens[pos];
                }
            }

            public Token ExpectNextToken(Token.TokenType tt)
            {
                var t = NextToken();
                expectTokenType(t, tt);
                return t;
            }

            public Token ExpectPeekToken(Token.TokenType tt)
            {
                var t = PeekToken();
                expectTokenType(t, tt);
                return t;
            }

            public void SkipWhitespace(bool requireOneWS = false)
            {
                bool foundWS = false;

                while (pos < tokens.Length && (tokens[pos].type == Token.TokenType.WhiteSpace || tokens[pos].type == Token.TokenType.Comment))
                {
                    foundWS = true;
                    pos++;
                    if (pos >= tokens.Length)
                    {
                        break;
                    }
                }

                if (requireOneWS && !foundWS)
                {
                    throw new ParserError("Expected Whitespace", tokens[pos]);
                }
            }

            public Token ExpectCurrentToken(Token.TokenType tt)
            {
                var t = CurrentToken();
                expectTokenType(t, tt);
                return t;
            }

            public override string ToString()
            {
                return CurrentToken().ToString();
            }
        }

        static Node parseRootDeclarations(ref ParseState ps, Scope scope)
        {
            var result = default(Node);
            return result;
        }

        static Node parseStatement(ref ParseState ps, Scope scope)
        {
            var result = default(Node);

            var current = ps.CurrentToken();
            if (current.type == Token.TokenType.Var)
            {
                result = parseVar(ref ps, scope);
            }
            if (current.type == Token.TokenType.Return)
            {
                result = parseReturn(ref ps, scope);
            }
            if (current.type == Token.TokenType.Identifier)
            {
                var next = ps.PeekToken(tokenMustExist: true, skipWS: true);
                switch (next.type)
                {
                    case Token.TokenType.OpenBracket:
                        result = parseFunctionCall(ref ps, scope);
                        break;
                    case Token.TokenType.Increment:
                    case Token.TokenType.Decrement:
                        result = parseVariableReference(ref ps, scope);
                        break;
                    case Token.TokenType.Dot:
                    case Token.TokenType.OpenSquareBracket:
                        result = parseAssignment(ref ps, scope);
                        break;
                    default:
                        if (next.isAssignmentOperator())
                            result = parseAssignment(ref ps, scope);
                        else
                            throw new ParserErrorExpected("assignment operator, function call, or increment/decrement", next.type.ToString(), next);
                        break;
                }
            }

            bool ignoreSemicolon = false;
            if (current.type == Token.TokenType.If)
            {
                result = parseIf(ref ps, scope);
                ignoreSemicolon = true;
            }

            if (current.type == Token.TokenType.For)
            {
                result = parseForLoop(ref ps, scope);
                ignoreSemicolon = true;
            }

            if (current.type == Token.TokenType.While)
            {
                result = parseWhileLoop(ref ps, scope);
                ignoreSemicolon = true;
            }

            // TODO: make this LET thing work for variables as well
            if (current.type == Token.TokenType.Let)
            {
                result = parseLet(ref ps, scope, ref ignoreSemicolon);
            }
            // TODO: check if inside loo
            if (current.type == Token.TokenType.Continue)
            {
                result = new ContinueLoop(current, scope);
            }
            // TODO: check if inside loop
            if (current.type == Token.TokenType.Break)
            {
                result = new BreakLoop(current, scope);
            }

            if (!ignoreSemicolon)
            {
                ps.NextToken(skipWS: true);
                ps.ExpectCurrentToken(Token.TokenType.Semicolon);
            }

            if (result == null)
            {
                throw new ParserError(string.Format("Unexpected token type: \"{0}\"", current.type), current);
            }
            return result;
        }

        static Node parseLet(ref ParseState ps, Scope scope, ref bool ignoreSemicolon)
        {
            ps.ExpectCurrentToken(Token.TokenType.Let);

            Node result = null;
            var tempState = ps;

            // TODO: do i really need 3 lookahead?
            tempState.ExpectNextToken(Token.TokenType.Identifier);
            tempState.ExpectNextToken(Token.TokenType.Assignment);
            var next = tempState.NextToken(tokenMustExist: false);

            if (next.type == Token.TokenType.Struct)
            {
                result = parseStructDefinition(ref ps, scope);
                ignoreSemicolon = true;
            }
            else if (next.type == Token.TokenType.Fun)
            {
                result = parseFunctionDefinition(ref ps, scope);
                ignoreSemicolon = true;
                if ((result as FunctionDefinition).external)
                {
                    ignoreSemicolon = false;
                }
            }
            else
            {
                ignoreSemicolon = false;
                result = parseVariableDefinition(ref ps, scope);
            }
            return result;
        }

        static Node parseWhileLoop(ref ParseState ps, Scope scope)
        {
            // while
            var current = ps.ExpectCurrentToken(Token.TokenType.While);

            // while (
            ps.ExpectNextToken(Token.TokenType.OpenBracket);

            var result = new WhileLoop(current, scope);

            var loopBodyScope = new Scope(scope);

            // while (i < 10
            ps.NextToken();
            result.condition = parseBinOp(ref ps, loopBodyScope);

            // while (i < 10)
            ps.ExpectNextToken(Token.TokenType.CloseBracket);

            // while (i < 10) { ... }
            ps.NextToken();
            result.loopBody = parseBlock(ref ps, scope, newScope: loopBodyScope);

            return result;
        }

        static Node parseForLoop(ref ParseState ps, Scope scope)
        {

            // for
            var current = ps.ExpectCurrentToken(Token.TokenType.For);

            // for(
            var ob = ps.NextToken();
            expectTokenType(ob, Token.TokenType.OpenBracket);

            var result = new ForLoop(current, scope);
            var loopBodyScope = new Scope(scope);

            // for(int i = 0
            var next = ps.PeekToken();
            if (next.type != Token.TokenType.Semicolon)
            {
                ps.NextToken();
                result.initializer = parseForInitializer(ref ps, loopBodyScope);
            }
            else
            {
                result.initializer = new List<Node>();
            }

            // for(int i = 0;
            ps.ExpectNextToken(Token.TokenType.Semicolon);

            next = ps.PeekToken();
            if (next.type != Token.TokenType.Semicolon)
            {
                // for(int i = 0; i < 10
                ps.NextToken();
                result.condition = parseBinOp(ref ps, loopBodyScope);
            }
            else
            {
                result.condition = new ConstBool(next, loopBodyScope, true);
            }

            // for(int i = 0; i < 10;
            ps.ExpectNextToken(Token.TokenType.Semicolon);

            next = ps.PeekToken();
            if (next.type != Token.TokenType.CloseBracket)
            {
                // for(int i = 0; i < 10; i = i + 1
                ps.NextToken();
                result.iterator = parseForIterator(ref ps, loopBodyScope);
            }
            else
            {
                result.iterator = new List<Node>();
            }

            // for(int i = 0; i < 10; i = i + 1)
            ps.ExpectNextToken(Token.TokenType.CloseBracket);

            // for(int i = 0; i < 10; i = i + 1) { ... }
            ps.NextToken();
            result.loopBody = parseBlock(ref ps, scope, newScope: loopBodyScope);

            return result;
        }

        static List<Node> parseForInitializer(ref ParseState ps, Scope scope)
        {
            return parseForStatements(ref ps, scope, declaration: true);
        }

        static List<Node> parseForIterator(ref ParseState ps, Scope scope)
        {
            return parseForStatements(ref ps, scope, declaration: false);
        }

        static List<Node> parseForStatements(ref ParseState ps, Scope scope, bool declaration)
        {
            var current = ps.CurrentToken();
            var next = ps.PeekToken();
            var result = new List<Node>();

            while (true)
            {
                if (current.type == Token.TokenType.Var && declaration)
                {
                    var vd = parseVar(ref ps, scope);
                    result.Add(vd);
                }
                else if (current.type == Token.TokenType.Identifier)
                {
                    if (next.type == Token.TokenType.CloseBracket
                        || next.type == Token.TokenType.Increment || next.type == Token.TokenType.Decrement)
                    {
                        var variable = parseVariableReference(ref ps, scope);
                        result.Add(variable);
                    }
                    else if (next.type == Token.TokenType.OpenBracket)
                    {
                        ps.NextToken();
                        var call = parseFunctionCall(ref ps, scope);
                        result.Add(call); ;
                    }
                    else if (next.isAssignmentOperator())
                    {
                        var assignment = parseAssignment(ref ps, scope);
                        result.Add(assignment);
                    }
                    else
                    {
                        throw new ParserError("Invalid statement in for " + (declaration ? "initializer" : "iterator"), current);
                    }
                }
                else if (current.type == Token.TokenType.Increment || current.type == Token.TokenType.Decrement)
                {
                    var variable = parseVariableReference(ref ps, scope);
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

                next = ps.PeekToken();
                if (next.type != Token.TokenType.Comma)
                {
                    break;
                }
                else
                {
                    ps.NextToken();
                    current = ps.NextToken();
                }
            }
            return result;
        }

        static Node parseIf(ref ParseState ps, Scope scope)
        {
            // if
            var current = ps.ExpectCurrentToken(Token.TokenType.If);

            // if (
            ps.ExpectNextToken(Token.TokenType.OpenBracket);

            var result = new IfCondition(current, scope);

            // if(i < 10
            ps.NextToken();
            result.condition = parseBinOp(ref ps, scope);

            // if(i < 10)
            ps.ExpectNextToken(Token.TokenType.CloseBracket);

            // if(i < 10) {
            ps.NextToken();
            result.thenBlock = parseBlock(ref ps, scope);

            // if(i < 10) { ... } elif
            var next = ps.PeekToken();
            while (next.type == Token.TokenType.Elif)
            {
                ps.NextToken();
                var elif = parseElif(ref ps, scope);
                result.elifs.Add(elif);
                next = ps.PeekToken();
            }

            if (next.type == Token.TokenType.Else)
            {
                ps.NextToken();
                ps.NextToken();
                result.elseBlock = parseBlock(ref ps, scope);
            }

            return result;
        }

        static Node parseElif(ref ParseState ps, Scope scope)
        {
            // elif
            var current = ps.ExpectCurrentToken(Token.TokenType.Elif);

            // elif (
            ps.ExpectNextToken(Token.TokenType.OpenBracket);

            var result = new Elif(current, scope);

            // elif(i < 10
            ps.NextToken();
            result.condition = parseBinOp(ref ps, scope);

            // elif(i < 10)
            ps.ExpectNextToken(Token.TokenType.CloseBracket);

            // elif(i < 10) {
            ps.NextToken();
            result.thenBlock = parseBlock(ref ps, scope);

            return result;
        }

        static Node parseReturn(ref ParseState ps, Scope scope)
        {
            var current = ps.ExpectCurrentToken(Token.TokenType.Return);

            var next = ps.PeekToken(tokenMustExist: true, skipWS: true);
            if (next.type == Token.TokenType.Semicolon)
            {
                var result = new ReturnFunction(current, scope);
                return result;
            }
            else
            {
                var result = new ReturnFunction(current, scope);
                ps.NextToken();
                result.expression = parseBinOp(ref ps, scope);
                return result;
            }
        }

        static bool activatePointers_rec(Node node)
        {
            if (node is ICanReturnPointer)
            {
                (node as ICanReturnPointer).returnPointer = true;
                var c = node.GetChilds().FirstOrDefault();
                if (c != null)
                {
                    activatePointers_rec(c);
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        static Node parseAssignment(ref ParseState ps, Scope scope)
        {
            var current = ps.ExpectCurrentToken(Token.TokenType.Identifier);
            var result = new Assignment(current, scope);
            var temp_ps = ps;

            result.target = parseBinOp(ref ps, scope);

            if (!activatePointers_rec(result.target))
            {
                throw new ParserErrorExpected(result.target.GetType().Name, "variable, struct field access, array element access", result.target.token);
            }

            var assign = ps.NextToken();
            if (!assign.isAssignmentOperator())
            {
                throw new ParserErrorExpected("assignment operator", assign.type.ToString(), assign);
            }

            ps.NextToken();
            var exp = parseBinOp(ref ps, scope);

            var compound = new BinOp(assign, scope);
            compound.left = parseBinOp(ref temp_ps, scope);
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

        static Node parseUninitializedArray(ref ParseState ps, Scope scope)
        {

            throw new System.NotImplementedException();
            //// var x = int32
            //var current = ps.ExpectCurrentToken(Token.TokenType.Identifier);

            //var result = new UninitializedArray(current);
            //result.elementTypeName = current.text;

            //// var x = int32[]
            //ps.ExpectNextToken(Token.TokenType.ArrayTypeBrackets);

            //// var x = int32[] { 
            //ps.ExpectNextToken(Token.TokenType.OpenCurly);

            //// var x = int32[] { 12
            //var cnt = ps.NextToken();
            //try
            //{
            //    result.length = int.Parse(cnt.text);
            //}
            //catch
            //{
            //    throw new ParserErrorExpected("compile time constant integer array length", cnt.text, cnt);
            //}

            //var cc = ps.NextToken();
            //expectTokenType(cc, Token.TokenType.CloseCurly);

            //return result;
        }

        static Node parseArrayConstructor(ref ParseState ps, Scope scope)
        {
            var current = ps.ExpectCurrentToken(Token.TokenType.OpenSquareBracket);
            var next = ps.PeekToken();

            var result = new ArrayConstructor(current, scope);
            while (next.type != Token.TokenType.CloseSquareBracket)
            {
                current = ps.NextToken();
                var elem = parseBinOp(ref ps, scope);
                result.elements.Add(elem);
                next = ps.PeekToken();
                if (next.type == Token.TokenType.Comma)
                {
                    ps.NextToken();
                }
                else
                {
                    expectTokenType(next, Token.TokenType.CloseSquareBracket);
                }
            }

            ps.NextToken();
            return result;
        }

        static Node parseVar(ref ParseState ps, Scope scope)
        {
            var current = ps.ExpectCurrentToken(Token.TokenType.Var);
            return parseVariableDefinition(ref ps, scope);
        }

        static Node parseVariableDefinition(ref ParseState ps, Scope scope)
        {
            var current = ps.CurrentToken();
            expectTokenType(current, Token.TokenType.Let, Token.TokenType.Var);

            bool isConstant = current.type == Token.TokenType.Let;

            var result = new VariableDefinition(current, scope);

            var v = ps.ExpectNextToken(Token.TokenType.Identifier);
            var variableName = v.text;
            result.variable = scope.AddVar(variableName, result, v);
            result.variable.isConstant = isConstant;

            ps.ExpectNextToken(Token.TokenType.Assignment);
            ps.NextToken();

            result.expression = parseBinOp(ref ps, scope);

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
        private static Node parseBinOp(ref ParseState ps, Scope scope)
        {
            return parseBinOp(ref ps, scope, 11);
        }
        private static Node parseBinOp(ref ParseState ps, Scope scope, int precedence)
        {
            if (precedence == 1)
            {
                return parseUnary(ref ps, scope);
            }
            var result = parseBinOp(ref ps, scope, precedence - 1);
            var otherFactor = default(Node);
            var next = ps.PeekToken();

            while (isBinOp(next, precedence))
            {
                // continue to the next token after the add or subtract
                // TODO: tokens must exist?
                ps.NextToken();
                ps.NextToken();

                otherFactor = parseBinOp(ref ps, scope, precedence - 1);
                var bo = new BinOp(next, scope);
                bo.left = result;
                bo.right = otherFactor;
                bo.SetTypeFromToken(next);
                result = bo;
                next = ps.PeekToken();
            }
            return result;
        }

        // operator precedence 1
        private static Node parseUnary(ref ParseState ps, Scope scope)
        {
            var current = ps.CurrentToken();

            // handle unary plus and minus, ! and ~
            if (UnaryOp.IsUnaryToken(current))
            {
                var result = new UnaryOp(current, scope);
                result.SetTypeFromToken(current);
                ps.NextToken();

                result.expression = parsePrimary(ref ps, scope);
                if (result.type == UnaryOp.UnaryOpType.AddressOf)
                {
                    if (result.expression is ICanReturnPointer)
                    {
                        (result.expression as ICanReturnPointer).returnPointer = true;
                    }
                    else
                        throw new ParserError($"Cannot take address of expression \"{ result.expression }\"", ps.CurrentToken());
                }
                return result;
            }

            var tempState = ps;
            var next = tempState.NextToken(tokenMustExist: false);
            // TODO: DO I REALLY NEED 2 LOOKAHEAD HERE?
            var nextNext = tempState.PeekToken();
            // handle type cast operator (T)x
            if (current.type == Token.TokenType.OpenBracket
                && next.type == Token.TokenType.Identifier
                // && nextNext.type == Token.TokenType.CloseBracket
                )
            {
                ps.NextToken();
                var type = parseTypeString(ref ps, scope);
                ps.ExpectNextToken(Token.TokenType.CloseBracket);

                ps.NextToken();
                var exp = parsePrimary(ref ps, scope);

                var result = new TypeCastOp(current, scope);
                // TODO: check if valid type (in type check phase?)
                result.typeString = type;
                result.expression = exp;
                return result;
            }

            return parsePrimary(ref ps, scope);
        }

        // operator precedence 0
        private static Node parsePrimary(ref ParseState ps, Scope scope)
        {
            var current = ps.CurrentToken();

            switch (current.type)
            {
                case Token.TokenType.IntNumber:
                    {
                        var result = new ConstInt(current, scope);
                        bool isHex = current.text.Length > 1 && current.text[1] == 'x';
                        if (isHex)
                        {
                            result.number = int.Parse(current.text.Substring(2), NumberStyles.AllowHexSpecifier);
                        }
                        else
                        {
                            result.number = int.Parse(current.text);
                        }
                        
                        return result;
                    }
                case Token.TokenType.FloatNumber:
                    {
                        var result = new ConstFloat(current, scope);
                        result.number = double.Parse(current.text, CultureInfo.InvariantCulture);
                        return result;
                    }
                case Token.TokenType.False:
                case Token.TokenType.True:
                    {
                        var result = new ConstBool(current, scope);
                        result.value = current.type == Token.TokenType.True;
                        return result;
                    }
                case Token.TokenType.String:
                    {
                        var result = new ConstString(current, scope);
                        result.s = current.text;
                        return result;
                    }
                case Token.TokenType.OpenBracket:
                    {
                        var exprStart = ps.NextToken();
                        var result = parseBinOp(ref ps, scope);
                        ps.ExpectNextToken(Token.TokenType.CloseBracket);
                        return result;
                    }
                case Token.TokenType.Decrement:
                case Token.TokenType.Increment:
                    return parseVariableReference(ref ps, scope);

                case Token.TokenType.Identifier:
                    return parsePrimaryIdent(ref ps, scope);
                case Token.TokenType.OpenSquareBracket:
                    return parseArrayConstructor(ref ps, scope);
                default:
                    throw new ParserError("Unexpected token type: " + current.type, current);
            }
        }

        static Node parsePrimaryIdent(ref ParseState ps, Scope scope)
        {
            var current = ps.ExpectCurrentToken(Token.TokenType.Identifier);

            bool exit = false;
            Node result = null;
            while (!exit)
            {
                var peek = ps.PeekToken();
                Node next = null;
                switch (peek.type)
                {
                    case Token.TokenType.OpenBracket:
                        next = parseFunctionCall(ref ps, scope);
                        break;
                    case Token.TokenType.Dot:
                        if (result == null)
                        {
                            result = parseVariableReference(ref ps, scope, true);
                        }
                        else
                        {
                            if (result is ICanReturnPointer)
                            {
                                (result as ICanReturnPointer).returnPointer = true;
                            }
                        }
                        ps.NextToken();
                        next = parseStructFieldAccess(ref ps, scope, result, false);
                        break;
                    case Token.TokenType.OpenSquareBracket:
                        if (result == null)
                        {
                            result = parseVariableReference(ref ps, scope, true);
                        }
                        else
                        {
                            if (result is ICanReturnPointer)
                            {
                                (result as ICanReturnPointer).returnPointer = true;
                            }
                        }
                        ps.NextToken();
                        next = parseArrayElementAccess(ref ps, scope, result, false);
                        break;

                    case Token.TokenType.OpenCurly:
                        next = parseStructConstructor(ref ps, scope);
                        break;
                    case Token.TokenType.ArrayTypeBrackets:
                        next = parseUninitializedArray(ref ps, scope);
                        break;

                    default:
                        if (result == null)
                        {
                            result = parseVariableReference(ref ps, scope, false);
                        }
                        exit = true;
                        break;
                }
                if (next != null)
                {
                    result = next;
                }
            }
            Debug.Assert(result != null);
            return result;
        }

        static Node parseStructConstructor(ref ParseState ps, Scope scope)
        {

            // var x = vec3_i32
            var current = ps.ExpectCurrentToken(Token.TokenType.Identifier);

            var result = new StructConstructor(current, scope);

            result.structName = current.text;

            // var x = vec3_i32 {
            var oc = ps.NextToken();
            expectTokenType(oc, Token.TokenType.OpenCurly);

            var next = ps.PeekToken();
            if (next.type != Token.TokenType.CloseCurly)
            {
                while (true)
                {
                    ps.NextToken();
                    var exp = parseBinOp(ref ps, scope);
                    result.argumentList.Add(exp);
                    next = ps.PeekToken();
                    if (next.type != Token.TokenType.Comma)
                    {
                        break;
                    }
                    else
                    {
                        // skip comma
                        ps.NextToken();
                    }
                }
            }

            ps.ExpectNextToken(Token.TokenType.CloseCurly);
            return result;
        }

        
        static Node parseStructFieldAccess(ref ParseState ps, Scope scope, Node left, bool returnPointer = false)
        {
            var current = ps.ExpectCurrentToken(Token.TokenType.Dot);

            var fieldName = ps.NextToken();
            expectTokenType(fieldName, Token.TokenType.Identifier);

            var result = new StructFieldAccess(current, scope);
            result.returnPointer = returnPointer;
            result.left = left;
            result.fieldName = fieldName.text;

            // TODO: what happens if the field is an array? [ ] no idea
            return result;
        }


        // TODO: handle multi dimensional arrays
        static Node parseArrayElementAccess(ref ParseState ps, Scope scope, Node left, bool returnPointer = false)
        {
            var current = ps.ExpectCurrentToken(Token.TokenType.OpenSquareBracket);
            ps.NextToken();

            var result = new ArrayElementAccess(current, scope);
            result.left = left;
            result.returnPointer = returnPointer;
            result.index = parseBinOp(ref ps, scope);

            ps.ExpectNextToken(Token.TokenType.CloseSquareBracket);

            return result;
        }

        static Node parseVariableReference(ref ParseState ps, Scope scope, bool returnPointer = false)
        {
            var current = ps.CurrentToken();
            expectTokenType(current, Token.TokenType.Identifier, Token.TokenType.Increment, Token.TokenType.Decrement);

            VariableReference result = new VariableReference(current, scope);
            result.returnPointer = returnPointer;
            result.inc = VariableReference.Incrementor.None;

            if (current.type == Token.TokenType.Increment || current.type == Token.TokenType.Decrement)
            {
                result.inc = current.type == Token.TokenType.Increment ?
                    VariableReference.Incrementor.preIncrement : VariableReference.Incrementor.preDecrement;
                var id = ps.NextToken();
                expectTokenType(id, Token.TokenType.Identifier);
                result.token = id;
                current = id;
            }

            var vd = scope.GetVar(current.text);
            if (scope.GetVar(current.text) != null)
            {
                result.vd = vd;
            }
            else
            {
                result.variableName = current.text;
            }

            var peek = ps.PeekToken();
            if (peek.type == Token.TokenType.Increment || peek.type == Token.TokenType.Decrement)
            {
                if (result.inc != VariableReference.Incrementor.None)
                {
                    throw new ParserError("You can't use both pre-increment and post-increment", peek);
                }
                ps.NextToken();
                result.inc = peek.type == Token.TokenType.Increment ?
                    VariableReference.Incrementor.postIncrement : VariableReference.Incrementor.postDecrement;
            }
            return result;
        }

        static Node parseFunctionCall(ref ParseState ps, Scope scope)
        {
            var current = ps.ExpectCurrentToken(Token.TokenType.Identifier);

            var result = new FunctionCall(current, scope);
            result.functionName = current.text;


            // TODO: resolve this at type checking time
            //if (scope.GetFunction(result.functionName) == null)
            //{
            //    throw new ParserError(string.Format("Undefined function \"{0}\"", result.functionName), current);
            //}

            ps.ExpectNextToken(Token.TokenType.OpenBracket);

            var next = ps.PeekToken();
            if (next.type != Token.TokenType.CloseBracket)
            {
                while (true)
                {
                    ps.NextToken();
                    var exp = parseBinOp(ref ps, scope);
                    result.argumentList.Add(exp);
                    next = ps.PeekToken();
                    if (next.type != Token.TokenType.Comma)
                    {
                        break;
                    }
                    else
                    {
                        // skip comma
                        ps.NextToken();
                    }
                }
            }
            ps.ExpectNextToken(Token.TokenType.CloseBracket);
            return result;
        }

        public static Node parseBlock(ref ParseState ps, Scope parentScope,
            Scope newScope = null)
        {
            var current = ps.ExpectCurrentToken(Token.TokenType.OpenCurly);

            if (newScope == null)
            {
                newScope = new Scope(parentScope);
            }
            var result = new Block(current, newScope);

            var next = ps.PeekToken();

            bool foundReturn = false;
            while (next.type != Token.TokenType.CloseCurly)
            {
                ps.NextToken();
                var s = parseStatement(ref ps, result.scope);
                // ignore statements after the return so that return is the last statement in the block
                if (!foundReturn)
                {
                    result.statements.Add(s);
                }
                if (s is ReturnFunction)
                {
                    foundReturn = true;
                }
                next = ps.PeekToken();
                if (ps.pos >= ps.tokens.Length || next == null)
                {
                    throw new ParserError("No matching \"}\" found", current);
                }

            }
            ps.NextToken();
            return result;
        }

        public static Node parseRoot(ref ParseState ps, Scope scope)
        {
            var current = ps.CurrentToken();
            var result = new Root(current, scope);

            var next = current;

            while (next.type != Token.TokenType.EOF)
            {
                Node decl = null;
                current = ps.CurrentToken();
                if (current.type == Token.TokenType.Var)
                {
                    decl = parseVar(ref ps, scope);
                }

                bool ignoreSemicolon = false;

                
                if (current.type == Token.TokenType.Let)
                {
                    decl = parseLet(ref ps, scope, ref ignoreSemicolon);
                }
                if (!ignoreSemicolon)
                {
                    ps.NextToken(skipWS: true);
                    ps.ExpectCurrentToken(Token.TokenType.Semicolon);
                }
                if (decl == null)
                {
                    throw new ParserError(string.Format("Unexpected token type: \"{0}\"", current.type), current);
                }
                result.declarations.Add(decl);

                next = ps.NextToken();
            }
            return result;
        }

        public static Node parseStructDefinition(ref ParseState ps, Scope scope)
        {
            // let
            var current = ps.ExpectCurrentToken(Token.TokenType.Let);

            // let foo
            var id = ps.ExpectNextToken(Token.TokenType.Identifier);

            // let foo = 
            ps.ExpectNextToken(Token.TokenType.Assignment);

            // let foo = struct
            ps.ExpectNextToken(Token.TokenType.Struct);

            // let foo = stuct { 
            ps.ExpectNextToken(Token.TokenType.OpenCurly);

            var result = new StructDefinition(current, scope);
            result.name = id.text;


            // add struct type to scope here to allow recursive structs
            scope.AddType(result.name, result, current);

            var next = ps.PeekToken();
            while (next.type != Token.TokenType.CloseCurly)
            {
                // let foo = struct { x 
                var ident = ps.ExpectNextToken(Token.TokenType.Identifier);

                // let foo = struct { x: 
                ps.ExpectNextToken(Token.TokenType.Colon);

                // let foo = struct { x: int32
                ps.NextToken();
                var ts = parseTypeString(ref ps, scope);

                var f = new StructDefinition.StructField();
                f.name = ident.text;
                f.typeString = ts;
                result.fields.Add(f);

                // let foo = struct { x: int32, ... 
                ps.ExpectNextToken(Token.TokenType.Semicolon);
                next = ps.PeekToken();
            }

            ps.NextToken();
            return result;
        }

        public static TypeString parseTypeString(ref ParseState ps, Scope scope)
        {
            var current = ps.ExpectCurrentToken(Token.TokenType.Identifier);
            var next = ps.PeekToken();

            TypeString result = new TypeString(current, scope);

            result.typeString = current.text;
            if (next.type == Token.TokenType.ArrayTypeBrackets)
            {
                result.isArrayType = true;
                ps.NextToken();
            }
            else if (next.type == Token.TokenType.Multiply)
            {
                result.isPointerType = true;
                while (ps.PeekToken().type == Token.TokenType.Multiply)
                {
                    result.pointerLevel++;
                    ps.NextToken();
                }
            }
            return result;
        }

        public static Node parseFunctionDefinition(ref ParseState ps, Scope scope)
        {
            var current = ps.ExpectCurrentToken(Token.TokenType.Let);

            var result = new FunctionDefinition(current, scope);

            if (scope.parent != null)
                throw new ParserError("functions can only be defined in the primary block for now!", current);

            // let foo
            var id = ps.ExpectNextToken(Token.TokenType.Identifier);
            
            // let foo = 
            ps.ExpectNextToken(Token.TokenType.Assignment);

            // let foo = fun
            ps.ExpectNextToken(Token.TokenType.Fun);

            // let foo = fun (
            ps.ExpectNextToken(Token.TokenType.OpenBracket);

            var next = ps.PeekToken();
            if (next.type != Token.TokenType.CloseBracket)
            {
                while (true)
                {
                    // let foo = fun (x
                    var pid = ps.ExpectNextToken(Token.TokenType.Identifier);

                    // let foo = fun (x: 
                    ps.ExpectNextToken(Token.TokenType.Colon);

                    // let foo = fun (x: int32
                    var ptyp = ps.ExpectNextToken(Token.TokenType.Identifier);
                    var typeString = parseTypeString(ref ps, scope);
                    var p = new FunctionDefinition.FunctionParameter();
                    p.name = pid.text;
                    p.typeString = typeString;
                    result.parameters.Add(p);

                    // let foo = fun (x: int32
                    next = ps.NextToken();
                    if (next == null)
                    {
                        throw new ParserError("Missing \")\" in function definition", ptyp);
                    }
                    if (next.type != Token.TokenType.CloseBracket)
                    {
                        // let foo = fun (x: int32,
                        expectTokenType(next, Token.TokenType.Comma);
                        continue;
                    }
                    else
                    {
                        // let foo = fun (x: int32)
                        ps.NextToken();
                        break;
                    }
                }
            }
            else
            {
                ps.NextToken();
                ps.NextToken();
                // let foo = fun ( ) =>
            }

            // let foo = fun (x: int32) =>
            ps.ExpectCurrentToken(Token.TokenType.FatArrow);

            ps.NextToken();
            // let foo = fun (x: int32) => { ... }


            scope.AddVar(id.text, result, current, isConst: true);
            result.funName = id.text;
            result.external = false;
            var funScope = new Scope(scope);
            var idx = 0;
            foreach (var pd in result.parameters)
            {
                funScope.AddFunctionParameter(pd.name, result, idx);
                idx++;
            }
            
            if (ps.CurrentToken().type == Token.TokenType.Identifier)
            {
                var return_type = parseTypeString(ref ps, scope);

                result.returnType = return_type;
                var ppt = ps.PeekToken(true);
                if (ppt.type == Token.TokenType.Semicolon)
                {
                    result.body = null;
                    result.external = true;
                }
                else
                {
                    ps.NextToken();
                    result.body = parseBlock(ref ps, null, funScope);
                }
            }
            else
            {
                result.body = parseBlock(ref ps, null, funScope);
            }
            return result;
        }
    }
}
