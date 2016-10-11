using LLVMSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PragmaScript
{
    partial class Backend
    {
        const int OptAggressiveThreshold = 275;
        void jitModule(bool useOptimizationPasses = true)
        {
            IntPtr error;

            var verifyFunction = LLVM.VerifyFunction(mainFunction, LLVMVerifierFailureAction.LLVMReturnStatusAction);
            if (verifyFunction.Value != 0)
            {
                Console.WriteLine("VerifyFunction error!");
            }

            var verifyModule = LLVM.VerifyModule(mod, LLVMVerifierFailureAction.LLVMReturnStatusAction, out error);
            if (verifyModule.Value != 0)
            {
                var s = Marshal.PtrToStringAnsi(error);
                Console.WriteLine("VerifyModule error: " + s);
                Console.WriteLine();
                LLVM.DumpModule(mod);
                return;
            }
            LLVM.DisposeMessage(error);
            LLVMExecutionEngineRef engine;

            LLVM.LinkInMCJIT();
            LLVM.InitializeNativeTarget();
            LLVM.InitializeNativeAsmPrinter();
            LLVM.InitializeNativeAsmParser();

            var platform = Environment.OSVersion.Platform;
            if (platform == PlatformID.Win32NT) // On Windows, LLVM currently (3.6) does not support PE/COFF
            {
                LLVM.SetTarget(mod, Marshal.PtrToStringAnsi(LLVM.GetDefaultTargetTriple()) + "-elf");
            }

            var options = new LLVMMCJITCompilerOptions();
            var optionsSize = (4 * sizeof(int)) + IntPtr.Size; // LLVMMCJITCompilerOptions has 4 ints and a pointer
            options.OptLevel = 3;
            LLVM.InitializeMCJITCompilerOptions(out options, optionsSize);
            var compileError = LLVM.CreateMCJITCompilerForModule(out engine, mod, out options, optionsSize, out error);
            if (compileError.Value != 0)
            {
                LLVM.PrintModuleToFile(mod, "output.ll", out error);
                var s = Marshal.PtrToStringAnsi(error);
                Console.WriteLine();
                Console.WriteLine("error: " + s);
                Console.WriteLine();
                LLVM.DumpModule(mod);
                return;
            }

            LLVMPassManagerRef pass = LLVM.CreatePassManager();
            /*
            < joker - eph > pragmascript: it should look like a sequence of 
            LLVMPassManagerBuilderSetOptLevel, LLVMPassManagerBuilderPopulateFunctionPassManager, and LLVMPassManagerBuilderPopulateModulePassManager
            oh and also LLVMPassManagerBuilderUseInlinerWithThreshold
            (before populating)
             the best way to figure out if your pipeline is correctly setup is to pass -debug-pass=Structure and compare to clang
              echo "" | clang -c -x c - -o /dev/null -O3 -mllvm -debug-pass=Structure  
            */
            if (useOptimizationPasses)
            {
                var pb = LLVM.PassManagerBuilderCreate();
                LLVM.PassManagerBuilderSetOptLevel(pb, 3);
                LLVM.PassManagerBuilderUseInlinerWithThreshold(pb, OptAggressiveThreshold);
                LLVM.PassManagerBuilderPopulateFunctionPassManager(pb, pass);
                LLVM.PassManagerBuilderPopulateModulePassManager(pb, pass);
                LLVM.RunPassManager(pass, mod);
            }
            else
            {
                LLVM.AddVerifierPass(pass);
                LLVM.RunPassManager(pass, mod);
            }

            if (CompilerOptions.debug)
            {
                LLVM.DumpModule(mod);
                IntPtr error_msg;
                LLVM.PrintModuleToFile(mod, "output.ll", out error_msg);
                LLVM.VerifyFunction(mainFunction, LLVMVerifierFailureAction.LLVMPrintMessageAction);
            }

            var main_fun_ptr = LLVM.GetPointerToGlobal(engine, mainFunction);
            var mainFunctionDelegate = (llvm_main)Marshal.GetDelegateForFunctionPointer(main_fun_ptr, typeof(llvm_main));

            // **************************** RUN THE THING **************************** 
            if (CompilerOptions.debug)
            {
                Console.WriteLine();
                Console.WriteLine("PROGRAM OUTPUT: ");
                Console.WriteLine("****************************");
                Console.WriteLine();
            }

            var answer = mainFunctionDelegate();

            if (CompilerOptions.debug)
            {
                Console.WriteLine();
                Console.WriteLine("****************************");
                Console.WriteLine();
            }

            // *********************************************************************** 
            // if (LLVM.WriteBitcodeToFile(mod, "main.bc") != 0)
            // {
            //     Console.WriteLine("error writing bitcode to file, skipping");
            // }
            LLVM.DisposeBuilder(builder);
            LLVM.DisposeExecutionEngine(engine);

            if (CompilerOptions.debug)
            {
                Console.WriteLine();
                Console.WriteLine("THE ANSWER IS: " + answer);
                Console.WriteLine();
            }
        }
    }
}
