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
   
    class Program
    {
        static void Main(string[] args)
        {
            parse("var x = ((float32)12 + 3.0); return (int32)x * 10;");
            // parse("var x = 1.0 / (float32)2; return (int32)(12.0 * x) + 36;");
            // parse("var y = -3 + (12 + 12) * 2; var x = y - 3; return x;");
            // parse("var y = (1 + 2 * 5 + 3) * (x + 4 / foo) + bar();");
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

                    //// abort on first error token
                    //if (t.type == Token.TokenType.Error)
                    //{
                    //    yield break;
                    //}
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

            Console.WriteLine();
            Console.WriteLine("PARSING...");
            Console.WriteLine();
            var root = AST.Parse(tokens);
            Console.WriteLine();
            if (root == null)
            {
                Console.ReadLine();
                return;
            }
            

            renderGraph(root, text);

            var backend = new Backend();
            backend.EmitAndRun(root, useOptimizations: false);

            Console.WriteLine(root);

        }
    }
}
