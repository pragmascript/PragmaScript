using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using static PragmaScript.SSA;
using static PragmaScript.SSA.Const;

namespace PragmaScript {
    partial class Backend {

        StringBuilder sb = new StringBuilder();
        string EscapeString(string s) {
            StringBuilder result = new StringBuilder();
            foreach (var c in s) {
                var x = (int)c;
                if (x >= 32 && x <= 126 && c != '"' && c != '\\') {
                    result.Append(c);
                } else {
                    result.Append("\\" + x.ToString("X2"));
                }
            }
            return result.ToString();
        }
        void AL(string line) {
            sb.AppendLine(line);
        }
        void AL() {
            sb.AppendLine();
        }
        void AP(string s) {
            sb.Append(s);
        }

        public void AOT() {
            sb = new StringBuilder();
            AL("target datalayout = \"e-m:w-i64:64-f80:128-n8:16:32:64-S128\"");
            AL("target triple = \"x86_64-pc-windows-msvc\"");
            AL();
            foreach (var v in mod.globals.args) {
                Debug.Assert(v is GlobalVariable || v is GlobalStringPtr || v is Function);
                if (v is GlobalVariable || v is GlobalStringPtr) {
                    AppendOp(v);
                }
            }
            foreach (var v in mod.globals.args) {
                if (v is Function) {
                    AppendOp(v);
                }
            }
            System.IO.File.WriteAllText("output.ll", sb.ToString());
        }


        void AppendInitializer(SSA.Value v) {
            switch (v) {
                case ConstInt i:
                    var it = i.type as IntegerType;
                    if (it.bitWidth == 1) {
                        if (i.data == 0) {
                            AP("false");
                        } else {
                            AP("true");
                        }
                    } else {
                        AP(i.data.ToString());
                    }
                    break;
                case ConstReal r:
                    AP(r.data.ToString());
                    break;
                case ConstPtr p:
                    if (p.data == 0) {
                        AP("null");
                    } else {
                        AP(p.data.ToString());
                    }
                    break;
                case Value caz when caz.op == Op.ConstAggregateZero:
                    AP("zeroinitializer");
                    break;
                default:
                    throw new InvalidCodePath();
            }
        }

        void AppendType(SSAType ssat) {
            switch (ssat) {
                case VoidType t:
                    AP("void");
                    break;
                case IntegerType t:
                    AP($"i{t.bitWidth}");
                    break;
                case FloatType t:
                    switch (t.width) {
                        case FloatType.FloatWidths.fp16:
                            AP("half");
                            break;
                        case FloatType.FloatWidths.fp32:
                            AP("float");
                            break;
                        case FloatType.FloatWidths.fp64:
                            AP("double");
                            break;
                    }
                    break;
                case PointerType t:
                    AppendType(t.elementType);
                    AP("*");
                    break;
                case ArrayType t:
                    AP($"[{t.elementCount} x ");
                    AppendType(t.elementType);
                    AP("]");
                    break;
                case StructType t:
                    if (t.packed) {
                        AP("<");
                    }
                    AP("{ ");
                    for (int idx = 0; idx < t.elementTypes.Count; ++idx) {
                        var et = t.elementTypes[idx];
                        AppendType(et);
                        if (idx != t.elementTypes.Count - 1) {
                            AP(", ");
                        }
                    }
                    AP(" }");
                    if (t.packed) {
                        AP(">");
                    }
                    break;
                case FunctionType t:
                    AppendType(t.returnType);
                    AP(" (");
                    for (int idx = 0; idx < t.argumentTypes.Count; ++idx) {
                        var et = t.argumentTypes[idx];
                        AppendType(et);
                        if (idx != t.argumentTypes.Count - 1) {
                            AP(", ");
                        }
                    }
                    AP(")");
                    break;
            }
        }

        void AppendOp(SSA.Value v) {
            switch (v.op) {
                case Op.GlobalStringPtr: {
                        var gsp = (GlobalStringPtr)v;
                        var es = EscapeString(gsp.data);
                        AL($"{gsp.name} = private unnamed_addr constant [{gsp.data.Length + 1} x i8] c\"{es}\\00\"");
                    }
                    break;
                case Op.GlobalVariable: {
                        var gv = (GlobalVariable)v;
                        AP($"{gv.name} = internal global {(gv.isConst ? "constant " : "")}");
                        var pt = (PointerType)gv.type;
                        AppendType(pt.elementType);
                        AP(" ");
                        AppendInitializer(gv.initializer);
                        AL();
                    }
                    break;
                case Op.Function: {
                        var f = (Function)v;
                        bool declare;
                        if (f.blocks == null || f.blocks.Count == 0) {
                            AP("declare ");
                            declare = true;
                        } else {
                            AP("define ");
                            declare = false;
                        }
                        var pt = (PointerType)f.type;
                        var ft = (FunctionType)pt.elementType;
                        AppendType(ft.returnType);
                        AP($" {f.name}(");
                        for (int i = 0; i < f.args.Count; ++i) {
                            var arg = (FunctionArgument)f.args[i];
                            Debug.Assert(arg.op == Op.FunctionArgument);
                            AppendType(arg.type);
                            if (arg.noalias) {
                                AP(" noalias");
                            }
                            if (arg.nocapture) {
                                AP(" nocapture");
                            }
                            if (arg.@readonly) {
                                AP(" readonly");
                            }
                            if (!declare) {
                                AP($" {arg.name}");
                            }
                            if (i != f.args.Count - 1) {
                                AP($", ");
                            }
                        }
                        if (!declare) {
                            AL(") #0 {");
                            foreach (var b in f.blocks) {
                                AL($"{b.name.Substring(1)}:");
                            }
                            AL("}");
                        } else {
                            AL(") #0");
                        }
                    }
                    break;
                case Op.ConstInt:
                    break;
                case Op.ConstReal:
                    break;
                case Op.ConstPtr:
                    break;
                case Op.ConstVoid:
                    break;
                case Op.Label:
                    break;
                case Op.Ret:
                    break;
                case Op.Br:
                    break;
                case Op.Call:
                    break;
                case Op.Alloca:
                    break;
                case Op.BitCast:
                    break;
                case Op.Store:
                    break;
                case Op.GEP:
                    break;
                case Op.Load:
                    break;
                case Op.Or:
                    break;
                case Op.Xor:
                    break;
                case Op.ICmp:
                    break;
                case Op.Add:
                    break;
                case Op.Sub:
                    break;
                case Op.Mul:
                    break;
                case Op.SDiv:
                    break;
                case Op.URem:
                    break;
                case Op.Shl:
                    break;
                case Op.AShr:
                    break;
                case Op.Neg:
                    break;
                case Op.FAdd:
                    break;
                case Op.FSub:
                    break;
                case Op.FMul:
                    break;
                case Op.FDiv:
                    break;
                case Op.FRem:
                    break;
                case Op.FCmp:
                    break;
                case Op.PtrToInt:
                    break;
                case Op.Phi:
                    break;
                case Op.FNeg:
                    break;
                case Op.Not:
                    break;
                case Op.SExt:
                    break;
                case Op.ZExt:
                    break;
                case Op.Trunc:
                    break;
                case Op.FPToSI:
                    break;
                case Op.FPToUI:
                    break;
                case Op.SIToFP:
                    break;
                case Op.UIToFP:
                    break;
                case Op.FPCast:
                    break;
                case Op.IntToPtr:
                    break;
                case Op.ConstAggregateZero:
                    break;
                case Op.FunctionArgument:
                    break;
                case Op.ExtractValue:
                    break;
            }
        }

    }
}
