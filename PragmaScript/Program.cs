using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Shields.GraphViz.Models;
using System.Threading;
using System.Text;
using static PragmaScript.AST;
using System.Diagnostics;

namespace PragmaScript
{

    static class CompilerOptions
    {
        public static bool debug = false;
        public static string inputFilename;
        public static int optimizationLevel;
        public static bool runAfterCompile;
        public static bool asm = false;
    }

    // http://llvm.lyngvig.org/Articles/Mapping-High-Level-Constructs-to-LLVM-IR
    class Program
    {
        static Dictionary<string, bool> files = new Dictionary<string, bool>();
        static void Main(string[] args)
        {
            parseARGS(args);

#if DEBUG
            CompilerOptions.debug = true;
            CompilerOptions.optimizationLevel = 0;
            CompilerOptions.runAfterCompile = true;
            // CompilerOptions.inputFilename = @"D:\Projects\Dotnet\PragmaScript\PragmaScript\Programs\handmade.prag";
            CompilerOptions.inputFilename    = @"D:\Projects\Dotnet\PragmaScript\PragmaScript\Programs\bugs.prag";
#endif
            if (CompilerOptions.inputFilename == null)
            {
                Console.WriteLine("Input file name missing!");
                return;
            }
            try
            {
                compile(CompilerOptions.inputFilename);
            }
            catch (FileNotFoundException)
            {
                writeError("Could not open input file!");
            }

        }

        static void writeError(string s)
        {
            Console.WriteLine(s + "!");
        }

        static void printHelp()
        {
            Console.WriteLine();
            Console.WriteLine("command line options: ");
            Console.WriteLine("-D: build in debug mode");
            Console.WriteLine("-O0: turn off optimizations");
            Console.WriteLine("-OX: turn on optimization level X in [1..3]");
            Console.WriteLine("-R: run program after compilation");
            Console.WriteLine("-ASM: output file with generated assembly");
        }
        static void parseARGS(string[] args)
        {
            foreach (var arg in args)
            {
                if (arg.TrimStart().StartsWith("-"))
                {
                    var x = arg.TrimStart().Remove(0, 1);
                    x = x.ToUpperInvariant();
                    switch (x)
                    {
                        case "D":
                        case "DEGUG":
                            CompilerOptions.debug = true;
                            break;
                        case "ASM":
                            CompilerOptions.asm = true;
                            break;
                        case "O0":
                            CompilerOptions.optimizationLevel = 0;
                            break;
                        case "O1":
                            CompilerOptions.optimizationLevel = 1;
                            break;
                        case "O2":
                            CompilerOptions.optimizationLevel = 2;
                            break;
                        case "O3":
                            CompilerOptions.optimizationLevel = 3;
                            break;
                        case "HELP":
                        case "-HELP":
                        case "H":
                            printHelp();
                            break;
                        case "R":
                        case "RUN":
                            CompilerOptions.runAfterCompile = true;
                            break;
                        default:
                            writeError("Unknown command line option");
                            break;
                    }
                }
                else
                {
                    CompilerOptions.inputFilename = arg;
                }
            }
        }


#if DEBUG
        static Graph getRenderGraph(Graph g, AST.Node node, string id)
        {
            var ns = NodeStatement.For(id);
            ns = ns.Set("label", node.ToString().Replace(@"\", @"\\")).Set("font", "Consolas");
            ns = ns.Set("shape", "box");
            var result = g.Add(ns);
            foreach (var c in node.GetChilds())
            {
                var cid = Guid.NewGuid().ToString();
                result = getRenderGraph(result, c, cid);
                if (c is AST.AnnotatedNode)
                {
                    var ca = (c as AST.AnnotatedNode);
                    var edge = EdgeStatement.For(id, cid).Set("label", ca.annotation).Set("font", "Consolas").Set("fontsize", "10");
                    result = result.Add(edge);
                }
                else
                {
                    result = result.Add(EdgeStatement.For(id, cid));
                }
            }
            return result;
        }

        static void renderGraph(AST.Node root, string label)
        {
            if (root == null)
                return;

            var filename = Path.GetFileNameWithoutExtension(root.token.filename) + ".png";

            var g = getRenderGraph(Graph.Undirected, root, Guid.NewGuid().ToString());
            //g = g.Add(AttributeStatement.Graph.Set("label", label))
            //    .Add(AttributeStatement.Graph.Set("labeljust", "l"))
            //    .Add(AttributeStatement.Graph.Set("fontname", "Consolas"));
            // g = g.Add(AttributeStatement.Node.Set("shape", "box"));


            var gv_path = @"C:\Users\pragma\Downloads\graphviz-2.38\release\bin\";

            if (Directory.Exists(gv_path))
            {
                var renderer = new Shields.GraphViz.Components.Renderer(gv_path);
                using (Stream file = File.Create(filename))
                {
                    AsyncHelper.RunSync(() =>
                        renderer.RunAsync(g, file, Shields.GraphViz.Services.RendererLayouts.Dot, Shields.GraphViz.Services.RendererFormats.Png, CancellationToken.None)
                     );
                }
            }
            else
            {
                Console.WriteLine("graphviz not found skipping rendering graph!");
            }
        }

#endif

        static Token[] tokenize(string text, string filename)
        {
            Token[] result = null;
            try
            {
                List<Token> ts = new List<Token>();
                Token.Tokenize(ts, text, filename);
                result = ts.ToArray();
            }
            catch (LexerError e)
            {
                Console.WriteLine(e.Message);
            }
            return result;
        }



        static void compile(string filename)
        {
            Console.WriteLine("parsing...");

            Queue<string> toImport = new Queue<string>();
            HashSet<string> imported = new HashSet<string>();

            Stack<Token[]> toCompile = new Stack<Token[]>();

            var ffn = Path.GetFullPath(filename);
            toImport.Enqueue(ffn);
            imported.Add(ffn);

            var scope = AST.MakeRootScope();
            var root = new AST.ProgramRoot(Token.Undefined, scope);

            bool parseError = false;

            while (toImport.Count > 0)
            {
                var fn = toImport.Dequeue();
                var text = File.ReadAllText(fn);
                var tokens = tokenize(text, fn);

                AST.ParseState ps;
                ps.pos = 0;
                ps.tokens = tokens;

                List<string> imports = null;
#if !DEBUG
                try
#endif
                {
                    imports = AST.parseImports(ref ps, scope);
                }
#if !DEBUG
                catch (ParserError error)
                {
                    Console.Error.WriteLine(error.Message);
                    parseError = true;
                }
#endif
                toCompile.Push(tokens);
                foreach (var import in imports)
                {
                    var dir = Path.GetDirectoryName(fn);
                    var imp_fn = Path.GetFullPath(Path.Combine(dir, import));
                    if (!imported.Contains(imp_fn))
                    {
                        toImport.Enqueue(imp_fn);
                        imported.Add(imp_fn);
                    }
                }
            }
            foreach (var tokens in toCompile)
            {
                AST.ParseState ps;
                ps.pos = 0;
                ps.tokens = tokens;
#if !DEBUG
                try
#endif
                {
                    var fileRoot = AST.parseFileRoot(ref ps, scope) as FileRoot;
                    root.files.Add(fileRoot);
                }
#if !DEBUG
                catch (ParserError error)
                {
                    Console.Error.WriteLine(error.Message);
                    parseError = true;
                }
#endif
            }

            //#if DEBUG
            //            Console.WriteLine("rendering graph...");
            //            foreach (var fr in root.files)
            //            {
            //                renderGraph(fr, "");
            //            }

            //#endif

            Console.WriteLine("type checking...");

            var tc = new TypeChecker();

            bool success = true;
            try
            {
                tc.CheckTypes(root);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                success = false;
            }
            if (!success || parseError)
            {
                return;
            }

            Console.WriteLine("backend...");
            var backend = new Backend(Backend.TargetPlatform.x64, tc);
            backend.EmitAndAOT(root, "output.o");
#if DEBUG
            Console.ReadLine();
#endif
        }
    }
}

