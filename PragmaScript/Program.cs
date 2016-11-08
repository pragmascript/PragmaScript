using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Shields.GraphViz.Models;
using System.Threading;
using System.Text;

namespace PragmaScript
{

    static class CompilerOptions
    {
        public static bool debug = false;
        public static List<string> inputFilenames = new List<string>(); 
        public static int optimizationLevel;
    }

    // http://llvm.lyngvig.org/Articles/Mapping-High-Level-Constructs-to-LLVM-IR
    class Program
    {
        static void Main(string[] args)
        {
            parseARGS(args);

#if DEBUG
            CompilerOptions.debug = true;
            CompilerOptions.optimizationLevel = 3;

            // CompilerOptions.inputFilenames.AddRange(new string[]{ @"Programs\preamble.ps", @"Programs\windows.ps", @"Programs\win32_handmade.ps" });
            // CompilerOptions.inputFilenames.AddRange(new string[] { @"Programs\preamble.ps", @"Programs\windows.ps", @"Programs\bugs.ps" });
            CompilerOptions.inputFilenames.AddRange(new string[] { @"Programs\bugs.ps" });
#endif
            if (CompilerOptions.inputFilenames.Count == 0)
            {
                Console.WriteLine("Input file name missing!");
                return;
            }
            try
            {
                
                var tokens = new List<Token[]>();
                foreach (var fn in CompilerOptions.inputFilenames)
                {
                    var text = File.ReadAllText(fn);
                    var filename = Path.GetFileName(fn);
                    try
                    {
                        List<Token> ts = new List<Token>(); ;
                        Token.Tokenize(ts, text, filename);
                        tokens.Add(ts.ToArray());
                    }
                    catch(LexerError e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                compile(tokens);
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
            Console.WriteLine("-D: turn on debug messages");
            Console.WriteLine("-O0: turn off optimizations");
            Console.WriteLine("-OX: turn on optimization level X in [1..3]");
            Console.WriteLine();
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
                        default:
                            writeError("Unknown command line option");
                            break;
                    }
                }
                else
                {
                    CompilerOptions.inputFilenames.Add(arg);
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
        static void compile(List<Token[]> tokens)
        {

            
            Console.WriteLine("parsing...");

            var scope = AST.MakeRootScope();
            var root = new AST.ProgramRoot(Token.Undefined, scope);
            foreach (var pt in tokens)
            {
                AST.FileRoot fr = null;
#if !DEBUG
                try
#endif
                {
                    fr = AST.ParseFile(pt, scope);
                }
#if !DEBUG
                catch (ParserError error)
                {
                    Console.Error.WriteLine(error.Message);
                }
#endif
                if (fr == null)
                {
                    return;
                }
                root.files.Add(fr);
            }

#if DEBUG
            Console.WriteLine("rendering graph...");
            foreach (var fr in root.files)
            {
                renderGraph(fr, "");
            }
            
#endif

            Console.WriteLine("type checking...");

            var tc = new TypeChecker();

            bool success = true;
#if false
            tc.CheckTypes(root as AST.Root);
#else
            try
            {
                tc.CheckTypes(root);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                success = false;
            }
#endif
            if (!success)
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

