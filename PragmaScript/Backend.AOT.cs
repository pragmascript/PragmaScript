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

        public void aot(string filename)
        {
            LLVMPassManagerRef pass = LLVM.CreatePassManager();
            var pb = LLVM.PassManagerBuilderCreate();
            LLVM.PassManagerBuilderSetOptLevel(pb, 3);
            LLVM.PassManagerBuilderUseInlinerWithThreshold(pb, OptAggressiveThreshold);
            LLVM.PassManagerBuilderPopulateFunctionPassManager(pb, pass);
            LLVM.PassManagerBuilderPopulateModulePassManager(pb, pass);
            LLVM.RunPassManager(pass, mod);

            var llcProcess = new Process();


            llcProcess.StartInfo.FileName = @"External\llc.exe";
            llcProcess.StartInfo.Arguments = $"-O3 -filetype obj -o {filename}";
            llcProcess.StartInfo.RedirectStandardInput = true;
            llcProcess.StartInfo.RedirectStandardOutput = true;
            llcProcess.StartInfo.UseShellExecute = false;


            llcProcess.Start();

            var llcInput = llcProcess.StandardInput;
            var llcOutput = llcProcess.StandardOutput;


            //string line = null;
            //do
            //{
            //    line = llcOutput.ReadLine();
            //    Console.WriteLine(line);
            //} while (line != null);

            var bitcode = LLVM.WriteBitcodeToMemoryBuffer(mod);

            var bitcodeSize = LLVM.GetBufferSize(bitcode);
            var bitcodeData = new byte[bitcodeSize];
            var bufferStart = GetBufferStart(bitcode);
            System.Runtime.InteropServices.Marshal.Copy(bufferStart, bitcodeData, 0, bitcodeSize);

            var bw = new BinaryWriter(llcInput.BaseStream);
            bw.Write(bitcodeData, 0, bitcodeSize);
            bw.Close();
            
            string line = null;
            do
            {
                line = llcOutput.ReadLine();
                Console.WriteLine(line);
            } while (line != null);

            Console.WriteLine("done");
            Console.ReadLine();


            // lld -flavor link output.o /ENTRY:main
            // -L"C:\Program Files\Microsoft Platform SDK\Lib\"
            // http://denisbider.blogspot.de/2016/04/hello-world-in-llvm-ir-language-without.html
        }
    }
}
