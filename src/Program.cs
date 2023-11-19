using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Diagnostics;

using static PragmaScript.AST;

using System.Web;
using System.Reflection;

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
        static void CreateNewProject(CompilerOptionsNew co)
        {
            static void CopyFromTemplate(string srcFilename, string destDir = "", params (string oldStr, string newStr)[] replacements)
            {
                var src = RelDir(Path.Combine(@"..\template\", srcFilename));
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
            if (co.CopySystemIncludes)
            {
                var src = RelDir("..\\include\\system");
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
            }
            CopyFromTemplate("main.prag");
            if (co.VSCode)
            {
                if (!Directory.Exists(".vscode"))
                {
                    Directory.CreateDirectory(".vscode");
                }
                CopyFromTemplate(@"launch.json", ".vscode");
                var includeDirEscaped = HttpUtility.JavaScriptStringEncode(Path.GetFullPath(RelDir("..\\include")));
                CopyFromTemplate(@"settings.json", ".vscode", ("$$includeDir$$", $"\"pragma.includeDirectory\": \"{includeDirEscaped}\""));
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


        static string GetVersionString()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyName = assembly.GetName();
            var version = assemblyName.Version;
            return version.ToString();
        }
        
        static void Main(string[] args)
        {
            Compiler compiler = new Compiler();

            StringWriter helpWriter = new StringWriter();
            
            CompilerOptionsBuild coBuild = null;
            CompilerOptionsNew coNew = null;

            var verbs = new List<CommandLineVerb>();

            var buildVerb = CompilerVerbs.CreateBuildVerb().Init();
            var newVerb = CompilerVerbs.CreateNewVerb().Init();
            verbs.Add(buildVerb);
            verbs.Add(newVerb);
            
            CommandLineParser commandLineParser = new CommandLineParser("pragma", GetVersionString(), verbs);
            var parsed = commandLineParser.Parse(args);

            if (parsed)
            {
                if (commandLineParser.activeVerb == buildVerb)
                {
                    coBuild = new CompilerOptionsBuild(buildVerb);
                }
                if (commandLineParser.activeVerb == newVerb)
                {
                    coNew = new CompilerOptionsNew(newVerb);
                }
            }
            // var parserResult = (new CommandLine.Parser(with => with.HelpWriter = helpWriter))
            //     .ParseArguments<CompilerOptionsBuild, CompilerOptionsNew>(args);
            // parserResult
            // .WithParsed<CompilerOptionsBuild>(r => coBuild = r)
            // .WithParsed<CompilerOptionsNew>(r => coNew = r)
            // .WithNotParsed(err =>
            // {
            //     if (!err.IsVersion())
            //     {
            //         // Console.WriteLine(helpWriter.ToString());

            //         var helpText = CommandLine.Text.HelpText.AutoBuild(parserResult, h =>
            //                         {
            //                             h.AdditionalNewLineAfterOption = false;
            //                             h.Copyright = "Copyright (c) 2023 pragmascript@googlemail.com";
            //                             return h;
            //                         }, e => e, verbsIndex: true);
            //         var helpStr = helpText.ToString().Replace("-O 3", "-O3");
            //         System.Console.WriteLine(helpStr);
            //     }
            //     else
            //     {
            //         var versionText = CommandLine.Text.HeadingInfo.Default.ToString();
            //         System.Console.WriteLine($"version {versionText}");
            //     }
            // });

#if false
            coBuild = new CompilerOptionsBuild();
            coBuild.debug = true;
            coBuild.inputFilename = @"g:\projects\pragma\test\main.prag";
            coBuild.libs = new List<string>();
            coBuild.lib_path = new List<string>();
            coBuild.include_dirs = new List<string>();
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
            coBuild.include_dirs = new List<string>(coBuild.include_dirs);
            
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
                    throw;
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

