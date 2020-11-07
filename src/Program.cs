using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Diagnostics;

using static PragmaScript.AST;


namespace PragmaScript
{
    public enum CompilerMessageType
    {
        Warning,
        Error,
        Info,
        Timing
    }

    class Options
    {
    }

    // http://llvm.lyngvig.org/Articles/Mapping-High-Level-Constructs-to-LLVM-IR
    class Program
    {

        static void Main(string[] args)
        {
            Compiler compiler = new Compiler();
            var co = new CompilerOptions();
            parseARGS(args);

#if true
            CompilerOptions._i.debug = true;
            var programDir = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"../publish/current/samples"));
            System.IO.Directory.SetCurrentDirectory(programDir);

            // CompilerOptions._i.buildExecuteable = false;
            // CompilerOptions._i.inputFilename = Path.Combine(programDir, "preamble.prag");
            // CompilerOptions._i.inputFilename = Path.Combine(programDir, "smallpt", "smallpt_win.prag");
            // CompilerOptions._i.inputFilename = Path.Combine(programDir, "handmade", "handmade.prag");
            // CompilerOptions._i.inputFilename = Path.Combine(programDir, "handmade", "win32_handmade.prag");
            // CompilerOptions._i.inputFilename = Path.Combine(programDir, "test", "array.prag");
            
            
            // CompilerOptions._i.inputFilename = Path.Combine(programDir, "basics", "hello_world.prag");
            

            
            // CompilerOptions._i.inputFilename = Path.Combine(programDir, "basics", "nintendo", "nintendo.prag");
            // CompilerOptions._i.inputFilename = Path.Combine(programDir, "basics", "hello_world.prag");
            CompilerOptions._i.inputFilename = Path.Combine(programDir, "editor", "editor_font.prag");
            // CompilerOptions._i.inputFilename = Path.Combine(programDir, "ld46", "ld.prag");
            // CompilerOptions._i.inputFilename = Path.Combine("test", "bugs.prag");
            // CompilerOptions._i.inputFilename = Path.Combine(programDir, "wasapi", "wasapi.prag");
            // CompilerOptions._i.inputFilename = Path.Combine(programDir, "raytracer", "raytracer.prag");
            // CompilerOptions._i.inputFilename = Path.Combine(programDir,  "work_queue.prag");
            // CompilerOptions._i.inputFilename = Path.Combine(programDir, "neural", "neural.prag");
            // Console.WriteLine(CompilerOptions._i.inputFilename);
#endif
            if (CompilerOptions._i.inputFilename == null)
            {
                Console.WriteLine("Input file name missing!");
                return;
            }

            // try
            {
                compiler.Compile(co);
            }
            // catch (Exception e)
            // {
            //     if (e is CompilerError || e is LexerError) 
            //     {
            //         CompilerMessage(e.Message, CompilerMessageType.Error);
            //     }
            //     else
            //     {
            //         throw e;
            //     }
            // }
            
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
            Console.WriteLine("    -O <filename>     set output filename");
            Console.WriteLine("    -D                build in debug mode");
            Console.WriteLine("    -O0               turn off optimizations");
            Console.WriteLine("    -OX               turn on optimization level X in [1..3]");
            Console.WriteLine("    -R                run program after compilation");
            Console.WriteLine("    -ASM              output generated assembly");
            Console.WriteLine("    -LL               output LLVM IL");
            Console.WriteLine("    -N <projectname>  [template] create new project with name and optional template");
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
            if (shouldPrint) {
                Console.WriteLine(message);
            }
        }

        static void parseARGS(string[] args)
        {
            for (int i = 0; i < args.Length; ++i)
            {
                var arg = args[i];
                if (arg.TrimStart().StartsWith("-"))
                {
                    var x = arg.TrimStart().Remove(0, 1);
                    x = x.ToUpperInvariant();
                    switch (x)
                    {
                        case "D":
                        case "DEGUG":
                            CompilerOptions._i.debug = true;
                            break;
                        case "ASM":
                            CompilerOptions._i.asm = true;
                            break;
                        case "LL":
                            CompilerOptions._i.ll = true;
                            break;
                        case "O0":
                            CompilerOptions._i.optimizationLevel = 0;
                            break;
                        case "O1":
                            CompilerOptions._i.optimizationLevel = 1;
                            break;
                        case "O2":
                            CompilerOptions._i.optimizationLevel = 2;
                            break;
                        case "O3":
                            CompilerOptions._i.optimizationLevel = 3;
                            break;
                        case "HELP":
                        case "-HELP":
                        case "H":
                            printHelp();
                            Environment.Exit(0);
                            break;
                        case "N":
                            createProject();
                            Environment.Exit(0);
                            break;
                        case "R":
                        case "RUN":
                            CompilerOptions._i.runAfterCompile = true;
                            break;
                        case "DLL":
                            CompilerOptions._i.dll = true;
                            break;
                        case "O":
                        case "OUTPUT":
                            CompilerOptions._i.output = args[++i];
                            break;
                        case "V":
                        case "VERBOSE":
                            CompilerOptions._i.verbose = true;
                            break;
                        default:
                            System.Console.WriteLine(("Unknown command line option"));
                            break;
                    }
                }
                else
                {
                    CompilerOptions._i.inputFilename = arg;
                }
            }
        }
    }
}

