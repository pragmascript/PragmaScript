; ModuleID = 'd:\Projects\Dotnet\PragmaScript\PragmaScript\bin\x64\Release\Programs\ll\preamble.ll'
target datalayout = "e-m:w-i64:64-f80:128-n8:16:32:64-S128"
target triple = "x86_64-pc-windows-msvc"

@str = private unnamed_addr constant [11 x i8] c"0123456789\00"
@str_arr = private global [10 x i8] zeroinitializer
@decimal_digits = internal global <{ i32, i8* }> zeroinitializer
@__intrinsics = internal global i1 false
@console_output_handle = internal global i64 0
@console_input_handle = internal global i64 0
@console_error_handle = internal global i64 0
@XInputGetState = internal global i32 (i32, <{ i32, <{ i16, i8, i8, i16, i16, i16, i16 }> }>*)* null
@str.1 = private unnamed_addr constant [1 x i8] zeroinitializer
@str_arr.2 = private global [0 x i8] zeroinitializer
@str.3 = private unnamed_addr constant [93 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cpreamble.prag\22, line 27, pos 8)\00"
@str_arr.4 = private global [0 x i8] zeroinitializer
@str.5 = private unnamed_addr constant [93 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cpreamble.prag\22, line 32, pos 8)\00"
@str.6 = private unnamed_addr constant [11 x i8] c"Assertion \00"
@str.7 = private unnamed_addr constant [2 x i8] c"\22\00"
@str.8 = private unnamed_addr constant [3 x i8] c"\22 \00"
@str.9 = private unnamed_addr constant [12 x i8] c"failed at: \00"
@str.10 = private unnamed_addr constant [2 x i8] c"\0A\00"
@str.11 = private unnamed_addr constant [29 x i8] c"c-string not null terminated\00"
@str_arr.12 = private global [0 x i8] zeroinitializer
@str.13 = private unnamed_addr constant [93 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cpreamble.prag\22, line 74, pos 8)\00"
@str_arr.14 = private global [0 x i8] zeroinitializer
@str.15 = private unnamed_addr constant [94 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cpreamble.prag\22, line 194, pos 9)\00"
@str.16 = private unnamed_addr constant [65 x i8] c"                                                                \00"
@str_arr.17 = private global [0 x i8] zeroinitializer
@str.18 = private unnamed_addr constant [94 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cpreamble.prag\22, line 220, pos 8)\00"
@str_arr.19 = private global [0 x i8] zeroinitializer
@str.20 = private unnamed_addr constant [94 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cpreamble.prag\22, line 244, pos 8)\00"
@str_arr.21 = private global [0 x i8] zeroinitializer
@str.22 = private unnamed_addr constant [94 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cpreamble.prag\22, line 257, pos 9)\00"
@str_arr.23 = private global [0 x i8] zeroinitializer
@str.24 = private unnamed_addr constant [94 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cpreamble.prag\22, line 283, pos 8)\00"
@str_arr.25 = private global [0 x i8] zeroinitializer
@str.26 = private unnamed_addr constant [94 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cpreamble.prag\22, line 300, pos 9)\00"
@str_arr.27 = private global [0 x i8] zeroinitializer
@str.28 = private unnamed_addr constant [94 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cpreamble.prag\22, line 306, pos 8)\00"
@str.29 = private unnamed_addr constant [3 x i8] c": \00"
@str.30 = private unnamed_addr constant [5 x i8] c"true\00"
@str.31 = private unnamed_addr constant [6 x i8] c"false\00"
@backbuffer = internal global <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }> zeroinitializer
@window = internal global <{ i64, i64, i32, i32 }> zeroinitializer
@window_requests_quit = internal global i1 false
@perf_count_freq = internal global i64 0
@str.32 = private unnamed_addr constant [100 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cwin32_handmade.prag\22, line 83, pos 32)\00"
@str_arr.33 = private global [0 x i8] zeroinitializer
@str.34 = private unnamed_addr constant [100 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cwin32_handmade.prag\22, line 92, pos 10)\00"
@str.35 = private unnamed_addr constant [101 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cwin32_handmade.prag\22, line 108, pos 32)\00"
@str_arr.36 = private global [0 x i8] zeroinitializer
@str.37 = private unnamed_addr constant [100 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cwin32_handmade.prag\22, line 125, pos 9)\00"
@str_arr.38 = private global [0 x i8] zeroinitializer
@str.39 = private unnamed_addr constant [100 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cwin32_handmade.prag\22, line 129, pos 9)\00"
@str.40 = private unnamed_addr constant [13 x i8] c"handmade.dll\00"
@str.41 = private unnamed_addr constant [18 x i8] c"handmade_temp.dll\00"
@str.42 = private unnamed_addr constant [101 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cwin32_handmade.prag\22, line 175, pos 35)\00"
@str.43 = private unnamed_addr constant [101 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cwin32_handmade.prag\22, line 175, pos 53)\00"
@str_arr.44 = private global [0 x i8] zeroinitializer
@str.45 = private unnamed_addr constant [100 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cwin32_handmade.prag\22, line 176, pos 9)\00"
@str.46 = private unnamed_addr constant [101 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cwin32_handmade.prag\22, line 177, pos 37)\00"
@str.47 = private unnamed_addr constant [23 x i8] c"game_update_and_render\00"
@str.48 = private unnamed_addr constant [101 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cwin32_handmade.prag\22, line 178, pos 89)\00"
@str_arr.49 = private global [0 x i8] zeroinitializer
@str.50 = private unnamed_addr constant [100 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cwin32_handmade.prag\22, line 179, pos 9)\00"
@str.51 = private unnamed_addr constant [18 x i8] c"game_output_sound\00"
@str.52 = private unnamed_addr constant [101 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cwin32_handmade.prag\22, line 180, pos 79)\00"
@str_arr.53 = private global [0 x i8] zeroinitializer
@str.54 = private unnamed_addr constant [100 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cwin32_handmade.prag\22, line 181, pos 9)\00"
@str.55 = private unnamed_addr constant [14 x i8] c"load library\0A\00"
@str_arr.56 = private global [0 x i8] zeroinitializer
@str.57 = private unnamed_addr constant [101 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cwin32_handmade.prag\22, line 193, pos 10)\00"
@str.58 = private unnamed_addr constant [16 x i8] c"unload library\0A\00"
@str.59 = private unnamed_addr constant [101 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cwin32_handmade.prag\22, line 207, pos 16)\00"
@str.60 = private unnamed_addr constant [101 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cwin32_handmade.prag\22, line 209, pos 28)\00"
@str.61 = private unnamed_addr constant [11 x i8] c"dsound.dll\00"
@str.62 = private unnamed_addr constant [101 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cwin32_handmade.prag\22, line 220, pos 41)\00"
@str_arr.63 = private global [0 x i8] zeroinitializer
@str.64 = private unnamed_addr constant [100 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cwin32_handmade.prag\22, line 221, pos 9)\00"
@str.65 = private unnamed_addr constant [18 x i8] c"DirectSoundCreate\00"
@str.66 = private unnamed_addr constant [101 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cwin32_handmade.prag\22, line 229, pos 75)\00"
@str_arr.67 = private global [0 x i8] zeroinitializer
@str.68 = private unnamed_addr constant [100 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cwin32_handmade.prag\22, line 230, pos 9)\00"
@str_arr.69 = private global [0 x i8] zeroinitializer
@str.70 = private unnamed_addr constant [100 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cwin32_handmade.prag\22, line 234, pos 9)\00"
@str_arr.71 = private global [0 x i8] zeroinitializer
@str.72 = private unnamed_addr constant [100 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cwin32_handmade.prag\22, line 236, pos 9)\00"
@str_arr.73 = private global [0 x i8] zeroinitializer
@str.74 = private unnamed_addr constant [100 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cwin32_handmade.prag\22, line 247, pos 9)\00"
@str_arr.75 = private global [0 x i8] zeroinitializer
@str.76 = private unnamed_addr constant [100 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cwin32_handmade.prag\22, line 258, pos 9)\00"
@str_arr.77 = private global [0 x i8] zeroinitializer
@str.78 = private unnamed_addr constant [100 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cwin32_handmade.prag\22, line 269, pos 9)\00"
@str_arr.79 = private global [0 x i8] zeroinitializer
@str.80 = private unnamed_addr constant [100 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cwin32_handmade.prag\22, line 288, pos 9)\00"
@str_arr.81 = private global [0 x i8] zeroinitializer
@str.82 = private unnamed_addr constant [100 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cwin32_handmade.prag\22, line 295, pos 9)\00"
@str_arr.83 = private global [0 x i8] zeroinitializer
@str.84 = private unnamed_addr constant [100 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cwin32_handmade.prag\22, line 300, pos 9)\00"
@str_arr.85 = private global [0 x i8] zeroinitializer
@str.86 = private unnamed_addr constant [101 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cwin32_handmade.prag\22, line 326, pos 10)\00"
@str_arr.87 = private global [0 x i8] zeroinitializer
@str.88 = private unnamed_addr constant [101 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cwin32_handmade.prag\22, line 341, pos 10)\00"
@str_arr.89 = private global [0 x i8] zeroinitializer
@str.90 = private unnamed_addr constant [101 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cwin32_handmade.prag\22, line 352, pos 10)\00"
@str_arr.91 = private global [0 x i8] zeroinitializer
@str.92 = private unnamed_addr constant [101 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cwin32_handmade.prag\22, line 353, pos 10)\00"
@str_arr.93 = private global [0 x i8] zeroinitializer
@str.94 = private unnamed_addr constant [100 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cwin32_handmade.prag\22, line 365, pos 9)\00"
@str_arr.95 = private global [0 x i8] zeroinitializer
@str.96 = private unnamed_addr constant [100 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cwin32_handmade.prag\22, line 414, pos 9)\00"
@str.97 = private unnamed_addr constant [2 x i8] c"L\00"
@str.98 = private unnamed_addr constant [2 x i8] c"P\00"
@str.99 = private unnamed_addr constant [20 x i8] c"input_recording.dat\00"
@str.100 = private unnamed_addr constant [101 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cwin32_handmade.prag\22, line 641, pos 21)\00"
@str_arr.101 = private global [0 x i8] zeroinitializer
@str.102 = private unnamed_addr constant [101 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cwin32_handmade.prag\22, line 645, pos 10)\00"
@str_arr.103 = private global [0 x i8] zeroinitializer
@str.104 = private unnamed_addr constant [101 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cwin32_handmade.prag\22, line 654, pos 10)\00"
@str.105 = private unnamed_addr constant [101 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cwin32_handmade.prag\22, line 676, pos 21)\00"
@str_arr.106 = private global [0 x i8] zeroinitializer
@str.107 = private unnamed_addr constant [101 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cwin32_handmade.prag\22, line 679, pos 10)\00"
@str_arr.108 = private global [0 x i8] zeroinitializer
@str.109 = private unnamed_addr constant [101 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cwin32_handmade.prag\22, line 693, pos 10)\00"
@str_arr.110 = private global [0 x i8] zeroinitializer
@str.111 = private unnamed_addr constant [100 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cwin32_handmade.prag\22, line 721, pos 9)\00"
@str.112 = private unnamed_addr constant [24 x i8] c"PragmaScriptWindowClass\00"
@str.113 = private unnamed_addr constant [101 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cwin32_handmade.prag\22, line 728, pos 25)\00"
@str.114 = private unnamed_addr constant [22 x i8] c"handmade-pragmascript\00"
@str.115 = private unnamed_addr constant [101 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cwin32_handmade.prag\22, line 729, pos 25)\00"
@str_arr.116 = private global [0 x i8] zeroinitializer
@str.117 = private unnamed_addr constant [100 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cwin32_handmade.prag\22, line 779, pos 9)\00"
@str_arr.118 = private global [0 x i8] zeroinitializer
@str.119 = private unnamed_addr constant [100 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cwin32_handmade.prag\22, line 797, pos 9)\00"
@str_arr.120 = private global [0 x i8] zeroinitializer
@str.121 = private unnamed_addr constant [100 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cwin32_handmade.prag\22, line 799, pos 9)\00"
@str_arr.122 = private global [0 x i8] zeroinitializer
@str.123 = private unnamed_addr constant [100 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cwin32_handmade.prag\22, line 808, pos 9)\00"
@str_arr.124 = private global [0 x i8] zeroinitializer
@str.125 = private unnamed_addr constant [101 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cwin32_handmade.prag\22, line 854, pos 10)\00"
@str_arr.126 = private global [0 x i8] zeroinitializer
@str.127 = private unnamed_addr constant [101 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cwin32_handmade.prag\22, line 860, pos 10)\00"
@str.128 = private unnamed_addr constant [8 x i8] c"ms/f   \00"
@str.129 = private unnamed_addr constant [7 x i8] c"fps   \00"
@str.130 = private unnamed_addr constant [11 x i8] c"mcycles   \00"
@str.131 = private unnamed_addr constant [3 x i8] c"dt\00"

; Function Attrs: nounwind
declare align 64 i8* @VirtualAlloc(i8* nocapture, i64, i32, i32) #0

; Function Attrs: nounwind
define i64 @_rdtsc() #0 {
  %1 = tail call { i32, i32 } asm sideeffect "rdtsc", "={ax},={dx},~{dirflag},~{fpsr},~{flags}"()
  %2 = extractvalue { i32, i32 } %1, 0
  %3 = extractvalue { i32, i32 } %1, 1
  %4 = zext i32 %3 to i64
  %5 = shl nuw i64 %4, 32
  %6 = zext i32 %2 to i64
  %7 = or i64 %5, %6
  ret i64 %7
}

; Function Attrs: nounwind
define void @__chkstk() #0 {
  call void asm sideeffect "push   %rcx \09\0Apush   %rax \09\0Acmp    $$0x1000,%rax \09\0Alea    24(%rsp),%rcx \09\0Ajb     1f \09\0A2: \09\0Asub    $$0x1000,%rcx \09\0Aorl    $$0,(%rcx) \09\0Asub    $$0x1000,%rax \09\0Acmp    $$0x1000,%rax \09\0Aja     2b \09\0A1: \09\0Asub    %rax,%rcx \09\0Aorl    $$0,(%rcx) \09\0Apop    %rax \09\0Apop    %rcx \09\0Aret \09\0A", "~{dirflag},~{fpsr},~{flags}"()
  ret void
}

; Function Attrs: nounwind readnone
declare float @llvm.cos.f32(float) #1

; Function Attrs: nounwind readnone
declare float @llvm.sin.f32(float) #1

; Function Attrs: nounwind readnone
declare float @llvm.fabs.f32(float) #1

; Function Attrs: nounwind readnone
declare float @llvm.sqrt.f32(float) #1

; Function Attrs: nounwind readnone
declare float @llvm.floor.f32(float) #1

; Function Attrs: nounwind readnone
declare float @llvm.trunc.f32(float) #1

; Function Attrs: nounwind readnone
declare float @llvm.ceil.f32(float) #1

; Function Attrs: nounwind readnone
declare float @llvm.round.f32(float) #1

; Function Attrs: argmemonly nounwind
declare void @llvm.memcpy.p0i8.p0i8.i32(i8* nocapture, i8* nocapture readonly, i32, i32, i1) #2

declare void @__dummy__()

; Function Attrs: nounwind
declare i32 @WriteFile(i64, i8* nocapture, i32, i32* nocapture, i8*) #0

; Function Attrs: nounwind
declare i32 @ReadFile(i64, i8* nocapture, i32, i32* nocapture, i8*) #0

define void @__init() {
vars:
  %arr_struct_alloca = alloca <{ i32, i8* }>
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* getelementptr inbounds ([10 x i8], [10 x i8]* @str_arr, i32 0, i32 0), i8* getelementptr inbounds ([11 x i8], [11 x i8]* @str, i32 0, i32 0), i32 10, i32 0, i1 false)
  %gep_arr_elem_ptr = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 0
  store i32 10, i32* %gep_arr_elem_ptr
  %gep_arr_elem_ptr1 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 1
  store i8* getelementptr inbounds ([10 x i8], [10 x i8]* @str_arr, i32 0, i32 0), i8** %gep_arr_elem_ptr1
  %arr_struct_load = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca
  br label %entry

entry:                                            ; preds = %vars
  store <{ i32, i8* }> %arr_struct_load, <{ i32, i8* }>* @decimal_digits
  %fun_call = call i1 @__hack_reserve_intrinsics()
  store i1 %fun_call, i1* @__intrinsics
  %fun_call2 = call i64 @GetStdHandle(i32 -11)
  store i64 %fun_call2, i64* @console_output_handle
  %fun_call3 = call i64 @GetStdHandle(i32 -10)
  store i64 %fun_call3, i64* @console_input_handle
  %fun_call4 = call i64 @GetStdHandle(i32 -12)
  store i64 %fun_call4, i64* @console_error_handle
  call void @main()
  ret void
}

; Function Attrs: nounwind
define internal i64 @kilobytes(i64 %bytes) #0 {
vars:
  br label %entry

entry:                                            ; preds = %vars
  %mul_tmp = mul i64 %bytes, 1024
  ret i64 %mul_tmp
}

; Function Attrs: nounwind
define internal i64 @megabytes(i64 %bytes) #0 {
vars:
  br label %entry

entry:                                            ; preds = %vars
  %mul_tmp = mul i64 %bytes, 1024
  %mul_tmp1 = mul i64 %mul_tmp, 1024
  ret i64 %mul_tmp1
}

; Function Attrs: nounwind
define internal i64 @gigabytes(i64 %bytes) #0 {
vars:
  br label %entry

entry:                                            ; preds = %vars
  %mul_tmp = mul i64 %bytes, 1024
  %mul_tmp1 = mul i64 %mul_tmp, 1024
  %mul_tmp2 = mul i64 %mul_tmp1, 1024
  ret i64 %mul_tmp2
}

; Function Attrs: nounwind
define internal i64 @terabytes(i64 %bytes) #0 {
vars:
  br label %entry

entry:                                            ; preds = %vars
  %mul_tmp = mul i64 %bytes, 1024
  %mul_tmp1 = mul i64 %mul_tmp, 1024
  %mul_tmp2 = mul i64 %mul_tmp1, 1024
  %mul_tmp3 = mul i64 %mul_tmp2, 1024
  ret i64 %mul_tmp3
}

; Function Attrs: nounwind
define internal void @print_string(<{ i32, i8* }> %s) #0 {
vars:
  %arr_struct_alloca = alloca <{ i32, i8* }>
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.2, i32 0, i32 0), i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr
  %gep_arr_elem_ptr1 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 1
  store i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.2, i32 0, i32 0), i8** %gep_arr_elem_ptr1
  %arr_struct_load = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca
  %arr_struct_alloca2 = alloca <{ i32, i8* }>
  %arr_elem_alloca = alloca i8, i32 92
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca, i8* getelementptr inbounds ([93 x i8], [93 x i8]* @str.3, i32 0, i32 0), i32 92, i32 0, i1 false)
  %gep_arr_elem_ptr3 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2, i32 0, i32 0
  store i32 92, i32* %gep_arr_elem_ptr3
  %gep_arr_elem_ptr4 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2, i32 0, i32 1
  store i8* %arr_elem_alloca, i8** %gep_arr_elem_ptr4
  %arr_struct_load5 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2
  br label %entry

entry:                                            ; preds = %vars
  %console_output_handle = load i64, i64* @console_output_handle
  %icmp_tmp = icmp ne i64 %console_output_handle, -1
  call void @assert(i1 %icmp_tmp, <{ i32, i8* }> %arr_struct_load, <{ i32, i8* }> %arr_struct_load5)
  %console_output_handle6 = load i64, i64* @console_output_handle
  %struct_field_extract = extractvalue <{ i32, i8* }> %s, 1
  %struct_field_extract7 = extractvalue <{ i32, i8* }> %s, 0
  %fun_call = call i32 @WriteFile(i64 %console_output_handle6, i8* %struct_field_extract, i32 %struct_field_extract7, i32* null, i8* null)
  ret void
}

; Function Attrs: nounwind
define internal void @print_error(<{ i32, i8* }> %s) #0 {
vars:
  %arr_struct_alloca = alloca <{ i32, i8* }>
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.4, i32 0, i32 0), i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr
  %gep_arr_elem_ptr1 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 1
  store i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.4, i32 0, i32 0), i8** %gep_arr_elem_ptr1
  %arr_struct_load = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca
  %arr_struct_alloca2 = alloca <{ i32, i8* }>
  %arr_elem_alloca = alloca i8, i32 92
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca, i8* getelementptr inbounds ([93 x i8], [93 x i8]* @str.5, i32 0, i32 0), i32 92, i32 0, i1 false)
  %gep_arr_elem_ptr3 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2, i32 0, i32 0
  store i32 92, i32* %gep_arr_elem_ptr3
  %gep_arr_elem_ptr4 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2, i32 0, i32 1
  store i8* %arr_elem_alloca, i8** %gep_arr_elem_ptr4
  %arr_struct_load5 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2
  br label %entry

entry:                                            ; preds = %vars
  %console_error_handle = load i64, i64* @console_error_handle
  %icmp_tmp = icmp ne i64 %console_error_handle, -1
  call void @assert(i1 %icmp_tmp, <{ i32, i8* }> %arr_struct_load, <{ i32, i8* }> %arr_struct_load5)
  %console_error_handle6 = load i64, i64* @console_error_handle
  %struct_field_extract = extractvalue <{ i32, i8* }> %s, 1
  %struct_field_extract7 = extractvalue <{ i32, i8* }> %s, 0
  %fun_call = call i32 @WriteFile(i64 %console_error_handle6, i8* %struct_field_extract, i32 %struct_field_extract7, i32* null, i8* null)
  ret void
}

; Function Attrs: nounwind
define internal void @assert(i1 %value, <{ i32, i8* }> %msg, <{ i32, i8* }> %filepos) #0 {
vars:
  %arr_struct_alloca = alloca <{ i32, i8* }>
  %arr_elem_alloca = alloca i8, i32 10
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca, i8* getelementptr inbounds ([11 x i8], [11 x i8]* @str.6, i32 0, i32 0), i32 10, i32 0, i1 false)
  %gep_arr_elem_ptr = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 0
  store i32 10, i32* %gep_arr_elem_ptr
  %gep_arr_elem_ptr1 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 1
  store i8* %arr_elem_alloca, i8** %gep_arr_elem_ptr1
  %arr_struct_load = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca
  %arr_struct_alloca4 = alloca <{ i32, i8* }>
  %arr_elem_alloca5 = alloca i8
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca5, i8* getelementptr inbounds ([2 x i8], [2 x i8]* @str.7, i32 0, i32 0), i32 1, i32 0, i1 false)
  %gep_arr_elem_ptr6 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca4, i32 0, i32 0
  store i32 1, i32* %gep_arr_elem_ptr6
  %gep_arr_elem_ptr7 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca4, i32 0, i32 1
  store i8* %arr_elem_alloca5, i8** %gep_arr_elem_ptr7
  %arr_struct_load8 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca4
  %arr_struct_alloca9 = alloca <{ i32, i8* }>
  %arr_elem_alloca10 = alloca i8, i32 2
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca10, i8* getelementptr inbounds ([3 x i8], [3 x i8]* @str.8, i32 0, i32 0), i32 2, i32 0, i1 false)
  %gep_arr_elem_ptr11 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca9, i32 0, i32 0
  store i32 2, i32* %gep_arr_elem_ptr11
  %gep_arr_elem_ptr12 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca9, i32 0, i32 1
  store i8* %arr_elem_alloca10, i8** %gep_arr_elem_ptr12
  %arr_struct_load13 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca9
  %arr_struct_alloca14 = alloca <{ i32, i8* }>
  %arr_elem_alloca15 = alloca i8, i32 11
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca15, i8* getelementptr inbounds ([12 x i8], [12 x i8]* @str.9, i32 0, i32 0), i32 11, i32 0, i1 false)
  %gep_arr_elem_ptr16 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca14, i32 0, i32 0
  store i32 11, i32* %gep_arr_elem_ptr16
  %gep_arr_elem_ptr17 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca14, i32 0, i32 1
  store i8* %arr_elem_alloca15, i8** %gep_arr_elem_ptr17
  %arr_struct_load18 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca14
  %arr_struct_alloca19 = alloca <{ i32, i8* }>
  %arr_elem_alloca20 = alloca i8
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca20, i8* getelementptr inbounds ([2 x i8], [2 x i8]* @str.10, i32 0, i32 0), i32 1, i32 0, i1 false)
  %gep_arr_elem_ptr21 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca19, i32 0, i32 0
  store i32 1, i32* %gep_arr_elem_ptr21
  %gep_arr_elem_ptr22 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca19, i32 0, i32 1
  store i8* %arr_elem_alloca20, i8** %gep_arr_elem_ptr22
  %arr_struct_load23 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca19
  br label %entry

entry:                                            ; preds = %vars
  %not_tmp = xor i1 %value, true
  br i1 %not_tmp, label %then, label %endif

then:                                             ; preds = %entry
  call void @print_error(<{ i32, i8* }> %arr_struct_load)
  %struct_field_extract = extractvalue <{ i32, i8* }> %msg, 0
  %icmp_tmp = icmp sgt i32 %struct_field_extract, 0
  br i1 %icmp_tmp, label %then2, label %endif3

then2:                                            ; preds = %then
  call void @print_error(<{ i32, i8* }> %arr_struct_load8)
  call void @print_error(<{ i32, i8* }> %msg)
  call void @print_error(<{ i32, i8* }> %arr_struct_load13)
  br label %endif3

endif3:                                           ; preds = %then2, %then
  call void @print_error(<{ i32, i8* }> %arr_struct_load18)
  call void @print_error(<{ i32, i8* }> %filepos)
  call void @print_error(<{ i32, i8* }> %arr_struct_load23)
  store i8 42, i8* null
  br label %endif

endif:                                            ; preds = %endif3, %entry
  ret void
}

; Function Attrs: nounwind
define internal i8* @cstr(<{ i32, i8* }> %value, <{ i32, i8* }> %filepos) #0 {
vars:
  %arr_struct_alloca = alloca <{ i32, i8* }>
  %arr_elem_alloca = alloca i8, i32 28
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca, i8* getelementptr inbounds ([29 x i8], [29 x i8]* @str.11, i32 0, i32 0), i32 28, i32 0, i1 false)
  %gep_arr_elem_ptr1 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 0
  store i32 28, i32* %gep_arr_elem_ptr1
  %gep_arr_elem_ptr2 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 1
  store i8* %arr_elem_alloca, i8** %gep_arr_elem_ptr2
  %arr_struct_load = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca
  br label %entry

entry:                                            ; preds = %vars
  %struct_field_extract = extractvalue <{ i32, i8* }> %value, 0
  %sub_tmp = sub i32 %struct_field_extract, 1
  %gep_arr_elem_ptr = extractvalue <{ i32, i8* }> %value, 1
  %gep_arr_elem = getelementptr i8, i8* %gep_arr_elem_ptr, i32 %sub_tmp
  %arr_elem = load i8, i8* %gep_arr_elem
  %icmp_tmp = icmp eq i8 %arr_elem, 0
  call void @assert(i1 %icmp_tmp, <{ i32, i8* }> %arr_struct_load, <{ i32, i8* }> %filepos)
  %struct_field_extract3 = extractvalue <{ i32, i8* }> %value, 1
  ret i8* %struct_field_extract3
}

; Function Attrs: nounwind
define internal i32 @ord(<{ i32, i8* }> %value) #0 {
vars:
  %arr_struct_alloca = alloca <{ i32, i8* }>
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.12, i32 0, i32 0), i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr
  %gep_arr_elem_ptr1 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 1
  store i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.12, i32 0, i32 0), i8** %gep_arr_elem_ptr1
  %arr_struct_load = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca
  %arr_struct_alloca2 = alloca <{ i32, i8* }>
  %arr_elem_alloca = alloca i8, i32 92
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca, i8* getelementptr inbounds ([93 x i8], [93 x i8]* @str.13, i32 0, i32 0), i32 92, i32 0, i1 false)
  %gep_arr_elem_ptr3 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2, i32 0, i32 0
  store i32 92, i32* %gep_arr_elem_ptr3
  %gep_arr_elem_ptr4 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2, i32 0, i32 1
  store i8* %arr_elem_alloca, i8** %gep_arr_elem_ptr4
  %arr_struct_load5 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2
  br label %entry

entry:                                            ; preds = %vars
  %struct_field_extract = extractvalue <{ i32, i8* }> %value, 0
  %icmp_tmp = icmp eq i32 %struct_field_extract, 1
  call void @assert(i1 %icmp_tmp, <{ i32, i8* }> %arr_struct_load, <{ i32, i8* }> %arr_struct_load5)
  %gep_arr_elem_ptr6 = extractvalue <{ i32, i8* }> %value, 1
  %gep_arr_elem = getelementptr i8, i8* %gep_arr_elem_ptr6, i32 0
  %arr_elem = load i8, i8* %gep_arr_elem
  %int_cast = sext i8 %arr_elem to i32
  ret i32 %int_cast
}

; Function Attrs: nounwind
define internal i8* @memset(i8* %dest, i32 %value, i64 %count) #0 {
vars:
  %data = alloca i8*
  br label %entry

entry:                                            ; preds = %vars
  store i8* %dest, i8** %data
  br label %while_cond

while_cond:                                       ; preds = %while, %entry
  %data1 = load i8*, i8** %data
  %ptr_add = getelementptr i8, i8* %dest, i64 %count
  %icmp_tmp = icmp ne i8* %data1, %ptr_add
  br i1 %icmp_tmp, label %while, label %while_end

while:                                            ; preds = %while_cond
  %postinc_load = load i8*, i8** %data
  %ptr_post_inc = getelementptr i8, i8* %postinc_load, i32 1
  store i8* %ptr_post_inc, i8** %data
  %int_trunc = trunc i32 %value to i8
  store i8 %int_trunc, i8* %postinc_load
  br label %while_cond

while_end:                                        ; preds = %while_cond
  ret i8* %dest
}

; Function Attrs: nounwind
define internal void @print_i32(i32 %value, i1 %signed) #0 {
vars:
  br label %entry

entry:                                            ; preds = %vars
  br i1 %signed, label %then, label %else

then:                                             ; preds = %entry
  %int_cast = sext i32 %value to i64
  call void @print_i64(i64 %int_cast, i1 true)
  br label %endif

else:                                             ; preds = %entry
  %int_cast1 = zext i32 %value to i64
  call void @print_i64(i64 %int_cast1, i1 false)
  br label %endif

endif:                                            ; preds = %else, %then
  ret void
}

; Function Attrs: nounwind
define internal void @print_i64(i64 %value, i1 %signed) #0 {
vars:
  %v = alloca i64
  %pd = alloca <{ <{ i32, i8* }>, i32 }>
  %arr_struct_alloca = alloca <{ i32, i8* }>
  %arr_elem_alloca = alloca i8, i32 64
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca, i8* getelementptr inbounds ([65 x i8], [65 x i8]* @str.16, i32 0, i32 0), i32 64, i32 0, i1 false)
  %gep_arr_elem_ptr = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 0
  store i32 64, i32* %gep_arr_elem_ptr
  %gep_arr_elem_ptr2 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 1
  store i8* %arr_elem_alloca, i8** %gep_arr_elem_ptr2
  %arr_struct_load = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca
  %arr_struct_alloca5 = alloca <{ i32, i8* }>
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.17, i32 0, i32 0), i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr6 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca5, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr6
  %gep_arr_elem_ptr7 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca5, i32 0, i32 1
  store i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.17, i32 0, i32 0), i8** %gep_arr_elem_ptr7
  %arr_struct_load8 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca5
  %arr_struct_alloca9 = alloca <{ i32, i8* }>
  %arr_elem_alloca10 = alloca i8, i32 93
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca10, i8* getelementptr inbounds ([94 x i8], [94 x i8]* @str.18, i32 0, i32 0), i32 93, i32 0, i1 false)
  %gep_arr_elem_ptr11 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca9, i32 0, i32 0
  store i32 93, i32* %gep_arr_elem_ptr11
  %gep_arr_elem_ptr12 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca9, i32 0, i32 1
  store i8* %arr_elem_alloca10, i8** %gep_arr_elem_ptr12
  %arr_struct_load13 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca9
  br label %entry

entry:                                            ; preds = %vars
  store i64 %value, i64* %v
  %struct_field_ptr = getelementptr inbounds <{ <{ i32, i8* }>, i32 }>, <{ <{ i32, i8* }>, i32 }>* %pd, i32 0, i32 1
  store i32 0, i32* %struct_field_ptr
  %struct_field_ptr1 = getelementptr inbounds <{ <{ i32, i8* }>, i32 }>, <{ <{ i32, i8* }>, i32 }>* %pd, i32 0, i32 0
  store <{ i32, i8* }> %arr_struct_load, <{ i32, i8* }>* %struct_field_ptr1
  %struct_field_ptr3 = getelementptr inbounds <{ <{ i32, i8* }>, i32 }>, <{ <{ i32, i8* }>, i32 }>* %pd, i32 0, i32 0
  %struct_field_ptr4 = getelementptr inbounds <{ i32, i8* }>, <{ i32, i8* }>* %struct_field_ptr3, i32 0, i32 0
  %struct_field = load i32, i32* %struct_field_ptr4
  %icmp_tmp = icmp eq i32 %struct_field, 64
  call void @assert(i1 %icmp_tmp, <{ i32, i8* }> %arr_struct_load8, <{ i32, i8* }> %arr_struct_load13)
  br i1 %signed, label %cand.rhs, label %cand.end

cand.rhs:                                         ; preds = %entry
  %v14 = load i64, i64* %v
  %icmp_tmp15 = icmp slt i64 %v14, 0
  br label %cand.end

cand.end:                                         ; preds = %cand.rhs, %entry
  %candphi = phi i1 [ false, %entry ], [ %icmp_tmp15, %cand.rhs ]
  br i1 %candphi, label %then, label %endif

then:                                             ; preds = %cand.end
  %v16 = load i64, i64* %v
  %neg_tmp = sub i64 0, %v16
  store i64 %neg_tmp, i64* %v
  call void @out_char(<{ <{ i32, i8* }>, i32 }>* %pd, i8 45)
  br label %endif

endif:                                            ; preds = %then, %cand.end
  %v17 = load i64, i64* %v
  call void @u64_to_ascii(<{ <{ i32, i8* }>, i32 }>* %pd, i64 %v17)
  %struct_field_ptr18 = getelementptr inbounds <{ <{ i32, i8* }>, i32 }>, <{ <{ i32, i8* }>, i32 }>* %pd, i32 0, i32 0
  %struct_field_ptr19 = getelementptr inbounds <{ i32, i8* }>, <{ i32, i8* }>* %struct_field_ptr18, i32 0, i32 0
  %struct_field_ptr20 = getelementptr inbounds <{ <{ i32, i8* }>, i32 }>, <{ <{ i32, i8* }>, i32 }>* %pd, i32 0, i32 1
  %struct_field21 = load i32, i32* %struct_field_ptr20
  store i32 %struct_field21, i32* %struct_field_ptr19
  %struct_field_ptr22 = getelementptr inbounds <{ <{ i32, i8* }>, i32 }>, <{ <{ i32, i8* }>, i32 }>* %pd, i32 0, i32 0
  %struct_field23 = load <{ i32, i8* }>, <{ i32, i8* }>* %struct_field_ptr22
  call void @print_string(<{ i32, i8* }> %struct_field23)
  ret void
}

; Function Attrs: nounwind
define internal void @print_f32(float %value, i32 %precision) #0 {
vars:
  br label %entry

entry:                                            ; preds = %vars
  %fp_to_fp_cast = fpext float %value to double
  call void @print_f64(double %fp_to_fp_cast, i32 %precision)
  ret void
}

; Function Attrs: nounwind
define internal void @out_char(<{ <{ i32, i8* }>, i32 }>* %dest, i8 %char) #0 {
vars:
  %arr_struct_alloca = alloca <{ i32, i8* }>
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.19, i32 0, i32 0), i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr
  %gep_arr_elem_ptr6 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 1
  store i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.19, i32 0, i32 0), i8** %gep_arr_elem_ptr6
  %arr_struct_load = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca
  %arr_struct_alloca7 = alloca <{ i32, i8* }>
  %arr_elem_alloca = alloca i8, i32 93
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca, i8* getelementptr inbounds ([94 x i8], [94 x i8]* @str.20, i32 0, i32 0), i32 93, i32 0, i1 false)
  %gep_arr_elem_ptr8 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca7, i32 0, i32 0
  store i32 93, i32* %gep_arr_elem_ptr8
  %gep_arr_elem_ptr9 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca7, i32 0, i32 1
  store i8* %arr_elem_alloca, i8** %gep_arr_elem_ptr9
  %arr_struct_load10 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca7
  br label %entry

entry:                                            ; preds = %vars
  %struct_field_ptr = getelementptr inbounds <{ <{ i32, i8* }>, i32 }>, <{ <{ i32, i8* }>, i32 }>* %dest, i32 0, i32 1
  %struct_arrow = load i32, i32* %struct_field_ptr
  %icmp_tmp = icmp sge i32 %struct_arrow, 0
  br i1 %icmp_tmp, label %cand.rhs, label %cand.end

cand.rhs:                                         ; preds = %entry
  %struct_field_ptr1 = getelementptr inbounds <{ <{ i32, i8* }>, i32 }>, <{ <{ i32, i8* }>, i32 }>* %dest, i32 0, i32 1
  %struct_arrow2 = load i32, i32* %struct_field_ptr1
  %struct_field_ptr3 = getelementptr inbounds <{ <{ i32, i8* }>, i32 }>, <{ <{ i32, i8* }>, i32 }>* %dest, i32 0, i32 0
  %struct_field_ptr4 = getelementptr inbounds <{ i32, i8* }>, <{ i32, i8* }>* %struct_field_ptr3, i32 0, i32 0
  %struct_field = load i32, i32* %struct_field_ptr4
  %icmp_tmp5 = icmp slt i32 %struct_arrow2, %struct_field
  br label %cand.end

cand.end:                                         ; preds = %cand.rhs, %entry
  %candphi = phi i1 [ false, %entry ], [ %icmp_tmp5, %cand.rhs ]
  call void @assert(i1 %candphi, <{ i32, i8* }> %arr_struct_load, <{ i32, i8* }> %arr_struct_load10)
  %struct_field_ptr11 = getelementptr inbounds <{ <{ i32, i8* }>, i32 }>, <{ <{ i32, i8* }>, i32 }>* %dest, i32 0, i32 0
  %struct_field_ptr12 = getelementptr inbounds <{ <{ i32, i8* }>, i32 }>, <{ <{ i32, i8* }>, i32 }>* %dest, i32 0, i32 1
  %postinc_load = load i32, i32* %struct_field_ptr12
  %postinc = add i32 %postinc_load, 1
  store i32 %postinc, i32* %struct_field_ptr12
  %gep_arr_elem_ptr13 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %struct_field_ptr11, i32 0, i32 1
  %arr_elem_ptr = load i8*, i8** %gep_arr_elem_ptr13
  %gep_arr_elem = getelementptr i8, i8* %arr_elem_ptr, i32 %postinc_load
  store i8 %char, i8* %gep_arr_elem
  ret void
}

; Function Attrs: nounwind
define internal void @u64_to_ascii(<{ <{ i32, i8* }>, i32 }>* %dest, i64 %value) #0 {
vars:
  %v = alloca i64
  %start = alloca i8*
  %index = alloca i32
  %arr_struct_alloca = alloca <{ i32, i8* }>
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.21, i32 0, i32 0), i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr6 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr6
  %gep_arr_elem_ptr7 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 1
  store i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.21, i32 0, i32 0), i8** %gep_arr_elem_ptr7
  %arr_struct_load = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca
  %arr_struct_alloca8 = alloca <{ i32, i8* }>
  %arr_elem_alloca = alloca i8, i32 93
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca, i8* getelementptr inbounds ([94 x i8], [94 x i8]* @str.22, i32 0, i32 0), i32 93, i32 0, i1 false)
  %gep_arr_elem_ptr9 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca8, i32 0, i32 0
  store i32 93, i32* %gep_arr_elem_ptr9
  %gep_arr_elem_ptr10 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca8, i32 0, i32 1
  store i8* %arr_elem_alloca, i8** %gep_arr_elem_ptr10
  %arr_struct_load11 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca8
  %digit = alloca i8
  %end = alloca i8*
  %temp = alloca i8
  br label %entry

entry:                                            ; preds = %vars
  store i64 %value, i64* %v
  %struct_field_ptr = getelementptr inbounds <{ <{ i32, i8* }>, i32 }>, <{ <{ i32, i8* }>, i32 }>* %dest, i32 0, i32 0
  %struct_field_ptr1 = getelementptr inbounds <{ <{ i32, i8* }>, i32 }>, <{ <{ i32, i8* }>, i32 }>* %dest, i32 0, i32 1
  %struct_arrow = load i32, i32* %struct_field_ptr1
  %gep_arr_elem_ptr = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %struct_field_ptr, i32 0, i32 1
  %arr_elem_ptr = load i8*, i8** %gep_arr_elem_ptr
  %gep_arr_elem = getelementptr i8, i8* %arr_elem_ptr, i32 %struct_arrow
  store i8* %gep_arr_elem, i8** %start
  br label %while_cond

while_cond:                                       ; preds = %endif, %entry
  br i1 true, label %while, label %while_end

while:                                            ; preds = %while_cond
  %v2 = load i64, i64* %v
  %urem_tmp = urem i64 %v2, 10
  %int_trunc = trunc i64 %urem_tmp to i32
  store i32 %int_trunc, i32* %index
  %index3 = load i32, i32* %index
  %icmp_tmp = icmp sge i32 %index3, 0
  br i1 %icmp_tmp, label %cand.rhs, label %cand.end

cand.rhs:                                         ; preds = %while
  %index4 = load i32, i32* %index
  %icmp_tmp5 = icmp slt i32 %index4, 10
  br label %cand.end

cand.end:                                         ; preds = %cand.rhs, %while
  %candphi = phi i1 [ false, %while ], [ %icmp_tmp5, %cand.rhs ]
  call void @assert(i1 %candphi, <{ i32, i8* }> %arr_struct_load, <{ i32, i8* }> %arr_struct_load11)
  %index12 = load i32, i32* %index
  %arr_elem_ptr13 = load i8*, i8** getelementptr inbounds (<{ i32, i8* }>, <{ i32, i8* }>* @decimal_digits, i32 0, i32 1)
  %gep_arr_elem14 = getelementptr i8, i8* %arr_elem_ptr13, i32 %index12
  %arr_elem = load i8, i8* %gep_arr_elem14
  store i8 %arr_elem, i8* %digit
  %digit15 = load i8, i8* %digit
  call void @out_char(<{ <{ i32, i8* }>, i32 }>* %dest, i8 %digit15)
  %v16 = load i64, i64* %v
  %div_tmp = udiv i64 %v16, 10
  store i64 %div_tmp, i64* %v
  %v17 = load i64, i64* %v
  %icmp_tmp18 = icmp eq i64 %v17, 0
  br i1 %icmp_tmp18, label %then, label %endif

then:                                             ; preds = %cand.end
  br label %while_end

endif:                                            ; preds = %cand.end
  br label %while_cond

while_end:                                        ; preds = %then, %while_cond
  %struct_field_ptr19 = getelementptr inbounds <{ <{ i32, i8* }>, i32 }>, <{ <{ i32, i8* }>, i32 }>* %dest, i32 0, i32 0
  %struct_field_ptr20 = getelementptr inbounds <{ <{ i32, i8* }>, i32 }>, <{ <{ i32, i8* }>, i32 }>* %dest, i32 0, i32 1
  %struct_arrow21 = load i32, i32* %struct_field_ptr20
  %gep_arr_elem_ptr22 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %struct_field_ptr19, i32 0, i32 1
  %arr_elem_ptr23 = load i8*, i8** %gep_arr_elem_ptr22
  %gep_arr_elem24 = getelementptr i8, i8* %arr_elem_ptr23, i32 %struct_arrow21
  store i8* %gep_arr_elem24, i8** %end
  br label %while_cond25

while_cond25:                                     ; preds = %while26, %while_end
  %start28 = load i8*, i8** %start
  %end29 = load i8*, i8** %end
  %icmp_tmp30 = icmp ult i8* %start28, %end29
  br i1 %icmp_tmp30, label %while26, label %while_end27

while26:                                          ; preds = %while_cond25
  %predec_load = load i8*, i8** %end
  %ptr_pre_dec = getelementptr i8, i8* %predec_load, i32 -1
  store i8* %ptr_pre_dec, i8** %end
  %end31 = load i8*, i8** %end
  %deref = load i8, i8* %end31
  store i8 %deref, i8* %temp
  %end32 = load i8*, i8** %end
  %start33 = load i8*, i8** %start
  %deref34 = load i8, i8* %start33
  store i8 %deref34, i8* %end32
  %start35 = load i8*, i8** %start
  %temp36 = load i8, i8* %temp
  store i8 %temp36, i8* %start35
  %preinc_load = load i8*, i8** %start
  %ptr_pre_inc = getelementptr i8, i8* %preinc_load, i32 1
  store i8* %ptr_pre_inc, i8** %start
  br label %while_cond25

while_end27:                                      ; preds = %while_cond25
  ret void
}

; Function Attrs: nounwind
define internal void @print_f64(double %value, i32 %precision) #0 {
vars:
  %v = alloca double
  %pd = alloca <{ <{ i32, i8* }>, i32 }>
  %arr_struct_alloca = alloca <{ i32, i8* }>
  %arr_elem_alloca = alloca i8, i32 64
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca, i8* getelementptr inbounds ([65 x i8], [65 x i8]* @str.16, i32 0, i32 0), i32 64, i32 0, i1 false)
  %gep_arr_elem_ptr = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 0
  store i32 64, i32* %gep_arr_elem_ptr
  %gep_arr_elem_ptr2 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 1
  store i8* %arr_elem_alloca, i8** %gep_arr_elem_ptr2
  %arr_struct_load = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca
  %arr_struct_alloca5 = alloca <{ i32, i8* }>
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.23, i32 0, i32 0), i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr6 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca5, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr6
  %gep_arr_elem_ptr7 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca5, i32 0, i32 1
  store i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.23, i32 0, i32 0), i8** %gep_arr_elem_ptr7
  %arr_struct_load8 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca5
  %arr_struct_alloca9 = alloca <{ i32, i8* }>
  %arr_elem_alloca10 = alloca i8, i32 93
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca10, i8* getelementptr inbounds ([94 x i8], [94 x i8]* @str.24, i32 0, i32 0), i32 93, i32 0, i1 false)
  %gep_arr_elem_ptr11 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca9, i32 0, i32 0
  store i32 93, i32* %gep_arr_elem_ptr11
  %gep_arr_elem_ptr12 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca9, i32 0, i32 1
  store i8* %arr_elem_alloca10, i8** %gep_arr_elem_ptr12
  %arr_struct_load13 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca9
  %int_part = alloca i64
  %first_fraction_char = alloca i32
  %last_non_zero = alloca i32
  %i = alloca i32
  %int_part29 = alloca i32
  %arr_struct_alloca38 = alloca <{ i32, i8* }>
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.25, i32 0, i32 0), i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr39 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca38, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr39
  %gep_arr_elem_ptr40 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca38, i32 0, i32 1
  store i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.25, i32 0, i32 0), i8** %gep_arr_elem_ptr40
  %arr_struct_load41 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca38
  %arr_struct_alloca42 = alloca <{ i32, i8* }>
  %arr_elem_alloca43 = alloca i8, i32 93
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca43, i8* getelementptr inbounds ([94 x i8], [94 x i8]* @str.26, i32 0, i32 0), i32 93, i32 0, i1 false)
  %gep_arr_elem_ptr44 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca42, i32 0, i32 0
  store i32 93, i32* %gep_arr_elem_ptr44
  %gep_arr_elem_ptr45 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca42, i32 0, i32 1
  store i8* %arr_elem_alloca43, i8** %gep_arr_elem_ptr45
  %arr_struct_load46 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca42
  %arr_struct_alloca60 = alloca <{ i32, i8* }>
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.27, i32 0, i32 0), i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr61 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca60, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr61
  %gep_arr_elem_ptr62 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca60, i32 0, i32 1
  store i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.27, i32 0, i32 0), i8** %gep_arr_elem_ptr62
  %arr_struct_load63 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca60
  %arr_struct_alloca64 = alloca <{ i32, i8* }>
  %arr_elem_alloca65 = alloca i8, i32 93
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca65, i8* getelementptr inbounds ([94 x i8], [94 x i8]* @str.28, i32 0, i32 0), i32 93, i32 0, i1 false)
  %gep_arr_elem_ptr66 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca64, i32 0, i32 0
  store i32 93, i32* %gep_arr_elem_ptr66
  %gep_arr_elem_ptr67 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca64, i32 0, i32 1
  store i8* %arr_elem_alloca65, i8** %gep_arr_elem_ptr67
  %arr_struct_load68 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca64
  br label %entry

entry:                                            ; preds = %vars
  store double %value, double* %v
  %struct_field_ptr = getelementptr inbounds <{ <{ i32, i8* }>, i32 }>, <{ <{ i32, i8* }>, i32 }>* %pd, i32 0, i32 1
  store i32 0, i32* %struct_field_ptr
  %struct_field_ptr1 = getelementptr inbounds <{ <{ i32, i8* }>, i32 }>, <{ <{ i32, i8* }>, i32 }>* %pd, i32 0, i32 0
  store <{ i32, i8* }> %arr_struct_load, <{ i32, i8* }>* %struct_field_ptr1
  %struct_field_ptr3 = getelementptr inbounds <{ <{ i32, i8* }>, i32 }>, <{ <{ i32, i8* }>, i32 }>* %pd, i32 0, i32 0
  %struct_field_ptr4 = getelementptr inbounds <{ i32, i8* }>, <{ i32, i8* }>* %struct_field_ptr3, i32 0, i32 0
  %struct_field = load i32, i32* %struct_field_ptr4
  %icmp_tmp = icmp eq i32 %struct_field, 64
  call void @assert(i1 %icmp_tmp, <{ i32, i8* }> %arr_struct_load8, <{ i32, i8* }> %arr_struct_load13)
  %v14 = load double, double* %v
  %fcmp_tmp = fcmp olt double %v14, 0.000000e+00
  br i1 %fcmp_tmp, label %then, label %endif

then:                                             ; preds = %entry
  %v15 = load double, double* %v
  %fneg_tmp = fsub double -0.000000e+00, %v15
  store double %fneg_tmp, double* %v
  call void @out_char(<{ <{ i32, i8* }>, i32 }>* %pd, i8 45)
  br label %endif

endif:                                            ; preds = %then, %entry
  %v16 = load double, double* %v
  %int_cast = fptoui double %v16 to i64
  store i64 %int_cast, i64* %int_part
  %int_part17 = load i64, i64* %int_part
  call void @u64_to_ascii(<{ <{ i32, i8* }>, i32 }>* %pd, i64 %int_part17)
  %v18 = load double, double* %v
  %int_part19 = load i64, i64* %int_part
  %int_to_float_cast = sitofp i64 %int_part19 to double
  %fsub_tmp = fsub double %v18, %int_to_float_cast
  store double %fsub_tmp, double* %v
  call void @out_char(<{ <{ i32, i8* }>, i32 }>* %pd, i8 46)
  %struct_field_ptr20 = getelementptr inbounds <{ <{ i32, i8* }>, i32 }>, <{ <{ i32, i8* }>, i32 }>* %pd, i32 0, i32 1
  %struct_field21 = load i32, i32* %struct_field_ptr20
  store i32 %struct_field21, i32* %first_fraction_char
  %struct_field_ptr22 = getelementptr inbounds <{ <{ i32, i8* }>, i32 }>, <{ <{ i32, i8* }>, i32 }>* %pd, i32 0, i32 1
  %struct_field23 = load i32, i32* %struct_field_ptr22
  store i32 %struct_field23, i32* %last_non_zero
  store i32 0, i32* %i
  br label %for_cond

for_cond:                                         ; preds = %for_iter, %endif
  %i24 = load i32, i32* %i
  %icmp_tmp25 = icmp slt i32 %i24, %precision
  br i1 %icmp_tmp25, label %for, label %end_for

for:                                              ; preds = %for_cond
  %v26 = load double, double* %v
  %fmul_tmp = fmul double %v26, 1.000000e+01
  store double %fmul_tmp, double* %v
  %v27 = load double, double* %v
  %int_cast28 = fptosi double %v27 to i32
  store i32 %int_cast28, i32* %int_part29
  %v30 = load double, double* %v
  %int_part31 = load i32, i32* %int_part29
  %int_to_float_cast32 = sitofp i32 %int_part31 to double
  %fsub_tmp33 = fsub double %v30, %int_to_float_cast32
  store double %fsub_tmp33, double* %v
  %int_part34 = load i32, i32* %int_part29
  %icmp_tmp35 = icmp slt i32 %int_part34, 10
  br i1 %icmp_tmp35, label %cand.rhs, label %cand.end

cand.rhs:                                         ; preds = %for
  %int_part36 = load i32, i32* %int_part29
  %icmp_tmp37 = icmp sge i32 %int_part36, 0
  br label %cand.end

cand.end:                                         ; preds = %cand.rhs, %for
  %candphi = phi i1 [ false, %for ], [ %icmp_tmp37, %cand.rhs ]
  call void @assert(i1 %candphi, <{ i32, i8* }> %arr_struct_load41, <{ i32, i8* }> %arr_struct_load46)
  %int_part47 = load i32, i32* %int_part29
  %arr_elem_ptr = load i8*, i8** getelementptr inbounds (<{ i32, i8* }>, <{ i32, i8* }>* @decimal_digits, i32 0, i32 1)
  %gep_arr_elem = getelementptr i8, i8* %arr_elem_ptr, i32 %int_part47
  %arr_elem = load i8, i8* %gep_arr_elem
  call void @out_char(<{ <{ i32, i8* }>, i32 }>* %pd, i8 %arr_elem)
  %int_part48 = load i32, i32* %int_part29
  %icmp_tmp49 = icmp ne i32 %int_part48, 0
  br i1 %icmp_tmp49, label %then50, label %endif51

then50:                                           ; preds = %cand.end
  %struct_field_ptr52 = getelementptr inbounds <{ <{ i32, i8* }>, i32 }>, <{ <{ i32, i8* }>, i32 }>* %pd, i32 0, i32 1
  %struct_field53 = load i32, i32* %struct_field_ptr52
  store i32 %struct_field53, i32* %last_non_zero
  br label %endif51

endif51:                                          ; preds = %then50, %cand.end
  br label %for_iter

for_iter:                                         ; preds = %endif51
  %preinc_load = load i32, i32* %i
  %preinc = add i32 %preinc_load, 1
  store i32 %preinc, i32* %i
  br label %for_cond

end_for:                                          ; preds = %for_cond
  %struct_field_ptr54 = getelementptr inbounds <{ <{ i32, i8* }>, i32 }>, <{ <{ i32, i8* }>, i32 }>* %pd, i32 0, i32 1
  %struct_field55 = load i32, i32* %struct_field_ptr54
  %struct_field_ptr56 = getelementptr inbounds <{ <{ i32, i8* }>, i32 }>, <{ <{ i32, i8* }>, i32 }>* %pd, i32 0, i32 0
  %struct_field_ptr57 = getelementptr inbounds <{ i32, i8* }>, <{ i32, i8* }>* %struct_field_ptr56, i32 0, i32 0
  %struct_field58 = load i32, i32* %struct_field_ptr57
  %icmp_tmp59 = icmp sle i32 %struct_field55, %struct_field58
  call void @assert(i1 %icmp_tmp59, <{ i32, i8* }> %arr_struct_load63, <{ i32, i8* }> %arr_struct_load68)
  %struct_field_ptr69 = getelementptr inbounds <{ <{ i32, i8* }>, i32 }>, <{ <{ i32, i8* }>, i32 }>* %pd, i32 0, i32 0
  %struct_field_ptr70 = getelementptr inbounds <{ i32, i8* }>, <{ i32, i8* }>* %struct_field_ptr69, i32 0, i32 0
  %struct_field_ptr71 = getelementptr inbounds <{ <{ i32, i8* }>, i32 }>, <{ <{ i32, i8* }>, i32 }>* %pd, i32 0, i32 1
  %struct_field72 = load i32, i32* %struct_field_ptr71
  store i32 %struct_field72, i32* %struct_field_ptr70
  %struct_field_ptr73 = getelementptr inbounds <{ <{ i32, i8* }>, i32 }>, <{ <{ i32, i8* }>, i32 }>* %pd, i32 0, i32 0
  %struct_field74 = load <{ i32, i8* }>, <{ i32, i8* }>* %struct_field_ptr73
  call void @print_string(<{ i32, i8* }> %struct_field74)
  ret void
}

; Function Attrs: nounwind
define internal void @debug_print_i64(<{ i32, i8* }> %name, i64 %value, i1 %signed) #0 {
vars:
  %arr_struct_alloca = alloca <{ i32, i8* }>
  %arr_elem_alloca = alloca i8, i32 2
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca, i8* getelementptr inbounds ([3 x i8], [3 x i8]* @str.29, i32 0, i32 0), i32 2, i32 0, i1 false)
  %gep_arr_elem_ptr = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 0
  store i32 2, i32* %gep_arr_elem_ptr
  %gep_arr_elem_ptr1 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 1
  store i8* %arr_elem_alloca, i8** %gep_arr_elem_ptr1
  %arr_struct_load = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca
  %arr_struct_alloca2 = alloca <{ i32, i8* }>
  %arr_elem_alloca3 = alloca i8
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca3, i8* getelementptr inbounds ([2 x i8], [2 x i8]* @str.10, i32 0, i32 0), i32 1, i32 0, i1 false)
  %gep_arr_elem_ptr4 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2, i32 0, i32 0
  store i32 1, i32* %gep_arr_elem_ptr4
  %gep_arr_elem_ptr5 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2, i32 0, i32 1
  store i8* %arr_elem_alloca3, i8** %gep_arr_elem_ptr5
  %arr_struct_load6 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2
  br label %entry

entry:                                            ; preds = %vars
  call void @print_string(<{ i32, i8* }> %name)
  call void @print_string(<{ i32, i8* }> %arr_struct_load)
  call void @print_i64(i64 %value, i1 %signed)
  call void @print_string(<{ i32, i8* }> %arr_struct_load6)
  ret void
}

; Function Attrs: nounwind
define internal void @debug_print_i32(<{ i32, i8* }> %name, i32 %value, i1 %signed) #0 {
vars:
  %arr_struct_alloca = alloca <{ i32, i8* }>
  %arr_elem_alloca = alloca i8, i32 2
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca, i8* getelementptr inbounds ([3 x i8], [3 x i8]* @str.29, i32 0, i32 0), i32 2, i32 0, i1 false)
  %gep_arr_elem_ptr = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 0
  store i32 2, i32* %gep_arr_elem_ptr
  %gep_arr_elem_ptr1 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 1
  store i8* %arr_elem_alloca, i8** %gep_arr_elem_ptr1
  %arr_struct_load = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca
  %arr_struct_alloca2 = alloca <{ i32, i8* }>
  %arr_elem_alloca3 = alloca i8
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca3, i8* getelementptr inbounds ([2 x i8], [2 x i8]* @str.10, i32 0, i32 0), i32 1, i32 0, i1 false)
  %gep_arr_elem_ptr4 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2, i32 0, i32 0
  store i32 1, i32* %gep_arr_elem_ptr4
  %gep_arr_elem_ptr5 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2, i32 0, i32 1
  store i8* %arr_elem_alloca3, i8** %gep_arr_elem_ptr5
  %arr_struct_load6 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2
  br label %entry

entry:                                            ; preds = %vars
  call void @print_string(<{ i32, i8* }> %name)
  call void @print_string(<{ i32, i8* }> %arr_struct_load)
  call void @print_i32(i32 %value, i1 %signed)
  call void @print_string(<{ i32, i8* }> %arr_struct_load6)
  ret void
}

; Function Attrs: nounwind
define internal void @debug_print_f32(<{ i32, i8* }> %name, float %value, i32 %precision) #0 {
vars:
  %arr_struct_alloca = alloca <{ i32, i8* }>
  %arr_elem_alloca = alloca i8, i32 2
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca, i8* getelementptr inbounds ([3 x i8], [3 x i8]* @str.29, i32 0, i32 0), i32 2, i32 0, i1 false)
  %gep_arr_elem_ptr = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 0
  store i32 2, i32* %gep_arr_elem_ptr
  %gep_arr_elem_ptr1 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 1
  store i8* %arr_elem_alloca, i8** %gep_arr_elem_ptr1
  %arr_struct_load = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca
  %arr_struct_alloca2 = alloca <{ i32, i8* }>
  %arr_elem_alloca3 = alloca i8
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca3, i8* getelementptr inbounds ([2 x i8], [2 x i8]* @str.10, i32 0, i32 0), i32 1, i32 0, i1 false)
  %gep_arr_elem_ptr4 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2, i32 0, i32 0
  store i32 1, i32* %gep_arr_elem_ptr4
  %gep_arr_elem_ptr5 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2, i32 0, i32 1
  store i8* %arr_elem_alloca3, i8** %gep_arr_elem_ptr5
  %arr_struct_load6 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2
  br label %entry

entry:                                            ; preds = %vars
  call void @print_string(<{ i32, i8* }> %name)
  call void @print_string(<{ i32, i8* }> %arr_struct_load)
  call void @print_f32(float %value, i32 %precision)
  call void @print_string(<{ i32, i8* }> %arr_struct_load6)
  ret void
}

; Function Attrs: nounwind
define internal void @debug_print_f64(<{ i32, i8* }> %name, double %value, i32 %precision) #0 {
vars:
  %arr_struct_alloca = alloca <{ i32, i8* }>
  %arr_elem_alloca = alloca i8, i32 2
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca, i8* getelementptr inbounds ([3 x i8], [3 x i8]* @str.29, i32 0, i32 0), i32 2, i32 0, i1 false)
  %gep_arr_elem_ptr = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 0
  store i32 2, i32* %gep_arr_elem_ptr
  %gep_arr_elem_ptr1 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 1
  store i8* %arr_elem_alloca, i8** %gep_arr_elem_ptr1
  %arr_struct_load = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca
  %arr_struct_alloca2 = alloca <{ i32, i8* }>
  %arr_elem_alloca3 = alloca i8
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca3, i8* getelementptr inbounds ([2 x i8], [2 x i8]* @str.10, i32 0, i32 0), i32 1, i32 0, i1 false)
  %gep_arr_elem_ptr4 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2, i32 0, i32 0
  store i32 1, i32* %gep_arr_elem_ptr4
  %gep_arr_elem_ptr5 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2, i32 0, i32 1
  store i8* %arr_elem_alloca3, i8** %gep_arr_elem_ptr5
  %arr_struct_load6 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2
  br label %entry

entry:                                            ; preds = %vars
  call void @print_string(<{ i32, i8* }> %name)
  call void @print_string(<{ i32, i8* }> %arr_struct_load)
  call void @print_f64(double %value, i32 %precision)
  call void @print_string(<{ i32, i8* }> %arr_struct_load6)
  ret void
}

; Function Attrs: nounwind
define internal void @debug_print_bool(<{ i32, i8* }> %name, i1 %value) #0 {
vars:
  %arr_struct_alloca = alloca <{ i32, i8* }>
  %arr_elem_alloca = alloca i8, i32 2
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca, i8* getelementptr inbounds ([3 x i8], [3 x i8]* @str.29, i32 0, i32 0), i32 2, i32 0, i1 false)
  %gep_arr_elem_ptr = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 0
  store i32 2, i32* %gep_arr_elem_ptr
  %gep_arr_elem_ptr1 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 1
  store i8* %arr_elem_alloca, i8** %gep_arr_elem_ptr1
  %arr_struct_load = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca
  %arr_struct_alloca2 = alloca <{ i32, i8* }>
  %arr_elem_alloca3 = alloca i8, i32 4
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca3, i8* getelementptr inbounds ([5 x i8], [5 x i8]* @str.30, i32 0, i32 0), i32 4, i32 0, i1 false)
  %gep_arr_elem_ptr4 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2, i32 0, i32 0
  store i32 4, i32* %gep_arr_elem_ptr4
  %gep_arr_elem_ptr5 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2, i32 0, i32 1
  store i8* %arr_elem_alloca3, i8** %gep_arr_elem_ptr5
  %arr_struct_load6 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2
  %arr_struct_alloca7 = alloca <{ i32, i8* }>
  %arr_elem_alloca8 = alloca i8, i32 5
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca8, i8* getelementptr inbounds ([6 x i8], [6 x i8]* @str.31, i32 0, i32 0), i32 5, i32 0, i1 false)
  %gep_arr_elem_ptr9 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca7, i32 0, i32 0
  store i32 5, i32* %gep_arr_elem_ptr9
  %gep_arr_elem_ptr10 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca7, i32 0, i32 1
  store i8* %arr_elem_alloca8, i8** %gep_arr_elem_ptr10
  %arr_struct_load11 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca7
  %arr_struct_alloca12 = alloca <{ i32, i8* }>
  %arr_elem_alloca13 = alloca i8
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca13, i8* getelementptr inbounds ([2 x i8], [2 x i8]* @str.10, i32 0, i32 0), i32 1, i32 0, i1 false)
  %gep_arr_elem_ptr14 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca12, i32 0, i32 0
  store i32 1, i32* %gep_arr_elem_ptr14
  %gep_arr_elem_ptr15 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca12, i32 0, i32 1
  store i8* %arr_elem_alloca13, i8** %gep_arr_elem_ptr15
  %arr_struct_load16 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca12
  br label %entry

entry:                                            ; preds = %vars
  call void @print_string(<{ i32, i8* }> %name)
  call void @print_string(<{ i32, i8* }> %arr_struct_load)
  br i1 %value, label %then, label %else

then:                                             ; preds = %entry
  call void @print_string(<{ i32, i8* }> %arr_struct_load6)
  br label %endif

else:                                             ; preds = %entry
  call void @print_string(<{ i32, i8* }> %arr_struct_load11)
  br label %endif

endif:                                            ; preds = %else, %then
  call void @print_string(<{ i32, i8* }> %arr_struct_load16)
  ret void
}

; Function Attrs: nounwind
define internal i1 @__hack_reserve_intrinsics() #0 {
vars:
  %arr_struct_alloca = alloca <{ i32, i8* }>
  %arr_elem_alloca = alloca i8, i32 0
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca, i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr
  %gep_arr_elem_ptr10 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 1
  store i8* %arr_elem_alloca, i8** %gep_arr_elem_ptr10
  %arr_struct_load = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca
  br label %entry

entry:                                            ; preds = %vars
  br i1 icmp eq (i64 ptrtoint (i8* (i8*, i32, i64)* @memset to i64), i64 1234), label %cor.end, label %cor.rhs

cor.rhs:                                          ; preds = %entry
  br label %cor.end

cor.end:                                          ; preds = %cor.rhs, %entry
  %corphi = phi i1 [ true, %entry ], [ icmp eq (i64 ptrtoint (void ()* @__chkstk to i64), i64 1234), %cor.rhs ]
  br i1 %corphi, label %cor.end2, label %cor.rhs1

cor.rhs1:                                         ; preds = %cor.end
  br label %cor.end2

cor.end2:                                         ; preds = %cor.rhs1, %cor.end
  %corphi3 = phi i1 [ true, %cor.end ], [ icmp eq (i64 ptrtoint (float (float)* @cosf to i64), i64 1234), %cor.rhs1 ]
  br i1 %corphi3, label %cor.end5, label %cor.rhs4

cor.rhs4:                                         ; preds = %cor.end2
  br label %cor.end5

cor.end5:                                         ; preds = %cor.rhs4, %cor.end2
  %corphi6 = phi i1 [ true, %cor.end2 ], [ icmp eq (i64 ptrtoint (float (float)* @sinf to i64), i64 1234), %cor.rhs4 ]
  br i1 %corphi6, label %cor.end8, label %cor.rhs7

cor.rhs7:                                         ; preds = %cor.end5
  br label %cor.end8

cor.end8:                                         ; preds = %cor.rhs7, %cor.end5
  %corphi9 = phi i1 [ true, %cor.end5 ], [ icmp eq (i64 ptrtoint (float (float)* @roundf to i64), i64 1234), %cor.rhs7 ]
  br i1 %corphi9, label %then, label %endif

then:                                             ; preds = %cor.end8
  call void @print_string(<{ i32, i8* }> %arr_struct_load)
  ret i1 true

endif:                                            ; preds = %cor.end8
  ret i1 false
}

; Function Attrs: nounwind
declare i64 @CreateFileA(i8*, i32, i32, i8*, i32, i32, i64) #0

; Function Attrs: nounwind
declare i32 @CopyFileA(i8*, i8*, i32) #0

; Function Attrs: nounwind
declare i32 @GetFileTime(i64, i64*, i64*, i64*) #0

; Function Attrs: nounwind
declare i32 @GetFileAttributesExA(i8*, i32, <{ i32, i64, i64, i64, i64 }>*) #0

; Function Attrs: nounwind
declare i64 @FindFirstFileA(i8*, <{ i32, i64, i64, i64, i64, i64, i8*, i8* }>*) #0

; Function Attrs: nounwind
declare i32 @FindClose(i64) #0

; Function Attrs: nounwind
declare i32 @GetFileSizeEx(i64, i64*) #0

; Function Attrs: nounwind
declare i32 @GetLastError() #0

; Function Attrs: nounwind
declare i64 @GetStdHandle(i32) #0

; Function Attrs: nounwind
declare i64 @GetModuleHandleA(i64) #0

; Function Attrs: nounwind
declare i16 @RegisterClassExA(<{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>*) #0

; Function Attrs: nounwind
declare i64 @DefWindowProcA(i64, i32, i64, i64) #0

; Function Attrs: nounwind
declare void @ExitProcess(i32) #0

; Function Attrs: nounwind
declare i64 @CreateWindowExA(i32, i8*, i8*, i32, i32, i32, i32, i32, i64, i64, i64, i64) #0

; Function Attrs: nounwind
declare i32 @GetMessageA(<{ i64, i32, i64, i64, i32, <{ i32, i32 }>, i64 }>*, i64, i32, i32) #0

; Function Attrs: nounwind
declare i32 @PeekMessageA(<{ i64, i32, i64, i64, i32, <{ i32, i32 }>, i64 }>*, i64, i32, i32, i32) #0

; Function Attrs: nounwind
declare i32 @TranslateMessage(<{ i64, i32, i64, i64, i32, <{ i32, i32 }>, i64 }>*) #0

; Function Attrs: nounwind
declare i64 @DispatchMessageA(<{ i64, i32, i64, i64, i32, <{ i32, i32 }>, i64 }>*) #0

; Function Attrs: nounwind
declare i64 @BeginPaint(i64, <{ i64, i32, <{ i32, i32, i32, i32 }>, i32, i32, i64, i64, i64, i64 }>*) #0

; Function Attrs: nounwind
declare i32 @EndPaint(i64, <{ i64, i32, <{ i32, i32, i32, i32 }>, i32, i32, i64, i64, i64, i64 }>*) #0

; Function Attrs: nounwind
declare void @PostQuitMessage(i32) #0

; Function Attrs: nounwind
declare i32 @GetClientRect(i64, <{ i32, i32, i32, i32 }>*) #0

; Function Attrs: nounwind
declare i64 @CreateDIBSection(i64, <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>*, i32, i8**, i64, i32) #0

; Function Attrs: nounwind
declare i32 @StretchDIBits(i64, i32, i32, i32, i32, i32, i32, i32, i32, i8*, <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>*, i32, i32) #0

; Function Attrs: nounwind
declare i32 @PatBlt(i64, i32, i32, i32, i32, i32) #0

; Function Attrs: nounwind
declare i32 @CloseHandle(i64) #0

; Function Attrs: nounwind
declare i32 @DeleteObject(i64) #0

; Function Attrs: nounwind
declare i64 @CreateCompatibleDC(i64) #0

; Function Attrs: nounwind
declare i64 @GetDC(i64) #0

; Function Attrs: nounwind
declare i32 @ReleaseDC(i64, i64) #0

; Function Attrs: nounwind
declare i64 @GetCompatibleDC(i64) #0

; Function Attrs: nounwind
declare i32 @VirtualFree(i8*, i64, i32) #0

; Function Attrs: nounwind
declare i64 @LoadLibraryA(i8*) #0

; Function Attrs: nounwind
declare i32 @FreeLibrary(i64) #0

; Function Attrs: nounwind
declare i8* @GetProcAddress(i64, i8*) #0

; Function Attrs: nounwind
declare i32 @QueryPerformanceCounter(i64*) #0

; Function Attrs: nounwind
declare i32 @QueryPerformanceFrequency(i64*) #0

; Function Attrs: nounwind
declare void @Sleep(i32) #0

; Function Attrs: nounwind
declare i32 @timeBeginPeriod(i32) #0

; Function Attrs: nounwind
declare i32 @GetDeviceCaps(i64, i32) #0

; Function Attrs: nounwind
declare i32 @GetCursorPos(<{ i32, i32 }>*) #0

; Function Attrs: nounwind
declare i16 @GetKeyState(i32) #0

; Function Attrs: nounwind
declare i32 @ScreenToClient(i64, <{ i32, i32 }>*) #0

; Function Attrs: nounwind
define internal float @remainder(float %x, float %y) #0 {
vars:
  br label %entry

entry:                                            ; preds = %vars
  %fdiv_tmp = fdiv float %x, %y
  %fun_call = call float @llvm.floor.f32(float %fdiv_tmp)
  %fmul_tmp = fmul float %fun_call, %y
  %fsub_tmp = fsub float %x, %fmul_tmp
  ret float %fsub_tmp
}

; Function Attrs: nounwind
define internal float @roundf(float %x) #0 {
vars:
  br label %entry

entry:                                            ; preds = %vars
  %fadd_tmp = fadd float %x, 5.000000e-01
  %fun_call = call float @llvm.trunc.f32(float %fadd_tmp)
  ret float %fun_call
}

; Function Attrs: nounwind
define internal float @sinf(float %x) #0 {
vars:
  %y = alloca float
  br label %entry

entry:                                            ; preds = %vars
  %fun_call = call float @llvm.sqrt.f32(float 0x3FCCCCCCC0000000)
  %fmul_tmp = fmul float 1.600000e+01, %fun_call
  %fun_call1 = call float @llvm.sqrt.f32(float 0x3FCCCCCCC0000000)
  %fdiv_tmp = fdiv float 0x3FE8CCCCC0000000, %fun_call1
  %fdiv_tmp2 = fdiv float %x, 0x401921FB60000000
  store float %fdiv_tmp2, float* %y
  %y3 = load float, float* %y
  %y4 = load float, float* %y
  %fadd_tmp = fadd float %y4, 5.000000e-01
  %fun_call5 = call float @llvm.floor.f32(float %fadd_tmp)
  %fsub_tmp = fsub float %y3, %fun_call5
  store float %fsub_tmp, float* %y
  %y6 = load float, float* %y
  %fmul_tmp7 = fmul float %fmul_tmp, %y6
  %y8 = load float, float* %y
  %fun_call9 = call float @llvm.fabs.f32(float %y8)
  %fsub_tmp10 = fsub float 5.000000e-01, %fun_call9
  %fmul_tmp11 = fmul float %fmul_tmp7, %fsub_tmp10
  store float %fmul_tmp11, float* %y
  %y12 = load float, float* %y
  %y13 = load float, float* %y
  %fun_call14 = call float @llvm.fabs.f32(float %y13)
  %fadd_tmp15 = fadd float %fdiv_tmp, %fun_call14
  %fmul_tmp16 = fmul float %y12, %fadd_tmp15
  ret float %fmul_tmp16
}

; Function Attrs: nounwind
define internal float @cosf(float %value) #0 {
vars:
  %x = alloca float
  %y = alloca float
  br label %entry

entry:                                            ; preds = %vars
  %fun_call = call float @llvm.sqrt.f32(float 0x3FCCCCCCC0000000)
  %fmul_tmp = fmul float 1.600000e+01, %fun_call
  %fun_call1 = call float @llvm.sqrt.f32(float 0x3FCCCCCCC0000000)
  %fdiv_tmp = fdiv float 0x3FE8CCCCC0000000, %fun_call1
  %fsub_tmp = fsub float 0x3FF921FB60000000, %value
  store float %fsub_tmp, float* %x
  %x2 = load float, float* %x
  %fdiv_tmp3 = fdiv float %x2, 0x401921FB60000000
  store float %fdiv_tmp3, float* %y
  %y4 = load float, float* %y
  %y5 = load float, float* %y
  %fadd_tmp = fadd float %y5, 5.000000e-01
  %fun_call6 = call float @llvm.floor.f32(float %fadd_tmp)
  %fsub_tmp7 = fsub float %y4, %fun_call6
  store float %fsub_tmp7, float* %y
  %y8 = load float, float* %y
  %fmul_tmp9 = fmul float %fmul_tmp, %y8
  %y10 = load float, float* %y
  %fun_call11 = call float @llvm.fabs.f32(float %y10)
  %fsub_tmp12 = fsub float 5.000000e-01, %fun_call11
  %fmul_tmp13 = fmul float %fmul_tmp9, %fsub_tmp12
  store float %fmul_tmp13, float* %y
  %y14 = load float, float* %y
  %y15 = load float, float* %y
  %fun_call16 = call float @llvm.fabs.f32(float %y15)
  %fadd_tmp17 = fadd float %fdiv_tmp, %fun_call16
  %fmul_tmp18 = fmul float %y14, %fadd_tmp17
  ret float %fmul_tmp18
}

; Function Attrs: nounwind
define internal float @lerp(float %a, float %b, float %t) #0 {
vars:
  br label %entry

entry:                                            ; preds = %vars
  %fsub_tmp = fsub float %b, %a
  %fmul_tmp = fmul float %fsub_tmp, %t
  %fadd_tmp = fadd float %a, %fmul_tmp
  ret float %fadd_tmp
}

; Function Attrs: nounwind
define internal i32 @abs_i32(i32 %value) #0 {
vars:
  br label %entry

entry:                                            ; preds = %vars
  %icmp_tmp = icmp sge i32 %value, 0
  br i1 %icmp_tmp, label %then, label %else

then:                                             ; preds = %entry
  ret i32 %value

else:                                             ; preds = %entry
  %neg_tmp = sub i32 0, %value
  ret i32 %neg_tmp

endif:                                            ; No predecessors!
  ret i32 0
}

; Function Attrs: nounwind
define internal i32 @clamp_i32(i32 %value, i32 %min, i32 %max) #0 {
vars:
  %arr_struct_alloca = alloca <{ i32, i8* }>
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.14, i32 0, i32 0), i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr
  %gep_arr_elem_ptr1 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 1
  store i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.14, i32 0, i32 0), i8** %gep_arr_elem_ptr1
  %arr_struct_load = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca
  %arr_struct_alloca2 = alloca <{ i32, i8* }>
  %arr_elem_alloca = alloca i8, i32 93
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca, i8* getelementptr inbounds ([94 x i8], [94 x i8]* @str.15, i32 0, i32 0), i32 93, i32 0, i1 false)
  %gep_arr_elem_ptr3 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2, i32 0, i32 0
  store i32 93, i32* %gep_arr_elem_ptr3
  %gep_arr_elem_ptr4 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2, i32 0, i32 1
  store i8* %arr_elem_alloca, i8** %gep_arr_elem_ptr4
  %arr_struct_load5 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2
  %result = alloca i32
  br label %entry

entry:                                            ; preds = %vars
  %icmp_tmp = icmp sle i32 %min, %max
  call void @assert(i1 %icmp_tmp, <{ i32, i8* }> %arr_struct_load, <{ i32, i8* }> %arr_struct_load5)
  store i32 %value, i32* %result
  %result6 = load i32, i32* %result
  %icmp_tmp7 = icmp slt i32 %result6, %min
  br i1 %icmp_tmp7, label %then, label %elif_0

then:                                             ; preds = %entry
  store i32 %min, i32* %result
  br label %endif

elif_0:                                           ; preds = %entry
  %result8 = load i32, i32* %result
  %icmp_tmp9 = icmp sgt i32 %result8, %max
  br i1 %icmp_tmp9, label %elif_0_then, label %endif

elif_0_then:                                      ; preds = %elif_0
  store i32 %max, i32* %result
  br label %endif

endif:                                            ; preds = %elif_0_then, %elif_0, %then
  %result10 = load i32, i32* %result
  ret i32 %result10
}

; Function Attrs: nounwind
define internal i1 @platform_write_file(<{ i32, i8* }> %name, <{ i32, i8* }> %buffer) #0 {
vars:
  %arr_struct_alloca = alloca <{ i32, i8* }>
  %arr_elem_alloca = alloca i8, i32 99
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca, i8* getelementptr inbounds ([100 x i8], [100 x i8]* @str.32, i32 0, i32 0), i32 99, i32 0, i1 false)
  %gep_arr_elem_ptr = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 0
  store i32 99, i32* %gep_arr_elem_ptr
  %gep_arr_elem_ptr1 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 1
  store i8* %arr_elem_alloca, i8** %gep_arr_elem_ptr1
  %arr_struct_load = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca
  %handle = alloca i64
  %bytes_written = alloca i32
  %result = alloca i32
  %arr_struct_alloca18 = alloca <{ i32, i8* }>
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.33, i32 0, i32 0), i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr19 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca18, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr19
  %gep_arr_elem_ptr20 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca18, i32 0, i32 1
  store i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.33, i32 0, i32 0), i8** %gep_arr_elem_ptr20
  %arr_struct_load21 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca18
  %arr_struct_alloca22 = alloca <{ i32, i8* }>
  %arr_elem_alloca23 = alloca i8, i32 99
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca23, i8* getelementptr inbounds ([100 x i8], [100 x i8]* @str.34, i32 0, i32 0), i32 99, i32 0, i1 false)
  %gep_arr_elem_ptr24 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca22, i32 0, i32 0
  store i32 99, i32* %gep_arr_elem_ptr24
  %gep_arr_elem_ptr25 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca22, i32 0, i32 1
  store i8* %arr_elem_alloca23, i8** %gep_arr_elem_ptr25
  %arr_struct_load26 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca22
  br label %entry

entry:                                            ; preds = %vars
  %fun_call = call i8* @cstr(<{ i32, i8* }> %name, <{ i32, i8* }> %arr_struct_load)
  %fun_call2 = call i64 @CreateFileA(i8* %fun_call, i32 1073741824, i32 0, i8* null, i32 2, i32 0, i64 0)
  store i64 %fun_call2, i64* %handle
  %handle3 = load i64, i64* %handle
  %icmp_tmp = icmp eq i64 %handle3, 0
  br i1 %icmp_tmp, label %then, label %endif

then:                                             ; preds = %entry
  %handle4 = load i64, i64* %handle
  %fun_call5 = call i32 @CloseHandle(i64 %handle4)
  ret i1 false

endif:                                            ; preds = %entry
  store i32 0, i32* %bytes_written
  %handle6 = load i64, i64* %handle
  %struct_field_extract = extractvalue <{ i32, i8* }> %buffer, 1
  %struct_field_extract7 = extractvalue <{ i32, i8* }> %buffer, 0
  %fun_call8 = call i32 @WriteFile(i64 %handle6, i8* %struct_field_extract, i32 %struct_field_extract7, i32* %bytes_written, i8* null)
  store i32 %fun_call8, i32* %result
  %result9 = load i32, i32* %result
  %icmp_tmp10 = icmp ne i32 %result9, 0
  br i1 %icmp_tmp10, label %then11, label %else

then11:                                           ; preds = %endif
  %handle13 = load i64, i64* %handle
  %fun_call14 = call i32 @CloseHandle(i64 %handle13)
  %struct_field_extract15 = extractvalue <{ i32, i8* }> %buffer, 0
  %bytes_written16 = load i32, i32* %bytes_written
  %icmp_tmp17 = icmp eq i32 %struct_field_extract15, %bytes_written16
  call void @assert(i1 %icmp_tmp17, <{ i32, i8* }> %arr_struct_load21, <{ i32, i8* }> %arr_struct_load26)
  ret i1 true

else:                                             ; preds = %endif
  %handle27 = load i64, i64* %handle
  %fun_call28 = call i32 @CloseHandle(i64 %handle27)
  ret i1 false

endif12:                                          ; No predecessors!
  %handle29 = load i64, i64* %handle
  %fun_call30 = call i32 @CloseHandle(i64 %handle29)
  ret i1 false
}

; Function Attrs: nounwind
define internal <{ i32, i8* }> @platform_read_file(<{ i32, i8* }> %name) #0 {
vars:
  %result = alloca <{ i32, i8* }>
  %arr_struct_alloca = alloca <{ i32, i8* }>
  %arr_elem_alloca = alloca i8, i32 100
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca, i8* getelementptr inbounds ([101 x i8], [101 x i8]* @str.35, i32 0, i32 0), i32 100, i32 0, i1 false)
  %gep_arr_elem_ptr = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 0
  store i32 100, i32* %gep_arr_elem_ptr
  %gep_arr_elem_ptr2 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 1
  store i8* %arr_elem_alloca, i8** %gep_arr_elem_ptr2
  %arr_struct_load = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca
  %handle = alloca i64
  %size = alloca i64
  %fsr = alloca i32
  %buffer = alloca i8*
  %arr_struct_alloca29 = alloca <{ i32, i8* }>
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.36, i32 0, i32 0), i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr30 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca29, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr30
  %gep_arr_elem_ptr31 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca29, i32 0, i32 1
  store i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.36, i32 0, i32 0), i8** %gep_arr_elem_ptr31
  %arr_struct_load32 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca29
  %arr_struct_alloca33 = alloca <{ i32, i8* }>
  %arr_elem_alloca34 = alloca i8, i32 99
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca34, i8* getelementptr inbounds ([100 x i8], [100 x i8]* @str.37, i32 0, i32 0), i32 99, i32 0, i1 false)
  %gep_arr_elem_ptr35 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca33, i32 0, i32 0
  store i32 99, i32* %gep_arr_elem_ptr35
  %gep_arr_elem_ptr36 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca33, i32 0, i32 1
  store i8* %arr_elem_alloca34, i8** %gep_arr_elem_ptr36
  %arr_struct_load37 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca33
  %size_32 = alloca i32
  %bytes_read = alloca i32
  %rfr = alloca i32
  %arr_struct_alloca46 = alloca <{ i32, i8* }>
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.38, i32 0, i32 0), i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr47 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca46, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr47
  %gep_arr_elem_ptr48 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca46, i32 0, i32 1
  store i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.38, i32 0, i32 0), i8** %gep_arr_elem_ptr48
  %arr_struct_load49 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca46
  %arr_struct_alloca50 = alloca <{ i32, i8* }>
  %arr_elem_alloca51 = alloca i8, i32 99
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca51, i8* getelementptr inbounds ([100 x i8], [100 x i8]* @str.39, i32 0, i32 0), i32 99, i32 0, i1 false)
  %gep_arr_elem_ptr52 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca50, i32 0, i32 0
  store i32 99, i32* %gep_arr_elem_ptr52
  %gep_arr_elem_ptr53 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca50, i32 0, i32 1
  store i8* %arr_elem_alloca51, i8** %gep_arr_elem_ptr53
  %arr_struct_load54 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca50
  br label %entry

entry:                                            ; preds = %vars
  %struct_field_ptr = getelementptr inbounds <{ i32, i8* }>, <{ i32, i8* }>* %result, i32 0, i32 1
  store i8* null, i8** %struct_field_ptr
  %struct_field_ptr1 = getelementptr inbounds <{ i32, i8* }>, <{ i32, i8* }>* %result, i32 0, i32 0
  store i32 0, i32* %struct_field_ptr1
  %fun_call = call i8* @cstr(<{ i32, i8* }> %name, <{ i32, i8* }> %arr_struct_load)
  %fun_call3 = call i64 @CreateFileA(i8* %fun_call, i32 -2147483648, i32 1, i8* null, i32 3, i32 0, i64 0)
  store i64 %fun_call3, i64* %handle
  %handle4 = load i64, i64* %handle
  %icmp_tmp = icmp eq i64 %handle4, 0
  br i1 %icmp_tmp, label %then, label %endif

then:                                             ; preds = %entry
  %result5 = load <{ i32, i8* }>, <{ i32, i8* }>* %result
  ret <{ i32, i8* }> %result5

endif:                                            ; preds = %entry
  store i64 0, i64* %size
  %handle6 = load i64, i64* %handle
  %fun_call7 = call i32 @GetFileSizeEx(i64 %handle6, i64* %size)
  store i32 %fun_call7, i32* %fsr
  %fsr8 = load i32, i32* %fsr
  %icmp_tmp9 = icmp eq i32 %fsr8, 0
  br i1 %icmp_tmp9, label %cor.end, label %cor.rhs

cor.rhs:                                          ; preds = %endif
  %size10 = load i64, i64* %size
  %icmp_tmp11 = icmp eq i64 %size10, 0
  br label %cor.end

cor.end:                                          ; preds = %cor.rhs, %endif
  %corphi = phi i1 [ true, %endif ], [ %icmp_tmp11, %cor.rhs ]
  br i1 %corphi, label %then12, label %endif13

then12:                                           ; preds = %cor.end
  %handle14 = load i64, i64* %handle
  %fun_call15 = call i32 @CloseHandle(i64 %handle14)
  %result16 = load <{ i32, i8* }>, <{ i32, i8* }>* %result
  ret <{ i32, i8* }> %result16

endif13:                                          ; preds = %cor.end
  %size17 = load i64, i64* %size
  %fun_call18 = call i8* @VirtualAlloc(i8* null, i64 %size17, i32 4096, i32 4)
  store i8* %fun_call18, i8** %buffer
  %buffer19 = load i8*, i8** %buffer
  %int_cast = ptrtoint i8* %buffer19 to i64
  %icmp_tmp20 = icmp eq i64 %int_cast, 0
  br i1 %icmp_tmp20, label %then21, label %endif22

then21:                                           ; preds = %endif13
  %handle23 = load i64, i64* %handle
  %fun_call24 = call i32 @CloseHandle(i64 %handle23)
  %result25 = load <{ i32, i8* }>, <{ i32, i8* }>* %result
  ret <{ i32, i8* }> %result25

endif22:                                          ; preds = %endif13
  %size26 = load i64, i64* %size
  %fun_call27 = call i64 @gigabytes(i64 4)
  %icmp_tmp28 = icmp ule i64 %size26, %fun_call27
  call void @assert(i1 %icmp_tmp28, <{ i32, i8* }> %arr_struct_load32, <{ i32, i8* }> %arr_struct_load37)
  %size38 = load i64, i64* %size
  %int_trunc = trunc i64 %size38 to i32
  store i32 %int_trunc, i32* %size_32
  %handle39 = load i64, i64* %handle
  %buffer40 = load i8*, i8** %buffer
  %size_3241 = load i32, i32* %size_32
  %fun_call42 = call i32 @ReadFile(i64 %handle39, i8* %buffer40, i32 %size_3241, i32* %bytes_read, i8* null)
  store i32 %fun_call42, i32* %rfr
  %bytes_read43 = load i32, i32* %bytes_read
  %size_3244 = load i32, i32* %size_32
  %icmp_tmp45 = icmp eq i32 %bytes_read43, %size_3244
  call void @assert(i1 %icmp_tmp45, <{ i32, i8* }> %arr_struct_load49, <{ i32, i8* }> %arr_struct_load54)
  %rfr55 = load i32, i32* %rfr
  %icmp_tmp56 = icmp eq i32 %rfr55, 0
  br i1 %icmp_tmp56, label %then57, label %endif58

then57:                                           ; preds = %endif22
  %buffer59 = load i8*, i8** %buffer
  %fun_call60 = call i32 @VirtualFree(i8* %buffer59, i64 0, i32 32768)
  %handle61 = load i64, i64* %handle
  %fun_call62 = call i32 @CloseHandle(i64 %handle61)
  %result63 = load <{ i32, i8* }>, <{ i32, i8* }>* %result
  ret <{ i32, i8* }> %result63

endif58:                                          ; preds = %endif22
  %struct_field_ptr64 = getelementptr inbounds <{ i32, i8* }>, <{ i32, i8* }>* %result, i32 0, i32 1
  %buffer65 = load i8*, i8** %buffer
  store i8* %buffer65, i8** %struct_field_ptr64
  %struct_field_ptr66 = getelementptr inbounds <{ i32, i8* }>, <{ i32, i8* }>* %result, i32 0, i32 0
  %size_3267 = load i32, i32* %size_32
  store i32 %size_3267, i32* %struct_field_ptr66
  %handle68 = load i64, i64* %handle
  %fun_call69 = call i32 @CloseHandle(i64 %handle68)
  %result70 = load <{ i32, i8* }>, <{ i32, i8* }>* %result
  ret <{ i32, i8* }> %result70
}

; Function Attrs: nounwind
define internal void @platform_free_file_memory(i8* %mem) #0 {
vars:
  br label %entry

entry:                                            ; preds = %vars
  %fun_call = call i32 @VirtualFree(i8* %mem, i64 0, i32 32768)
  ret void
}

; Function Attrs: nounwind
define internal void @refresh_game_dll(<{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>* %game) #0 {
vars:
  %arr_struct_alloca = alloca <{ i32, i8* }>
  %arr_elem_alloca = alloca i8, i32 13
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca, i8* getelementptr inbounds ([13 x i8], [13 x i8]* @str.40, i32 0, i32 0), i32 13, i32 0, i1 false)
  %gep_arr_elem_ptr = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 0
  store i32 13, i32* %gep_arr_elem_ptr
  %gep_arr_elem_ptr1 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 1
  store i8* %arr_elem_alloca, i8** %gep_arr_elem_ptr1
  %arr_struct_load = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca
  %current_write_time = alloca i64
  br label %entry

entry:                                            ; preds = %vars
  %fun_call = call i64 @get_last_write_time(<{ i32, i8* }> %arr_struct_load)
  store i64 %fun_call, i64* %current_write_time
  %current_write_time2 = load i64, i64* %current_write_time
  %struct_field_ptr = getelementptr inbounds <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>, <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>* %game, i32 0, i32 4
  %struct_arrow = load i64, i64* %struct_field_ptr
  %icmp_tmp = icmp ne i64 %current_write_time2, %struct_arrow
  br i1 %icmp_tmp, label %then, label %endif

then:                                             ; preds = %entry
  call void @unload_game_dll(<{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>* %game)
  %fun_call3 = call <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }> @load_game_dll()
  store <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }> %fun_call3, <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>* %game
  br label %endif

endif:                                            ; preds = %then, %entry
  ret void
}

; Function Attrs: nounwind
define internal <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }> @load_game_dll() #0 {
vars:
  %arr_struct_alloca = alloca <{ i32, i8* }>
  %arr_elem_alloca = alloca i8, i32 13
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca, i8* getelementptr inbounds ([13 x i8], [13 x i8]* @str.40, i32 0, i32 0), i32 13, i32 0, i1 false)
  %gep_arr_elem_ptr = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 0
  store i32 13, i32* %gep_arr_elem_ptr
  %gep_arr_elem_ptr1 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 1
  store i8* %arr_elem_alloca, i8** %gep_arr_elem_ptr1
  %arr_struct_load = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca
  %source_str = alloca <{ i32, i8* }>
  %arr_struct_alloca2 = alloca <{ i32, i8* }>
  %arr_elem_alloca3 = alloca i8, i32 18
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca3, i8* getelementptr inbounds ([18 x i8], [18 x i8]* @str.41, i32 0, i32 0), i32 18, i32 0, i1 false)
  %gep_arr_elem_ptr4 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2, i32 0, i32 0
  store i32 18, i32* %gep_arr_elem_ptr4
  %gep_arr_elem_ptr5 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2, i32 0, i32 1
  store i8* %arr_elem_alloca3, i8** %gep_arr_elem_ptr5
  %arr_struct_load6 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2
  %temp_str = alloca <{ i32, i8* }>
  %struct_alloca = alloca <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>
  %arr_struct_alloca9 = alloca <{ i32, i8* }>
  %arr_elem_alloca10 = alloca i8, i32 100
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca10, i8* getelementptr inbounds ([101 x i8], [101 x i8]* @str.42, i32 0, i32 0), i32 100, i32 0, i1 false)
  %gep_arr_elem_ptr11 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca9, i32 0, i32 0
  store i32 100, i32* %gep_arr_elem_ptr11
  %gep_arr_elem_ptr12 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca9, i32 0, i32 1
  store i8* %arr_elem_alloca10, i8** %gep_arr_elem_ptr12
  %arr_struct_load13 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca9
  %arr_struct_alloca16 = alloca <{ i32, i8* }>
  %arr_elem_alloca17 = alloca i8, i32 100
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca17, i8* getelementptr inbounds ([101 x i8], [101 x i8]* @str.43, i32 0, i32 0), i32 100, i32 0, i1 false)
  %gep_arr_elem_ptr18 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca16, i32 0, i32 0
  store i32 100, i32* %gep_arr_elem_ptr18
  %gep_arr_elem_ptr19 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca16, i32 0, i32 1
  store i8* %arr_elem_alloca17, i8** %gep_arr_elem_ptr19
  %arr_struct_load20 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca16
  %copy_result = alloca i32
  %arr_struct_alloca24 = alloca <{ i32, i8* }>
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.44, i32 0, i32 0), i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr25 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca24, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr25
  %gep_arr_elem_ptr26 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca24, i32 0, i32 1
  store i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.44, i32 0, i32 0), i8** %gep_arr_elem_ptr26
  %arr_struct_load27 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca24
  %arr_struct_alloca28 = alloca <{ i32, i8* }>
  %arr_elem_alloca29 = alloca i8, i32 99
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca29, i8* getelementptr inbounds ([100 x i8], [100 x i8]* @str.45, i32 0, i32 0), i32 99, i32 0, i1 false)
  %gep_arr_elem_ptr30 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca28, i32 0, i32 0
  store i32 99, i32* %gep_arr_elem_ptr30
  %gep_arr_elem_ptr31 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca28, i32 0, i32 1
  store i8* %arr_elem_alloca29, i8** %gep_arr_elem_ptr31
  %arr_struct_load32 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca28
  %arr_struct_alloca35 = alloca <{ i32, i8* }>
  %arr_elem_alloca36 = alloca i8, i32 100
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca36, i8* getelementptr inbounds ([101 x i8], [101 x i8]* @str.46, i32 0, i32 0), i32 100, i32 0, i1 false)
  %gep_arr_elem_ptr37 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca35, i32 0, i32 0
  store i32 100, i32* %gep_arr_elem_ptr37
  %gep_arr_elem_ptr38 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca35, i32 0, i32 1
  store i8* %arr_elem_alloca36, i8** %gep_arr_elem_ptr38
  %arr_struct_load39 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca35
  %arr_struct_alloca44 = alloca <{ i32, i8* }>
  %arr_elem_alloca45 = alloca i8, i32 23
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca45, i8* getelementptr inbounds ([23 x i8], [23 x i8]* @str.47, i32 0, i32 0), i32 23, i32 0, i1 false)
  %gep_arr_elem_ptr46 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca44, i32 0, i32 0
  store i32 23, i32* %gep_arr_elem_ptr46
  %gep_arr_elem_ptr47 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca44, i32 0, i32 1
  store i8* %arr_elem_alloca45, i8** %gep_arr_elem_ptr47
  %arr_struct_load48 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca44
  %arr_struct_alloca49 = alloca <{ i32, i8* }>
  %arr_elem_alloca50 = alloca i8, i32 100
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca50, i8* getelementptr inbounds ([101 x i8], [101 x i8]* @str.48, i32 0, i32 0), i32 100, i32 0, i1 false)
  %gep_arr_elem_ptr51 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca49, i32 0, i32 0
  store i32 100, i32* %gep_arr_elem_ptr51
  %gep_arr_elem_ptr52 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca49, i32 0, i32 1
  store i8* %arr_elem_alloca50, i8** %gep_arr_elem_ptr52
  %arr_struct_load53 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca49
  %arr_struct_alloca60 = alloca <{ i32, i8* }>
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.49, i32 0, i32 0), i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr61 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca60, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr61
  %gep_arr_elem_ptr62 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca60, i32 0, i32 1
  store i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.49, i32 0, i32 0), i8** %gep_arr_elem_ptr62
  %arr_struct_load63 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca60
  %arr_struct_alloca64 = alloca <{ i32, i8* }>
  %arr_elem_alloca65 = alloca i8, i32 99
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca65, i8* getelementptr inbounds ([100 x i8], [100 x i8]* @str.50, i32 0, i32 0), i32 99, i32 0, i1 false)
  %gep_arr_elem_ptr66 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca64, i32 0, i32 0
  store i32 99, i32* %gep_arr_elem_ptr66
  %gep_arr_elem_ptr67 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca64, i32 0, i32 1
  store i8* %arr_elem_alloca65, i8** %gep_arr_elem_ptr67
  %arr_struct_load68 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca64
  %arr_struct_alloca72 = alloca <{ i32, i8* }>
  %arr_elem_alloca73 = alloca i8, i32 18
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca73, i8* getelementptr inbounds ([18 x i8], [18 x i8]* @str.51, i32 0, i32 0), i32 18, i32 0, i1 false)
  %gep_arr_elem_ptr74 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca72, i32 0, i32 0
  store i32 18, i32* %gep_arr_elem_ptr74
  %gep_arr_elem_ptr75 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca72, i32 0, i32 1
  store i8* %arr_elem_alloca73, i8** %gep_arr_elem_ptr75
  %arr_struct_load76 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca72
  %arr_struct_alloca77 = alloca <{ i32, i8* }>
  %arr_elem_alloca78 = alloca i8, i32 100
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca78, i8* getelementptr inbounds ([101 x i8], [101 x i8]* @str.52, i32 0, i32 0), i32 100, i32 0, i1 false)
  %gep_arr_elem_ptr79 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca77, i32 0, i32 0
  store i32 100, i32* %gep_arr_elem_ptr79
  %gep_arr_elem_ptr80 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca77, i32 0, i32 1
  store i8* %arr_elem_alloca78, i8** %gep_arr_elem_ptr80
  %arr_struct_load81 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca77
  %arr_struct_alloca90 = alloca <{ i32, i8* }>
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.53, i32 0, i32 0), i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr91 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca90, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr91
  %gep_arr_elem_ptr92 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca90, i32 0, i32 1
  store i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.53, i32 0, i32 0), i8** %gep_arr_elem_ptr92
  %arr_struct_load93 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca90
  %arr_struct_alloca94 = alloca <{ i32, i8* }>
  %arr_elem_alloca95 = alloca i8, i32 99
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca95, i8* getelementptr inbounds ([100 x i8], [100 x i8]* @str.54, i32 0, i32 0), i32 99, i32 0, i1 false)
  %gep_arr_elem_ptr96 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca94, i32 0, i32 0
  store i32 99, i32* %gep_arr_elem_ptr96
  %gep_arr_elem_ptr97 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca94, i32 0, i32 1
  store i8* %arr_elem_alloca95, i8** %gep_arr_elem_ptr97
  %arr_struct_load98 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca94
  %arr_struct_alloca100 = alloca <{ i32, i8* }>
  %arr_elem_alloca101 = alloca i8, i32 13
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca101, i8* getelementptr inbounds ([14 x i8], [14 x i8]* @str.55, i32 0, i32 0), i32 13, i32 0, i1 false)
  %gep_arr_elem_ptr102 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca100, i32 0, i32 0
  store i32 13, i32* %gep_arr_elem_ptr102
  %gep_arr_elem_ptr103 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca100, i32 0, i32 1
  store i8* %arr_elem_alloca101, i8** %gep_arr_elem_ptr103
  %arr_struct_load104 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca100
  br label %entry

entry:                                            ; preds = %vars
  store <{ i32, i8* }> %arr_struct_load, <{ i32, i8* }>* %source_str
  store <{ i32, i8* }> %arr_struct_load6, <{ i32, i8* }>* %temp_str
  %struct_arg_0 = getelementptr inbounds <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>, <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>* %struct_alloca, i32 0, i32 0
  store i64 0, i64* %struct_arg_0
  %struct_arg_1 = getelementptr inbounds <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>, <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>* %struct_alloca, i32 0, i32 1
  store i1 (i8*, i8*, i8*)* null, i1 (i8*, i8*, i8*)** %struct_arg_1
  %struct_arg_2 = getelementptr inbounds <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>, <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>* %struct_alloca, i32 0, i32 2
  store void (i8*, i8*)* null, void (i8*, i8*)** %struct_arg_2
  %struct_arg_3 = getelementptr inbounds <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>, <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>* %struct_alloca, i32 0, i32 3
  store i1 false, i1* %struct_arg_3
  %struct_arg_4 = getelementptr inbounds <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>, <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>* %struct_alloca, i32 0, i32 4
  store i64 0, i64* %struct_arg_4
  %struct_field_ptr = getelementptr inbounds <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>, <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>* %struct_alloca, i32 0, i32 4
  %source_str7 = load <{ i32, i8* }>, <{ i32, i8* }>* %source_str
  %fun_call = call i64 @get_last_write_time(<{ i32, i8* }> %source_str7)
  store i64 %fun_call, i64* %struct_field_ptr
  %source_str8 = load <{ i32, i8* }>, <{ i32, i8* }>* %source_str
  %fun_call14 = call i8* @cstr(<{ i32, i8* }> %source_str8, <{ i32, i8* }> %arr_struct_load13)
  %temp_str15 = load <{ i32, i8* }>, <{ i32, i8* }>* %temp_str
  %fun_call21 = call i8* @cstr(<{ i32, i8* }> %temp_str15, <{ i32, i8* }> %arr_struct_load20)
  %fun_call22 = call i32 @CopyFileA(i8* %fun_call14, i8* %fun_call21, i32 0)
  store i32 %fun_call22, i32* %copy_result
  %copy_result23 = load i32, i32* %copy_result
  %icmp_tmp = icmp ne i32 %copy_result23, 0
  call void @assert(i1 %icmp_tmp, <{ i32, i8* }> %arr_struct_load27, <{ i32, i8* }> %arr_struct_load32)
  %struct_field_ptr33 = getelementptr inbounds <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>, <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>* %struct_alloca, i32 0, i32 0
  %temp_str34 = load <{ i32, i8* }>, <{ i32, i8* }>* %temp_str
  %fun_call40 = call i8* @cstr(<{ i32, i8* }> %temp_str34, <{ i32, i8* }> %arr_struct_load39)
  %fun_call41 = call i64 @LoadLibraryA(i8* %fun_call40)
  store i64 %fun_call41, i64* %struct_field_ptr33
  %struct_field_ptr42 = getelementptr inbounds <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>, <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>* %struct_alloca, i32 0, i32 1
  %struct_field_ptr43 = getelementptr inbounds <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>, <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>* %struct_alloca, i32 0, i32 0
  %struct_field = load i64, i64* %struct_field_ptr43
  %fun_call54 = call i8* @cstr(<{ i32, i8* }> %arr_struct_load48, <{ i32, i8* }> %arr_struct_load53)
  %fun_call55 = call i8* @GetProcAddress(i64 %struct_field, i8* %fun_call54)
  %pointer_bit_cast = bitcast i8* %fun_call55 to i1 (<{ i8*, i64, i8*, i64, i8* }>*, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>*, <{ i8*, i32, i32, i32 }>*)*
  %hmpf = bitcast i1 (<{ i8*, i64, i8*, i64, i8* }>*, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>*, <{ i8*, i32, i32, i32 }>*)* %pointer_bit_cast to i1 (i8*, i8*, i8*)*
  store i1 (i8*, i8*, i8*)* %hmpf, i1 (i8*, i8*, i8*)** %struct_field_ptr42
  %struct_field_ptr56 = getelementptr inbounds <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>, <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>* %struct_alloca, i32 0, i32 1
  %struct_field57 = load i1 (i8*, i8*, i8*)*, i1 (i8*, i8*, i8*)** %struct_field_ptr56
  %pointer_bit_cast58 = bitcast i1 (i8*, i8*, i8*)* %struct_field57 to i8*
  %icmp_tmp59 = icmp ne i8* %pointer_bit_cast58, null
  call void @assert(i1 %icmp_tmp59, <{ i32, i8* }> %arr_struct_load63, <{ i32, i8* }> %arr_struct_load68)
  %struct_field_ptr69 = getelementptr inbounds <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>, <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>* %struct_alloca, i32 0, i32 2
  %struct_field_ptr70 = getelementptr inbounds <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>, <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>* %struct_alloca, i32 0, i32 0
  %struct_field71 = load i64, i64* %struct_field_ptr70
  %fun_call82 = call i8* @cstr(<{ i32, i8* }> %arr_struct_load76, <{ i32, i8* }> %arr_struct_load81)
  %fun_call83 = call i8* @GetProcAddress(i64 %struct_field71, i8* %fun_call82)
  %pointer_bit_cast84 = bitcast i8* %fun_call83 to void (<{ i8*, i64, i8*, i64, i8* }>*, <{ i16*, i32, i32, float }>*)*
  %hmpf85 = bitcast void (<{ i8*, i64, i8*, i64, i8* }>*, <{ i16*, i32, i32, float }>*)* %pointer_bit_cast84 to void (i8*, i8*)*
  store void (i8*, i8*)* %hmpf85, void (i8*, i8*)** %struct_field_ptr69
  %struct_field_ptr86 = getelementptr inbounds <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>, <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>* %struct_alloca, i32 0, i32 2
  %struct_field87 = load void (i8*, i8*)*, void (i8*, i8*)** %struct_field_ptr86
  %pointer_bit_cast88 = bitcast void (i8*, i8*)* %struct_field87 to i8*
  %icmp_tmp89 = icmp ne i8* %pointer_bit_cast88, null
  call void @assert(i1 %icmp_tmp89, <{ i32, i8* }> %arr_struct_load93, <{ i32, i8* }> %arr_struct_load98)
  %struct_field_ptr99 = getelementptr inbounds <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>, <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>* %struct_alloca, i32 0, i32 3
  store i1 true, i1* %struct_field_ptr99
  call void @print_string(<{ i32, i8* }> %arr_struct_load104)
  %result = load <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>, <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>* %struct_alloca
  ret <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }> %result
}

; Function Attrs: nounwind
define internal void @unload_game_dll(<{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>* %interface) #0 {
vars:
  %result = alloca i32
  %arr_struct_alloca = alloca <{ i32, i8* }>
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.56, i32 0, i32 0), i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr
  %gep_arr_elem_ptr5 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 1
  store i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.56, i32 0, i32 0), i8** %gep_arr_elem_ptr5
  %arr_struct_load = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca
  %arr_struct_alloca6 = alloca <{ i32, i8* }>
  %arr_elem_alloca = alloca i8, i32 100
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca, i8* getelementptr inbounds ([101 x i8], [101 x i8]* @str.57, i32 0, i32 0), i32 100, i32 0, i1 false)
  %gep_arr_elem_ptr7 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca6, i32 0, i32 0
  store i32 100, i32* %gep_arr_elem_ptr7
  %gep_arr_elem_ptr8 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca6, i32 0, i32 1
  store i8* %arr_elem_alloca, i8** %gep_arr_elem_ptr8
  %arr_struct_load9 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca6
  %arr_struct_alloca14 = alloca <{ i32, i8* }>
  %arr_elem_alloca15 = alloca i8, i32 15
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca15, i8* getelementptr inbounds ([16 x i8], [16 x i8]* @str.58, i32 0, i32 0), i32 15, i32 0, i1 false)
  %gep_arr_elem_ptr16 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca14, i32 0, i32 0
  store i32 15, i32* %gep_arr_elem_ptr16
  %gep_arr_elem_ptr17 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca14, i32 0, i32 1
  store i8* %arr_elem_alloca15, i8** %gep_arr_elem_ptr17
  %arr_struct_load18 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca14
  br label %entry

entry:                                            ; preds = %vars
  %struct_field_ptr = getelementptr inbounds <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>, <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>* %interface, i32 0, i32 0
  %struct_arrow = load i64, i64* %struct_field_ptr
  %icmp_tmp = icmp ne i64 %struct_arrow, 0
  br i1 %icmp_tmp, label %then, label %endif

then:                                             ; preds = %entry
  %struct_field_ptr1 = getelementptr inbounds <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>, <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>* %interface, i32 0, i32 0
  %struct_arrow2 = load i64, i64* %struct_field_ptr1
  %fun_call = call i32 @FreeLibrary(i64 %struct_arrow2)
  store i32 %fun_call, i32* %result
  %result3 = load i32, i32* %result
  %icmp_tmp4 = icmp ne i32 %result3, 0
  call void @assert(i1 %icmp_tmp4, <{ i32, i8* }> %arr_struct_load, <{ i32, i8* }> %arr_struct_load9)
  br label %endif

endif:                                            ; preds = %then, %entry
  %struct_field_ptr10 = getelementptr inbounds <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>, <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>* %interface, i32 0, i32 1
  store i1 (i8*, i8*, i8*)* null, i1 (i8*, i8*, i8*)** %struct_field_ptr10
  %struct_field_ptr11 = getelementptr inbounds <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>, <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>* %interface, i32 0, i32 2
  store void (i8*, i8*)* null, void (i8*, i8*)** %struct_field_ptr11
  %struct_field_ptr12 = getelementptr inbounds <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>, <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>* %interface, i32 0, i32 3
  store i1 false, i1* %struct_field_ptr12
  %struct_field_ptr13 = getelementptr inbounds <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>, <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>* %interface, i32 0, i32 0
  store i64 0, i64* %struct_field_ptr13
  call void @print_string(<{ i32, i8* }> %arr_struct_load18)
  ret void
}

; Function Attrs: nounwind
define internal i64 @get_last_write_time(<{ i32, i8* }> %filename) #0 {
vars:
  %arr_struct_alloca = alloca <{ i32, i8* }>
  %arr_elem_alloca = alloca i8, i32 100
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca, i8* getelementptr inbounds ([101 x i8], [101 x i8]* @str.59, i32 0, i32 0), i32 100, i32 0, i1 false)
  %gep_arr_elem_ptr = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 0
  store i32 100, i32* %gep_arr_elem_ptr
  %gep_arr_elem_ptr1 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 1
  store i8* %arr_elem_alloca, i8** %gep_arr_elem_ptr1
  %arr_struct_load = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca
  %cs = alloca i8*
  %data = alloca <{ i32, i64, i64, i64, i64 }>
  %arr_struct_alloca2 = alloca <{ i32, i8* }>
  %arr_elem_alloca3 = alloca i8, i32 13
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca3, i8* getelementptr inbounds ([13 x i8], [13 x i8]* @str.40, i32 0, i32 0), i32 13, i32 0, i1 false)
  %gep_arr_elem_ptr4 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2, i32 0, i32 0
  store i32 13, i32* %gep_arr_elem_ptr4
  %gep_arr_elem_ptr5 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2, i32 0, i32 1
  store i8* %arr_elem_alloca3, i8** %gep_arr_elem_ptr5
  %arr_struct_load6 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2
  %arr_struct_alloca7 = alloca <{ i32, i8* }>
  %arr_elem_alloca8 = alloca i8, i32 100
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca8, i8* getelementptr inbounds ([101 x i8], [101 x i8]* @str.60, i32 0, i32 0), i32 100, i32 0, i1 false)
  %gep_arr_elem_ptr9 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca7, i32 0, i32 0
  store i32 100, i32* %gep_arr_elem_ptr9
  %gep_arr_elem_ptr10 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca7, i32 0, i32 1
  store i8* %arr_elem_alloca8, i8** %gep_arr_elem_ptr10
  %arr_struct_load11 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca7
  br label %entry

entry:                                            ; preds = %vars
  %fun_call = call i8* @cstr(<{ i32, i8* }> %filename, <{ i32, i8* }> %arr_struct_load)
  store i8* %fun_call, i8** %cs
  %fun_call12 = call i8* @cstr(<{ i32, i8* }> %arr_struct_load6, <{ i32, i8* }> %arr_struct_load11)
  %fun_call13 = call i32 @GetFileAttributesExA(i8* %fun_call12, i32 0, <{ i32, i64, i64, i64, i64 }>* %data)
  %struct_field_ptr = getelementptr inbounds <{ i32, i64, i64, i64, i64 }>, <{ i32, i64, i64, i64, i64 }>* %data, i32 0, i32 3
  %struct_field = load i64, i64* %struct_field_ptr
  ret i64 %struct_field
}

; Function Attrs: nounwind
define internal <{ i8* }>* @init_direct_sound(i64 %hwnd, i32 %buffer_size, i32 %samples_per_second) #0 {
vars:
  %arr_struct_alloca = alloca <{ i32, i8* }>
  %arr_elem_alloca = alloca i8, i32 11
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca, i8* getelementptr inbounds ([11 x i8], [11 x i8]* @str.61, i32 0, i32 0), i32 11, i32 0, i1 false)
  %gep_arr_elem_ptr = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 0
  store i32 11, i32* %gep_arr_elem_ptr
  %gep_arr_elem_ptr1 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 1
  store i8* %arr_elem_alloca, i8** %gep_arr_elem_ptr1
  %arr_struct_load = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca
  %arr_struct_alloca2 = alloca <{ i32, i8* }>
  %arr_elem_alloca3 = alloca i8, i32 100
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca3, i8* getelementptr inbounds ([101 x i8], [101 x i8]* @str.62, i32 0, i32 0), i32 100, i32 0, i1 false)
  %gep_arr_elem_ptr4 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2, i32 0, i32 0
  store i32 100, i32* %gep_arr_elem_ptr4
  %gep_arr_elem_ptr5 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2, i32 0, i32 1
  store i8* %arr_elem_alloca3, i8** %gep_arr_elem_ptr5
  %arr_struct_load6 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2
  %dsound_library = alloca i64
  %arr_struct_alloca9 = alloca <{ i32, i8* }>
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.63, i32 0, i32 0), i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr10 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca9, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr10
  %gep_arr_elem_ptr11 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca9, i32 0, i32 1
  store i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.63, i32 0, i32 0), i8** %gep_arr_elem_ptr11
  %arr_struct_load12 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca9
  %arr_struct_alloca13 = alloca <{ i32, i8* }>
  %arr_elem_alloca14 = alloca i8, i32 99
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca14, i8* getelementptr inbounds ([100 x i8], [100 x i8]* @str.64, i32 0, i32 0), i32 99, i32 0, i1 false)
  %gep_arr_elem_ptr15 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca13, i32 0, i32 0
  store i32 99, i32* %gep_arr_elem_ptr15
  %gep_arr_elem_ptr16 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca13, i32 0, i32 1
  store i8* %arr_elem_alloca14, i8** %gep_arr_elem_ptr16
  %arr_struct_load17 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca13
  %DirectSoundCreate = alloca i32 (i8*, <{ i8* }>**, i8*)*
  %arr_struct_alloca19 = alloca <{ i32, i8* }>
  %arr_elem_alloca20 = alloca i8, i32 18
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca20, i8* getelementptr inbounds ([18 x i8], [18 x i8]* @str.65, i32 0, i32 0), i32 18, i32 0, i1 false)
  %gep_arr_elem_ptr21 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca19, i32 0, i32 0
  store i32 18, i32* %gep_arr_elem_ptr21
  %gep_arr_elem_ptr22 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca19, i32 0, i32 1
  store i8* %arr_elem_alloca20, i8** %gep_arr_elem_ptr22
  %arr_struct_load23 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca19
  %arr_struct_alloca24 = alloca <{ i32, i8* }>
  %arr_elem_alloca25 = alloca i8, i32 100
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca25, i8* getelementptr inbounds ([101 x i8], [101 x i8]* @str.66, i32 0, i32 0), i32 100, i32 0, i1 false)
  %gep_arr_elem_ptr26 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca24, i32 0, i32 0
  store i32 100, i32* %gep_arr_elem_ptr26
  %gep_arr_elem_ptr27 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca24, i32 0, i32 1
  store i8* %arr_elem_alloca25, i8** %gep_arr_elem_ptr27
  %arr_struct_load28 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca24
  %arr_struct_alloca34 = alloca <{ i32, i8* }>
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.67, i32 0, i32 0), i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr35 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca34, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr35
  %gep_arr_elem_ptr36 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca34, i32 0, i32 1
  store i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.67, i32 0, i32 0), i8** %gep_arr_elem_ptr36
  %arr_struct_load37 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca34
  %arr_struct_alloca38 = alloca <{ i32, i8* }>
  %arr_elem_alloca39 = alloca i8, i32 99
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca39, i8* getelementptr inbounds ([100 x i8], [100 x i8]* @str.68, i32 0, i32 0), i32 99, i32 0, i1 false)
  %gep_arr_elem_ptr40 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca38, i32 0, i32 0
  store i32 99, i32* %gep_arr_elem_ptr40
  %gep_arr_elem_ptr41 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca38, i32 0, i32 1
  store i8* %arr_elem_alloca39, i8** %gep_arr_elem_ptr41
  %arr_struct_load42 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca38
  %dsound_obj = alloca <{ i8* }>*
  %error = alloca i32
  %arr_struct_alloca46 = alloca <{ i32, i8* }>
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.69, i32 0, i32 0), i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr47 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca46, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr47
  %gep_arr_elem_ptr48 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca46, i32 0, i32 1
  store i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.69, i32 0, i32 0), i8** %gep_arr_elem_ptr48
  %arr_struct_load49 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca46
  %arr_struct_alloca50 = alloca <{ i32, i8* }>
  %arr_elem_alloca51 = alloca i8, i32 99
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca51, i8* getelementptr inbounds ([100 x i8], [100 x i8]* @str.70, i32 0, i32 0), i32 99, i32 0, i1 false)
  %gep_arr_elem_ptr52 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca50, i32 0, i32 0
  store i32 99, i32* %gep_arr_elem_ptr52
  %gep_arr_elem_ptr53 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca50, i32 0, i32 1
  store i8* %arr_elem_alloca51, i8** %gep_arr_elem_ptr53
  %arr_struct_load54 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca50
  %arr_struct_alloca62 = alloca <{ i32, i8* }>
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.71, i32 0, i32 0), i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr63 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca62, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr63
  %gep_arr_elem_ptr64 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca62, i32 0, i32 1
  store i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.71, i32 0, i32 0), i8** %gep_arr_elem_ptr64
  %arr_struct_load65 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca62
  %arr_struct_alloca66 = alloca <{ i32, i8* }>
  %arr_elem_alloca67 = alloca i8, i32 99
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca67, i8* getelementptr inbounds ([100 x i8], [100 x i8]* @str.72, i32 0, i32 0), i32 99, i32 0, i1 false)
  %gep_arr_elem_ptr68 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca66, i32 0, i32 0
  store i32 99, i32* %gep_arr_elem_ptr68
  %gep_arr_elem_ptr69 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca66, i32 0, i32 1
  store i8* %arr_elem_alloca67, i8** %gep_arr_elem_ptr69
  %arr_struct_load70 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca66
  %struct_alloca = alloca <{ i32, i32, i32, i32, i8* }>
  %primary_buffer = alloca <{ i8* }>*
  %arr_struct_alloca89 = alloca <{ i32, i8* }>
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.73, i32 0, i32 0), i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr90 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca89, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr90
  %gep_arr_elem_ptr91 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca89, i32 0, i32 1
  store i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.73, i32 0, i32 0), i8** %gep_arr_elem_ptr91
  %arr_struct_load92 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca89
  %arr_struct_alloca93 = alloca <{ i32, i8* }>
  %arr_elem_alloca94 = alloca i8, i32 99
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca94, i8* getelementptr inbounds ([100 x i8], [100 x i8]* @str.74, i32 0, i32 0), i32 99, i32 0, i1 false)
  %gep_arr_elem_ptr95 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca93, i32 0, i32 0
  store i32 99, i32* %gep_arr_elem_ptr95
  %gep_arr_elem_ptr96 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca93, i32 0, i32 1
  store i8* %arr_elem_alloca94, i8** %gep_arr_elem_ptr96
  %arr_struct_load97 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca93
  %struct_alloca98 = alloca <{ i16, i16, i32, i32, i16, i16, i16 }>
  %arr_struct_alloca131 = alloca <{ i32, i8* }>
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.75, i32 0, i32 0), i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr132 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca131, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr132
  %gep_arr_elem_ptr133 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca131, i32 0, i32 1
  store i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.75, i32 0, i32 0), i8** %gep_arr_elem_ptr133
  %arr_struct_load134 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca131
  %arr_struct_alloca135 = alloca <{ i32, i8* }>
  %arr_elem_alloca136 = alloca i8, i32 99
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca136, i8* getelementptr inbounds ([100 x i8], [100 x i8]* @str.76, i32 0, i32 0), i32 99, i32 0, i1 false)
  %gep_arr_elem_ptr137 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca135, i32 0, i32 0
  store i32 99, i32* %gep_arr_elem_ptr137
  %gep_arr_elem_ptr138 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca135, i32 0, i32 1
  store i8* %arr_elem_alloca136, i8** %gep_arr_elem_ptr138
  %arr_struct_load139 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca135
  %struct_alloca140 = alloca <{ i32, i32, i32, i32, i8* }>
  %secondary_buffer = alloca <{ i8* }>*
  %arr_struct_alloca164 = alloca <{ i32, i8* }>
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.77, i32 0, i32 0), i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr165 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca164, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr165
  %gep_arr_elem_ptr166 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca164, i32 0, i32 1
  store i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.77, i32 0, i32 0), i8** %gep_arr_elem_ptr166
  %arr_struct_load167 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca164
  %arr_struct_alloca168 = alloca <{ i32, i8* }>
  %arr_elem_alloca169 = alloca i8, i32 99
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca169, i8* getelementptr inbounds ([100 x i8], [100 x i8]* @str.78, i32 0, i32 0), i32 99, i32 0, i1 false)
  %gep_arr_elem_ptr170 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca168, i32 0, i32 0
  store i32 99, i32* %gep_arr_elem_ptr170
  %gep_arr_elem_ptr171 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca168, i32 0, i32 1
  store i8* %arr_elem_alloca169, i8** %gep_arr_elem_ptr171
  %arr_struct_load172 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca168
  br label %entry

entry:                                            ; preds = %vars
  %fun_call = call i8* @cstr(<{ i32, i8* }> %arr_struct_load, <{ i32, i8* }> %arr_struct_load6)
  %fun_call7 = call i64 @LoadLibraryA(i8* %fun_call)
  store i64 %fun_call7, i64* %dsound_library
  %dsound_library8 = load i64, i64* %dsound_library
  %icmp_tmp = icmp ne i64 %dsound_library8, 0
  call void @assert(i1 %icmp_tmp, <{ i32, i8* }> %arr_struct_load12, <{ i32, i8* }> %arr_struct_load17)
  %pointer_bit_cast = bitcast i32 (i8*, <{ i8* }>**, i8*)** %DirectSoundCreate to i8**
  %dsound_library18 = load i64, i64* %dsound_library
  %fun_call29 = call i8* @cstr(<{ i32, i8* }> %arr_struct_load23, <{ i32, i8* }> %arr_struct_load28)
  %fun_call30 = call i8* @GetProcAddress(i64 %dsound_library18, i8* %fun_call29)
  store i8* %fun_call30, i8** %pointer_bit_cast
  %DirectSoundCreate31 = load i32 (i8*, <{ i8* }>**, i8*)*, i32 (i8*, <{ i8* }>**, i8*)** %DirectSoundCreate
  %pointer_bit_cast32 = bitcast i32 (i8*, <{ i8* }>**, i8*)* %DirectSoundCreate31 to i8*
  %icmp_tmp33 = icmp ne i8* %pointer_bit_cast32, null
  call void @assert(i1 %icmp_tmp33, <{ i32, i8* }> %arr_struct_load37, <{ i32, i8* }> %arr_struct_load42)
  %fun_ptr_load = load i32 (i8*, <{ i8* }>**, i8*)*, i32 (i8*, <{ i8* }>**, i8*)** %DirectSoundCreate
  %fun_call43 = call i32 %fun_ptr_load(i8* null, <{ i8* }>** %dsound_obj, i8* null)
  store i32 %fun_call43, i32* %error
  %error44 = load i32, i32* %error
  %icmp_tmp45 = icmp eq i32 %error44, 0
  call void @assert(i1 %icmp_tmp45, <{ i32, i8* }> %arr_struct_load49, <{ i32, i8* }> %arr_struct_load54)
  %struct_arrow_load = load <{ i8* }>*, <{ i8* }>** %dsound_obj
  %struct_field_ptr = getelementptr inbounds <{ i8* }>, <{ i8* }>* %struct_arrow_load, i32 0, i32 0
  %struct_arrow_load55 = load i8*, i8** %struct_field_ptr
  %hack_bitcast = bitcast i8* %struct_arrow_load55 to <{ i8*, i8*, i8*, i32 (i8*, i8*, i8**, i8*)*, i8*, i8*, i32 (i8*, i64, i32)*, i8*, i8*, i8*, i8* }>*
  %struct_field_ptr56 = getelementptr inbounds <{ i8*, i8*, i8*, i32 (i8*, i8*, i8**, i8*)*, i8*, i8*, i32 (i8*, i64, i32)*, i8*, i8*, i8*, i8* }>, <{ i8*, i8*, i8*, i32 (i8*, i8*, i8**, i8*)*, i8*, i8*, i32 (i8*, i64, i32)*, i8*, i8*, i8*, i8* }>* %hack_bitcast, i32 0, i32 6
  %fun_ptr_load57 = load i32 (i8*, i64, i32)*, i32 (i8*, i64, i32)** %struct_field_ptr56
  %dsound_obj58 = load <{ i8* }>*, <{ i8* }>** %dsound_obj
  %fun_param_hack = bitcast <{ i8* }>* %dsound_obj58 to i8*
  %fun_call59 = call i32 %fun_ptr_load57(i8* %fun_param_hack, i64 %hwnd, i32 2)
  store i32 %fun_call59, i32* %error
  %error60 = load i32, i32* %error
  %icmp_tmp61 = icmp eq i32 %error60, 0
  call void @assert(i1 %icmp_tmp61, <{ i32, i8* }> %arr_struct_load65, <{ i32, i8* }> %arr_struct_load70)
  %struct_arg_0 = getelementptr inbounds <{ i32, i32, i32, i32, i8* }>, <{ i32, i32, i32, i32, i8* }>* %struct_alloca, i32 0, i32 0
  store i32 0, i32* %struct_arg_0
  %struct_arg_1 = getelementptr inbounds <{ i32, i32, i32, i32, i8* }>, <{ i32, i32, i32, i32, i8* }>* %struct_alloca, i32 0, i32 1
  store i32 0, i32* %struct_arg_1
  %struct_arg_2 = getelementptr inbounds <{ i32, i32, i32, i32, i8* }>, <{ i32, i32, i32, i32, i8* }>* %struct_alloca, i32 0, i32 2
  store i32 0, i32* %struct_arg_2
  %struct_arg_3 = getelementptr inbounds <{ i32, i32, i32, i32, i8* }>, <{ i32, i32, i32, i32, i8* }>* %struct_alloca, i32 0, i32 3
  store i32 0, i32* %struct_arg_3
  %struct_arg_4 = getelementptr inbounds <{ i32, i32, i32, i32, i8* }>, <{ i32, i32, i32, i32, i8* }>* %struct_alloca, i32 0, i32 4
  store i8* null, i8** %struct_arg_4
  %struct_field_ptr71 = getelementptr inbounds <{ i32, i32, i32, i32, i8* }>, <{ i32, i32, i32, i32, i8* }>* %struct_alloca, i32 0, i32 1
  store i32 1, i32* %struct_field_ptr71
  %struct_field_ptr72 = getelementptr inbounds <{ i32, i32, i32, i32, i8* }>, <{ i32, i32, i32, i32, i8* }>* %struct_alloca, i32 0, i32 0
  store i32 ptrtoint (<{ i32, i32, i32, i32, i8* }>* getelementptr (<{ i32, i32, i32, i32, i8* }>, <{ i32, i32, i32, i32, i8* }>* null, i32 1) to i32), i32* %struct_field_ptr72
  %struct_field_ptr73 = getelementptr inbounds <{ i32, i32, i32, i32, i8* }>, <{ i32, i32, i32, i32, i8* }>* %struct_alloca, i32 0, i32 2
  store i32 0, i32* %struct_field_ptr73
  %struct_field_ptr74 = getelementptr inbounds <{ i32, i32, i32, i32, i8* }>, <{ i32, i32, i32, i32, i8* }>* %struct_alloca, i32 0, i32 3
  store i32 0, i32* %struct_field_ptr74
  %struct_field_ptr75 = getelementptr inbounds <{ i32, i32, i32, i32, i8* }>, <{ i32, i32, i32, i32, i8* }>* %struct_alloca, i32 0, i32 4
  store i8* null, i8** %struct_field_ptr75
  %struct_arrow_load76 = load <{ i8* }>*, <{ i8* }>** %dsound_obj
  %struct_field_ptr77 = getelementptr inbounds <{ i8* }>, <{ i8* }>* %struct_arrow_load76, i32 0, i32 0
  %struct_arrow_load78 = load i8*, i8** %struct_field_ptr77
  %hack_bitcast79 = bitcast i8* %struct_arrow_load78 to <{ i8*, i8*, i8*, i32 (i8*, i8*, i8**, i8*)*, i8*, i8*, i32 (i8*, i64, i32)*, i8*, i8*, i8*, i8* }>*
  %struct_field_ptr80 = getelementptr inbounds <{ i8*, i8*, i8*, i32 (i8*, i8*, i8**, i8*)*, i8*, i8*, i32 (i8*, i64, i32)*, i8*, i8*, i8*, i8* }>, <{ i8*, i8*, i8*, i32 (i8*, i8*, i8**, i8*)*, i8*, i8*, i32 (i8*, i64, i32)*, i8*, i8*, i8*, i8* }>* %hack_bitcast79, i32 0, i32 3
  %fun_ptr_load81 = load i32 (i8*, i8*, i8**, i8*)*, i32 (i8*, i8*, i8**, i8*)** %struct_field_ptr80
  %dsound_obj82 = load <{ i8* }>*, <{ i8* }>** %dsound_obj
  %fun_param_hack83 = bitcast <{ i8* }>* %dsound_obj82 to i8*
  %fun_param_hack84 = bitcast <{ i32, i32, i32, i32, i8* }>* %struct_alloca to i8*
  %fun_param_hack85 = bitcast <{ i8* }>** %primary_buffer to i8**
  %fun_call86 = call i32 %fun_ptr_load81(i8* %fun_param_hack83, i8* %fun_param_hack84, i8** %fun_param_hack85, i8* null)
  store i32 %fun_call86, i32* %error
  %error87 = load i32, i32* %error
  %icmp_tmp88 = icmp eq i32 %error87, 0
  call void @assert(i1 %icmp_tmp88, <{ i32, i8* }> %arr_struct_load92, <{ i32, i8* }> %arr_struct_load97)
  %struct_arg_099 = getelementptr inbounds <{ i16, i16, i32, i32, i16, i16, i16 }>, <{ i16, i16, i32, i32, i16, i16, i16 }>* %struct_alloca98, i32 0, i32 0
  store i16 0, i16* %struct_arg_099
  %struct_arg_1100 = getelementptr inbounds <{ i16, i16, i32, i32, i16, i16, i16 }>, <{ i16, i16, i32, i32, i16, i16, i16 }>* %struct_alloca98, i32 0, i32 1
  store i16 0, i16* %struct_arg_1100
  %struct_arg_2101 = getelementptr inbounds <{ i16, i16, i32, i32, i16, i16, i16 }>, <{ i16, i16, i32, i32, i16, i16, i16 }>* %struct_alloca98, i32 0, i32 2
  store i32 0, i32* %struct_arg_2101
  %struct_arg_3102 = getelementptr inbounds <{ i16, i16, i32, i32, i16, i16, i16 }>, <{ i16, i16, i32, i32, i16, i16, i16 }>* %struct_alloca98, i32 0, i32 3
  store i32 0, i32* %struct_arg_3102
  %struct_arg_4103 = getelementptr inbounds <{ i16, i16, i32, i32, i16, i16, i16 }>, <{ i16, i16, i32, i32, i16, i16, i16 }>* %struct_alloca98, i32 0, i32 4
  store i16 0, i16* %struct_arg_4103
  %struct_arg_5 = getelementptr inbounds <{ i16, i16, i32, i32, i16, i16, i16 }>, <{ i16, i16, i32, i32, i16, i16, i16 }>* %struct_alloca98, i32 0, i32 5
  store i16 0, i16* %struct_arg_5
  %struct_arg_6 = getelementptr inbounds <{ i16, i16, i32, i32, i16, i16, i16 }>, <{ i16, i16, i32, i32, i16, i16, i16 }>* %struct_alloca98, i32 0, i32 6
  store i16 0, i16* %struct_arg_6
  %struct_field_ptr104 = getelementptr inbounds <{ i16, i16, i32, i32, i16, i16, i16 }>, <{ i16, i16, i32, i32, i16, i16, i16 }>* %struct_alloca98, i32 0, i32 0
  store i16 1, i16* %struct_field_ptr104
  %struct_field_ptr105 = getelementptr inbounds <{ i16, i16, i32, i32, i16, i16, i16 }>, <{ i16, i16, i32, i32, i16, i16, i16 }>* %struct_alloca98, i32 0, i32 1
  store i16 2, i16* %struct_field_ptr105
  %struct_field_ptr106 = getelementptr inbounds <{ i16, i16, i32, i32, i16, i16, i16 }>, <{ i16, i16, i32, i32, i16, i16, i16 }>* %struct_alloca98, i32 0, i32 5
  store i16 16, i16* %struct_field_ptr106
  %struct_field_ptr107 = getelementptr inbounds <{ i16, i16, i32, i32, i16, i16, i16 }>, <{ i16, i16, i32, i32, i16, i16, i16 }>* %struct_alloca98, i32 0, i32 2
  store i32 %samples_per_second, i32* %struct_field_ptr107
  %struct_field_ptr108 = getelementptr inbounds <{ i16, i16, i32, i32, i16, i16, i16 }>, <{ i16, i16, i32, i32, i16, i16, i16 }>* %struct_alloca98, i32 0, i32 4
  %struct_field_ptr109 = getelementptr inbounds <{ i16, i16, i32, i32, i16, i16, i16 }>, <{ i16, i16, i32, i32, i16, i16, i16 }>* %struct_alloca98, i32 0, i32 1
  %struct_field = load i16, i16* %struct_field_ptr109
  %struct_field_ptr110 = getelementptr inbounds <{ i16, i16, i32, i32, i16, i16, i16 }>, <{ i16, i16, i32, i32, i16, i16, i16 }>* %struct_alloca98, i32 0, i32 5
  %struct_field111 = load i16, i16* %struct_field_ptr110
  %mul_tmp = mul i16 %struct_field, %struct_field111
  %div_tmp = sdiv i16 %mul_tmp, 8
  store i16 %div_tmp, i16* %struct_field_ptr108
  %struct_field_ptr112 = getelementptr inbounds <{ i16, i16, i32, i32, i16, i16, i16 }>, <{ i16, i16, i32, i32, i16, i16, i16 }>* %struct_alloca98, i32 0, i32 3
  %struct_field_ptr113 = getelementptr inbounds <{ i16, i16, i32, i32, i16, i16, i16 }>, <{ i16, i16, i32, i32, i16, i16, i16 }>* %struct_alloca98, i32 0, i32 2
  %struct_field114 = load i32, i32* %struct_field_ptr113
  %struct_field_ptr115 = getelementptr inbounds <{ i16, i16, i32, i32, i16, i16, i16 }>, <{ i16, i16, i32, i32, i16, i16, i16 }>* %struct_alloca98, i32 0, i32 4
  %struct_field116 = load i16, i16* %struct_field_ptr115
  %int_cast = sext i16 %struct_field116 to i32
  %mul_tmp117 = mul i32 %struct_field114, %int_cast
  store i32 %mul_tmp117, i32* %struct_field_ptr112
  %struct_field_ptr118 = getelementptr inbounds <{ i16, i16, i32, i32, i16, i16, i16 }>, <{ i16, i16, i32, i32, i16, i16, i16 }>* %struct_alloca98, i32 0, i32 6
  store i16 0, i16* %struct_field_ptr118
  %struct_arrow_load119 = load <{ i8* }>*, <{ i8* }>** %primary_buffer
  %struct_field_ptr120 = getelementptr inbounds <{ i8* }>, <{ i8* }>* %struct_arrow_load119, i32 0, i32 0
  %struct_arrow_load121 = load i8*, i8** %struct_field_ptr120
  %hack_bitcast122 = bitcast i8* %struct_arrow_load121 to <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>*
  %struct_field_ptr123 = getelementptr inbounds <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>, <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>* %hack_bitcast122, i32 0, i32 14
  %fun_ptr_load124 = load i32 (i8*, i8*)*, i32 (i8*, i8*)** %struct_field_ptr123
  %primary_buffer125 = load <{ i8* }>*, <{ i8* }>** %primary_buffer
  %fun_param_hack126 = bitcast <{ i8* }>* %primary_buffer125 to i8*
  %fun_param_hack127 = bitcast <{ i16, i16, i32, i32, i16, i16, i16 }>* %struct_alloca98 to i8*
  %fun_call128 = call i32 %fun_ptr_load124(i8* %fun_param_hack126, i8* %fun_param_hack127)
  store i32 %fun_call128, i32* %error
  %error129 = load i32, i32* %error
  %icmp_tmp130 = icmp eq i32 %error129, 0
  call void @assert(i1 %icmp_tmp130, <{ i32, i8* }> %arr_struct_load134, <{ i32, i8* }> %arr_struct_load139)
  %struct_arg_0141 = getelementptr inbounds <{ i32, i32, i32, i32, i8* }>, <{ i32, i32, i32, i32, i8* }>* %struct_alloca140, i32 0, i32 0
  store i32 0, i32* %struct_arg_0141
  %struct_arg_1142 = getelementptr inbounds <{ i32, i32, i32, i32, i8* }>, <{ i32, i32, i32, i32, i8* }>* %struct_alloca140, i32 0, i32 1
  store i32 0, i32* %struct_arg_1142
  %struct_arg_2143 = getelementptr inbounds <{ i32, i32, i32, i32, i8* }>, <{ i32, i32, i32, i32, i8* }>* %struct_alloca140, i32 0, i32 2
  store i32 0, i32* %struct_arg_2143
  %struct_arg_3144 = getelementptr inbounds <{ i32, i32, i32, i32, i8* }>, <{ i32, i32, i32, i32, i8* }>* %struct_alloca140, i32 0, i32 3
  store i32 0, i32* %struct_arg_3144
  %struct_arg_4145 = getelementptr inbounds <{ i32, i32, i32, i32, i8* }>, <{ i32, i32, i32, i32, i8* }>* %struct_alloca140, i32 0, i32 4
  store i8* null, i8** %struct_arg_4145
  %struct_field_ptr146 = getelementptr inbounds <{ i32, i32, i32, i32, i8* }>, <{ i32, i32, i32, i32, i8* }>* %struct_alloca140, i32 0, i32 0
  store i32 ptrtoint (<{ i32, i32, i32, i32, i8* }>* getelementptr (<{ i32, i32, i32, i32, i8* }>, <{ i32, i32, i32, i32, i8* }>* null, i32 1) to i32), i32* %struct_field_ptr146
  %struct_field_ptr147 = getelementptr inbounds <{ i32, i32, i32, i32, i8* }>, <{ i32, i32, i32, i32, i8* }>* %struct_alloca140, i32 0, i32 1
  store i32 65536, i32* %struct_field_ptr147
  %struct_field_ptr148 = getelementptr inbounds <{ i32, i32, i32, i32, i8* }>, <{ i32, i32, i32, i32, i8* }>* %struct_alloca140, i32 0, i32 2
  store i32 %buffer_size, i32* %struct_field_ptr148
  %struct_field_ptr149 = getelementptr inbounds <{ i32, i32, i32, i32, i8* }>, <{ i32, i32, i32, i32, i8* }>* %struct_alloca140, i32 0, i32 4
  %hmpf = bitcast <{ i16, i16, i32, i32, i16, i16, i16 }>* %struct_alloca98 to i8*
  store i8* %hmpf, i8** %struct_field_ptr149
  %struct_field_ptr150 = getelementptr inbounds <{ i32, i32, i32, i32, i8* }>, <{ i32, i32, i32, i32, i8* }>* %struct_alloca140, i32 0, i32 3
  store i32 0, i32* %struct_field_ptr150
  %struct_arrow_load151 = load <{ i8* }>*, <{ i8* }>** %dsound_obj
  %struct_field_ptr152 = getelementptr inbounds <{ i8* }>, <{ i8* }>* %struct_arrow_load151, i32 0, i32 0
  %struct_arrow_load153 = load i8*, i8** %struct_field_ptr152
  %hack_bitcast154 = bitcast i8* %struct_arrow_load153 to <{ i8*, i8*, i8*, i32 (i8*, i8*, i8**, i8*)*, i8*, i8*, i32 (i8*, i64, i32)*, i8*, i8*, i8*, i8* }>*
  %struct_field_ptr155 = getelementptr inbounds <{ i8*, i8*, i8*, i32 (i8*, i8*, i8**, i8*)*, i8*, i8*, i32 (i8*, i64, i32)*, i8*, i8*, i8*, i8* }>, <{ i8*, i8*, i8*, i32 (i8*, i8*, i8**, i8*)*, i8*, i8*, i32 (i8*, i64, i32)*, i8*, i8*, i8*, i8* }>* %hack_bitcast154, i32 0, i32 3
  %fun_ptr_load156 = load i32 (i8*, i8*, i8**, i8*)*, i32 (i8*, i8*, i8**, i8*)** %struct_field_ptr155
  %dsound_obj157 = load <{ i8* }>*, <{ i8* }>** %dsound_obj
  %fun_param_hack158 = bitcast <{ i8* }>* %dsound_obj157 to i8*
  %fun_param_hack159 = bitcast <{ i32, i32, i32, i32, i8* }>* %struct_alloca140 to i8*
  %fun_param_hack160 = bitcast <{ i8* }>** %secondary_buffer to i8**
  %fun_call161 = call i32 %fun_ptr_load156(i8* %fun_param_hack158, i8* %fun_param_hack159, i8** %fun_param_hack160, i8* null)
  store i32 %fun_call161, i32* %error
  %error162 = load i32, i32* %error
  %icmp_tmp163 = icmp eq i32 %error162, 0
  call void @assert(i1 %icmp_tmp163, <{ i32, i8* }> %arr_struct_load167, <{ i32, i8* }> %arr_struct_load172)
  %secondary_buffer173 = load <{ i8* }>*, <{ i8* }>** %secondary_buffer
  ret <{ i8* }>* %secondary_buffer173
}

; Function Attrs: nounwind
define internal void @clear_sound_buffer(<{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output) #0 {
vars:
  %region1 = alloca i8*
  %region1_size = alloca i32
  %region2 = alloca i8*
  %region2_size = alloca i32
  %error = alloca i32
  %arr_struct_alloca = alloca <{ i32, i8* }>
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.79, i32 0, i32 0), i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr
  %gep_arr_elem_ptr9 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 1
  store i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.79, i32 0, i32 0), i8** %gep_arr_elem_ptr9
  %arr_struct_load = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca
  %arr_struct_alloca10 = alloca <{ i32, i8* }>
  %arr_elem_alloca = alloca i8, i32 99
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca, i8* getelementptr inbounds ([100 x i8], [100 x i8]* @str.80, i32 0, i32 0), i32 99, i32 0, i1 false)
  %gep_arr_elem_ptr11 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca10, i32 0, i32 0
  store i32 99, i32* %gep_arr_elem_ptr11
  %gep_arr_elem_ptr12 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca10, i32 0, i32 1
  store i8* %arr_elem_alloca, i8** %gep_arr_elem_ptr12
  %arr_struct_load13 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca10
  %dest_sample = alloca i32*
  %sample_count = alloca i32
  %i = alloca i32
  %arr_struct_alloca25 = alloca <{ i32, i8* }>
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.81, i32 0, i32 0), i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr26 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca25, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr26
  %gep_arr_elem_ptr27 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca25, i32 0, i32 1
  store i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.81, i32 0, i32 0), i8** %gep_arr_elem_ptr27
  %arr_struct_load28 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca25
  %arr_struct_alloca29 = alloca <{ i32, i8* }>
  %arr_elem_alloca30 = alloca i8, i32 99
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca30, i8* getelementptr inbounds ([100 x i8], [100 x i8]* @str.82, i32 0, i32 0), i32 99, i32 0, i1 false)
  %gep_arr_elem_ptr31 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca29, i32 0, i32 0
  store i32 99, i32* %gep_arr_elem_ptr31
  %gep_arr_elem_ptr32 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca29, i32 0, i32 1
  store i8* %arr_elem_alloca30, i8** %gep_arr_elem_ptr32
  %arr_struct_load33 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca29
  %arr_struct_alloca51 = alloca <{ i32, i8* }>
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.83, i32 0, i32 0), i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr52 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca51, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr52
  %gep_arr_elem_ptr53 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca51, i32 0, i32 1
  store i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.83, i32 0, i32 0), i8** %gep_arr_elem_ptr53
  %arr_struct_load54 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca51
  %arr_struct_alloca55 = alloca <{ i32, i8* }>
  %arr_elem_alloca56 = alloca i8, i32 99
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca56, i8* getelementptr inbounds ([100 x i8], [100 x i8]* @str.84, i32 0, i32 0), i32 99, i32 0, i1 false)
  %gep_arr_elem_ptr57 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca55, i32 0, i32 0
  store i32 99, i32* %gep_arr_elem_ptr57
  %gep_arr_elem_ptr58 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca55, i32 0, i32 1
  store i8* %arr_elem_alloca56, i8** %gep_arr_elem_ptr58
  %arr_struct_load59 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca55
  br label %entry

entry:                                            ; preds = %vars
  %struct_field_ptr = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 0
  %struct_arrow_load = load i8*, i8** %struct_field_ptr
  %hack_bitcast = bitcast i8* %struct_arrow_load to <{ i8* }>*
  %struct_field_ptr1 = getelementptr inbounds <{ i8* }>, <{ i8* }>* %hack_bitcast, i32 0, i32 0
  %struct_arrow_load2 = load i8*, i8** %struct_field_ptr1
  %hack_bitcast3 = bitcast i8* %struct_arrow_load2 to <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>*
  %struct_field_ptr4 = getelementptr inbounds <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>, <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>* %hack_bitcast3, i32 0, i32 11
  %fun_ptr_load = load i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)** %struct_field_ptr4
  %struct_field_ptr5 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 0
  %struct_arrow = load i8*, i8** %struct_field_ptr5
  %struct_field_ptr6 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 4
  %struct_arrow7 = load i32, i32* %struct_field_ptr6
  %fun_call = call i32 %fun_ptr_load(i8* %struct_arrow, i32 0, i32 %struct_arrow7, i8** %region1, i32* %region1_size, i8** %region2, i32* %region2_size, i32 0)
  store i32 %fun_call, i32* %error
  %error8 = load i32, i32* %error
  %icmp_tmp = icmp eq i32 %error8, 0
  call void @assert(i1 %icmp_tmp, <{ i32, i8* }> %arr_struct_load, <{ i32, i8* }> %arr_struct_load13)
  %region114 = load i8*, i8** %region1
  %pointer_bit_cast = bitcast i8* %region114 to i32*
  store i32* %pointer_bit_cast, i32** %dest_sample
  %region1_size15 = load i32, i32* %region1_size
  %struct_field_ptr16 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 2
  %struct_arrow17 = load i32, i32* %struct_field_ptr16
  %div_tmp = sdiv i32 %region1_size15, %struct_arrow17
  store i32 %div_tmp, i32* %sample_count
  store i32 0, i32* %i
  br label %for_cond

for_cond:                                         ; preds = %for_iter, %entry
  %i18 = load i32, i32* %i
  %sample_count19 = load i32, i32* %sample_count
  %icmp_tmp20 = icmp slt i32 %i18, %sample_count19
  br i1 %icmp_tmp20, label %for, label %end_for

for:                                              ; preds = %for_cond
  %postinc_load = load i32*, i32** %dest_sample
  %ptr_post_inc = getelementptr i32, i32* %postinc_load, i32 1
  store i32* %ptr_post_inc, i32** %dest_sample
  store i32 0, i32* %postinc_load
  br label %for_iter

for_iter:                                         ; preds = %for
  %preinc_load = load i32, i32* %i
  %preinc = add i32 %preinc_load, 1
  store i32 %preinc, i32* %i
  br label %for_cond

end_for:                                          ; preds = %for_cond
  %struct_field_ptr21 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 5
  %region122 = load i8*, i8** %region1
  store i8* %region122, i8** %struct_field_ptr21
  %region2_size23 = load i32, i32* %region2_size
  %icmp_tmp24 = icmp eq i32 %region2_size23, 0
  call void @assert(i1 %icmp_tmp24, <{ i32, i8* }> %arr_struct_load28, <{ i32, i8* }> %arr_struct_load33)
  %struct_field_ptr34 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 0
  %struct_arrow_load35 = load i8*, i8** %struct_field_ptr34
  %hack_bitcast36 = bitcast i8* %struct_arrow_load35 to <{ i8* }>*
  %struct_field_ptr37 = getelementptr inbounds <{ i8* }>, <{ i8* }>* %hack_bitcast36, i32 0, i32 0
  %struct_arrow_load38 = load i8*, i8** %struct_field_ptr37
  %hack_bitcast39 = bitcast i8* %struct_arrow_load38 to <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>*
  %struct_field_ptr40 = getelementptr inbounds <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>, <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>* %hack_bitcast39, i32 0, i32 19
  %fun_ptr_load41 = load i32 (i8*, i8*, i32, i8*, i32)*, i32 (i8*, i8*, i32, i8*, i32)** %struct_field_ptr40
  %struct_field_ptr42 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 0
  %struct_arrow43 = load i8*, i8** %struct_field_ptr42
  %region144 = load i8*, i8** %region1
  %region1_size45 = load i32, i32* %region1_size
  %region246 = load i8*, i8** %region2
  %region2_size47 = load i32, i32* %region2_size
  %fun_call48 = call i32 %fun_ptr_load41(i8* %struct_arrow43, i8* %region144, i32 %region1_size45, i8* %region246, i32 %region2_size47)
  store i32 %fun_call48, i32* %error
  %error49 = load i32, i32* %error
  %icmp_tmp50 = icmp eq i32 %error49, 0
  call void @assert(i1 %icmp_tmp50, <{ i32, i8* }> %arr_struct_load54, <{ i32, i8* }> %arr_struct_load59)
  ret void
}

; Function Attrs: nounwind
define internal void @fill_sound_buffer(<{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, <{ i16*, i32, i32, float }>* %game_sound_output) #0 {
vars:
  %byte_lock_position = alloca i32
  %bytes_to_write = alloca i32
  %region1 = alloca i8*
  %region1_size = alloca i32
  %region2 = alloca i8*
  %region2_size = alloca i32
  %error = alloca i32
  %sample_count_1 = alloca i32
  %sample_count_2 = alloca i32
  %arr_struct_alloca = alloca <{ i32, i8* }>
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.85, i32 0, i32 0), i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr
  %gep_arr_elem_ptr25 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 1
  store i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.85, i32 0, i32 0), i8** %gep_arr_elem_ptr25
  %arr_struct_load = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca
  %arr_struct_alloca26 = alloca <{ i32, i8* }>
  %arr_elem_alloca = alloca i8, i32 100
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca, i8* getelementptr inbounds ([101 x i8], [101 x i8]* @str.86, i32 0, i32 0), i32 100, i32 0, i1 false)
  %gep_arr_elem_ptr27 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca26, i32 0, i32 0
  store i32 100, i32* %gep_arr_elem_ptr27
  %gep_arr_elem_ptr28 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca26, i32 0, i32 1
  store i8* %arr_elem_alloca, i8** %gep_arr_elem_ptr28
  %arr_struct_load29 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca26
  %source_sample = alloca i16*
  %dest_sample = alloca i16*
  %i = alloca i32
  %i49 = alloca i32
  %arr_struct_alloca82 = alloca <{ i32, i8* }>
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.87, i32 0, i32 0), i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr83 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca82, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr83
  %gep_arr_elem_ptr84 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca82, i32 0, i32 1
  store i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.87, i32 0, i32 0), i8** %gep_arr_elem_ptr84
  %arr_struct_load85 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca82
  %arr_struct_alloca86 = alloca <{ i32, i8* }>
  %arr_elem_alloca87 = alloca i8, i32 100
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca87, i8* getelementptr inbounds ([101 x i8], [101 x i8]* @str.88, i32 0, i32 0), i32 100, i32 0, i1 false)
  %gep_arr_elem_ptr88 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca86, i32 0, i32 0
  store i32 100, i32* %gep_arr_elem_ptr88
  %gep_arr_elem_ptr89 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca86, i32 0, i32 1
  store i8* %arr_elem_alloca87, i8** %gep_arr_elem_ptr89
  %arr_struct_load90 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca86
  %last_sample = alloca i8*
  %last_sample111 = alloca i8*
  %arr_struct_alloca127 = alloca <{ i32, i8* }>
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.89, i32 0, i32 0), i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr128 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca127, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr128
  %gep_arr_elem_ptr129 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca127, i32 0, i32 1
  store i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.89, i32 0, i32 0), i8** %gep_arr_elem_ptr129
  %arr_struct_load130 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca127
  %arr_struct_alloca131 = alloca <{ i32, i8* }>
  %arr_elem_alloca132 = alloca i8, i32 100
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca132, i8* getelementptr inbounds ([101 x i8], [101 x i8]* @str.90, i32 0, i32 0), i32 100, i32 0, i1 false)
  %gep_arr_elem_ptr133 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca131, i32 0, i32 0
  store i32 100, i32* %gep_arr_elem_ptr133
  %gep_arr_elem_ptr134 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca131, i32 0, i32 1
  store i8* %arr_elem_alloca132, i8** %gep_arr_elem_ptr134
  %arr_struct_load135 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca131
  %arr_struct_alloca147 = alloca <{ i32, i8* }>
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.91, i32 0, i32 0), i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr148 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca147, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr148
  %gep_arr_elem_ptr149 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca147, i32 0, i32 1
  store i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.91, i32 0, i32 0), i8** %gep_arr_elem_ptr149
  %arr_struct_load150 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca147
  %arr_struct_alloca151 = alloca <{ i32, i8* }>
  %arr_elem_alloca152 = alloca i8, i32 100
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca152, i8* getelementptr inbounds ([101 x i8], [101 x i8]* @str.92, i32 0, i32 0), i32 100, i32 0, i1 false)
  %gep_arr_elem_ptr153 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca151, i32 0, i32 0
  store i32 100, i32* %gep_arr_elem_ptr153
  %gep_arr_elem_ptr154 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca151, i32 0, i32 1
  store i8* %arr_elem_alloca152, i8** %gep_arr_elem_ptr154
  %arr_struct_load155 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca151
  br label %entry

entry:                                            ; preds = %vars
  %struct_field_ptr = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 9
  %struct_arrow = load i32, i32* %struct_field_ptr
  store i32 %struct_arrow, i32* %byte_lock_position
  %struct_field_ptr1 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 10
  %struct_arrow2 = load i32, i32* %struct_field_ptr1
  store i32 %struct_arrow2, i32* %bytes_to_write
  %struct_field_ptr3 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 0
  %struct_arrow_load = load i8*, i8** %struct_field_ptr3
  %hack_bitcast = bitcast i8* %struct_arrow_load to <{ i8* }>*
  %struct_field_ptr4 = getelementptr inbounds <{ i8* }>, <{ i8* }>* %hack_bitcast, i32 0, i32 0
  %struct_arrow_load5 = load i8*, i8** %struct_field_ptr4
  %hack_bitcast6 = bitcast i8* %struct_arrow_load5 to <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>*
  %struct_field_ptr7 = getelementptr inbounds <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>, <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>* %hack_bitcast6, i32 0, i32 11
  %fun_ptr_load = load i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)** %struct_field_ptr7
  %struct_field_ptr8 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 0
  %struct_arrow9 = load i8*, i8** %struct_field_ptr8
  %byte_lock_position10 = load i32, i32* %byte_lock_position
  %bytes_to_write11 = load i32, i32* %bytes_to_write
  %fun_call = call i32 %fun_ptr_load(i8* %struct_arrow9, i32 %byte_lock_position10, i32 %bytes_to_write11, i8** %region1, i32* %region1_size, i8** %region2, i32* %region2_size, i32 0)
  store i32 %fun_call, i32* %error
  %region1_size12 = load i32, i32* %region1_size
  %struct_field_ptr13 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 2
  %struct_arrow14 = load i32, i32* %struct_field_ptr13
  %div_tmp = sdiv i32 %region1_size12, %struct_arrow14
  store i32 %div_tmp, i32* %sample_count_1
  %region2_size15 = load i32, i32* %region2_size
  %struct_field_ptr16 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 2
  %struct_arrow17 = load i32, i32* %struct_field_ptr16
  %div_tmp18 = sdiv i32 %region2_size15, %struct_arrow17
  store i32 %div_tmp18, i32* %sample_count_2
  %error19 = load i32, i32* %error
  %icmp_tmp = icmp eq i32 %error19, 0
  br i1 %icmp_tmp, label %then, label %endif

then:                                             ; preds = %entry
  %sample_count_120 = load i32, i32* %sample_count_1
  %sample_count_221 = load i32, i32* %sample_count_2
  %add_tmp = add i32 %sample_count_120, %sample_count_221
  %struct_field_ptr22 = getelementptr inbounds <{ i16*, i32, i32, float }>, <{ i16*, i32, i32, float }>* %game_sound_output, i32 0, i32 1
  %struct_arrow23 = load i32, i32* %struct_field_ptr22
  %icmp_tmp24 = icmp eq i32 %add_tmp, %struct_arrow23
  call void @assert(i1 %icmp_tmp24, <{ i32, i8* }> %arr_struct_load, <{ i32, i8* }> %arr_struct_load29)
  %struct_field_ptr30 = getelementptr inbounds <{ i16*, i32, i32, float }>, <{ i16*, i32, i32, float }>* %game_sound_output, i32 0, i32 0
  %struct_arrow31 = load i16*, i16** %struct_field_ptr30
  store i16* %struct_arrow31, i16** %source_sample
  %region132 = load i8*, i8** %region1
  %pointer_bit_cast = bitcast i8* %region132 to i16*
  store i16* %pointer_bit_cast, i16** %dest_sample
  store i32 0, i32* %i
  br label %for_cond

for_cond:                                         ; preds = %for_iter, %then
  %i33 = load i32, i32* %i
  %sample_count_134 = load i32, i32* %sample_count_1
  %icmp_tmp35 = icmp slt i32 %i33, %sample_count_134
  br i1 %icmp_tmp35, label %for, label %end_for

for:                                              ; preds = %for_cond
  %postinc_load = load i16*, i16** %dest_sample
  %ptr_post_inc = getelementptr i16, i16* %postinc_load, i32 1
  store i16* %ptr_post_inc, i16** %dest_sample
  %postinc_load36 = load i16*, i16** %source_sample
  %ptr_post_inc37 = getelementptr i16, i16* %postinc_load36, i32 1
  store i16* %ptr_post_inc37, i16** %source_sample
  %deref = load i16, i16* %postinc_load36
  store i16 %deref, i16* %postinc_load
  %postinc_load38 = load i16*, i16** %dest_sample
  %ptr_post_inc39 = getelementptr i16, i16* %postinc_load38, i32 1
  store i16* %ptr_post_inc39, i16** %dest_sample
  %postinc_load40 = load i16*, i16** %source_sample
  %ptr_post_inc41 = getelementptr i16, i16* %postinc_load40, i32 1
  store i16* %ptr_post_inc41, i16** %source_sample
  %deref42 = load i16, i16* %postinc_load40
  store i16 %deref42, i16* %postinc_load38
  br label %for_iter

for_iter:                                         ; preds = %for
  %preinc_load = load i32, i32* %i
  %preinc = add i32 %preinc_load, 1
  store i32 %preinc, i32* %i
  br label %for_cond

end_for:                                          ; preds = %for_cond
  %region243 = load i8*, i8** %region2
  %pointer_bit_cast44 = bitcast i8* %region243 to i16*
  store i16* %pointer_bit_cast44, i16** %dest_sample
  store i32 0, i32* %i49
  br label %for_cond45

for_cond45:                                       ; preds = %for_iter47, %end_for
  %i50 = load i32, i32* %i49
  %sample_count_251 = load i32, i32* %sample_count_2
  %icmp_tmp52 = icmp slt i32 %i50, %sample_count_251
  br i1 %icmp_tmp52, label %for46, label %end_for48

for46:                                            ; preds = %for_cond45
  %postinc_load53 = load i16*, i16** %dest_sample
  %ptr_post_inc54 = getelementptr i16, i16* %postinc_load53, i32 1
  store i16* %ptr_post_inc54, i16** %dest_sample
  %postinc_load55 = load i16*, i16** %source_sample
  %ptr_post_inc56 = getelementptr i16, i16* %postinc_load55, i32 1
  store i16* %ptr_post_inc56, i16** %source_sample
  %deref57 = load i16, i16* %postinc_load55
  store i16 %deref57, i16* %postinc_load53
  %postinc_load58 = load i16*, i16** %dest_sample
  %ptr_post_inc59 = getelementptr i16, i16* %postinc_load58, i32 1
  store i16* %ptr_post_inc59, i16** %dest_sample
  %postinc_load60 = load i16*, i16** %source_sample
  %ptr_post_inc61 = getelementptr i16, i16* %postinc_load60, i32 1
  store i16* %ptr_post_inc61, i16** %source_sample
  %deref62 = load i16, i16* %postinc_load60
  store i16 %deref62, i16* %postinc_load58
  br label %for_iter47

for_iter47:                                       ; preds = %for46
  %preinc_load63 = load i32, i32* %i49
  %preinc64 = add i32 %preinc_load63, 1
  store i32 %preinc64, i32* %i49
  br label %for_cond45

end_for48:                                        ; preds = %for_cond45
  %struct_field_ptr65 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 0
  %struct_arrow_load66 = load i8*, i8** %struct_field_ptr65
  %hack_bitcast67 = bitcast i8* %struct_arrow_load66 to <{ i8* }>*
  %struct_field_ptr68 = getelementptr inbounds <{ i8* }>, <{ i8* }>* %hack_bitcast67, i32 0, i32 0
  %struct_arrow_load69 = load i8*, i8** %struct_field_ptr68
  %hack_bitcast70 = bitcast i8* %struct_arrow_load69 to <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>*
  %struct_field_ptr71 = getelementptr inbounds <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>, <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>* %hack_bitcast70, i32 0, i32 19
  %fun_ptr_load72 = load i32 (i8*, i8*, i32, i8*, i32)*, i32 (i8*, i8*, i32, i8*, i32)** %struct_field_ptr71
  %struct_field_ptr73 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 0
  %struct_arrow74 = load i8*, i8** %struct_field_ptr73
  %region175 = load i8*, i8** %region1
  %region1_size76 = load i32, i32* %region1_size
  %region277 = load i8*, i8** %region2
  %region2_size78 = load i32, i32* %region2_size
  %fun_call79 = call i32 %fun_ptr_load72(i8* %struct_arrow74, i8* %region175, i32 %region1_size76, i8* %region277, i32 %region2_size78)
  store i32 %fun_call79, i32* %error
  %error80 = load i32, i32* %error
  %icmp_tmp81 = icmp eq i32 %error80, 0
  call void @assert(i1 %icmp_tmp81, <{ i32, i8* }> %arr_struct_load85, <{ i32, i8* }> %arr_struct_load90)
  %sample_count_291 = load i32, i32* %sample_count_2
  %icmp_tmp92 = icmp sgt i32 %sample_count_291, 0
  br i1 %icmp_tmp92, label %then93, label %elif_0

then93:                                           ; preds = %end_for48
  %region295 = load i8*, i8** %region2
  %pointer_bit_cast96 = bitcast i8* %region295 to i32*
  %sample_count_297 = load i32, i32* %sample_count_2
  %ptr_add = getelementptr i32, i32* %pointer_bit_cast96, i32 %sample_count_297
  %pointer_bit_cast98 = bitcast i32* %ptr_add to i8*
  store i8* %pointer_bit_cast98, i8** %last_sample
  %struct_field_ptr99 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 6
  %last_sample100 = load i8*, i8** %last_sample
  %struct_field_ptr101 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 5
  %struct_arrow102 = load i8*, i8** %struct_field_ptr101
  %ptr_to_int = ptrtoint i8* %last_sample100 to i64
  %ptr_to_int103 = ptrtoint i8* %struct_arrow102 to i64
  %sub = sub i64 %ptr_to_int, %ptr_to_int103
  %div = sdiv i64 %sub, ptrtoint (i8* getelementptr (i8, i8* null, i32 1) to i64)
  %int_trunc = trunc i64 %div to i32
  store i32 %int_trunc, i32* %struct_field_ptr99
  br label %endif94

elif_0:                                           ; preds = %end_for48
  %sample_count_1104 = load i32, i32* %sample_count_1
  %icmp_tmp105 = icmp sgt i32 %sample_count_1104, 0
  br i1 %icmp_tmp105, label %elif_0_then, label %endif94

elif_0_then:                                      ; preds = %elif_0
  %region1106 = load i8*, i8** %region1
  %pointer_bit_cast107 = bitcast i8* %region1106 to i32*
  %sample_count_1108 = load i32, i32* %sample_count_1
  %ptr_add109 = getelementptr i32, i32* %pointer_bit_cast107, i32 %sample_count_1108
  %pointer_bit_cast110 = bitcast i32* %ptr_add109 to i8*
  store i8* %pointer_bit_cast110, i8** %last_sample111
  %struct_field_ptr112 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 6
  %last_sample113 = load i8*, i8** %last_sample111
  %struct_field_ptr114 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 5
  %struct_arrow115 = load i8*, i8** %struct_field_ptr114
  %ptr_to_int116 = ptrtoint i8* %last_sample113 to i64
  %ptr_to_int117 = ptrtoint i8* %struct_arrow115 to i64
  %sub118 = sub i64 %ptr_to_int116, %ptr_to_int117
  %div119 = sdiv i64 %sub118, ptrtoint (i8* getelementptr (i8, i8* null, i32 1) to i64)
  %int_trunc120 = trunc i64 %div119 to i32
  store i32 %int_trunc120, i32* %struct_field_ptr112
  br label %endif94

endif94:                                          ; preds = %elif_0_then, %elif_0, %then93
  %sample_count_2121 = load i32, i32* %sample_count_2
  %icmp_tmp122 = icmp eq i32 %sample_count_2121, 0
  br i1 %icmp_tmp122, label %cor.end, label %cor.rhs

cor.rhs:                                          ; preds = %endif94
  %sample_count_1123 = load i32, i32* %sample_count_1
  %icmp_tmp124 = icmp ne i32 %sample_count_1123, 0
  br i1 %icmp_tmp124, label %cand.rhs, label %cand.end

cand.rhs:                                         ; preds = %cor.rhs
  %sample_count_2125 = load i32, i32* %sample_count_2
  %icmp_tmp126 = icmp ne i32 %sample_count_2125, 0
  br label %cand.end

cand.end:                                         ; preds = %cand.rhs, %cor.rhs
  %candphi = phi i1 [ false, %cor.rhs ], [ %icmp_tmp126, %cand.rhs ]
  br label %cor.end

cor.end:                                          ; preds = %cand.end, %endif94
  %corphi = phi i1 [ true, %endif94 ], [ %candphi, %cand.end ]
  call void @assert(i1 %corphi, <{ i32, i8* }> %arr_struct_load130, <{ i32, i8* }> %arr_struct_load135)
  %struct_field_ptr136 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 6
  %struct_arrow137 = load i32, i32* %struct_field_ptr136
  %icmp_tmp138 = icmp sge i32 %struct_arrow137, 0
  br i1 %icmp_tmp138, label %cand.rhs139, label %cand.end140

cand.rhs139:                                      ; preds = %cor.end
  %struct_field_ptr141 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 6
  %struct_arrow142 = load i32, i32* %struct_field_ptr141
  %struct_field_ptr143 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 4
  %struct_arrow144 = load i32, i32* %struct_field_ptr143
  %icmp_tmp145 = icmp slt i32 %struct_arrow142, %struct_arrow144
  br label %cand.end140

cand.end140:                                      ; preds = %cand.rhs139, %cor.end
  %candphi146 = phi i1 [ false, %cor.end ], [ %icmp_tmp145, %cand.rhs139 ]
  call void @assert(i1 %candphi146, <{ i32, i8* }> %arr_struct_load150, <{ i32, i8* }> %arr_struct_load155)
  br label %endif

endif:                                            ; preds = %cand.end140, %entry
  ret void
}

; Function Attrs: nounwind
define internal void @update_next_sound_write_positon(<{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output) #0 {
vars:
  %play_cursor = alloca i32
  %write_cursor = alloca i32
  %error = alloca i32
  %arr_struct_alloca = alloca <{ i32, i8* }>
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.93, i32 0, i32 0), i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr
  %gep_arr_elem_ptr7 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 1
  store i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.93, i32 0, i32 0), i8** %gep_arr_elem_ptr7
  %arr_struct_load = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca
  %arr_struct_alloca8 = alloca <{ i32, i8* }>
  %arr_elem_alloca = alloca i8, i32 99
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca, i8* getelementptr inbounds ([100 x i8], [100 x i8]* @str.94, i32 0, i32 0), i32 99, i32 0, i1 false)
  %gep_arr_elem_ptr9 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca8, i32 0, i32 0
  store i32 99, i32* %gep_arr_elem_ptr9
  %gep_arr_elem_ptr10 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca8, i32 0, i32 1
  store i8* %arr_elem_alloca, i8** %gep_arr_elem_ptr10
  %arr_struct_load11 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca8
  %time = alloca i64
  %cursor_dt = alloca double
  %d_bytes = alloca i32
  %position_to_write = alloca i32
  %delta_cursor = alloca i32
  %arr_struct_alloca139 = alloca <{ i32, i8* }>
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.95, i32 0, i32 0), i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr140 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca139, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr140
  %gep_arr_elem_ptr141 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca139, i32 0, i32 1
  store i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.95, i32 0, i32 0), i8** %gep_arr_elem_ptr141
  %arr_struct_load142 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca139
  %arr_struct_alloca143 = alloca <{ i32, i8* }>
  %arr_elem_alloca144 = alloca i8, i32 99
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca144, i8* getelementptr inbounds ([100 x i8], [100 x i8]* @str.96, i32 0, i32 0), i32 99, i32 0, i1 false)
  %gep_arr_elem_ptr145 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca143, i32 0, i32 0
  store i32 99, i32* %gep_arr_elem_ptr145
  %gep_arr_elem_ptr146 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca143, i32 0, i32 1
  store i8* %arr_elem_alloca144, i8** %gep_arr_elem_ptr146
  %arr_struct_load147 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca143
  %target_cursor = alloca i32
  %safety_bytes = alloca i32
  %bytes_to_write = alloca i32
  br label %entry

entry:                                            ; preds = %vars
  %struct_field_ptr = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 0
  %struct_arrow_load = load i8*, i8** %struct_field_ptr
  %hack_bitcast = bitcast i8* %struct_arrow_load to <{ i8* }>*
  %struct_field_ptr1 = getelementptr inbounds <{ i8* }>, <{ i8* }>* %hack_bitcast, i32 0, i32 0
  %struct_arrow_load2 = load i8*, i8** %struct_field_ptr1
  %hack_bitcast3 = bitcast i8* %struct_arrow_load2 to <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>*
  %struct_field_ptr4 = getelementptr inbounds <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>, <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>* %hack_bitcast3, i32 0, i32 4
  %fun_ptr_load = load i32 (i8*, i32*, i32*)*, i32 (i8*, i32*, i32*)** %struct_field_ptr4
  %struct_field_ptr5 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 0
  %struct_arrow = load i8*, i8** %struct_field_ptr5
  %fun_call = call i32 %fun_ptr_load(i8* %struct_arrow, i32* %play_cursor, i32* %write_cursor)
  store i32 %fun_call, i32* %error
  %error6 = load i32, i32* %error
  %icmp_tmp = icmp eq i32 %error6, 0
  call void @assert(i1 %icmp_tmp, <{ i32, i8* }> %arr_struct_load, <{ i32, i8* }> %arr_struct_load11)
  %play_cursor12 = load i32, i32* %play_cursor
  %write_cursor13 = load i32, i32* %write_cursor
  %icmp_tmp14 = icmp eq i32 %play_cursor12, %write_cursor13
  br i1 %icmp_tmp14, label %then, label %endif

then:                                             ; preds = %entry
  ret void

endif:                                            ; preds = %entry
  %struct_field_ptr15 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 11
  %struct_arrow16 = load i1, i1* %struct_field_ptr15
  %not_tmp = xor i1 %struct_arrow16, true
  br i1 %not_tmp, label %then17, label %endif18

then17:                                           ; preds = %endif
  %struct_field_ptr19 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 6
  %write_cursor20 = load i32, i32* %write_cursor
  store i32 %write_cursor20, i32* %struct_field_ptr19
  %struct_field_ptr21 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 8
  %fun_call22 = call i64 @get_perf_counter()
  store i64 %fun_call22, i64* %struct_field_ptr21
  br label %endif18

endif18:                                          ; preds = %then17, %endif
  %play_cursor23 = load i32, i32* %play_cursor
  %struct_field_ptr24 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 7
  %struct_arrow25 = load i32, i32* %struct_field_ptr24
  %icmp_tmp26 = icmp eq i32 %play_cursor23, %struct_arrow25
  br i1 %icmp_tmp26, label %then27, label %else

then27:                                           ; preds = %endif18
  %fun_call29 = call i32 @QueryPerformanceCounter(i64* %time)
  %time30 = load i64, i64* %time
  %struct_field_ptr31 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 8
  %struct_arrow32 = load i64, i64* %struct_field_ptr31
  %sub_tmp = sub i64 %time30, %struct_arrow32
  %int_to_float_cast = sitofp i64 %sub_tmp to double
  %perf_count_freq = load i64, i64* @perf_count_freq
  %int_to_float_cast33 = sitofp i64 %perf_count_freq to double
  %fdiv_tmp = fdiv double %int_to_float_cast, %int_to_float_cast33
  store double %fdiv_tmp, double* %cursor_dt
  %struct_field_ptr34 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 2
  %struct_arrow35 = load i32, i32* %struct_field_ptr34
  %cursor_dt36 = load double, double* %cursor_dt
  %struct_field_ptr37 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 1
  %struct_arrow38 = load i32, i32* %struct_field_ptr37
  %int_to_float_cast39 = sitofp i32 %struct_arrow38 to double
  %fmul_tmp = fmul double %cursor_dt36, %int_to_float_cast39
  %int_cast = fptosi double %fmul_tmp to i32
  %mul_tmp = mul i32 %struct_arrow35, %int_cast
  store i32 %mul_tmp, i32* %d_bytes
  %play_cursor40 = load i32, i32* %play_cursor
  %d_bytes41 = load i32, i32* %d_bytes
  %add_tmp = add i32 %play_cursor40, %d_bytes41
  store i32 %add_tmp, i32* %play_cursor
  %write_cursor42 = load i32, i32* %write_cursor
  %d_bytes43 = load i32, i32* %d_bytes
  %add_tmp44 = add i32 %write_cursor42, %d_bytes43
  store i32 %add_tmp44, i32* %write_cursor
  %play_cursor45 = load i32, i32* %play_cursor
  %struct_field_ptr46 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 4
  %struct_arrow47 = load i32, i32* %struct_field_ptr46
  %urem_tmp = urem i32 %play_cursor45, %struct_arrow47
  store i32 %urem_tmp, i32* %play_cursor
  %write_cursor48 = load i32, i32* %write_cursor
  %struct_field_ptr49 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 4
  %struct_arrow50 = load i32, i32* %struct_field_ptr49
  %urem_tmp51 = urem i32 %write_cursor48, %struct_arrow50
  store i32 %urem_tmp51, i32* %write_cursor
  br label %endif28

else:                                             ; preds = %endif18
  %struct_field_ptr52 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 7
  %play_cursor53 = load i32, i32* %play_cursor
  store i32 %play_cursor53, i32* %struct_field_ptr52
  %struct_field_ptr54 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 8
  %fun_call55 = call i64 @get_perf_counter()
  store i64 %fun_call55, i64* %struct_field_ptr54
  br label %endif28

endif28:                                          ; preds = %else, %then27
  %struct_field_ptr56 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 6
  %struct_arrow57 = load i32, i32* %struct_field_ptr56
  store i32 %struct_arrow57, i32* %position_to_write
  %play_cursor58 = load i32, i32* %play_cursor
  %struct_field_ptr59 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 7
  %struct_arrow60 = load i32, i32* %struct_field_ptr59
  %icmp_tmp61 = icmp sgt i32 %play_cursor58, %struct_arrow60
  br i1 %icmp_tmp61, label %then62, label %endif63

then62:                                           ; preds = %endif28
  %struct_field_ptr64 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 6
  %struct_arrow65 = load i32, i32* %struct_field_ptr64
  %struct_field_ptr66 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 7
  %struct_arrow67 = load i32, i32* %struct_field_ptr66
  %icmp_tmp68 = icmp sge i32 %struct_arrow65, %struct_arrow67
  br i1 %icmp_tmp68, label %cand.rhs, label %cand.end

cand.rhs:                                         ; preds = %then62
  %struct_field_ptr69 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 6
  %struct_arrow70 = load i32, i32* %struct_field_ptr69
  %play_cursor71 = load i32, i32* %play_cursor
  %icmp_tmp72 = icmp sle i32 %struct_arrow70, %play_cursor71
  br label %cand.end

cand.end:                                         ; preds = %cand.rhs, %then62
  %candphi = phi i1 [ false, %then62 ], [ %icmp_tmp72, %cand.rhs ]
  br i1 %candphi, label %then73, label %endif74

then73:                                           ; preds = %cand.end
  %write_cursor75 = load i32, i32* %write_cursor
  store i32 %write_cursor75, i32* %position_to_write
  br label %endif74

endif74:                                          ; preds = %then73, %cand.end
  br label %endif63

endif63:                                          ; preds = %endif74, %endif28
  %play_cursor76 = load i32, i32* %play_cursor
  %struct_field_ptr77 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 7
  %struct_arrow78 = load i32, i32* %struct_field_ptr77
  %icmp_tmp79 = icmp slt i32 %play_cursor76, %struct_arrow78
  br i1 %icmp_tmp79, label %then80, label %endif81

then80:                                           ; preds = %endif63
  %struct_field_ptr82 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 6
  %struct_arrow83 = load i32, i32* %struct_field_ptr82
  %struct_field_ptr84 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 7
  %struct_arrow85 = load i32, i32* %struct_field_ptr84
  %icmp_tmp86 = icmp sge i32 %struct_arrow83, %struct_arrow85
  br i1 %icmp_tmp86, label %cor.end, label %cor.rhs

cor.rhs:                                          ; preds = %then80
  %struct_field_ptr87 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 6
  %struct_arrow88 = load i32, i32* %struct_field_ptr87
  %play_cursor89 = load i32, i32* %play_cursor
  %icmp_tmp90 = icmp sle i32 %struct_arrow88, %play_cursor89
  br label %cor.end

cor.end:                                          ; preds = %cor.rhs, %then80
  %corphi = phi i1 [ true, %then80 ], [ %icmp_tmp90, %cor.rhs ]
  br i1 %corphi, label %then91, label %endif92

then91:                                           ; preds = %cor.end
  %write_cursor93 = load i32, i32* %write_cursor
  store i32 %write_cursor93, i32* %position_to_write
  br label %endif92

endif92:                                          ; preds = %then91, %cor.end
  br label %endif81

endif81:                                          ; preds = %endif92, %endif63
  %play_cursor94 = load i32, i32* %play_cursor
  %write_cursor95 = load i32, i32* %write_cursor
  %icmp_tmp96 = icmp slt i32 %play_cursor94, %write_cursor95
  br i1 %icmp_tmp96, label %then97, label %endif98

then97:                                           ; preds = %endif81
  %position_to_write99 = load i32, i32* %position_to_write
  %play_cursor100 = load i32, i32* %play_cursor
  %icmp_tmp101 = icmp sge i32 %position_to_write99, %play_cursor100
  br i1 %icmp_tmp101, label %cand.rhs102, label %cand.end103

cand.rhs102:                                      ; preds = %then97
  %position_to_write104 = load i32, i32* %position_to_write
  %write_cursor105 = load i32, i32* %write_cursor
  %icmp_tmp106 = icmp slt i32 %position_to_write104, %write_cursor105
  br label %cand.end103

cand.end103:                                      ; preds = %cand.rhs102, %then97
  %candphi107 = phi i1 [ false, %then97 ], [ %icmp_tmp106, %cand.rhs102 ]
  br i1 %candphi107, label %then108, label %endif109

then108:                                          ; preds = %cand.end103
  %write_cursor110 = load i32, i32* %write_cursor
  store i32 %write_cursor110, i32* %position_to_write
  br label %endif109

endif109:                                         ; preds = %then108, %cand.end103
  %write_cursor111 = load i32, i32* %write_cursor
  %play_cursor112 = load i32, i32* %play_cursor
  %sub_tmp113 = sub i32 %write_cursor111, %play_cursor112
  store i32 %sub_tmp113, i32* %delta_cursor
  br label %endif98

endif98:                                          ; preds = %endif109, %endif81
  %play_cursor114 = load i32, i32* %play_cursor
  %write_cursor115 = load i32, i32* %write_cursor
  %icmp_tmp116 = icmp sgt i32 %play_cursor114, %write_cursor115
  br i1 %icmp_tmp116, label %then117, label %endif118

then117:                                          ; preds = %endif98
  %position_to_write119 = load i32, i32* %position_to_write
  %play_cursor120 = load i32, i32* %play_cursor
  %icmp_tmp121 = icmp sge i32 %position_to_write119, %play_cursor120
  br i1 %icmp_tmp121, label %cor.end123, label %cor.rhs122

cor.rhs122:                                       ; preds = %then117
  %position_to_write124 = load i32, i32* %position_to_write
  %write_cursor125 = load i32, i32* %write_cursor
  %icmp_tmp126 = icmp slt i32 %position_to_write124, %write_cursor125
  br label %cor.end123

cor.end123:                                       ; preds = %cor.rhs122, %then117
  %corphi127 = phi i1 [ true, %then117 ], [ %icmp_tmp126, %cor.rhs122 ]
  br i1 %corphi127, label %then128, label %endif129

then128:                                          ; preds = %cor.end123
  %write_cursor130 = load i32, i32* %write_cursor
  store i32 %write_cursor130, i32* %position_to_write
  br label %endif129

endif129:                                         ; preds = %then128, %cor.end123
  %struct_field_ptr131 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 4
  %struct_arrow132 = load i32, i32* %struct_field_ptr131
  %play_cursor133 = load i32, i32* %play_cursor
  %sub_tmp134 = sub i32 %struct_arrow132, %play_cursor133
  %write_cursor135 = load i32, i32* %write_cursor
  %add_tmp136 = add i32 %sub_tmp134, %write_cursor135
  store i32 %add_tmp136, i32* %delta_cursor
  br label %endif118

endif118:                                         ; preds = %endif129, %endif98
  %delta_cursor137 = load i32, i32* %delta_cursor
  %icmp_tmp138 = icmp sgt i32 %delta_cursor137, 0
  call void @assert(i1 %icmp_tmp138, <{ i32, i8* }> %arr_struct_load142, <{ i32, i8* }> %arr_struct_load147)
  %struct_field_ptr148 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 3
  %struct_arrow149 = load i32, i32* %struct_field_ptr148
  %div_tmp = sdiv i32 %struct_arrow149, 2
  store i32 %div_tmp, i32* %safety_bytes
  %write_cursor150 = load i32, i32* %write_cursor
  store i32 %write_cursor150, i32* %target_cursor
  br label %while_cond

while_cond:                                       ; preds = %while, %endif118
  %target_cursor151 = load i32, i32* %target_cursor
  %write_cursor152 = load i32, i32* %write_cursor
  %delta_cursor153 = load i32, i32* %delta_cursor
  %add_tmp154 = add i32 %write_cursor152, %delta_cursor153
  %icmp_tmp155 = icmp sle i32 %target_cursor151, %add_tmp154
  br i1 %icmp_tmp155, label %while, label %while_end

while:                                            ; preds = %while_cond
  %target_cursor156 = load i32, i32* %target_cursor
  %struct_field_ptr157 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 3
  %struct_arrow158 = load i32, i32* %struct_field_ptr157
  %add_tmp159 = add i32 %target_cursor156, %struct_arrow158
  store i32 %add_tmp159, i32* %target_cursor
  br label %while_cond

while_end:                                        ; preds = %while_cond
  %target_cursor160 = load i32, i32* %target_cursor
  %safety_bytes161 = load i32, i32* %safety_bytes
  %add_tmp162 = add i32 %target_cursor160, %safety_bytes161
  store i32 %add_tmp162, i32* %target_cursor
  %target_cursor163 = load i32, i32* %target_cursor
  %struct_field_ptr164 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 4
  %struct_arrow165 = load i32, i32* %struct_field_ptr164
  %icmp_tmp166 = icmp sgt i32 %target_cursor163, %struct_arrow165
  br i1 %icmp_tmp166, label %then167, label %endif168

then167:                                          ; preds = %while_end
  %target_cursor169 = load i32, i32* %target_cursor
  %struct_field_ptr170 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 4
  %struct_arrow171 = load i32, i32* %struct_field_ptr170
  %sub_tmp172 = sub i32 %target_cursor169, %struct_arrow171
  store i32 %sub_tmp172, i32* %target_cursor
  br label %endif168

endif168:                                         ; preds = %then167, %while_end
  store i32 0, i32* %bytes_to_write
  %target_cursor173 = load i32, i32* %target_cursor
  %play_cursor174 = load i32, i32* %play_cursor
  %icmp_tmp175 = icmp sgt i32 %target_cursor173, %play_cursor174
  br i1 %icmp_tmp175, label %then176, label %endif177

then176:                                          ; preds = %endif168
  %position_to_write178 = load i32, i32* %position_to_write
  %target_cursor179 = load i32, i32* %target_cursor
  %icmp_tmp180 = icmp slt i32 %position_to_write178, %target_cursor179
  br i1 %icmp_tmp180, label %cand.rhs181, label %cand.end182

cand.rhs181:                                      ; preds = %then176
  %position_to_write183 = load i32, i32* %position_to_write
  %play_cursor184 = load i32, i32* %play_cursor
  %icmp_tmp185 = icmp sgt i32 %position_to_write183, %play_cursor184
  br label %cand.end182

cand.end182:                                      ; preds = %cand.rhs181, %then176
  %candphi186 = phi i1 [ false, %then176 ], [ %icmp_tmp185, %cand.rhs181 ]
  br i1 %candphi186, label %then187, label %endif188

then187:                                          ; preds = %cand.end182
  %target_cursor189 = load i32, i32* %target_cursor
  %position_to_write190 = load i32, i32* %position_to_write
  %sub_tmp191 = sub i32 %target_cursor189, %position_to_write190
  store i32 %sub_tmp191, i32* %bytes_to_write
  br label %endif188

endif188:                                         ; preds = %then187, %cand.end182
  br label %endif177

endif177:                                         ; preds = %endif188, %endif168
  %target_cursor192 = load i32, i32* %target_cursor
  %play_cursor193 = load i32, i32* %play_cursor
  %icmp_tmp194 = icmp slt i32 %target_cursor192, %play_cursor193
  br i1 %icmp_tmp194, label %then195, label %endif196

then195:                                          ; preds = %endif177
  %position_to_write197 = load i32, i32* %position_to_write
  %play_cursor198 = load i32, i32* %play_cursor
  %icmp_tmp199 = icmp sgt i32 %position_to_write197, %play_cursor198
  br i1 %icmp_tmp199, label %then200, label %endif201

then200:                                          ; preds = %then195
  %struct_field_ptr202 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 4
  %struct_arrow203 = load i32, i32* %struct_field_ptr202
  %position_to_write204 = load i32, i32* %position_to_write
  %sub_tmp205 = sub i32 %struct_arrow203, %position_to_write204
  store i32 %sub_tmp205, i32* %bytes_to_write
  %bytes_to_write206 = load i32, i32* %bytes_to_write
  %target_cursor207 = load i32, i32* %target_cursor
  %add_tmp208 = add i32 %bytes_to_write206, %target_cursor207
  store i32 %add_tmp208, i32* %bytes_to_write
  br label %endif201

endif201:                                         ; preds = %then200, %then195
  %position_to_write209 = load i32, i32* %position_to_write
  %target_cursor210 = load i32, i32* %target_cursor
  %icmp_tmp211 = icmp slt i32 %position_to_write209, %target_cursor210
  br i1 %icmp_tmp211, label %then212, label %endif213

then212:                                          ; preds = %endif201
  %target_cursor214 = load i32, i32* %target_cursor
  %position_to_write215 = load i32, i32* %position_to_write
  %sub_tmp216 = sub i32 %target_cursor214, %position_to_write215
  store i32 %sub_tmp216, i32* %bytes_to_write
  br label %endif213

endif213:                                         ; preds = %then212, %endif201
  br label %endif196

endif196:                                         ; preds = %endif213, %endif177
  %struct_field_ptr217 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 9
  %position_to_write218 = load i32, i32* %position_to_write
  store i32 %position_to_write218, i32* %struct_field_ptr217
  %struct_field_ptr219 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 10
  %bytes_to_write220 = load i32, i32* %bytes_to_write
  store i32 %bytes_to_write220, i32* %struct_field_ptr219
  %struct_field_ptr221 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 11
  %struct_arrow222 = load i1, i1* %struct_field_ptr221
  %not_tmp223 = xor i1 %struct_arrow222, true
  br i1 %not_tmp223, label %cand.rhs224, label %cand.end225

cand.rhs224:                                      ; preds = %endif196
  %bytes_to_write226 = load i32, i32* %bytes_to_write
  %icmp_tmp227 = icmp sgt i32 %bytes_to_write226, 0
  br label %cand.end225

cand.end225:                                      ; preds = %cand.rhs224, %endif196
  %candphi228 = phi i1 [ false, %endif196 ], [ %icmp_tmp227, %cand.rhs224 ]
  br i1 %candphi228, label %then229, label %endif230

then229:                                          ; preds = %cand.end225
  %struct_field_ptr231 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 11
  store i1 true, i1* %struct_field_ptr231
  br label %endif230

endif230:                                         ; preds = %then229, %cand.end225
  ret void
}

; Function Attrs: nounwind
define internal void @create_backbuffer(<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* %buffer, i32 %target_width, i32 %target_height) #0 {
vars:
  %bitmap_size = alloca i32
  br label %entry

entry:                                            ; preds = %vars
  %struct_field_ptr = getelementptr inbounds <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* %buffer, i32 0, i32 2
  store i32 %target_width, i32* %struct_field_ptr
  %struct_field_ptr1 = getelementptr inbounds <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* %buffer, i32 0, i32 3
  store i32 %target_height, i32* %struct_field_ptr1
  %struct_field_ptr2 = getelementptr inbounds <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* %buffer, i32 0, i32 0
  %struct_field_ptr3 = getelementptr inbounds <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>* %struct_field_ptr2, i32 0, i32 0
  %struct_field_ptr4 = getelementptr inbounds <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>* %struct_field_ptr3, i32 0, i32 0
  store i32 40, i32* %struct_field_ptr4
  %struct_field_ptr5 = getelementptr inbounds <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* %buffer, i32 0, i32 0
  %struct_field_ptr6 = getelementptr inbounds <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>* %struct_field_ptr5, i32 0, i32 0
  %struct_field_ptr7 = getelementptr inbounds <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>* %struct_field_ptr6, i32 0, i32 1
  %struct_field_ptr8 = getelementptr inbounds <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* %buffer, i32 0, i32 2
  %struct_arrow = load i32, i32* %struct_field_ptr8
  store i32 %struct_arrow, i32* %struct_field_ptr7
  %struct_field_ptr9 = getelementptr inbounds <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* %buffer, i32 0, i32 0
  %struct_field_ptr10 = getelementptr inbounds <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>* %struct_field_ptr9, i32 0, i32 0
  %struct_field_ptr11 = getelementptr inbounds <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>* %struct_field_ptr10, i32 0, i32 2
  %struct_field_ptr12 = getelementptr inbounds <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* %buffer, i32 0, i32 3
  %struct_arrow13 = load i32, i32* %struct_field_ptr12
  %neg_tmp = sub i32 0, %struct_arrow13
  store i32 %neg_tmp, i32* %struct_field_ptr11
  %struct_field_ptr14 = getelementptr inbounds <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* %buffer, i32 0, i32 0
  %struct_field_ptr15 = getelementptr inbounds <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>* %struct_field_ptr14, i32 0, i32 0
  %struct_field_ptr16 = getelementptr inbounds <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>* %struct_field_ptr15, i32 0, i32 3
  store i16 1, i16* %struct_field_ptr16
  %struct_field_ptr17 = getelementptr inbounds <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* %buffer, i32 0, i32 0
  %struct_field_ptr18 = getelementptr inbounds <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>* %struct_field_ptr17, i32 0, i32 0
  %struct_field_ptr19 = getelementptr inbounds <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>* %struct_field_ptr18, i32 0, i32 4
  store i16 32, i16* %struct_field_ptr19
  %struct_field_ptr20 = getelementptr inbounds <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* %buffer, i32 0, i32 0
  %struct_field_ptr21 = getelementptr inbounds <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>* %struct_field_ptr20, i32 0, i32 0
  %struct_field_ptr22 = getelementptr inbounds <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>* %struct_field_ptr21, i32 0, i32 5
  store i32 0, i32* %struct_field_ptr22
  %struct_field_ptr23 = getelementptr inbounds <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* %buffer, i32 0, i32 2
  %struct_arrow24 = load i32, i32* %struct_field_ptr23
  %mul_tmp = mul i32 4, %struct_arrow24
  %struct_field_ptr25 = getelementptr inbounds <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* %buffer, i32 0, i32 3
  %struct_arrow26 = load i32, i32* %struct_field_ptr25
  %mul_tmp27 = mul i32 %mul_tmp, %struct_arrow26
  store i32 %mul_tmp27, i32* %bitmap_size
  %struct_field_ptr28 = getelementptr inbounds <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* %buffer, i32 0, i32 1
  %struct_arrow29 = load i8*, i8** %struct_field_ptr28
  %icmp_tmp = icmp ne i8* %struct_arrow29, null
  br i1 %icmp_tmp, label %then, label %endif

then:                                             ; preds = %entry
  %struct_field_ptr30 = getelementptr inbounds <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* %buffer, i32 0, i32 1
  %struct_arrow31 = load i8*, i8** %struct_field_ptr30
  %fun_call = call i32 @VirtualFree(i8* %struct_arrow31, i64 0, i32 32768)
  br label %endif

endif:                                            ; preds = %then, %entry
  %struct_field_ptr32 = getelementptr inbounds <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* %buffer, i32 0, i32 1
  %bitmap_size33 = load i32, i32* %bitmap_size
  %int_cast = zext i32 %bitmap_size33 to i64
  %fun_call34 = call i8* @VirtualAlloc(i8* null, i64 %int_cast, i32 4096, i32 4)
  store i8* %fun_call34, i8** %struct_field_ptr32
  %struct_field_ptr35 = getelementptr inbounds <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* %buffer, i32 0, i32 4
  %struct_field_ptr36 = getelementptr inbounds <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* %buffer, i32 0, i32 2
  %struct_arrow37 = load i32, i32* %struct_field_ptr36
  %mul_tmp38 = mul i32 %struct_arrow37, 4
  store i32 %mul_tmp38, i32* %struct_field_ptr35
  ret void
}

; Function Attrs: nounwind
define internal void @blit_to_screen(<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* %buffer) #0 {
vars:
  br label %entry

entry:                                            ; preds = %vars
  %struct_field = load i64, i64* getelementptr inbounds (<{ i64, i64, i32, i32 }>, <{ i64, i64, i32, i32 }>* @window, i32 0, i32 1)
  %struct_field_ptr = getelementptr inbounds <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* %buffer, i32 0, i32 2
  %struct_arrow = load i32, i32* %struct_field_ptr
  %struct_field_ptr1 = getelementptr inbounds <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* %buffer, i32 0, i32 3
  %struct_arrow2 = load i32, i32* %struct_field_ptr1
  %struct_field_ptr3 = getelementptr inbounds <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* %buffer, i32 0, i32 2
  %struct_arrow4 = load i32, i32* %struct_field_ptr3
  %struct_field_ptr5 = getelementptr inbounds <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* %buffer, i32 0, i32 3
  %struct_arrow6 = load i32, i32* %struct_field_ptr5
  %struct_field_ptr7 = getelementptr inbounds <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* %buffer, i32 0, i32 1
  %struct_arrow8 = load i8*, i8** %struct_field_ptr7
  %struct_field_ptr9 = getelementptr inbounds <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* %buffer, i32 0, i32 0
  %fun_call = call i32 @StretchDIBits(i64 %struct_field, i32 0, i32 0, i32 %struct_arrow, i32 %struct_arrow2, i32 0, i32 0, i32 %struct_arrow4, i32 %struct_arrow6, i8* %struct_arrow8, <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>* %struct_field_ptr9, i32 0, i32 13369376)
  ret void
}

; Function Attrs: nounwind
define internal void @update_window_rect() #0 {
vars:
  %rect = alloca <{ i32, i32, i32, i32 }>
  br label %entry

entry:                                            ; preds = %vars
  %struct_field = load i64, i64* getelementptr inbounds (<{ i64, i64, i32, i32 }>, <{ i64, i64, i32, i32 }>* @window, i32 0, i32 0)
  %fun_call = call i32 @GetClientRect(i64 %struct_field, <{ i32, i32, i32, i32 }>* %rect)
  %struct_field_ptr = getelementptr inbounds <{ i32, i32, i32, i32 }>, <{ i32, i32, i32, i32 }>* %rect, i32 0, i32 2
  %struct_field1 = load i32, i32* %struct_field_ptr
  %struct_field_ptr2 = getelementptr inbounds <{ i32, i32, i32, i32 }>, <{ i32, i32, i32, i32 }>* %rect, i32 0, i32 0
  %struct_field3 = load i32, i32* %struct_field_ptr2
  %sub_tmp = sub i32 %struct_field1, %struct_field3
  store i32 %sub_tmp, i32* getelementptr inbounds (<{ i64, i64, i32, i32 }>, <{ i64, i64, i32, i32 }>* @window, i32 0, i32 2)
  %struct_field_ptr4 = getelementptr inbounds <{ i32, i32, i32, i32 }>, <{ i32, i32, i32, i32 }>* %rect, i32 0, i32 3
  %struct_field5 = load i32, i32* %struct_field_ptr4
  %struct_field_ptr6 = getelementptr inbounds <{ i32, i32, i32, i32 }>, <{ i32, i32, i32, i32 }>* %rect, i32 0, i32 1
  %struct_field7 = load i32, i32* %struct_field_ptr6
  %sub_tmp8 = sub i32 %struct_field5, %struct_field7
  store i32 %sub_tmp8, i32* getelementptr inbounds (<{ i64, i64, i32, i32 }>, <{ i64, i64, i32, i32 }>* @window, i32 0, i32 3)
  ret void
}

; Function Attrs: nounwind
define internal i64 @main_window_callback(i64 %window_handle, i32 %message, i64 %w_param, i64 %l_param) #0 {
vars:
  %result = alloca i64
  %paint = alloca <{ i64, i32, <{ i32, i32, i32, i32 }>, i32, i32, i64, i64, i64, i64 }>
  %context = alloca i64
  br label %entry

entry:                                            ; preds = %vars
  store i64 0, i64* %result
  %icmp_tmp = icmp eq i32 %message, 5
  br i1 %icmp_tmp, label %then, label %elif_0

then:                                             ; preds = %entry
  call void @update_window_rect()
  br label %endif

elif_0:                                           ; preds = %entry
  %icmp_tmp1 = icmp eq i32 %message, 16
  br i1 %icmp_tmp1, label %elif_0_then, label %elif_1

elif_0_then:                                      ; preds = %elif_0
  store i1 true, i1* @window_requests_quit
  br label %endif

elif_1:                                           ; preds = %elif_0
  %icmp_tmp2 = icmp eq i32 %message, 2
  br i1 %icmp_tmp2, label %elif_1_then, label %elif_2

elif_1_then:                                      ; preds = %elif_1
  store i1 true, i1* @window_requests_quit
  br label %endif

elif_2:                                           ; preds = %elif_1
  %icmp_tmp3 = icmp eq i32 %message, 15
  br i1 %icmp_tmp3, label %elif_2_then, label %else

elif_2_then:                                      ; preds = %elif_2
  %fun_call = call i64 @BeginPaint(i64 %window_handle, <{ i64, i32, <{ i32, i32, i32, i32 }>, i32, i32, i64, i64, i64, i64 }>* %paint)
  store i64 %fun_call, i64* %context
  call void @blit_to_screen(<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer)
  %fun_call4 = call i32 @EndPaint(i64 %window_handle, <{ i64, i32, <{ i32, i32, i32, i32 }>, i32, i32, i64, i64, i64, i64 }>* %paint)
  br label %endif

else:                                             ; preds = %elif_2
  %fun_call5 = call i64 @DefWindowProcA(i64 %window_handle, i32 %message, i64 %w_param, i64 %l_param)
  store i64 %fun_call5, i64* %result
  br label %endif

endif:                                            ; preds = %else, %elif_2_then, %elif_1_then, %elif_0_then, %then
  %result6 = load i64, i64* %result
  ret i64 %result6
}

; Function Attrs: nounwind
define internal void @update_game_button(<{ i1, i1, i1 }>* %button, i1 %is_pressed) #0 {
vars:
  br label %entry

entry:                                            ; preds = %vars
  br i1 %is_pressed, label %cand.rhs, label %cand.end

cand.rhs:                                         ; preds = %entry
  %struct_field_ptr = getelementptr inbounds <{ i1, i1, i1 }>, <{ i1, i1, i1 }>* %button, i32 0, i32 0
  %struct_arrow = load i1, i1* %struct_field_ptr
  %not_tmp = xor i1 %struct_arrow, true
  br label %cand.end

cand.end:                                         ; preds = %cand.rhs, %entry
  %candphi = phi i1 [ false, %entry ], [ %not_tmp, %cand.rhs ]
  br i1 %candphi, label %then, label %endif

then:                                             ; preds = %cand.end
  %struct_field_ptr1 = getelementptr inbounds <{ i1, i1, i1 }>, <{ i1, i1, i1 }>* %button, i32 0, i32 1
  store i1 true, i1* %struct_field_ptr1
  br label %endif

endif:                                            ; preds = %then, %cand.end
  %not_tmp2 = xor i1 %is_pressed, true
  br i1 %not_tmp2, label %cand.rhs3, label %cand.end4

cand.rhs3:                                        ; preds = %endif
  %struct_field_ptr5 = getelementptr inbounds <{ i1, i1, i1 }>, <{ i1, i1, i1 }>* %button, i32 0, i32 0
  %struct_arrow6 = load i1, i1* %struct_field_ptr5
  br label %cand.end4

cand.end4:                                        ; preds = %cand.rhs3, %endif
  %candphi7 = phi i1 [ false, %endif ], [ %struct_arrow6, %cand.rhs3 ]
  br i1 %candphi7, label %then8, label %endif9

then8:                                            ; preds = %cand.end4
  %struct_field_ptr10 = getelementptr inbounds <{ i1, i1, i1 }>, <{ i1, i1, i1 }>* %button, i32 0, i32 2
  store i1 true, i1* %struct_field_ptr10
  br label %endif9

endif9:                                           ; preds = %then8, %cand.end4
  %struct_field_ptr11 = getelementptr inbounds <{ i1, i1, i1 }>, <{ i1, i1, i1 }>* %button, i32 0, i32 0
  store i1 %is_pressed, i1* %struct_field_ptr11
  ret void
}

; Function Attrs: nounwind
define internal void @get_mouse_input(<{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %game_input) #0 {
vars:
  %cursor_pos = alloca <{ i32, i32 }>
  br label %entry

entry:                                            ; preds = %vars
  %fun_call = call i32 @GetCursorPos(<{ i32, i32 }>* %cursor_pos)
  %struct_field = load i64, i64* getelementptr inbounds (<{ i64, i64, i32, i32 }>, <{ i64, i64, i32, i32 }>* @window, i32 0, i32 0)
  %fun_call1 = call i32 @ScreenToClient(i64 %struct_field, <{ i32, i32 }>* %cursor_pos)
  %struct_field_ptr = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %game_input, i32 0, i32 8
  %struct_field_ptr2 = getelementptr inbounds <{ i32, i32 }>, <{ i32, i32 }>* %cursor_pos, i32 0, i32 0
  %struct_field3 = load i32, i32* %struct_field_ptr2
  store i32 %struct_field3, i32* %struct_field_ptr
  %struct_field_ptr4 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %game_input, i32 0, i32 9
  %struct_field_ptr5 = getelementptr inbounds <{ i32, i32 }>, <{ i32, i32 }>* %cursor_pos, i32 0, i32 1
  %struct_field6 = load i32, i32* %struct_field_ptr5
  store i32 %struct_field6, i32* %struct_field_ptr4
  %struct_field_ptr7 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %game_input, i32 0, i32 5
  %fun_call8 = call i16 @GetKeyState(i32 1)
  %and_tmp = and i16 %fun_call8, -32768
  %icmp_tmp = icmp ne i16 %and_tmp, 0
  call void @update_game_button(<{ i1, i1, i1 }>* %struct_field_ptr7, i1 %icmp_tmp)
  %struct_field_ptr9 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %game_input, i32 0, i32 6
  %fun_call10 = call i16 @GetKeyState(i32 2)
  %and_tmp11 = and i16 %fun_call10, -32768
  %icmp_tmp12 = icmp ne i16 %and_tmp11, 0
  call void @update_game_button(<{ i1, i1, i1 }>* %struct_field_ptr9, i1 %icmp_tmp12)
  %struct_field_ptr13 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %game_input, i32 0, i32 7
  %fun_call14 = call i16 @GetKeyState(i32 4)
  %and_tmp15 = and i16 %fun_call14, -32768
  %icmp_tmp16 = icmp ne i16 %and_tmp15, 0
  call void @update_game_button(<{ i1, i1, i1 }>* %struct_field_ptr13, i1 %icmp_tmp16)
  ret void
}

; Function Attrs: nounwind
define internal void @process_pending_messages(<{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %game_input, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %win_state) #0 {
vars:
  %msg = alloca <{ i64, i32, i64, i64, i32, <{ i32, i32 }>, i64 }>
  %message = alloca i32
  %w_param = alloca i32
  %l_param = alloca i32
  %vk_code = alloca i32
  %was_down = alloca i1
  %is_down = alloca i1
  %key_up = alloca i1
  %key_down = alloca i1
  %arr_struct_alloca = alloca <{ i32, i8* }>
  %arr_elem_alloca = alloca i8
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca, i8* getelementptr inbounds ([2 x i8], [2 x i8]* @str.97, i32 0, i32 0), i32 1, i32 0, i1 false)
  %gep_arr_elem_ptr = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 0
  store i32 1, i32* %gep_arr_elem_ptr
  %gep_arr_elem_ptr65 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 1
  store i8* %arr_elem_alloca, i8** %gep_arr_elem_ptr65
  %arr_struct_load = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca
  %arr_struct_alloca79 = alloca <{ i32, i8* }>
  %arr_elem_alloca80 = alloca i8
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca80, i8* getelementptr inbounds ([2 x i8], [2 x i8]* @str.98, i32 0, i32 0), i32 1, i32 0, i1 false)
  %gep_arr_elem_ptr81 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca79, i32 0, i32 0
  store i32 1, i32* %gep_arr_elem_ptr81
  %gep_arr_elem_ptr82 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca79, i32 0, i32 1
  store i8* %arr_elem_alloca80, i8** %gep_arr_elem_ptr82
  %arr_struct_load83 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca79
  br label %entry

entry:                                            ; preds = %vars
  br label %while_cond

while_cond:                                       ; preds = %endif, %entry
  %fun_call = call i32 @PeekMessageA(<{ i64, i32, i64, i64, i32, <{ i32, i32 }>, i64 }>* %msg, i64 0, i32 0, i32 0, i32 1)
  %icmp_tmp = icmp ne i32 %fun_call, 0
  br i1 %icmp_tmp, label %while, label %while_end

while:                                            ; preds = %while_cond
  %struct_field_ptr = getelementptr inbounds <{ i64, i32, i64, i64, i32, <{ i32, i32 }>, i64 }>, <{ i64, i32, i64, i64, i32, <{ i32, i32 }>, i64 }>* %msg, i32 0, i32 1
  %struct_field = load i32, i32* %struct_field_ptr
  store i32 %struct_field, i32* %message
  %message1 = load i32, i32* %message
  %icmp_tmp2 = icmp eq i32 %message1, 18
  br i1 %icmp_tmp2, label %then, label %elif_0

then:                                             ; preds = %while
  %struct_field_ptr3 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %game_input, i32 0, i32 11
  store i1 true, i1* %struct_field_ptr3
  br label %endif

elif_0:                                           ; preds = %while
  %message4 = load i32, i32* %message
  %icmp_tmp5 = icmp eq i32 %message4, 260
  br i1 %icmp_tmp5, label %cor.end, label %cor.rhs

cor.rhs:                                          ; preds = %elif_0
  %message6 = load i32, i32* %message
  %icmp_tmp7 = icmp eq i32 %message6, 261
  br label %cor.end

cor.end:                                          ; preds = %cor.rhs, %elif_0
  %corphi = phi i1 [ true, %elif_0 ], [ %icmp_tmp7, %cor.rhs ]
  br i1 %corphi, label %cor.end9, label %cor.rhs8

cor.rhs8:                                         ; preds = %cor.end
  %message10 = load i32, i32* %message
  %icmp_tmp11 = icmp eq i32 %message10, 256
  br label %cor.end9

cor.end9:                                         ; preds = %cor.rhs8, %cor.end
  %corphi12 = phi i1 [ true, %cor.end ], [ %icmp_tmp11, %cor.rhs8 ]
  br i1 %corphi12, label %cor.end14, label %cor.rhs13

cor.rhs13:                                        ; preds = %cor.end9
  %message15 = load i32, i32* %message
  %icmp_tmp16 = icmp eq i32 %message15, 257
  br label %cor.end14

cor.end14:                                        ; preds = %cor.rhs13, %cor.end9
  %corphi17 = phi i1 [ true, %cor.end9 ], [ %icmp_tmp16, %cor.rhs13 ]
  br i1 %corphi17, label %elif_0_then, label %else

elif_0_then:                                      ; preds = %cor.end14
  %struct_field_ptr18 = getelementptr inbounds <{ i64, i32, i64, i64, i32, <{ i32, i32 }>, i64 }>, <{ i64, i32, i64, i64, i32, <{ i32, i32 }>, i64 }>* %msg, i32 0, i32 2
  %struct_field19 = load i64, i64* %struct_field_ptr18
  %shr_tmp = ashr i64 %struct_field19, 32
  %int_trunc = trunc i64 %shr_tmp to i32
  store i32 %int_trunc, i32* %w_param
  %struct_field_ptr20 = getelementptr inbounds <{ i64, i32, i64, i64, i32, <{ i32, i32 }>, i64 }>, <{ i64, i32, i64, i64, i32, <{ i32, i32 }>, i64 }>* %msg, i32 0, i32 3
  %struct_field21 = load i64, i64* %struct_field_ptr20
  %shr_tmp22 = ashr i64 %struct_field21, 32
  %int_trunc23 = trunc i64 %shr_tmp22 to i32
  store i32 %int_trunc23, i32* %l_param
  %w_param24 = load i32, i32* %w_param
  store i32 %w_param24, i32* %vk_code
  %l_param25 = load i32, i32* %l_param
  %and_tmp = and i32 %l_param25, 1073741824
  %icmp_tmp26 = icmp ne i32 %and_tmp, 0
  store i1 %icmp_tmp26, i1* %was_down
  %l_param27 = load i32, i32* %l_param
  %and_tmp28 = and i32 %l_param27, -2147483648
  %icmp_tmp29 = icmp eq i32 %and_tmp28, 0
  store i1 %icmp_tmp29, i1* %is_down
  %was_down30 = load i1, i1* %was_down
  br i1 %was_down30, label %cand.rhs, label %cand.end

cand.rhs:                                         ; preds = %elif_0_then
  %is_down31 = load i1, i1* %is_down
  %not_tmp = xor i1 %is_down31, true
  br label %cand.end

cand.end:                                         ; preds = %cand.rhs, %elif_0_then
  %candphi = phi i1 [ false, %elif_0_then ], [ %not_tmp, %cand.rhs ]
  store i1 %candphi, i1* %key_up
  %was_down32 = load i1, i1* %was_down
  %not_tmp33 = xor i1 %was_down32, true
  br i1 %not_tmp33, label %cand.rhs34, label %cand.end35

cand.rhs34:                                       ; preds = %cand.end
  %is_down36 = load i1, i1* %is_down
  br label %cand.end35

cand.end35:                                       ; preds = %cand.rhs34, %cand.end
  %candphi37 = phi i1 [ false, %cand.end ], [ %is_down36, %cand.rhs34 ]
  store i1 %candphi37, i1* %key_down
  %vk_code38 = load i32, i32* %vk_code
  %icmp_tmp39 = icmp eq i32 %vk_code38, 27
  br i1 %icmp_tmp39, label %then40, label %elif_041

then40:                                           ; preds = %cand.end35
  %struct_field_ptr43 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %game_input, i32 0, i32 11
  store i1 true, i1* %struct_field_ptr43
  br label %endif42

elif_041:                                         ; preds = %cand.end35
  %vk_code45 = load i32, i32* %vk_code
  %icmp_tmp46 = icmp eq i32 %vk_code45, 37
  br i1 %icmp_tmp46, label %elif_0_then44, label %elif_1

elif_0_then44:                                    ; preds = %elif_041
  %struct_field_ptr47 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %game_input, i32 0, i32 2
  %is_down48 = load i1, i1* %is_down
  call void @update_game_button(<{ i1, i1, i1 }>* %struct_field_ptr47, i1 %is_down48)
  br label %endif42

elif_1:                                           ; preds = %elif_041
  %vk_code49 = load i32, i32* %vk_code
  %icmp_tmp50 = icmp eq i32 %vk_code49, 39
  br i1 %icmp_tmp50, label %elif_1_then, label %elif_2

elif_1_then:                                      ; preds = %elif_1
  %struct_field_ptr51 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %game_input, i32 0, i32 3
  %is_down52 = load i1, i1* %is_down
  call void @update_game_button(<{ i1, i1, i1 }>* %struct_field_ptr51, i1 %is_down52)
  br label %endif42

elif_2:                                           ; preds = %elif_1
  %vk_code53 = load i32, i32* %vk_code
  %icmp_tmp54 = icmp eq i32 %vk_code53, 38
  br i1 %icmp_tmp54, label %elif_2_then, label %elif_3

elif_2_then:                                      ; preds = %elif_2
  %struct_field_ptr55 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %game_input, i32 0, i32 1
  %is_down56 = load i1, i1* %is_down
  call void @update_game_button(<{ i1, i1, i1 }>* %struct_field_ptr55, i1 %is_down56)
  br label %endif42

elif_3:                                           ; preds = %elif_2
  %vk_code57 = load i32, i32* %vk_code
  %icmp_tmp58 = icmp eq i32 %vk_code57, 40
  br i1 %icmp_tmp58, label %elif_3_then, label %elif_4

elif_3_then:                                      ; preds = %elif_3
  %struct_field_ptr59 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %game_input, i32 0, i32 4
  %is_down60 = load i1, i1* %is_down
  call void @update_game_button(<{ i1, i1, i1 }>* %struct_field_ptr59, i1 %is_down60)
  br label %endif42

elif_4:                                           ; preds = %elif_3
  %key_down61 = load i1, i1* %key_down
  br i1 %key_down61, label %cand.rhs62, label %cand.end63

cand.rhs62:                                       ; preds = %elif_4
  %vk_code64 = load i32, i32* %vk_code
  %fun_call66 = call i32 @ord(<{ i32, i8* }> %arr_struct_load)
  %icmp_tmp67 = icmp eq i32 %vk_code64, %fun_call66
  br label %cand.end63

cand.end63:                                       ; preds = %cand.rhs62, %elif_4
  %candphi68 = phi i1 [ false, %elif_4 ], [ %icmp_tmp67, %cand.rhs62 ]
  br i1 %candphi68, label %elif_4_then, label %elif_5

elif_4_then:                                      ; preds = %cand.end63
  %key_down69 = load i1, i1* %key_down
  br i1 %key_down69, label %then70, label %endif71

then70:                                           ; preds = %elif_4_then
  %struct_field_ptr72 = getelementptr inbounds <{ i8*, i1, i1, i1, i1, i64, i64 }>, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %win_state, i32 0, i32 1
  %struct_field_ptr73 = getelementptr inbounds <{ i8*, i1, i1, i1, i1, i64, i64 }>, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %win_state, i32 0, i32 1
  %struct_arrow = load i1, i1* %struct_field_ptr73
  %not_tmp74 = xor i1 %struct_arrow, true
  store i1 %not_tmp74, i1* %struct_field_ptr72
  br label %endif71

endif71:                                          ; preds = %then70, %elif_4_then
  br label %endif42

elif_5:                                           ; preds = %cand.end63
  %key_down75 = load i1, i1* %key_down
  br i1 %key_down75, label %cand.rhs76, label %cand.end77

cand.rhs76:                                       ; preds = %elif_5
  %vk_code78 = load i32, i32* %vk_code
  %fun_call84 = call i32 @ord(<{ i32, i8* }> %arr_struct_load83)
  %icmp_tmp85 = icmp eq i32 %vk_code78, %fun_call84
  br label %cand.end77

cand.end77:                                       ; preds = %cand.rhs76, %elif_5
  %candphi86 = phi i1 [ false, %elif_5 ], [ %icmp_tmp85, %cand.rhs76 ]
  br i1 %candphi86, label %elif_5_then, label %endif42

elif_5_then:                                      ; preds = %cand.end77
  %key_down87 = load i1, i1* %key_down
  br i1 %key_down87, label %then88, label %endif89

then88:                                           ; preds = %elif_5_then
  %struct_field_ptr90 = getelementptr inbounds <{ i8*, i1, i1, i1, i1, i64, i64 }>, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %win_state, i32 0, i32 2
  %struct_field_ptr91 = getelementptr inbounds <{ i8*, i1, i1, i1, i1, i64, i64 }>, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %win_state, i32 0, i32 2
  %struct_arrow92 = load i1, i1* %struct_field_ptr91
  %not_tmp93 = xor i1 %struct_arrow92, true
  store i1 %not_tmp93, i1* %struct_field_ptr90
  br label %endif89

endif89:                                          ; preds = %then88, %elif_5_then
  br label %endif42

endif42:                                          ; preds = %endif89, %cand.end77, %endif71, %elif_3_then, %elif_2_then, %elif_1_then, %elif_0_then44, %then40
  br label %endif

else:                                             ; preds = %cor.end14
  %fun_call94 = call i32 @TranslateMessage(<{ i64, i32, i64, i64, i32, <{ i32, i32 }>, i64 }>* %msg)
  %fun_call95 = call i64 @DispatchMessageA(<{ i64, i32, i64, i64, i32, <{ i32, i32 }>, i64 }>* %msg)
  br label %endif

endif:                                            ; preds = %else, %endif42, %then
  br label %while_cond

while_end:                                        ; preds = %while_cond
  ret void
}

; Function Attrs: nounwind
define internal float @get_duration(i64 %c0, i64 %c1) #0 {
vars:
  br label %entry

entry:                                            ; preds = %vars
  %sub_tmp = sub i64 %c0, %c1
  %int_to_float_cast = sitofp i64 %sub_tmp to float
  %perf_count_freq = load i64, i64* @perf_count_freq
  %int_to_float_cast1 = sitofp i64 %perf_count_freq to float
  %fdiv_tmp = fdiv float %int_to_float_cast, %int_to_float_cast1
  ret float %fdiv_tmp
}

; Function Attrs: nounwind
define internal i64 @get_perf_counter() #0 {
vars:
  %result = alloca i64
  br label %entry

entry:                                            ; preds = %vars
  %fun_call = call i32 @QueryPerformanceCounter(i64* %result)
  %result1 = load i64, i64* %result
  ret i64 %result1
}

; Function Attrs: nounwind
define internal void @record_input(<{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %new_input, <{ i8*, i64, i8*, i64, i8* }>* %game_memory, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %win_state) #0 {
vars:
  %arr_struct_alloca = alloca <{ i32, i8* }>
  %arr_elem_alloca = alloca i8, i32 20
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca, i8* getelementptr inbounds ([20 x i8], [20 x i8]* @str.99, i32 0, i32 0), i32 20, i32 0, i1 false)
  %gep_arr_elem_ptr = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 0
  store i32 20, i32* %gep_arr_elem_ptr
  %gep_arr_elem_ptr24 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 1
  store i8* %arr_elem_alloca, i8** %gep_arr_elem_ptr24
  %arr_struct_load = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca
  %arr_struct_alloca25 = alloca <{ i32, i8* }>
  %arr_elem_alloca26 = alloca i8, i32 100
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca26, i8* getelementptr inbounds ([101 x i8], [101 x i8]* @str.100, i32 0, i32 0), i32 100, i32 0, i1 false)
  %gep_arr_elem_ptr27 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca25, i32 0, i32 0
  store i32 100, i32* %gep_arr_elem_ptr27
  %gep_arr_elem_ptr28 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca25, i32 0, i32 1
  store i8* %arr_elem_alloca26, i8** %gep_arr_elem_ptr28
  %arr_struct_load29 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca25
  %bytes_written = alloca i32
  %total_size = alloca i64
  %arr_struct_alloca37 = alloca <{ i32, i8* }>
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.101, i32 0, i32 0), i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr38 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca37, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr38
  %gep_arr_elem_ptr39 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca37, i32 0, i32 1
  store i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.101, i32 0, i32 0), i8** %gep_arr_elem_ptr39
  %arr_struct_load40 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca37
  %arr_struct_alloca41 = alloca <{ i32, i8* }>
  %arr_elem_alloca42 = alloca i8, i32 100
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca42, i8* getelementptr inbounds ([101 x i8], [101 x i8]* @str.102, i32 0, i32 0), i32 100, i32 0, i1 false)
  %gep_arr_elem_ptr43 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca41, i32 0, i32 0
  store i32 100, i32* %gep_arr_elem_ptr43
  %gep_arr_elem_ptr44 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca41, i32 0, i32 1
  store i8* %arr_elem_alloca42, i8** %gep_arr_elem_ptr44
  %arr_struct_load45 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca41
  %arr_struct_alloca73 = alloca <{ i32, i8* }>
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.103, i32 0, i32 0), i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr74 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca73, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr74
  %gep_arr_elem_ptr75 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca73, i32 0, i32 1
  store i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.103, i32 0, i32 0), i8** %gep_arr_elem_ptr75
  %arr_struct_load76 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca73
  %arr_struct_alloca77 = alloca <{ i32, i8* }>
  %arr_elem_alloca78 = alloca i8, i32 100
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca78, i8* getelementptr inbounds ([101 x i8], [101 x i8]* @str.104, i32 0, i32 0), i32 100, i32 0, i1 false)
  %gep_arr_elem_ptr79 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca77, i32 0, i32 0
  store i32 100, i32* %gep_arr_elem_ptr79
  %gep_arr_elem_ptr80 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca77, i32 0, i32 1
  store i8* %arr_elem_alloca78, i8** %gep_arr_elem_ptr80
  %arr_struct_load81 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca77
  %bytes_written82 = alloca i32
  br label %entry

entry:                                            ; preds = %vars
  %struct_field_ptr = getelementptr inbounds <{ i8*, i1, i1, i1, i1, i64, i64 }>, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %win_state, i32 0, i32 3
  %struct_arrow = load i1, i1* %struct_field_ptr
  br i1 %struct_arrow, label %cand.rhs, label %cand.end

cand.rhs:                                         ; preds = %entry
  %struct_field_ptr1 = getelementptr inbounds <{ i8*, i1, i1, i1, i1, i64, i64 }>, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %win_state, i32 0, i32 1
  %struct_arrow2 = load i1, i1* %struct_field_ptr1
  br label %cand.end

cand.end:                                         ; preds = %cand.rhs, %entry
  %candphi = phi i1 [ false, %entry ], [ %struct_arrow2, %cand.rhs ]
  br i1 %candphi, label %cand.rhs3, label %cand.end4

cand.rhs3:                                        ; preds = %cand.end
  %struct_field_ptr5 = getelementptr inbounds <{ i8*, i1, i1, i1, i1, i64, i64 }>, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %win_state, i32 0, i32 2
  %struct_arrow6 = load i1, i1* %struct_field_ptr5
  br label %cand.end4

cand.end4:                                        ; preds = %cand.rhs3, %cand.end
  %candphi7 = phi i1 [ false, %cand.end ], [ %struct_arrow6, %cand.rhs3 ]
  br i1 %candphi7, label %then, label %endif

then:                                             ; preds = %cand.end4
  %struct_field_ptr8 = getelementptr inbounds <{ i8*, i1, i1, i1, i1, i64, i64 }>, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %win_state, i32 0, i32 1
  store i1 false, i1* %struct_field_ptr8
  br label %endif

endif:                                            ; preds = %then, %cand.end4
  %struct_field_ptr9 = getelementptr inbounds <{ i8*, i1, i1, i1, i1, i64, i64 }>, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %win_state, i32 0, i32 1
  %struct_arrow10 = load i1, i1* %struct_field_ptr9
  br i1 %struct_arrow10, label %cand.rhs11, label %cand.end12

cand.rhs11:                                       ; preds = %endif
  %struct_field_ptr13 = getelementptr inbounds <{ i8*, i1, i1, i1, i1, i64, i64 }>, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %win_state, i32 0, i32 3
  %struct_arrow14 = load i1, i1* %struct_field_ptr13
  %not_tmp = xor i1 %struct_arrow14, true
  br label %cand.end12

cand.end12:                                       ; preds = %cand.rhs11, %endif
  %candphi15 = phi i1 [ false, %endif ], [ %not_tmp, %cand.rhs11 ]
  br i1 %candphi15, label %then16, label %endif17

then16:                                           ; preds = %cand.end12
  %struct_field_ptr18 = getelementptr inbounds <{ i8*, i1, i1, i1, i1, i64, i64 }>, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %win_state, i32 0, i32 2
  %struct_arrow19 = load i1, i1* %struct_field_ptr18
  br i1 %struct_arrow19, label %then20, label %endif21

then20:                                           ; preds = %then16
  ret void

endif21:                                          ; preds = %then16
  %struct_field_ptr22 = getelementptr inbounds <{ i8*, i1, i1, i1, i1, i64, i64 }>, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %win_state, i32 0, i32 3
  store i1 true, i1* %struct_field_ptr22
  %struct_field_ptr23 = getelementptr inbounds <{ i8*, i1, i1, i1, i1, i64, i64 }>, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %win_state, i32 0, i32 5
  %fun_call = call i8* @cstr(<{ i32, i8* }> %arr_struct_load, <{ i32, i8* }> %arr_struct_load29)
  %fun_call30 = call i64 @CreateFileA(i8* %fun_call, i32 1073741824, i32 0, i8* null, i32 2, i32 0, i64 0)
  store i64 %fun_call30, i64* %struct_field_ptr23
  store i32 0, i32* %bytes_written
  %struct_field_ptr31 = getelementptr inbounds <{ i8*, i64, i8*, i64, i8* }>, <{ i8*, i64, i8*, i64, i8* }>* %game_memory, i32 0, i32 1
  %struct_arrow32 = load i64, i64* %struct_field_ptr31
  %struct_field_ptr33 = getelementptr inbounds <{ i8*, i64, i8*, i64, i8* }>, <{ i8*, i64, i8*, i64, i8* }>* %game_memory, i32 0, i32 3
  %struct_arrow34 = load i64, i64* %struct_field_ptr33
  %add_tmp = add i64 %struct_arrow32, %struct_arrow34
  store i64 %add_tmp, i64* %total_size
  %total_size35 = load i64, i64* %total_size
  %fun_call36 = call i64 @gigabytes(i64 4)
  %icmp_tmp = icmp slt i64 %total_size35, %fun_call36
  call void @assert(i1 %icmp_tmp, <{ i32, i8* }> %arr_struct_load40, <{ i32, i8* }> %arr_struct_load45)
  %struct_field_ptr46 = getelementptr inbounds <{ i8*, i1, i1, i1, i1, i64, i64 }>, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %win_state, i32 0, i32 5
  %struct_arrow47 = load i64, i64* %struct_field_ptr46
  %struct_field_ptr48 = getelementptr inbounds <{ i8*, i64, i8*, i64, i8* }>, <{ i8*, i64, i8*, i64, i8* }>* %game_memory, i32 0, i32 2
  %struct_arrow49 = load i8*, i8** %struct_field_ptr48
  %total_size50 = load i64, i64* %total_size
  %int_trunc = trunc i64 %total_size50 to i32
  %fun_call51 = call i32 @WriteFile(i64 %struct_arrow47, i8* %struct_arrow49, i32 %int_trunc, i32* %bytes_written, i8* null)
  br label %endif17

endif17:                                          ; preds = %endif21, %cand.end12
  %struct_field_ptr52 = getelementptr inbounds <{ i8*, i1, i1, i1, i1, i64, i64 }>, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %win_state, i32 0, i32 1
  %struct_arrow53 = load i1, i1* %struct_field_ptr52
  %not_tmp54 = xor i1 %struct_arrow53, true
  br i1 %not_tmp54, label %cand.rhs55, label %cand.end56

cand.rhs55:                                       ; preds = %endif17
  %struct_field_ptr57 = getelementptr inbounds <{ i8*, i1, i1, i1, i1, i64, i64 }>, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %win_state, i32 0, i32 3
  %struct_arrow58 = load i1, i1* %struct_field_ptr57
  br label %cand.end56

cand.end56:                                       ; preds = %cand.rhs55, %endif17
  %candphi59 = phi i1 [ false, %endif17 ], [ %struct_arrow58, %cand.rhs55 ]
  br i1 %candphi59, label %then60, label %endif61

then60:                                           ; preds = %cand.end56
  %struct_field_ptr62 = getelementptr inbounds <{ i8*, i1, i1, i1, i1, i64, i64 }>, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %win_state, i32 0, i32 5
  %struct_arrow63 = load i64, i64* %struct_field_ptr62
  %fun_call64 = call i32 @CloseHandle(i64 %struct_arrow63)
  %struct_field_ptr65 = getelementptr inbounds <{ i8*, i1, i1, i1, i1, i64, i64 }>, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %win_state, i32 0, i32 3
  store i1 false, i1* %struct_field_ptr65
  br label %endif61

endif61:                                          ; preds = %then60, %cand.end56
  %struct_field_ptr66 = getelementptr inbounds <{ i8*, i1, i1, i1, i1, i64, i64 }>, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %win_state, i32 0, i32 1
  %struct_arrow67 = load i1, i1* %struct_field_ptr66
  br i1 %struct_arrow67, label %then68, label %endif69

then68:                                           ; preds = %endif61
  %struct_field_ptr70 = getelementptr inbounds <{ i8*, i1, i1, i1, i1, i64, i64 }>, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %win_state, i32 0, i32 2
  %struct_arrow71 = load i1, i1* %struct_field_ptr70
  %not_tmp72 = xor i1 %struct_arrow71, true
  call void @assert(i1 %not_tmp72, <{ i32, i8* }> %arr_struct_load76, <{ i32, i8* }> %arr_struct_load81)
  store i32 0, i32* %bytes_written82
  %struct_field_ptr83 = getelementptr inbounds <{ i8*, i1, i1, i1, i1, i64, i64 }>, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %win_state, i32 0, i32 5
  %struct_arrow84 = load i64, i64* %struct_field_ptr83
  %pointer_bit_cast = bitcast <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %new_input to i8*
  %fun_call85 = call i32 @WriteFile(i64 %struct_arrow84, i8* %pointer_bit_cast, i32 ptrtoint (<{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* getelementptr (<{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* null, i32 1) to i32), i32* %bytes_written82, i8* null)
  br label %endif69

endif69:                                          ; preds = %then68, %endif61
  ret void
}

; Function Attrs: nounwind
define internal void @replay_input(<{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %new_input, <{ i8*, i64, i8*, i64, i8* }>* %game_memory, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %win_state) #0 {
vars:
  %arr_struct_alloca = alloca <{ i32, i8* }>
  %arr_elem_alloca = alloca i8, i32 20
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca, i8* getelementptr inbounds ([20 x i8], [20 x i8]* @str.99, i32 0, i32 0), i32 20, i32 0, i1 false)
  %gep_arr_elem_ptr = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 0
  store i32 20, i32* %gep_arr_elem_ptr
  %gep_arr_elem_ptr24 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 1
  store i8* %arr_elem_alloca, i8** %gep_arr_elem_ptr24
  %arr_struct_load = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca
  %arr_struct_alloca25 = alloca <{ i32, i8* }>
  %arr_elem_alloca26 = alloca i8, i32 100
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca26, i8* getelementptr inbounds ([101 x i8], [101 x i8]* @str.105, i32 0, i32 0), i32 100, i32 0, i1 false)
  %gep_arr_elem_ptr27 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca25, i32 0, i32 0
  store i32 100, i32* %gep_arr_elem_ptr27
  %gep_arr_elem_ptr28 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca25, i32 0, i32 1
  store i8* %arr_elem_alloca26, i8** %gep_arr_elem_ptr28
  %arr_struct_load29 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca25
  %bytes_read = alloca i32
  %total_size = alloca i64
  %arr_struct_alloca37 = alloca <{ i32, i8* }>
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.106, i32 0, i32 0), i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr38 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca37, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr38
  %gep_arr_elem_ptr39 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca37, i32 0, i32 1
  store i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.106, i32 0, i32 0), i8** %gep_arr_elem_ptr39
  %arr_struct_load40 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca37
  %arr_struct_alloca41 = alloca <{ i32, i8* }>
  %arr_elem_alloca42 = alloca i8, i32 100
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca42, i8* getelementptr inbounds ([101 x i8], [101 x i8]* @str.107, i32 0, i32 0), i32 100, i32 0, i1 false)
  %gep_arr_elem_ptr43 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca41, i32 0, i32 0
  store i32 100, i32* %gep_arr_elem_ptr43
  %gep_arr_elem_ptr44 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca41, i32 0, i32 1
  store i8* %arr_elem_alloca42, i8** %gep_arr_elem_ptr44
  %arr_struct_load45 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca41
  %struct_alloca = alloca <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>
  %arr_struct_alloca73 = alloca <{ i32, i8* }>
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.108, i32 0, i32 0), i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr74 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca73, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr74
  %gep_arr_elem_ptr75 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca73, i32 0, i32 1
  store i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.108, i32 0, i32 0), i8** %gep_arr_elem_ptr75
  %arr_struct_load76 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca73
  %arr_struct_alloca77 = alloca <{ i32, i8* }>
  %arr_elem_alloca78 = alloca i8, i32 100
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca78, i8* getelementptr inbounds ([101 x i8], [101 x i8]* @str.109, i32 0, i32 0), i32 100, i32 0, i1 false)
  %gep_arr_elem_ptr79 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca77, i32 0, i32 0
  store i32 100, i32* %gep_arr_elem_ptr79
  %gep_arr_elem_ptr80 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca77, i32 0, i32 1
  store i8* %arr_elem_alloca78, i8** %gep_arr_elem_ptr80
  %arr_struct_load81 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca77
  %bytes_read82 = alloca i32
  br label %entry

entry:                                            ; preds = %vars
  %struct_field_ptr = getelementptr inbounds <{ i8*, i1, i1, i1, i1, i64, i64 }>, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %win_state, i32 0, i32 4
  %struct_arrow = load i1, i1* %struct_field_ptr
  br i1 %struct_arrow, label %cand.rhs, label %cand.end

cand.rhs:                                         ; preds = %entry
  %struct_field_ptr1 = getelementptr inbounds <{ i8*, i1, i1, i1, i1, i64, i64 }>, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %win_state, i32 0, i32 2
  %struct_arrow2 = load i1, i1* %struct_field_ptr1
  br label %cand.end

cand.end:                                         ; preds = %cand.rhs, %entry
  %candphi = phi i1 [ false, %entry ], [ %struct_arrow2, %cand.rhs ]
  br i1 %candphi, label %cand.rhs3, label %cand.end4

cand.rhs3:                                        ; preds = %cand.end
  %struct_field_ptr5 = getelementptr inbounds <{ i8*, i1, i1, i1, i1, i64, i64 }>, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %win_state, i32 0, i32 1
  %struct_arrow6 = load i1, i1* %struct_field_ptr5
  br label %cand.end4

cand.end4:                                        ; preds = %cand.rhs3, %cand.end
  %candphi7 = phi i1 [ false, %cand.end ], [ %struct_arrow6, %cand.rhs3 ]
  br i1 %candphi7, label %then, label %endif

then:                                             ; preds = %cand.end4
  %struct_field_ptr8 = getelementptr inbounds <{ i8*, i1, i1, i1, i1, i64, i64 }>, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %win_state, i32 0, i32 2
  store i1 false, i1* %struct_field_ptr8
  br label %endif

endif:                                            ; preds = %then, %cand.end4
  %struct_field_ptr9 = getelementptr inbounds <{ i8*, i1, i1, i1, i1, i64, i64 }>, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %win_state, i32 0, i32 2
  %struct_arrow10 = load i1, i1* %struct_field_ptr9
  br i1 %struct_arrow10, label %cand.rhs11, label %cand.end12

cand.rhs11:                                       ; preds = %endif
  %struct_field_ptr13 = getelementptr inbounds <{ i8*, i1, i1, i1, i1, i64, i64 }>, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %win_state, i32 0, i32 4
  %struct_arrow14 = load i1, i1* %struct_field_ptr13
  %not_tmp = xor i1 %struct_arrow14, true
  br label %cand.end12

cand.end12:                                       ; preds = %cand.rhs11, %endif
  %candphi15 = phi i1 [ false, %endif ], [ %not_tmp, %cand.rhs11 ]
  br i1 %candphi15, label %then16, label %endif17

then16:                                           ; preds = %cand.end12
  %struct_field_ptr18 = getelementptr inbounds <{ i8*, i1, i1, i1, i1, i64, i64 }>, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %win_state, i32 0, i32 1
  %struct_arrow19 = load i1, i1* %struct_field_ptr18
  br i1 %struct_arrow19, label %then20, label %endif21

then20:                                           ; preds = %then16
  ret void

endif21:                                          ; preds = %then16
  %struct_field_ptr22 = getelementptr inbounds <{ i8*, i1, i1, i1, i1, i64, i64 }>, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %win_state, i32 0, i32 4
  store i1 true, i1* %struct_field_ptr22
  %struct_field_ptr23 = getelementptr inbounds <{ i8*, i1, i1, i1, i1, i64, i64 }>, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %win_state, i32 0, i32 6
  %fun_call = call i8* @cstr(<{ i32, i8* }> %arr_struct_load, <{ i32, i8* }> %arr_struct_load29)
  %fun_call30 = call i64 @CreateFileA(i8* %fun_call, i32 -2147483648, i32 1, i8* null, i32 3, i32 0, i64 0)
  store i64 %fun_call30, i64* %struct_field_ptr23
  store i32 0, i32* %bytes_read
  %struct_field_ptr31 = getelementptr inbounds <{ i8*, i64, i8*, i64, i8* }>, <{ i8*, i64, i8*, i64, i8* }>* %game_memory, i32 0, i32 1
  %struct_arrow32 = load i64, i64* %struct_field_ptr31
  %struct_field_ptr33 = getelementptr inbounds <{ i8*, i64, i8*, i64, i8* }>, <{ i8*, i64, i8*, i64, i8* }>* %game_memory, i32 0, i32 3
  %struct_arrow34 = load i64, i64* %struct_field_ptr33
  %add_tmp = add i64 %struct_arrow32, %struct_arrow34
  store i64 %add_tmp, i64* %total_size
  %total_size35 = load i64, i64* %total_size
  %fun_call36 = call i64 @gigabytes(i64 4)
  %icmp_tmp = icmp slt i64 %total_size35, %fun_call36
  call void @assert(i1 %icmp_tmp, <{ i32, i8* }> %arr_struct_load40, <{ i32, i8* }> %arr_struct_load45)
  %struct_field_ptr46 = getelementptr inbounds <{ i8*, i1, i1, i1, i1, i64, i64 }>, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %win_state, i32 0, i32 6
  %struct_arrow47 = load i64, i64* %struct_field_ptr46
  %struct_field_ptr48 = getelementptr inbounds <{ i8*, i64, i8*, i64, i8* }>, <{ i8*, i64, i8*, i64, i8* }>* %game_memory, i32 0, i32 2
  %struct_arrow49 = load i8*, i8** %struct_field_ptr48
  %total_size50 = load i64, i64* %total_size
  %int_trunc = trunc i64 %total_size50 to i32
  %fun_call51 = call i32 @ReadFile(i64 %struct_arrow47, i8* %struct_arrow49, i32 %int_trunc, i32* %bytes_read, i8* null)
  br label %endif17

endif17:                                          ; preds = %endif21, %cand.end12
  %struct_field_ptr52 = getelementptr inbounds <{ i8*, i1, i1, i1, i1, i64, i64 }>, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %win_state, i32 0, i32 2
  %struct_arrow53 = load i1, i1* %struct_field_ptr52
  %not_tmp54 = xor i1 %struct_arrow53, true
  br i1 %not_tmp54, label %cand.rhs55, label %cand.end56

cand.rhs55:                                       ; preds = %endif17
  %struct_field_ptr57 = getelementptr inbounds <{ i8*, i1, i1, i1, i1, i64, i64 }>, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %win_state, i32 0, i32 4
  %struct_arrow58 = load i1, i1* %struct_field_ptr57
  br label %cand.end56

cand.end56:                                       ; preds = %cand.rhs55, %endif17
  %candphi59 = phi i1 [ false, %endif17 ], [ %struct_arrow58, %cand.rhs55 ]
  br i1 %candphi59, label %then60, label %endif61

then60:                                           ; preds = %cand.end56
  %struct_field_ptr62 = getelementptr inbounds <{ i8*, i1, i1, i1, i1, i64, i64 }>, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %win_state, i32 0, i32 6
  %struct_arrow63 = load i64, i64* %struct_field_ptr62
  %fun_call64 = call i32 @CloseHandle(i64 %struct_arrow63)
  %struct_field_ptr65 = getelementptr inbounds <{ i8*, i1, i1, i1, i1, i64, i64 }>, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %win_state, i32 0, i32 4
  store i1 false, i1* %struct_field_ptr65
  %struct_arg_0 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca, i32 0, i32 0
  store <{ float, float }> zeroinitializer, <{ float, float }>* %struct_arg_0
  %struct_arg_1 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca, i32 0, i32 1
  store <{ i1, i1, i1 }> zeroinitializer, <{ i1, i1, i1 }>* %struct_arg_1
  %struct_arg_2 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca, i32 0, i32 2
  store <{ i1, i1, i1 }> zeroinitializer, <{ i1, i1, i1 }>* %struct_arg_2
  %struct_arg_3 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca, i32 0, i32 3
  store <{ i1, i1, i1 }> zeroinitializer, <{ i1, i1, i1 }>* %struct_arg_3
  %struct_arg_4 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca, i32 0, i32 4
  store <{ i1, i1, i1 }> zeroinitializer, <{ i1, i1, i1 }>* %struct_arg_4
  %struct_arg_5 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca, i32 0, i32 5
  store <{ i1, i1, i1 }> zeroinitializer, <{ i1, i1, i1 }>* %struct_arg_5
  %struct_arg_6 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca, i32 0, i32 6
  store <{ i1, i1, i1 }> zeroinitializer, <{ i1, i1, i1 }>* %struct_arg_6
  %struct_arg_7 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca, i32 0, i32 7
  store <{ i1, i1, i1 }> zeroinitializer, <{ i1, i1, i1 }>* %struct_arg_7
  %struct_arg_8 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca, i32 0, i32 8
  store i32 0, i32* %struct_arg_8
  %struct_arg_9 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca, i32 0, i32 9
  store i32 0, i32* %struct_arg_9
  %struct_arg_10 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca, i32 0, i32 10
  store i32 0, i32* %struct_arg_10
  %struct_arg_11 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca, i32 0, i32 11
  store i1 false, i1* %struct_arg_11
  %cleared_input = load <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca
  store <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }> %cleared_input, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %new_input
  br label %endif61

endif61:                                          ; preds = %then60, %cand.end56
  %struct_field_ptr66 = getelementptr inbounds <{ i8*, i1, i1, i1, i1, i64, i64 }>, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %win_state, i32 0, i32 2
  %struct_arrow67 = load i1, i1* %struct_field_ptr66
  br i1 %struct_arrow67, label %then68, label %endif69

then68:                                           ; preds = %endif61
  %struct_field_ptr70 = getelementptr inbounds <{ i8*, i1, i1, i1, i1, i64, i64 }>, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %win_state, i32 0, i32 1
  %struct_arrow71 = load i1, i1* %struct_field_ptr70
  %not_tmp72 = xor i1 %struct_arrow71, true
  call void @assert(i1 %not_tmp72, <{ i32, i8* }> %arr_struct_load76, <{ i32, i8* }> %arr_struct_load81)
  store i32 0, i32* %bytes_read82
  %struct_field_ptr83 = getelementptr inbounds <{ i8*, i1, i1, i1, i1, i64, i64 }>, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %win_state, i32 0, i32 6
  %struct_arrow84 = load i64, i64* %struct_field_ptr83
  %pointer_bit_cast = bitcast <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %new_input to i8*
  %fun_call85 = call i32 @ReadFile(i64 %struct_arrow84, i8* %pointer_bit_cast, i32 ptrtoint (<{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* getelementptr (<{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* null, i32 1) to i32), i32* %bytes_read82, i8* null)
  %bytes_read86 = load i32, i32* %bytes_read82
  %icmp_tmp87 = icmp eq i32 %bytes_read86, 0
  br i1 %icmp_tmp87, label %then88, label %endif89

then88:                                           ; preds = %then68
  %struct_field_ptr90 = getelementptr inbounds <{ i8*, i1, i1, i1, i1, i64, i64 }>, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %win_state, i32 0, i32 6
  %struct_arrow91 = load i64, i64* %struct_field_ptr90
  %fun_call92 = call i32 @CloseHandle(i64 %struct_arrow91)
  %struct_field_ptr93 = getelementptr inbounds <{ i8*, i1, i1, i1, i1, i64, i64 }>, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %win_state, i32 0, i32 4
  store i1 false, i1* %struct_field_ptr93
  br label %endif89

endif89:                                          ; preds = %then88, %then68
  br label %endif69

endif69:                                          ; preds = %endif89, %endif61
  ret void
}

; Function Attrs: nounwind
define internal void @main() #0 {
vars:
  %error = alloca i32
  %arr_struct_alloca = alloca <{ i32, i8* }>
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.110, i32 0, i32 0), i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr
  %gep_arr_elem_ptr3 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 1
  store i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.110, i32 0, i32 0), i8** %gep_arr_elem_ptr3
  %arr_struct_load = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca
  %arr_struct_alloca4 = alloca <{ i32, i8* }>
  %arr_elem_alloca = alloca i8, i32 99
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca, i8* getelementptr inbounds ([100 x i8], [100 x i8]* @str.111, i32 0, i32 0), i32 99, i32 0, i1 false)
  %gep_arr_elem_ptr5 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca4, i32 0, i32 0
  store i32 99, i32* %gep_arr_elem_ptr5
  %gep_arr_elem_ptr6 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca4, i32 0, i32 1
  store i8* %arr_elem_alloca, i8** %gep_arr_elem_ptr6
  %arr_struct_load7 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca4
  %arr_struct_alloca9 = alloca <{ i32, i8* }>
  %arr_elem_alloca10 = alloca i8, i32 24
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca10, i8* getelementptr inbounds ([24 x i8], [24 x i8]* @str.112, i32 0, i32 0), i32 24, i32 0, i1 false)
  %gep_arr_elem_ptr11 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca9, i32 0, i32 0
  store i32 24, i32* %gep_arr_elem_ptr11
  %gep_arr_elem_ptr12 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca9, i32 0, i32 1
  store i8* %arr_elem_alloca10, i8** %gep_arr_elem_ptr12
  %arr_struct_load13 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca9
  %arr_struct_alloca14 = alloca <{ i32, i8* }>
  %arr_elem_alloca15 = alloca i8, i32 100
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca15, i8* getelementptr inbounds ([101 x i8], [101 x i8]* @str.113, i32 0, i32 0), i32 100, i32 0, i1 false)
  %gep_arr_elem_ptr16 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca14, i32 0, i32 0
  store i32 100, i32* %gep_arr_elem_ptr16
  %gep_arr_elem_ptr17 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca14, i32 0, i32 1
  store i8* %arr_elem_alloca15, i8** %gep_arr_elem_ptr17
  %arr_struct_load18 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca14
  %class_name = alloca i8*
  %arr_struct_alloca20 = alloca <{ i32, i8* }>
  %arr_elem_alloca21 = alloca i8, i32 22
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca21, i8* getelementptr inbounds ([22 x i8], [22 x i8]* @str.114, i32 0, i32 0), i32 22, i32 0, i1 false)
  %gep_arr_elem_ptr22 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca20, i32 0, i32 0
  store i32 22, i32* %gep_arr_elem_ptr22
  %gep_arr_elem_ptr23 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca20, i32 0, i32 1
  store i8* %arr_elem_alloca21, i8** %gep_arr_elem_ptr23
  %arr_struct_load24 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca20
  %arr_struct_alloca25 = alloca <{ i32, i8* }>
  %arr_elem_alloca26 = alloca i8, i32 100
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca26, i8* getelementptr inbounds ([101 x i8], [101 x i8]* @str.115, i32 0, i32 0), i32 100, i32 0, i1 false)
  %gep_arr_elem_ptr27 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca25, i32 0, i32 0
  store i32 100, i32* %gep_arr_elem_ptr27
  %gep_arr_elem_ptr28 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca25, i32 0, i32 1
  store i8* %arr_elem_alloca26, i8** %gep_arr_elem_ptr28
  %arr_struct_load29 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca25
  %window_name = alloca i8*
  %instance = alloca i64
  %struct_alloca = alloca <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>
  %monitor_hz = alloca i32
  %game_target_fps = alloca i32
  %target_sec_per_frame = alloca float
  %sound_output = alloca <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>
  %sound_buffer = alloca <{ i8* }>*
  %game_sample_buffer = alloca i16*
  %arr_struct_alloca78 = alloca <{ i32, i8* }>
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.116, i32 0, i32 0), i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr79 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca78, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr79
  %gep_arr_elem_ptr80 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca78, i32 0, i32 1
  store i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.116, i32 0, i32 0), i8** %gep_arr_elem_ptr80
  %arr_struct_load81 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca78
  %arr_struct_alloca82 = alloca <{ i32, i8* }>
  %arr_elem_alloca83 = alloca i8, i32 99
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca83, i8* getelementptr inbounds ([100 x i8], [100 x i8]* @str.117, i32 0, i32 0), i32 99, i32 0, i1 false)
  %gep_arr_elem_ptr84 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca82, i32 0, i32 0
  store i32 99, i32* %gep_arr_elem_ptr84
  %gep_arr_elem_ptr85 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca82, i32 0, i32 1
  store i8* %arr_elem_alloca83, i8** %gep_arr_elem_ptr85
  %arr_struct_load86 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca82
  %base_address = alloca i8*
  %struct_alloca88 = alloca <{ i1 (<{ i32, i8* }>, <{ i32, i8* }>)*, <{ i32, i8* }> (<{ i32, i8* }>)*, void (i8*)* }>
  %struct_alloca95 = alloca <{ i8*, i64, i8*, i64, i8* }>
  %total_size = alloca i64
  %arr_struct_alloca115 = alloca <{ i32, i8* }>
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.118, i32 0, i32 0), i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr116 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca115, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr116
  %gep_arr_elem_ptr117 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca115, i32 0, i32 1
  store i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.118, i32 0, i32 0), i8** %gep_arr_elem_ptr117
  %arr_struct_load118 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca115
  %arr_struct_alloca119 = alloca <{ i32, i8* }>
  %arr_elem_alloca120 = alloca i8, i32 99
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca120, i8* getelementptr inbounds ([100 x i8], [100 x i8]* @str.119, i32 0, i32 0), i32 99, i32 0, i1 false)
  %gep_arr_elem_ptr121 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca119, i32 0, i32 0
  store i32 99, i32* %gep_arr_elem_ptr121
  %gep_arr_elem_ptr122 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca119, i32 0, i32 1
  store i8* %arr_elem_alloca120, i8** %gep_arr_elem_ptr122
  %arr_struct_load123 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca119
  %arr_struct_alloca132 = alloca <{ i32, i8* }>
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.120, i32 0, i32 0), i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr133 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca132, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr133
  %gep_arr_elem_ptr134 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca132, i32 0, i32 1
  store i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.120, i32 0, i32 0), i8** %gep_arr_elem_ptr134
  %arr_struct_load135 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca132
  %arr_struct_alloca136 = alloca <{ i32, i8* }>
  %arr_elem_alloca137 = alloca i8, i32 99
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca137, i8* getelementptr inbounds ([100 x i8], [100 x i8]* @str.121, i32 0, i32 0), i32 99, i32 0, i1 false)
  %gep_arr_elem_ptr138 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca136, i32 0, i32 0
  store i32 99, i32* %gep_arr_elem_ptr138
  %gep_arr_elem_ptr139 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca136, i32 0, i32 1
  store i8* %arr_elem_alloca137, i8** %gep_arr_elem_ptr139
  %arr_struct_load140 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca136
  %struct_alloca143 = alloca <{ i8*, i1, i1, i1, i1, i64, i64 }>
  %game = alloca <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>
  %arr_struct_alloca161 = alloca <{ i32, i8* }>
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.122, i32 0, i32 0), i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr162 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca161, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr162
  %gep_arr_elem_ptr163 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca161, i32 0, i32 1
  store i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.122, i32 0, i32 0), i8** %gep_arr_elem_ptr163
  %arr_struct_load164 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca161
  %arr_struct_alloca165 = alloca <{ i32, i8* }>
  %arr_elem_alloca166 = alloca i8, i32 99
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca166, i8* getelementptr inbounds ([100 x i8], [100 x i8]* @str.123, i32 0, i32 0), i32 99, i32 0, i1 false)
  %gep_arr_elem_ptr167 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca165, i32 0, i32 0
  store i32 99, i32* %gep_arr_elem_ptr167
  %gep_arr_elem_ptr168 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca165, i32 0, i32 1
  store i8* %arr_elem_alloca166, i8** %gep_arr_elem_ptr168
  %arr_struct_load169 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca165
  %running = alloca i1
  %frames = alloca i32
  %last_fps = alloca float
  %last_counter_debug_stats = alloca i64
  %current_cycle_count = alloca i64
  %last_cycle_count = alloca i64
  %game_start_time = alloca i64
  %struct_alloca172 = alloca <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>
  %struct_alloca185 = alloca <{ i16*, i32, i32, float }>
  %struct_alloca195 = alloca <{ float, float }>
  %frame_start_counter = alloca i64
  %last_frame_start_counter = alloca i64
  %struct_alloca205 = alloca <{ i8*, i32, i32, i32 }>
  %t = alloca float
  %arr_struct_alloca233 = alloca <{ i32, i8* }>
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.124, i32 0, i32 0), i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr234 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca233, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr234
  %gep_arr_elem_ptr235 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca233, i32 0, i32 1
  store i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.124, i32 0, i32 0), i8** %gep_arr_elem_ptr235
  %arr_struct_load236 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca233
  %arr_struct_alloca237 = alloca <{ i32, i8* }>
  %arr_elem_alloca238 = alloca i8, i32 100
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca238, i8* getelementptr inbounds ([101 x i8], [101 x i8]* @str.125, i32 0, i32 0), i32 100, i32 0, i1 false)
  %gep_arr_elem_ptr239 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca237, i32 0, i32 0
  store i32 100, i32* %gep_arr_elem_ptr239
  %gep_arr_elem_ptr240 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca237, i32 0, i32 1
  store i8* %arr_elem_alloca238, i8** %gep_arr_elem_ptr240
  %arr_struct_load241 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca237
  %arr_struct_alloca256 = alloca <{ i32, i8* }>
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.126, i32 0, i32 0), i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr257 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca256, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr257
  %gep_arr_elem_ptr258 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca256, i32 0, i32 1
  store i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.126, i32 0, i32 0), i8** %gep_arr_elem_ptr258
  %arr_struct_load259 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca256
  %arr_struct_alloca260 = alloca <{ i32, i8* }>
  %arr_elem_alloca261 = alloca i8, i32 100
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca261, i8* getelementptr inbounds ([101 x i8], [101 x i8]* @str.127, i32 0, i32 0), i32 100, i32 0, i1 false)
  %gep_arr_elem_ptr262 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca260, i32 0, i32 0
  store i32 100, i32* %gep_arr_elem_ptr262
  %gep_arr_elem_ptr263 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca260, i32 0, i32 1
  store i8* %arr_elem_alloca261, i8** %gep_arr_elem_ptr263
  %arr_struct_load264 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca260
  %frame_end_counter = alloca i64
  %wait_time = alloca float
  %sleep_ms = alloca i32
  %cycles_elapsed = alloca i64
  %frame_ms = alloca float
  %arr_struct_alloca321 = alloca <{ i32, i8* }>
  %arr_elem_alloca322 = alloca i8
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca322, i8* getelementptr inbounds ([2 x i8], [2 x i8]* @str.10, i32 0, i32 0), i32 1, i32 0, i1 false)
  %gep_arr_elem_ptr323 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca321, i32 0, i32 0
  store i32 1, i32* %gep_arr_elem_ptr323
  %gep_arr_elem_ptr324 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca321, i32 0, i32 1
  store i8* %arr_elem_alloca322, i8** %gep_arr_elem_ptr324
  %arr_struct_load325 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca321
  %arr_struct_alloca327 = alloca <{ i32, i8* }>
  %arr_elem_alloca328 = alloca i8, i32 7
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca328, i8* getelementptr inbounds ([8 x i8], [8 x i8]* @str.128, i32 0, i32 0), i32 7, i32 0, i1 false)
  %gep_arr_elem_ptr329 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca327, i32 0, i32 0
  store i32 7, i32* %gep_arr_elem_ptr329
  %gep_arr_elem_ptr330 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca327, i32 0, i32 1
  store i8* %arr_elem_alloca328, i8** %gep_arr_elem_ptr330
  %arr_struct_load331 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca327
  %arr_struct_alloca338 = alloca <{ i32, i8* }>
  %arr_elem_alloca339 = alloca i8, i32 6
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca339, i8* getelementptr inbounds ([7 x i8], [7 x i8]* @str.129, i32 0, i32 0), i32 6, i32 0, i1 false)
  %gep_arr_elem_ptr340 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca338, i32 0, i32 0
  store i32 6, i32* %gep_arr_elem_ptr340
  %gep_arr_elem_ptr341 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca338, i32 0, i32 1
  store i8* %arr_elem_alloca339, i8** %gep_arr_elem_ptr341
  %arr_struct_load342 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca338
  %arr_struct_alloca346 = alloca <{ i32, i8* }>
  %arr_elem_alloca347 = alloca i8, i32 10
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca347, i8* getelementptr inbounds ([11 x i8], [11 x i8]* @str.130, i32 0, i32 0), i32 10, i32 0, i1 false)
  %gep_arr_elem_ptr348 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca346, i32 0, i32 0
  store i32 10, i32* %gep_arr_elem_ptr348
  %gep_arr_elem_ptr349 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca346, i32 0, i32 1
  store i8* %arr_elem_alloca347, i8** %gep_arr_elem_ptr349
  %arr_struct_load350 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca346
  %arr_struct_alloca351 = alloca <{ i32, i8* }>
  %arr_elem_alloca352 = alloca i8, i32 2
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca352, i8* getelementptr inbounds ([3 x i8], [3 x i8]* @str.131, i32 0, i32 0), i32 2, i32 0, i1 false)
  %gep_arr_elem_ptr353 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca351, i32 0, i32 0
  store i32 2, i32* %gep_arr_elem_ptr353
  %gep_arr_elem_ptr354 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca351, i32 0, i32 1
  store i8* %arr_elem_alloca352, i8** %gep_arr_elem_ptr354
  %arr_struct_load355 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca351
  br label %entry

entry:                                            ; preds = %vars
  %fun_call = call i32 @QueryPerformanceFrequency(i64* @perf_count_freq)
  store i32 0, i32* %error
  %fun_call1 = call i32 @timeBeginPeriod(i32 1)
  store i32 %fun_call1, i32* %error
  %error2 = load i32, i32* %error
  %icmp_tmp = icmp eq i32 %error2, 0
  call void @assert(i1 %icmp_tmp, <{ i32, i8* }> %arr_struct_load, <{ i32, i8* }> %arr_struct_load7)
  store i32 960, i32* getelementptr inbounds (<{ i64, i64, i32, i32 }>, <{ i64, i64, i32, i32 }>* @window, i32 0, i32 2)
  store i32 540, i32* getelementptr inbounds (<{ i64, i64, i32, i32 }>, <{ i64, i64, i32, i32 }>* @window, i32 0, i32 3)
  %struct_field = load i32, i32* getelementptr inbounds (<{ i64, i64, i32, i32 }>, <{ i64, i64, i32, i32 }>* @window, i32 0, i32 2)
  %struct_field8 = load i32, i32* getelementptr inbounds (<{ i64, i64, i32, i32 }>, <{ i64, i64, i32, i32 }>* @window, i32 0, i32 3)
  call void @create_backbuffer(<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i32 %struct_field, i32 %struct_field8)
  %fun_call19 = call i8* @cstr(<{ i32, i8* }> %arr_struct_load13, <{ i32, i8* }> %arr_struct_load18)
  store i8* %fun_call19, i8** %class_name
  %fun_call30 = call i8* @cstr(<{ i32, i8* }> %arr_struct_load24, <{ i32, i8* }> %arr_struct_load29)
  store i8* %fun_call30, i8** %window_name
  %fun_call31 = call i64 @GetModuleHandleA(i64 0)
  store i64 %fun_call31, i64* %instance
  %struct_arg_0 = getelementptr inbounds <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>, <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>* %struct_alloca, i32 0, i32 0
  store i32 0, i32* %struct_arg_0
  %struct_arg_1 = getelementptr inbounds <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>, <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>* %struct_alloca, i32 0, i32 1
  store i32 0, i32* %struct_arg_1
  %struct_arg_2 = getelementptr inbounds <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>, <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>* %struct_alloca, i32 0, i32 2
  store i8* null, i8** %struct_arg_2
  %struct_arg_3 = getelementptr inbounds <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>, <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>* %struct_alloca, i32 0, i32 3
  store i32 0, i32* %struct_arg_3
  %struct_arg_4 = getelementptr inbounds <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>, <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>* %struct_alloca, i32 0, i32 4
  store i32 0, i32* %struct_arg_4
  %struct_arg_5 = getelementptr inbounds <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>, <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>* %struct_alloca, i32 0, i32 5
  store i64 0, i64* %struct_arg_5
  %struct_arg_6 = getelementptr inbounds <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>, <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>* %struct_alloca, i32 0, i32 6
  store i64 0, i64* %struct_arg_6
  %struct_arg_7 = getelementptr inbounds <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>, <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>* %struct_alloca, i32 0, i32 7
  store i64 0, i64* %struct_arg_7
  %struct_arg_8 = getelementptr inbounds <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>, <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>* %struct_alloca, i32 0, i32 8
  store i64 0, i64* %struct_arg_8
  %struct_arg_9 = getelementptr inbounds <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>, <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>* %struct_alloca, i32 0, i32 9
  store i8* null, i8** %struct_arg_9
  %struct_arg_10 = getelementptr inbounds <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>, <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>* %struct_alloca, i32 0, i32 10
  store i8* null, i8** %struct_arg_10
  %struct_arg_11 = getelementptr inbounds <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>, <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>* %struct_alloca, i32 0, i32 11
  store i64 0, i64* %struct_arg_11
  %struct_field_ptr = getelementptr inbounds <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>, <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>* %struct_alloca, i32 0, i32 0
  store i32 ptrtoint (<{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>* getelementptr (<{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>, <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>* null, i32 1) to i32), i32* %struct_field_ptr
  %struct_field_ptr32 = getelementptr inbounds <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>, <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>* %struct_alloca, i32 0, i32 1
  store i32 3, i32* %struct_field_ptr32
  %struct_field_ptr33 = getelementptr inbounds <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>, <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>* %struct_alloca, i32 0, i32 2
  store i8* bitcast (i64 (i64, i32, i64, i64)* @main_window_callback to i8*), i8** %struct_field_ptr33
  %struct_field_ptr34 = getelementptr inbounds <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>, <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>* %struct_alloca, i32 0, i32 5
  %instance35 = load i64, i64* %instance
  store i64 %instance35, i64* %struct_field_ptr34
  %struct_field_ptr36 = getelementptr inbounds <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>, <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>* %struct_alloca, i32 0, i32 10
  %class_name37 = load i8*, i8** %class_name
  store i8* %class_name37, i8** %struct_field_ptr36
  %fun_call38 = call i16 @RegisterClassExA(<{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>* %struct_alloca)
  %class_name39 = load i8*, i8** %class_name
  %window_name40 = load i8*, i8** %window_name
  %struct_field41 = load i32, i32* getelementptr inbounds (<{ i64, i64, i32, i32 }>, <{ i64, i64, i32, i32 }>* @window, i32 0, i32 2)
  %struct_field42 = load i32, i32* getelementptr inbounds (<{ i64, i64, i32, i32 }>, <{ i64, i64, i32, i32 }>* @window, i32 0, i32 3)
  %instance43 = load i64, i64* %instance
  %fun_call44 = call i64 @CreateWindowExA(i32 0, i8* %class_name39, i8* %window_name40, i32 282001408, i32 -2147483648, i32 -2147483648, i32 %struct_field41, i32 %struct_field42, i64 0, i64 0, i64 %instance43, i64 0)
  store i64 %fun_call44, i64* getelementptr inbounds (<{ i64, i64, i32, i32 }>, <{ i64, i64, i32, i32 }>* @window, i32 0, i32 0)
  %struct_field45 = load i64, i64* getelementptr inbounds (<{ i64, i64, i32, i32 }>, <{ i64, i64, i32, i32 }>* @window, i32 0, i32 0)
  %fun_call46 = call i64 @GetDC(i64 %struct_field45)
  store i64 %fun_call46, i64* getelementptr inbounds (<{ i64, i64, i32, i32 }>, <{ i64, i64, i32, i32 }>* @window, i32 0, i32 1)
  call void @update_window_rect()
  store i32 60, i32* %monitor_hz
  %monitor_hz47 = load i32, i32* %monitor_hz
  store i32 %monitor_hz47, i32* %game_target_fps
  %game_target_fps48 = load i32, i32* %game_target_fps
  %int_to_float_cast = sitofp i32 %game_target_fps48 to float
  %fdiv_tmp = fdiv float 1.000000e+00, %int_to_float_cast
  store float %fdiv_tmp, float* %target_sec_per_frame
  %struct_field_ptr49 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 1
  store i32 48000, i32* %struct_field_ptr49
  %struct_field_ptr50 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 2
  store i32 mul (i32 ptrtoint (i16* getelementptr (i16, i16* null, i32 1) to i32), i32 2), i32* %struct_field_ptr50
  %struct_field_ptr51 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 3
  %struct_field_ptr52 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 2
  %struct_field53 = load i32, i32* %struct_field_ptr52
  %struct_field_ptr54 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 1
  %struct_field55 = load i32, i32* %struct_field_ptr54
  %mul_tmp = mul i32 %struct_field53, %struct_field55
  %game_target_fps56 = load i32, i32* %game_target_fps
  %div_tmp = sdiv i32 %mul_tmp, %game_target_fps56
  store i32 %div_tmp, i32* %struct_field_ptr51
  %struct_field_ptr57 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 4
  %struct_field_ptr58 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 1
  %struct_field59 = load i32, i32* %struct_field_ptr58
  %struct_field_ptr60 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 2
  %struct_field61 = load i32, i32* %struct_field_ptr60
  %mul_tmp62 = mul i32 %struct_field59, %struct_field61
  %mul_tmp63 = mul i32 %mul_tmp62, 1
  store i32 %mul_tmp63, i32* %struct_field_ptr57
  %struct_field64 = load i64, i64* getelementptr inbounds (<{ i64, i64, i32, i32 }>, <{ i64, i64, i32, i32 }>* @window, i32 0, i32 0)
  %struct_field_ptr65 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 4
  %struct_field66 = load i32, i32* %struct_field_ptr65
  %struct_field_ptr67 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 1
  %struct_field68 = load i32, i32* %struct_field_ptr67
  %fun_call69 = call <{ i8* }>* @init_direct_sound(i64 %struct_field64, i32 %struct_field66, i32 %struct_field68)
  store <{ i8* }>* %fun_call69, <{ i8* }>** %sound_buffer
  %struct_field_ptr70 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 0
  %sound_buffer71 = load <{ i8* }>*, <{ i8* }>** %sound_buffer
  %hmpf = bitcast <{ i8* }>* %sound_buffer71 to i8*
  store i8* %hmpf, i8** %struct_field_ptr70
  %struct_field_ptr72 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 4
  %struct_field73 = load i32, i32* %struct_field_ptr72
  %int_cast = sext i32 %struct_field73 to i64
  %fun_call74 = call i8* @VirtualAlloc(i8* null, i64 %int_cast, i32 4096, i32 4)
  %pointer_bit_cast = bitcast i8* %fun_call74 to i16*
  store i16* %pointer_bit_cast, i16** %game_sample_buffer
  %game_sample_buffer75 = load i16*, i16** %game_sample_buffer
  %pointer_bit_cast76 = bitcast i16* %game_sample_buffer75 to i8*
  %icmp_tmp77 = icmp ne i8* %pointer_bit_cast76, null
  call void @assert(i1 %icmp_tmp77, <{ i32, i8* }> %arr_struct_load81, <{ i32, i8* }> %arr_struct_load86)
  store i8* null, i8** %base_address
  br i1 true, label %then, label %else

then:                                             ; preds = %entry
  %fun_call87 = call i64 @terabytes(i64 2)
  %int_to_ptr = inttoptr i64 %fun_call87 to i8*
  store i8* %int_to_ptr, i8** %base_address
  br label %endif

else:                                             ; preds = %entry
  store i8* null, i8** %base_address
  br label %endif

endif:                                            ; preds = %else, %then
  %struct_arg_089 = getelementptr inbounds <{ i1 (<{ i32, i8* }>, <{ i32, i8* }>)*, <{ i32, i8* }> (<{ i32, i8* }>)*, void (i8*)* }>, <{ i1 (<{ i32, i8* }>, <{ i32, i8* }>)*, <{ i32, i8* }> (<{ i32, i8* }>)*, void (i8*)* }>* %struct_alloca88, i32 0, i32 0
  store i1 (<{ i32, i8* }>, <{ i32, i8* }>)* null, i1 (<{ i32, i8* }>, <{ i32, i8* }>)** %struct_arg_089
  %struct_arg_190 = getelementptr inbounds <{ i1 (<{ i32, i8* }>, <{ i32, i8* }>)*, <{ i32, i8* }> (<{ i32, i8* }>)*, void (i8*)* }>, <{ i1 (<{ i32, i8* }>, <{ i32, i8* }>)*, <{ i32, i8* }> (<{ i32, i8* }>)*, void (i8*)* }>* %struct_alloca88, i32 0, i32 1
  store <{ i32, i8* }> (<{ i32, i8* }>)* null, <{ i32, i8* }> (<{ i32, i8* }>)** %struct_arg_190
  %struct_arg_291 = getelementptr inbounds <{ i1 (<{ i32, i8* }>, <{ i32, i8* }>)*, <{ i32, i8* }> (<{ i32, i8* }>)*, void (i8*)* }>, <{ i1 (<{ i32, i8* }>, <{ i32, i8* }>)*, <{ i32, i8* }> (<{ i32, i8* }>)*, void (i8*)* }>* %struct_alloca88, i32 0, i32 2
  store void (i8*)* null, void (i8*)** %struct_arg_291
  %struct_field_ptr92 = getelementptr inbounds <{ i1 (<{ i32, i8* }>, <{ i32, i8* }>)*, <{ i32, i8* }> (<{ i32, i8* }>)*, void (i8*)* }>, <{ i1 (<{ i32, i8* }>, <{ i32, i8* }>)*, <{ i32, i8* }> (<{ i32, i8* }>)*, void (i8*)* }>* %struct_alloca88, i32 0, i32 0
  store i1 (<{ i32, i8* }>, <{ i32, i8* }>)* @platform_write_file, i1 (<{ i32, i8* }>, <{ i32, i8* }>)** %struct_field_ptr92
  %struct_field_ptr93 = getelementptr inbounds <{ i1 (<{ i32, i8* }>, <{ i32, i8* }>)*, <{ i32, i8* }> (<{ i32, i8* }>)*, void (i8*)* }>, <{ i1 (<{ i32, i8* }>, <{ i32, i8* }>)*, <{ i32, i8* }> (<{ i32, i8* }>)*, void (i8*)* }>* %struct_alloca88, i32 0, i32 1
  store <{ i32, i8* }> (<{ i32, i8* }>)* @platform_read_file, <{ i32, i8* }> (<{ i32, i8* }>)** %struct_field_ptr93
  %struct_field_ptr94 = getelementptr inbounds <{ i1 (<{ i32, i8* }>, <{ i32, i8* }>)*, <{ i32, i8* }> (<{ i32, i8* }>)*, void (i8*)* }>, <{ i1 (<{ i32, i8* }>, <{ i32, i8* }>)*, <{ i32, i8* }> (<{ i32, i8* }>)*, void (i8*)* }>* %struct_alloca88, i32 0, i32 2
  store void (i8*)* @platform_free_file_memory, void (i8*)** %struct_field_ptr94
  %struct_arg_096 = getelementptr inbounds <{ i8*, i64, i8*, i64, i8* }>, <{ i8*, i64, i8*, i64, i8* }>* %struct_alloca95, i32 0, i32 0
  store i8* null, i8** %struct_arg_096
  %struct_arg_197 = getelementptr inbounds <{ i8*, i64, i8*, i64, i8* }>, <{ i8*, i64, i8*, i64, i8* }>* %struct_alloca95, i32 0, i32 1
  store i64 0, i64* %struct_arg_197
  %struct_arg_298 = getelementptr inbounds <{ i8*, i64, i8*, i64, i8* }>, <{ i8*, i64, i8*, i64, i8* }>* %struct_alloca95, i32 0, i32 2
  store i8* null, i8** %struct_arg_298
  %struct_arg_399 = getelementptr inbounds <{ i8*, i64, i8*, i64, i8* }>, <{ i8*, i64, i8*, i64, i8* }>* %struct_alloca95, i32 0, i32 3
  store i64 0, i64* %struct_arg_399
  %struct_arg_4100 = getelementptr inbounds <{ i8*, i64, i8*, i64, i8* }>, <{ i8*, i64, i8*, i64, i8* }>* %struct_alloca95, i32 0, i32 4
  store i8* null, i8** %struct_arg_4100
  %struct_field_ptr101 = getelementptr inbounds <{ i8*, i64, i8*, i64, i8* }>, <{ i8*, i64, i8*, i64, i8* }>* %struct_alloca95, i32 0, i32 1
  %fun_call102 = call i64 @megabytes(i64 1)
  store i64 %fun_call102, i64* %struct_field_ptr101
  %struct_field_ptr103 = getelementptr inbounds <{ i8*, i64, i8*, i64, i8* }>, <{ i8*, i64, i8*, i64, i8* }>* %struct_alloca95, i32 0, i32 3
  store i64 0, i64* %struct_field_ptr103
  %struct_field_ptr104 = getelementptr inbounds <{ i8*, i64, i8*, i64, i8* }>, <{ i8*, i64, i8*, i64, i8* }>* %struct_alloca95, i32 0, i32 1
  %struct_field105 = load i64, i64* %struct_field_ptr104
  %struct_field_ptr106 = getelementptr inbounds <{ i8*, i64, i8*, i64, i8* }>, <{ i8*, i64, i8*, i64, i8* }>* %struct_alloca95, i32 0, i32 3
  %struct_field107 = load i64, i64* %struct_field_ptr106
  %add_tmp = add i64 %struct_field105, %struct_field107
  store i64 %add_tmp, i64* %total_size
  %struct_field_ptr108 = getelementptr inbounds <{ i8*, i64, i8*, i64, i8* }>, <{ i8*, i64, i8*, i64, i8* }>* %struct_alloca95, i32 0, i32 2
  %base_address109 = load i8*, i8** %base_address
  %total_size110 = load i64, i64* %total_size
  %fun_call111 = call i8* @VirtualAlloc(i8* %base_address109, i64 %total_size110, i32 12288, i32 4)
  store i8* %fun_call111, i8** %struct_field_ptr108
  %struct_field_ptr112 = getelementptr inbounds <{ i8*, i64, i8*, i64, i8* }>, <{ i8*, i64, i8*, i64, i8* }>* %struct_alloca95, i32 0, i32 2
  %struct_field113 = load i8*, i8** %struct_field_ptr112
  %icmp_tmp114 = icmp ne i8* %struct_field113, null
  call void @assert(i1 %icmp_tmp114, <{ i32, i8* }> %arr_struct_load118, <{ i32, i8* }> %arr_struct_load123)
  %struct_field_ptr124 = getelementptr inbounds <{ i8*, i64, i8*, i64, i8* }>, <{ i8*, i64, i8*, i64, i8* }>* %struct_alloca95, i32 0, i32 4
  %struct_field_ptr125 = getelementptr inbounds <{ i8*, i64, i8*, i64, i8* }>, <{ i8*, i64, i8*, i64, i8* }>* %struct_alloca95, i32 0, i32 2
  %struct_field126 = load i8*, i8** %struct_field_ptr125
  %struct_field_ptr127 = getelementptr inbounds <{ i8*, i64, i8*, i64, i8* }>, <{ i8*, i64, i8*, i64, i8* }>* %struct_alloca95, i32 0, i32 1
  %struct_field128 = load i64, i64* %struct_field_ptr127
  %ptr_add = getelementptr i8, i8* %struct_field126, i64 %struct_field128
  store i8* %ptr_add, i8** %struct_field_ptr124
  %struct_field_ptr129 = getelementptr inbounds <{ i8*, i64, i8*, i64, i8* }>, <{ i8*, i64, i8*, i64, i8* }>* %struct_alloca95, i32 0, i32 4
  %struct_field130 = load i8*, i8** %struct_field_ptr129
  %icmp_tmp131 = icmp ne i8* %struct_field130, null
  call void @assert(i1 %icmp_tmp131, <{ i32, i8* }> %arr_struct_load135, <{ i32, i8* }> %arr_struct_load140)
  %struct_field_ptr141 = getelementptr inbounds <{ i8*, i64, i8*, i64, i8* }>, <{ i8*, i64, i8*, i64, i8* }>* %struct_alloca95, i32 0, i32 0
  %hmpf142 = bitcast <{ i1 (<{ i32, i8* }>, <{ i32, i8* }>)*, <{ i32, i8* }> (<{ i32, i8* }>)*, void (i8*)* }>* %struct_alloca88 to i8*
  store i8* %hmpf142, i8** %struct_field_ptr141
  %struct_arg_0144 = getelementptr inbounds <{ i8*, i1, i1, i1, i1, i64, i64 }>, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %struct_alloca143, i32 0, i32 0
  store i8* null, i8** %struct_arg_0144
  %struct_arg_1145 = getelementptr inbounds <{ i8*, i1, i1, i1, i1, i64, i64 }>, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %struct_alloca143, i32 0, i32 1
  store i1 false, i1* %struct_arg_1145
  %struct_arg_2146 = getelementptr inbounds <{ i8*, i1, i1, i1, i1, i64, i64 }>, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %struct_alloca143, i32 0, i32 2
  store i1 false, i1* %struct_arg_2146
  %struct_arg_3147 = getelementptr inbounds <{ i8*, i1, i1, i1, i1, i64, i64 }>, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %struct_alloca143, i32 0, i32 3
  store i1 false, i1* %struct_arg_3147
  %struct_arg_4148 = getelementptr inbounds <{ i8*, i1, i1, i1, i1, i64, i64 }>, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %struct_alloca143, i32 0, i32 4
  store i1 false, i1* %struct_arg_4148
  %struct_arg_5149 = getelementptr inbounds <{ i8*, i1, i1, i1, i1, i64, i64 }>, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %struct_alloca143, i32 0, i32 5
  store i64 0, i64* %struct_arg_5149
  %struct_arg_6150 = getelementptr inbounds <{ i8*, i1, i1, i1, i1, i64, i64 }>, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %struct_alloca143, i32 0, i32 6
  store i64 0, i64* %struct_arg_6150
  %struct_field_ptr151 = getelementptr inbounds <{ i8*, i1, i1, i1, i1, i64, i64 }>, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %struct_alloca143, i32 0, i32 0
  %hmpf152 = bitcast <{ i8*, i64, i8*, i64, i8* }>* %struct_alloca95 to i8*
  store i8* %hmpf152, i8** %struct_field_ptr151
  %fun_call153 = call <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }> @load_game_dll()
  store <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }> %fun_call153, <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>* %game
  call void @clear_sound_buffer(<{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output)
  %struct_arrow_load = load <{ i8* }>*, <{ i8* }>** %sound_buffer
  %struct_field_ptr154 = getelementptr inbounds <{ i8* }>, <{ i8* }>* %struct_arrow_load, i32 0, i32 0
  %struct_arrow_load155 = load i8*, i8** %struct_field_ptr154
  %hack_bitcast = bitcast i8* %struct_arrow_load155 to <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>*
  %struct_field_ptr156 = getelementptr inbounds <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>, <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>* %hack_bitcast, i32 0, i32 12
  %fun_ptr_load = load i32 (i8*, i32, i32, i32)*, i32 (i8*, i32, i32, i32)** %struct_field_ptr156
  %sound_buffer157 = load <{ i8* }>*, <{ i8* }>** %sound_buffer
  %fun_param_hack = bitcast <{ i8* }>* %sound_buffer157 to i8*
  %fun_call158 = call i32 %fun_ptr_load(i8* %fun_param_hack, i32 0, i32 0, i32 1)
  store i32 %fun_call158, i32* %error
  %error159 = load i32, i32* %error
  %icmp_tmp160 = icmp eq i32 %error159, 0
  call void @assert(i1 %icmp_tmp160, <{ i32, i8* }> %arr_struct_load164, <{ i32, i8* }> %arr_struct_load169)
  store i1 true, i1* %running
  store i32 0, i32* %frames
  store float 6.000000e+01, float* %last_fps
  store i64 0, i64* %last_counter_debug_stats
  %fun_call170 = call i64 @_rdtsc()
  store i64 %fun_call170, i64* %last_cycle_count
  %fun_call171 = call i64 @get_perf_counter()
  store i64 %fun_call171, i64* %game_start_time
  %struct_arg_0173 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca172, i32 0, i32 0
  store <{ float, float }> zeroinitializer, <{ float, float }>* %struct_arg_0173
  %struct_arg_1174 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca172, i32 0, i32 1
  store <{ i1, i1, i1 }> zeroinitializer, <{ i1, i1, i1 }>* %struct_arg_1174
  %struct_arg_2175 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca172, i32 0, i32 2
  store <{ i1, i1, i1 }> zeroinitializer, <{ i1, i1, i1 }>* %struct_arg_2175
  %struct_arg_3176 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca172, i32 0, i32 3
  store <{ i1, i1, i1 }> zeroinitializer, <{ i1, i1, i1 }>* %struct_arg_3176
  %struct_arg_4177 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca172, i32 0, i32 4
  store <{ i1, i1, i1 }> zeroinitializer, <{ i1, i1, i1 }>* %struct_arg_4177
  %struct_arg_5178 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca172, i32 0, i32 5
  store <{ i1, i1, i1 }> zeroinitializer, <{ i1, i1, i1 }>* %struct_arg_5178
  %struct_arg_6179 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca172, i32 0, i32 6
  store <{ i1, i1, i1 }> zeroinitializer, <{ i1, i1, i1 }>* %struct_arg_6179
  %struct_arg_7180 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca172, i32 0, i32 7
  store <{ i1, i1, i1 }> zeroinitializer, <{ i1, i1, i1 }>* %struct_arg_7180
  %struct_arg_8181 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca172, i32 0, i32 8
  store i32 0, i32* %struct_arg_8181
  %struct_arg_9182 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca172, i32 0, i32 9
  store i32 0, i32* %struct_arg_9182
  %struct_arg_10183 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca172, i32 0, i32 10
  store i32 0, i32* %struct_arg_10183
  %struct_arg_11184 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca172, i32 0, i32 11
  store i1 false, i1* %struct_arg_11184
  %struct_arg_0186 = getelementptr inbounds <{ i16*, i32, i32, float }>, <{ i16*, i32, i32, float }>* %struct_alloca185, i32 0, i32 0
  store i16* null, i16** %struct_arg_0186
  %struct_arg_1187 = getelementptr inbounds <{ i16*, i32, i32, float }>, <{ i16*, i32, i32, float }>* %struct_alloca185, i32 0, i32 1
  store i32 0, i32* %struct_arg_1187
  %struct_arg_2188 = getelementptr inbounds <{ i16*, i32, i32, float }>, <{ i16*, i32, i32, float }>* %struct_alloca185, i32 0, i32 2
  store i32 0, i32* %struct_arg_2188
  %struct_arg_3189 = getelementptr inbounds <{ i16*, i32, i32, float }>, <{ i16*, i32, i32, float }>* %struct_alloca185, i32 0, i32 3
  store float 0.000000e+00, float* %struct_arg_3189
  %struct_field_ptr190 = getelementptr inbounds <{ i16*, i32, i32, float }>, <{ i16*, i32, i32, float }>* %struct_alloca185, i32 0, i32 0
  %game_sample_buffer191 = load i16*, i16** %game_sample_buffer
  store i16* %game_sample_buffer191, i16** %struct_field_ptr190
  %struct_field_ptr192 = getelementptr inbounds <{ i16*, i32, i32, float }>, <{ i16*, i32, i32, float }>* %struct_alloca185, i32 0, i32 2
  %struct_field_ptr193 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 1
  %struct_field194 = load i32, i32* %struct_field_ptr193
  store i32 %struct_field194, i32* %struct_field_ptr192
  %struct_arg_0196 = getelementptr inbounds <{ float, float }>, <{ float, float }>* %struct_alloca195, i32 0, i32 0
  store float 0.000000e+00, float* %struct_arg_0196
  %struct_arg_1197 = getelementptr inbounds <{ float, float }>, <{ float, float }>* %struct_alloca195, i32 0, i32 1
  store float 0.000000e+00, float* %struct_arg_1197
  %fun_call198 = call i64 @get_perf_counter()
  %target_sec_per_frame199 = load float, float* %target_sec_per_frame
  %perf_count_freq = load i64, i64* @perf_count_freq
  %int_to_float_cast200 = sitofp i64 %perf_count_freq to float
  %fmul_tmp = fmul float %target_sec_per_frame199, %int_to_float_cast200
  %int_cast201 = fptosi float %fmul_tmp to i64
  %sub_tmp = sub i64 %fun_call198, %int_cast201
  store i64 %sub_tmp, i64* %frame_start_counter
  br label %while_cond

while_cond:                                       ; preds = %endif310, %endif
  %running202 = load i1, i1* %running
  br i1 %running202, label %while, label %while_end

while:                                            ; preds = %while_cond
  %frame_start_counter203 = load i64, i64* %frame_start_counter
  store i64 %frame_start_counter203, i64* %last_frame_start_counter
  %fun_call204 = call i64 @get_perf_counter()
  store i64 %fun_call204, i64* %frame_start_counter
  call void @process_pending_messages(<{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca172, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %struct_alloca143)
  %struct_arg_0206 = getelementptr inbounds <{ i8*, i32, i32, i32 }>, <{ i8*, i32, i32, i32 }>* %struct_alloca205, i32 0, i32 0
  store i8* null, i8** %struct_arg_0206
  %struct_arg_1207 = getelementptr inbounds <{ i8*, i32, i32, i32 }>, <{ i8*, i32, i32, i32 }>* %struct_alloca205, i32 0, i32 1
  store i32 0, i32* %struct_arg_1207
  %struct_arg_2208 = getelementptr inbounds <{ i8*, i32, i32, i32 }>, <{ i8*, i32, i32, i32 }>* %struct_alloca205, i32 0, i32 2
  store i32 0, i32* %struct_arg_2208
  %struct_arg_3209 = getelementptr inbounds <{ i8*, i32, i32, i32 }>, <{ i8*, i32, i32, i32 }>* %struct_alloca205, i32 0, i32 3
  store i32 0, i32* %struct_arg_3209
  %struct_field_ptr210 = getelementptr inbounds <{ i8*, i32, i32, i32 }>, <{ i8*, i32, i32, i32 }>* %struct_alloca205, i32 0, i32 0
  %struct_field211 = load i8*, i8** getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i32 0, i32 1)
  store i8* %struct_field211, i8** %struct_field_ptr210
  %struct_field_ptr212 = getelementptr inbounds <{ i8*, i32, i32, i32 }>, <{ i8*, i32, i32, i32 }>* %struct_alloca205, i32 0, i32 1
  %struct_field213 = load i32, i32* getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i32 0, i32 2)
  store i32 %struct_field213, i32* %struct_field_ptr212
  %struct_field_ptr214 = getelementptr inbounds <{ i8*, i32, i32, i32 }>, <{ i8*, i32, i32, i32 }>* %struct_alloca205, i32 0, i32 2
  %struct_field215 = load i32, i32* getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i32 0, i32 3)
  store i32 %struct_field215, i32* %struct_field_ptr214
  %struct_field_ptr216 = getelementptr inbounds <{ i8*, i32, i32, i32 }>, <{ i8*, i32, i32, i32 }>* %struct_alloca205, i32 0, i32 3
  %struct_field217 = load i32, i32* getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i32 0, i32 4)
  store i32 %struct_field217, i32* %struct_field_ptr216
  %frame_start_counter218 = load i64, i64* %frame_start_counter
  %game_start_time219 = load i64, i64* %game_start_time
  %fun_call220 = call float @get_duration(i64 %frame_start_counter218, i64 %game_start_time219)
  store float %fun_call220, float* %t
  %struct_field_ptr221 = getelementptr inbounds <{ float, float }>, <{ float, float }>* %struct_alloca195, i32 0, i32 1
  %t222 = load float, float* %t
  store float %t222, float* %struct_field_ptr221
  %struct_field_ptr223 = getelementptr inbounds <{ float, float }>, <{ float, float }>* %struct_alloca195, i32 0, i32 0
  %frame_start_counter224 = load i64, i64* %frame_start_counter
  %last_frame_start_counter225 = load i64, i64* %last_frame_start_counter
  %fun_call226 = call float @get_duration(i64 %frame_start_counter224, i64 %last_frame_start_counter225)
  store float %fun_call226, float* %struct_field_ptr223
  %struct_field_ptr227 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca172, i32 0, i32 0
  %game_time = load <{ float, float }>, <{ float, float }>* %struct_alloca195
  store <{ float, float }> %game_time, <{ float, float }>* %struct_field_ptr227
  %struct_field_ptr228 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca172, i32 0, i32 11
  %struct_field_ptr229 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca172, i32 0, i32 11
  %struct_field230 = load i1, i1* %struct_field_ptr229
  %window_requests_quit = load i1, i1* @window_requests_quit
  %or_tmp = or i1 %struct_field230, %window_requests_quit
  store i1 %or_tmp, i1* %struct_field_ptr228
  call void @get_mouse_input(<{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca172)
  call void @record_input(<{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca172, <{ i8*, i64, i8*, i64, i8* }>* %struct_alloca95, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %struct_alloca143)
  call void @replay_input(<{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca172, <{ i8*, i64, i8*, i64, i8* }>* %struct_alloca95, <{ i8*, i1, i1, i1, i1, i64, i64 }>* %struct_alloca143)
  %struct_field_ptr231 = getelementptr inbounds <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>, <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>* %game, i32 0, i32 3
  %struct_field232 = load i1, i1* %struct_field_ptr231
  call void @assert(i1 %struct_field232, <{ i32, i8* }> %arr_struct_load236, <{ i32, i8* }> %arr_struct_load241)
  %struct_field_ptr242 = getelementptr inbounds <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>, <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>* %game, i32 0, i32 1
  %fun_ptr_load243 = load i1 (i8*, i8*, i8*)*, i1 (i8*, i8*, i8*)** %struct_field_ptr242
  %fun_param_hack244 = bitcast <{ i8*, i64, i8*, i64, i8* }>* %struct_alloca95 to i8*
  %fun_param_hack245 = bitcast <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca172 to i8*
  %fun_param_hack246 = bitcast <{ i8*, i32, i32, i32 }>* %struct_alloca205 to i8*
  %fun_call247 = call i1 %fun_ptr_load243(i8* %fun_param_hack244, i8* %fun_param_hack245, i8* %fun_param_hack246)
  store i1 %fun_call247, i1* %running
  call void @update_next_sound_write_positon(<{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output)
  %struct_field_ptr248 = getelementptr inbounds <{ i16*, i32, i32, float }>, <{ i16*, i32, i32, float }>* %struct_alloca185, i32 0, i32 1
  %struct_field_ptr249 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 10
  %struct_field250 = load i32, i32* %struct_field_ptr249
  %struct_field_ptr251 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 2
  %struct_field252 = load i32, i32* %struct_field_ptr251
  %div_tmp253 = sdiv i32 %struct_field250, %struct_field252
  store i32 %div_tmp253, i32* %struct_field_ptr248
  %struct_field_ptr254 = getelementptr inbounds <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>, <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>* %game, i32 0, i32 3
  %struct_field255 = load i1, i1* %struct_field_ptr254
  call void @assert(i1 %struct_field255, <{ i32, i8* }> %arr_struct_load259, <{ i32, i8* }> %arr_struct_load264)
  %struct_field_ptr265 = getelementptr inbounds <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>, <{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>* %game, i32 0, i32 2
  %fun_ptr_load266 = load void (i8*, i8*)*, void (i8*, i8*)** %struct_field_ptr265
  %fun_param_hack267 = bitcast <{ i8*, i64, i8*, i64, i8* }>* %struct_alloca95 to i8*
  %fun_param_hack268 = bitcast <{ i16*, i32, i32, float }>* %struct_alloca185 to i8*
  call void %fun_ptr_load266(i8* %fun_param_hack267, i8* %fun_param_hack268)
  call void @fill_sound_buffer(<{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, <{ i16*, i32, i32, float }>* %struct_alloca185)
  %running269 = load i1, i1* %running
  %not_tmp = xor i1 %running269, true
  br i1 %not_tmp, label %then270, label %endif271

then270:                                          ; preds = %while
  call void @clear_sound_buffer(<{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output)
  %struct_field_ptr272 = getelementptr inbounds <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>, <{ i8*, i32, i32, i32, i32, i8*, i32, i32, i64, i32, i32, i1 }>* %sound_output, i32 0, i32 0
  %struct_arrow_load273 = load i8*, i8** %struct_field_ptr272
  %hack_bitcast274 = bitcast i8* %struct_arrow_load273 to <{ i8* }>*
  %struct_field_ptr275 = getelementptr inbounds <{ i8* }>, <{ i8* }>* %hack_bitcast274, i32 0, i32 0
  %struct_arrow_load276 = load i8*, i8** %struct_field_ptr275
  %hack_bitcast277 = bitcast i8* %struct_arrow_load276 to <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>*
  %struct_field_ptr278 = getelementptr inbounds <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>, <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>* %hack_bitcast277, i32 0, i32 18
  %fun_ptr_load279 = load i32 ()*, i32 ()** %struct_field_ptr278
  %fun_call280 = call i32 %fun_ptr_load279()
  br label %endif271

endif271:                                         ; preds = %then270, %while
  call void @blit_to_screen(<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer)
  %fun_call281 = call i64 @_rdtsc()
  store i64 %fun_call281, i64* %current_cycle_count
  %fun_call282 = call i64 @get_perf_counter()
  store i64 %fun_call282, i64* %frame_end_counter
  %frame_end_counter283 = load i64, i64* %frame_end_counter
  %frame_start_counter284 = load i64, i64* %frame_start_counter
  %fun_call285 = call float @get_duration(i64 %frame_end_counter283, i64 %frame_start_counter284)
  store float %fun_call285, float* %wait_time
  br label %while_cond286

while_cond286:                                    ; preds = %endif298, %endif271
  %wait_time289 = load float, float* %wait_time
  %target_sec_per_frame290 = load float, float* %target_sec_per_frame
  %fcmp_tmp = fcmp olt float %wait_time289, %target_sec_per_frame290
  br i1 %fcmp_tmp, label %while287, label %while_end288

while287:                                         ; preds = %while_cond286
  %target_sec_per_frame291 = load float, float* %target_sec_per_frame
  %wait_time292 = load float, float* %wait_time
  %fsub_tmp = fsub float %target_sec_per_frame291, %wait_time292
  %fmul_tmp293 = fmul float 1.000000e+03, %fsub_tmp
  %int_cast294 = fptosi float %fmul_tmp293 to i32
  store i32 %int_cast294, i32* %sleep_ms
  %sleep_ms295 = load i32, i32* %sleep_ms
  %icmp_tmp296 = icmp sgt i32 %sleep_ms295, 0
  br i1 %icmp_tmp296, label %then297, label %endif298

then297:                                          ; preds = %while287
  %sleep_ms299 = load i32, i32* %sleep_ms
  call void @Sleep(i32 %sleep_ms299)
  br label %endif298

endif298:                                         ; preds = %then297, %while287
  %fun_call300 = call i64 @get_perf_counter()
  store i64 %fun_call300, i64* %frame_end_counter
  %frame_end_counter301 = load i64, i64* %frame_end_counter
  %frame_start_counter302 = load i64, i64* %frame_start_counter
  %fun_call303 = call float @get_duration(i64 %frame_end_counter301, i64 %frame_start_counter302)
  store float %fun_call303, float* %wait_time
  br label %while_cond286

while_end288:                                     ; preds = %while_cond286
  %postinc_load = load i32, i32* %frames
  %postinc = add i32 %postinc_load, 1
  store i32 %postinc, i32* %frames
  %frame_end_counter304 = load i64, i64* %frame_end_counter
  %last_counter_debug_stats305 = load i64, i64* %last_counter_debug_stats
  %sub_tmp306 = sub i64 %frame_end_counter304, %last_counter_debug_stats305
  %perf_count_freq307 = load i64, i64* @perf_count_freq
  %icmp_tmp308 = icmp sgt i64 %sub_tmp306, %perf_count_freq307
  br i1 %icmp_tmp308, label %then309, label %endif310

then309:                                          ; preds = %while_end288
  br i1 true, label %then311, label %endif312

then311:                                          ; preds = %then309
  %frame_end_counter313 = load i64, i64* %frame_end_counter
  store i64 %frame_end_counter313, i64* %last_counter_debug_stats
  %current_cycle_count314 = load i64, i64* %current_cycle_count
  %last_cycle_count315 = load i64, i64* %last_cycle_count
  %sub_tmp316 = sub i64 %current_cycle_count314, %last_cycle_count315
  store i64 %sub_tmp316, i64* %cycles_elapsed
  %struct_field_ptr317 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca172, i32 0, i32 0
  %struct_field_ptr318 = getelementptr inbounds <{ float, float }>, <{ float, float }>* %struct_field_ptr317, i32 0, i32 0
  %struct_field319 = load float, float* %struct_field_ptr318
  %fmul_tmp320 = fmul float 1.000000e+03, %struct_field319
  store float %fmul_tmp320, float* %frame_ms
  call void @print_string(<{ i32, i8* }> %arr_struct_load325)
  %frame_ms326 = load float, float* %frame_ms
  call void @print_f32(float %frame_ms326, i32 4)
  call void @print_string(<{ i32, i8* }> %arr_struct_load331)
  %frame_ms332 = load float, float* %frame_ms
  %fdiv_tmp333 = fdiv float 1.000000e+03, %frame_ms332
  call void @print_f32(float %fdiv_tmp333, i32 4)
  %last_fps334 = load float, float* %last_fps
  %fmul_tmp335 = fmul float 5.000000e-01, %last_fps334
  %frame_ms336 = load float, float* %frame_ms
  %fdiv_tmp337 = fdiv float 1.000000e+03, %frame_ms336
  %fadd_tmp = fadd float %fmul_tmp335, %fdiv_tmp337
  store float %fadd_tmp, float* %last_fps
  call void @print_string(<{ i32, i8* }> %arr_struct_load342)
  %cycles_elapsed343 = load i64, i64* %cycles_elapsed
  %int_to_float_cast344 = sitofp i64 %cycles_elapsed343 to double
  %fdiv_tmp345 = fdiv double %int_to_float_cast344, 1.000000e+06
  call void @print_f64(double %fdiv_tmp345, i32 4)
  call void @print_string(<{ i32, i8* }> %arr_struct_load350)
  %struct_field_ptr356 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca172, i32 0, i32 0
  %struct_field_ptr357 = getelementptr inbounds <{ float, float }>, <{ float, float }>* %struct_field_ptr356, i32 0, i32 0
  %struct_field358 = load float, float* %struct_field_ptr357
  call void @debug_print_f32(<{ i32, i8* }> %arr_struct_load355, float %struct_field358, i32 4)
  br label %endif312

endif312:                                         ; preds = %then311, %then309
  call void @refresh_game_dll(<{ i64, i1 (i8*, i8*, i8*)*, void (i8*, i8*)*, i1, i64 }>* %game)
  br label %endif310

endif310:                                         ; preds = %endif312, %while_end288
  %current_cycle_count359 = load i64, i64* %current_cycle_count
  store i64 %current_cycle_count359, i64* %last_cycle_count
  br label %while_cond

while_end:                                        ; preds = %while_cond
  call void @ExitProcess(i32 0)
  ret void
}

attributes #0 = { nounwind }
attributes #1 = { nounwind readnone }
attributes #2 = { argmemonly nounwind }
