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
            if (args.Length == 0)
                return;

            parseARGS(args);
#if DEBUG
            CompilerOptions.debug = true;
            CompilerOptions.useOptimizations = false;
#endif
            try
            {
                var text = File.ReadAllText(CompilerOptions.inputFilename);
                run(text);
            }
            catch (FileNotFoundException)
            {
                writeError("Could not open input file");
            }
        }

        static void writeError(string s)
        {
            Console.WriteLine(s + "!");
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

        static void Test()
        {
            // parse(@"{ return 3 / 2; }");
            //  parse("{ var x = ((float32)12 + 3.0); print(); print(); return (int32)x * 10; }");
            // parse("{ var x = 1.0 / (float32)2; return (int32)(12.0 * x) + 36; }");
            // parse("{ var y = -3 + (12 + 12) * 2; var x = y - 3; return x; }");
            // parse("{ var x = !(12 < 4 << 5); var y = (1 + 2 * 5 + 3) * (x + 4 / foo) + bar(); }");
            // parse("{ return 3; var x = 5; }");

            var p1 = @"
{ 
    var x = (int32)(-3.0 * 4.0);
    if (x > 0)
    {
        return 5;
    }
    else
    {
        x = 2 * x;
    } 
    return x; 
}";
            var p2 = @"
{ 
    for (var i = 10; i > 0; --i)
    {
        print();
    }
    return 0;
}";

            // parse("{ var i = 0; var x = i-- + 1; return i; }");
            // parse("{ var i = 0; var x = --i + 1; return i; }");
            // parse(p2);


            // parse("{ return foo(); }");
            // parse("{ var x = foo() > 3 || foo() == 3 || foo() == -1; return (int32)x;}");
            // parse("{ var x = foo() < 3 || foo() == 2 || foo() == -1; return (int32)x;}");

            run("{ var x = foo() >= 3 && foo() > 1 && foo() > 0; return (int32)x; }");
            
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
                    pos += t.length;
                }
                yield return Token.NewLine(pos, i);
            }

            yield return Token.EOF;
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

        static void renderGraph(AST.Node root, string label)
        {
            if (root == null)
                return;
            var g = getRenderGraph(Graph.Undirected, root, Guid.NewGuid().ToString());
            g = g.Add(AttributeStatement.Graph.Set("label", label));
            var renderer = new Shields.GraphViz.Components.Renderer(@"C:\Program Files (x86)\Graphviz2.38\bin\");
            using (Stream file = File.Create("graph.png"))
            {
                AsyncHelper.RunSync(() =>
                    renderer.RunAsync(g, file, Shields.GraphViz.Services.RendererLayouts.Dot, Shields.GraphViz.Services.RendererFormats.Png, CancellationToken.None)
                 );
            }
        }

        static void run(string text)
        {

            var tokens = tokenize(text).ToList();

            if (CompilerOptions.debug)
            {
                Console.WriteLine("input: " + text);
                Console.WriteLine();
                Console.WriteLine("tokens: ");
                int pos = 0;
                foreach (var t in tokens)
                {
                    if (t.type != Token.TokenType.WhiteSpace)
                        Console.WriteLine("{0}: {1}", pos++, t);
                }

                Console.ReadLine();

                Console.WriteLine();
                Console.WriteLine("PARSING...");
                Console.WriteLine();


            }
            var root = AST.Parse(tokens);
            if (root == null)
            {
                return;
            }
            if (CompilerOptions.debug)
            {
                Console.WriteLine();
                try
                {
                    renderGraph(root, text);
                }
                catch(Exception)
                {
                    Console.WriteLine("graphviz not found. no graph rendering!");
                }
            }
            var backend = new Backend();

            backend.EmitAndRun(root, useOptimizations: CompilerOptions.useOptimizations);
#if DEBUG
            Console.ReadLine();
#endif
        }
    }
}
