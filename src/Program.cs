// #define OLD_BACKEND


using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static PragmaScript.AST;
using System.Diagnostics;



namespace PragmaScript
{
    public static class Extensions
    {
        public static char at(this string s, int idx)
        {
            if (idx >= 0 && idx < s.Length) {
                return s[idx];
            } else {
                return '\0';
            }
        }
    }
    static class CompilerOptions
    {
        public static bool debug = true;
        public static string inputFilename;
        public static int optimizationLevel;
        public static string cpu = "native";
        public static bool runAfterCompile;
        public static bool asm = false;
        public static bool ll = false;
        public static bool bc = false;
        public static List<string> libs = new List<string>();
        public static List<string> lib_path = new List<string>();
        public static bool dll = false;
        public static string output = "output.exe";

        internal static FunctionDefinition entry;
    }

    // http://llvm.lyngvig.org/Articles/Mapping-High-Level-Constructs-to-LLVM-IR
    class Program
    {
        static Dictionary<string, bool> files = new Dictionary<string, bool>();
        static void Main(string[] args)
        {
            parseARGS(args);
            
#if DEBUG
            // CompilerOptions.debug = true;
            // CompilerOptions.asm = false;
            // CompilerOptions.optimizationLevel = 3;
            // CompilerOptions.runAfterCompile = true;
#endif
#if false
#if DOTNETCORE
            var programDir = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "samples"));
#else
            var programDir = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..", "samples"));
#endif
            // CompilerOptions.inputFilename = Path.Combine(programDir, "smallpt", "smallpt_win.prag");
            // CompilerOptions.inputFilename = Path.Combine(programDir, "handmade", "win32_handmade.prag");
            CompilerOptions.inputFilename = Path.Combine(programDir, "basics", "hello_world.prag");
#endif
            if (CompilerOptions.inputFilename == null) {
                Console.WriteLine("Input file name missing!");
                return;
            }
            try {
                compile(CompilerOptions.inputFilename);
            }
            catch (FileNotFoundException) {
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
            Console.WriteLine("OVERVIEW:");
            Console.WriteLine("    pragma compiler");
            Console.WriteLine();
            Console.WriteLine("USAGE:");
            Console.WriteLine("    pragma.exe [options] <input>");
            Console.WriteLine();
            Console.WriteLine("OPTIONS:");
            Console.WriteLine("    -O <filename>   set output filename");
            Console.WriteLine("    -D              build in debug mode");
            Console.WriteLine("    -O0             turn off optimizations");
            Console.WriteLine("    -OX             turn on optimization level X in [1..3]");
            Console.WriteLine("    -R              run program after compilation");
            Console.WriteLine("    -ASM            output generated assembly");
            Console.WriteLine("    -LL             output LLVM IL");
            Console.WriteLine("    -BC             output LLVM Bitcode");

        }
        static void parseARGS(string[] args)
        {
            for (int i = 0; i < args.Length; ++i) {
                var arg = args[i];
                if (arg.TrimStart().StartsWith("-")) {
                    var x = arg.TrimStart().Remove(0, 1);
                    x = x.ToUpperInvariant();
                    switch (x) {
                        case "D":
                        case "DEGUG":
                            CompilerOptions.debug = true;
                            break;
                        case "ASM":
                            CompilerOptions.asm = true;
                            break;
                        case "LL":
                            CompilerOptions.ll = true;
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
                            Environment.Exit(0);
                            break;
                        case "R":
                        case "RUN":
                            CompilerOptions.runAfterCompile = true;
                            break;
                        case "DLL":
                            CompilerOptions.dll = true;
                            break;
                        case "O":
                        case "OUTPUT":
                            CompilerOptions.output = args[++i];
                            break;
                        default:
                            writeError("Unknown command line option");
                            break;
                    }
                } else {
                    CompilerOptions.inputFilename = arg;
                }
            }
        }


#if FALSE
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
            try {
                List<Token> ts = new List<Token>();
                Token.Tokenize(ts, text, filename);
                result = ts.ToArray();
            }
            catch (LexerError e) {
                Console.WriteLine(e.Message);
            }
            return result;
        }


        static void skipWhitespace(string text, ref int idx)
        {
            while (char.IsWhiteSpace(text.at(idx))) {
                idx++;
            }
        }

        class PrepIf
        {
            public bool Condition;
            public bool InElse;
        }
        static string preprocess(string text)
        {
            HashSet<string> defines = new HashSet<string>();
            defines.Add("TRUE");
            if (CompilerOptions.debug) {
                defines.Add("DEBUG");
            } else {
                defines.Add("RELEASE");
            }

            StringBuilder result = new StringBuilder(text.Length);
            int idx = 0;


            Stack<PrepIf> ifs = new Stack<PrepIf>();

            bool inside_line_comment = false;

            Func<string, bool> nextString = (s) => {
                for (int i = 0; i < s.Length; ++i) {
                    if (char.ToUpper(text.at(idx + 1 + i)) != s[i]) {
                        return false;
                    }
                }
                return true;
            };
            while (true) {
                if (text.at(idx - 1) == '/' && text.at(idx) == '/') {
                    inside_line_comment = true;
                }
                if (!inside_line_comment && text.at(idx) == '#') {
                    if (nextString("IF")) {
                        idx += 3;
                        if (idx >= text.Length) {
                            return result.ToString();
                        }
                        skipWhitespace(text, ref idx);
                        if (idx >= text.Length) {
                            return result.ToString();
                        }
                        int ident_start = idx;
                        idx--;
                        while (Token.isIdentifierChar(text.at(++idx))) { }
                        int ident_end = idx;
                        var ident = text.Substring(ident_start, ident_end - ident_start);
                        var con = defines.Contains(ident);
                        ifs.Push(new PrepIf { Condition = con, InElse = false });
                    } else if (nextString("DEFINE")) {
                        idx += 7;
                        if (idx >= text.Length) {
                            return result.ToString();
                        }
                        skipWhitespace(text, ref idx);
                        if (idx >= text.Length) {
                            return result.ToString();
                        }
                        int ident_start = idx;
                        idx--;
                        while (Token.isIdentifierChar(text.at(++idx))) { }
                        int ident_end = idx;
                        var ident = text.Substring(ident_start, ident_end - ident_start);
                        defines.Add(ident);
                        Console.WriteLine($"#DEFINE {ident}");
                    } else if (nextString("UNDEF")) {
                        idx += 7;
                        if (idx >= text.Length) {
                            return result.ToString();
                        }
                        skipWhitespace(text, ref idx);
                        if (idx >= text.Length) {
                            return result.ToString();
                        }
                        int ident_start = idx;
                        idx--;
                        while (Token.isIdentifierChar(text.at(++idx))) { }
                        int ident_end = idx;
                        var ident = text.Substring(ident_start, ident_end - ident_start);
                        defines.Remove(ident);
                        Console.WriteLine($"#UNDEF {ident}");
                    } else {
                        int ident_start = idx + 1;
                        while (Token.isIdentifierChar(text.at(++idx))) { }
                        int ident_end = idx;
                        var ident = text.Substring(ident_start, ident_end - ident_start);

                        if (ident.ToUpper() == "ELSE") {
                            ifs.Peek().InElse = true;
                        } else if (ident.ToUpper() == "ENDIF") {
                            ifs.Pop();
                        }
                    }
                }
                var skip = false;
                if (ifs.Count > 0) {
                    var _if = ifs.Peek();
                    skip = !(_if.Condition ^ _if.InElse);
                }

                // HACK: keep newlines for error reporting
                if (!skip || text[idx] == '\r' || text[idx] == '\n') {
                    if (text[idx] == '\n') {
                        inside_line_comment = false;
                    }
                    result.Append(text[idx]);
                }
                idx++;
                if (idx >= text.Length) {
                    var result_string = result.ToString();
                    return result_string;
                }

            }
        }

        static void setEntryPoint(FunctionDefinition entry)
        {

            Debug.Assert(entry.GetAttribute("COMPILE.ENTRY") == "TRUE");
            var d = entry;

            var output = d.GetAttribute("COMPILE.OUTPUT", false);
            if (output != null) {
                CompilerOptions.output = output;
            }

            var asm = d.GetAttribute("COMPILE.ASM");
            if (asm == "TRUE") {
                CompilerOptions.asm = true;
            } else if (asm == "FALSE") {
                CompilerOptions.asm = false;
            }
            var ll = d.GetAttribute("COMPILE.LL");
            if (ll == "TRUE") {
                CompilerOptions.ll = true;
            } else if (ll == "FALSE") {
                CompilerOptions.ll = false;
            }
            var bc = d.GetAttribute("COMPILE.BC");
            if (bc == "TRUE") {
                CompilerOptions.bc = true;
            } else if (bc == "FALSE") {
                CompilerOptions.bc = false;
            }

            var opt = d.GetAttribute("COMPILE.OPT");
            if (int.TryParse(opt, out int opt_level)) {
                CompilerOptions.optimizationLevel = opt_level;
            }

            var cpu = d.GetAttribute("COMPILE.CPU");
            if (!string.IsNullOrWhiteSpace(cpu)) {
                CompilerOptions.cpu = cpu;
            }

            var run = d.GetAttribute("COMPILE.RUN");
            if (run == "TRUE") {
                CompilerOptions.runAfterCompile = true;
            } else if (run == "FALSE") {
                CompilerOptions.runAfterCompile = false;
            }

            var dll = d.GetAttribute("COMPILE.DLL");
            if (dll == "TRUE") {
                CompilerOptions.dll = true;
            } else if (dll == "FALSE") {
                CompilerOptions.dll = false;
            }


            var libs = d.GetAttribute("COMPILE.LIBS", upperCase: false);
            if (libs != null) {
                var ls = libs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var lib in ls) {
                    var tlib = lib.Trim();
                    CompilerOptions.libs.Add(tlib);
                }
            }

            var lib_path = d.GetAttribute("COMPILE.PATH", upperCase: false);
            if (lib_path != null) {
                var lp = lib_path.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var p in lp) {
                    var tp = p.Trim();
                    CompilerOptions.lib_path.Add(tp);
                }

            }
        }

        static void compile(string filename)
        {
            var timer = new Stopwatch();
            Console.Write("parsing...");
            timer.Start();

            Queue<string> toImport = new Queue<string>();
            HashSet<string> imported = new HashSet<string>();

            Stack<Token[]> toCompile = new Stack<Token[]>();

            var ffn = Path.GetFullPath(filename);
            toImport.Enqueue(ffn);
            imported.Add(ffn);

            var scope = AST.MakeRootScope();
            var root = new AST.ProgramRoot(Token.Undefined, scope);

            bool parseError = false;

            while (toImport.Count > 0) {
                var fn = toImport.Dequeue();
                var text = File.ReadAllText(fn);
                text = preprocess(text);
                var tokens = tokenize(text, fn);

                AST.ParseState ps = new ParseState();
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
                catch (ParserError error) {
                    Console.Error.WriteLine(error.Message);
                    parseError = true;
                }
#endif
                toCompile.Push(tokens);
                foreach (var import in imports) {
                    var dir = Path.GetDirectoryName(fn);
                    var imp_fn = Path.GetFullPath(Path.Combine(dir, import));
                    if (!imported.Contains(imp_fn)) {
                        toImport.Enqueue(imp_fn);
                        imported.Add(imp_fn);
                    }
                }
            }
            foreach (var tokens in toCompile) {
                AST.ParseState ps = new ParseState();
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
                catch (ParserError error) {
                    Console.Error.WriteLine(error.Message);
                    parseError = true;
                }
#endif
            }

            var entry = CompilerOptions.entry;
            if (entry != null) {
                setEntryPoint(entry);
            }


#if DEBUG
            //Console.WriteLine("rendering graph...");
            //foreach (var fr in root.files)
            //{
            //    renderGraph(fr, "");
            //}

#endif
            timer.Stop();
#if DISPLAY_TIMINGS
            Console.WriteLine($"{timer.ElapsedMilliseconds}ms");
#else
            Console.WriteLine();
#endif
            Console.Write("type checking...");
            timer.Reset();
            timer.Start();

            var tc = new TypeChecker();

            bool success = true;
            try {
                tc.CheckTypes(root);
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                success = false;
            }
            if (!success || parseError) {
                return;
            }

            timer.Stop();
#if DISPLAY_TIMINGS
            Console.WriteLine($"{timer.ElapsedMilliseconds}ms");
#else
            Console.WriteLine();
#endif
            Console.Write("de-sugar...");
            timer.Reset();
            timer.Start();

            ParseTreeTransformations.Init(root);
            ParseTreeTransformations.Desugar(tc.embeddings, tc);
            ParseTreeTransformations.Desugar(tc.namespaceAccesses, tc);

            timer.Stop();
#if DISPLAY_TIMINGS
            Console.WriteLine($"{timer.ElapsedMilliseconds}ms");
#else
            Console.WriteLine();
#endif

            if (entry == null) {
                Console.WriteLine("warning: No program entry point defined.");
            }

            Console.Write("backend...");
            timer.Reset();
            timer.Start();
#if OLD_BACKEND
            var backend = new BackendLLVM(BackendLLVM.TargetPlatform.x64, tc);
            backend.Emit(root, entry);
#else 
            var backend = new Backend(tc);
            backend.Visit(root, entry);

#endif
            timer.Stop();
#if DISPLAY_TIMINGS
            Console.WriteLine($"{timer.ElapsedMilliseconds}ms");
#else
            Console.WriteLine();
#endif
            backend.AOT();
#if DEBUG
            // Console.ReadLine();
#endif
        }
    }
}

