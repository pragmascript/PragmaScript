using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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

        NumberFormatInfo nfi = new NumberFormatInfo();
        public void AOT() {
            nfi.NumberDecimalSeparator = ".";
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

        void AppendConstValue(SSA.Value v) {
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
                    AP(r.data.ToString(nfi));
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

        void Indent() {
            if (isIndented) {
                AP("  ");
            }
        }

        void AppendAssignSSA(Value v) {
            Indent();
            AP($"{v.name} = ");
        }

        void AppendArgument(Value arg, bool appendType = true) {
            if (appendType) {
                AppendType(arg.type);
                AP(" ");
            }
            // TODO(pragma): handle constant properly
            if (arg.isConst) {
                AppendConstValue(arg);
            } else {
                AP(arg.name);
            }
        }

        void AppendConversionOp(Value v, string name) {
            AppendAssignSSA(v);
            AP($"{name} ");
            AppendArgument(v.args[0]);
            AP(" to ");
            AppendType(v.type);
            AL();
        }
        void AppendBinOp(Value v, string name) {
            AppendAssignSSA(v);
            AP($"{name} ");
            AppendArgument(v.args[0]);
            AP(", ");
            AppendArgument(v.args[1], false);
            AL();
        }

        bool isIndented = false;
        void AppendOp(SSA.Value v) {
            switch (v.op) {
                case Op.FunctionArgument:
                case Op.ConstAggregateZero:
                case Op.ConstInt:
                case Op.ConstReal:
                case Op.ConstPtr:
                case Op.ConstVoid:
                case Op.Label:
                    throw new InvalidCodePath();

                case Op.GlobalStringPtr: {
                        var gsp = (GlobalStringPtr)v;
                        var es = EscapeString(gsp.data);
                        AL($"{gsp.name} = private unnamed_addr constant [{gsp.data.Length + 1} x i8] c\"{es}\\00\"");
                    }
                    break;
                case Op.GlobalVariable: {
                        var gv = (GlobalVariable)v;
                        AppendAssignSSA(gv);
                        AP($"internal global {(gv.isConst ? "constant " : "")}");
                        var pt = (PointerType)gv.type;
                        AppendType(pt.elementType);
                        AP(" ");
                        AppendConstValue(gv.initializer);
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
                                isIndented = true;
                                foreach (var op in b.args) {
                                    AppendOp(op);
                                }
                                isIndented = false;
                            }
                            AL("}");
                        } else {
                            AL(") #0");
                        }
                    }
                    break;
                case Op.Br:
                    Indent();
                    AP("br label");
                    AppendArgument(v.args[0]);
                    AL();
                    break;
                case Op.Phi:
                    break;
                case Op.Call: {
                        if (v.type.kind != TypeKind.Void) {
                            AppendAssignSSA(v);
                        } else {
                            Indent();
                        }
                        AP("call ");
                        var fun = v.args[0];
                        AppendArgument(fun);
                        AP("(");
                        for (int i = 1; i < v.args.Count; ++i) {
                            AppendType(v.args[i].type);
                            AP(" ");
                            AppendArgument(v.args[i]);
                            if (i != v.args.Count - 1) {
                                AP(", ");
                            }
                        }
                        AL(")");
                    }
                    break;
                case Op.Ret:
                    Indent();
                    AP("ret ");
                    if (v.type.kind == TypeKind.Void) {
                        AL("void");
                    } else {
                        AppendArgument(v.args[0]);
                        AL();
                    }
                    break;
                case Op.Alloca:
                    AppendAssignSSA(v);
                    AP("alloca ");
                    var et = ((PointerType)v.type).elementType;
                    AppendType(et);
                    AL();
                    break;
           
                case Op.Store: {
                        Indent();
                        AP("store ");
                        var val = v.args[0];
                        var ptr = v.args[1];
                        AppendArgument(val);
                        AP(", ");
                        AppendArgument(ptr);
                        AL();
                    }
                    break;
                case Op.Load:
                    AppendAssignSSA(v);
                    AP("load ");
                    AppendType(v.type);
                    AP(", ");
                    AppendArgument(v.args[0]);
                    AL();
                    break;
                case Op.GEP: {
                        var gep = (GetElementPtr)v;
                        var arg0 = v.args[0];
                        AppendAssignSSA(v);
                        AP("getelementptr ");
                        if (gep.inBounds) {
                            AP("inbounds ");
                        }
                        AppendType(gep.baseType);
                        AP(", ");
                        AppendArgument(arg0);
                        for (int i = 1; i < gep.args.Count; ++i) {
                            AP(", ");
                            AppendArgument(v.args[i]);
                        }
                        AL();
                    }
                    break;
                case Op.ExtractValue: {
                        var arg0 = v.args[0];
                        AppendAssignSSA(v);
                        AP("extractvalue ");
                        AppendArgument(arg0);
                        for (int i = 1; i < v.args.Count; ++i) {
                            AP(", ");
                            AppendArgument(v.args[i], false);
                        }
                        AL();
                    }
                    break;
                case Op.And:
                    AppendBinOp(v, "and");
                    break;
                case Op.Or:
                    AppendBinOp(v, "or");
                    break;
                case Op.Xor:
                    AppendBinOp(v, "xor");
                    break;
                case Op.Not:
                    AppendAssignSSA(v);
                    AP("xor");
                    AppendArgument(v);
                    AP(", -1");
                    break;
                case Op.Add:
                    AppendBinOp(v, "add");
                    break;
                case Op.Sub:
                    AppendBinOp(v, "sub");
                    break;
                case Op.Mul:
                    AppendBinOp(v, "mul");
                    break;
                case Op.SDiv:
                    AppendBinOp(v, "sdiv");
                    break;
                case Op.UDiv:
                    AppendBinOp(v, "udiv");
                    break;
                case Op.URem:
                    AppendBinOp(v, "urem");
                    break;
                case Op.SRem:
                    AppendBinOp(v, "srem");
                    break;
                case Op.Shl:
                    AppendBinOp(v, "shl");
                    break;
                case Op.AShr:
                    AppendBinOp(v, "ashr");
                    break;
                case Op.LShr:
                    AppendBinOp(v, "lshr");
                    break;
                case Op.FAdd:
                    AppendBinOp(v, "fadd");
                    break;
                case Op.FSub:
                    AppendBinOp(v, "fsub");
                    break;
                case Op.FMul:
                    AppendBinOp(v, "fmul");
                    break;
                case Op.FDiv:
                    AppendBinOp(v, "fdiv");
                    break;
                case Op.FRem:
                    AppendBinOp(v, "frem");
                    break;
                case Op.ICmp:
                    AppendBinOp(v, "icmp");
                    break;
                case Op.FCmp:
                    AppendBinOp(v, "fcmp");
                    break;
                case Op.BitCast:
                    AppendConversionOp(v, "bitcast");
                    break;
                case Op.PtrToInt:
                    AppendConversionOp(v, "ptrtoint");
                    break;
                case Op.IntToPtr:
                    AppendConversionOp(v, "inttoptr");
                    break;
                case Op.Trunc:
                    AppendConversionOp(v, "trunc");
                    break;
                case Op.ZExt:
                    AppendConversionOp(v, "zext");
                    break;
                case Op.SExt:
                    AppendConversionOp(v, "sext");
                    break;
                case Op.FPToSI:
                    AppendConversionOp(v, "fptosi");
                    break;
                case Op.FPToUI:
                    AppendConversionOp(v, "fptoui");
                    break;
                case Op.SIToFP:
                    AppendConversionOp(v, "sitofp");
                    break;
                case Op.UIToFP:
                    AppendConversionOp(v, "uitofp");
                    break;
                case Op.FPCast: {
                        var sourceType = (FloatType)v.args[0].type;
                        var destType = (FloatType)v.type;
                        if (sourceType.BitWidth > destType.BitWidth) {
                            AppendConversionOp(v, "fptrunc");
                        } else if (sourceType.BitWidth < destType.BitWidth) {
                            AppendConversionOp(v, "fpext");
                        } else {
                            throw new InvalidCodePath();
                        }
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

    }
}
