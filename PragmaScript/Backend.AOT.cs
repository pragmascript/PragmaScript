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

        public void aotModule(string filename, int optLevel)
        {
            Debug.Assert(optLevel >= 0 && optLevel <= 3);

#if DEBUG
            {
                IntPtr error_msg;
                LLVM.PrintModuleToFile(mod, "output.ll", out error_msg);
            }
#endif

            byte[] obj_data;
            {
                var bitcode = LLVM.WriteBitcodeToMemoryBuffer(mod);
                var bitcodeSize = LLVM.GetBufferSize(bitcode);
                var bufferStart = GetBufferStart(bitcode);
                obj_data = new byte[bitcodeSize];
                System.Runtime.InteropServices.Marshal.Copy(bufferStart, obj_data, 0, bitcodeSize);
            }


            if (optLevel > 0)
            {
                Console.WriteLine("optimizer...");
                var optProcess = new Process();
                optProcess.StartInfo.FileName = @"External\opt.exe";
                optProcess.StartInfo.Arguments = $"-O{optLevel} -f";
                optProcess.StartInfo.RedirectStandardInput = true;
                optProcess.StartInfo.RedirectStandardOutput = true;
                optProcess.StartInfo.UseShellExecute = false;
                optProcess.Start();
                var optInput = optProcess.StandardInput;
                var optOutput = optProcess.StandardOutput;
                var bw = new BinaryWriter(optInput.BaseStream);
                bw.Write(obj_data, 0, obj_data.Length);
                bw.Close();
                using (var ms = new MemoryStream())
                {
                    optOutput.BaseStream.CopyTo(ms);
                    obj_data = ms.ToArray();
                }
                optProcess.WaitForExit();
                optProcess.Close();

#if DEBUG
                {
                    optProcess = new Process();
                    optProcess.StartInfo.FileName = @"External\opt.exe";
                    optProcess.StartInfo.Arguments = $"-disable-opt -S -o output_opt.ll";
                    optProcess.StartInfo.RedirectStandardInput = true;
                    optProcess.StartInfo.RedirectStandardOutput = false;
                    optProcess.StartInfo.UseShellExecute = false;
                    optProcess.Start();
                    optInput = optProcess.StandardInput;
                    bw = new BinaryWriter(optInput.BaseStream);
                    bw.Write(obj_data, 0, obj_data.Length);
                    bw.Close();

                    optProcess.WaitForExit();
                    optProcess.Close();
                }
#endif

            }


            {
                Console.WriteLine("assembler...");
                var llcProcess = new Process();
                llcProcess.StartInfo.FileName = @"External\llc.exe";
                llcProcess.StartInfo.Arguments = $"-O{optLevel} -filetype obj";
                llcProcess.StartInfo.RedirectStandardInput = true;
                llcProcess.StartInfo.RedirectStandardOutput = true;
                llcProcess.StartInfo.UseShellExecute = false;
                llcProcess.Start();
                var llcInput = llcProcess.StandardInput;
                var llcOutput = llcProcess.StandardOutput;

                var bw = new BinaryWriter(llcInput.BaseStream);
                bw.Write(obj_data, 0, obj_data.Length);
                bw.Close();
                using (var fs = new FileStream(filename, FileMode.Create))
                {
                    llcOutput.BaseStream.CopyTo(fs);
                }
                llcProcess.WaitForExit();
                llcProcess.Close();
#if DEBUG
                {
                    //Console.WriteLine("assembler...");
                    llcProcess = new Process();
                    llcProcess.StartInfo.FileName = @"External\llc.exe";
                    llcProcess.StartInfo.Arguments = $"-O{optLevel} --x86-asm-syntax=intel -filetype asm -o output.asm";
                    llcProcess.StartInfo.RedirectStandardInput = true;
                    llcProcess.StartInfo.RedirectStandardOutput = false;
                    llcProcess.StartInfo.UseShellExecute = false;
                    llcProcess.Start();
                    llcInput = llcProcess.StandardInput;
                    bw = new BinaryWriter(llcInput.BaseStream);
                    bw.Write(obj_data, 0, obj_data.Length);
                    bw.Close();
                    llcProcess.WaitForExit();
                    llcProcess.Close();
                }
#endif
            }
            {
                Console.WriteLine("linker...");
                var lldProcess = new Process();
                lldProcess.StartInfo.FileName = @"External\lld-link.exe";
                lldProcess.StartInfo.Arguments = $"kernel32.lib {filename} /entry:__init /subsystem:console  /libpath:\"C:\\Program Files (x86)\\Windows Kits\\10\\Lib\\10.0.10240.0\\ucrt\\x64\" /libpath:\"C:\\Program Files (x86)\\Windows Kits\\8.1\\Lib\\winv6.3\\um\\x64\"";
                lldProcess.StartInfo.RedirectStandardInput = false;
                lldProcess.StartInfo.RedirectStandardOutput = false;
                lldProcess.StartInfo.UseShellExecute = false;
                lldProcess.Start();
                lldProcess.WaitForExit();
                lldProcess.Close();
            }
#if DEBUG
            {
                Console.WriteLine("running...");
                var outputProcess = new Process();
                var fn = Path.GetFileNameWithoutExtension(filename) + ".exe";
                outputProcess.StartInfo.FileName = fn;
                outputProcess.StartInfo.Arguments = "";
                outputProcess.StartInfo.RedirectStandardInput = false;
                outputProcess.StartInfo.RedirectStandardOutput = false;
                outputProcess.StartInfo.UseShellExecute = false;
                outputProcess.Start();
                outputProcess.WaitForExit();
                outputProcess.Close();
            }
#endif

            Console.WriteLine("done.");
        }
    }
}
