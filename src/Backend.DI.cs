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
        // http://eli.thegreenplace.net/2012/12/17/dumping-a-c-objects-memory-layout-with-clang/
        Dictionary<string, int> debugInfoNodeLookup = new Dictionary<string, int>();
        Dictionary<AST.Node, int> debugInfoScopeLookup = new Dictionary<AST.Node, int>();
        Dictionary<string, int> debugInfoFileLookup = new Dictionary<string, int>();
        Dictionary<FrontendType, int> debugInfoTypeLookup = new Dictionary<FrontendType, int>();
        List<int> debugGlobalVariableExperssionIndices = new List<int>();
        int debugInfoCompileUnitIdx = -1;
        string debugGlobalVariableArrayPlaceholder;
        List<int> debugInfoModuleFlags = new List<int>();
        int debugInfoIdentFlag = -1;
        AST.ProgramRoot debugRootNode;
        void AppendDebugInfo(Value v) {
            if (!CompilerOptions.debugInfo) {
                return;
            }
            if (debugCurrentEmitBlock.name == "%vars") {
                return;
            }
            var locIdx = GetDILocation(v);
            if (locIdx >= 0) {
                AP($", !dbg !{locIdx}");
            }
        }
        void AppendFunctionDebugInfo(Value value) {
            if (!CompilerOptions.debugInfo) {
                return;
            }
            if (value.debugContextNode != null) {
                if (debugRootNode == null) {
                    GetDICompileUnit((AST.ProgramRoot)value.debugContextNode.parent.parent);
                }
                var fd = (AST.FunctionDefinition)value.debugContextNode;
                var subprogramIdx = GetDISubprogram(fd);
                if (subprogramIdx >= 0) {
                    AP($" !dbg !{subprogramIdx}");
                }
            }
        }
        void AppendFunctionArgumentsDebugInfo(Value value) {
            return;
            if (!CompilerOptions.debugInfo) {
                return;
            }
            var f = (Function)value;
            var n = value.debugContextNode;
            if (n != null) {
                var fft = (FrontendFunctionType)typeChecker.GetNodeType(n);
                var scopeIdx = GetDIScope(value.debugContextNode);

                // TODO(pragma): make a copy of function arguments to stack to be able to debug them
                for (int paramIdx = 0; paramIdx < fft.parameters.Count; ++paramIdx) {
                    var arg = (FunctionArgument)f.args[paramIdx];
                    var name = fft.parameters[paramIdx].name;
                    var param_ft = fft.parameters[paramIdx].type;
                    var nodeString = $"!DILocalVariable(name: \"{name}\", arg: {paramIdx+1}, scope: !{GetDIScope(n)}, file: !{GetDIFile(n)}, line: {n.token.Line}, type: !{GetDIType(param_ft)})"; 
                    var localVarIdx = AddDebugInfoNode(nodeString);
                    var locIdx = GetDILocation(value);
                    AP("  call void @llvm.dbg.declare(metadata ");
                    AppendType(arg.type);
                    AP(" ");
                    AP(arg.name);
                    AP($", metadata !{localVarIdx}, metadata !DIExpression()), !dbg !{locIdx}");
                    AL();
                }
            }
        }

        void AppendDebugDeclareLocalVariable(Value value) {
            if (!CompilerOptions.debugInfo) {
                return;
            }
            var ft = typeChecker.GetNodeType(value.debugContextNode);
            // switch (ft) {
            //     case FrontendSliceType str when ft.name == "string":
            //     break;
            //     case var _ when FrontendType.IsIntegerType(ft):
            //     break;
            // }
            var n = value.debugContextNode;
            string name = null;
            switch (n) {
                case AST.VariableDefinition vd:
                    name = vd.variable.name;
                    break;
                case AST.StructConstructor sc: {
                        if (sc.parent is AST.VariableDefinition vdd) {
                            name = vdd.variable.name;
                        }
                    }
                    break;
                case AST.ArrayConstructor ac: {
                        if (ac.parent is AST.VariableDefinition vdd) {
                            name = vdd.variable.name;
                        }
                    }
                    break;
                default:
                    return;
            }
            if (name == null) {
                return;
            }
            var nodeString = $"!DILocalVariable(name: \"{name}\", scope: !{GetDIScope(n.scope.owner)}, file: !{GetDIFile(n)}, line: {n.token.Line}, type: !{GetDIType(ft)})"; 
            var localVarIdx = AddDebugInfoNode(nodeString);
            var locIdx = GetDILocation(value);
            AP("  call void @llvm.dbg.declare(metadata ");
            AppendType(value.type);
            AP(" ");
            AP(value.name);
            AP($", metadata !{localVarIdx}, metadata !DIExpression()), !dbg !{locIdx}");
            AL();
        }

        void AppendGlobalVariableDebugInfo(GlobalVariable gv) {
            if (!CompilerOptions.debugInfo) {
                return;
            }
            var rootScope = gv.debugContextNode.scope.GetRootScope();
            var rootNode = rootScope.owner;
            var n = gv.debugContextNode;
            var ft = typeChecker.GetNodeType(n);

            string name = null;
            if (n is AST.VariableDefinition vd) {
                name = vd.variable.name;
            } else if (n.parent is AST.VariableDefinition parent_vd) {
                name = parent_vd.variable.name;
            }
            if (name != null) {
                var globalVariableNodeString = $"distinct !DIGlobalVariable(name: \"{name}\", scope: !{GetDIScope(rootNode)}, file: !{GetDIFile(n)}, line: {n.token.Line}, type: !{GetDIType(ft)}, isLocal: true, isDefinition: true)";
                var globalVariableIdx = AddDebugInfoNode(globalVariableNodeString);
                var nodeString = $"!DIGlobalVariableExpression(var: !{globalVariableIdx}, expr: !DIExpression())";
                var globalVariableExpressionIdx = AddDebugInfoNode(nodeString);
                debugGlobalVariableExperssionIndices.Add(globalVariableExpressionIdx);
                if (globalVariableExpressionIdx >= 0) {
                    AP($", !dbg !{globalVariableExpressionIdx}");
                }
            }
        }
   
        int AddDebugInfoNode(string info) {
            if (!debugInfoNodeLookup.TryGetValue(info, out int result)) {
                result = debugInfoNodeLookup.Count;
                debugInfoNodeLookup.Add(info, result);
            } 
            return result;
        }
        int GetDIScope(AST.Node scopeRoot) {
            int scopeIdx = -1;
            if (scopeRoot != null && !debugInfoScopeLookup.TryGetValue(scopeRoot, out scopeIdx)) {
                switch (scopeRoot) {
                    case AST.ProgramRoot programRoot:
                        scopeIdx = GetDICompileUnit(programRoot);
                        break;
                    case AST.FunctionDefinition fun:
                        scopeIdx = GetDISubprogram(fun);
                        break;
                    case AST.Block block:
                        if (block.parent is AST.FunctionDefinition fd) {
                            scopeIdx = GetDISubprogram(fd);
                        } else {
                            scopeIdx = GetDILexicalBlock(block);
                        }
                        break;
                    case AST.Module ns:
                        // TODO(pragma): NAMESPACES
                        scopeIdx = GetDIScope(debugRootNode);
                        break;
                    default:
                        scopeIdx = -1;
                        break;
                }
            }
            return scopeIdx;
        }
        int GetDILocation(Value v) {
            var n = v.debugContextNode;
            if (n == null) {
                return -1;
            }
            var scopeRoot = n?.scope?.owner;
            // TODO(pragma): HACK
            if (scopeRoot is AST.ProgramRoot || scopeRoot is AST.Module) {
                if (debugCurrentEmitFunction != null) {
                    scopeRoot = debugCurrentEmitFunction.debugContextNode;
                    if (scopeRoot is AST.ProgramRoot || scopeRoot is AST.Module) {
                        return -1;
                    }
                } else {
                    return -1;
                }
            }
            var scopeIdx = GetDIScope(scopeRoot);
            var locationIdx = -1;
            if (scopeIdx >= 0) {
                string nodeString = $"!DILocation(line: {n.token.Line}, column: {n.token.Pos}, scope: !{scopeIdx})";
                locationIdx = AddDebugInfoNode(nodeString);
            }
            
            return locationIdx;
        }
        int GetDILexicalBlock(AST.Block block) {
            if (!debugInfoScopeLookup.TryGetValue(block, out int lexicalBlockIdx)) {
                var scopeIdx = GetDIScope(block.scope.parent.owner);
                var nodeString = $"distinct !DILexicalBlock(scope: !{scopeIdx}, file: !{GetDIFile(block)}, line: {block.token.Line}, column: {block.token.Pos})";
                lexicalBlockIdx = AddDebugInfoNode(nodeString);
            }
            return lexicalBlockIdx;
        }
        int GetDISubprogram(AST.FunctionDefinition fd) {
            if (fd.body == null) {
                return -1;
            }
            if (!debugInfoScopeLookup.TryGetValue(fd, out int subprogramIdx)) {
                AST.Block block = (AST.Block)fd.body;
                var ft = typeChecker.GetNodeType(fd);
                var variablesIdx = AddDebugInfoNode("!{}");
                string nodeString = $"distinct !DISubprogram(name: \"{fd.funName}\", linkageName: \"{fd.funName}\", file: !{GetDIFile(block)}, line: {fd.token.Line}, type: !{GetDIType(ft, true)}, isLocal: true, isDefinition: true, scopeLine: {block.token.Line}, flags: DIFlagPrototyped, isOptimized: false, unit: !{debugInfoCompileUnitIdx}, variables: !{variablesIdx})";
                subprogramIdx = AddDebugInfoNode(nodeString);
                debugInfoScopeLookup.Add(fd, subprogramIdx);
            }
            return subprogramIdx;
        }

        
        int GetDICompileUnit(AST.ProgramRoot root) {
            if (!debugInfoScopeLookup.TryGetValue(root, out int compileUnitIdx)) {
                string emptyArray = "!{}";
                var emptyArrayIdx = AddDebugInfoNode(emptyArray);

                // HACK: TODO(pragma): remove
                var placeholder = System.Guid.NewGuid().ToString();
                var debugGlobalVariableArrayIdx = AddDebugInfoNode(placeholder);
                debugGlobalVariableArrayPlaceholder = placeholder;

                string producer = "\"pragma version 0.1 (build 8)\"";
                string nodeString = $"distinct !DICompileUnit(language: DW_LANG_C_plus_plus, file: !{GetDIFile(root)}, producer: {producer}, isOptimized: false, runtimeVersion: 0, emissionKind: FullDebug, retainedTypes: !{emptyArrayIdx}, enums: !{emptyArrayIdx}, globals: !{debugGlobalVariableArrayIdx})";
                compileUnitIdx = AddDebugInfoNode(nodeString);
                debugInfoScopeLookup.Add(root, compileUnitIdx);
                debugInfoCompileUnitIdx = compileUnitIdx;

              
                string debugInfoVersion = "!{i32 2, !\"Debug Info Version\", i32 3}";
                var debugInfoVersionIdx = AddDebugInfoNode(debugInfoVersion);
                debugInfoModuleFlags.Add(debugInfoVersionIdx);

                string codeViewVersion = "!{i32 2, !\"CodeView\", i32 1}";
                var codeViewVersionIdx = AddDebugInfoNode(codeViewVersion);
                debugInfoModuleFlags.Add(codeViewVersionIdx);

                string wcharSize = "!{i32 1, !\"wchar_size\", i32 2}";
                var wcharSizeIdx = AddDebugInfoNode(wcharSize);
                debugInfoModuleFlags.Add(wcharSizeIdx);

                string picLevel = "!{i32 7, !\"PIC Level\", i32 2}";
                var picLevelIdx = AddDebugInfoNode(picLevel);
                debugInfoModuleFlags.Add(picLevelIdx);

                // TODO(pragma): versions??
                string ident = $"!{{!{producer}}}";
                debugInfoIdentFlag = AddDebugInfoNode(ident);


                debugRootNode = root;
            }
            return compileUnitIdx;
        }
        void FixUpGlobalVariableDebugInfoList() {
            var palceholderIdx = debugInfoNodeLookup[debugGlobalVariableArrayPlaceholder];
            debugInfoNodeLookup.Remove(debugGlobalVariableArrayPlaceholder);
            var indexStrings = debugGlobalVariableExperssionIndices.Select(idx => "!" + idx.ToString());
            var nodeString = $"!{{{string.Join(", ", indexStrings)}}}";
            debugInfoNodeLookup.Add(nodeString, palceholderIdx);
        }

        int GetDIFile(AST.Node node) {
            if (!debugInfoFileLookup.TryGetValue(node.token.filename, out int fileIdx)) {
                var fn = Backend.EscapeString(System.IO.Path.GetFileName(node.token.filename));
                var dir = Backend.EscapeString(System.IO.Path.GetDirectoryName(node.token.filename));
                string checksum;
                using (var md5 = System.Security.Cryptography.MD5.Create()) {
                    using (var stream = File.OpenRead(node.token.filename)) {
                        checksum = Backend.EscapeString(BitConverter.ToString(md5.ComputeHash(stream)).Replace("-","‌​").ToLower());
                    }
                }
                var nodeString = $"!DIFile(filename: \"{fn}\", directory: \"{dir}\", checksumkind: CSK_MD5, checksum: \"{checksum}\")";
                fileIdx = AddDebugInfoNode(nodeString);
                debugInfoFileLookup.Add(node.token.filename, fileIdx);
            }
            return fileIdx;
        }
        // http://www.catb.org/esr/structure-packing/
        int GetDIType(FrontendType ft, bool noFunctionPointer = false) {
            if (!debugInfoTypeLookup.TryGetValue(ft, out int typeIdx)) {
                string nodeString = null;
                if (FrontendType.IsIntegerType(ft)) {
                    nodeString = $"!DIBasicType(name: \"{ft.name}\", size: {8*GetSizeOfFrontendType(ft)}, encoding: DW_ATE_signed)";
                }
                else if (FrontendType.IsFloatType(ft)) {
                    nodeString = $"!DIBasicType(name: \"{ft.name}\", size: {8*GetSizeOfFrontendType(ft)}, encoding: DW_ATE_float)";
                }
                else if (FrontendType.IsBoolType(ft)) {
                    nodeString = $"!DIBasicType(name: \"bool\", size: {8*GetSizeOfFrontendType(ft)}, encoding: DW_ATE_boolean)";
                }
                else if (ft is FrontendFunctionType fft) {
                    
                    string tl;
                    if (FrontendType.IsVoidType(fft.returnType)) {
                        if (fft.parameters.Count > 0) {
                            tl = "null, " + String.Join(", ", fft.parameters.Select(p => { return "!" + GetDIType(p.type).ToString(); }));
                        } else {
                            tl = "null";
                        }
                    } else {
                        var types = new List<FrontendType>();
                        types.Add(fft.returnType);
                        types.AddRange(fft.parameters.Select(p => p.type));
                        tl = String.Join(", ", types.Select(t => { return "!" + GetDIType(t).ToString(); }));
                    }
                    
                    var typeListNodeString = $"!{{{tl}}}";
                    var typeListIdx = AddDebugInfoNode(typeListNodeString);

                    var functionNodeString = $"!DISubroutineType(types: !{typeListIdx})";
                    if (noFunctionPointer) {
                        nodeString = functionNodeString;
                    } else {
                        var functionTypeIdx = AddDebugInfoNode(functionNodeString);
                        nodeString = $"!DIDerivedType(tag: DW_TAG_pointer_type, baseType: !{functionTypeIdx}, size: {8*GetSizeOfFrontendType(FrontendType.ptr)})";
                    }
                }
                else if (ft is FrontendStructType fst) {
                    // reserve slot for new struct type
                    // if recursive calls want same type use reserved slot idx
                    // to avoid stack overflow
                    // assumes that struct has not already been defined. We _really_ need our type system to have exactly one
                    // frontendtype class per type so we can use it as keys in dict                
                    var placeholder = System.Guid.NewGuid().ToString();
                    var structIdx = AddDebugInfoNode(placeholder);
                    debugInfoTypeLookup.Add(ft, structIdx);

                    var node = (AST.StructDeclaration)typeChecker.GetTypeRoot(fst);
                    var memberListIndices = new List<int>();
                    var offsets = GetOffsetsOfStruct(fst);
                    for (int idx = 0; idx < fst.fields.Count; ++idx) {
                        var f = fst.fields[idx];
                        string memberNodeString;

                        if (node != null) {
                            var ts = node.fields[idx].typeString;
                            memberNodeString = $"!DIDerivedType(tag: DW_TAG_member, name: \"{f.name}\", scope: !{structIdx}, file: !{GetDIFile(ts)}, line: {ts.token.Line}, baseType: !{GetDIType(f.type)}, size: {8*GetSizeOfFrontendType(f.type)}, offset: {8*offsets[idx]})";
                        } else {
                            memberNodeString = $"!DIDerivedType(tag: DW_TAG_member, name: \"{f.name}\", scope: !{structIdx}, baseType: !{GetDIType(f.type)}, size: {8*GetSizeOfFrontendType(f.type)}, offset: {8*offsets[idx]})";
                        }
                        memberListIndices.Add(AddDebugInfoNode(memberNodeString));
                        // token is in node.fields.typeString.token
                    }
                    var memberListNodeString = $"!{{{String.Join(", ", memberListIndices.Select(t_idx => "!" + t_idx))}}}";
                    var memberListIdx = AddDebugInfoNode(memberListNodeString);
                    nodeString = $"distinct !DICompositeType(tag: DW_TAG_structure_type, name: \"{fst}\", size: {8*GetDISizeOfStruct(fst)}, elements: !{memberListIdx})";

                    debugInfoNodeLookup.Remove(placeholder);
                    debugInfoNodeLookup.Add(nodeString, structIdx);
                    return structIdx;
                }
                else if (ft is FrontendPointerType fpt) {
                    nodeString = $"!DIDerivedType(tag: DW_TAG_pointer_type, baseType: !{GetDIType(fpt.elementType)}, size: {8*GetSizeOfFrontendType(fpt)})";
                } else if (ft is FrontendArrayType fat) {
                    var subranges = new List<string>();
                    for (int i = 0; i < fat.dims.Count; ++i) {
                        var subrangeNodeString = $"!DISubrange(count: {fat.dims[i]})";
                        var subrangeIdx = AddDebugInfoNode(subrangeNodeString);
                        subranges.Add("!" + subrangeIdx.ToString());
                    }
                    var elementsIdx = AddDebugInfoNode($"!{{{string.Join(", ", subranges)}}}");
                    nodeString = $"!DICompositeType(tag: DW_TAG_array_type, baseType: !{GetDIType(fat.elementType)}, size: {SizeOfArrayType(fat)}, elements: !{elementsIdx})";
                }

                Debug.Assert(nodeString != null);
                typeIdx = AddDebugInfoNode(nodeString);
                debugInfoTypeLookup.Add(ft, typeIdx);
            }
            return typeIdx;
        }

        // TODO(pragma): maybe promote this to Frontend
        int GetDISizeOfStruct(FrontendStructType st) {
            return GetSizeOfFrontendType(st);
        }

        int GetSizeOfFrontendType(FrontendType t) {
            switch (t) {
                case var _ when t.Equals(FrontendType.i8):
                    return 1;
                case var _ when t.Equals(FrontendType.i16):
                    return 2;
                case var _ when t.Equals(FrontendType.i32):
                    return 4;
                case var _ when t.Equals(FrontendType.i64):
                    return 8;
                case var _ when t.Equals(FrontendFunctionType.f32):
                    return 4;
                case var _ when t.Equals(FrontendFunctionType.f64):
                    return 8;
                case var _ when t.Equals(FrontendType.bool_):
                    return 1; // TODO(pragma): switch to b8 b16 b32?
                case var _ when t.Equals(FrontendStructType.mm):
                    return 8;
                case FrontendPointerType _:
                    return 8;
                case FrontendFunctionType _:
                    return 8;
                case FrontendStructType fst:
                    return GetSizeOfStructType(fst);
                case FrontendArrayType fat:
                    return SizeOfArrayType(fat);

            }
            Debug.Assert(false);
            return -1;
        }
        
        int GetAlignmentOfFrontendType(FrontendType t) {
            switch (t) {
                case var _ when t.Equals(FrontendType.i8):
                    return 1;
                case var _ when t.Equals(FrontendType.i16):
                    return 2;
                case var _ when t.Equals(FrontendType.i32):
                    return 4;
                case var _ when t.Equals(FrontendType.i64):
                    return 8;
                case var _ when t.Equals(FrontendFunctionType.f32):
                    return 4;
                case var _ when t.Equals(FrontendFunctionType.f64):
                    return 8;
                case var _ when t.Equals(FrontendType.bool_):
                    return 1; // TODO(pragma): switch to b8 b16 b32?
                case var _ when t.Equals(FrontendType.mm):
                    return 8;
                case FrontendPointerType _:
                    return 8;
                case FrontendFunctionType _:
                    return 8;
                case FrontendStructType fst:
                    return GetAlignmentOfStruct(fst);
                case FrontendArrayType fat:
                    return SizeOfArrayType(fat);

            }
            Debug.Assert(false);
            return -1;
        }
        int GetAlignmentOfStruct(FrontendStructType st) {
            return st.fields.Select(f => GetAlignmentOfFrontendType(f.type)).Max();
        }
        int GetAlignmentOfArray(FrontendArrayType at) {
            return GetAlignmentOfFrontendType(at.elementType);
        }
        int AlignPow2(int value, int alignment) {
            return (value + (alignment - 1)) & ~(alignment - 1);
        }
        int Align4(int value) {
            return (value + 3) & ~3;
        }
        int Align8(int value) {
            return (value + 7) & ~7;
        }
        int Align16(int value) {
            return (value + 15) & ~15;
        }
        List<int> GetOffsetsOfStruct(FrontendStructType st) {
            // TODO(pragma): is packed
            var result = new List<int>();
            int pos = 0;
            foreach (var f in st.fields) {
                var sizeInBytes = GetSizeOfFrontendType(f.type);
                var alignment = st.packed ? 1 : GetAlignmentOfFrontendType(f.type);
                pos = AlignPow2(pos, alignment);
                result.Add(pos);
                pos += sizeInBytes;
                Debug.Assert(sizeInBytes > 0);
            }
            return result;
        }
        int SizeOfArrayType(FrontendArrayType at) {
            return GetSizeOfFrontendType(at.elementType) * at.dims.Aggregate(1, (a,b) => a * b);
        }
        int GetSizeOfStructType(FrontendStructType st) {
            int pos = 0;
            foreach (var f in st.fields) {
                var sizeInBytes = GetSizeOfFrontendType(f.type);
                var alignment = st.packed ? 1 : GetAlignmentOfFrontendType(f.type);
                pos = AlignPow2(pos, alignment);
                pos += sizeInBytes;
            }
            var alignmentStruct = st.packed ? 1 : GetAlignmentOfStruct(st);
            pos = AlignPow2(pos, alignmentStruct);
            return pos;
        }

    }
}