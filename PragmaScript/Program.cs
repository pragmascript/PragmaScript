using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LLVMSharp;
using System.Runtime.InteropServices;

using Shields.GraphViz.Models;
using System.Threading;

namespace PragmaScript
{
    internal static class AsyncHelper
    {
        private static readonly TaskFactory _myTaskFactory = new
          TaskFactory(CancellationToken.None,
                      TaskCreationOptions.None,
                      TaskContinuationOptions.None,
                      TaskScheduler.Default);

        public static TResult RunSync<TResult>(Func<Task<TResult>> func)
        {
            return AsyncHelper._myTaskFactory
              .StartNew<Task<TResult>>(func)
              .Unwrap<TResult>()
              .GetAwaiter()
              .GetResult();
        }

        public static void RunSync(Func<Task> func)
        {
            AsyncHelper._myTaskFactory
              .StartNew<Task>(func)
              .Unwrap()
              .GetAwaiter()
              .GetResult();
        }
    }

    class Token
    {
        public enum TokenType
        {
            WhiteSpace, Let, Var, Fun, Identifier, OpenBracket, CloseBracket, Number, Assignment, Error, Add, Subtract, Multiply, Divide, Semicolon
        }

        public TokenType type { get; private set; }
        public string text { get; private set; }
        public string errorMessage { get; private set; }
        public int lineNumber { get; private set; }
        public int pos { get; private set; }
        public int length { get; private set; }

        static Dictionary<string, TokenType> keywords;
        static Dictionary<string, TokenType> operators;


        static Token()
        {
            keywords = new Dictionary<string, TokenType>();
            keywords.Add("let", TokenType.Let);
            keywords.Add("var", TokenType.Var);

            operators = new Dictionary<string, TokenType>();
            operators.Add("=", TokenType.Assignment);
            operators.Add("(", TokenType.OpenBracket);
            operators.Add(")", TokenType.CloseBracket);
            operators.Add("+", TokenType.Add);
            operators.Add("-", TokenType.Subtract);
            operators.Add("*", TokenType.Multiply);
            operators.Add("/", TokenType.Divide);
            operators.Add(";", TokenType.Semicolon);
        }

        public static bool isIdentifierChar(char c)
        {
            return char.IsLetter(c) || c == '_';
        }

        public static bool isKeyword(string identifier)
        {
            return keywords.ContainsKey(identifier);
        }

        public static bool isOperator(char c)
        {
            return operators.ContainsKey(c.ToString());
        }

        public static Token EmitError(Token t, string errorMessage)
        {
            t.errorMessage = errorMessage;
            t.type = TokenType.Error;
            return t;
        }
        public static Token Parse(string line, int pos, int lineNumber)
        {
            var t = new Token();
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
                        return EmitError(t, "Only one decimal seperator is allowed!");
                    }
                    containsDecimalSeperator = current == '.';

                    t.length++;
                    pos++;
                    if (pos >= line.Length)
                        break;
                    current = line[pos];
                }
                t.type = TokenType.Number;
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

            // check if current char is operator
            if (isOperator(current))
            {
                // TOOD: support combined operators of multipel chars ( e.g. +=, == )
                t.length = 1;
                t.type = operators[current.ToString()];
                t.text = line.Substring(t.pos, t.length);
                return t;
            }

            t.length = 1;
            t.text = line.Substring(t.pos, t.length);
            return EmitError(t, "Syntax error!");
        }

        public override string ToString()
        {
            if (type != TokenType.Error)
            {
                return string.Format("({0}, {1}, {2}, \"{3}\")", type.ToString(), lineNumber, pos, text);
            }
            else
            {
                return string.Format("({0}, {1}, {2}, \"{3}\")", "error: " + errorMessage, lineNumber, pos, text);
            }

        }
    }


    class InvalidCodePath : Exception
    {
        public InvalidCodePath()
            : base("Program should never get here!")
        { }
    }

    class CompilerError : Exception
    {
        public CompilerError(string message, Token t)
            : base(String.Format("error: {0}! at {1}", message, t))
        {
        }
    }

    class CompilerErrorExpected : CompilerError
    {
        public CompilerErrorExpected(string expected, string got, Token t)
            : base(string.Format("expected \"{0}\", but got \"{1}\""), t)
        {

        }
    }

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
            expectTokenType(current, Token.TokenType.Assignment);

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


            // is operator precedence 2
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
            var current = tokens[pos];
            expectTokenType(current, Token.TokenType.Number);

            var result = new ConstInt();
            result.number = int.Parse(current.text);
            return result;
        }

        private static Node parseFactor(IList<Token> tokens, ref int pos)
        {
            throw new NotImplementedException();
        }

        static void expectTokenType(Token t, Token.TokenType type)
        {
            if (t.type != type)
                throw new CompilerErrorExpected(type.ToString(), t.ToString(), t);
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
            throw new NotImplementedException();
        }

        static Token peekToken(IList<Token> tokens, int pos, bool tokenMustExist = false, bool skipWS = false)
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

        static Token nextToken(IList<Token> tokens, ref int pos, bool skipWS = false)
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

    class Backend
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int Add(int a, int b);
        public static void Test()
        {

            LLVMBool False = new LLVMBool(0);
            LLVMModuleRef mod = LLVM.ModuleCreateWithName("LLVMSharpIntro");

            LLVMTypeRef[] param_types = { LLVM.Int32Type(), LLVM.Int32Type() };
            LLVMTypeRef ret_type = LLVM.FunctionType(LLVM.Int32Type(), out param_types[0], 2, False);
            LLVMValueRef sum = LLVM.AddFunction(mod, "sum", ret_type);

            LLVMBasicBlockRef entry = LLVM.AppendBasicBlock(sum, "entry");

            LLVMBuilderRef builder = LLVM.CreateBuilder();
            LLVM.PositionBuilderAtEnd(builder, entry);
            LLVMValueRef tmp = LLVM.BuildAdd(builder, LLVM.GetParam(sum, 0), LLVM.GetParam(sum, 1), "tmp");
            LLVM.BuildRet(builder, tmp);

            IntPtr error;
            LLVM.VerifyModule(mod, LLVMVerifierFailureAction.LLVMAbortProcessAction, out error);
            LLVM.DisposeMessage(error);

            LLVMExecutionEngineRef engine;

            LLVM.LinkInMCJIT();
            LLVM.InitializeX86Target();
            LLVM.InitializeX86TargetInfo();
            LLVM.InitializeX86TargetMC();
            LLVM.InitializeX86AsmPrinter();

            var platform = Environment.OSVersion.Platform;
            if (platform == PlatformID.Win32NT) // On Windows, LLVM currently (3.6) does not support PE/COFF
            {
                LLVM.SetTarget(mod, Marshal.PtrToStringAnsi(LLVM.GetDefaultTargetTriple()) + "-elf");
            }

            var options = new LLVMMCJITCompilerOptions();
            var optionsSize = (4 * sizeof(int)) + IntPtr.Size; // LLVMMCJITCompilerOptions has 4 ints and a pointer

            LLVM.InitializeMCJITCompilerOptions(out options, optionsSize);
            LLVM.CreateMCJITCompilerForModule(out engine, mod, out options, optionsSize, out error);

            var addMethod = (Add)Marshal.GetDelegateForFunctionPointer(LLVM.GetPointerToGlobal(engine, sum), typeof(Add));
            int result = addMethod(10, 15);

            Console.WriteLine("Result of sum is: " + result);

            if (LLVM.WriteBitcodeToFile(mod, "sum.bc") != 0)
            {
                Console.WriteLine("error writing bitcode to file, skipping");
            }

            LLVM.DumpModule(mod);

            LLVM.DisposeBuilder(builder);
            LLVM.DisposeExecutionEngine(engine);
            Console.ReadKey();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Backend.Test();
            // parse("var x = 12;");
            parse("var y = 1 + 2 - 4 + 6;");
            Console.ReadLine();
        }


        static IEnumerable<Token> tokenize(string text)
        {
            List<Token> result = new List<Token>();

            var lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            for (int i = 0; i < lines.Length; ++i)
            {
                var line = lines[i];
                var pos = 0;
                while (pos < line.Length)
                {
                    var t = Token.Parse(line, pos, i);
                    yield return t;

                    // abort on first error token
                    if (t.type == Token.TokenType.Error)
                    {
                        yield break;
                    }
                    pos += t.length;
                }
            }
        }



        static Graph getRenderGraph(Graph g, AST.Node node, string id)
        {
            var ns = NodeStatement.For(id);
            ns = ns.Set("label", node.ToString());
            var result = g.Add(ns);
            foreach (var c in node.GetChilds())
            {
                var cid = Guid.NewGuid().ToString();
                result = getRenderGraph(result, c, cid);
                result = result.Add(EdgeStatement.For(id, cid));
            }
            return result;
        }

        static void renderGraph(AST.Node root)
        {
            var g = getRenderGraph(Graph.Undirected, root, Guid.NewGuid().ToString());
            var renderer = new Shields.GraphViz.Components.Renderer(@"C:\Program Files (x86)\Graphviz2.38\bin\");
            using (Stream file = File.Create("graph.png"))
            {
                AsyncHelper.RunSync(() =>
                    renderer.RunAsync(g, file, Shields.GraphViz.Services.RendererLayouts.Dot, Shields.GraphViz.Services.RendererFormats.Png, CancellationToken.None)
                 );
            }
        }

        static void parse(string text)
        {
            var tokens = tokenize(text).ToList();
            Console.WriteLine("input: " + text);
            Console.WriteLine();
            Console.WriteLine("tokens: ");
            int pos = 0;
            foreach (var t in tokens)
            {
                if (t.type != Token.TokenType.WhiteSpace)
                    Console.WriteLine("{0}: {1}", pos++, t);
            }

            var root = AST.Parse(tokens);
            renderGraph(root);
            Console.WriteLine(root);

        }
    }
}
