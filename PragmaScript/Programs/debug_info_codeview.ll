; ModuleID = '/tmp/gcc-explorer-compiler1161018-71-bm3lxz/example.cpp'
source_filename = "/tmp/gcc-explorer-compiler1161018-71-bm3lxz/example.cpp"
target datalayout = "e-m:w-i64:64-f80:128-n8:16:32:64-S128"
target triple = "x86_64--windows-msvc18.0.0"

$"\01??_C@_02IKAHHCAI@?$CFi?$AA@" = comdat any


; Function Attrs: nounwind uwtable
define void @"\01?bar@@YAXPEADH@Z"(i8*, i32) #0 !dbg !11 {
  %3 = alloca i32, align 4
  %4 = alloca i8*, align 8
  store i32 %1, i32* %3, align 4
  call void @llvm.dbg.declare(metadata i32* %3, metadata !17, metadata !18), !dbg !19
  store i8* %0, i8** %4, align 8
  call void @llvm.dbg.declare(metadata i8** %4, metadata !20, metadata !18), !dbg !21
  ret void, !dbg !22
}

; Function Attrs: nounwind readnone
declare void @llvm.dbg.declare(metadata, metadata, metadata) #1

; Function Attrs: nounwind uwtable
define void @"\01?foo@@YAXXZ"() #0 !dbg !23 {
  %1 = alloca i32, align 4
  call void @llvm.dbg.declare(metadata i32* %1, metadata !26, metadata !18), !dbg !28
  store i32 0, i32* %1, align 4, !dbg !28
  br label %2, !dbg !29

; <label>:2:                                      ; preds = %7, %0
  %3 = load i32, i32* %1, align 4, !dbg !30
  %4 = icmp slt i32 %3, 12, !dbg !32
  br i1 %4, label %5, label %10, !dbg !33

; <label>:5:                                      ; preds = %2
  %6 = load i32, i32* %1, align 4, !dbg !34
  call void @"\01?bar@@YAXPEADH@Z"(i8* getelementptr inbounds ([3 x i8], [3 x i8]* @"\01??_C@_02IKAHHCAI@?$CFi?$AA@", i32 0, i32 0), i32 %6), !dbg !36
  br label %7, !dbg !37

; <label>:7:                                      ; preds = %5
  %8 = load i32, i32* %1, align 4, !dbg !38
  %9 = add nsw i32 %8, 1, !dbg !38
  store i32 %9, i32* %1, align 4, !dbg !38
  br label %2, !dbg !39, !llvm.loop !40

; <label>:10:                                     ; preds = %2
  ret void, !dbg !42
}

; Function Attrs: nounwind uwtable
define i64 @"\01?_rdtsc@@YA_KXZ"() #0 !dbg !43 {
  %1 = alloca i32, align 4
  %2 = alloca i32, align 4
  call void @llvm.dbg.declare(metadata i32* %1, metadata !45, metadata !18), !dbg !48
  call void @llvm.dbg.declare(metadata i32* %2, metadata !49, metadata !18), !dbg !50
  %3 = call { i32, i32 } asm sideeffect "rdtsc", "={ax},={dx},~{dirflag},~{fpsr},~{flags}"() #2, !dbg !51, !srcloc !52
  %4 = extractvalue { i32, i32 } %3, 0, !dbg !51
  %5 = extractvalue { i32, i32 } %3, 1, !dbg !51
  store i32 %4, i32* %1, align 4, !dbg !51
  store i32 %5, i32* %2, align 4, !dbg !51
  %6 = load i32, i32* %2, align 4, !dbg !53
  %7 = zext i32 %6 to i64, !dbg !53
  %8 = shl i64 %7, 32, !dbg !54
  %9 = load i32, i32* %1, align 4, !dbg !55
  %10 = zext i32 %9 to i64, !dbg !55
  %11 = or i64 %8, %10, !dbg !56
  ret i64 %11, !dbg !57
}

; Function Attrs: nounwind uwtable
define void @"\01?__chkstk@@YAXXZ"() #0 !dbg !58 {
  call void asm sideeffect "push   %rcx \09\0Apush   %rax \09\0Acmp    $$0x1000,%rax \09\0Alea    24(%rsp),%rcx \09\0Ajb     1f \09\0A2: \09\0Asub    $$0x1000,%rcx \09\0Aorl    $$0,(%rcx) \09\0Asub    $$0x1000,%rax \09\0Acmp    $$0x1000,%rax \09\0Aja     2b \09\0A1: \09\0Asub    %rax,%rcx \09\0Aorl    $$0,(%rcx) \09\0Apop    %rax \09\0Apop    %rcx \09\0Aret \09\0A", "~{dirflag},~{fpsr},~{flags}"() #2, !dbg !59, !srcloc !60
  ret void, !dbg !61
}

attributes #0 = { nounwind uwtable "disable-tail-calls"="false" "less-precise-fpmad"="false" "no-frame-pointer-elim"="false" "no-infs-fp-math"="false" "no-jump-tables"="false" "no-nans-fp-math"="false" "no-signed-zeros-fp-math"="false" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+fxsr,+mmx,+sse,+sse2,+x87" "unsafe-fp-math"="false" "use-soft-float"="false" }
attributes #1 = { nounwind readnone }
attributes #2 = { nounwind }

!llvm.dbg.cu = !{!0}
!llvm.module.flags = !{!7, !8, !9}
!llvm.ident = !{!10}

!0 = distinct !DICompileUnit(language: DW_LANG_C_plus_plus, file: !1, producer: "clang version 3.9.0 (tags/RELEASE_390/final)", isOptimized: false, runtimeVersion: 0, emissionKind: FullDebug, enums: !2, retainedTypes: !3)
!1 = !DIFile(filename: "/tmp/gcc-explorer-compiler1161018-71-bm3lxz/example.cpp", directory: "/gcc-explorer")
!2 = !{}
!3 = !{!4}
!4 = !DIDerivedType(tag: DW_TAG_typedef, name: "uint64_t", file: !5, line: 109, baseType: !6)
!5 = !DIFile(filename: "/opt/gcc-explorer/clang+llvm-3.9.0-x86_64-linux-gnu-ubuntu-16.04/bin/../lib/clang/3.9.0/include/stdint.h", directory: "/gcc-explorer")
!6 = !DIBasicType(name: "long long unsigned int", size: 64, align: 64, encoding: DW_ATE_unsigned)
!7 = !{i32 2, !"CodeView", i32 1}
!8 = !{i32 2, !"Debug Info Version", i32 3}
!9 = !{i32 1, !"PIC Level", i32 2}
!10 = !{!"clang version 3.9.0 (tags/RELEASE_390/final)"}
!11 = distinct !DISubprogram(name: "bar", linkageName: "\01?bar@@YAXPEADH@Z", scope: !1, file: !1, line: 3, type: !12, isLocal: false, isDefinition: true, scopeLine: 3, flags: DIFlagPrototyped, isOptimized: false, unit: !0, variables: !2)
!12 = !DISubroutineType(types: !13)
!13 = !{null, !14, !16}
!14 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !15, size: 64, align: 64)
!15 = !DIBasicType(name: "char", size: 8, align: 8, encoding: DW_ATE_signed_char)
!16 = !DIBasicType(name: "int", size: 32, align: 32, encoding: DW_ATE_signed)
!17 = !DILocalVariable(name: "value", arg: 2, scope: !11, file: !1, line: 3, type: !16)
!18 = !DIExpression()
!19 = !DILocation(line: 3, column: 25, scope: !11)
!20 = !DILocalVariable(name: "fmt", arg: 1, scope: !11, file: !1, line: 3, type: !14)
!21 = !DILocation(line: 3, column: 16, scope: !11)
!22 = !DILocation(line: 4, column: 1, scope: !11)
!23 = distinct !DISubprogram(name: "foo", linkageName: "\01?foo@@YAXXZ", scope: !1, file: !1, line: 6, type: !24, isLocal: false, isDefinition: true, scopeLine: 6, flags: DIFlagPrototyped, isOptimized: false, unit: !0, variables: !2)
!24 = !DISubroutineType(types: !25)
!25 = !{null}
!26 = !DILocalVariable(name: "i", scope: !27, file: !1, line: 7, type: !16)
!27 = distinct !DILexicalBlock(scope: !23, file: !1, line: 7, column: 3)
!28 = !DILocation(line: 7, column: 12, scope: !27)
!29 = !DILocation(line: 7, column: 8, scope: !27)
!30 = !DILocation(line: 7, column: 19, scope: !31)
!31 = distinct !DILexicalBlock(scope: !27, file: !1, line: 7, column: 3)
!32 = !DILocation(line: 7, column: 21, scope: !31)
!33 = !DILocation(line: 7, column: 3, scope: !27)
!34 = !DILocation(line: 9, column: 15, scope: !35)
!35 = distinct !DILexicalBlock(scope: !31, file: !1, line: 8, column: 3)
!36 = !DILocation(line: 9, column: 5, scope: !35)
!37 = !DILocation(line: 10, column: 3, scope: !35)
!38 = !DILocation(line: 7, column: 27, scope: !31)
!39 = !DILocation(line: 7, column: 3, scope: !31)
!40 = distinct !{!40, !41}
!41 = !DILocation(line: 7, column: 3, scope: !23)
!42 = !DILocation(line: 11, column: 1, scope: !23)
!43 = distinct !DISubprogram(name: "_rdtsc", linkageName: "\01?_rdtsc@@YA_KXZ", scope: !1, file: !1, line: 15, type: !44, isLocal: false, isDefinition: true, scopeLine: 16, flags: DIFlagPrototyped, isOptimized: false, unit: !0, variables: !2)
!44 = !DISubroutineType(types: !3)
!45 = !DILocalVariable(name: "eax", scope: !43, file: !1, line: 17, type: !46)
!46 = !DIDerivedType(tag: DW_TAG_typedef, name: "uint32_t", file: !5, line: 183, baseType: !47)
!47 = !DIBasicType(name: "unsigned int", size: 32, align: 32, encoding: DW_ATE_unsigned)
!48 = !DILocation(line: 17, column: 12, scope: !43)
!49 = !DILocalVariable(name: "edx", scope: !43, file: !1, line: 17, type: !46)
!50 = !DILocation(line: 17, column: 17, scope: !43)
!51 = !DILocation(line: 18, column: 3, scope: !43)
!52 = !{i32 191}
!53 = !DILocation(line: 19, column: 21, scope: !43)
!54 = !DILocation(line: 19, column: 25, scope: !43)
!55 = !DILocation(line: 19, column: 34, scope: !43)
!56 = !DILocation(line: 19, column: 32, scope: !43)
!57 = !DILocation(line: 19, column: 3, scope: !43)
!58 = distinct !DISubprogram(name: "__chkstk", linkageName: "\01?__chkstk@@YAXXZ", scope: !1, file: !1, line: 23, type: !24, isLocal: false, isDefinition: true, scopeLine: 24, flags: DIFlagPrototyped, isOptimized: false, unit: !0, variables: !2)
!59 = !DILocation(line: 25, column: 3, scope: !58)
!60 = !{i32 299, i32 334, i32 368, i32 410, i32 453, i32 485, i32 510, i32 566, i32 605, i32 661, i32 717, i32 763, i32 788, i32 827, i32 866, i32 900, i32 934}
!61 = !DILocation(line: 43, column: 1, scope: !58)
