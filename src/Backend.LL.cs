using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Linq;

using static PragmaScript.SSA;

namespace PragmaScript {
    partial class Backend {

        StringBuilder sb = new StringBuilder();
        static string EscapeString(string s) {
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
        int AddAttrib(FunctionAttribs attrib) {
            if (functionAttribs.TryGetValue(attrib, out var idx)) {
                return idx;
            } else {
                idx = functionAttribs.Count;
                functionAttribs.Add(attrib, idx);
                return idx;
            }
        }

        string preamble = @"
; declare align 64 i8* @VirtualAlloc(i8* nocapture, i64, i32, i32) #0 

define i64 @_rdtsc() #0 {
  %1 = tail call { i32, i32 } asm sideeffect ""rdtsc"", ""={ax},={dx},~{ dirflag},~{ fpsr},~{ flags}""() 
  %2 = extractvalue { i32, i32 } %1, 0
  %3 = extractvalue { i32, i32 } %1, 1
  %4 = zext i32 %3 to i64
  %5 = shl nuw i64 %4, 32
  %6 = zext i32 %2 to i64
  %7 = or i64 %5, %6
  ret i64 %7
}

define void @__chkstk() #0 {
  call void asm sideeffect ""push   %rcx \09\0Apush   %rax \09\0Acmp    $$0x1000,%rax \09\0Alea    24(%rsp),%rcx \09\0Ajb     1f \09\0A2: \09\0Asub    $$0x1000,%rcx \09\0Aorl    $$0,(%rcx) \09\0Asub    $$0x1000,%rax \09\0Acmp    $$0x1000,%rax \09\0Aja     2b \09\0A1: \09\0Asub    %rax,%rcx \09\0Aorl    $$0,(%rcx) \09\0Apop    %rax \09\0Apop    %rcx \09\0Aret \09\0A"", ""~{dirflag},~{fpsr},~{flags}""()
  ret void
}

declare void @llvm.dbg.declare(metadata, metadata, metadata) #0
";
        NumberFormatInfo nfi = new NumberFormatInfo();
        Function debugCurrentEmitFunction;
        Block debugCurrentEmitBlock;
        string emitLL() {
            nfi.NumberDecimalSeparator = ".";
            sb = new StringBuilder();
            AL("target datalayout = \"e-m:w-i64:64-f80:128-n8:16:32:64-S128\"");
            // AL("target triple = \"x86_64-pc-windows-msvc\"");
            AL("target triple = \"x86_64-pc-windows-msvc19.11.25508\"");
            AL();
            foreach (var v in mod.globals.args) {
                Debug.Assert(v is GlobalVariable || v is GlobalStringPtr || v is Function);
                if (v is GlobalVariable || v is GlobalStringPtr) {
                    AppendOp(v);
                }
            }
            sb.Append(preamble);
            foreach (var v in mod.globals.args) {
                if (v is Function f) {
                    if (f.isStub) {
                        continue;
                    }
                    debugCurrentEmitFunction = f;
                    AppendOp(v);
                }
            }
            debugCurrentEmitFunction = null;

            // function attributes
            foreach (var kv in functionAttribs) {
                AP($"attributes #{kv.Value} = {{ ");
                var attribs = kv.Key;
                if (attribs.HasFlag(FunctionAttribs.nounwind)) {
                    AP("nounwind ");
                }
                if (attribs.HasFlag(FunctionAttribs.readnone)) {
                    AP("readnone ");
                }
                if (attribs.HasFlag(FunctionAttribs.argmemonly)) {
                    AP("argmemonly ");
                }
                if (CompilerOptions.optimizationLevel == 0) {
                    AP("noinline optnone ");
                }
                if (CompilerOptions.debug) {
                    AP("uwtable ");
                   //  AP("uwtable \"correctly-rounded-divide-sqrt-fp-math\"=\"false\" \"disable-tail-calls\"=\"false\" \"less-precise-fpmad\"=\"false\" \"no-frame-pointer-elim\"=\"false\" \"no-infs-fp-math\"=\"false\" \"no-jump-tables\"=\"false\" \"no-nans-fp-math\"=\"false\" \"no-signed-zeros-fp-math\"=\"false\" \"no-trapping-math\"=\"false\" \"stack-protector-buffer-size\"=\"8\" \"target-cpu\"=\"x86-64\" \"target-features\"=\"+fxsr,+mmx,+sse,+sse2,+x87\" \"unsafe-fp-math\"=\"false\" \"use-soft-float\"=\"false\" ");
                }
               
                AL("}");
            }

            // debug info
            if (CompilerOptions.debug) {
                AL();
                AL($"!llvm.dbg.cu = !{{!{debugInfoCompileUnitIdx}}}");
                AL($"!llvm.module.flags = !{{{string.Join(", ", debugInfoModuleFlags.Select(idx => $"!{idx}"))}}}");
                AL($"!llvm.ident = !{{!{debugInfoIdentFlag}}}");

                AL();
                var nodeLookupReverse = new Dictionary<int, string>();
                foreach (var kv in debugInfoNodeLookup) {
                    nodeLookupReverse.Add(kv.Value, kv.Key);
                }
                for (int i = 0; i < nodeLookupReverse.Count; ++i) {
                    AL($"!{i} = {nodeLookupReverse[i]}");
                }
            }
            
            return sb.ToString();
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
                    string fs = null;
                    var rt = (FloatType)r.type;
                    if (rt.width == FloatType.FloatWidths.fp64) {
                        var bytes = BitConverter.GetBytes(r.data);
                        var number = BitConverter.ToUInt64(bytes, 0);
                        fs = $"0x{number.ToString("X16", nfi)}";
                    } else if (rt.width == FloatType.FloatWidths.fp32) {
                        var f32 = (float)r.data;
                        var bytes = BitConverter.GetBytes((double)f32);
                        var number = BitConverter.ToUInt64(bytes, 0); ;
                        fs = $"0x{number.ToString("X16", nfi)}";
                    } else if (rt.width == FloatType.FloatWidths.fp16) {
                        throw new NotImplementedException();
                    }
                    AP(fs);
                    break;
                case ConstPtr p:
                    if (p.data == 0) {
                        AP("null");
                    } else {
                        AP(p.data.ToString());
                    }
                    break;
                case ConstArray a: {
                        AP("[ ");
                        for (int i = 0; i < a.data.Count; ++i) {
                            var d = a.data[i];
                            AppendType(d.type);
                            AP(" ");
                            AppendConstValue(d);
                            if (i != a.data.Count - 1) {
                                AP(", ");
                            }
                        }
                        AP(" ]");
                    }
                    break;
                case ConstStruct st: {
                        AP("{ ");
                        for (int i = 0; i < st.elements.Count; ++i) {
                            var el = st.elements[i];
                            AppendType(el.type);
                            AP(" ");
                            AppendConstValue(el);
                            if (i != st.elements.Count - 1) {
                                AP(", ");
                            }
                        }
                        AP(" }");
                    }
                    break;
                case Value caz when caz.op == Op.ConstAggregateZero:
                    AP("zeroinitializer");
                    break;
                default:
                    AppendOp(v, true);
                    break;
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
                case LabelType t:
                    AP("label");
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


        void AppendConversionOp(Value v, string name, bool isConst) {
            if (!isConst) {
                AppendAssignSSA(v);
            }
            AP($"{name} ");
            if (isConst) {
                AP("(");
            }
            AppendArgument(v.args[0]);
            AP(" to ");
            AppendType(v.type);
            if (isConst) {
                AP(")");
            } else {
                AppendDebugInfo(v);
                AL();
            }
        }

        void AppendBinOp(Value v, string name, bool isConst) {
            if (!isConst) {
                AppendAssignSSA(v);
            }
            AP($"{name} ");
            if (isConst) {
                AP("(");
            }
            AppendArgument(v.args[0]);
            AP(", ");
            if (!isConst) {
                AppendArgument(v.args[1], false);
            } else {
                AppendArgument(v.args[1], true);
            }

            if (isConst) {
                AP(")");
            } else {
                AppendDebugInfo(v);
                AL();
            }
        }

        bool isIndented = false;
        void AppendOp(SSA.Value v, bool isConst = false) {
            switch (v.op) {
                case Op.FunctionArgument:
                case Op.ConstAggregateZero:
                case Op.ConstInt:
                case Op.ConstReal:
                case Op.ConstArray:
                case Op.ConstStruct:
                case Op.ConstPtr:
                case Op.ConstVoid:
                case Op.Label:
                    throw new InvalidCodePath();
                case Op.Function: {
                        Debug.Assert(!isConst);
                        var f = (Function)v;
                        bool declare;
                        if (f.blocks == null || f.blocks.Count == 0) {
                            AP("declare ");
                            declare = true;
                        } else {
                            AP("define ");
                            if (f.exportDLL) {
                                AP("dllexport ");
                            }
                            if (!f.exportDLL && f.internalLinkage) {
                                AP("internal ");
                            }
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
                        var attribIdx = AddAttrib(f.attribs);
                        if (!declare) {
                            AP($") #{attribIdx}");
                            AppendFunctionDebugInfo(v);
                            AL(" {");
                            foreach (var b in f.blocks) {
                                debugCurrentEmitBlock = b;
                                AL($"{b.name.Substring(1)}:");
                                isIndented = true;
                                if (b.name == "%vars") {
                                    AppendFunctionArgumentsDebugInfo(v);
                                }
                                foreach (var op in b.args) {
                                    AppendOp(op);
                                }
                                isIndented = false;
                            }
                            AL("}");
                        } else {
                            AL($") #{attribIdx}");
                        }
                        
                        AL();
                    }
                    break;
                case Op.GlobalStringPtr: {
                        if (!isConst) {
                            var gsp = (GlobalStringPtr)v;
                            var es = EscapeString(gsp.data);
                            AL($"{gsp.name} = private unnamed_addr constant [{gsp.data.Length + 1} x i8] c\"{es}\\00\"");
                        } else {
                            AP(v.name);
                        }
                    }
                    break;
                case Op.GlobalVariable: {
                        if (!isConst) {
                            var gv = (GlobalVariable)v;
                            AppendAssignSSA(gv);
                            AP($"internal global {(gv.isConstantVariable ? "constant " : "")}");
                            var pt = (PointerType)gv.type;
                            AppendType(pt.elementType);
                            AP(" ");
                            AppendConstValue(gv.initializer);
                            AL();
                        } else {
                            AP(v.name);
                        }
                    }
                    break;
                case Op.Br: {
                        if (v.args.Count == 1) {
                            Debug.Assert(!isConst);
                            Indent();
                            AP("br ");
                            AppendArgument(v.args[0]);
                            AppendDebugInfo(v);
                            AL();
                        } else {
                            Indent();
                            AP("br ");
                            AppendArgument(v.args[0]);
                            AP(", ");
                            AppendArgument(v.args[1]);
                            AP(", ");
                            AppendArgument(v.args[2]);
                            AppendDebugInfo(v);
                            AL();
                        }

                    }

                    break;
                case Op.Phi: {
                        AppendAssignSSA(v);
                        AP("phi ");
                        AppendType(v.type);
                        AP(" ");
                        var phi = (Phi)v;
                        for (int i = 0; i < phi.incoming.Count; ++i) {
                            AP("[ ");
                            var inc = phi.incoming[i];
                            AppendArgument(inc.v, false);
                            AP(", ");
                            AppendArgument(inc.b, false);
                            AP(" ]");
                            if (i != phi.incoming.Count - 1) {
                                AP(", ");
                            }
                        }
                        AppendDebugInfo(v);
                        AL();
                    }
                    break;
                case Op.Call: {
                        Debug.Assert(!isConst);
                        if (v.type.kind != TypeKind.Void) {
                            AppendAssignSSA(v);
                        } else {
                            Indent();
                        }
                        AP("call ");
                        var fun = v.args[0];
                        var pt = (PointerType)fun.type;
                        var ft = (FunctionType)pt.elementType;
                        AppendType(ft.returnType);
                        AP(" ");
                        AppendArgument(fun, false);
                        AP("(");
                        for (int i = 1; i < v.args.Count; ++i) {
                            AppendArgument(v.args[i]);
                            if (i != v.args.Count - 1) {
                                AP(", ");
                            }
                        }

                        AP(")");
                        AppendDebugInfo(v);
                        AL();
                    }
                    break;
                case Op.Ret:
                    Debug.Assert(!isConst);
                    Indent();
                    AP("ret ");
                    if (v.type.kind == TypeKind.Void) {
                        AP("void");
                        AppendDebugInfo(v);
                        AL();
                    } else {
                        AppendArgument(v.args[0]);
                        AppendDebugInfo(v);
                        AL();
                    }
                    break;
                case Op.Alloca:
                    Debug.Assert(!isConst);
                    AppendAssignSSA(v);
                    AP("alloca ");
                    var et = ((PointerType)v.type).elementType;
                    AppendType(et);
                    if (v.args != null) {
                        AP(", ");
                        AppendArgument(v.args[0]);
                    }
                    AppendDebugInfo(v);
                    AL();
                    AppendDebugDeclareLocalVariable(v);
                    
                    break;
                case Op.Store: {
                        Debug.Assert(!isConst);
                        Indent();
                        AP("store ");
                        var val = v.args[0];
                        var ptr = v.args[1];
                        AppendArgument(val);
                        AP(", ");
                        AppendArgument(ptr);
                        AppendDebugInfo(v);
                        AL();
                    }
                    break;
                case Op.Load:
                    Debug.Assert(!isConst);
                    AppendAssignSSA(v);
                    AP("load ");
                    AppendType(v.type);
                    AP(", ");
                    AppendArgument(v.args[0]);
                    AppendDebugInfo(v);
                    AL();
                    break;
                case Op.GEP: {
                        var gep = (GetElementPtr)v;
                        var arg0 = v.args[0];
                        if (!isConst) {
                            AppendAssignSSA(v);
                        }
                        AP("getelementptr ");
                        if (gep.inBounds) {
                            AP("inbounds ");
                        }
                        if (isConst) {
                            AP("(");
                        }
                        AppendType(gep.baseType);
                        AP(", ");
                        AppendArgument(arg0);
                        for (int i = 1; i < gep.args.Count; ++i) {
                            AP(", ");
                            AppendArgument(v.args[i]);
                        }
                        if (isConst) {
                            AP(")");
                        } else {
                            AppendDebugInfo(v);
                            AL();
                        }
                    }
                    break;
                case Op.ExtractValue: {
                        var arg0 = v.args[0];
                        if (!isConst) {
                            AppendAssignSSA(v);
                        }
                        AP("extractvalue ");
                        if (isConst) {
                            AP("(");
                        }
                        AppendArgument(arg0);
                        for (int i = 1; i < v.args.Count; ++i) {
                            AP(", ");
                            AppendArgument(v.args[i], false);
                        }
                        if (isConst) {
                            AP(")");
                        } else {
                            AppendDebugInfo(v);
                            AL();
                        }
                    }
                    break;
                case Op.And:
                    AppendBinOp(v, "and", isConst);
                    break;
                case Op.Or:
                    AppendBinOp(v, "or", isConst);
                    break;
                case Op.Xor:
                    AppendBinOp(v, "xor", isConst);
                    break;
                case Op.Not: {
                        // https://jonathan2251.github.io/lbd/otherinst.html
                        AppendAssignSSA(v);
                        AP("xor ");
                        AppendArgument(v.args[0]);
                        var it = (IntegerType)v.args[0].type;
                        if (it.bitWidth == 1) {
                            AP(", true");
                            AppendDebugInfo(v);
                            AL();
                        } else {
                            AP(", -1");
                            AppendDebugInfo(v);
                            AL();
                        }
                    }
                    break;
                case Op.Add:
                    AppendBinOp(v, "add", isConst);
                    break;
                case Op.Sub:
                    AppendBinOp(v, "sub", isConst);
                    break;
                case Op.Mul:
                    AppendBinOp(v, "mul", isConst);
                    break;
                case Op.SDiv:
                    AppendBinOp(v, "sdiv", isConst);
                    break;
                case Op.UDiv:
                    AppendBinOp(v, "udiv", isConst);
                    break;
                case Op.URem:
                    AppendBinOp(v, "urem", isConst);
                    break;
                case Op.SRem:
                    AppendBinOp(v, "srem", isConst);
                    break;
                case Op.Shl:
                    AppendBinOp(v, "shl", isConst);
                    break;
                case Op.AShr:
                    AppendBinOp(v, "ashr", isConst);
                    break;
                case Op.LShr:
                    AppendBinOp(v, "lshr", isConst);
                    break;
                case Op.FAdd:
                    AppendBinOp(v, "fadd", isConst);
                    break;
                case Op.FSub:
                    AppendBinOp(v, "fsub", isConst);
                    break;
                case Op.FMul:
                    AppendBinOp(v, "fmul", isConst);
                    break;
                case Op.FDiv:
                    AppendBinOp(v, "fdiv", isConst);
                    break;
                case Op.FRem:
                    AppendBinOp(v, "frem", isConst);
                    break;
                case Op.ICmp: {
                        var icmp = (ICmp)v;
                        AppendBinOp(v, $"icmp {icmp.icmpType}", isConst);
                    }
                    break;
                case Op.FCmp: {
                        var fcmp = (FCmp)v;
                        if (fcmp.fcmpType == FcmpType.@true) {
                            AppendBinOp(v, "fcmp true", isConst);
                        } else if (fcmp.fcmpType == FcmpType.@false) {
                            AppendBinOp(v, "fcmp false", isConst);
                        } else {
                            AppendBinOp(v, $"fcmp {fcmp.fcmpType}", isConst);
                        }
                    }
                    break;
                case Op.BitCast:
                    AppendConversionOp(v, "bitcast", isConst);
                    break;
                case Op.PtrToInt:
                    AppendConversionOp(v, "ptrtoint", isConst);
                    break;
                case Op.IntToPtr:
                    AppendConversionOp(v, "inttoptr", isConst);
                    break;
                case Op.Trunc:
                    AppendConversionOp(v, "trunc", isConst);
                    break;
                case Op.ZExt:
                    AppendConversionOp(v, "zext", isConst);
                    break;
                case Op.SExt:
                    AppendConversionOp(v, "sext", isConst);
                    break;
                case Op.FPToSI:
                    AppendConversionOp(v, "fptosi", isConst);
                    break;
                case Op.FPToUI:
                    AppendConversionOp(v, "fptoui", isConst);
                    break;
                case Op.SIToFP:
                    AppendConversionOp(v, "sitofp", isConst);
                    break;
                case Op.UIToFP:
                    AppendConversionOp(v, "uitofp", isConst);
                    break;
                case Op.FPCast: {
                        var sourceType = (FloatType)v.args[0].type;
                        var destType = (FloatType)v.type;
                        if (sourceType.BitWidth > destType.BitWidth) {
                            AppendConversionOp(v, "fptrunc", isConst);
                        } else if (sourceType.BitWidth < destType.BitWidth) {
                            AppendConversionOp(v, "fpext", isConst);
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
