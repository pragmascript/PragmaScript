using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

using static PragmaScript.SSA;
using static PragmaScript.SSA.Const;

namespace PragmaScript
{
    public enum Platform
    {
        WindowsX64, LinuxX64
    }
    partial class Backend
    {
        TypeChecker typeChecker;
        Dictionary<Scope.VariableDefinition, Value> variables = new Dictionary<Scope.VariableDefinition, Value>();
        Stack<Value> valueStack = new Stack<Value>();
        Dictionary<string, Value> stringTable = new Dictionary<string, Value>();

        Dictionary<FunctionAttribs, int> functionAttribs;
        string exeDir;
        Platform platform;

        public Backend(TypeChecker typeChecker, Platform platform)
        {
            functionAttribs = new Dictionary<FunctionAttribs, int>();
            this.typeChecker = typeChecker;
            mod = new Module();
            builder = new Builder(mod);
            exeDir = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            functionAttribs.Add(FunctionAttribs.nounwind, 0);
            this.platform = platform;
        }


        public void AOT()
        {
            var timer = new Stopwatch();
            timer.Start();
            var ll = emitLL();


            int optLevel = CompilerOptions._i.optimizationLevel;
            Debug.Assert(optLevel >= 0 && optLevel <= 4);
            var arch = "x86-64";

            var outputDir = Path.GetDirectoryName(CompilerOptions._i.inputFilename);
            var outputTempDir = Path.Combine(outputDir, "obj");
            var outputTemp = Path.Combine(outputTempDir, Path.GetFileNameWithoutExtension(CompilerOptions._i.output));
            var outputBinDir = Path.Combine(outputDir, "bin");
            var outputBin = Path.Combine(outputBinDir, Path.GetFileNameWithoutExtension(CompilerOptions._i.output));

            Func<string, string> oxt = (ext) => "\"" + outputTemp + ext + "\"";
            Func<string, string> ox = (ext) => "\"" + outputBin + ext + "\"";

            Directory.CreateDirectory(outputTempDir);
            Directory.CreateDirectory(outputBinDir);

            if (CompilerOptions._i.ll)
            {
                File.WriteAllText(outputTemp + ".ll", ll);
            }
            var bufferSize = 100 * 1024 * 1024;
            var buffer = new byte[bufferSize];


            timer.Stop();
            Program.CompilerMessage($"backend preparation time: {timer.ElapsedMilliseconds}ms", CompilerMessageType.Timing);
            timer.Reset();
            timer.Start();
            var mcpu = CompilerOptions._i.cpu.ToLower();
            bool error = false;

            bool use_fast_flags = false;
            if (optLevel > 3)
            {
                optLevel = 3;
                use_fast_flags = true;
            }


            if (optLevel > 0)
            {
                var opt_string = !use_fast_flags ? optLevel.ToString() : "fast";
                Program.CompilerMessage($"optimizer... (O{opt_string})", CompilerMessageType.Info);
                var optProcess = new Process();

                switch (platform)
                {
                    case Platform.WindowsX64:
                        optProcess.StartInfo.FileName = RelDir(@"external/opt.exe");
                        break;
                    case Platform.LinuxX64:
                        optProcess.StartInfo.FileName = RelDir(@"external/opt");
                        break;
                }


                var fast_flags = "";
                if (use_fast_flags)
                {
                    fast_flags = "-enable-no-infs-fp-math -enable-no-nans-fp-math -enable-no-signed-zeros-fp-math -enable-unsafe-fp-math -enable-no-trapping-fp-math -fp-contract=fast ";
                }
                if (CompilerOptions._i.ll)
                {
                    optProcess.StartInfo.Arguments = $"{oxt(".ll")} -O{optLevel} {fast_flags}-march={arch} -mcpu={mcpu} -S -o {oxt("_opt.ll")}";
                    optProcess.StartInfo.RedirectStandardInput = false;
                    optProcess.StartInfo.RedirectStandardOutput = false;
                    optProcess.StartInfo.UseShellExecute = false;
                    Program.CompilerMessage("***************************************", CompilerMessageType.Info);
                    Program.CompilerMessage($"opt: {optProcess.StartInfo.FileName}", CompilerMessageType.Info);
                    Program.CompilerMessage($"arg: {optProcess.StartInfo.Arguments}", CompilerMessageType.Info);
                    Program.CompilerMessage("***************************************", CompilerMessageType.Info);

                    optProcess.Start();
                    optProcess.WaitForExit();
                }
                else
                {
                    optProcess.StartInfo.Arguments = $"-O{optLevel} {fast_flags}-march={arch} -mcpu={mcpu} -f";
                    optProcess.StartInfo.RedirectStandardInput = true;
                    optProcess.StartInfo.RedirectStandardOutput = true;
                    optProcess.StartInfo.UseShellExecute = false;
                    Program.CompilerMessage("***************************************", CompilerMessageType.Info);
                    Program.CompilerMessage($"opt: {optProcess.StartInfo.FileName}", CompilerMessageType.Info);
                    Program.CompilerMessage($"arg: {optProcess.StartInfo.Arguments}", CompilerMessageType.Info);
                    Program.CompilerMessage("***************************************", CompilerMessageType.Info);

                    optProcess.Start();
                    var writer = optProcess.StandardInput;
                    var reader = optProcess.StandardOutput;
                    writer.Write(ll);
                    writer.Close();
                    //writer.BaseStream.Write(buffer, 0, bufferSize);
                    //writer.Close();

                    var pos = 0;
                    var count = 0;
                    while (true)
                    {
                        var bytes_read = reader.BaseStream.Read(buffer, pos, buffer.Length - count);
                        pos += bytes_read;
                        count += bytes_read;
                        if (bytes_read == 0)
                        {
                            break;
                        }
                    }
                    Debug.Assert(count < buffer.Length);
                    reader.Close();
                    bufferSize = count;
                    if (CompilerOptions._i.bc)
                    {
                        File.WriteAllBytes(oxt("_opt.bc"), buffer.Take(bufferSize).ToArray());
                    }
                    optProcess.WaitForExit();
                }
                if (optProcess.ExitCode != 0)
                {
                    error = true;
                }
                optProcess.Close();
            }
            var inp = oxt("_opt.ll");
            if (optLevel == 0)
            {
                inp = oxt(".ll");
            }
            if (!error && CompilerOptions._i.asm)
            {
                Program.CompilerMessage("assembler...(debug)", CompilerMessageType.Info);
                var llcProcess = new Process();

                switch (platform)
                {
                    case Platform.WindowsX64:
                        llcProcess.StartInfo.FileName = RelDir("external/llc.exe");
                        break;
                    case Platform.LinuxX64:
                        llcProcess.StartInfo.FileName = RelDir("external/llc");
                        break;
                }

                if (CompilerOptions._i.ll)
                {
                    llcProcess.StartInfo.Arguments = $"{inp} -O{optLevel} -march={arch} -mcpu={mcpu} -filetype=asm -o {oxt(".asm")}";
                    llcProcess.StartInfo.RedirectStandardInput = false;
                    llcProcess.StartInfo.RedirectStandardOutput = false;
                    llcProcess.StartInfo.UseShellExecute = false;
                    Program.CompilerMessage("***************************************", CompilerMessageType.Info);
                    Program.CompilerMessage($"llc: {llcProcess.StartInfo.FileName}", CompilerMessageType.Info);
                    Program.CompilerMessage($"arg: {llcProcess.StartInfo.Arguments}", CompilerMessageType.Info);
                    Program.CompilerMessage("***************************************", CompilerMessageType.Info);

                    llcProcess.Start();
                    llcProcess.WaitForExit();
                }
                else
                {
                    llcProcess.StartInfo.Arguments = $"-O{optLevel} -march={arch} -mcpu={mcpu} -filetype=asm -o {oxt(".asm")}";
                    llcProcess.StartInfo.RedirectStandardInput = true;
                    llcProcess.StartInfo.RedirectStandardOutput = false;
                    llcProcess.StartInfo.UseShellExecute = false;
                    Program.CompilerMessage("***************************************", CompilerMessageType.Info);
                    Program.CompilerMessage($"llc: {llcProcess.StartInfo.FileName}", CompilerMessageType.Info);
                    Program.CompilerMessage($"arg: {llcProcess.StartInfo.Arguments}", CompilerMessageType.Info);
                    Program.CompilerMessage("***************************************", CompilerMessageType.Info);
                    llcProcess.Start();
                    var writer = llcProcess.StandardInput;
                    if (optLevel > 0)
                    {
                        writer.BaseStream.Write(buffer, 0, bufferSize);
                    }
                    else
                    {
                        writer.Write(ll);
                    }
                    writer.Close();
                    llcProcess.WaitForExit();
                }

                if (llcProcess.ExitCode != 0)
                {
                    error = true;
                }
                llcProcess.Close();
            }
            if (!error)
            {
                Program.CompilerMessage("assembler...", CompilerMessageType.Info);
                var llcProcess = new Process();

                switch (platform)
                {
                    case Platform.WindowsX64:
                        llcProcess.StartInfo.FileName = RelDir("external/llc.exe");
                        break;
                    case Platform.LinuxX64:
                        llcProcess.StartInfo.FileName = RelDir("external/llc");
                        break;
                }

                if (CompilerOptions._i.ll)
                {
                    llcProcess.StartInfo.Arguments = $"{inp} -O{optLevel} -march={arch} -mcpu={mcpu} -filetype=obj -o {oxt(".o")}";
                    llcProcess.StartInfo.RedirectStandardInput = false;
                    llcProcess.StartInfo.RedirectStandardOutput = false;
                    llcProcess.StartInfo.UseShellExecute = false;
                    Program.CompilerMessage("***************************************", CompilerMessageType.Info);
                    Program.CompilerMessage($"llc: {llcProcess.StartInfo.FileName}", CompilerMessageType.Info);
                    Program.CompilerMessage($"arg: {llcProcess.StartInfo.Arguments}", CompilerMessageType.Info);
                    Program.CompilerMessage("***************************************", CompilerMessageType.Info);
                    llcProcess.Start();
                    llcProcess.WaitForExit();
                }
                else
                {
                    llcProcess.StartInfo.Arguments = $"-O{optLevel} -march={arch} -mcpu={mcpu} -filetype=obj -o {oxt(".o")}";
                    llcProcess.StartInfo.RedirectStandardInput = true;
                    llcProcess.StartInfo.RedirectStandardOutput = false;
                    llcProcess.StartInfo.UseShellExecute = false;
                    Program.CompilerMessage("***************************************", CompilerMessageType.Info);
                    Program.CompilerMessage($"llc: {llcProcess.StartInfo.FileName}", CompilerMessageType.Info);
                    Program.CompilerMessage($"arg: {llcProcess.StartInfo.Arguments}", CompilerMessageType.Info);
                    Program.CompilerMessage("***************************************", CompilerMessageType.Info);


                    llcProcess.Start();
                    var writer = llcProcess.StandardInput;
                    if (optLevel > 0)
                    {
                        writer.BaseStream.Write(buffer, 0, bufferSize);
                    }
                    else
                    {
                        writer.Write(ll);
                    }
                    writer.Close();
                    llcProcess.WaitForExit();
                }
                if (llcProcess.ExitCode != 0)
                {
                    error = true;
                }
                llcProcess.Close();
            }
            if (!error)
            {
                var libs = String.Join(" ", CompilerOptions._i.libs);
                var lib_path = String.Join(" /libpath:", CompilerOptions._i.lib_path.Select(s => "\"" + s + "\""));
                Program.CompilerMessage("linker...", CompilerMessageType.Info);
                var lldProcess = new Process();

                switch (platform)
                {
                    case Platform.WindowsX64:
                        {
                            lldProcess.StartInfo.FileName = RelDir("external/lld-link.exe");
                            var flags = "/entry:__init";
                            if (CompilerOptions._i.dll)
                            {
                                flags += $" /NODEFAULTLIB /dll /out:{ox(".dll")}";
                            }
                            else
                            {
                                flags += $" /NODEFAULTLIB /subsystem:CONSOLE /out:{ox(".exe")}";
                            }
                            if (CompilerOptions._i.debug)
                            {
                                flags += " /DEBUG";
                            }
                            lldProcess.StartInfo.Arguments = $"{libs} {oxt(".o")} {flags} /libpath:{lib_path}";
                        }
                        break;
                    case Platform.LinuxX64:
                        lldProcess.StartInfo.FileName = RelDir("external/ld.lld");// "/usr/bin/ld"; // RelDir("external/ld.lld");
                        lldProcess.StartInfo.Arguments = $"-o {ox("")} {oxt(".o")} -e__init";
                        break;
                }


                // Console.WriteLine($"linker: \"{lldProcess.StartInfo.Arguments}\"");
                lldProcess.StartInfo.RedirectStandardInput = false;
                lldProcess.StartInfo.RedirectStandardOutput = false;
                lldProcess.StartInfo.UseShellExecute = false;
                Program.CompilerMessage("***************************************", CompilerMessageType.Info);
                Program.CompilerMessage($"lld: {lldProcess.StartInfo.FileName}", CompilerMessageType.Info);
                Program.CompilerMessage($"arg: {lldProcess.StartInfo.Arguments}", CompilerMessageType.Info);
                Program.CompilerMessage("***************************************", CompilerMessageType.Info);
                lldProcess.Start();
                lldProcess.WaitForExit();
                if (lldProcess.ExitCode != 0)
                {
                    error = true;
                }
                lldProcess.Close();
            }

            timer.Stop();
            Program.CompilerMessage($"backend llvm time: {timer.ElapsedMilliseconds}ms", CompilerMessageType.Timing);
            timer.Reset();
            timer.Start();


            if (!error && CompilerOptions._i.runAfterCompile)
            {
                Program.CompilerMessage("running...", CompilerMessageType.Info);
                var outputProcess = new Process();
                outputProcess.StartInfo.WorkingDirectory = outputBinDir;
                switch (platform)
                {
                    case Platform.WindowsX64:
                        outputProcess.StartInfo.FileName = ox(".exe");
                        break;
                    case Platform.LinuxX64:
                        outputProcess.StartInfo.FileName = outputBin;
                        break;
                }
                outputProcess.StartInfo.Arguments = "";
                outputProcess.StartInfo.RedirectStandardInput = false;
                outputProcess.StartInfo.RedirectStandardOutput = false;
                outputProcess.StartInfo.UseShellExecute = false;
                outputProcess.Start();
                outputProcess.WaitForExit();
                if (outputProcess.ExitCode != 0)
                {
                    error = true;
                }
                outputProcess.Close();
            }


            timer.Stop();
            Program.CompilerMessage($"Run time: {timer.ElapsedMilliseconds} ms", CompilerMessageType.Timing);
            Program.CompilerMessage("done.", CompilerMessageType.Info);
        }

        public string RelDir(string dir)
        {
            string result = Path.Combine(exeDir, dir);
            return result;
        }

        static bool isConstVariableDefinition(AST.Node node)
        {
            if (node is AST.VariableDefinition vd)
            {
                return vd.variable.isConstant;
            }
            return false;
        }

        static bool isGlobalVariableDefinition(AST.Node node)
        {
            if (node is AST.VariableDefinition vd)
            {
                return vd.variable.isGlobal;
            }
            return false;
        }

        public void Visit(AST.ProgramRoot node, AST.FunctionDefinition main)
        {
            // HACK:
            AST.FileRoot merge = new AST.FileRoot(Token.Undefined, node.scope);
            merge.parent = node;
            foreach (var fr in node.files)
            {
                foreach (var decl in fr.declarations)
                {
                    merge.declarations.Add(decl);
                }
            }
            Visit(merge, main);
        }

        public void Visit(AST.FileRoot node, AST.FunctionDefinition main)
        {
            var constVariables = new List<AST.Node>();
            var enumDeclarations = new List<AST.Node>();
            var functionDefinitions = new List<AST.Node>();
            var globalVariables = new List<AST.Node>();
            var modules = new List<AST.Module>();
            var other = new List<AST.Node>();

            // visit function definitions make prototypes
            foreach (var decl in node.declarations)
            {

                if (isConstVariableDefinition(decl))
                {
                    constVariables.Add(decl);
                }
                else if (isGlobalVariableDefinition(decl))
                {
                    globalVariables.Add(decl);
                }
                else if (decl is AST.FunctionDefinition)
                {
                    functionDefinitions.Add(decl);
                    if (!(decl as AST.FunctionDefinition).external)
                    {
                        other.Add(decl);
                    }
                }
                else if (decl is AST.EnumDeclaration)
                {
                    enumDeclarations.Add(decl);
                }
                else if (decl is AST.Module ns)
                {
                    modules.Add(ns);
                }
                else
                {
                    other.Add(decl);
                }
            }

            FunctionType ft;
            if (CompilerOptions._i.dll)
            {
                ft = new FunctionType(i32_t, mm_t, i32_t, ptr_t);
            }
            else
            {
                ft = new FunctionType(void_t);
            }

            Block entry;
            Block vars;
            if (CompilerOptions._i.buildExecuteable)
            {
                var initFun = new AST.FunctionDefinition(main.token, main.scope);
                initFun.parent = node;
                initFun.body = main.body;
                initFun.funName = "__init";
                var tp = new FrontendFunctionType("__init");
                tp.returnType = FrontendType.void_;
                typeChecker.ResolveNode(initFun, tp);

                var function = builder.AddFunction(ft, initFun, "__init");
                function.internalLinkage = false;
                vars = builder.AppendBasicBlock(function, "vars");
                entry = builder.AppendBasicBlock(function, "entry");
                builder.context.SetFunctionBlocks(function, vars, entry, null);

                builder.PositionAtEnd(entry);
            }
            else
            {
                var function = builder.AddFunction(ft, debugRootNode, "__init");
                entry = builder.AppendBasicBlock(function, "entry");
                vars = builder.AppendBasicBlock(function, "vars");
                builder.context.SetFunctionBlocks(function, vars, entry, null);
                builder.PositionAtEnd(entry);
            }

            foreach (var decl in functionDefinitions)
            {
                Visit(decl as AST.FunctionDefinition, proto: true);
            }
            foreach (var decl in constVariables)
            {
                Visit(decl as AST.VariableDefinition);
            }
            foreach (var decl in enumDeclarations)
            {
                Visit(decl as AST.EnumDeclaration);
            }
            foreach (var decl in globalVariables)
            {
                Visit(decl as AST.VariableDefinition);
            }
            foreach (var ns in modules)
            {
                Visit(ns, definitions: true);
            }
            foreach (var decl in other)
            {
                Visit(decl);
            }
            foreach (var ns in modules)
            {
                Visit(ns, definitions: false);
            }

            builder.PositionAtEnd(entry);

            if (main != null)
            {
                var mf = variables[main.scope.GetVar(main.funName, main.token).First];
                builder.BuildCall(mf, node);
            }

            if (CompilerOptions._i.dll)
            {
                builder.BuildRet(one_i32_v, node);
            }
            else
            {
                if (platform == Platform.LinuxX64)
                {
                    builder.BuildEmit("call void @__exit()", node, "exit");
                }
                builder.BuildRet(void_v, node);
            }
            builder.PositionAtEnd(vars);
            builder.BuildBr(entry, node);
            // builder.PositionAtEnd(blockTemp);
        }


        // TODO(pragma): avoid calling this twice?
        public void Visit(AST.Module node, bool definitions = false)
        {
            var functionDefinitions = new List<AST.Node>();
            var constVariables = new List<AST.Node>();
            var enumDeclarations = new List<AST.Node>();
            var variables = new List<AST.Node>();
            var modules = new List<AST.Module>();
            var other = new List<AST.Node>();

            // visit function definitions make prototypes
            foreach (var decl in node.declarations)
            {
                if (decl is AST.VariableDefinition vd)
                {
                    if (vd.variable.isConstant)
                    {
                        constVariables.Add(decl);
                    }
                    else
                    {
                        variables.Add(decl);
                    }
                }
                else if (decl is AST.FunctionDefinition)
                {
                    functionDefinitions.Add(decl);
                    if (!(decl as AST.FunctionDefinition).external)
                    {
                        other.Add(decl);
                    }
                }
                else if (decl is AST.Module ns)
                {
                    modules.Add(ns);
                }
                else if (decl is AST.EnumDeclaration)
                {
                    enumDeclarations.Add(decl);
                }
                else
                {
                    other.Add(decl);
                }
            }
            if (definitions)
            {
                foreach (var decl in functionDefinitions)
                {
                    Visit(decl as AST.FunctionDefinition, proto: true);
                }
                foreach (var decl in constVariables)
                {
                    Visit(decl as AST.VariableDefinition);
                }
                foreach (var decl in enumDeclarations)
                {
                    Visit(decl as AST.EnumDeclaration);
                }
                foreach (var decl in variables)
                {
                    Visit(decl as AST.VariableDefinition);
                }
                foreach (var ms in modules)
                {
                    Visit(ms, definitions: true);
                }
            }
            else
            {
                foreach (var decl in other)
                {
                    Visit(decl);
                }
                foreach (var ms in modules)
                {
                    Visit(ms, definitions: false);
                }
            }
        }

        public void Visit(AST.ConstInt node)
        {
            var nt = typeChecker.GetNodeType(node);
            var ct = GetTypeRef(nt);
            Value result;
            if (ct.kind == TypeKind.Half || ct.kind == TypeKind.Float || ct.kind == TypeKind.Double)
            {
                result = new ConstReal(ct, node.number);
            }
            else
            {
                Debug.Assert(ct.kind == TypeKind.Integer);
                result = new ConstInt(ct, (ulong)node.number);
            }
            valueStack.Push(result);
        }

        public void Visit(AST.ConstFloat node)
        {
            var ct = GetTypeRef(typeChecker.GetNodeType(node));
            var result = new ConstReal(ct, node.number);
            valueStack.Push(result);
        }

        public void Visit(AST.ConstBool node)
        {
            var result = node.value ? true_v : false_v;
            valueStack.Push(result);
        }

        public void Visit(AST.ConstString node, bool needsConversion = true)
        {
            var str = node.s;
            if (needsConversion)
            {
                str = node.ConvertString();
            }

            Value str_ptr;

            if (!stringTable.TryGetValue(str, out str_ptr))
            {

                str_ptr = builder.BuildGlobalStringPtr(str, node, "str");

                stringTable.Add(str, str_ptr);
            }


            var type = FrontendType.string_;
            var arr_struct_type = GetTypeRef(type);
            var insert = builder.GetInsertBlock();


            builder.PositionAtEnd(builder.context.currentFunctionContext.vars);
            builder.BuildComment("ConstString [VARS] START", node);

            var arr_struct_ptr = builder.BuildAlloca(arr_struct_type, node, "arr_struct_alloca", 16);
            var str_length = (uint)str.Length;
            var elem_type = GetTypeRef(type.elementType);

            var size = new ConstInt(i32_t, str_length);

            Value arr_elem_ptr;

            if (node.scope.function != null)
            {
                arr_elem_ptr = builder.BuildArrayAlloca(elem_type, size, node, "arr_elem_alloca");
            }
            else
            {
                var at = new ArrayType(elem_type, str_length);
                arr_elem_ptr = builder.AddGlobal(at, node, "str_arr", true);
                // if we are in a "global" scope dont allocate on the stack
                ((GlobalVariable)arr_elem_ptr).SetInitializer(builder.ConstNull(at));
                arr_elem_ptr = builder.BuildBitCast(arr_elem_ptr, new PointerType(elem_type), node, "str_ptr");
            }
            builder.BuildMemCpy(arr_elem_ptr, str_ptr, size, node);

            // set array length in struct
            var gep_arr_length = builder.BuildGEP(arr_struct_ptr, node, "gep_arr_elem_ptr", false, zero_i32_v, zero_i32_v);
            builder.BuildStore(new ConstInt(i32_t, str_length), gep_arr_length, node);

            // set array elem pointer in struct
            // NOTE(pragma): slice .data field is index 2 in struct, that's why we have "two_i32_v" here.
            var gep_arr_elem_ptr = builder.BuildGEP(arr_struct_ptr, node, "gep_arr_elem_ptr", false, zero_i32_v, two_i32_v);
            builder.BuildStore(arr_elem_ptr, gep_arr_elem_ptr, node);

            var arr_struct = builder.BuildLoad(arr_struct_ptr, node, "arr_struct_load");
            valueStack.Push(arr_struct);

            builder.BuildComment("ConstString [VARS] END", node);
            builder.PositionAtEnd(insert);
        }


        void InvalidBinOp(AST.BinOp node)
        {
            var text = $"Binary operator \"{node.token.text}\" not defined for types \"{this.typeChecker.GetNodeType(node.left)}\" and \"{this.typeChecker.GetNodeType(node.right)}\"!";
            throw new CompilerError(text, node.token);
        }

        public void Visit(AST.BinOp node)
        {
            if (node.type == AST.BinOp.BinOpType.ConditionalOR)
            {
                VisitConditionalOR(node);
                return;
            }
            if (node.type == AST.BinOp.BinOpType.ConditionaAND)
            {
                visitConditionalAND(node);
                return;
            }

            Visit(node.left);
            var left = valueStack.Pop();
            Visit(node.right);
            var right = valueStack.Pop();

            var leftFrontendType = typeChecker.GetNodeType(node.left);
            var rightFrontendType = typeChecker.GetNodeType(node.right);

            var leftType = left.type;
            var rightType = right.type;


            Value result = null;
            if (leftFrontendType.Equals(FrontendType.bool_))
            {
                switch (node.type)
                {
                    case AST.BinOp.BinOpType.LogicalAND:
                        result = builder.BuildAnd(left, right, node, "and_tmp");
                        break;
                    case AST.BinOp.BinOpType.LogicalOR:
                        result = builder.BuildOr(left, right, node, "or_tmp");
                        break;
                    case AST.BinOp.BinOpType.LogicalXOR:
                        result = builder.BuildXor(left, right, node, "xor_tmp");
                        break;
                    case AST.BinOp.BinOpType.Equal:
                        result = builder.BuildICmp(left, right, IcmpType.eq, node, "icmp_tmp");
                        break;
                    case AST.BinOp.BinOpType.NotEqual:
                        result = builder.BuildICmp(left, right, IcmpType.ne, node, "icmp_tmp");
                        break;
                    default:
                        InvalidBinOp(node);
                        break;
                }
            }
            else
            {
                switch (leftType.kind)
                {
                    case TypeKind.Vector when ((VectorType)leftType).elementType.kind == TypeKind.Integer:
                    case TypeKind.Integer:
                        switch (node.type)
                        {
                            case AST.BinOp.BinOpType.Add:
                                result = builder.BuildAdd(left, right, node, "add_tmp");
                                break;
                            case AST.BinOp.BinOpType.Subract:
                                result = builder.BuildSub(left, right, node, "sub_tmp");
                                break;
                            case AST.BinOp.BinOpType.Multiply:
                                result = builder.BuildMul(left, right, node, "mul_tmp");
                                break;
                            case AST.BinOp.BinOpType.Divide:
                                result = builder.BuildSDiv(left, right, node, "div_tmp");
                                break;
                            case AST.BinOp.BinOpType.DivideUnsigned:
                                result = builder.BuildUDiv(left, right, node, "div_tmp");
                                break;
                            case AST.BinOp.BinOpType.LeftShift:
                                result = builder.BuildShl(left, right, node, "shl_tmp");
                                break;
                            case AST.BinOp.BinOpType.RightShift:
                                result = builder.BuildAShr(left, right, node, "shr_tmp");
                                break;
                            case AST.BinOp.BinOpType.RightShiftUnsigned:
                                result = builder.BuildLShr(left, right, node, "shr_tmp");
                                break;
                            case AST.BinOp.BinOpType.Remainder:
                                result = builder.BuildURem(left, right, node, "urem_tmp");
                                break;
                            case AST.BinOp.BinOpType.Equal:
                                result = builder.BuildICmp(left, right, IcmpType.eq, node, "icmp_tmp");
                                break;
                            case AST.BinOp.BinOpType.NotEqual:
                                result = builder.BuildICmp(left, right, IcmpType.ne, node, "icmp_tmp");
                                break;
                            case AST.BinOp.BinOpType.Greater:
                                result = builder.BuildICmp(left, right, IcmpType.sgt, node, "icmp_tmp");
                                break;
                            case AST.BinOp.BinOpType.GreaterEqual:
                                result = builder.BuildICmp(left, right, IcmpType.sge, node, "icmp_tmp");
                                break;
                            case AST.BinOp.BinOpType.Less:
                                result = builder.BuildICmp(left, right, IcmpType.slt, node, "icmp_tmp");
                                break;
                            case AST.BinOp.BinOpType.LessEqual:
                                result = builder.BuildICmp(left, right, IcmpType.sle, node, "icmp_tmp");
                                break;
                            case AST.BinOp.BinOpType.GreaterUnsigned:
                                result = builder.BuildICmp(left, right, IcmpType.ugt, node, "icmp_tmp");
                                break;
                            case AST.BinOp.BinOpType.GreaterEqualUnsigned:
                                result = builder.BuildICmp(left, right, IcmpType.uge, node, "icmp_tmp");
                                break;
                            case AST.BinOp.BinOpType.LessUnsigned:
                                result = builder.BuildICmp(left, right, IcmpType.ult, node, "icmp_tmp");
                                break;
                            case AST.BinOp.BinOpType.LessEqualUnsigned:
                                result = builder.BuildICmp(left, right, IcmpType.ule, node, "icmp_tmp");
                                break;
                            case AST.BinOp.BinOpType.LogicalAND:
                                result = builder.BuildAnd(left, right, node, "and_tmp");
                                break;
                            case AST.BinOp.BinOpType.LogicalOR:
                                result = builder.BuildOr(left, right, node, "or_tmp");
                                break;
                            case AST.BinOp.BinOpType.LogicalXOR:
                                result = builder.BuildXor(left, right, node, "xor_tmp");
                                break;
                            default:
                                InvalidBinOp(node);
                                break;
                        }
                        break;
                    case TypeKind.Vector when ((VectorType)leftType).elementType.kind == TypeKind.Double:
                    case TypeKind.Vector when ((VectorType)leftType).elementType.kind == TypeKind.Float:
                    case TypeKind.Vector when ((VectorType)leftType).elementType.kind == TypeKind.Half:
                    case TypeKind.Double:
                    case TypeKind.Float:
                    case TypeKind.Half:
                        switch (node.type)
                        {
                            case AST.BinOp.BinOpType.Add:
                                result = builder.BuildFAdd(left, right, node, "fadd_tmp");
                                break;
                            case AST.BinOp.BinOpType.Subract:
                                result = builder.BuildFSub(left, right, node, "fsub_tmp");
                                break;
                            case AST.BinOp.BinOpType.Multiply:
                                result = builder.BuildFMul(left, right, node, "fmul_tmp");
                                break;
                            case AST.BinOp.BinOpType.Divide:
                                result = builder.BuildFDiv(left, right, node, "fdiv_tmp");
                                break;
                            case AST.BinOp.BinOpType.Remainder:
                                result = builder.BuildFRem(left, right, node, "frem_tmp");
                                break;
                            case AST.BinOp.BinOpType.Equal:
                                result = builder.BuildFCmp(left, right, FcmpType.oeq, node, "fcmp_tmp");
                                break;
                            case AST.BinOp.BinOpType.NotEqual:
                                result = builder.BuildFCmp(left, right, FcmpType.one, node, "fcmp_tmp");
                                break;
                            case AST.BinOp.BinOpType.Greater:
                                result = builder.BuildFCmp(left, right, FcmpType.ogt, node, "fcmp_tmp");
                                break;
                            case AST.BinOp.BinOpType.GreaterEqual:
                                result = builder.BuildFCmp(left, right, FcmpType.oge, node, "fcmp_tmp");
                                break;
                            case AST.BinOp.BinOpType.Less:
                                result = builder.BuildFCmp(left, right, FcmpType.olt, node, "fcmp_tmp");
                                break;
                            case AST.BinOp.BinOpType.LessEqual:
                                result = builder.BuildFCmp(left, right, FcmpType.ole, node, "fcmp_tmp");
                                break;
                            default:
                                InvalidBinOp(node);
                                break;
                        }
                        break;
                    case TypeKind.Pointer:
                        {
                            if (rightType.kind == TypeKind.Integer)
                            {
                                switch (node.type)
                                {
                                    case AST.BinOp.BinOpType.Add:
                                        {
                                            result = builder.BuildGEP(left, node, "ptr_add", false, right);
                                        }
                                        break;
                                    case AST.BinOp.BinOpType.Subract:
                                        {
                                            var n_right = builder.BuildNeg(right, node, "ptr_add_neg");
                                            result = builder.BuildGEP(left, node, "ptr_add", false, n_right);
                                        }
                                        break;
                                    default:
                                        InvalidBinOp(node);
                                        break;
                                }
                                break;
                            }
                            else if (rightType.kind == TypeKind.Pointer)
                            {
                                switch (node.type)
                                {
                                    case AST.BinOp.BinOpType.Subract:
                                        {
                                            var li = builder.BuildPtrToInt(left, mm_t, node, "ptr_to_int");
                                            var ri = builder.BuildPtrToInt(right, mm_t, node, "ptr_to_int");
                                            var sub = builder.BuildSub(li, ri, node, "sub");
                                            var let = ((PointerType)leftType).elementType;
                                            var size_of = builder.BuildSizeOf(let, node);
                                            result = builder.BuildSDiv(sub, size_of, node, "div");
                                        }
                                        break;
                                    case AST.BinOp.BinOpType.GreaterUnsigned:
                                        result = builder.BuildICmp(left, right, IcmpType.ugt, node, "icmp_tmp");
                                        break;
                                    case AST.BinOp.BinOpType.GreaterEqualUnsigned:
                                        result = builder.BuildICmp(left, right, IcmpType.uge, node, "icmp_tmp");
                                        break;
                                    case AST.BinOp.BinOpType.LessUnsigned:
                                        result = builder.BuildICmp(left, right, IcmpType.ult, node, "icmp_tmp");
                                        break;
                                    case AST.BinOp.BinOpType.LessEqualUnsigned:
                                        result = builder.BuildICmp(left, right, IcmpType.ule, node, "icmp_tmp");
                                        break;
                                    case AST.BinOp.BinOpType.Equal:
                                        result = builder.BuildICmp(left, right, IcmpType.eq, node, "icmp_tmp");
                                        break;
                                    case AST.BinOp.BinOpType.NotEqual:
                                        result = builder.BuildICmp(left, right, IcmpType.ne, node, "icmp_tmp");
                                        break;
                                    default:
                                        InvalidBinOp(node);
                                        break;
                                }
                            }
                            else
                                InvalidBinOp(node);
                        }
                        break;
                    default:
                        InvalidBinOp(node);
                        break;
                }
            }
            valueStack.Push(result);
        }

        void VisitConditionalOR(AST.BinOp op)
        {
            Visit(op.left);
            var cmp = valueStack.Pop();
            Debug.Assert(SSAType.IsBoolType(cmp.type));
            var block = builder.GetInsertBlock();

            var cor_rhs = builder.AppendBasicBlock("cor.rhs");

            // TODO(pragma): why do i need this?
            builder.MoveBasicBlockAfter(cor_rhs, block);

            var cor_end = builder.AppendBasicBlock("cor.end");

            // TODO(pragma): why do i need this?
            builder.MoveBasicBlockAfter(cor_end, cor_rhs);

            builder.BuildCondBr(cmp, cor_end, cor_rhs, op);

            // cor.rhs: 
            builder.PositionAtEnd(cor_rhs);
            Visit(op.right);

            var cor_rhs_tv = valueStack.Pop();
            Debug.Assert(SSAType.IsBoolType(cor_rhs_tv.type));
            builder.BuildBr(cor_end, op);

            cor_rhs = builder.GetInsertBlock();

            // cor.end:
            builder.PositionAtEnd(cor_end);

            var phi = builder.BuildPhi(bool_t, op, "corphi", (true_v, block), (cor_rhs_tv, cor_rhs));

            valueStack.Push(phi);
        }

        void visitConditionalAND(AST.BinOp op)
        {
            Visit(op.left);
            var cmp = valueStack.Pop();
            Debug.Assert(SSAType.IsBoolType(cmp.type));
            var block = builder.GetInsertBlock();

            var cand_rhs = builder.AppendBasicBlock("cand.rhs");
            builder.MoveBasicBlockAfter(cand_rhs, block);

            var cand_end = builder.AppendBasicBlock("cand.end");
            builder.MoveBasicBlockAfter(cand_end, cand_rhs);

            builder.BuildCondBr(cmp, cand_rhs, cand_end, op);

            // cor.rhs: 
            builder.PositionAtEnd(cand_rhs);
            Visit(op.right);
            var cand_rhs_tv = valueStack.Pop();
            Debug.Assert(SSAType.IsBoolType(cand_rhs_tv.type));

            builder.BuildBr(cand_end, op);
            cand_rhs = builder.GetInsertBlock();

            // cor.end:
            builder.PositionAtEnd(cand_end);
            var phi = builder.BuildPhi(bool_t, op, "candphi", (false_v, block), (cand_rhs_tv, cand_rhs));

            valueStack.Push(phi);
        }

        void InvalidUnaryOp(AST.UnaryOp node)
        {
            var text = $"Unary operator \"{node.token.text}\" not defined for type \"{this.typeChecker.GetNodeType(node.expression)}\"!";
            throw new CompilerError(text, node.token);
        }

        public void Visit(AST.UnaryOp node)
        {
            if (node.type == AST.UnaryOp.UnaryOpType.SizeOf)
            {
                var fet = typeChecker.GetNodeType(node.expression);
                var et = GetTypeRef(fet);

                valueStack.Push(builder.BuildSizeOf(et, node));
                return;
            }

            Visit(node.expression);

            var v = valueStack.Pop();
            var vtype = v.type;
            Value result;

            switch (node.type)
            {
                case AST.UnaryOp.UnaryOpType.Add:
                    result = v;
                    break;
                case AST.UnaryOp.UnaryOpType.Subract:
                    switch (vtype.kind)
                    {
                        case TypeKind.Half:
                        case TypeKind.Float:
                        case TypeKind.Double:
                            result = builder.BuildFNeg(v, node, "fneg_tmp");
                            break;
                        case TypeKind.Integer:
                            result = builder.BuildNeg(v, node, "neg_tmp");
                            break;
                        default:
                            InvalidUnaryOp(node);
                            throw new InvalidCodePath();
                    }
                    break;
                case AST.UnaryOp.UnaryOpType.LogicalNot:
                    Debug.Assert(SSAType.IsBoolType(vtype));
                    result = builder.BuildNot(v, node, "not_tmp");
                    break;
                case AST.UnaryOp.UnaryOpType.Complement:
                    result = builder.BuildXor(v, new ConstInt(vtype, unchecked((ulong)-1)), node, "complement_tmp");
                    break;
                case AST.UnaryOp.UnaryOpType.AddressOf:
                    // HACK: for NOW this happens via returnPointer nonsense
                    result = v;
                    if (v.type is PointerType pt)
                    {
                        if (pt.elementType is ArrayType at)
                        {
                            result = builder.BuildBitCast(v, new PointerType(at.elementType), node, "address_of_array");
                        }
                    }
                    break;
                case AST.UnaryOp.UnaryOpType.Dereference:
                    result = v;
                    if (!node.returnPointer)
                    {
                        result = builder.BuildLoad(result, node, "deref");
                    }

                    break;
                case AST.UnaryOp.UnaryOpType.PreInc:
                    {
                        result = builder.BuildLoad(v, node, "preinc_load");
                        if (!(vtype is PointerType))
                        {
                            throw new CompilerError("Cannot take pointer of element.", node.token);
                        }
                        var vet = (vtype as PointerType).elementType;
                        var vet_kind = vet.kind;
                        switch (vet_kind)
                        {
                            case TypeKind.Integer:
                                result = builder.BuildAdd(result, new ConstInt(vet, 1), node, "preinc");
                                break;
                            case TypeKind.Half:
                            case TypeKind.Float:
                            case TypeKind.Double:
                                result = builder.BuildFAdd(result, new ConstReal(vet, 1.0), node, "preinc");
                                break;
                            case TypeKind.Pointer:
                                result = builder.BuildGEP(result, node, "ptr_pre_inc", false, one_i32_v);
                                break;
                            default:
                                InvalidUnaryOp(node);
                                throw new InvalidCodePath();
                        }
                        builder.BuildStore(result, v, node);
                    }
                    break;
                case AST.UnaryOp.UnaryOpType.PreDec:
                    {
                        result = builder.BuildLoad(v, node, "predec_load");
                        if (!(vtype is PointerType))
                        {
                            throw new CompilerError("Cannot take pointer of element.", node.token);
                        }

                        var vet = (vtype as PointerType).elementType;
                        var vet_kind = vet.kind;
                        switch (vet_kind)
                        {
                            case TypeKind.Integer:
                                result = builder.BuildSub(result, new ConstInt(vet, 1), node, "predec");
                                break;
                            case TypeKind.Half:
                            case TypeKind.Float:
                            case TypeKind.Double:
                                result = builder.BuildFSub(result, new ConstReal(vet, 1.0), node, "predec");
                                break;
                            case TypeKind.Pointer:
                                result = builder.BuildGEP(result, node, "ptr_pre_dec", false, neg_1_i32_v);
                                break;
                            default:
                                InvalidUnaryOp(node);
                                break;
                        }
                        builder.BuildStore(result, v, node);
                    }
                    break;
                case AST.UnaryOp.UnaryOpType.PostInc:
                    {
                        result = builder.BuildLoad(v, node, "postinc_load");
                        if (!(vtype is PointerType))
                        {
                            throw new CompilerError("Cannot take pointer of element.", node.token);
                        }
                        var vet = (vtype as PointerType).elementType;
                        var vet_kind = vet.kind;
                        switch (vet_kind)
                        {
                            case TypeKind.Integer:
                                {
                                    var inc = builder.BuildAdd(result, new ConstInt(vet, 1), node, "postinc");
                                    builder.BuildStore(inc, v, node);
                                }
                                break;
                            case TypeKind.Half:
                            case TypeKind.Float:
                            case TypeKind.Double:
                                {
                                    var inc = builder.BuildFAdd(result, new ConstReal(vet, 1.0), node, "postinc");
                                    builder.BuildStore(inc, v, node);
                                }
                                break;
                            case TypeKind.Pointer:
                                {
                                    var inc = builder.BuildGEP(result, node, "ptr_post_inc", false, one_i32_v);
                                    builder.BuildStore(inc, v, node);
                                }
                                break;
                            default:
                                InvalidUnaryOp(node);
                                break;
                        }
                    }
                    break;
                case AST.UnaryOp.UnaryOpType.PostDec:
                    {
                        result = builder.BuildLoad(v, node, "postdec_load");
                        if (!(vtype is PointerType))
                        {
                            throw new CompilerError("Cannot take pointer of element.", node.token);
                        }
                        var vet = (vtype as PointerType).elementType;
                        var vet_kind = vet.kind;
                        switch (vet_kind)
                        {
                            case TypeKind.Integer:
                                {
                                    var inc = builder.BuildSub(result, new ConstInt(vet, 1), node, "postdec");
                                    builder.BuildStore(inc, v, node);
                                }
                                break;
                            case TypeKind.Half:
                            case TypeKind.Float:
                            case TypeKind.Double:
                                {
                                    var inc = builder.BuildFSub(result, new ConstReal(vet, 1.0), node, "postdec");
                                    builder.BuildStore(inc, v, node);
                                }
                                break;
                            case TypeKind.Pointer:
                                {
                                    var inc = builder.BuildGEP(result, node, "ptr_post_dec", false, neg_1_i32_v);
                                    builder.BuildStore(inc, v, node);
                                }
                                break;
                            default:
                                InvalidUnaryOp(node);
                                break;
                        }
                    }
                    break;
                default:
                    InvalidUnaryOp(node);
                    throw new InvalidCodePath();
            }
            valueStack.Push(result);
        }

        void InvalidTypeCastOp(AST.TypeCastOp node)
        {
            var baseType = typeChecker.GetNodeType(node.expression);
            var targetType = typeChecker.GetNodeType(node);
            var text = $"Typecast operator \"{node.token.text}\" not defined for base type \"{baseType}\" and target type \"{targetType}\"!";
            throw new CompilerError(text, node.token);
        }

        public void Visit(AST.TypeCastOp node)
        {
            Visit(node.expression);
            var frontendSourceType = typeChecker.GetNodeType(node.expression);

            var v = valueStack.Pop();
            var vtype = v.type;

            var typeName = node.typeString.ToString(); // node.type.ToString();

            Value result = null;
            var frontendTargetType = typeChecker.GetNodeType(node);
            var targetType = GetTypeRef(frontendTargetType);

            if (targetType.EqualType(vtype))
            {
                result = v;
                valueStack.Push(result);
                return;
            }

            //var ttk = targetType.kind;
            //var vtk = vtype.kind;
            switch (targetType)
            {
                case IntegerType t_it:
                    switch (vtype)
                    {
                        case IntegerType v_it:
                            if (t_it.bitWidth > v_it.bitWidth)
                            {
                                if (!node.unsigned)
                                {
                                    result = builder.BuildSExt(v, targetType, node, "int_cast");
                                }
                                else
                                {
                                    result = builder.BuildZExt(v, targetType, node, "int_cast");
                                }
                            }
                            else if (t_it.bitWidth < v_it.bitWidth)
                            {
                                result = builder.BuildTrunc(v, targetType, node, "int_trunc");
                            }
                            else if (t_it.bitWidth == v_it.bitWidth)
                            {
                                result = builder.BuildBitCast(v, targetType, node, "int_bitcast");
                            }
                            break;
                        case FloatType v_ft:
                            if (!node.unsigned)
                            {
                                result = builder.BuildFPToSI(v, targetType, node, "int_cast");
                            }
                            else
                            {
                                result = builder.BuildFPToUI(v, targetType, node, "int_cast");
                            }
                            break;
                        case PointerType v_pt:
                            result = builder.BuildPtrToInt(v, targetType, node, "int_cast");
                            break;
                        default:
                            InvalidTypeCastOp(node);
                            break;
                    }
                    break;
                case FloatType t_ft:
                    switch (vtype)
                    {
                        case IntegerType v_it:
                            if (!node.unsigned)
                            {
                                result = builder.BuildSIToFP(v, targetType, node, "int_to_float_cast");
                            }
                            else
                            {
                                result = builder.BuildUIToFP(v, targetType, node, "int_to_float_cast");
                            }
                            break;
                        case FloatType v_fp:
                            result = builder.BuildFPCast(v, targetType, node, "fp_to_fp_cast");
                            break;
                        default:
                            InvalidTypeCastOp(node);
                            break;
                    }
                    break;
                case PointerType t_pt:
                    switch (vtype)
                    {
                        case IntegerType v_it:
                            result = builder.BuildIntToPtr(v, targetType, node, "int_to_ptr");
                            break;
                        case PointerType v_pt:
                            result = builder.BuildBitCast(v, targetType, node, "pointer_bit_cast");
                            break;
                        default:
                            InvalidTypeCastOp(node);
                            break;
                    }
                    break;
                case VectorType t_vec:
                    if (vtype is VectorType v_vec)
                    {
                        if (t_vec.elementCount != v_vec.elementCount)
                        {
                            if (SizeOfVectorType((FrontendVectorType)frontendSourceType) != SizeOfVectorType((FrontendVectorType)frontendTargetType))
                            {
                                InvalidTypeCastOp(node);
                            }
                            else
                            {
                                result = builder.BuildBitCast(v, targetType, node, "vec_vec_bitcast");
                            }
                            break;
                        }

                        switch (t_vec.elementType)
                        {
                            case IntegerType t_elem_it:
                                switch (v_vec.elementType)
                                {
                                    case IntegerType v_elem_it:
                                        if (t_elem_it.bitWidth > v_elem_it.bitWidth)
                                        {
                                            if (!node.unsigned)
                                            {
                                                result = builder.BuildSExt(v, targetType, node, "vec_int_cast");
                                            }
                                            else
                                            {
                                                result = builder.BuildZExt(v, targetType, node, "vec_int_cast");
                                            }
                                        }
                                        else if (t_elem_it.bitWidth < v_elem_it.bitWidth)
                                        {
                                            result = builder.BuildTrunc(v, targetType, node, "vec_int_trunc");
                                        }
                                        else if (t_elem_it.bitWidth == v_elem_it.bitWidth)
                                        {
                                            result = builder.BuildBitCast(v, targetType, node, "vec_int_bitcast");
                                        }
                                        break;
                                    case FloatType v_elem_ft:
                                        if (!node.unsigned)
                                        {
                                            result = builder.BuildFPToSI(v, targetType, node, "vec_int_cast");
                                        }
                                        else
                                        {
                                            result = builder.BuildFPToUI(v, targetType, node, "vec_int_cast");
                                        }
                                        break;
                                    case PointerType v_elem_pt:
                                        result = builder.BuildPtrToInt(v, targetType, node, "vec_int_cast");
                                        break;
                                    default:
                                        InvalidTypeCastOp(node);
                                        break;
                                }
                                break;
                            case FloatType t_elem_ft:
                                switch (v_vec.elementType)
                                {
                                    case IntegerType v_it:
                                        if (!node.unsigned)
                                        {
                                            result = builder.BuildSIToFP(v, targetType, node, "vec_int_to_float_cast");
                                        }
                                        else
                                        {
                                            result = builder.BuildUIToFP(v, targetType, node, "vec_int_to_float_cast");
                                        }
                                        break;
                                    default:
                                        InvalidTypeCastOp(node);
                                        break;
                                }
                                break;
                            case PointerType t_elem_pt:
                                switch (v_vec.elementType)
                                {
                                    case IntegerType v_it:
                                        result = builder.BuildIntToPtr(v, targetType, node, "vec_int_to_ptr");
                                        break;
                                    case PointerType v_pt:
                                        result = builder.BuildBitCast(v, targetType, node, "vec_pointer_bit_cast");
                                        break;
                                    default:
                                        InvalidTypeCastOp(node);
                                        break;
                                }
                                break;
                            default:
                                InvalidTypeCastOp(node);
                                break;
                        }
                    }
                    else
                    {
                        InvalidTypeCastOp(node);
                    }
                    break;
                default:
                    InvalidTypeCastOp(node);
                    break;
            }

            Debug.Assert(result != null);
            valueStack.Push(result);
        }

        void VisitCompoundLiteralStruct(AST.CompoundLiteral node, FrontendStructType fst, bool isConst = false, bool returnPointer = false)
        {
            var sc = node;
            var structType = (StructType)GetTypeRef(fst);
            if (!isConst)
            {
                var insert = builder.GetInsertBlock();
                builder.PositionAtEnd(builder.context.currentFunctionContext.vars);

                var align = GetMinimumAlignmentForBackend(structType);
                var struct_ptr = builder.BuildAlloca(structType, node, "struct_alloca", align);
                builder.PositionAtEnd(insert);
                for (int i = 0; i < fst.fields.Count; ++i)
                {
                    if (i < node.argumentList.Count)
                    {
                        Visit(sc.argumentList[i]);
                        var arg = valueStack.Pop();
                        var arg_ptr = builder.BuildStructGEP(struct_ptr, i, node, "struct_arg_" + i);
                        builder.BuildStore(arg, arg_ptr, node);
                    }
                    else
                    {
                        var arg_ptr = builder.BuildStructGEP(struct_ptr, i, node, "struct_arg_" + i);
                        var et = (arg_ptr.type as PointerType).elementType;
                        builder.BuildStore(builder.ConstNull(et), arg_ptr, node);
                    }
                }

                if (node.returnPointer || returnPointer)
                {
                    valueStack.Push(struct_ptr);
                }
                else
                {
                    var load = builder.BuildLoad(struct_ptr, node, "struct_constr_load");
                    valueStack.Push(load);
                }
            }
            else
            {
                Debug.Assert(!returnPointer && !node.returnPointer);
                var elements = new List<Value>();
                for (int i = 0; i < fst.fields.Count; ++i)
                {
                    if (i < node.argumentList.Count)
                    {
                        Visit(sc.argumentList[i]);
                        var el = valueStack.Pop();
                        if (!el.isConst)
                        {
                            throw new CompilerError($"Element {i + 1} of compound literal must be a compile-time constant.", node.argumentList[1].token);
                        }
                        elements.Add(el);
                    }
                    else
                    {
                        var et = structType.elementTypes[i];
                        elements.Add(builder.ConstNull(et));
                    }
                }
                var result = new ConstStruct(structType, elements);
                valueStack.Push(result);
            }
        }

        void VisitCompoundLiteralVector(AST.CompoundLiteral node, FrontendVectorType fvt, bool isConst = false, bool returnPointer = false)
        {
            var sc = node;
            var vecType = (VectorType)GetTypeRef(fvt);
            if (!isConst)
            {
                var insert = builder.GetInsertBlock();

                Debug.Assert(node.argumentList.Count == 0 || node.argumentList.Count == fvt.length);

                Value vec;
                if (sc.argumentList.Count == 0)
                {
                    vec = builder.ConstNull(vecType);
                }
                else
                {
                    List<Value> constValues = new List<Value>(fvt.length);
                    List<Value> nonConstValues = new List<Value>(fvt.length);
                    for (int i = 0; i < fvt.length; ++i)
                    {
                        Visit(sc.argumentList[i]);
                        var arg = valueStack.Pop();
                        if (arg.isConst)
                        {
                            constValues.Add(arg);
                            nonConstValues.Add(null);
                        }
                        else
                        {
                            constValues.Add(Value.Undefined(vecType.elementType));
                            nonConstValues.Add(arg);
                        }
                    }
                    vec = new ConstVec(vecType, constValues);
                    for (int i = 0; i < nonConstValues.Count; ++i)
                    {
                        var arg_v = nonConstValues[i];
                        if (arg_v != null)
                        {
                            vec = builder.BuildInsertElement(vec, arg_v, new ConstInt(Const.i32_t, (ulong)i), node, "compound_literal_insert");
                        }
                    }
                }

                if (node.returnPointer || returnPointer)
                {
                    builder.PositionAtEnd(builder.context.currentFunctionContext.vars);
                    var align = GetMinimumAlignmentForBackend(vecType);
                    var vec_ptr = builder.BuildAlloca(vecType, node, "vec_alloca", align);
                    builder.PositionAtEnd(insert);
                    builder.BuildStore(vec, vec_ptr, node, align: align);
                    valueStack.Push(vec_ptr);
                }
                else
                {
                    valueStack.Push(vec);
                }
            }
            else
            {
                Value vec;
                if (sc.argumentList.Count == 0)
                {
                    vec = builder.ConstNull(vecType);
                }
                else
                {
                    Debug.Assert(node.argumentList.Count == fvt.length);
                    List<Value> constValues = new List<Value>(fvt.length);
                    for (int i = 0; i < fvt.length; ++i)
                    {
                        Visit(sc.argumentList[i]);
                        var arg = valueStack.Pop();
                        Debug.Assert(arg.isConst);
                        constValues.Add(arg);
                    }
                    vec = new ConstVec(vecType, constValues);
                }
                valueStack.Push(vec);
            }
        }

        public void Visit(AST.CompoundLiteral node, bool isConst = false, bool returnPointer = false)
        {
            var sc = node;
            var nt = typeChecker.GetNodeType(node);
            if (nt is FrontendStructType fst)
            {
                VisitCompoundLiteralStruct(node, fst, isConst, returnPointer);
            }
            else if (nt is FrontendVectorType fvt)
            {
                VisitCompoundLiteralVector(node, fvt, isConst, returnPointer);
            }
        }

        public void Visit(AST.SliceOp node, bool returnPointer = false)
        {
            Visit(node.left);
            var ptr = valueStack.Pop();

            var s_ft = typeChecker.GetNodeType(node) as FrontendSliceType;
            var structType = (StructType)GetTypeRef(s_ft);
            var insert = builder.GetInsertBlock();
            builder.PositionAtEnd(builder.context.currentFunctionContext.vars);
            builder.BuildComment("AST.SliceOp [VARS] BEGIN", node);
            var align = GetMinimumAlignmentForBackend(structType);
            var struct_ptr = builder.BuildAlloca(structType, node, "slice_alloca", align);
            builder.BuildComment("AST.SliceOp [VARS] END", node);
            builder.PositionAtEnd(insert);
            builder.BuildComment("AST.SliceOp [INSERT] BEGIN", node);

            ptr = builder.BuildBitCast(ptr, structType.elementTypes[2], node, "slice_hack_cast");

            Visit(node.capacity);
            Value capacity = valueStack.Pop();

            Visit(node.from);
            Value from = valueStack.Pop();

            Visit(node.to);
            Value to = valueStack.Pop();

            var slice_from_neg = builder.AppendBasicBlock("slice_from_neg");
            builder.MoveBasicBlockAfter(slice_from_neg, insert);
            var slice_from_neg_end = builder.AppendBasicBlock("slice_from_neg_end");
            builder.MoveBasicBlockAfter(slice_from_neg_end, slice_from_neg);

            Value from_neg_cond = builder.BuildICmp(from, Const.zero_i32_v, IcmpType.slt, node, "from_neg_cond");
            builder.BuildCondBr(from_neg_cond, slice_from_neg, slice_from_neg_end, node);

            // slice_from_neg:
            builder.PositionAtEnd(slice_from_neg);
            Value from_neg = builder.BuildAdd(capacity, from, node, "from_neg");
            builder.BuildBr(slice_from_neg_end, node);

            // slice_from_neg_end:
            builder.PositionAtEnd(slice_from_neg_end);
            var from_phi = builder.BuildPhi(Const.i32_t, node, "from_phi", (from, insert), (from_neg, slice_from_neg));

            var slice_to_neg = builder.AppendBasicBlock("slice_to_neg");
            builder.MoveBasicBlockAfter(slice_to_neg, slice_from_neg_end);
            var slice_to_neg_end = builder.AppendBasicBlock("slice_to_neg_end");
            builder.MoveBasicBlockAfter(slice_to_neg_end, slice_to_neg);

            Value to_neg_cond = builder.BuildICmp(to, Const.zero_i32_v, IcmpType.slt, node, "to_neg_cond");
            builder.BuildCondBr(to_neg_cond, slice_to_neg, slice_to_neg_end, node);

            // slice_to_neg:
            builder.PositionAtEnd(slice_to_neg);
            Value to_neg = builder.BuildAdd(capacity, to, node, "to_neg");
            builder.BuildBr(slice_to_neg_end, node);

            // slice_to_neg_end:
            builder.PositionAtEnd(slice_to_neg_end);
            var to_phi = builder.BuildPhi(Const.i32_t, node, "to_phi", (to, slice_from_neg_end), (to_neg, slice_to_neg));



            Value length = builder.BuildSub(to_phi, from_phi, node, "slice_length");
            Value slice_capacity = builder.BuildSub(capacity, from_phi, node, "slice_capacity");
            Value data = builder.BuildGEP(ptr, node, "slice_ptr_offset", false, from_phi);

            var length_ptr = builder.BuildStructGEP(struct_ptr, 0, node, "slice_arg_length");
            builder.BuildStore(length, length_ptr, node);

            var capacity_ptr = builder.BuildStructGEP(struct_ptr, 1, node, "slice_arg_capacity");
            builder.BuildStore(slice_capacity, capacity_ptr, node);

            var data_ptr = (Value)builder.BuildStructGEP(struct_ptr, 2, node, "slice_arg_data");
            builder.BuildStore(data, data_ptr, node);

            if (node.returnPointer || returnPointer)
            {
                valueStack.Push(struct_ptr);
            }
            else
            {
                var load = builder.BuildLoad(struct_ptr, node, "slice_load");
                valueStack.Push(load);
            }
            builder.BuildComment("AST.SliceOp [INSERT] END", node);
        }

        public void Visit(AST.ArrayConstructor node, bool isConst = false, bool returnPointer = false)
        {
            var ac = node;
            var ac_type = typeChecker.GetNodeType(node) as FrontendArrayType;
            var arr_type = (ArrayType)GetTypeRef(ac_type);

            if (!isConst)
            {
                var insert = builder.GetInsertBlock();
                builder.PositionAtEnd(builder.context.currentFunctionContext.vars);
                int align = GetMinimumAlignmentForBackend(arr_type);
                var arr_ptr = builder.BuildAlloca(arr_type, node, "arr_alloca", align);

                builder.PositionAtEnd(insert);
                Debug.Assert(arr_type.elementCount == node.elements.Count);
                // TODO(pragma): possible optimization for all constant elements
                for (int i = 0; i < node.elements.Count; ++i)
                {
                    Visit(node.elements[i]);
                    var elem = valueStack.Pop();
                    var dest = builder.BuildGEP(arr_ptr, node, "arr_elem_store", true, zero_i32_v, new ConstInt(i32_t, (ulong)i));
                    builder.BuildStore(elem, dest, node);
                }


                if (node.returnPointer || returnPointer)
                {
                    valueStack.Push(arr_ptr);
                }
                else
                {
                    var load = builder.BuildLoad(arr_ptr, node, "arr_cstr_load");
                    valueStack.Push(load);
                }


            }
            else
            {
                var elements = new List<Value>();
                Debug.Assert(arr_type.elementCount == node.elements.Count);
                for (int i = 0; i < node.elements.Count; ++i)
                {
                    Visit(node.elements[i]);
                    var el = valueStack.Pop();
                    if (!el.isConst)
                    {
                        throw new CompilerError($"Element {i + 1} of array constructor must be a compile-time constant.", node.elements[1].token);
                    }
                    elements.Add(el);
                }
                var result = new ConstArray(arr_type, elements);
                valueStack.Push(result);
            }
        }



        public void Visit(AST.VariableDefinition node)
        {
            if (node.variable.isConstant)
            {
                switch (node.expression)
                {
                    case AST.CompoundLiteral cl:
                        Visit(cl, isConst: true);
                        break;
                    case AST.ArrayConstructor ac:
                        Visit(ac, isConst: true);
                        break;
                    default:
                        Visit(node.expression);
                        break;
                }
                var v = valueStack.Pop();
                variables[node.variable] = v;
                return;
            }

            if (!builder.context.isGlobal)
            {
                Debug.Assert(node.expression != null || node.typeString != null);

                SSAType vType;
                Value v;
                if (node.expression != null)
                {
                    switch (node.expression)
                    {
                        case AST.CompoundLiteral cl:
                            Visit(cl, returnPointer: true);
                            break;
                        case AST.ArrayConstructor ac:
                            Visit(ac, returnPointer: true);
                            break;
                        default:
                            Visit(node.expression);
                            break;
                    }
                    v = valueStack.Pop();
                    vType = v.type;
                }
                else
                {
                    v = null;
                    vType = GetTypeRef(typeChecker.GetNodeType(node.typeString));
                }

                Value result;
                if (node.expression != null && node.expression is AST.CompoundLiteral)
                {
                    result = v;
                }
                else if (node.expression != null && node.expression is AST.ArrayConstructor)
                {
                    result = v;
                }
                else
                {
                    var insert = builder.GetInsertBlock();
                    builder.PositionAtEnd(builder.context.currentFunctionContext.vars);
                    int align = GetMinimumAlignmentForBackend(vType);
                    result = builder.BuildAlloca(vType, node, node.variable.name, align);
                    variables[node.variable] = result;
                    builder.PositionAtEnd(insert);
                    if (v != null)
                    {
                        builder.BuildStore(v, result, node);
                    }
                }
                if (node.typeString != null && node.typeString.allocationCount > 0)
                {
                    var insert = builder.GetInsertBlock();
                    builder.PositionAtEnd(builder.context.currentFunctionContext.vars);
                    Debug.Assert(node.expression == null);

                    var ac = new ConstInt(i32_t, (ulong)node.typeString.allocationCount);
                    if (!(vType is PointerType))
                    {
                        throw new CompilerError("Cannot take pointer of element.", node.expression.token);
                    }
                    var et = (vType as PointerType).elementType;

                    var alloc = builder.BuildArrayAlloca(et, ac, node, "alloca");
                    builder.BuildStore(alloc, result, node);
                    builder.PositionAtEnd(insert);
                }
                variables[node.variable] = result;
            }
            else
            { // is global
                if (node.expression != null && node.expression is AST.CompoundLiteral)
                {
                    var sc = node.expression as AST.CompoundLiteral;
                    var compoundType = GetTypeRef(typeChecker.GetNodeType(sc));

                    int align = GetMinimumAlignmentForBackend(compoundType);
                    var v = builder.AddGlobal(compoundType, node, node.variable.name, false, align);

                    variables[node.variable] = v;
                    v.SetInitializer(builder.ConstNull(compoundType));

                    for (int i = 0; i < sc.argumentList.Count; ++i)
                    {
                        Visit(sc.argumentList[i]);
                        var arg = valueStack.Pop();
                        var arg_ptr = builder.BuildStructGEP(v, i, node, "struct_arg_" + i);
                        builder.BuildStore(arg, arg_ptr, node);
                    }
                }
                else if (node.expression is AST.ArrayConstructor)
                {
                    throw new System.NotImplementedException();
                }
                else
                {

                    if (node.expression != null)
                    {
                        Visit(node.expression);
                        var result = valueStack.Pop();
                        var resultType = result.type;
                        int align = GetMinimumAlignmentForBackend(resultType);
                        var v = builder.AddGlobal(resultType, node, node.variable.name, false, align);
                        variables[node.variable] = v;
                        // LVM.SetLinkage(v, LLVMLinkage.LLVMInternalLinkage);
                        if (result.isConst)
                        {
                            v.SetInitializer(result);
                        }
                        else
                        {
                            v.SetInitializer(builder.ConstNull(resultType));
                            builder.BuildStore(result, v, node);
                        }
                    }
                    else
                    {
                        var vType = GetTypeRef(typeChecker.GetNodeType(node.typeString));
                        int align = GetMinimumAlignmentForBackend(vType);
                        var v = builder.AddGlobal(vType, node, node.variable.name, false, align);
                        variables[node.variable] = v;
                        v.SetInitializer(builder.ConstNull(vType));
                    }
                }
            }
        }

        public void Visit(AST.Assignment node)
        {
            if (node.type == AST.Assignment.AssignmentType.Regular)
            {
                Visit(node.left);
                var target = valueStack.Pop();
                var targetType = target.type;
                bool isVolatile = target.flags.HasFlag(SSAFlags.@volatile);
                // var targetTypeName = typeToString(targetType);

                Visit(node.right);
                var result = valueStack.Pop();
                var resultType = result.type;
                // var resultTypeName = typeToString(resultType);

                if (!(targetType is PointerType))
                {
                    throw new CompilerError("Cannot take pointer of element.", node.left.token);
                }
                var et = (targetType as PointerType).elementType;
                if (!et.EqualType(resultType))
                {
                    target = builder.BuildBitCast(target, new PointerType(resultType), node, "hmpf");
                }
                var align = GetMinimumAlignmentForBackend(resultType);
                builder.BuildStore(result, target, node, isVolatile, align);
                valueStack.Push(result);
            }
            else if (node.type == AST.Assignment.AssignmentType.Vector)
            {
                var iea = (AST.IndexedElementAccess)node.left;
                Visit(iea.left);
                var vecPtr = valueStack.Pop();
                bool isVolatile = vecPtr.flags.HasFlag(SSAFlags.@volatile);
                var vecType = ((PointerType)vecPtr.type).elementType;
                var align = GetMinimumAlignmentForBackend(vecType);
                var vec = builder.BuildLoad(vecPtr, node, "vec_load", isVolatile, align);

                Debug.Assert(iea.indices.Count == 1);
                Visit(iea.indices[0]);
                var idx = valueStack.Pop();
                Visit(node.right);
                var result = valueStack.Pop();
                var insert = builder.BuildInsertElement(vec, result, idx, node, "vec_insert");
                builder.BuildStore(insert, vecPtr, node, isVolatile);
                valueStack.Push(result);
            }
        }

        public void Visit(AST.Block node)
        {
            // HACK: DO a prepass with sorting
            foreach (var s in node.statements)
            {
                if (isConstVariableDefinition(s))
                {
                    Visit(s);
                    valueStack.Clear();
                }
            }
            foreach (var s in node.statements)
            {
                if (!isConstVariableDefinition(s))
                {
                    Visit(s);
                    valueStack.Clear();
                }
            }
        }

        public void Visit(AST.VariableReference node)
        {
            var ov = node.scope.GetVar(node.variableName, node.token);
            Scope.VariableDefinition vd;
            if (!ov.IsOverloaded)
            {
                vd = ov.First;
            }
            else
            {
                vd = ov.variables[node.overloadedIdx];
            }
            // if variable is function paramter just return it immediately
            if (vd.isFunctionParameter)
            {
                var f = builder.context.currentFunction;
                var pr = builder.GetParam(f, vd.parameterIdx);
                valueStack.Push(pr);
                return;
            }
            var nt = typeChecker.GetNodeType(node);

            if (!variables.TryGetValue(vd, out var v))
            {
                throw new CompilerError("Ordering violation or can't use non constant Value in constant declaration!", node.token);
            }
            Value result;
            if (vd.isConstant)
            {
                if (node.returnPointer && !(v is Function))
                {
                    throw new CompilerError("Cannot take pointer of constant!", node.token);
                }
                result = v;
                // Debug.Assert(LLVM.IsConstant(v));
            }
            else
            {
                result = v;
                if (!node.returnPointer)
                {
                    int align = GetMinimumAlignmentForBackend(v.type);
                    result = builder.BuildLoad(v, node, vd.name, align: align);
                }
            }
            valueStack.Push(result);
        }

        public void VisitSIMDFunction(AST.FunctionCall node, FrontendFunctionType feft)
        {
            switch (feft.funName)
            {
                case "slli_si128":
                    {
                        Visit(node.argumentList[0]);
                        var v = valueStack.Pop();
                        Visit(node.argumentList[1]);
                        var shift = valueStack.Pop();
                        if (!shift.isConst)
                        {
                            throw new CompilerError($"Argument 2 to \"{feft.funName}\" must be a compile-time constant.", node.argumentList[1].token);
                        }
                        var i8_16x_t = new VectorType(16, Const.i8_t);
                        var zero = builder.ConstNull(i8_16x_t);
                        var bc = builder.BuildBitCast(v, i8_16x_t, node, "vec_bc");
                        var maskValues = new List<Value>();
                        var shiftValue = (int)((ConstInt)shift).data;
                        shiftValue = Math.Clamp(shiftValue, 0, 16);
                        for (int i = 0; i < 16; ++i)
                        {
                            maskValues.Add(new ConstInt(Const.i32_t, (ulong)(16 + i - shiftValue)));
                        }
                        var mask = new ConstVec(new VectorType(16, Const.i32_t), maskValues);
                        var shuffle = builder.BuildShuffleVector(zero, bc, mask, node, "slli_shuffle");
                        var result = builder.BuildBitCast(shuffle, v.type, node, "vec_bc");
                        valueStack.Push(result);
                    }
                    break;
                case "srli_si128":
                    {
                        Visit(node.argumentList[0]);
                        var v = valueStack.Pop();
                        Visit(node.argumentList[1]);
                        var shift = valueStack.Pop();
                        if (!shift.isConst)
                        {
                            throw new CompilerError($"Argument 2 to \"{feft.funName}\" must be a compile-time constant.", node.argumentList[1].token);
                        }
                        var i8_16x_t = new VectorType(16, Const.i8_t);
                        var zero = builder.ConstNull(i8_16x_t);
                        var bc = builder.BuildBitCast(v, i8_16x_t, node, "vec_bc");
                        var maskValues = new List<Value>();
                        var shiftValue = (int)((ConstInt)shift).data;
                        shiftValue = Math.Clamp(shiftValue, 0, 16);
                        for (int i = 0; i < 16; ++i)
                        {
                            maskValues.Add(new ConstInt(Const.i32_t, (ulong)(shiftValue + i)));
                        }
                        var mask = new ConstVec(new VectorType(16, Const.i32_t), maskValues);
                        var shuffle = builder.BuildShuffleVector(bc, zero, mask, node, "slli_shuffle");
                        var result = builder.BuildBitCast(shuffle, v.type, node, "vec_bc");
                        valueStack.Push(result);
                    }
                    break;
            }
        }

        public void VisitSpecialFunction(AST.FunctionCall node, FrontendFunctionType feft)
        {
            if (node.left.scope.module != null && node.left.scope.module.name == "SIMD")
            {
                VisitSIMDFunction(node, feft);
                return;
            }


            // TODO(pragma): remove string compares
            switch (feft.funName)
            {
                case "__file_pos__":
                    {
                        var callsite = builder.context.callsite;
                        var s = new AST.ConstString(node.left.token, callsite.scope);
                        var fp = node.left.token.FilePosBackendString();
                        if (callsite != null)
                        {
                            fp = callsite.token.FilePosBackendString();
                        }
                        s.s = fp;
                        Visit(s, false);
                    }
                    break;
                case "len":
                    {
                        var at = (FrontendArrayType)typeChecker.GetNodeType(node.argumentList[0]);
                        int length = -1;
                        Debug.Assert(at.dims.Count > 0);
                        Value result;
                        if (at.dims.Count == 1)
                        {
                            length = at.dims.First();
                            result = new ConstInt(mm_t, (ulong)length);
                        }
                        else
                        {
                            if (node.argumentList.Count > 1)
                            {
                                var data = new List<Value>();
                                foreach (var d in at.dims)
                                {
                                    data.Add(new ConstInt(mm_t, (ulong)d));
                                }
                                var arr = new ConstArray(new ArrayType(mm_t, (uint)data.Count), data);
                                Visit(node.argumentList[1]);
                                var idx = valueStack.Pop();
                                if (!idx.isConst)
                                {
                                    throw new CompilerError("Argument 2 of \"len\" must be a compile-time constant.", node.argumentList[1].token);
                                }
                                result = builder.BuildExtractValue(arr, node, "len_extract", idx);
                            }
                            else
                            {
                                ulong mul = 1;
                                foreach (var d in at.dims)
                                {
                                    mul *= (ulong)d;
                                }
                                result = new ConstInt(mm_t, (ulong)mul);
                            }
                        }
                        valueStack.Push(result);
                    }
                    break;
                case "__emit__":
                    {
                        Debug.Assert(node.argumentList.Count == 1);
                        var str = node.argumentList[0] as AST.ConstString;

                        if (str == null)
                        {
                            throw new CompilerError("Argument 1 of \"__emit__\" must be a compile-time constant string.", node.argumentList[0].token);
                        }
                        builder.BuildEmit(str.Verbatim(), node);
                    }
                    break;
                case "atomic_compare_and_swap":
                    {
                        Debug.Assert(node.argumentList.Count == 3);
                        Visit(node.argumentList[0]);
                        var dest = valueStack.Pop();
                        Visit(node.argumentList[1]);
                        var target = valueStack.Pop();
                        Visit(node.argumentList[2]);
                        var comperand = valueStack.Pop();
                        var cmpxchg = builder.BuildCmpxchg(dest, comperand, target, node, "cmpxchg_tuple");
                        var result = builder.BuildExtractValue(cmpxchg, node, "cmpxchg_prev_val", zero_i32_v);
                        valueStack.Push(result);
                    }
                    break;
                case "atomic_add":
                    {
                        Debug.Assert(node.argumentList.Count == 2);
                        Visit(node.argumentList[0]);
                        var ptr = valueStack.Pop();
                        Visit(node.argumentList[1]);
                        var value = valueStack.Pop();
                        var result = builder.BuildAtomicRMW(ptr, value, AtomicRMWType.add, node, "atomic_add");
                        valueStack.Push(result);
                    }
                    break;
                case "atomic_sub":
                    {
                        Debug.Assert(node.argumentList.Count == 2);
                        Visit(node.argumentList[0]);
                        var ptr = valueStack.Pop();
                        Visit(node.argumentList[1]);
                        var value = valueStack.Pop();
                        var result = builder.BuildAtomicRMW(ptr, value, AtomicRMWType.sub, node, "atomic_sub");
                        valueStack.Push(result);
                    }
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }

        public void Visit(AST.FunctionCall node)
        {
            var feft = typeChecker.GetNodeType(node.left) as FrontendFunctionType;

            if (feft != null)
            {
                if (feft.specialFun)
                {
                    VisitSpecialFunction(node, feft);
                    return;
                }
                if (feft.inactiveConditional)
                {
                    return;
                }
            }
            else
            {
                var st = (FrontendSumType)typeChecker.GetNodeType(node.left);
                var vr = (AST.VariableReference)node.left;
                feft = (FrontendFunctionType)st.types[vr.overloadedIdx];
                if (feft.specialFun)
                {
                    VisitSpecialFunction(node, feft);
                    return;
                }
                if (feft.inactiveConditional)
                {
                    return;
                }
            }

            Visit(node.left);
            var f = valueStack.Pop();

            if (!(f is Function))
            {
                f = builder.BuildLoad(f, node, "fun_ptr_load");
            }

            var cnt = feft.parameters.Count; // System.Math.Max(1, feft.parameters.Count);
            Value[] parameters = new Value[cnt];

            var ft = (f.type as PointerType).elementType as FunctionType;
            var rt = ft.returnType;
            var ps = ft.argumentTypes;
            for (int i = 0; i < node.argumentList.Count; ++i)
            {
                Visit(node.argumentList[i]);
                parameters[i] = valueStack.Pop();

                // HACK: RETHINK THIS NONSENSE SOON
                if (!parameters[i].type.EqualType(ps[i]))
                {
                    parameters[i] = builder.BuildBitCast(parameters[i], ps[i], node, "fun_param_hack");
                }
            }

            if (node.argumentList.Count < feft.parameters.Count)
            {
                var fd = typeChecker.GetFunctionDefinition(feft);
                var fts = fd.typeString.functionTypeString;
                for (int idx = node.argumentList.Count; idx < feft.parameters.Count; ++idx)
                {
                    builder.context.SetCallsite(node);
                    Visit(fts.parameters[idx].defaultValueExpression);
                    builder.context.SetCallsite(null);
                    parameters[idx] = valueStack.Pop();
                }
            }

            // http://lists.cs.uiuc.edu/pipermail/llvmdev/2008-May/014844.html
            if (rt.kind == TypeKind.Void)
            {
                builder.BuildCall(f, node, null, parameters);
            }
            else
            {
                var v = builder.BuildCall(f, node, "fun_call", parameters);
                valueStack.Push(v);
            }
        }

        public void Visit(AST.ReturnFunction node)
        {
            var fc = builder.context.currentFunctionContext;
            if (node.expression != null)
            {
                Visit(node.expression);
                var v = valueStack.Pop();
                fc.RegisterReturnEdge(v, builder.GetInsertBlock());
                // builder.BuildRet(v, node);
            }
            else
            {
                builder.BuildRetVoid(node);
            }
            var rb = fc.@return;
            builder.BuildBr(rb, node);
        }

        public void Visit(AST.IfCondition node)
        {
            Visit(node.condition);
            var condition = valueStack.Pop();
            Debug.Assert(SSAType.IsBoolType(condition.type));

            var insert = builder.GetInsertBlock();

            var thenBlock = builder.AppendBasicBlock("then");
            builder.MoveBasicBlockAfter(thenBlock, insert);

            var lastBlock = thenBlock;
            var elifBlocks = new List<Block>();
            var idx = 0;
            foreach (var elif in node.elifs)
            {
                var elifBlock = builder.AppendBasicBlock("elif_" + (idx++));
                builder.MoveBasicBlockAfter(elifBlock, lastBlock);
                lastBlock = elifBlock;
                elifBlocks.Add(elifBlock);
            }

            Block elseBlock = null;
            Block endIfBlock = null;
            if (node.elseBlock != null)
            {
                elseBlock = builder.AppendBasicBlock("else");
                builder.MoveBasicBlockAfter(elseBlock, lastBlock);
                lastBlock = elseBlock;
            }

            endIfBlock = builder.AppendBasicBlock("endif");
            builder.MoveBasicBlockAfter(endIfBlock, lastBlock);
            lastBlock = endIfBlock;

            var nextFail = endIfBlock;
            if (elifBlocks.Count > 0)
            {
                nextFail = elifBlocks.First();
            }
            else if (node.elseBlock != null)
            {
                nextFail = elseBlock;
            }

            builder.BuildCondBr(condition, thenBlock, nextFail, node);

            builder.PositionAtEnd(thenBlock);
            Visit(node.thenBlock);

            if (!builder.GetInsertBlock().HasTerminator())
            {
                builder.BuildBr(endIfBlock, node);
            }

            for (int i = 0; i < elifBlocks.Count; ++i)
            {
                var elif = elifBlocks[i];

                var elifThen = builder.AppendBasicBlock("elif_" + i + "_then");
                builder.MoveBasicBlockAfter(elifThen, elif);

                builder.PositionAtEnd(elif);
                var elifNode = node.elifs[i] as AST.Elif;
                Visit(elifNode.condition);
                var elifCond = valueStack.Pop();


                var nextBlock = endIfBlock;
                if (node.elseBlock != null)
                {
                    nextBlock = elseBlock;
                }
                if (i < elifBlocks.Count - 1)
                {
                    nextBlock = elifBlocks[i + 1];
                }
                builder.BuildCondBr(elifCond, elifThen, nextBlock, node);

                builder.PositionAtEnd(elifThen);
                Visit(elifNode.thenBlock);
                if (!builder.GetInsertBlock().HasTerminator())
                {
                    builder.BuildBr(endIfBlock, node);
                }
            }

            if (node.elseBlock != null)
            {
                builder.PositionAtEnd(elseBlock);
                Visit(node.elseBlock);
                if (!builder.GetInsertBlock().HasTerminator())
                {
                    builder.BuildBr(endIfBlock, node);
                }
            }
            builder.PositionAtEnd(endIfBlock);
        }

        public void Visit(AST.ForLoop node)
        {
            var insert = builder.GetInsertBlock();

            var loopPre = builder.AppendBasicBlock("for_cond");
            builder.MoveBasicBlockAfter(loopPre, insert);
            var loopBody = builder.AppendBasicBlock("for");
            builder.MoveBasicBlockAfter(loopBody, loopPre);
            var loopIter = builder.AppendBasicBlock("for_iter");
            builder.MoveBasicBlockAfter(loopIter, loopBody);
            var endFor = builder.AppendBasicBlock("end_for");
            builder.MoveBasicBlockAfter(endFor, loopIter);

            foreach (var n in node.initializer)
            {
                Visit(n);
            }
            builder.BuildBr(loopPre, node);

            builder.PositionAtEnd(loopPre);
            Visit(node.condition);

            var condition = valueStack.Pop();
            Debug.Assert(SSAType.IsBoolType(condition.type));
            builder.BuildCondBr(condition, loopBody, endFor, node);
            builder.PositionAtEnd(loopBody);

            builder.context.PushLoop(loopIter, endFor);
            Visit(node.loopBody);
            if (!builder.GetInsertBlock().HasTerminator())
            {
                builder.BuildBr(loopIter, node);
            }
            builder.context.PopLoop();

            builder.PositionAtEnd(loopIter);
            foreach (var n in node.iterator)
            {
                Visit(n);
            }

            builder.BuildBr(loopPre, node);
            builder.PositionAtEnd(endFor);
        }

        public void Visit(AST.WhileLoop node)
        {
            var insert = builder.GetInsertBlock();

            var loopPre = builder.AppendBasicBlock("while_cond");
            builder.MoveBasicBlockAfter(loopPre, insert);
            var loopBody = builder.AppendBasicBlock("while");
            builder.MoveBasicBlockAfter(loopBody, loopPre);
            var loopEnd = builder.AppendBasicBlock("while_end");
            builder.MoveBasicBlockAfter(loopEnd, loopBody);

            builder.BuildBr(loopPre, node);

            builder.PositionAtEnd(loopPre);
            Visit(node.condition);

            var condition = valueStack.Pop();
            Debug.Assert(SSAType.IsBoolType(condition.type));
            builder.BuildCondBr(condition, loopBody, loopEnd, node);
            builder.PositionAtEnd(loopBody);

            builder.context.PushLoop(loopPre, loopEnd);
            Visit(node.loopBody);
            builder.context.PopLoop();

            if (!builder.GetInsertBlock().HasTerminator())
            {
                builder.BuildBr(loopPre, node);
            }
            builder.PositionAtEnd(loopEnd);
        }

        HashSet<Block> CalculateReachableBlocks()
        {
            HashSet<Block> reachableBlocks = new HashSet<Block>();
            void CalculateReachableBlocksRec(Block block)
            {
                if (reachableBlocks.Contains(block))
                {
                    return;
                }
                reachableBlocks.Add(block);
                if (block.args.Count > 0)
                {
                    var last = block.args.Last();
                    if (last.op == Op.Br)
                    {
                        foreach (var v in last.args)
                        {
                            if (v is Block target)
                            {
                                CalculateReachableBlocksRec(target);
                            }
                        }
                    }
                }
            }
            var entry = builder.context.currentFunctionContext.entry;
            CalculateReachableBlocksRec(entry);
            return reachableBlocks;
        }

        void BuildReturnBlock(SSAType returnType, AST.Node node)
        {
            var reachableBlocks = CalculateReachableBlocks();

            var fc = builder.context.currentFunctionContext;
            var edges = fc.returnEdges;
            var insertBlock = builder.GetInsertBlock();
            var rb = fc.@return;
            if (returnType.kind != TypeKind.Void && !insertBlock.HasTerminator() && reachableBlocks.Contains(insertBlock))
            {
                throw new CompilerError("Not all codepaths return a value!", node.token);
                // if (returnType.kind != TypeKind.Void) {
                //     var dummy = builder.BuildBitCast(zero_i32_v, returnType, node, "dummy");
                //     fc.RegisterReturnEdge(dummy, insertBlock);    
                // }
                // builder.BuildBr(rb, node);
            }

            var notReachableBlocks = new HashSet<Block>(builder.context.currentFunction.blocks);
            notReachableBlocks.ExceptWith(reachableBlocks);
            foreach (var b in notReachableBlocks)
            {
                if (b != fc.vars && b != fc.@return)
                {
                    builder.RemoveBasicBlock(b);
                }
            }
            edges.RemoveAll(e => notReachableBlocks.Contains(e.Item2));

            if (returnType.kind == TypeKind.Void)
            {
                if (!insertBlock.HasTerminator() && reachableBlocks.Contains(insertBlock))
                {
                    builder.BuildBr(rb, node);
                }
                builder.PositionAtEnd(rb);
                builder.BuildRetVoid(node);
            }
            else
            {
                builder.PositionAtEnd(rb);

                var phi = builder.BuildPhi(returnType, node, "return_phi", edges.ToArray());
                builder.BuildRet(phi, node);
            }
        }

        public void Visit(AST.FunctionDefinition node, bool proto = false)
        {
            var fun = typeChecker.GetNodeType(node) as FrontendFunctionType;
            if (fun.inactiveConditional)
            {
                return;
            }
            if (proto)
            {
                if (node.isFunctionTypeDeclaration())
                {
                    return;
                }
                var funPointer = GetTypeRef(fun) as PointerType;
                var funType = funPointer.elementType as FunctionType;
                Debug.Assert(funPointer != null);
                Debug.Assert(funType != null);

                var functionName = node.externalFunctionName != null ? node.externalFunctionName : node.funName;
                var function = builder.AddFunction(funType, node, functionName, fun.parameters.Select(p => p.name).ToArray());
                function.isStub = node.HasAttribute("STUB");
                if (node.HasAttribute("READNONE"))
                {
                    function.attribs |= FunctionAttribs.readnone;
                }
                if (node.HasAttribute("ARGMEMONLY"))
                {
                    function.attribs |= FunctionAttribs.argmemonly;
                }
                if (node.HasAttribute("LLVM"))
                {
                    function.attribs |= FunctionAttribs.lvvm;
                }
                if (function.isStub)
                {
                    if (function.name != $"@{functionName}")
                    {
                        throw new CompilerError($"STUB name (\"{function.name}\") does not correspond to function name. (\"@{functionName}\")", node.token);
                    }
                }
                variables.Add(node.variableDefinition, function);
            }
            else
            {
                if (node.external || node.body == null)
                {
                    return;
                }

                var function = variables[node.variableDefinition] as Function;

                // TODO(pragma): ugly hack?
                function.debugContextNode = node;
                if (node.HasAttribute("DLL.EXPORT"))
                {
                    function.exportDLL = true;
                }

                var vars = builder.AppendBasicBlock(function, "vars");
                var entry = builder.AppendBasicBlock(function, "entry");
                var @return = builder.AppendBasicBlock(function, "return");

                builder.context.SetFunctionBlocks(function, vars, entry, @return);

                var blockTemp = builder.GetInsertBlock();
                builder.PositionAtEnd(entry);

                if (node.body != null)
                {
                    Visit(node.body);
                }

                var returnType = GetTypeRef(fun.returnType);
                BuildReturnBlock(returnType, node.body);

                builder.PositionAtEnd(vars);
                builder.BuildBr(entry, node.body);

                builder.PositionAtEnd(blockTemp);
            }
        }


        public void Visit(AST.BreakLoop node)
        {
            Debug.Assert(builder.context.IsLoop());
            builder.BuildBr(builder.context.PeekLoop().end, node);
        }

        public void Visit(AST.ContinueLoop node)
        {
            Debug.Assert(builder.context.IsLoop());
            builder.BuildBr(builder.context.PeekLoop().next, node);
        }

        public void Visit(AST.IndexedElementAccess node)
        {
            Visit(node.left);
            var arr = valueStack.Pop();

            var indices = new List<Value>();
            foreach (var idx in node.indices)
            {
                Visit(idx);
                var v = valueStack.Pop();
                indices.Add(v);
            }

            Value arr_elem_ptr = arr;

            Value result = null;
            bool isValue = false;

            var lt = typeChecker.GetNodeType(node.left);
            if (lt is FrontendArrayType at)
            {
                builder.BuildComment("IndexedElementAccess::FrontendArrayType BEGIN", node);
                Value idx = null;
                if (indices.Count == 1)
                {
                    idx = indices[0];
                }
                else
                {
                    var multiply = new int[at.dims.Count];
                    for (int i = 0; i < at.dims.Count; ++i)
                    {
                        multiply[i] = 1;
                        for (int j = i + 1; j < at.dims.Count; ++j)
                        {
                            multiply[i] *= at.dims[j];
                        }
                    }
                    for (int i = 0; i < indices.Count; ++i)
                    {
                        var mp = multiply[i];
                        Value temp;
                        if (mp != 1)
                        {
                            temp = builder.BuildMul(indices[i], new ConstInt(i32_t, (ulong)multiply[i]), node, name: "arr_dim_mul");
                        }
                        else
                        {
                            temp = indices[i];
                        }
                        if (idx != null)
                        {
                            idx = builder.BuildAdd(idx, temp, node, name: "arr_dim_add");
                        }
                        else
                        {
                            idx = temp;
                        }
                    }
                }
                if (arr.op != Op.FunctionArgument && !arr.isConst)
                {
                    result = builder.BuildGEP(arr_elem_ptr, node, "gep_arr_elem", false, zero_i32_v, idx);
                }
                else
                {
                    result = builder.BuildExtractValue(arr, node, "gep_arr_elem_ptr", idx);
                    isValue = true;
                    Debug.Assert(!node.returnPointer);
                }
                builder.BuildComment("IndexedElementAccess::FrontendArrayType END", node);
            }
            else if (lt is FrontendSliceType st)
            {
                builder.BuildComment("IndexedElementAccess::FrontendSliceType BEGIN", node);
                Debug.Assert(indices.Count == 1);
                var idx = indices[0];
                // is not function argument?
                if (arr.type.kind == TypeKind.Pointer)
                {
                    // .data is struct member with idx 2
                    var gep_arr_elem_ptr = builder.BuildGEP(arr, node, "gep_arr_elem_ptr", false, zero_i32_v, two_i32_v);
                    arr_elem_ptr = builder.BuildLoad(gep_arr_elem_ptr, node, "arr_elem_ptr");
                }
                else
                {
                    Debug.Assert(arr.type.kind == TypeKind.Struct);
                    // .data is struct member with idx 2
                    arr_elem_ptr = builder.BuildExtractValue(arr, node, "gep_arr_elem_ptr", two_i32_v);
                }
                var ptr_type = new PointerType(GetTypeRef(st.elementType));
                arr_elem_ptr = builder.BuildBitCast(arr_elem_ptr, ptr_type, node, "arr_elem_ptr_cast");
                var gep_arr_elem = builder.BuildGEP(arr_elem_ptr, node, "gep_arr_elem", false, idx);

                result = gep_arr_elem;
                builder.BuildComment("IndexedElementAccess::FrontendSliceType END", node);
            }
            else if (lt is FrontendVectorType vt)
            {
                Debug.Assert(indices.Count == 1);
                var vec = arr;
                if (arr.op != Op.FunctionArgument)
                {
                    var vecType = ((PointerType)vec.type).elementType;
                    int align = GetMinimumAlignmentForBackend(vecType);
                    Debug.Assert(vecType.kind == TypeKind.Vector);
                    vec = builder.BuildLoad(vec, node, "vec_load", align: align);
                }
                var idx = indices[0];
                result = builder.BuildExtractElement(vec, idx, node, "vec_extract");
                isValue = true;
            }
            else if (lt is FrontendPointerType pt)
            {
                builder.BuildComment("IndexedElementAccess::FrontendPointerType START", node);
                Debug.Assert(indices.Count == 1);
                var idx = indices[0];
                if (arr.op != Op.FunctionArgument && !arr.isConst)
                {
                    arr_elem_ptr = builder.BuildLoad(arr_elem_ptr, node, "arr_elem_ptr");
                    result = builder.BuildGEP(arr_elem_ptr, node, "gep_arr_elem", inBounds: true, idx);
                }
                else
                {
                    result = builder.BuildGEP(arr_elem_ptr, node, "gep_arr_elem", inBounds: true, idx);
                }
                builder.BuildComment("IndexedElementAccess::FrontendPointerType END", node);
            }
            else
            {
                Debug.Assert(false);
            }
            if (!isValue && !node.returnPointer)
            {
                result = builder.BuildLoad(result, node, "arr_elem");
            }
            valueStack.Push(result);
        }



        public void Visit(AST.FieldAccess node)
        {
            Visit(node.left);
            var v = valueStack.Pop();
            FrontendStructType s;
            if (node.IsArrow)
            {
                s = (typeChecker.GetNodeType(node.left) as FrontendPointerType).elementType as FrontendStructType;
            }
            else
            {
                s = typeChecker.GetNodeType(node.left) as FrontendStructType;
            }
            var idx = s.GetFieldIndex(node.fieldName);

            // is not function argument?
            // assume that when its _NOT_ a pointer then it will be a function argument
            if (v.op != Op.FunctionArgument && v.type.kind == TypeKind.Pointer)
            {

                var v_et = ((PointerType)v.type).elementType;
                // HACK: we hit limit of recursive type so just perform bitcast
                if (v_et.kind != TypeKind.Pointer || v_et is PointerType vpt && vpt.elementType.kind != TypeKind.Struct)
                {
                    var sp = new PointerType(GetTypeRef(s));
                    if (node.IsArrow)
                    {
                        sp = new PointerType(sp);
                    }
                    v = builder.BuildBitCast(v, sp, node, "hack_bitcast");
                }
                if (node.IsArrow)
                {
                    v = builder.BuildLoad(v, node, name: "struct_arrow_load");
                }

                Value result;
                result = builder.BuildGEP(v, node, "struct_field_ptr", true, zero_i32_v, new ConstInt(i32_t, (ulong)idx));

                var fe_nt = typeChecker.GetNodeType(node);
                var be_nt = new PointerType(GetTypeRef(fe_nt));

                if (!be_nt.EqualType(result.type))
                {
                    result = builder.BuildBitCast(result, be_nt, node, "hack_cast");
                }
                if (!node.returnPointer)
                {
                    result = builder.BuildLoad(result, node, name: "struct_field", isVolatile: node.IsVolatile);
                }
                else
                {
                    if (node.IsVolatile)
                    {
                        result.flags |= SSAFlags.@volatile;
                    }
                }
                valueStack.Push(result);

                return;
            }
            else
            {
                Value result;
                if (node.IsArrow)
                {
                    result = builder.BuildGEP(v, node, "struct_field_ptr", true, zero_i32_v, new ConstInt(i32_t, (ulong)idx));

                    var fe_nt = typeChecker.GetNodeType(node);
                    var be_nt = new PointerType(GetTypeRef(fe_nt));

                    if (!be_nt.EqualType(result.type))
                    {
                        result = builder.BuildBitCast(result, be_nt, node, "hack_cast");
                    }
                    if (!node.returnPointer)
                    {
                        result = builder.BuildLoad(result, node, name: "struct_arrow", isVolatile: node.IsVolatile);
                    }
                    else
                    {
                        if (node.IsVolatile)
                        {
                            result.flags |= SSAFlags.@volatile;
                        }
                    }
                }
                else
                {
                    var fe_nt = typeChecker.GetNodeType(node);
                    var be_nt = GetTypeRef(fe_nt);

                    uint[] uindices = { (uint)idx };
                    result = builder.BuildExtractValue(v, node, "struct_field_extract", new ConstInt(i32_t, (ulong)idx));
                    if (result.type.kind == TypeKind.Pointer && !result.type.EqualType(be_nt))
                    {
                        result = builder.BuildBitCast(result, be_nt, node, "hack_bitcast");
                    }
                }

                valueStack.Push(result);
                return;
            }
        }

        public void Visit(AST.StructDeclaration node)
        {
        }

        public void Visit(AST.EnumDeclaration node)
        {
            var enumFrontendType = typeChecker.GetNodeType(node) as FrontendEnumType;
            var intType = GetTypeRef(enumFrontendType.integerType);
            foreach (var e in node.entries)
            {
                var vd = node.scope.variables[e.name];
                variables.Add(vd.First, new ConstInt(intType, e.value));
            }
        }

        // TODO(pragma): generate this code
        public void Visit(AST.Node node)
        {
            // TODO(pragma): make a seperate flag for this

            switch (node)
            {
                case AST.ProgramRoot n:
                    Visit(n);
                    break;
                case AST.FileRoot n:
                    Visit(n);
                    break;
                case AST.Module n:
                    Visit(n);
                    break;
                case AST.Block n:
                    Visit(n);
                    break;
                case AST.Elif n:
                    Visit(n);
                    break;
                case AST.IfCondition n:
                    Visit(n);
                    break;
                case AST.ForLoop n:
                    Visit(n);
                    break;
                case AST.WhileLoop n:
                    Visit(n);
                    break;
                case AST.VariableDefinition n:
                    Visit(n);
                    break;
                case AST.FunctionDefinition n:
                    Visit(n);
                    break;
                case AST.CompoundLiteral n:
                    Visit(n);
                    break;
                case AST.StructDeclaration n:
                    Visit(n);
                    break;
                case AST.EnumDeclaration n:
                    Visit(n);
                    break;
                case AST.FunctionCall n:
                    Visit(n);
                    break;
                case AST.VariableReference n:
                    Visit(n);
                    break;
                case AST.Assignment n:
                    Visit(n);
                    break;
                case AST.ConstInt n:
                    Visit(n);
                    break;
                case AST.ConstFloat n:
                    Visit(n);
                    break;
                case AST.ConstBool n:
                    Visit(n);
                    break;
                case AST.ConstString n:
                    Visit(n);
                    break;
                case AST.ArrayConstructor n:
                    Visit(n);
                    break;
                case AST.FieldAccess n:
                    Visit(n);
                    break;
                case AST.IndexedElementAccess n:
                    Visit(n);
                    break;
                case AST.BreakLoop n:
                    Visit(n);
                    break;
                case AST.ContinueLoop n:
                    Visit(n);
                    break;
                case AST.ReturnFunction n:
                    Visit(n);
                    break;
                case AST.BinOp n:
                    Visit(n);
                    break;
                case AST.UnaryOp n:
                    Visit(n);
                    break;
                case AST.TypeCastOp n:
                    Visit(n);
                    break;
                case AST.TypeString n:
                    Visit(n);
                    break;
                case AST.SliceOp n:
                    Visit(n);
                    break;
                default:
                    throw new System.NotImplementedException();
            }
        }

    }
}
