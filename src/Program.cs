using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Diagnostics;

using static PragmaScript.AST;

using CommandLine;
using CommandLine.Text;


namespace PragmaScript
{
    public enum CompilerMessageType
    {
        Warning,
        Error,
        Info,
        Timing
    }


    // http://llvm.lyngvig.org/Articles/Mapping-High-Level-Constructs-to-LLVM-IR
    class Program
    {
        static string exeDir;
        static Program()
        {
            exeDir = AppContext.BaseDirectory;
        }
        public static string RelDir(string dir)
        {
            string result = Path.Combine(Program.exeDir, dir);
            return result;
        }

        static void TestCommandLineParser()
        {
            static void Test(string[] args)
            {
                CommandLine.Parser.Default.ParseArguments<CompilerOptionsBuild>(args)
                .WithParsed<CompilerOptionsBuild>(co =>
                {
                    Console.WriteLine($"success: {co}");
                }).WithNotParsed(err =>
                {
                    foreach (var e in err)
                    {
                        Console.WriteLine($"error: {e}");
                    }
                });
            }
            var arg_0 = new string[] { "-d", "nase.prag" };
            Test(arg_0);

            var arg_1 = new string[] { "-d", "--generate-debug-info", "nase.prag" };
            Test(arg_1);

            var arg_2 = new string[] { };
            Test(arg_2);

            var arg_3 = new string[] { "--help" };
            Test(arg_3);

            var arg_4 = new string[] { "-O3", "nase.prag" };
            Test(arg_4);

            var arg_5 = new string[] { "nase.prag", "-l", "kernel32.lib;user32.lib", "-v" };
            Test(arg_5);

            var arg_6 = @"-v -d c:\Projects\dotnet\PragmaScript\publish\current\samples\editor\edit.prag".Split();
            Test(arg_6);
        }

        static void CreateNewProject(CompilerOptionsNew co)
        {
            void CopyFromTemplate(string srcFilename, string destDir = "", params (string, string)[] replacements)
            {
                var src = Path.Combine(@"..\template\", srcFilename);
                Debug.Assert(File.Exists(src), srcFilename);
                var txt = File.ReadAllText(src);
                var dest = Path.Combine(destDir, Path.GetFileName(src));
                if (replacements != null && replacements.Length > 0)
                {
                    foreach (var (oldStr, newStr) in replacements)
                    {
                        txt = txt.Replace(oldStr, newStr);
                    }
                }
                if (!File.Exists(dest))
                {
                    File.WriteAllText(dest, txt);
                }
            }

            var src = RelDir(@"..\include\system");
            var dest = "system";
            if (!Directory.Exists(dest))
            {
                Directory.CreateDirectory(dest);
            }
            if (!Directory.Exists(src))
            {
                System.Console.WriteLine($"Could not find system directory at \"{Path.GetFullPath(src)}\". Aborting...");
                return;
            }
            foreach (var f in Directory.GetFiles(src))
            {
                var destPath = Path.Combine(dest, Path.GetFileName(f));
                if (!File.Exists(destPath))
                {
                    File.Copy(f, destPath, overwrite: false);
                }
            }
            CopyFromTemplate("main.prag");

            if (co.VSCode)
            {
                if (!Directory.Exists(".vscode"))
                {
                    Directory.CreateDirectory(".vscode");
                }
                CopyFromTemplate(@"launch.json", ".vscode");
                CopyFromTemplate(@"settings.json", ".vscode");
                CopyFromTemplate(@"tasks.json", ".vscode");
                var ps = new ProcessStartInfo("code", ".") { UseShellExecute = true };
                Process.Start(ps);
            }
            else
            {
                var ps = new ProcessStartInfo("pragma_edit", ".") { UseShellExecute = true };
                Process.Start(ps);
            }
        }

        static void Main(string[] args)
        {
            Compiler compiler = new Compiler();

            StringWriter helpWriter = new StringWriter();
            CompilerOptionsBuild coBuild = null;
            CompilerOptionsNew coNew = null;
            var parserResult = (new CommandLine.Parser(with => with.HelpWriter = helpWriter))
                .ParseArguments<CompilerOptionsBuild, CompilerOptionsNew>(args);
            parserResult
            .WithParsed<CompilerOptionsBuild>(r => coBuild = r)
            .WithParsed<CompilerOptionsNew>(r => coNew = r)
            .WithNotParsed(err =>
            {
                if (!err.IsVersion())
                {
                    // Console.WriteLine(helpWriter.ToString());

                    var helpText = CommandLine.Text.HelpText.AutoBuild(parserResult, h =>
                                    {
                                        h.AdditionalNewLineAfterOption = false;
                                        h.Copyright = "Copyright (c) 2021 pragmascript@googlemail.com";
                                        return h;
                                    }, e => e, verbsIndex: true);
                    var helpStr = helpText.ToString().Replace("-O 3", "-O3");
                    System.Console.WriteLine(helpStr);
                }
                else
                {
                    var versionText = CommandLine.Text.HeadingInfo.Default.ToString();
                    System.Console.WriteLine($"version {versionText}");
                }
            });

#if false
            coBuild = new CompilerOptionsBuild();
            coBuild.debug = true;
            var programDir = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"../../publish/current/samples"));
            System.IO.Directory.SetCurrentDirectory(programDir);
            coBuild.inputFilename = Path.Combine(programDir, "basics", "hello_world.prag");
            coBuild.libs = new List<string>();
            coBuild.lib_path = new List<string>();
#else
            if (coNew != null)
            {
                CreateNewProject(coNew);
                return;
            }

            if (coBuild == null)
            {
                return;
            }
#endif
            coBuild.libs = new List<string>(coBuild.libs);
            coBuild.lib_path = new List<string>(coBuild.lib_path);

            try
            {
                compiler.Compile(coBuild);
            }
            catch (Exception e)
            {
                if (e is CompilerError || e is LexerError)
                {
                    CompilerMessage(e.Message, CompilerMessageType.Error);
                }
                else
                {
                    Debugger.Launch();
                    throw e;
                }
            }
        }


        public static void CompilerMessage(string message, CompilerMessageType type)
        {
            bool shouldPrint = CompilerOptionsBuild._i.verbose;
            switch (type)
            {
                case CompilerMessageType.Warning:
                case CompilerMessageType.Error:
                    shouldPrint = true;
                    break;
            }
            if (shouldPrint)
            {
                Console.WriteLine(message);
            }
        }
    }
}

