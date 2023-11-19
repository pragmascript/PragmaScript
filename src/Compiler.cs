
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using static PragmaScript.AST;

namespace PragmaScript
{

    public class Compiler
    {
        Func<string, string> GetFileText;

        public Compiler(Func<string, string> GetFileText = null)
        {
            this.GetFileText = GetFileText;
        }
        
        string ResolveImportPath(string import, string dir, IList<string> includeDirs)
        {
            var result = Path.GetFullPath(Path.Combine(dir, import));
            if (!File.Exists(result))
            {
                foreach (var incDir in includeDirs)
                {
                    result = Path.GetFullPath(Path.Combine(incDir, import));
                    if (File.Exists(result)) 
                    {
                        break;
                    }
                }    
            }
            return result;
        }
        public (Scope root, TypeChecker tc) Compile(CompilerOptionsBuild options)
        {
            CompilerOptionsBuild._i.lib_path.Add(Path.GetFullPath(Program.RelDir("..\\lib")));
            CompilerOptionsBuild._i.include_dirs.Add(Path.GetFullPath(Program.RelDir("..\\include")));
            
            Platform platform;
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
            {
                platform = Platform.WindowsX64;
                CompilerOptionsBuild._i.libs.Add("kernel32.lib");
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
                    throw new CompilerError($"Could not read import file", import_token);
                }

                if (string.IsNullOrWhiteSpace(text))
                {
                    throw new CompilerError($"Empty import file!", import_token);
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
                    var importPath = ResolveImportPath(import, dir, options.include_dirs);
                    if (!imported.Contains(importPath))
                    {
                        toImport.Enqueue((importPath, token));
                        imported.Add(importPath);
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

            var entry = CompilerOptionsBuild._i.entry;
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
                CompilerOptionsBuild._i.output = output;
            }

            var asm = d.GetAttribute("COMPILE.ASM");
            if (asm == "TRUE")
            {
                CompilerOptionsBuild._i.asm = true;
            }
            else if (asm == "FALSE")
            {
                CompilerOptionsBuild._i.asm = false;
            }
            var ll = d.GetAttribute("COMPILE.LL");
            if (ll == "TRUE")
            {
                CompilerOptionsBuild._i.ll = true;
            }
            else if (ll == "FALSE")
            {
                CompilerOptionsBuild._i.ll = false;
            }
            var bc = d.GetAttribute("COMPILE.BC");
            if (bc == "TRUE")
            {
                CompilerOptionsBuild._i.bc = true;
            }
            else if (bc == "FALSE")
            {
                CompilerOptionsBuild._i.bc = false;
            }

            var opt = d.GetAttribute("COMPILE.OPT");
            if (int.TryParse(opt, out int opt_level))
            {
                CompilerOptionsBuild._i.optimizationLevel = opt_level;
            }

            var debugInfo = d.GetAttribute("COMPILE.DEBUGINFO");
            if (debugInfo == "TRUE")
            {
                CompilerOptionsBuild._i.debugInfo = true;
            }
            else if (debugInfo == "FALSE")
            {
                CompilerOptionsBuild._i.debugInfo = false;
            }

            var cpu = d.GetAttribute("COMPILE.CPU");
            if (!string.IsNullOrWhiteSpace(cpu))
            {
                CompilerOptionsBuild._i.cpu = cpu;
            }

            var run = d.GetAttribute("COMPILE.RUN");
            if (run == "TRUE")
            {
                CompilerOptionsBuild._i.runAfterCompile = true;
            }
            else if (run == "FALSE")
            {
                CompilerOptionsBuild._i.runAfterCompile = false;
            }

            var dll = d.GetAttribute("COMPILE.DLL");
            if (dll == "TRUE")
            {
                CompilerOptionsBuild._i.dll = true;
            }
            else if (dll == "FALSE")
            {
                CompilerOptionsBuild._i.dll = false;
            }


            var libs = d.GetAttribute("COMPILE.LIBS", upperCase: false);
            if (libs != null)
            {
                var ls = libs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var lib in ls)
                {
                    var tlib = lib.Trim();
                    CompilerOptionsBuild._i.libs.Add(tlib);
                }
            }

            {
                var fullPath = Path.GetFullPath(@"..\lib", Path.GetDirectoryName(entry.token.filename));
                CompilerOptionsBuild._i.lib_path.Add(fullPath);
            }
            var lib_path = d.GetAttribute("COMPILE.PATH", upperCase: false);
            if (lib_path != null)
            {
                var lp = lib_path.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var p in lp)
                {
                    var tp = p.Trim();
                    var fullPath = Path.GetFullPath(tp, Path.GetDirectoryName(entry.token.filename));
                    CompilerOptionsBuild._i.lib_path.Add(fullPath);
                }
            }
        }
    }
}



