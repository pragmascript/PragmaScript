
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PragmaScript
{

    public class CommandLineParser
    {
        string exe;
        string version;
        List<CommandLineVerb> verbs;
        internal CommandLineVerb activeVerb;
        public CommandLineParser(string exe, string version, List<CommandLineVerb> verbs)
        {
            this.exe = exe;
            this.version = version;
            this.verbs = verbs;
        }

        void PrintUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine($"  {exe} [options] [command] [command-options]");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("  -h, --help    Display help.");
            Console.WriteLine("  --version     Display version information.");
            Console.WriteLine();
            Console.WriteLine("Commands:");
            foreach (var verb in verbs)
            {
                Console.WriteLine($"  {verb.name,-10}{verb.helpText}");
            }
            Console.WriteLine();
            Console.WriteLine("To display additional help for a specific command run:");
            Console.WriteLine($"  {exe} [command] --help");
        }
        void PrintHelp()
        {
            PrintUsage();
        }
        
        void PrintHelp(CommandLineVerb verb)
        {
            Console.WriteLine("Description:");
            Console.WriteLine($"  {verb.helpText}");
            Console.WriteLine();
            string ValuesList()
            {
                if (verb.values.Count > 0)
                {
                    return string.Join(" ", verb.values.Select(v => v.metaName)).Trim() + " ";
                }
                else
                {
                    return "";
                }
            }
            Console.WriteLine("Usage:");
            Console.WriteLine($"  {exe} {verb.name} {ValuesList()}[options]");
            Console.WriteLine();
            Console.WriteLine("Arguments:");
            foreach (var v in verb.values)
            {
                Console.WriteLine($"  {v.metaName,-30}{v.helpText}");
            }
            Console.WriteLine();
            Console.WriteLine("Options:");

            string OptionShortName(CommandLineOption option)
            {
                if (!string.IsNullOrEmpty(option.shortName))
                {
                    return $", -{option.shortName}";
                }
                else
                {
                    return "";
                }
            }
            foreach (var o in verb.options)
            {
                Console.WriteLine($"  --{o.longName + OptionShortName(o),-30}{o.helpText}");
            }

        }
        void VerbNotFound(string name) {
            Console.WriteLine($"Unknown command: \"{name}\"!");
            Console.WriteLine();
            PrintUsage();
        }

        void OptionNotFound(string name) {
            Console.WriteLine($"Unknown option: \"{name}\"!");
            Console.WriteLine();
            PrintUsage();
        }

        void MissingOptionArgument(string name)
        {
            Console.WriteLine($"Missign argument: \"{name}\"!");
            Console.WriteLine();
        }

        void ExpectedIntOptionArgument(string name)
        {
            Console.WriteLine($"Option \"{name}\" must be followed by an integer argument!");
            Console.WriteLine();
        }

        (string, int) Next(string[] args, int pos) 
        {
            if (pos >= args.Length)
            {
                return (null, pos);
            }
            else
            {
                return (args[pos], pos + 1);
            }
        }

        bool CurrentIsHelp(string[] args, int pos)
        {
            if (pos >= args.Length)
            {
                return false;
            }
            return IsHelp(args[pos]);
        }


        bool ParseVerb(CommandLineVerb verb, int pos, string[] args)
        {
            bool parseStringValue(out string dest, string name)
            {
                (dest, pos) = Next(args, pos);
                if (dest == null)
                {
                    MissingOptionArgument(name);
                    return false;
                }
                return true;
            }
            bool parseIntValue(out int dest, string name)
            {
                (var s, pos) = Next(args, pos);
                if (s == null)
                {
                    MissingOptionArgument(name);
                    dest = 0;
                    return false;
                }
                if (int.TryParse(s, out dest) == false)
                {
                    // TODO(pragma): check for allowed range?
                    ExpectedIntOptionArgument(name);
                    return false;
                }
                return true;
            }
            bool parseListValue(out List<string> dest, string name, string separator)
            {
                (var s, pos) = Next(args, pos);
                if (s == null)
                {
                    MissingOptionArgument(name);
                    dest = new List<string>();
                    return false;
                }
                Debug.Assert(!string.IsNullOrEmpty(separator));
                var pieces = s.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                Debug.Assert(pieces.Length >= 1);
                dest = new List<string>(pieces);
                return true;
            }

            for (var valueIdx = 0; valueIdx < verb.values.Count; ++valueIdx)
            {
                if (CurrentIsHelp(args, pos))
                {
                    PrintHelp(verb);
                    return false;
                }
                var value = verb.values[valueIdx];
                Debug.Assert(value.position == valueIdx);
                switch (value.valueType)
                {
                    case CommandLineValue.ValueType.StringValue:
                        if (!parseStringValue(out value.stringValue, value.metaName))
                        {
                            return false;
                        }
                        break;
                    case CommandLineValue.ValueType.IntValue:
                        if (!parseIntValue(out value.intValue, value.metaName))
                        {
                            return false;
                        }
                        break;
                    case CommandLineValue.ValueType.ListValue:
                        if (!parseListValue(out value.listValue, value.metaName, value.separator))
                        {
                            return false;
                        }
                        break;
                }
            }
            while (true)
            {
                if (CurrentIsHelp(args, pos))
                {
                    PrintHelp(verb);
                    return false;
                }
                (var current, pos) = Next(args, pos);
                if (current == null) 
                {
                    return true;
                }
                if (current.StartsWith('-')) {
                    CommandLineOption option;
                    if (current.StartsWith("--"))
                    {
                        var longName = current[2..];
                        option = verb.options.Where(o => o.longName == longName).FirstOrDefault();
                    }
                    else
                    {
                        var shortName = current[1..];
                        option = verb.options.Where(o => o.shortName == shortName).FirstOrDefault();
                    }
                    if (option == null)
                    {
                        OptionNotFound(current);
                        return false;
                    }
                    switch (option.optionType)
                    {
                        case CommandLineOption.OptionType.Switch:
                            option.switchValue = true;
                            break;
                        case CommandLineOption.OptionType.StringValue:
                            if (!parseStringValue(out option.stringValue, current))
                            {
                                return false;
                            }
                            break;
                        case CommandLineOption.OptionType.IntValue:
                            if (!parseIntValue(out option.intValue, current))
                            {
                                return false;
                            }
                            break;
                        case CommandLineOption.OptionType.ListValue:
                            if (!parseListValue(out option.listValue, current, option.separator))
                            {
                                return false;
                            }
                            break;
                    }
                }
            }
        }

        bool IsHelp(string s)
        {
            return s == "-h" || s == "help" || s == "--help" || s == "-help";
        }

        public bool Parse(string[] args)
        {
            activeVerb = null;
            if (args.Length == 0)
            {
                PrintUsage();
                return false;
            }
            int pos = 0;
            var verbName = args[pos++];
            if (IsHelp(verbName)) 
            {
                PrintHelp();
                return false;
            }
            if (verbName == "--version")
            {
                Console.WriteLine(version);
                return false;
            }
            var verb = verbs.Where(v => v.name == verbName).FirstOrDefault();
            if (verb == null)
            {
                VerbNotFound(verbName);
                return false;
            }
            if (!ParseVerb(verb, pos, args) )
            {
                return false;
            }

            activeVerb = verb;
            return true;
        }
    }

    public class CommandLineVerb
    {
        public string name;
        public string helpText;
        public List<CommandLineValue> values = new List<CommandLineValue>();
        public List<CommandLineOption> options = new List<CommandLineOption>();

        public Dictionary<string, CommandLineValue> valueLookup = new Dictionary<string, CommandLineValue>();
        public Dictionary<string, CommandLineOption> optionLookup = new Dictionary<string, CommandLineOption>();

        public CommandLineVerb Init()
        {
            foreach (var value in values)
            {
                valueLookup.Add(value.metaName, value);
            }
            foreach (var option in options)
            {
                optionLookup.Add(option.longName, option);
            }
            return this;
        }
    }

    public class CommandLineOption
    {
        public enum OptionType
        {
            Switch, StringValue, IntValue, ListValue,            
        }
        public OptionType optionType;
        public string longName;
        public string shortName;
        public string helpText;
        public string defaultValue;
        public string separator;

        public bool switchValue;
        public string stringValue;
        public int intValue;
        public List<string> listValue = new List<string>();

        public CommandLineOption Init()
        {
            if (defaultValue != null)
            {
                switch (optionType)
                {
                    case OptionType.Switch:
                        Debug.Assert(false);
                        break;
                    case OptionType.StringValue:
                        stringValue = defaultValue;
                        break;
                    case OptionType.IntValue:
                        intValue = int.Parse(defaultValue);
                        break;
                    case OptionType.ListValue:
                        Debug.Assert(!string.IsNullOrEmpty(separator));
                        listValue = new List<string>(defaultValue.Split(separator, StringSplitOptions.RemoveEmptyEntries));
                        break;
                }
            }
            return this;
        }
    }

    public class CommandLineValue
    {
        public enum ValueType
        {
            StringValue, IntValue, ListValue,            
        }
        public ValueType valueType;
        public int position;
        public string metaName;
        public string helpText;
        public string separator;

        
        public string stringValue;
        public int intValue;
        public List<string> listValue = new List<string>();
    }

    public static class CompilerVerbs
    {
        public static CommandLineVerb CreateBuildVerb()
        {
            var result = new CommandLineVerb
            {
                name = "build",
                helpText = "Compile and build the project.",
            };
            var v = new CommandLineValue { 
                valueType = CommandLineValue.ValueType.StringValue,
                position = 0,
                metaName = "input-filename",
                helpText = "Source filename to compile.",
            };
            result.values.Add(v);
            
            CommandLineOption o;
            o = new CommandLineOption {
                optionType = CommandLineOption.OptionType.Switch,
                longName = "debug",
                shortName = "d",
                helpText = "Compile with DEBUG preprocessor define.",
            }.Init();
            result.options.Add(o);

            o = new CommandLineOption {
                optionType = CommandLineOption.OptionType.Switch,
                longName = "generate-debug-info",
                shortName = "g",
                helpText = "Generate debugging information.",
            }.Init();
            result.options.Add(o);

            o = new CommandLineOption {
                optionType = CommandLineOption.OptionType.StringValue,
                longName = "output-filename",
                shortName = "o",
                helpText = "Filename of the output executeable.",
                defaultValue = "output.exe",
            }.Init();
            result.options.Add(o);

            o = new CommandLineOption {
                optionType = CommandLineOption.OptionType.IntValue,
                longName = "optimization-level",
                shortName = "O",
                helpText = "Backend optimization level between 0 and 4.",
                defaultValue = "0",
            }.Init();
            result.options.Add(o);

            o = new CommandLineOption {
                optionType = CommandLineOption.OptionType.Switch,
                longName = "run",
                shortName = "r",
                helpText = "Run executeable after compilation.",
            }.Init();
            result.options.Add(o);

            o = new CommandLineOption {
                optionType = CommandLineOption.OptionType.Switch,
                longName = "emit-asm",
                helpText = "Output generated assembly.",
            }.Init();
            result.options.Add(o);

            o = new CommandLineOption {
                optionType = CommandLineOption.OptionType.Switch,
                longName = "emit-llvm",
                helpText = "Output generated LLVM IR.",
            }.Init();
            result.options.Add(o);
                
            o = new CommandLineOption {
                optionType = CommandLineOption.OptionType.ListValue,
                longName = "libs",
                shortName = "l",
                helpText = "';'-separated list of static libraries to link against.",
                separator = ";",
            }.Init();
            result.options.Add(o);

            o = new CommandLineOption {
                optionType = CommandLineOption.OptionType.ListValue,
                longName = "lib-dirs",
                shortName = "L",
                helpText = "';'-separated list of library search directories.",
                separator = ";",
            }.Init();
            result.options.Add(o);

            o = new CommandLineOption {
                optionType = CommandLineOption.OptionType.ListValue,
                longName = "include-dirs",
                shortName = "I",
                helpText = "';'-separated list of include search directories.",
                separator = ";",
            }.Init();
            result.options.Add(o);

            o = new CommandLineOption {
                optionType = CommandLineOption.OptionType.Switch,
                longName = "shared-library",
                helpText = "Compile output binary as a shared library.",
            }.Init();
            result.options.Add(o);

            o = new CommandLineOption {
                optionType = CommandLineOption.OptionType.Switch,
                longName = "verbose",
                shortName = "v",
                helpText = "Compiler output messages set to verbose."
            }.Init();
            result.options.Add(o);

            o = new CommandLineOption {
                optionType = CommandLineOption.OptionType.Switch,
                longName = "dry-run",
                helpText = "Compile and output errors only; no executeable will be generated.",
            }.Init();
            result.options.Add(o);

            return result;
        }

        public static CommandLineVerb CreateNewVerb() 
        {
            var result = new CommandLineVerb
            {
                name = "new",
                helpText = "Creates a new pragma project in the current diretory.",
            };
            CommandLineOption o;

            o = new CommandLineOption {
                optionType = CommandLineOption.OptionType.Switch,
                longName = "vscode",
                helpText = "Generate default VSCode Tasks, Launch and Settings files.",
            }.Init();
            result.options.Add(o);

            o = new CommandLineOption {
                optionType = CommandLineOption.OptionType.Switch,
                longName = "copy-system-includes",
                helpText = "Copies system includes to project directory.",
            }.Init();
            result.options.Add(o);

            return result;
        }
    }

    public class CompilerOptionsBuild
    {
        public static CompilerOptionsBuild _i;

        public string inputFilename { get; set; }
        public bool debug { get; set; }
        public bool debugInfo { get; set; }
        public string output { get; set; }
        public int optimizationLevel { get; set; }

        public string cpu = "native";
        public bool runAfterCompile { get; set; }

        public bool asm { get; set; }
        public bool ll { get; set; }
        public bool bc = false;

        public List<string> libs { get; set; }
        public List<string> lib_path { get; set; }
        public List<string> include_dirs { get; set; }

        public bool dll { get; set; }

        public bool verbose { get; set; }

        public bool dryRun { get; set; }

        public bool buildExecuteable { get { return !dryRun; } set { dryRun = !value; } }
        public bool useFastMath
        {
            get { return optimizationLevel > 3; }
        }

        internal PragmaScript.AST.FunctionDefinition entry;
        public CompilerOptionsBuild()
        {
            _i = this;
        }

        public CompilerOptionsBuild(CommandLineVerb verb)
        {
            _i = this;
            inputFilename = verb.valueLookup["input-filename"].stringValue;
            debug = verb.optionLookup["debug"].switchValue;
            debugInfo = verb.optionLookup["generate-debug-info"].switchValue;
            output = verb.optionLookup["output-filename"].stringValue;
            optimizationLevel = verb.optionLookup["optimization-level"].intValue;
            runAfterCompile = verb.optionLookup["run"].switchValue;
            asm = verb.optionLookup["emit-asm"].switchValue;
            ll = verb.optionLookup["emit-llvm"].switchValue;
            libs = verb.optionLookup["libs"].listValue;
            lib_path = verb.optionLookup["lib-dirs"].listValue;
            include_dirs = verb.optionLookup["include-dirs"].listValue;
            dll = verb.optionLookup["shared-library"].switchValue;
            verbose = verb.optionLookup["verbose"].switchValue;
            dryRun = verb.optionLookup["dry-run"].switchValue;
        }

        // [CommandLine.Text.Usage(ApplicationAlias = "pragma")]
        // public static IEnumerable<CommandLine.Text.Example> Examples
        // {
        //     get
        //     {
        //         yield return new CommandLine.Text.Example("Debug build \"hello.prag\" output \"hello.exe\"", new CommandLine.UnParserSettings() { PreferShortName = true },
        //             new CompilerOptionsBuild { inputFilename = "hello.prag", output = "hello.exe", debug = true, debugInfo = true });
        //         yield return new CommandLine.Text.Example("Compile optimized release build and run", new CommandLine.UnParserSettings() { PreferShortName = true },
        //             new CompilerOptionsBuild { inputFilename = "hello.prag", output = "hello.exe", optimizationLevel = 3, runAfterCompile = true, libs = new string[] { "user32.lib", "libopenlibm.a" } });
        //     }
        // }
    }


    public class CompilerOptionsNew
    {
        public enum NewProjectType { HelloWorld, Empty }
        public bool VSCode { get; set; } = false;
        public bool CopySystemIncludes { get; set; } = false;

        public CompilerOptionsNew(CommandLineVerb verb)
        {
            VSCode = verb.optionLookup["vscode"].switchValue;
            CopySystemIncludes = verb.optionLookup["copy-system-includes"].switchValue;
        }
    }
}