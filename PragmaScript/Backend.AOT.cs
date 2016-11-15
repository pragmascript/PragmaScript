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

        public void aotModule(string filename)
        {
            int optLevel = CompilerOptions.optimizationLevel;
            Debug.Assert(optLevel >= 0 && optLevel <= 3);

            string arch=null;
            switch (platform)
            {
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

            {
                IntPtr error_msg;
                LLVM.PrintModuleToFile(mod, "output.ll", out error_msg);
            }
            if (optLevel > 0)
            {
                Console.WriteLine($"optimizer... (O{optLevel})");
                var optProcess = new Process();
                optProcess.StartInfo.FileName = RelDir(@"External\opt.exe");
                optProcess.StartInfo.Arguments = $"output.ll -O{optLevel} -march={arch} -mcpu=native -S -o output_opt.ll";
                optProcess.StartInfo.RedirectStandardInput = false;
                optProcess.StartInfo.RedirectStandardOutput = false;
                optProcess.StartInfo.UseShellExecute = false;
                optProcess.Start();
                optProcess.WaitForExit();
                optProcess.Close();

            }
            var inp = "output_opt.ll";
            if (optLevel == 0)
            {
                inp = "output.ll";
            }
            if (CompilerOptions.asm)
            {
                Console.WriteLine("assembler...(debug)");
                var llcProcess = new Process();
                llcProcess.StartInfo.FileName = RelDir(@"External\llc.exe");
                llcProcess.StartInfo.Arguments = $"{inp} -O{optLevel} -march={arch} -mcpu=nehalem -filetype=asm -o output.asm";
                llcProcess.StartInfo.RedirectStandardInput = false;
                llcProcess.StartInfo.RedirectStandardOutput = false;
                llcProcess.StartInfo.UseShellExecute = false;
                llcProcess.Start();
                llcProcess.WaitForExit();
                llcProcess.Close();
            }

            {
                Console.WriteLine("assembler...");
                var llcProcess = new Process();
                llcProcess.StartInfo.FileName = RelDir(@"External\llc.exe");
                llcProcess.StartInfo.Arguments = $"{inp} -O{optLevel} -march={arch} -mcpu=native -filetype=obj -o output.o";
                llcProcess.StartInfo.RedirectStandardInput = false;
                llcProcess.StartInfo.RedirectStandardOutput = false;
                llcProcess.StartInfo.UseShellExecute = false;
                llcProcess.Start();
                llcProcess.WaitForExit();
                llcProcess.Close();
            }
            {
                Console.WriteLine("linker...");
                var lldProcess = new Process();
                lldProcess.StartInfo.FileName = RelDir(@"External\lld-link.exe");
                lldProcess.StartInfo.Arguments = $"kernel32.lib user32.lib gdi32.lib output.o /entry:__init /subsystem:CONSOLE  /libpath:\"C:\\Program Files (x86)\\Windows Kits\\8.1\\Lib\\winv6.3\\um\\x64\"";
                lldProcess.StartInfo.RedirectStandardInput = false;
                lldProcess.StartInfo.RedirectStandardOutput = false;
                lldProcess.StartInfo.UseShellExecute = false;
                lldProcess.Start();
                lldProcess.WaitForExit();
                lldProcess.Close();
            }

            if (CompilerOptions.runAfterCompile)
            {
                Console.WriteLine("running...");
                var outputProcess = new Process();
                var dir = Directory.GetCurrentDirectory();
                var fn = Path.Combine(dir, Path.GetFileNameWithoutExtension(filename) + ".exe");
                outputProcess.StartInfo.FileName = fn;
                outputProcess.StartInfo.Arguments = "";
                outputProcess.StartInfo.RedirectStandardInput = false;
                outputProcess.StartInfo.RedirectStandardOutput = false;
                outputProcess.StartInfo.UseShellExecute = false;
                outputProcess.Start();
                outputProcess.WaitForExit();
                outputProcess.Close();
            }


            Console.WriteLine("done.");
        }
    }
}
