using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Runtime.InteropServices;

using LLVMSharp;
using System.IO;


namespace PragmaScript
{
    partial class Backend
    {


        // NOTE: function signature is broken in LLVMSharp 3.7 so we declare it here manually
        [DllImport("libLLVM.dll", EntryPoint = "LLVMGetBufferStart", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetBufferStart(LLVMMemoryBufferRef @MemBuf);

        public void aotModule()
        {
            int optLevel = CompilerOptions.optimizationLevel;
            Debug.Assert(optLevel >= 0 && optLevel <= 3);
            string arch = null;
            switch (platform) {
                case TargetPlatform.x86:
                    arch = "x86";
                    throw new NotImplementedException();
                    break;
                case TargetPlatform.x64:
                    LLVM.SetDataLayout(mod, "e-m:w-i64:64-f80:128-n8:16:32:64-S128");
                    var target = Marshal.PtrToStringAnsi(LLVM.GetDefaultTargetTriple());
                    LLVM.SetTarget(mod, target);
                    arch = "x86-64";
                    break;
                default:
                    throw new InvalidCodePath();
            }


            var outputDir = Path.GetDirectoryName(CompilerOptions.inputFilename);
            var output = Path.Combine(outputDir, Path.GetFileNameWithoutExtension(CompilerOptions.output));
            var outputTempDir = Path.Combine(outputDir, "obj");
            var outputTemp = Path.Combine(outputTempDir, Path.GetFileNameWithoutExtension(CompilerOptions.output));
            Func<string, string> oxt = (ext) => outputTemp + ext;
            Func<string, string> ox = (ext) => output + ext;

            Directory.CreateDirectory(outputTempDir);

            var error = false;
            {
                IntPtr error_msg;
                error = LLVM.PrintModuleToFile(mod, $"{oxt(".ll")}", out error_msg);
                if (error) {
                    Console.WriteLine(Marshal.PtrToStringAnsi(error_msg));
                }
            }

            if (optLevel > 0) {
                Console.WriteLine($"optimizer... (O{optLevel})");
                var optProcess = new Process();
                optProcess.StartInfo.FileName = RelDir(@"External\opt.exe");
                optProcess.StartInfo.Arguments = $"{oxt(".ll")} -O{optLevel} -march={arch} -mcpu=native -S -o {oxt("_opt.ll")}";
                optProcess.StartInfo.RedirectStandardInput = false;
                optProcess.StartInfo.RedirectStandardOutput = false;
                optProcess.StartInfo.UseShellExecute = false;
                optProcess.Start();
                optProcess.WaitForExit();
                if (optProcess.ExitCode != 0) {
                    error = true;
                }
                optProcess.Close();
            }
            var inp = oxt("_opt.ll");
            if (optLevel == 0) {
                inp = oxt(".ll");
            }
            if (!error && CompilerOptions.asm) {
                Console.WriteLine("assembler...(debug)");
                var llcProcess = new Process();
                llcProcess.StartInfo.FileName = RelDir(@"External\llc.exe");
                llcProcess.StartInfo.Arguments = $"{inp} -O{optLevel} -march={arch} -mcpu=native -filetype=asm -o {oxt(".asm")}";
                llcProcess.StartInfo.RedirectStandardInput = false;
                llcProcess.StartInfo.RedirectStandardOutput = false;
                llcProcess.StartInfo.UseShellExecute = false;
                llcProcess.Start();
                llcProcess.WaitForExit();
                if (llcProcess.ExitCode != 0) {
                    error = true;
                }
                llcProcess.Close();
            }
            if (!error) {
                Console.WriteLine("assembler...");
                var llcProcess = new Process();
                llcProcess.StartInfo.FileName = RelDir(@"External\llc.exe");
                llcProcess.StartInfo.Arguments = $"{inp} -O{optLevel} -march={arch} -mcpu=native -filetype=obj -o {oxt(".o")}";
                llcProcess.StartInfo.RedirectStandardInput = false;
                llcProcess.StartInfo.RedirectStandardOutput = false;
                llcProcess.StartInfo.UseShellExecute = false;
                llcProcess.Start();
                llcProcess.WaitForExit();
                if (llcProcess.ExitCode != 0) {
                    error = true;
                }
                llcProcess.Close();
            }
            if (!error) {
                var libs = String.Join(" ", CompilerOptions.libs);
                var lib_path = String.Join(" ", CompilerOptions.lib_path);

                Console.WriteLine("linker...");
                var lldProcess = new Process();
                lldProcess.StartInfo.FileName = RelDir(@"External\lld-link.exe");
                var flags = "/entry:__init";
                if (CompilerOptions.dll) {
                    flags += $" /dll /out:\"{ox(".dll")}\"";
                } else {
                    flags += $" /subsystem:CONSOLE /out:{ox(".exe")}";
                }

                lldProcess.StartInfo.Arguments = $"{libs} \"{oxt(".o")}\" {flags} /libpath:\"{lib_path}\"";
                lldProcess.StartInfo.RedirectStandardInput = false;
                lldProcess.StartInfo.RedirectStandardOutput = false;
                lldProcess.StartInfo.UseShellExecute = false;
                lldProcess.Start();
                lldProcess.WaitForExit();
                if (lldProcess.ExitCode != 0) {
                    error = true;
                }
                lldProcess.Close();
            }

            if (!error && CompilerOptions.runAfterCompile) {
                Console.WriteLine("running...");
                var outputProcess = new Process();
                var dir = Directory.GetCurrentDirectory();
                var fn = Path.Combine(dir, $"{ox(".exe")}");
                outputProcess.StartInfo.FileName = fn;
                outputProcess.StartInfo.Arguments = "";
                outputProcess.StartInfo.RedirectStandardInput = false;
                outputProcess.StartInfo.RedirectStandardOutput = false;
                outputProcess.StartInfo.UseShellExecute = false;
                outputProcess.Start();
                outputProcess.WaitForExit();
                if (outputProcess.ExitCode != 0) {
                    error = true;
                }
                outputProcess.Close();
            }


            Console.WriteLine("done.");
        }
    }
}
