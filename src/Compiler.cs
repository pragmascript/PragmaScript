
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using static PragmaScript.AST;

namespace PragmaScript
{
    public class CompilerOptions
    {
        public static CompilerOptions _i;
        internal bool debug = false;
        public bool debugInfo = true;
        public string inputFilename;
        public int optimizationLevel;
        public string cpu = "native";
        public bool runAfterCompile;
        public bool buildExecuteable = true;
        public bool asm = false;
        public bool ll = false;
        public bool bc = false;
        public List<string> libs = new List<string>();
        public List<string> lib_path = new List<string>();
        public bool dll = false;
        public string output = "output.exe";

        public bool useFastMath
        {
            get { return optimizationLevel > 3; }
        }

        internal FunctionDefinition entry;
        public CompilerOptions()
        {
            _i = this;
        }
    }


    public class Compiler
    {
        Func<string, string> GetFileText;
        bool logToConsole;

        public Compiler(Func<string, string> GetFileText = null, bool logToConsole = true)
        {
            this.GetFileText = GetFileText;
            this.logToConsole = logToConsole;
        }

        public void Log(string s)
        {
            if (logToConsole)
            {
                System.Console.WriteLine(s);
            }
        }

        public (Scope root, TypeChecker tc) Compile(CompilerOptions options)
        {
            var filename = options.inputFilename;
            var buildExecuteable = options.buildExecuteable;

            var timer = new Stopwatch();
            Log("parsing...");
            timer.Start();

            Queue<(string, Token)> toImport = new Queue<(string, Token)>();
            HashSet<string> imported = new HashSet<string>();

            Stack<Token[]> toCompile = new Stack<Token[]>();

            var ffn = filename;
            if (!Path.IsPathRooted(filename)) 
            {
                ffn = Path.GetFullPath(filename);
            }
            toImport.Enqueue((ffn, Token.Undefined));
            imported.Add(ffn);

            var scope = AST.MakeRootScope();
            var rootToken = Token.UndefinedRoot(ffn);
            var root = new AST.ProgramRoot(rootToken, scope);

            while (toImport.Count > 0)
            {
                var (fn, import_token) = toImport.Dequeue();
                string text;
                try
                {
                    if (GetFileText != null)
                    {
                        text = GetFileText(fn);
                    }
                    else
                    {
                        text = File.ReadAllText(fn);
                    }
                }
                catch (Exception)
                {
                    throw new CompilerError($"Could not read import file \"{fn}\"", import_token);
                }

                if (string.IsNullOrWhiteSpace(text))
                {
                    throw new CompilerError($"Empty import file \"{fn}\"", import_token);
                }

                text = Preprocessor.Preprocess(text);
                var tokens = Tokenize(text, fn);

                AST.ParseState ps = new ParseState();
                ps.pos = 0;
                ps.tokens = tokens;

                List<(string, Token)> imports = null;

                imports = AST.ParseImports(ref ps, scope);

                toCompile.Push(tokens);
                foreach (var (import, token) in imports)
                {
                    var dir = Path.GetDirectoryName(fn);
                    var imp_fn = Path.GetFullPath(Path.Combine(dir, import));
                    if (!imported.Contains(imp_fn))
                    {
                        toImport.Enqueue((imp_fn, token));
                        imported.Add(imp_fn);
                    }
                }
            }
            foreach (var tokens in toCompile)
            {
                AST.ParseState ps = new ParseState();
                ps.pos = 0;
                ps.tokens = tokens;
                // Log($"parsing {ps.tokens[0].filename}...");
                var fileRoot = AST.ParseFileRoot(ref ps, scope) as FileRoot;
                root.files.Add(fileRoot);
            }

            var entry = CompilerOptions._i.entry;
            if (entry != null)
            {
                SetEntryPoint(entry);
            }

            timer.Stop();
            Log($"{timer.ElapsedMilliseconds}ms");

            Log("type checking...");
            timer.Reset();
            timer.Start();


            var tc = new TypeChecker();

            tc.CheckTypes(root);

            timer.Stop();
            Log($"{timer.ElapsedMilliseconds}ms");
            Log("de-sugar...");
            timer.Reset();
            timer.Start();

            ParseTreeTransformations.Init(root);
            ParseTreeTransformations.Desugar(tc.embeddings, tc);
            ParseTreeTransformations.Desugar(tc.namespaceAccesses, tc);

            timer.Stop();
            Log($"{timer.ElapsedMilliseconds}ms");

            Log("backend...");
            timer.Reset();
            timer.Start();
            var backend = new Backend(tc);
            if (buildExecuteable && entry == null)
            {
                throw new CompilerError("No entry point defined", root.token);
            }
            backend.Visit(root, entry);
            timer.Stop();
            Log($"{timer.ElapsedMilliseconds}ms");
            if (buildExecuteable)
            {
                backend.AOT();
            }
            return (scope, tc);
        }

        Token[] Tokenize(string text, string filename)
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
                Log(e.Message);
            }
            return result;
        }

        void SetEntryPoint(FunctionDefinition entry)
        {
            Debug.Assert(entry.GetAttribute("COMPILE.ENTRY") == "TRUE");
            var d = entry;

            var output = d.GetAttribute("COMPILE.OUTPUT", false);
            if (output != null)
            {
                CompilerOptions._i.output = output;
            }

            var asm = d.GetAttribute("COMPILE.ASM");
            if (asm == "TRUE")
            {
                CompilerOptions._i.asm = true;
            }
            else if (asm == "FALSE")
            {
                CompilerOptions._i.asm = false;
            }
            var ll = d.GetAttribute("COMPILE.LL");
            if (ll == "TRUE")
            {
                CompilerOptions._i.ll = true;
            }
            else if (ll == "FALSE")
            {
                CompilerOptions._i.ll = false;
            }
            var bc = d.GetAttribute("COMPILE.BC");
            if (bc == "TRUE")
            {
                CompilerOptions._i.bc = true;
            }
            else if (bc == "FALSE")
            {
                CompilerOptions._i.bc = false;
            }

            var opt = d.GetAttribute("COMPILE.OPT");
            if (int.TryParse(opt, out int opt_level))
            {
                CompilerOptions._i.optimizationLevel = opt_level;
            }

            var debugInfo = d.GetAttribute("COMPILE.DEBUGINFO");
            if (debugInfo == "TRUE")
            {
                CompilerOptions._i.debugInfo = true;
            }
            else if (debugInfo == "FALSE")
            {
                CompilerOptions._i.debugInfo = false;
            }

            var cpu = d.GetAttribute("COMPILE.CPU");
            if (!string.IsNullOrWhiteSpace(cpu))
            {
                CompilerOptions._i.cpu = cpu;
            }

            var run = d.GetAttribute("COMPILE.RUN");
            if (run == "TRUE")
            {
                CompilerOptions._i.runAfterCompile = true;
            }
            else if (run == "FALSE")
            {
                CompilerOptions._i.runAfterCompile = false;
            }

            var dll = d.GetAttribute("COMPILE.DLL");
            if (dll == "TRUE")
            {
                CompilerOptions._i.dll = true;
            }
            else if (dll == "FALSE")
            {
                CompilerOptions._i.dll = false;
            }


            var libs = d.GetAttribute("COMPILE.LIBS", upperCase: false);
            if (libs != null)
            {
                var ls = libs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var lib in ls)
                {
                    var tlib = lib.Trim();
                    CompilerOptions._i.libs.Add(tlib);
                }
            }

            var lib_path = d.GetAttribute("COMPILE.PATH", upperCase: false);
            if (lib_path != null)
            {
                var lp = lib_path.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var p in lp)
                {
                    var tp = p.Trim();
                    CompilerOptions._i.lib_path.Add(tp);
                }
            }
        }

    }
}



