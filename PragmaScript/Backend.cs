using LLVMSharp;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace PragmaScript
{


    partial class Backend
    {

        public class ExecutionContext
        {
            public LLVMValueRef function;
            public string functionName;
            public LLVMBasicBlockRef entry;
            public LLVMBasicBlockRef vars;

            public bool loop;
            public LLVMBasicBlockRef loopNext;
            public LLVMBasicBlockRef loopEnd;

            public ExecutionContext(LLVMValueRef function, string functionName, LLVMBasicBlockRef entry, LLVMBasicBlockRef vars)
            {
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
            public static readonly LLVMValueRef OneInt32;
            public static readonly LLVMValueRef OneFloat32;
            public static readonly LLVMValueRef True;
            public static readonly LLVMValueRef False;

            public static readonly LLVMTypeRef Float32Type;
            public static readonly LLVMTypeRef Int32Type;
            public static readonly LLVMTypeRef Int64Type;
            public static readonly LLVMTypeRef Int8Type;
            public static readonly LLVMTypeRef Int8PointerType;
            public static readonly LLVMTypeRef BoolType;
            public static readonly LLVMTypeRef VoidType;

            static Const()
            {
                TrueBool = new LLVMBool(1);
                FalseBool = new LLVMBool(0);

                OneInt32 = LLVM.ConstInt(LLVM.Int32Type(), 1, new LLVMBool(1));
                OneFloat32 = LLVM.ConstReal(LLVM.FloatType(), 1.0);


                NegativeOneInt32 = LLVM.ConstInt(LLVM.Int32Type(), unchecked((ulong)-1), TrueBool);
                ZeroInt32 = LLVM.ConstInt(LLVM.Int32Type(), 0, new LLVMBool(1));
                True = LLVM.ConstInt(LLVM.Int1Type(), (ulong)1, new LLVMBool(0));
                False = LLVM.ConstInt(LLVM.Int1Type(), (ulong)0, new LLVMBool(0));

                Float32Type = LLVM.FloatType();
                Int32Type = LLVM.Int32Type();
                Int64Type = LLVM.Int64Type();

                Int8Type = LLVM.IntType(8);
                Int8PointerType = LLVM.PointerType(LLVM.Int8Type(), 0);

                BoolType = LLVM.Int1Type();
                VoidType = LLVM.VoidType();
            }
        }



        Stack<LLVMValueRef> valueStack = new Stack<LLVMValueRef>();
        Dictionary<string, LLVMValueRef> variables = new Dictionary<string, LLVMValueRef>();
        Dictionary<string, LLVMValueRef> functions = new Dictionary<string, LLVMValueRef>();

        public Stack<ExecutionContext> ctx = new Stack<ExecutionContext>();

        LLVMModuleRef mod;
        LLVMBuilderRef builder;
        LLVMValueRef mainFunction;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int llvm_main();

        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //public delegate void void_del();
        //public static void_del print;

        List<Delegate> functionDelegates = new List<Delegate>();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void void_int_del(int x);
        public static void_int_del print_i32;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void void_byte_del(byte x);
        public static void_byte_del print_i8;

        // http://stackoverflow.com/questions/14106619/passing-delegate-to-a-unmanaged-method-which-expects-a-delegate-method-with-an-i
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void void_float_del(float x);
        public static void_float_del print_f32;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void void_string_del(int length, IntPtr str);
        public static void_string_del print;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr intptr_void_del();
        public static intptr_void_del read_string;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void void_void_del();
        public static void_void_del print_cat;

        public Backend()
        {
            print_i32 += (x) =>
            {
                Console.Write(x);
            };

            print_i8 += (x) =>
            {
                Console.Write(x);
            };

            print_f32 += (x) =>
            {
                Console.Write(x);
            };


            print += (length, ptr) =>
            {
                Console.Write(Marshal.PtrToStringAnsi(ptr, length));
                             
            };

            read_string += () =>
            {
                var str = Console.ReadLine();
                return Marshal.StringToHGlobalAnsi(str);
            };

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

            addDelegate(print_i32, "print_i32");
            addDelegate(print_i8, "print_i8");
            //addDelegate(print_f32, "print_f32");
            addDelegate(print, "print");
            //addDelegate(read_string, "read");
            //addDelegate(print_cat, "cat");
        }


        // TODO: cache struct times at definition time
        static LLVMTypeRef getTypeRef(PragmaScript.AST.FrontendStructType t)
        {
            LLVMTypeRef[] ets = new LLVMTypeRef[t.fields.Count];
            for (int i = 0; i < ets.Length; ++i)
            {
                ets[i] = getTypeRef(t.fields[i].type);
            }

            // TODO packed?
            return LLVM.StructType(ets, true);
        }

        // TODO: cache struct times at definition time
        static LLVMTypeRef getTypeRef(PragmaScript.AST.FrontendArrayType t)
        {
            LLVMTypeRef[] ets = new LLVMTypeRef[2];

            ets[0] = Const.Int32Type;
            ets[1] = LLVM.PointerType(getTypeRef(t.elementType), 0);

            // TODO packed?
            return LLVM.StructType(ets, true);
        }

        static LLVMTypeRef getTypeRef(PragmaScript.AST.FrontendType t)
        {
            if (t.Equals(AST.FrontendType.int32))
            {
                return Const.Int32Type;
            }
            if (t.Equals(AST.FrontendType.int64))
            {
                return Const.Int64Type;
            }
            if  (t.Equals(AST.FrontendType.int8))
            {
                return Const.Int8Type;
            }
            if (t.Equals(AST.FrontendType.float32))
            {
                return Const.Float32Type;
            }
            if (t.Equals(AST.FrontendType.bool_))
            {
                return Const.BoolType;
            }
            if (t.Equals(AST.FrontendType.void_))
            {
                return Const.VoidType;
            }
            if (t.Equals(AST.FrontendType.string_))
            {
                return getTypeRef(t as AST.FrontendArrayType);
            }
            if (t is AST.FrontendArrayType)
            {
                return getTypeRef(t as AST.FrontendArrayType);
            }
            if (t is AST.FrontendStructType)
            {
                return getTypeRef(t as AST.FrontendStructType);
            }
            else
            {
                throw new InvalidCodePath();
            }
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
            functions.Add(name, llvmFuncPtr);
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


        void addPreamble()
        {
            {
                LLVMTypeRef[] main_param_types = { LLVM.Int32Type(), LLVM.Int32Type() };
                LLVMTypeRef main_fun_type = LLVM.FunctionType(LLVM.Int32Type(), out main_param_types[0], 0, Const.FalseBool);
                mainFunction = LLVM.AddFunction(mod, "main", main_fun_type);
                LLVM.AddFunctionAttr(mainFunction, LLVMAttribute.LLVMNoUnwindAttribute);
                LLVMBasicBlockRef vars = LLVM.AppendBasicBlock(mainFunction, "vars");
                LLVMBasicBlockRef entry = LLVM.AppendBasicBlock(mainFunction, "entry");

                var c = new ExecutionContext(mainFunction, "main", entry, vars);
                ctx.Push(c);

            }
            {
                LLVMTypeRef[] param_types = { LLVM.Int32Type() };
                LLVMTypeRef fun_type = LLVM.FunctionType(LLVM.Int64Type(), param_types, Const.FalseBool);
                var fun = LLVM.AddFunction(mod, "GetStdHandle", fun_type);
                LLVM.AddFunctionAttr(fun, LLVMAttribute.LLVMNoUnwindAttribute);
                functions.Add("GetStdHandle", fun);
            }

            var byte_ptr_type = LLVM.PointerType(LLVM.Int8Type(), 0);
            {
                LLVMTypeRef[] param_types = {
                    LLVM.Int64Type(),                      // 0
                    byte_ptr_type,  // 1
                    LLVM.Int32Type(),                      // 2
                    byte_ptr_type, // 3
                    byte_ptr_type,  // 4
                };
                LLVMTypeRef fun_type = LLVM.FunctionType(LLVM.Int32Type(), param_types, Const.FalseBool);
                var fun = LLVM.AddFunction(mod, "WriteFile", fun_type);
                
                var p1 = LLVM.GetParam(fun, 1);
                // LLVM.AddAttribute(p1, LLVMAttribute.LLVMNoCaptureAttribute);
                var p3 = LLVM.GetParam(fun, 3);
                // LLVM.AddAttribute(p3, LLVMAttribute.LLVMNoCaptureAttribute);

                LLVM.AddFunctionAttr(fun, LLVMAttribute.LLVMNoUnwindAttribute);
                functions.Add("WriteFile", fun);
            }
        }

        void prepareModule()
        {
            mod = LLVM.ModuleCreateWithName("WhatIsThisIDontEven");
            addPreamble();
            builder = LLVM.CreateBuilder();
            
            // HACK: 
            LLVM.PositionBuilderAtEnd(builder, ctx.Peek().vars);

            var byte_ptr_type = LLVM.PointerType(Const.Int8Type, 0);
            var nullptr = LLVM.BuildAlloca(builder, byte_ptr_type, "nullptr_alloca");
            variables["nullptr"] = nullptr;

            LLVM.PositionBuilderAtEnd(builder, ctx.Peek().entry);
            LLVM.BuildStore(builder, LLVM.ConstPointerNull(byte_ptr_type), nullptr);
        }

        void emit(AST.Node root)
        {
            prepareModule();
            Visit(root);
            insertMissingReturn(Const.Int32Type);
            LLVM.PositionBuilderAtEnd(builder, ctx.Peek().vars);
            LLVM.BuildBr(builder, ctx.Peek().entry);
        }

        public void EmitAndJIT(AST.Node root, bool useOptimizations)
        {
            emit(root);
            executeModule(useOptimizations);
        }

        public void EmitAndAOT(AST.Node root, string filename)
        {
            emit(root);
            aot(filename);
        }


   }

}
