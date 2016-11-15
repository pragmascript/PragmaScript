using LLVMSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace PragmaScript
{


    partial class Backend
    {

        public enum TargetPlatform { x86, x64 }
        TargetPlatform platform;

        public class ExecutionContext
        {
            public bool global = false;
            public LLVMValueRef function;
            public string functionName;
            public LLVMBasicBlockRef entry;
            public LLVMBasicBlockRef vars;

            public bool loop;
            public LLVMBasicBlockRef loopNext;
            public LLVMBasicBlockRef loopEnd;


            public ExecutionContext(LLVMValueRef function, string functionName, LLVMBasicBlockRef entry, LLVMBasicBlockRef vars, bool global = false)
            {
                this.global = global;
                this.function = function;
                this.functionName = functionName;
                this.entry = entry;
                this.vars = vars;
                loop = false;
                loopNext = default(LLVMBasicBlockRef);
                loopEnd = default(LLVMBasicBlockRef);
            }

            public ExecutionContext(ExecutionContext other)
            {
                global = false;
                function = other.function;
                functionName = other.functionName;
                entry = other.entry;
                vars = other.vars;
                loop = other.loop;
                loopNext = other.loopNext;
                loopEnd = other.loopEnd;
            }

        }


        public static class Const
        {
            public static readonly LLVMBool TrueBool;
            public static readonly LLVMBool FalseBool;
            public static readonly LLVMValueRef NegativeOneInt32;
            public static readonly LLVMValueRef ZeroInt32;
            public static readonly LLVMValueRef ZeroInt64;
            public static readonly LLVMValueRef OneInt32;
            public static readonly LLVMValueRef NegOneInt32;
            public static readonly LLVMValueRef OneFloat32;
            public static readonly LLVMValueRef True;
            public static readonly LLVMValueRef False;
            public static readonly LLVMValueRef NullPtr;

            public static readonly LLVMTypeRef Float32Type;
            public static readonly LLVMTypeRef Float64Type;
            public static readonly LLVMTypeRef Int16Type;
            public static readonly LLVMTypeRef Int32Type;
            public static readonly LLVMTypeRef Int64Type;
            public static readonly LLVMTypeRef Int8Type;
            public static LLVMTypeRef mm;
            public static readonly LLVMTypeRef Int8PointerType;
            public static readonly LLVMTypeRef BoolType;
            public static readonly LLVMTypeRef VoidType;

            static Const()
            {
                TrueBool = new LLVMBool(1);
                FalseBool = new LLVMBool(0);

                OneInt32 = LLVM.ConstInt(LLVM.Int32Type(), 1, new LLVMBool(1));
                unchecked
                {
                    NegOneInt32 = LLVM.ConstInt(LLVM.Int32Type(), (ulong)-1, new LLVMBool(1));
                }
                OneFloat32 = LLVM.ConstReal(LLVM.FloatType(), 1.0);


                NegativeOneInt32 = LLVM.ConstInt(LLVM.Int32Type(), unchecked((ulong)-1), TrueBool);
                ZeroInt32 = LLVM.ConstInt(LLVM.Int32Type(), 0, new LLVMBool(1));
                ZeroInt64 = LLVM.ConstInt(LLVM.Int64Type(), 0, new LLVMBool(1));

                True = LLVM.ConstInt(LLVM.Int1Type(), (ulong)1, new LLVMBool(0));
                False = LLVM.ConstInt(LLVM.Int1Type(), (ulong)0, new LLVMBool(0));

                Float32Type = LLVM.FloatType();
                Float64Type = LLVM.DoubleType();

                Int16Type = LLVM.Int16Type();
                Int32Type = LLVM.Int32Type();
                Int64Type = LLVM.Int64Type();
                Int8Type = LLVM.IntType(8);


                Int8PointerType = LLVM.PointerType(LLVM.Int8Type(), 0);



                NullPtr = LLVM.ConstPointerNull(Int8PointerType);

                BoolType = LLVM.Int1Type();
                VoidType = LLVM.VoidType();
            }

            public static void Init(TargetPlatform platform)
            {
                switch (platform)
                {
                    case TargetPlatform.x86:
                        mm = LLVM.Int32Type();
                        break;
                    case TargetPlatform.x64:
                        mm = LLVM.Int64Type();
                        break;
                }
            }
        }



        Stack<LLVMValueRef> valueStack = new Stack<LLVMValueRef>();
        Dictionary<string, LLVMValueRef> variables = new Dictionary<string, LLVMValueRef>();

        public Stack<ExecutionContext> ctx = new Stack<ExecutionContext>();

        LLVMModuleRef mod;
        LLVMBuilderRef builder;
        //  LLVMValueRef mainFunction;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int llvm_main();

        List<Delegate> functionDelegates = new List<Delegate>();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void void_void_del();
        public static void_void_del print_cat;


        public TypeChecker typeChecker;

        static string exeDir;
        public static string RelDir(string dir)
        {
            string result = Path.Combine(exeDir, dir);
            return result;
        }

        public Backend(TargetPlatform platform, TypeChecker typeChecker)
        {
            exeDir = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

            this.typeChecker = typeChecker;
            this.platform = platform;

            print_cat += () =>
            {
                var str =
                @"
:~:~:~:::::::::::,,,,::::::::,:,,.:,,...........,,,.,,:==~~:::~~:,:~~==+===++++
::~~=====:::~:::::::::::::::::::,.::,............,,.,,,~~=~~:::::,::~===+==++++
:::========~,:~:~,:::::::::::::::,::,,...........,,,,,,:~===:::~:,::~~==+==++++
,::========+=~::~:,::~~:::::::::,,,~:,...........,,,,,,,~====::~~::::~=+++=++++
:,~~~=======+=:::~:,::::::::::::~~:,,,,...........,,,:,,:~=++=::~::~:~====+++++
:,=~=======+++=:,:~:,,,:::::~~~~===,:,,,.,.....,,.,,,:,,,~=++=~:~~==~~~~===++++
,:=~===+=++++===~:,::,:,:::::::~~==~,::,......,.,.,,,,:,::=+++======~~===~+++++
,,=~~==+++++++=++~::::,::::,,,,:~~=~:,:,,,,.,,,,,,,,,,:,::~=+++===~~=~====~++++
,,====++++++++++=+=:::::~~,,::::~~::::::,,,,,.,,,,,,,,:,::~=+=+====~~~=====~+++
:,=====++++++++++=+=~~:~::::~~~~~==~~~::,,,,.,,,,,,,,,:::~=+=+++=====~~~=====++
:~======+++++++++======~~======~:~~~~::::,,,,,,::,,:,,::~=+==+======~~~~=====++
::=====++++++++=+======++=======:::~===~=~::::~~~~~~=~==~===++========~~=====++
::===++++++++=++++++++==+=====+===========~::~:===~:~=======++===~=~~~~~=====++
::~=========+++++++++++=++++++++++++++====+~::~==~:===~==============~~~=====++
~~~========++++++++++++++++==+++++===++++~=~~:~=+:~++=============~~~~====+===~
~~~=+==+++=++++++++++++++======+++===++++=~==::==:=++==++++=======~~=~~==+==~~~
,,,,==+++===++++++++++========+++++===+++====:~==~=++==+++++====~~~~~~=++=~~~~~
:,,::=+=====+==++++++++++======+++++==+++=+==~~~===++===+++=====~~~=~===~~~~~==
,:,,:+==~===++++++++++++==::::::~=+++++++=++==~====++===+++=++===~~~~==~~~~~=++
~,:::=+===+++++++++++++=~::::~~:,,:=+??++=++===++====+++=+========~~~=~~~~~=+=+
::::~~~=++++++++++++++=:,~~~:,,~~~,:~+???++++==++===+++~:,..,:~==+=~~~~~:~===++
~~~::~=+++++++===+++++=,~~:,...,:~~::+???++++==+++=+++:,,,:::,,======~~:~~===++
~~=~~=++++++====+++++?+,~=,,:,..,:=:,~+??+++++++++=++=,,,.,,:~:~=+++==~~~====++
~~:~=+++++++====+++++++~,:,,:,.,,~~:,,=+??+++++++++++~,,,,,,,~I,==+==~~~~~====+
~~~=++=+++=====++++++++?~,:,...,:~~,,,:+?++++==~===++:,~?,,,.:+::+====~~:~~===+
+===+=========++++++===+++~,,,:~~~,,,,~==++======~===~,:,,..,:~,:++===~~:~~====
+=======~~===+++++++===~~~====~~~===~~~=+==~===~~~~~==:::,,,::,,======~::~~~===
+=========+++++++++++==+==~~:~~===++=~~=====~::::~~~~~=~=+~::~===~~~~~~::~~~===
+====++++++++??+++++++++++==========~~=====~:,,,:::,:~~=~=++=~~::~~~=~~~~~~~===
+==+++??++++?????++++++++=+===++=====+++++:,,:,::::::~~==~===~~~~==~====~~~~===
+++++????++????????+++=+++++++++++++++++++==~,.,:::.,~++=======~~=~======~~~===
+=++????????????+++++++?++++++++++++==+++++===~:::::=++++++========~===+=====~~
++++???++++?++?+++????????+++++=+++++?+++++++++~,,:+++++++=====+=======+++=~~~~
++++??????????????????????????????+++=+++++++==~::~++++++++=~~=+=======++=~~:::
+++++??????????????????????+??+++++++++++++===~:,:~==+++++=+++=======++++=~~~~~
++++++????????????????????????????+++++++++==~~:,,:~=+++++++=++=+=====+++=~:~~~
++++++++????????????????????????+++++++++===~:,,.,,~==++++++++++===+++++=~::~~~
+++?+++??????????++?????????????+++++++===~::::,,,,::==+++++++++===+++++=::~~==
++?????????????+++++?????+++?+++++++++==~::~==~~~~~~::===+++++++===++++=~:~~~==
++++????++++++++++++++++++++++++++++===~~============:~==+++++++++++++==::~~=~~
+++??+?+++++++++++++++++++++++++========++============~==+++++++++++++=:~~~~~~~
++++++++++++++++++++++++++++++++++++++++++++++==+++======++++++++++++=~::~~~~~~
++++++++++++++++++++++++++++++++++++++++++++++==+++++++++=+++=+++++++=~~~~~~~~~
?????++++++++++++++++++++++++++++++++++++++++==+++++++=======++++++==~~::~~~~::
??????+++++++++++++++++++++++++++++++++++++++==+===============+++==~~~~~:~::~~
??????++++++++++++++++++++++++++++++++++++====================++++=~:~~~~::::~~
?????????++++++++++++++++++++++++++++++++=====~============+===++=~:::~~~:::~~~
??????????+++++++++++++++++++===+=++==========~=================~~:::::~::::~~~
????????????+++++++++++++++=========~=======~===================~:::::::::~~~~~
??????????????+++++++++++++=+=++==============~~~~~~~~~~~~~====~~::::::,:::~~~~";
                Console.Write(str);
            };


            Const.Init(platform);
        }


        // TODO: cache struct times at definition time
        static LLVMTypeRef getTypeRef(FrontendStructType t, int depth)
        {
            LLVMTypeRef[] ets = new LLVMTypeRef[t.fields.Count];
            for (int i = 0; i < ets.Length; ++i)
            {
                ets[i] = getTypeRef(t.fields[i].type, depth + 1);
            }

            // TODO packed?
            return LLVM.StructType(ets, true);
        }

        // TODO: cache struct times at definition time
        static LLVMTypeRef getTypeRef(FrontendArrayType t, int depth)
        {
            LLVMTypeRef[] ets = new LLVMTypeRef[2];

            ets[0] = Const.Int32Type;
            ets[1] = LLVM.PointerType(getTypeRef(t.elementType, depth), 0);

            return LLVM.StructType(ets, true);
        }

        static LLVMTypeRef getTypeRef(FrontendPointerType t, int depth)
        {
            if (depth > 0 &&t.elementType is FrontendStructType)
            {
                return Const.Int8PointerType;
            }
            else
            {
                var et = getTypeRef(t.elementType, depth);
                return LLVM.PointerType(et, 0);
            }
        }

        static LLVMTypeRef getTypeRef(FrontendFunctionType t, int depth)
        {
            var fun = t;
            var cnt = Math.Max(1, fun.parameters.Count);
            var par = new LLVMTypeRef[cnt];

            // TODO: what if we have a recursive function parameter here?
            // do i need to inc depth?
            for (int i = 0; i < fun.parameters.Count; ++i)
            {
                par[i] = getTypeRef(fun.parameters[i].type, depth);
            }
            var returnType = getTypeRef(fun.returnType, 0);
            var funType = LLVM.FunctionType(returnType, out par[0],
                            (uint)fun.parameters.Count, Const.FalseBool);

            return LLVM.PointerType(funType, 0);
        }


        static LLVMTypeRef GetTypeRef(FrontendType t)
        {
            return getTypeRef(t, 0);
        }

        static LLVMTypeRef getTypeRef(FrontendType t, int depth)
        {
            if (t.Equals(FrontendType.i16))
            {
                return Const.Int16Type;
            }
            if (t.Equals(FrontendType.i32))
            {
                return Const.Int32Type;
            }
            if (t.Equals(FrontendType.i64))
            {
                return Const.Int64Type;
            }
            if (t.Equals(FrontendType.i8))
            {
                return Const.Int8Type;
            }
            if (t.Equals(FrontendType.mm))
            {
                return Const.mm;
            }
            if (t.Equals(FrontendType.f32))
            {
                return Const.Float32Type;
            }
            if (t.Equals(FrontendType.f64))
            {
                return Const.Float64Type;
            }
            if (t.Equals(FrontendType.bool_))
            {
                return Const.BoolType;
            }
            if (t.Equals(FrontendType.void_))
            {
                return Const.VoidType;
            }
            if (t.Equals(FrontendType.string_))
            {
                return getTypeRef(t as FrontendArrayType, depth);
            }
            if (t is FrontendArrayType)
            {
                return getTypeRef(t as FrontendArrayType, depth);
            }
            if (t is FrontendStructType)
            {
                return getTypeRef(t as FrontendStructType, depth);
            }
            if (t is FrontendPointerType)
            {
                return getTypeRef(t as FrontendPointerType, depth);
            }
            if (t is FrontendFunctionType)
            {
                return getTypeRef(t as FrontendFunctionType, depth);
            }
            throw new InvalidCodePath();
        }

        static LLVMTypeRef getTypeRef(Type t)
        {
            if (t == typeof(Int32))
            {
                return Const.Int32Type;
            }
            else if (t == typeof(float))
            {
                return Const.Float32Type;
            }
            else if (t == typeof(void))
            {
                return Const.VoidType;
            }
            else if (t == typeof(IntPtr))
            {
                return Const.Int8PointerType;
            }
            else if (t == typeof(byte))
            {
                return Const.Int8Type;
            }
            else
            {
                throw new InvalidCodePath();
            }
        }

        public static bool isEqualType(LLVMTypeRef a, LLVMTypeRef b)
        {
            return a.Pointer == b.Pointer;
        }

        public static string typeToString(LLVMTypeRef t)
        {
            return Marshal.PtrToStringAnsi(LLVM.PrintTypeToString(t));
        }

        void addDelegate<T>(T del, string name) where T : class
        {
            if (!typeof(T).IsSubclassOf(typeof(Delegate)))
            {
                throw new InvalidOperationException(typeof(T).Name + " is not a delegate type");
            }
            var info = typeof(T).GetMethod("Invoke");
            var parameters = info.GetParameters();


            LLVMTypeRef[] param_types = new LLVMTypeRef[Math.Max(parameters.Length, 1)];

            for (int i = 0; i < parameters.Length; ++i)
            {
                var p = parameters[i];
                var pt = p.ParameterType;
                param_types[i] = getTypeRef(pt);
            }

            var returnTypeRef = getTypeRef(info.ReturnType);

            var fun_type = LLVM.FunctionType(returnTypeRef, param_types, Const.FalseBool);
            IntPtr functionPtr = Marshal.GetFunctionPointerForDelegate(del as Delegate);
            var llvmFuncPtr = LLVM.ConstIntToPtr(LLVM.ConstInt(LLVM.Int64Type(), (ulong)functionPtr, Const.FalseBool), LLVM.PointerType(fun_type, 0));
            variables.Add(name, llvmFuncPtr);
        }

        void insertMissingReturn(LLVMTypeRef returnType)
        {
            var term = LLVM.GetBasicBlockTerminator(LLVM.GetInsertBlock(builder));
            if (term.Pointer == IntPtr.Zero)
            {
                if (isEqualType(returnType, Const.VoidType))
                {
                    LLVM.BuildRetVoid(builder);
                }
                else
                {
                    var dummy = LLVM.BuildBitCast(builder, Const.ZeroInt32, returnType, "dummy");
                    LLVM.BuildRet(builder, dummy);
                }
            }
        }

        // http://stackoverflow.com/questions/11985247/llvm-insert-intrinsic-function-cos
        // http://stackoverflow.com/questions/27681500/generate-call-to-intrinsic-using-llvm-c-api
        void addIntrinsics()
        {
            {
                LLVMTypeRef[] param_types = { Const.Float32Type };
                LLVMTypeRef fn_type = LLVM.FunctionType(Const.Float32Type, out param_types[0], 1, false);
                LLVMValueRef fn = LLVM.AddFunction(mod, "llvm.cos.f32", fn_type);
                variables.Add("cos", fn);
            }

            {
                LLVMTypeRef[] param_types = { Const.Float32Type };
                LLVMTypeRef fn_type = LLVM.FunctionType(Const.Float32Type, out param_types[0], 1, false);
                LLVMValueRef fn = LLVM.AddFunction(mod, "llvm.sin.f32", fn_type);
                variables.Add("sin", fn);
            }

            {
                LLVMTypeRef[] param_types = { Const.Float32Type };
                LLVMTypeRef fn_type = LLVM.FunctionType(Const.Float32Type, out param_types[0], 1, false);
                LLVMValueRef fn = LLVM.AddFunction(mod, "llvm.fabs.f32", fn_type);
                variables.Add("abs", fn);
            }
            {
                LLVMTypeRef[] param_types = { Const.Float32Type };
                LLVMTypeRef fn_type = LLVM.FunctionType(Const.Float32Type, out param_types[0], 1, false);
                LLVMValueRef fn = LLVM.AddFunction(mod, "llvm.sqrt.f32", fn_type);
                variables.Add("sqrt", fn);
            }

            {
                LLVMTypeRef[] param_types = { Const.Float32Type };
                LLVMTypeRef fn_type = LLVM.FunctionType(Const.Float32Type, out param_types[0], 1, false);
                LLVMValueRef fn = LLVM.AddFunction(mod, "llvm.floor.f32", fn_type);
                variables.Add("floor", fn);
            }
            {
                LLVMTypeRef[] param_types = { Const.Float32Type };
                LLVMTypeRef fn_type = LLVM.FunctionType(Const.Float32Type, out param_types[0], 1, false);
                LLVMValueRef fn = LLVM.AddFunction(mod, "llvm.trunc.f32", fn_type);
                variables.Add("trunc", fn);
            }
            {
                LLVMTypeRef[] param_types = { Const.Float32Type };
                LLVMTypeRef fn_type = LLVM.FunctionType(Const.Float32Type, out param_types[0], 1, false);
                LLVMValueRef fn = LLVM.AddFunction(mod, "llvm.ceil.f32", fn_type);
                variables.Add("ceil", fn);
            }
            {
                LLVMTypeRef[] param_types = { Const.Float32Type };
                LLVMTypeRef fn_type = LLVM.FunctionType(Const.Float32Type, out param_types[0], 1, false);
                LLVMValueRef fn = LLVM.AddFunction(mod, "llvm.round.f32", fn_type);
                variables.Add("round", fn);
            }
        }



        void addPreamble()
        {

            var byte_ptr_type = LLVM.PointerType(LLVM.Int8Type(), 0);
            {
                LLVMTypeRef[] param_types = {
                    LLVM.Int64Type(),    // 0
                    byte_ptr_type,       // 1
                    LLVM.Int32Type(),    // 2
                    LLVM.PointerType(LLVM.Int32Type(), 0),       // 3
                    byte_ptr_type,       // 4
                };
                LLVMTypeRef fun_type = LLVM.FunctionType(LLVM.Int32Type(), param_types, Const.FalseBool);
                var fun = LLVM.AddFunction(mod, "WriteFile", fun_type);

                var p1 = LLVM.GetParam(fun, 1);
                LLVM.AddAttribute(p1, LLVMAttribute.LLVMNoCaptureAttribute);
                var p3 = LLVM.GetParam(fun, 3);
                LLVM.AddAttribute(p3, LLVMAttribute.LLVMNoCaptureAttribute);

                LLVM.AddFunctionAttr(fun, LLVMAttribute.LLVMNoUnwindAttribute);
                variables.Add("WriteFile", fun);
            }

            {
                LLVMTypeRef[] param_types = {
                    LLVM.Int64Type(),   // 0
                    byte_ptr_type,      // 1
                    LLVM.Int32Type(),   // 2
                    LLVM.PointerType(LLVM.Int32Type(), 0),       // 3
                    byte_ptr_type,      // 4
                };
                LLVMTypeRef fun_type = LLVM.FunctionType(LLVM.Int32Type(), param_types, Const.FalseBool);
                var fun = LLVM.AddFunction(mod, "ReadFile", fun_type);

                var p1 = LLVM.GetParam(fun, 1);
                LLVM.AddAttribute(p1, LLVMAttribute.LLVMNoCaptureAttribute);
                var p3 = LLVM.GetParam(fun, 3);
                LLVM.AddAttribute(p3, LLVMAttribute.LLVMNoCaptureAttribute);

                LLVM.AddFunctionAttr(fun, LLVMAttribute.LLVMNoUnwindAttribute);
                variables.Add("ReadFile", fun);
            }
        }

        void prepareModule()
        {
            // mod = LLVM.ModuleCreateWithName("WhatIsThisIDontEven");
            LLVMMemoryBufferRef buf;
            IntPtr msg;
            LLVM.CreateMemoryBufferWithContentsOfFile(RelDir(@"Programs\ll\preamble.ll"), out buf, out msg);
            if (msg != IntPtr.Zero)
            {
                Console.WriteLine(Marshal.PtrToStringAnsi(msg));
            }


            var ctx = LLVM.GetGlobalContext();
            LLVM.ParseIRInContext(ctx, buf, out mod, out msg);

            if (msg != IntPtr.Zero)
            {
                Console.WriteLine(Marshal.PtrToStringAnsi(msg));
            }
            
            var fun = LLVM.GetNamedFunction(mod, "VirtualAlloc");
            Debug.Assert(fun.Pointer != IntPtr.Zero);
            variables.Add("VirtualAlloc", fun);

            fun = LLVM.GetNamedFunction(mod, "_rdtsc");
            Debug.Assert(fun.Pointer != IntPtr.Zero);
            variables.Add("_rdtsc", fun);

            fun = LLVM.GetNamedFunction(mod, "__chkstk");
            Debug.Assert(fun.Pointer != IntPtr.Zero);
            variables.Add("__chkstk", fun);

            // Console.WriteLine(Marshal.PtrToStringAnsi(msg));
            //LLVM.LinkModules(mod, m, LLVMLinkerMode.LLVMLinkerDestroySource, out msg);
            //Console.WriteLine(Marshal.PtrToStringAnsi(msg));
            addIntrinsics();
            addPreamble();


            builder = LLVM.CreateBuilder();

            // HACK: 
            var nullptr = LLVM.ConstPointerNull(LLVM.PointerType(LLVM.Int8Type(), 0));
            variables["nullptr"] = nullptr;
        }


        void emit(AST.Node root)
        {
            prepareModule();
            Visit(root);
        }


        public void EmitAndAOT(AST.Node root, string filename)
        {
            emit(root);
            aotModule(filename);
        }
    }
}
