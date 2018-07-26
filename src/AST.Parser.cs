﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace PragmaScript {
    partial class AST
    {
        public struct ParseState
        {
            public Token[] tokens;
            public int pos;

            public bool foundAttrib;
            public List<(string key, string value)> attribs;


            public Token CurrentToken()
            {
                return tokens[pos];
            }
 
            public ParseState Peek(bool tokenMustExist = false, bool skipWS = true)
            {
                int tempPos = pos;
                pos++;
                if (skipWS) {
                    SkipWhitespace();
                }

                if (pos >= tokens.Length) {
                    if (tokenMustExist) {
                        throw new ParserError("Missing next token", tokens[pos - 1]);
                    } else {
                        return this;
                    }
                }
                ParseState result = this;
                pos = tempPos;

                return result;
            }

            public Token PeekToken(bool tokenMustExist = false, bool skipWS = true)
            {
                int tempPos = pos;
                pos++;
                if (skipWS) {
                    SkipWhitespace();
                }

                if (pos >= tokens.Length) {
                    if (tokenMustExist) {
                        throw new ParserError("Missing next token", tokens[pos - 1]);
                    } else {
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
                if (skipWS) {
                    SkipWhitespace();
                }

                if (pos >= tokens.Length) {
                    if (tokenMustExist) {
                        throw new ParserError("Missing next token", tokens[pos]);
                    } else {
                        return null;
                    }
                } else {
                    return tokens[pos];
                }
            }

            public Token ExpectNextToken(Token.TokenType tt)
            {
                var t = NextToken();
                expectTokenType(t, tt);
                return t;
            }

            public Token ExpectNextToken(params Token.TokenType[] tts)
            {
                var t = NextToken();
                expectTokenType(t, tts);
                return t;
            }

            public Token ExpectPeekToken(Token.TokenType tt)
            {
                var t = PeekToken();
                expectTokenType(t, tt);
                return t;
            }
            public Token ExpectPeekToken(params Token.TokenType[] tts)
            {
                var t = PeekToken();
                expectTokenType(t, tts);
                return t;
            }

            public void SkipWhitespace(bool requireOneWS = false)
            {
                bool foundWS = false;

                while (pos < tokens.Length && (tokens[pos].type == Token.TokenType.WhiteSpace || tokens[pos].type == Token.TokenType.Comment)) {
                    foundWS = true;
                    pos++;
                    if (pos >= tokens.Length) {
                        break;
                    }
                }

                if (requireOneWS && !foundWS) {
                    throw new ParserError("Expected Whitespace", tokens[pos]);
                }
            }

            public Token ExpectCurrentToken(Token.TokenType tt)
            {
                var t = CurrentToken();
                expectTokenType(t, tt);
                return t;
            }

            public Token ExpectCurrentToken(params Token.TokenType[] tts)
            {
                var t = CurrentToken();
                expectTokenType(t, tts);
                return t;
            }

            public override string ToString()
            {
                return CurrentToken().ToString();
            }
        }


        public static List<string> parseImports(ref ParseState ps, Scope scope)
        {
            var result = new List<string>();
            ps.SkipWhitespace();
            while (true) {
                if (ps.CurrentToken().type == Token.TokenType.Import) {
                    var s = ps.ExpectNextToken(Token.TokenType.String);
                    result.Add(s.text.Substring(1, s.text.Length - 2));
                    ps.NextToken();
                } else {
                    break;
                }
            }
            return result;
        }

        public static Node parseModule(ref ParseState ps, Scope scope)
        {
            var current = ps.ExpectCurrentToken(Token.TokenType.Module);

            var ident = ps.ExpectNextToken(Token.TokenType.Identifier);
            var path = new List<string>();
            path.Add(ident.text);
            while (true) {
                if (ps.PeekToken().type == Token.TokenType.ModuleOp) {
                    ps.NextToken();
                    ident = ps.NextToken();
                    path.Add(ident.text);
                } else {
                    break;
                }
            }
            var ns = scope.AddModule(path);
            
            var result = new AST.Module(current, ns.scope);
            ns.scope.owner = result;

            ps.ExpectNextToken(Token.TokenType.OpenCurly);
            ps.NextToken();
            while (ps.CurrentToken().type != Token.TokenType.CloseCurly) {
                var decl = parseDeclaration(ref ps, ns.scope);
                result.declarations.Add(decl);
            }
            return result;
        }

        public static Node parseDeclaration(ref ParseState ps, Scope scope)
        {
            var current = ps.CurrentToken();
            Node result = null;
            bool ignoreSemicolon = false;
            bool foundWith = false;
            do {
                foundWith = false;
                switch (current.type) {
                    case Token.TokenType.Var:
                    case Token.TokenType.Let: {
                            result = parseLetVar(ref ps, scope, ref ignoreSemicolon);
                            if (ps.attribs?.Count > 0) {
                                foreach (var a in ps.attribs) {
                                    if (a.value != null) {
                                        result.AddAttribute(a.key, a.value);
                                    } else {
                                        result.AddAttribte(a.key);
                                    }
                                }
                                ps.attribs.Clear();
                                ps.foundAttrib = false;
                                if (result.HasAttribute("PACKED")) {
                                    if (result is StructDeclaration sd) {
                                        sd.packed = true;
                                    }
                                }
                                if (result.HasAttribute("COMPILE.ENTRY")) {
                                    if (result is FunctionDefinition) {
                                        if (CompilerOptions.entry != null) {
                                            throw new ParserError("Program entry point already defined!", result.token);
                                        }
                                        CompilerOptions.entry = result as FunctionDefinition;
                                    }
                                }
                            }

                        }
                        break;
                    case Token.TokenType.Module: {
                            result = parseModule(ref ps, scope);
                            ignoreSemicolon = true;
                        }
                        break;
                    case Token.TokenType.With: {
                            ps.ExpectCurrentToken(Token.TokenType.With);
                            var ident = ps.ExpectNextToken(Token.TokenType.Identifier);
                            var path = new List<string>();
                            path.Add(ident.text);
                            while (true) {
                                if (ps.PeekToken().type == Token.TokenType.Dot) {
                                    ps.NextToken();
                                    ident = ps.NextToken();
                                    path.Add(ident.text);
                                } else {
                                    break;
                                }
                            }
                            var ns = scope.AddModule(path, root: true);
                            scope.importedModules.Add(ns);
                            foundWith = true;
                            ps.ExpectNextToken(Token.TokenType.Semicolon);
                            current = ps.NextToken();
                        }
                        break;
                    case Token.TokenType.OpenSquareBracket: {
                            ps.NextToken();

                            while (true) {
                                var key = parsePrimary(ref ps, scope) as AST.ConstString;
                                if (key == null) {
                                    throw new ParserError("Expected string constant in attribute", key.token);
                                }

                                string a_value = null;
                                if (ps.PeekToken().type == Token.TokenType.Colon) {
                                    ps.ExpectNextToken(Token.TokenType.Colon);
                                    ps.NextToken();
                                    var value = parsePrimary(ref ps, scope) as AST.ConstString;
                                    if (value == null) {
                                        throw new ParserError("Expected string constant in attribute", key.token);
                                    }
                                    a_value = value.Vebatim();
                                }

                                var a_key = key.Vebatim().ToUpper();
                                if (ps.attribs == null) {
                                    ps.attribs = new List<(string key, string value)>();
                                }
                                ps.attribs.Add((a_key, a_value));

                                ps.ExpectNextToken(Token.TokenType.CloseSquareBracket, Token.TokenType.Comma);
                                if (ps.CurrentToken().type == Token.TokenType.CloseSquareBracket) {
                                    break;
                                }
                                ps.NextToken();
                            }
                            current = ps.NextToken();
                            ps.foundAttrib = true;
                        }
                        break;
                }
            } while (ps.foundAttrib || foundWith);
            if (result == null) {
                throw new ParserError(string.Format("Unexpected token type: \"{0}\"", current.type), current);
            }
            if (!ignoreSemicolon) {
                ps.ExpectNextToken(Token.TokenType.Semicolon);
            }
            ps.NextToken();

            return result;
        }

        public static Node parseFileRoot(ref ParseState ps, Scope scope)
        {
            ps.SkipWhitespace();
            parseImports(ref ps, scope);

            var current = ps.CurrentToken();
            var result = new FileRoot(current, scope);

            while (ps.CurrentToken().type != Token.TokenType.EOF) {
                var decl = parseDeclaration(ref ps, scope);
                result.declarations.Add(decl);
            }
            return result;
        }

        static Node parseStatement(ref ParseState ps, Scope scope, bool ignoreNextChar = false)
        {
            var result = default(Node);
            var current = ps.CurrentToken();
            var next = ps.PeekToken(tokenMustExist: true, skipWS: true);
            bool ignoreSemicolon = false;
            switch (current.type) {
                case Token.TokenType.Return:
                    result = parseReturn(ref ps, scope);
                    break;
                case Token.TokenType.If:
                    result = parseIf(ref ps, scope);
                    ignoreSemicolon = true;
                    break;
                case Token.TokenType.For:
                    result = parseForLoop(ref ps, scope);
                    ignoreSemicolon = true;
                    break;
                case Token.TokenType.While:
                    result = parseWhileLoop(ref ps, scope);
                    ignoreSemicolon = true;
                    break;
                case Token.TokenType.Var:
                case Token.TokenType.Let:
                    result = parseLetVar(ref ps, scope, ref ignoreSemicolon);
                    break;
                case Token.TokenType.Continue:
                    result = new ContinueLoop(current, scope);
                    break;
                case Token.TokenType.Break:
                    result = new BreakLoop(current, scope);
                    break;
                case Token.TokenType.OpenCurly:
                    result = parseBlock(ref ps, scope);
                    ignoreSemicolon = true;
                    break;
                default:
                    result = parseBinOp(ref ps, scope);
                    if (!(result is Assignment || result is FunctionCall
                        || UnaryOp.IsUnaryStatement(result))) {
                        throw new ParserErrorExpected("assignment operator, function call, or increment/decrement", next.type.ToString(), next);
                    }
                    break;
            }
            if (!ignoreSemicolon && !ignoreNextChar) {
                ps.NextToken(skipWS: true);
                ps.ExpectCurrentToken(Token.TokenType.Semicolon);
            }
            if (result == null) {
                throw new ParserError(string.Format("Unexpected token type: \"{0}\"", current.type), current);
            }
            return result;
        }

        static Node parseLetVar(ref ParseState ps, Scope scope, ref bool ignoreSemicolon)
        {
            var current = ps.ExpectCurrentToken(Token.TokenType.Let, Token.TokenType.Var);
            Node result = null;

            var tempState = ps;
            var ident = tempState.ExpectNextToken(Token.TokenType.Identifier);

            TypeString typeString = null;


            
            if (tempState.PeekToken().type == Token.TokenType.Colon) {
                tempState.NextToken();
                tempState.NextToken();

                // TODO(pragma): why are calling parseTypeString twice? Here and in parseVariableDefinition.
                typeString = parseTypeString(ref tempState, scope);
            }
            if (tempState.PeekToken().type == Token.TokenType.Assignment) {
                tempState.NextToken();
            }

            var next = tempState.NextToken();
            if (next.type == Token.TokenType.Extern) {
                next = tempState.NextToken();
                if (next.type == Token.TokenType.OpenBracket) {
                    next = tempState.NextToken();
                    var str = tempState.ExpectCurrentToken(Token.TokenType.String);
                    tempState.ExpectNextToken(Token.TokenType.CloseBracket);
                    next = tempState.NextToken();
                }
            }

            ignoreSemicolon = false;
            if (next.type == Token.TokenType.Struct) {
                result = parseStructDeclaration(ref ps, scope);
            } else if (next.type == Token.TokenType.Fun) {
                result = parseFunctionDefinition(ref ps, scope);
                if ((result as FunctionDefinition).body != null) {
                    ignoreSemicolon = true;
                }
            } else if (next.type == Token.TokenType.OpenCurly || next.type == Token.TokenType.Extern) {
                result = parseFunctionDefinition(ref ps, scope);
                if (typeString == null) {
                    throw new ParserError("Function definitions without \"fun\" keyword must supply a :TypeString prior to the assignment.", next);
                }
                ignoreSemicolon = true;
            } else {
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

            var loopBodyScope = new Scope(scope, scope.function);

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
            ps.ExpectNextToken(Token.TokenType.OpenBracket);

            var result = new ForLoop(current, scope);
            var loopBodyScope = new Scope(scope, scope.function);

            // for(int i = 0
            var next = ps.PeekToken();
            if (next.type != Token.TokenType.Semicolon) {
                ps.NextToken();
                result.initializer = parseForInitializer(ref ps, loopBodyScope);
            } else {
                result.initializer = new List<Node>();
            }

            // for(int i = 0;
            ps.ExpectNextToken(Token.TokenType.Semicolon);

            next = ps.PeekToken();
            if (next.type != Token.TokenType.Semicolon) {
                // for(int i = 0; i < 10
                ps.NextToken();
                result.condition = parseBinOp(ref ps, loopBodyScope);
            } else {
                result.condition = new ConstBool(next, loopBodyScope, true);
            }

            // for(int i = 0; i < 10;
            ps.ExpectNextToken(Token.TokenType.Semicolon);

            next = ps.PeekToken();
            if (next.type != Token.TokenType.CloseBracket) {
                // for(int i = 0; i < 10; i = i + 1
                ps.NextToken();
                result.iterator = parseForIterator(ref ps, loopBodyScope);
            } else {
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

            while (true) {

                if (current.type != Token.TokenType.Comma && current.type != Token.TokenType.Semicolon
                    && current.type != Token.TokenType.CloseBracket) {
                    var s = parseStatement(ref ps, scope, ignoreNextChar: true);
                    bool allowed = false;
                    allowed |= (s is Assignment);
                    allowed |= (s is VariableDefinition) & declaration;
                    allowed |= (s is FunctionCall);
                    allowed |= UnaryOp.IsUnaryStatement(s);
                    if (!allowed) {
                        throw new ParserError("Invalid statement in for " + (declaration ? "initializer" : "iterator"), current);
                    }
                    result.Add(s);
                } else {
                    if (declaration) {
                        expectTokenType(current, Token.TokenType.Comma, Token.TokenType.Semicolon);
                    } else {
                        expectTokenType(current, Token.TokenType.Comma, Token.TokenType.CloseBracket);
                    }
                }

                next = ps.PeekToken();
                if (next.type != Token.TokenType.Comma) {
                    break;
                } else {
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
            while (next.type == Token.TokenType.Elif) {
                ps.NextToken();
                var elif = parseElif(ref ps, scope);
                result.elifs.Add(elif);
                next = ps.PeekToken();
            }

            if (next.type == Token.TokenType.Else) {
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
            if (next.type == Token.TokenType.Semicolon) {
                var result = new ReturnFunction(current, scope);
                return result;
            } else {
                var result = new ReturnFunction(current, scope);
                ps.NextToken();
                result.expression = parseBinOp(ref ps, scope);
                return result;
            }
        }

        internal static bool activateReturnPointer(Node node)
        {
            if (node is ICanReturnPointer && (node as ICanReturnPointer).CanReturnPointer()) {
                (node as ICanReturnPointer).returnPointer = true;
                return true;
            } else {
                return false;
            }
        }

        static Node parseAssignment(ref ParseState ps, Scope scope, int precedence)
        {
            var left = parseBinOp(ref ps, scope, precedence - 1);
            var right = default(Node);
            var next = ps.PeekToken();

            List<Assignment> assignments = new List<Assignment>();

            while (next.isAssignmentOperator()) {
                // continue to the next token after the add or subtract
                ps.NextToken();
                ps.NextToken();
                right = parseBinOp(ref ps, scope, precedence - 1);

                Node result;
                var a = new Assignment(next, scope);
                if (left is Assignment) {
                    var old_a = left as Assignment;
                    var ro = old_a.right;
                    a.left = ro;
                    a.right = right;
                    old_a.right = a;
                    assignments.Add(a);
                    a = old_a;
                } else {
                    a.left = left;
                    a.right = right;
                    assignments.Add(a);
                }

                result = a;
                left = a;
                next = ps.PeekToken();
            }

            for (int i = 0; i < assignments.Count; ++i) {
                var a = assignments[i];

                if (a.token.type != Token.TokenType.Assignment) {
                    var left_copy = a.left.DeepCloneTree();
                    var compound = new BinOp(a.token, scope);
                    compound.left = left_copy;
                    compound.right = a.right;
                    a.right = compound;
                    switch (a.token.type) {
                        case Token.TokenType.PlusEquals:
                            compound.type = BinOp.BinOpType.Add;
                            break;
                        case Token.TokenType.LeftShiftEquals:
                            compound.type = BinOp.BinOpType.LeftShift;
                            break;
                        case Token.TokenType.RightShiftEquals:
                            compound.type = BinOp.BinOpType.RightShift;
                            break;
                        case Token.TokenType.RightShiftEqualsUnsigned:
                            compound.type = BinOp.BinOpType.RightShiftUnsigned;
                            break;
                        case Token.TokenType.XorEquals:
                            compound.type = BinOp.BinOpType.LogicalXOR;
                            break;
                        case Token.TokenType.OrEquals:
                            compound.type = BinOp.BinOpType.LogicalOR;
                            break;
                        case Token.TokenType.DivideEquals:
                            compound.type = BinOp.BinOpType.Divide;
                            break;
                        case Token.TokenType.DivideEqualsUnsigned:
                            compound.type = BinOp.BinOpType.DivideUnsigned;
                            break;
                        case Token.TokenType.AndEquals:
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
                    }
                }
                if (!activateReturnPointer(a.left)) {
                    if (a.left is VariableReference) {
                        throw new ParserError("Cannot assign to constant variable", a.left.token);
                    } else {
                        throw new ParserErrorExpected(a.left.GetType().Name, "cannot take address of left side for assignment", a.left.token);
                    }
                }
            }
            return left;
        }

        static Node parseUninitializedArray(ref ParseState ps, Scope scope)
        {
            throw new System.NotImplementedException();
        }

        static void arrayConstructorRec(ref ParseState ps, Scope scope, List<Node> elements, out List<int> dimensions) {
                dimensions = new List<int>();
            ps.ExpectCurrentToken(Token.TokenType.OpenSquareBracket);
            var current = ps.NextToken();

            if (current.type == Token.TokenType.OpenSquareBracket) {
                List<int> lastDims = null;
                int outerDim = 0;
                while (true) {
                    outerDim++;
                    arrayConstructorRec(ref ps, scope, elements, out var dims);
                      if (lastDims != null) {
                        if (!Enumerable.SequenceEqual(lastDims, dims)) {
                            throw new ParserError("Array constructor has non matching dimensions", ps.CurrentToken());
                        }
                    }
                    lastDims = dims;
                    current = ps.ExpectNextToken(Token.TokenType.Comma, Token.TokenType.CloseSquareBracket);
                    if (current.type == Token.TokenType.CloseSquareBracket) {
                        break;
                    } else {
                        ps.NextToken();
                    }
                }
                ps.ExpectCurrentToken(Token.TokenType.CloseSquareBracket);
                dimensions = new List<int>();
                dimensions.Add(outerDim);
                dimensions.AddRange(lastDims);
            } else {
                int count = 0;
                while (current.type != Token.TokenType.CloseSquareBracket) {
                    count++;
                    var elem = parseBinOp(ref ps, scope);
                    elements.Add(elem);
                    current = ps.ExpectNextToken(Token.TokenType.Comma, Token.TokenType.CloseSquareBracket);
                    if (current.type == Token.TokenType.Comma) {
                        ps.NextToken();
                    }
                }
                dimensions = new List<int>();
                dimensions.Add(count);
            }
        }


        static Node parseArrayConstructor(ref ParseState ps, Scope scope)
        {
            var current = ps.ExpectCurrentToken(Token.TokenType.OpenSquareBracket);
            var next = ps.PeekToken();
            var result = new ArrayConstructor(current, scope);
            arrayConstructorRec(ref ps, scope, result.elements, out result.dims);
            
            return result;
        }

        static Node parseVariableDefinition(ref ParseState ps, Scope scope)
        {
            var current = ps.ExpectCurrentToken(Token.TokenType.Let, Token.TokenType.Var);
            bool isConstant = current.type == Token.TokenType.Let;
            var result = new VariableDefinition(current, scope);
            var v = ps.ExpectNextToken(Token.TokenType.Identifier);
            var variableName = v.text;

            var at = ps.ExpectNextToken(Token.TokenType.Colon, Token.TokenType.Assignment);
            ps.NextToken();
            if (at.type == Token.TokenType.Colon) {
                result.typeString = parseTypeString(ref ps, scope);

                var peek = ps.ExpectPeekToken(Token.TokenType.Assignment, Token.TokenType.Semicolon);
                if (peek.type == Token.TokenType.Assignment) {
                    if (result.typeString.allocationCount > 0) {
                        throw new ParserError("Allocation typestring is implicitly initialized. Explicit initialization is invalid.", peek);
                    }
                    ps.NextToken();
                    ps.NextToken();
                    result.expression = parseBinOp(ref ps, scope);
                }
            } else {
                result.expression = parseBinOp(ref ps, scope);
            }

            result.variable = scope.AddVar(variableName, result, v);
            result.variable.isConstant = isConstant;
            Debug.Assert(result.expression != null || result.typeString != null);

            return result;
        }

        static bool isBinOp(Token t, int precedence)
        {
            var tt = t.type;
            switch (precedence) {
                case 2:
                    return tt == Token.TokenType.Multiply
                        || tt == Token.TokenType.Divide
                        || tt == Token.TokenType.DivideUnsigned
                        || tt == Token.TokenType.Remainder;
                case 3:
                    return tt == Token.TokenType.Add
                        || tt == Token.TokenType.Subtract;
                case 4:
                    return tt == Token.TokenType.LeftShift
                        || tt == Token.TokenType.RightShift
                        || tt == Token.TokenType.RightShiftUnsigned;
                case 5:
                    return tt == Token.TokenType.Less
                        || tt == Token.TokenType.Greater
                        || tt == Token.TokenType.LessEqual
                        || tt == Token.TokenType.GreaterEqual
                        || tt == Token.TokenType.LessUnsigned
                        || tt == Token.TokenType.GreaterUnsigned
                        || tt == Token.TokenType.LessEqualUnsigned
                        || tt == Token.TokenType.GreaterEqualUnsigned;
                case 6:
                    return tt == Token.TokenType.Equal
                        || tt == Token.TokenType.NotEquals;
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
                case 12:
                    // TODO: conditional ?: operator
                    return false;
                case 13:
                    return tt == Token.TokenType.Assignment
                        || tt == Token.TokenType.MultiplyEquals
                        || tt == Token.TokenType.DivideEquals
                        || tt == Token.TokenType.DivideEqualsUnsigned
                        || tt == Token.TokenType.RemainderEquals
                        || tt == Token.TokenType.PlusEquals
                        || tt == Token.TokenType.MinusEquals
                        || tt == Token.TokenType.LeftShiftEquals
                        || tt == Token.TokenType.RightShiftEquals
                        || tt == Token.TokenType.AndEquals
                        || tt == Token.TokenType.XorEquals
                        || tt == Token.TokenType.OrEquals;
                default:
                    throw new InvalidCodePath();

            }
        }
        private static Node parseBinOp(ref ParseState ps, Scope scope)
        {
            return parseBinOp(ref ps, scope, 13);
        }
        private static Node parseBinOp(ref ParseState ps, Scope scope, int precedence)
        {
            if (precedence == 13) {
                return parseAssignment(ref ps, scope, precedence);
            }
            if (precedence == 1) {
                return parseTypeOperator(ref ps, scope);
            }
            var left = parseBinOp(ref ps, scope, precedence - 1);
            var right = default(Node);
            var next = ps.PeekToken();

            while (isBinOp(next, precedence)) {
                // continue to the next token after the add or subtract
                // TODO: tokens must exist?
                ps.NextToken();
                ps.NextToken();

                right = parseBinOp(ref ps, scope, precedence - 1);
                var bo = new BinOp(next, scope);
                bo.left = left;
                bo.right = right;
                bo.SetTypeFromToken(next);
                left = bo;
                next = ps.PeekToken();
            }
            return left;
        }

        private static Node parseTypeOperator(ref ParseState ps, Scope scope) {
            
            var left = parseUnary(ref ps, scope);
            var next = ps.PeekToken();
            while (next.type == Token.TokenType.At || next.type == Token.TokenType.UnsignedCast) {
                ps.NextToken();
                ps.NextToken();
                var right = parseTypeString(ref ps, scope);
                var result = new TypeCastOp(next, scope);
                result.expression = left;
                result.typeString = right;
                result.unsigned = next.type == Token.TokenType.UnsignedCast;
                left = result;
                next = ps.PeekToken();
            }
            return left;
        }

        // operator precedence 1
        private static Node parseUnary(ref ParseState ps, Scope scope)
        {
            var current = ps.CurrentToken();

            if (current.type == Token.TokenType.At || current.type == Token.TokenType.UnsignedCast) {
                ps.NextToken();
                var ts = parseTypeString(ref ps, scope);
                ps.NextToken();
                var exp = parsePrimary(ref ps, scope);
                var result = new TypeCastOp(current, scope);
                result.typeString = ts;
                result.expression = exp;
                result.unsigned = current.type == Token.TokenType.UnsignedCast;
                return result;
            } 
            // handle unary plus and minus, ! and ~
            else if (UnaryOp.IsUnaryToken(current)) {
                var result = new UnaryOp(current, scope);
                result.SetTypeFromToken(current, prefix: true);

                if (result.type == UnaryOp.UnaryOpType.SizeOf) {
                    ps.ExpectNextToken(Token.TokenType.OpenBracket);
                    ps.NextToken();
                    result.expression = parseTypeString(ref ps, scope);
                    ps.ExpectNextToken(Token.TokenType.CloseBracket);
                } else {
                    ps.NextToken();
                    result.expression = parsePrimary(ref ps, scope);

                    if (result.type == UnaryOp.UnaryOpType.AddressOf ||
                        result.type == UnaryOp.UnaryOpType.PreInc ||
                        result.type == UnaryOp.UnaryOpType.PreDec) {
                        if (!activateReturnPointer(result.expression))
                            throw new ParserError($"Cannot take address of expression \"{ result.expression }\"", ps.CurrentToken());
                    }
                }
                return result;
            }
            return parsePrimary(ref ps, scope);
        }

        // operator precedence 0
        private static Node parsePrimary(ref ParseState ps, Scope scope)
        {
            var current = ps.CurrentToken();

            switch (current.type) {
                case Token.TokenType.IntNumber: {
                        var result = new ConstInt(current, scope);
                        bool isHex = current.text.Length > 1 && current.text[1] == 'x';
                        if (isHex) {
                            result.number = ulong.Parse(current.text.Substring(2), NumberStyles.AllowHexSpecifier);
                        } else {
                            result.number = (ulong)decimal.Parse(current.text);
                        }

                        return result;
                    }
                case Token.TokenType.FloatNumber: {
                        var result = new ConstFloat(current, scope);
                        result.number = double.Parse(current.text, CultureInfo.InvariantCulture);
                        return result;
                    }
                case Token.TokenType.False:
                case Token.TokenType.True: {
                        var result = new ConstBool(current, scope);
                        result.value = current.type == Token.TokenType.True;
                        return result;
                    }
                case Token.TokenType.String: {
                        var result = new ConstString(current, scope);
                        result.s = current.text;
                        return result;
                    }
                case Token.TokenType.OpenBracket: {
                        var exprStart = ps.NextToken();
                        var result = parseBinOp(ref ps, scope);
                        ps.ExpectNextToken(Token.TokenType.CloseBracket);
                        return result;
                    }
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

            // try to parse a struct constructor
            {
                var temp = ps;

                // TODO(pragma): this is ugly and a hack.
                bool legalTypeString = true;
                TypeString typeString = null;
                try {
                    typeString = parseTypeString(ref temp, scope);
                } catch {
                    legalTypeString = false;
                }

                // oki we found a struct constructor
                if (legalTypeString && temp.PeekToken().type == Token.TokenType.OpenCurly) {
                    ps = temp;
                    ps.NextToken();
                    result = new StructConstructor(current, scope);
                    (result as StructConstructor).typeString = typeString;

                    var next = ps.PeekToken();
                    if (next.type != Token.TokenType.CloseCurly) {
                        while (true) {
                            ps.NextToken();
                            var exp = parseBinOp(ref ps, scope);
                            (result as StructConstructor).argumentList.Add(exp);
                            next = ps.PeekToken();
                            if (next.type != Token.TokenType.Comma) {
                                break;
                            } else {
                                // skip comma
                                ps.NextToken();
                            }
                        }
                    }
                    ps.ExpectNextToken(Token.TokenType.CloseCurly);
                }
            }

            while (!exit) {
                var peek = ps.PeekToken();
                Node next = null;
                switch (peek.type) {
                    case Token.TokenType.OpenBracket:
                        if (result == null) {
                            result = parseVariableReference(ref ps, scope, true);
                        } else {
                            if (!activateReturnPointer(result)) {
                                throw new ParserError("cannot take address of lvalue", result.token);
                            }
                        }
                        ps.NextToken();
                        next = parseFunctionCall(ref ps, scope, result);
                        break;
                    case Token.TokenType.Dot:
                        if (result == null) {
                            result = parseVariableReference(ref ps, scope, true);
                        } else {
                            if (!activateReturnPointer(result)) {
                                throw new ParserError("cannot take address of lvalue", result.token);
                            }
                        }
                        ps.NextToken();
                        next = parseStructFieldAccess(ref ps, scope, result, false);
                        break;
                    case Token.TokenType.OpenSquareBracket:
                        var tempState = ps;
                        tempState.NextToken();
                        bool isSliceOp = false;
                        while (true) {
                            if (tempState.CurrentToken().type == Token.TokenType.Colon) {
                                isSliceOp = true;
                            }
                            if (tempState.PeekToken().type == Token.TokenType.CloseSquareBracket) {
                                break;
                            }
                            tempState.NextToken();
                        }
                        if (!isSliceOp) {
                            if (result == null) {
                                result = parseVariableReference(ref ps, scope, true);
                            } else {
                                if (!activateReturnPointer(result)) {
                                    throw new ParserError("cannot take address of lvalue", result.token);
                                }
                            }
                            ps.NextToken();
                            next = parseIndexedElementAccess(ref ps, scope, result, false);
                        } else {
                            if (result == null) {
                                result = parseVariableReference(ref ps, scope, false);
                            }
                            ps.NextToken();
                            next = parseSliceOp(ref ps, scope, result, false);
                        }
                        break;

                    // TODO(pragma): unitialized array handling:
                    // case Token.TokenType.ArrayTypeBrackets:
                    //     next = parseUninitializedArray(ref ps, scope);
                    //     break;
                    case Token.TokenType.Increment:
                    case Token.TokenType.Decrement:
                        if (result == null) {
                            result = parseVariableReference(ref ps, scope, true);
                        } else {
                            if (!activateReturnPointer(result)) {
                                throw new ParserError("cannot take address of lvalue", result.token);
                            }

                        }
                        ps.NextToken();
                        next = new UnaryOp(peek, scope);
                        (next as UnaryOp).expression = result;
                        (next as UnaryOp).SetTypeFromToken(peek, prefix: false);
                        break;
                    default:
                        if (result == null) {
                            result = parseVariableReference(ref ps, scope, false);
                        } else {
                            exit = true;
                        }
                        break;
                }
                if (next != null) {
                    result = next;
                }
            }
            Debug.Assert(result != null);
            return result;
        }

        static Node parseStructFieldAccess(ref ParseState ps, Scope scope, Node left, bool returnPointer = false)
        {
            var current = ps.ExpectCurrentToken(Token.TokenType.Dot);

            var fieldName = ps.NextToken();
            expectTokenType(fieldName, Token.TokenType.Identifier);

            var result = new FieldAccess(current, scope);
            result.returnPointer = returnPointer;
            result.left = left;
            result.fieldName = fieldName.text;

            // TODO: what happens if the field is an array? [ ] no idea
            return result;
        }


        static Node parseSliceOp(ref ParseState ps, Scope scope, Node left, bool returnPointer = false) {
            var current = ps.ExpectCurrentToken(Token.TokenType.OpenSquareBracket);
            ps.NextToken();

            var result = new SliceOp(current, scope);
            result.left = left;
            result.returnPointer = returnPointer;
            if (ps.CurrentToken().type != Token.TokenType.Colon) {
                result.from = parseBinOp(ref ps, scope);
                ps.NextToken();
            }
            ps.ExpectCurrentToken(Token.TokenType.Colon);
            ps.NextToken();
            if (ps.CurrentToken().type != Token.TokenType.CloseSquareBracket) {
                result.to = parseBinOp(ref ps, scope);
                ps.ExpectNextToken(Token.TokenType.CloseSquareBracket);
            }
            return result;


        }
        static Node parseIndexedElementAccess(ref ParseState ps, Scope scope, Node left, bool returnPointer = false)
        {
            var current = ps.ExpectCurrentToken(Token.TokenType.OpenSquareBracket);
            ps.NextToken();
            var indices = new List<Node>();
            var result = new IndexedElementAccess(current, scope);
            result.left = left;
            result.indices = indices;
            result.returnPointer = returnPointer;
            while (true) {
                indices.Add(parseBinOp(ref ps, scope));
                if (ps.PeekToken().type == Token.TokenType.CloseSquareBracket) {
                    break;
                }
                ps.ExpectNextToken(Token.TokenType.Comma);
                ps.NextToken();
            }
            ps.ExpectNextToken(Token.TokenType.CloseSquareBracket);
            return result;
        }

        static Node parseVariableReference(ref ParseState ps, Scope scope, bool returnPointer = false)
        {
            var current = ps.CurrentToken();
            expectTokenType(current, Token.TokenType.Identifier);

            
            VariableReference result = new VariableReference(current, scope);
            if (ps.PeekToken().type == Token.TokenType.ModuleOp) {
                var path = new List<string>();
                while (ps.PeekToken().type == Token.TokenType.ModuleOp) {
                    path.Add(current.text);
                    ps.NextToken();
                    current = ps.ExpectNextToken(Token.TokenType.Identifier);
                }
                result.modulePath = path;
            }
            
            result.returnPointer = returnPointer;
            result.variableName = current.text;

            return result;
        }

        static Node parseFunctionCall(ref ParseState ps, Scope scope, Node left)
        {
            var current = ps.ExpectCurrentToken(Token.TokenType.OpenBracket);

            var result = new FunctionCall(current, scope);
            result.left = left;

            var next = ps.PeekToken();
            if (next.type != Token.TokenType.CloseBracket) {
                while (true) {
                    ps.NextToken();
                    var exp = parseBinOp(ref ps, scope);
                    result.argumentList.Add(exp);
                    next = ps.PeekToken();
                    if (next.type != Token.TokenType.Comma) {
                        break;
                    } else {
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

            if (newScope == null) {
                newScope = new Scope(parentScope, parentScope.function);
            }
            var result = new Block(current, newScope);
            newScope.owner = result;

            var next = ps.PeekToken();

            bool foundReturn = false;
            while (next.type != Token.TokenType.CloseCurly) {
                ps.NextToken();
                var s = parseStatement(ref ps, result.scope);
                // ignore statements after the return so that return is the last statement in the block
                if (!foundReturn) {
                    result.statements.Add(s);
                }
                if (s is ReturnFunction) {
                    foundReturn = true;
                }
                next = ps.PeekToken();
                if (ps.pos >= ps.tokens.Length || next == null) {
                    throw new ParserError("No matching \"}\" found", current);
                }

            }
            ps.NextToken();
            return result;
        }

        public static Node parseStructDeclaration(ref ParseState ps, Scope scope)
        {
            // let
            var current = ps.ExpectCurrentToken(Token.TokenType.Let, Token.TokenType.Var);
            bool isVar = current.type == Token.TokenType.Var;
            bool isLet = !isVar;

            // let foo
            var id = ps.ExpectNextToken(Token.TokenType.Identifier);

            var result = new StructDeclaration(current, scope);
            result.name = id.text;

            // let foo = 
            ps.ExpectNextToken(Token.TokenType.Assignment);

            // let foo = struct
            ps.ExpectNextToken(Token.TokenType.Struct);

            result.fields = parseParamList(ref ps, scope);

            if (isLet) {
                // add struct type to scope here to allow recursive structs
                scope.AddType(result.name, result, current);
            } else {
                throw new System.NotImplementedException();
            }

            return result;
        }

        public static TypeString parseTypeString(ref ParseState ps, Scope scope)
        {
            var current = ps.ExpectCurrentToken(Token.TokenType.Identifier,
                                                Token.TokenType.Fun, Token.TokenType.Struct);

            TypeString result = new TypeString(current, scope);
            if (current.type == Token.TokenType.Fun) {
                result.kind = TypeString.TypeKind.Function;
                result.functionTypeString = new TypeString.FunctionTypeString();
                var fts = result.functionTypeString;
                // let foo = fun 
                fts.parameters = parseParamList(ref ps, scope);

                // let foo = fun (x: int32) =>
                ps.ExpectNextToken(Token.TokenType.FatArrow);
                ps.NextToken();

                // let foo = fun (x: int32) => void 
                var return_type = parseTypeString(ref ps, scope);
                fts.returnType = return_type;
                return result;
            } else if (current.type == Token.TokenType.Struct) {
                result.kind = TypeString.TypeKind.Struct;
                throw new System.NotImplementedException();
            } else {
                result.kind = TypeString.TypeKind.Other;
                result.fullyQualifiedName.path.Add(current.text);

                var next = ps.PeekToken();
                while (next.type == Token.TokenType.ModuleOp) {
                    ps.NextToken();
                    var ident = ps.ExpectNextToken(Token.TokenType.Identifier);
                    result.fullyQualifiedName.path.Add(ident.text);
                    next = ps.PeekToken();
                }
                if (next.type == Token.TokenType.SliceBrackets) {
                    result.isSliceType = true;
                    ps.NextToken();
                }
                if (next.type == Token.TokenType.OpenSquareBracket) {
                    result.isArrayType = true;
                    result.arrayDims = new List<int>();
                    ps.NextToken();
                    while (true) {
                        var length = ps.ExpectNextToken(Token.TokenType.IntNumber);
                        result.arrayDims.Add(int.Parse(length.text));
                        if (ps.PeekToken().type == Token.TokenType.CloseSquareBracket) {
                            break;
                        } else {
                            ps.ExpectNextToken(Token.TokenType.Comma);
                        }
                    }
                    ps.ExpectNextToken(Token.TokenType.CloseSquareBracket);

                } else if (next.type == Token.TokenType.Multiply) {
                    result.isPointerType = true;
                    while (ps.PeekToken().type == Token.TokenType.Multiply) {
                        result.pointerLevel++;
                        ps.NextToken();
                    }
                    // if (ps.PeekToken().type != Token.TokenType.CloseBracket
                    //     && ps.PeekToken().type != Token.TokenType.Semicolon
                    //     && ps.PeekToken().type != Token.TokenType.OpenCurly
                    //     && ps.PeekToken().type != Token.TokenType.Assignment) {
                    //     ps.NextToken();
                    //     int alloc = 0;
                    //     if (ps.CurrentToken().type == Token.TokenType.IntNumber) {
                    //         alloc = int.Parse(ps.CurrentToken().text);
                    //     }
                    //     result.allocationCount = alloc;
                    // }
                }
                return result;
            }
        }

        public static List<NamedParameter> parseParamList(ref ParseState ps, Scope scope)
        {
            var result = new List<NamedParameter>();
            // let foo = stuct ( 
            ps.ExpectNextToken(Token.TokenType.OpenBracket);

            var next = ps.PeekToken();
            bool firstOptionalParameter = false;
            while (next.type != Token.TokenType.CloseBracket) {
                var p = new AST.NamedParameter();
                // let foo = struct ( x 
                var at = ps.ExpectNextToken(Token.TokenType.Identifier, Token.TokenType.At);

                if (next.type == Token.TokenType.At) {
                    p.embed = true;
                    ps.ExpectNextToken(Token.TokenType.Identifier);
                }
                var ident = ps.CurrentToken();


                // let foo = struct ( x: 
                ps.ExpectNextToken(Token.TokenType.Colon);

                // let foo = struct ( x: int32
                ps.NextToken();
                var ts = parseTypeString(ref ps, scope);


                p.name = ident.text;
                p.typeString = ts;
                result.Add(p);

                if (ps.PeekToken().type == Token.TokenType.Assignment) {
                    firstOptionalParameter = true;
                    ps.NextToken();
                    ps.NextToken();
                    var exp = parseBinOp(ref ps, scope);
                    p.defaultValueExpression = exp;
                } else {
                    if (firstOptionalParameter) {
                        throw new ParserError("Required parameter after optional parameters is not allowed.", ident);
                    }
                }

                // let foo = struct ( x: int32; ... 
                if (ps.PeekToken().type == Token.TokenType.CloseBracket) {
                    next = ps.PeekToken();
                } else {
                    next = ps.ExpectNextToken(Token.TokenType.Semicolon);
                    next = ps.PeekToken();
                }
            }
            ps.NextToken();

            return result;
        }

        public static Node parseFunctionDefinition(ref ParseState ps, Scope scope)
        {
            var current = ps.ExpectCurrentToken(Token.TokenType.Let, Token.TokenType.Var);
            bool isVar = current.type == Token.TokenType.Var;
            bool isLet = !isVar;

            var result = new FunctionDefinition(current, scope);

            if (scope.function != null)
                throw new ParserError("nested functions not supported yet.", current);

            // let foo
            var id = ps.ExpectNextToken(Token.TokenType.Identifier);

            // let foo = 
            var next = ps.ExpectNextToken(Token.TokenType.Assignment, Token.TokenType.Colon);

            TypeString typeString = null;
            if (next.type == Token.TokenType.Colon) {
                ps.NextToken();
                typeString = parseTypeString(ref ps, scope);
            }
            result.typeString = typeString;

            // let foo = [extern]
            if (ps.PeekToken().type == Token.TokenType.Extern) {
                if (isVar) {
                    throw new ParserError("extern keyword not allowed on function variables", ps.PeekToken());
                }
                result.external = true;
                ps.NextToken();
                if (ps.PeekToken().type == Token.TokenType.OpenBracket) {
                    ps.NextToken();
                    var str = ps.ExpectNextToken(Token.TokenType.String);
                    result.externalFunctionName = str.text.Substring(1, str.text.Length - 2);
                    ps.ExpectNextToken(Token.TokenType.CloseBracket);
                }
            } else {
                result.external = false;
            }

            ps.NextToken();

            if (result.typeString == null) {
                result.typeString = parseTypeString(ref ps, scope);
                Debug.Assert(result.typeString.kind == TypeString.TypeKind.Function);
            }

            result.funName = id.text;
            var funScope = new Scope(scope, result);

            if (!result.external) {
                if (ps.PeekToken().type != Token.TokenType.Semicolon) {
                    if (result.external) {
                        throw new ParserError("External functions can't have a body", ps.CurrentToken());
                    }
                    ps.NextToken();
                    result.body = parseBlock(ref ps, null, funScope);
                } else {
                    result.body = null;
                }
            }

            if (result.external || result.body != null) {
                var vd = scope.AddVar(id.text, result, current, isConst: true, allowOverloading: true);
                result.variableDefinition = vd;
            } else {
                if (isVar) {
                    throw new ParserError("variable type declarations are not allowed", current);
                }
                scope.AddType(id.text, result, current);
            }
            return result;
        }
    }
}