
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using CommandLine;
using static PragmaScript.AST;

namespace PragmaScript
{

    public class CompilerOptions
    {
        public static CompilerOptions _i;

        [Value(0, MetaName = "input-filename", Required = true, HelpText = "Source filename to compile.")]
        public string inputFilename { get; set; }

        [Option('d', "debug", HelpText = "Compile with DEBUG preprocessor define.")]
        public bool debug { get; set; }

        [Option('g', "generate-debug-info", HelpText = "Generate debugging information.")]
        public bool debugInfo { get; set; }

        [Option('o', "output-filename", HelpText = "Filename of the output executeable.", Default = "output.exe")]
        public string output { get; set; }

        [Option('O', "optimization-level", HelpText = "Backend optimization level between 0 and 4.", Default = 0)]
        public int optimizationLevel { get; set; }

        public string cpu = "native";

        [Option('r', "run", HelpText = "Run executeable after compilation.")]
        public bool runAfterCompile { get; set; }

        [Option("emit-asm", HelpText = "Output generated assembly.")]
        public bool asm { get; set; }
        [Option("emit-llvm", HelpText = "Output generated LLVM IR.")]
        public bool ll { get; set; }

        public bool bc = false;

        [Option('l', "libs", HelpText = "';' separated list of static libraries to link against.", Separator = ';')]
        public IList<string> libs { get; set; }
        [Option('L', "lib-dirs", HelpText = "';' separated list of library search directories.", Separator = ';')]
        public IList<string> lib_path { get; set; }

        [Option("shared-library", HelpText = "Set binary type to a shared library file.")]
        public bool dll { get; set; }

        [Option('v', "verbose", HelpText = "Set output to verbose messages.")]
        public bool verbose { get; set; }

        [Option("dry-run", HelpText = "Compile and output errors only; no executeable will be generated.")]
        public bool dryRun { get; set; }

        public bool buildExecuteable { get { return !dryRun; } set { dryRun = !value; } }
        public bool useFastMath
        {
            get { return optimizationLevel > 3; }
        }

        internal FunctionDefinition entry;
        public CompilerOptions()
        {
            _i = this;
        }

        [CommandLine.Text.Usage(ApplicationAlias = "pragma")]
        public static IEnumerable<CommandLine.Text.Example> Examples
        {
            get
            {
                yield return new CommandLine.Text.Example("Debug build \"hello.prag\" outut \"hello.exe\"", new CommandLine.UnParserSettings() { PreferShortName = true },
                    new CompilerOptions { inputFilename = "hello.prag", output = "hello.exe", debug = true, debugInfo = true });
                yield return new CommandLine.Text.Example("Compile optimized release build and run", new CommandLine.UnParserSettings() { PreferShortName = true },
                    new CompilerOptions { inputFilename = "hello.prag", output = "hello.exe", optimizationLevel = 3, runAfterCompile = true, libs = new string[] { "user32.lib", "libopenlibm.a" } });
            }
        }
    }

    public class Compiler
    {
        Func<string, string> GetFileText;

        public Compiler(Func<string, string> GetFileText = null)
        {
            this.GetFileText = GetFileText;
        }

        public (Scope root, TypeChecker tc) Compile(CompilerOptions options)
        {
            Platform platform;
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
            {
                platform = Platform.WindowsX64;
            }
            else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux))
            {
                platform = Platform.LinuxX64;
            }
            else
            {
                throw new Exception("Platform not supported");
            }

            var filename = options.inputFilename;
            var buildExecuteable = options.buildExecuteable;

            var timer = new Stopwatch();
            Program.CompilerMessage("parsing...", CompilerMessageType.Info);
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

                text = Preprocessor.Preprocess(text, platform);
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
            Program.CompilerMessage($"{timer.ElapsedMilliseconds}ms", CompilerMessageType.Timing);
            Program.CompilerMessage("type checking...", CompilerMessageType.Info);
            timer.Reset();
            timer.Start();


            var tc = new TypeChecker();

            tc.CheckTypes(root);

            timer.Stop();
            Program.CompilerMessage($"{timer.ElapsedMilliseconds}ms", CompilerMessageType.Timing);
            Program.CompilerMessage("de-sugar...", CompilerMessageType.Info);
            timer.Reset();
            timer.Start();

            ParseTreeTransformations.Init(root);
            ParseTreeTransformations.Desugar(tc.embeddings, tc);
            ParseTreeTransformations.Desugar(tc.namespaceAccesses, tc);

            timer.Stop();
            Program.CompilerMessage($"{timer.ElapsedMilliseconds}ms", CompilerMessageType.Timing);

            Program.CompilerMessage("backend...", CompilerMessageType.Info);
            timer.Reset();
            timer.Start();
            var backend = new Backend(tc, platform);
            if (buildExecuteable && entry == null)
            {
                throw new CompilerError("No entry point defined", root.token);
            }
            backend.Visit(root, entry);
            timer.Stop();
            Program.CompilerMessage($"{timer.ElapsedMilliseconds}ms", CompilerMessageType.Timing);
            if (buildExecuteable)
            {
                backend.AOT();
            }
            return (scope, tc);
        }

        Token[] Tokenize(string text, string filename)
        {
            Token[] result = null;

            List<Token> ts = new List<Token>();
            Token.Tokenize(ts, text, filename);
            result = ts.ToArray();

            // try
            // {
            //     List<Token> ts = new List<Token>();
            //     Token.Tokenize(ts, text, filename);
            //     result = ts.ToArray();
            // }
            // catch (LexerError e)
            // {

            //     throw new CompilerError(e.message);
            //     // Program.CompilerMessage(e.Message, CompilerMessageType.Error);
            // }
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

            {
                var fullPath = Path.GetFullPath(@"..\lib", Path.GetDirectoryName(entry.token.filename));
                CompilerOptions._i.lib_path.Add(fullPath);
            }
            var lib_path = d.GetAttribute("COMPILE.PATH", upperCase: false);
            if (lib_path != null)
            {
                var lp = lib_path.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var p in lp)
                {
                    var tp = p.Trim();
                    var fullPath = Path.GetFullPath(tp, Path.GetDirectoryName(entry.token.filename));
                    CompilerOptions._i.lib_path.Add(fullPath);
                }
            }
        }
    }
}



