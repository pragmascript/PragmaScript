using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Diagnostics;

using static PragmaScript.AST;

using CommandLine;



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

        static void TestCommandLineParser()
        {
            static void Test(string[] args)
            {
                CommandLine.Parser.Default.ParseArguments<CompilerOptions>(args)
                .WithParsed<CompilerOptions>(co =>
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


        static void Main(string[] args)
        {
            Compiler compiler = new Compiler();

            CompilerOptions co = null;
            CommandLine.Parser.Default.ParseArguments<CompilerOptions>(args).WithParsed(result =>
            {
                co = result;
            });

            if (co == null)
            {
                return;
            }

            co.libs = new List<string>(co.libs);
            co.lib_path = new List<string>(co.lib_path);

#if False
            CompilerOptions._i.debug = true;
            var programDir = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"../publish/current/samples"));
            System.IO.Directory.SetCurrentDirectory(programDir);

            // CompilerOptions._i.buildExecuteable  = false;
            // CompilerOptions._i.inputFilename = Path.Combine(programDir, "preamble.prag");
            // CompilerOptions._i.inputFilename = Path.Combine(programDir, "smallpt", "smallpt_win.prag");
            // CompilerOptions._i.inputFilename = Path.Combine(programDir, "handmade", "handmade.prag");
            // CompilerOptions._i.inputFilename = Path.Combine(programDir, "handmade", "win32_handmade.prag");
            // CompilerOptions._i.inputFilename = Path.Combine(programDir, "test", "array.prag");


            // CompilerOptions._i.inputFilename = Path.Combine(programDir, "basics", "hello_world.prag");



            // CompilerOptions._i.inputFilename = Path.Combine(programDir, "basics", "nintendo", "nintendo.prag");
            // CompilerOptions._i.inputFilename = Path.Combine(programDir, "basics", "hello_world.prag");
            // CompilerOptions._i.inputFilename = Path.Combine(programDir, "editor", "edit.prag");
            // CompilerOptions._i.inputFilename = Path.Combine(programDir, "ld46", "ld.prag");
            CompilerOptions._i.inputFilename = Path.Combine("test", "bugs.prag");
            // CompilerOptions._i.inputFilename = Path.Combine(programDir, "wasapi", "wasapi.prag");
            // CompilerOptions._i.inputFilename = Path.Combine(programDir, "raytracer", "raytracer.prag");
            // CompilerOptions._i.inputFilename = Path.Combine(programDir,  "work_queue.prag");
            // CompilerOptions._i.inputFilename = Path.Combine(programDir, "neural", "neural.prag");
            // Console.WriteLine(CompilerOptions._i.inputFilename);
#endif
            try
            {
                compiler.Compile(co);
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

        static void createProject()
        {
            Console.WriteLine("Enter name: ");
            var name = Console.ReadLine();
        }


        public static void CompilerMessage(string message, CompilerMessageType type)
        {
            bool shouldPrint = CompilerOptions._i.verbose;
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

