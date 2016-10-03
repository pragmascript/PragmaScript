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
    // http://denisbider.blogspot.de/2016/04/hello-world-in-llvm-ir-language-without.html
    partial class Backend
    {

        // NOTE: function signature is broken in LLVMSharp 3.7 so we declare it here manually
        [DllImport("libLLVM.dll", EntryPoint = "LLVMGetBufferStart", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetBufferStart(LLVMMemoryBufferRef @MemBuf);

        public void aot(string filename)
        {
            var bitcode = LLVM.WriteBitcodeToMemoryBuffer(mod);
            var bitcodeSize = LLVM.GetBufferSize(bitcode);
            var bitcodeData = new byte[bitcodeSize];
            var bufferStart = GetBufferStart(bitcode);

            //IntPtr error_msg;
            //LLVM.PrintModuleToFile(mod, "output.ll", out error_msg);

            System.Runtime.InteropServices.Marshal.Copy(bufferStart, bitcodeData, 0, bitcodeSize);
            byte[] obj_data = bitcodeData;

            {
                Console.WriteLine("optimizer...");
                var optProcess = new Process();
                optProcess.StartInfo.FileName = @"External\opt.exe";
                optProcess.StartInfo.Arguments = $"-O3 -f";
                optProcess.StartInfo.RedirectStandardInput = true;
                optProcess.StartInfo.RedirectStandardOutput = true;
                optProcess.StartInfo.UseShellExecute = false;
                optProcess.Start();
                var optInput = optProcess.StandardInput;
                var optOutput = optProcess.StandardOutput;
                var bw = new BinaryWriter(optInput.BaseStream);
                bw.Write(bitcodeData, 0, bitcodeSize);
                bw.Close();
                using (var ms = new MemoryStream())
                {
                    optOutput.BaseStream.CopyTo(ms);
                    obj_data = ms.ToArray();
                }
                //using (var fs = new FileStream("output.ll", FileMode.Create))
                //{
                //    optOutput.BaseStream.CopyTo(fs);
                //}
                optProcess.Close();
            }

            {
                Console.WriteLine("assembler...");
                var llcProcess = new Process();
                llcProcess.StartInfo.FileName = @"External\llc.exe";
                llcProcess.StartInfo.Arguments = $"-O3 -filetype obj";
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
                llcProcess.Close();
            }
            {
                Console.WriteLine("linker...");
                var lldProcess = new Process();
                lldProcess.StartInfo.FileName = @"External\lld-link.exe";
                lldProcess.StartInfo.Arguments = $"{filename} kernel32.lib /entry:main /subsystem:console /libpath:\"C:\\Program Files (x86)\\Windows Kits\\8.1\\Lib\\winv6.3\\um\\x64\"";
                lldProcess.StartInfo.RedirectStandardInput = false;
                lldProcess.StartInfo.RedirectStandardOutput = false;
                lldProcess.StartInfo.UseShellExecute = false;
                lldProcess.Start();
                lldProcess.Close();

            }
            Console.WriteLine("done.");
        }
    }
}
