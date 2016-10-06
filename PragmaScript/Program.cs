using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Shields.GraphViz.Models;
using System.Threading;

namespace PragmaScript
{

    static class CompilerOptions
    {
        public static bool debug = false;
        public static string inputFilename;
        public static bool useOptimizations = true;
    }

    // http://llvm.lyngvig.org/Articles/Mapping-High-Level-Constructs-to-LLVM-IR
    class Program
    {
        static void Main(string[] args)
        {
            parseARGS(args);


#if DEBUG
            CompilerOptions.debug = true;
            CompilerOptions.useOptimizations = false;
            CompilerOptions.inputFilename = @"Programs\bugs.ps";
#endif
            if (CompilerOptions.inputFilename == null)
            {
                Console.WriteLine("Input file name missing!");
                return;
            }
            try
            {
                var text = File.ReadAllText(CompilerOptions.inputFilename);
                run(text);
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
            Console.WriteLine("-O1: turn on optimizations");
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
                            CompilerOptions.useOptimizations = false;
                            break;
                        case "O1":
                        case "O2":
                        case "O3":
                            CompilerOptions.useOptimizations = true;
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
            var g = getRenderGraph(Graph.Undirected, root, Guid.NewGuid().ToString());
            //g = g.Add(AttributeStatement.Graph.Set("label", label))
            //    .Add(AttributeStatement.Graph.Set("labeljust", "l"))
            //    .Add(AttributeStatement.Graph.Set("fontname", "Consolas"));
            // g = g.Add(AttributeStatement.Node.Set("shape", "box"));


            var gv_path = @"C:\Users\pragma\Downloads\graphviz-2.38\release\bin\";

            if (Directory.Exists(gv_path))
            {
                var renderer = new Shields.GraphViz.Components.Renderer(gv_path);
                using (Stream file = File.Create("graph.png"))
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
        static void run(string text)
        {

            var tokens = Token.Tokenize(text);

            if (CompilerOptions.debug)
            {
                //Console.WriteLine("input: " + text);
                //Console.WriteLine();
                //Console.WriteLine("tokens: ");
                //int pos = 0;
                //foreach (var t in tokens)
                //{
                //    if (t.type != Token.TokenType.WhiteSpace)
                //        Console.WriteLine("{0}: {1}", pos++, t);
                //}

                // Console.ReadLine();
            }

            Console.WriteLine("parsing...");

            var scope = AST.MakeRootScope();
            var root = AST.Parse(tokens, scope);
            if (root == null)
            {
                return;
            }
#if DEBUG
            renderGraph(root, text);
#endif
            AST.TypeCheck(root, scope);
            var backend = new Backend();
            // 
            // backend.EmitAndJIT(root, useOptimizations: CompilerOptions.useOptimizations);
            backend.EmitAndAOT(root, "output.o");
#if DEBUG
            Console.ReadLine();
#endif
        }
  
    }






}

