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
@str_arr.32 = private global [0 x i8] zeroinitializer
@str.33 = private unnamed_addr constant [94 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Chandmade.prag\22, line 78, pos 15)\00"

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

define i32 @__init(i64, i32, i8*) {
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
  ret i32 1
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
define internal <{ <{ float, float }>, <{ float, float }> }> @min_max_rect(i32 %min_x, i32 %min_y, i32 %max_x, i32 %max_y) #0 {
vars:
  %struct_alloca = alloca <{ float, float }>
  %struct_alloca2 = alloca <{ float, float }>
  %struct_alloca8 = alloca <{ <{ float, float }>, <{ float, float }> }>
  br label %entry

entry:                                            ; preds = %vars
  %int_to_float_cast = sitofp i32 %min_x to float
  %struct_arg_0 = getelementptr inbounds <{ float, float }>, <{ float, float }>* %struct_alloca, i32 0, i32 0
  store float %int_to_float_cast, float* %struct_arg_0
  %int_to_float_cast1 = sitofp i32 %min_y to float
  %struct_arg_1 = getelementptr inbounds <{ float, float }>, <{ float, float }>* %struct_alloca, i32 0, i32 1
  store float %int_to_float_cast1, float* %struct_arg_1
  %sub_tmp = sub i32 %max_x, %min_x
  %int_to_float_cast3 = sitofp i32 %sub_tmp to float
  %struct_arg_04 = getelementptr inbounds <{ float, float }>, <{ float, float }>* %struct_alloca2, i32 0, i32 0
  store float %int_to_float_cast3, float* %struct_arg_04
  %sub_tmp5 = sub i32 %max_y, %min_y
  %int_to_float_cast6 = sitofp i32 %sub_tmp5 to float
  %struct_arg_17 = getelementptr inbounds <{ float, float }>, <{ float, float }>* %struct_alloca2, i32 0, i32 1
  store float %int_to_float_cast6, float* %struct_arg_17
  %pos = load <{ float, float }>, <{ float, float }>* %struct_alloca
  %struct_arg_09 = getelementptr inbounds <{ <{ float, float }>, <{ float, float }> }>, <{ <{ float, float }>, <{ float, float }> }>* %struct_alloca8, i32 0, i32 0
  store <{ float, float }> %pos, <{ float, float }>* %struct_arg_09
  %size = load <{ float, float }>, <{ float, float }>* %struct_alloca2
  %struct_arg_110 = getelementptr inbounds <{ <{ float, float }>, <{ float, float }> }>, <{ <{ float, float }>, <{ float, float }> }>* %struct_alloca8, i32 0, i32 1
  store <{ float, float }> %size, <{ float, float }>* %struct_arg_110
  %result = load <{ <{ float, float }>, <{ float, float }> }>, <{ <{ float, float }>, <{ float, float }> }>* %struct_alloca8
  ret <{ <{ float, float }>, <{ float, float }> }> %result
}

; Function Attrs: nounwind
define internal <{ <{ float, float }>, <{ float, float }> }> @center_half_size_rect(i32 %x, i32 %y, i32 %half_size) #0 {
vars:
  %struct_alloca = alloca <{ float, float }>
  %struct_alloca3 = alloca <{ float, float }>
  %struct_alloca9 = alloca <{ <{ float, float }>, <{ float, float }> }>
  br label %entry

entry:                                            ; preds = %vars
  %sub_tmp = sub i32 %x, %half_size
  %int_to_float_cast = sitofp i32 %sub_tmp to float
  %struct_arg_0 = getelementptr inbounds <{ float, float }>, <{ float, float }>* %struct_alloca, i32 0, i32 0
  store float %int_to_float_cast, float* %struct_arg_0
  %sub_tmp1 = sub i32 %y, %half_size
  %int_to_float_cast2 = sitofp i32 %sub_tmp1 to float
  %struct_arg_1 = getelementptr inbounds <{ float, float }>, <{ float, float }>* %struct_alloca, i32 0, i32 1
  store float %int_to_float_cast2, float* %struct_arg_1
  %mul_tmp = mul i32 2, %half_size
  %int_to_float_cast4 = sitofp i32 %mul_tmp to float
  %struct_arg_05 = getelementptr inbounds <{ float, float }>, <{ float, float }>* %struct_alloca3, i32 0, i32 0
  store float %int_to_float_cast4, float* %struct_arg_05
  %mul_tmp6 = mul i32 2, %half_size
  %int_to_float_cast7 = sitofp i32 %mul_tmp6 to float
  %struct_arg_18 = getelementptr inbounds <{ float, float }>, <{ float, float }>* %struct_alloca3, i32 0, i32 1
  store float %int_to_float_cast7, float* %struct_arg_18
  %pos = load <{ float, float }>, <{ float, float }>* %struct_alloca
  %struct_arg_010 = getelementptr inbounds <{ <{ float, float }>, <{ float, float }> }>, <{ <{ float, float }>, <{ float, float }> }>* %struct_alloca9, i32 0, i32 0
  store <{ float, float }> %pos, <{ float, float }>* %struct_arg_010
  %size = load <{ float, float }>, <{ float, float }>* %struct_alloca3
  %struct_arg_111 = getelementptr inbounds <{ <{ float, float }>, <{ float, float }> }>, <{ <{ float, float }>, <{ float, float }> }>* %struct_alloca9, i32 0, i32 1
  store <{ float, float }> %size, <{ float, float }>* %struct_arg_111
  %result = load <{ <{ float, float }>, <{ float, float }> }>, <{ <{ float, float }>, <{ float, float }> }>* %struct_alloca9
  ret <{ <{ float, float }>, <{ float, float }> }> %result
}

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
define dllexport i1 @game_update_and_render(<{ i8*, i64, i8*, i64, i8* }>* %memory, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %input, <{ i8*, i32, i32, i32 }>* %render_target) #0 {
vars:
  %result = alloca i1
  %game_state = alloca <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>*
  %t = alloca float
  %struct_alloca = alloca <{ float, float, float, float }>
  %struct_alloca29 = alloca <{ float, float, float, float }>
  br label %entry

entry:                                            ; preds = %vars
  store i1 true, i1* %result
  %fun_call = call <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>* @get_game_state(<{ i8*, i64, i8*, i64, i8* }>* %memory)
  store <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>* %fun_call, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>** %game_state
  %game_state1 = load <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>*, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>** %game_state
  call void @handle_player_input(<{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>* %game_state1, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %input)
  %struct_arrow_load = load <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>*, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>** %game_state
  %struct_field_ptr = getelementptr inbounds <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow_load, i32 0, i32 8
  %struct_field = load i1, i1* %struct_field_ptr
  br i1 %struct_field, label %then, label %endif

then:                                             ; preds = %entry
  %struct_field_ptr2 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %input, i32 0, i32 0
  %struct_field_ptr3 = getelementptr inbounds <{ float, float }>, <{ float, float }>* %struct_field_ptr2, i32 0, i32 1
  %struct_field4 = load float, float* %struct_field_ptr3
  %struct_arrow_load5 = load <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>*, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>** %game_state
  %struct_field_ptr6 = getelementptr inbounds <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow_load5, i32 0, i32 9
  %struct_field7 = load float, float* %struct_field_ptr6
  %fsub_tmp = fsub float %struct_field4, %struct_field7
  %fcmp_tmp = fcmp ogt float %fsub_tmp, 2.500000e-01
  br i1 %fcmp_tmp, label %then8, label %else

then8:                                            ; preds = %then
  store i1 false, i1* %result
  %struct_arrow_load10 = load <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>*, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>** %game_state
  %struct_field_ptr11 = getelementptr inbounds <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow_load10, i32 0, i32 2
  store float 0.000000e+00, float* %struct_field_ptr11
  br label %endif9

else:                                             ; preds = %then
  %struct_field_ptr12 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %input, i32 0, i32 0
  %struct_field_ptr13 = getelementptr inbounds <{ float, float }>, <{ float, float }>* %struct_field_ptr12, i32 0, i32 1
  %struct_field14 = load float, float* %struct_field_ptr13
  %struct_arrow_load15 = load <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>*, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>** %game_state
  %struct_field_ptr16 = getelementptr inbounds <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow_load15, i32 0, i32 9
  %struct_field17 = load float, float* %struct_field_ptr16
  %fsub_tmp18 = fsub float %struct_field14, %struct_field17
  %fdiv_tmp = fdiv float %fsub_tmp18, 2.500000e-01
  store float %fdiv_tmp, float* %t
  %struct_arrow_load19 = load <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>*, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>** %game_state
  %struct_field_ptr20 = getelementptr inbounds <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow_load19, i32 0, i32 11
  %t21 = load float, float* %t
  store float %t21, float* %struct_field_ptr20
  %struct_arrow_load22 = load <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>*, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>** %game_state
  %struct_field_ptr23 = getelementptr inbounds <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow_load22, i32 0, i32 2
  %struct_arrow_load24 = load <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>*, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>** %game_state
  %struct_field_ptr25 = getelementptr inbounds <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow_load24, i32 0, i32 10
  %struct_field26 = load float, float* %struct_field_ptr25
  %t27 = load float, float* %t
  %fun_call28 = call float @lerp(float %struct_field26, float 0.000000e+00, float %t27)
  store float %fun_call28, float* %struct_field_ptr23
  br label %endif9

endif9:                                           ; preds = %else, %then8
  br label %endif

endif:                                            ; preds = %endif9, %entry
  %struct_arg_0 = getelementptr inbounds <{ float, float, float, float }>, <{ float, float, float, float }>* %struct_alloca, i32 0, i32 0
  store float 0x3FC99999A0000000, float* %struct_arg_0
  %struct_arg_1 = getelementptr inbounds <{ float, float, float, float }>, <{ float, float, float, float }>* %struct_alloca, i32 0, i32 1
  store float 0x3FC99999A0000000, float* %struct_arg_1
  %struct_arg_2 = getelementptr inbounds <{ float, float, float, float }>, <{ float, float, float, float }>* %struct_alloca, i32 0, i32 2
  store float 0x3FC99999A0000000, float* %struct_arg_2
  %struct_arg_3 = getelementptr inbounds <{ float, float, float, float }>, <{ float, float, float, float }>* %struct_alloca, i32 0, i32 3
  store float 1.000000e+00, float* %struct_arg_3
  %struct_arg_030 = getelementptr inbounds <{ float, float, float, float }>, <{ float, float, float, float }>* %struct_alloca29, i32 0, i32 0
  store float 1.000000e+00, float* %struct_arg_030
  %struct_arg_131 = getelementptr inbounds <{ float, float, float, float }>, <{ float, float, float, float }>* %struct_alloca29, i32 0, i32 1
  store float 0.000000e+00, float* %struct_arg_131
  %struct_arg_232 = getelementptr inbounds <{ float, float, float, float }>, <{ float, float, float, float }>* %struct_alloca29, i32 0, i32 2
  store float 0.000000e+00, float* %struct_arg_232
  %struct_arg_333 = getelementptr inbounds <{ float, float, float, float }>, <{ float, float, float, float }>* %struct_alloca29, i32 0, i32 3
  store float 1.000000e+00, float* %struct_arg_333
  %struct_field_ptr34 = getelementptr inbounds <{ i8*, i32, i32, i32 }>, <{ i8*, i32, i32, i32 }>* %render_target, i32 0, i32 1
  %struct_arrow = load i32, i32* %struct_field_ptr34
  %struct_field_ptr35 = getelementptr inbounds <{ i8*, i32, i32, i32 }>, <{ i8*, i32, i32, i32 }>* %render_target, i32 0, i32 2
  %struct_arrow36 = load i32, i32* %struct_field_ptr35
  %fun_call37 = call <{ <{ float, float }>, <{ float, float }> }> @min_max_rect(i32 0, i32 0, i32 %struct_arrow, i32 %struct_arrow36)
  %background_color = load <{ float, float, float, float }>, <{ float, float, float, float }>* %struct_alloca
  call void @draw_rectangle(<{ i8*, i32, i32, i32 }>* %render_target, <{ <{ float, float }>, <{ float, float }> }> %fun_call37, <{ float, float, float, float }> %background_color)
  %struct_field_ptr38 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %input, i32 0, i32 5
  %struct_field_ptr39 = getelementptr inbounds <{ i1, i1, i1 }>, <{ i1, i1, i1 }>* %struct_field_ptr38, i32 0, i32 0
  %struct_field40 = load i1, i1* %struct_field_ptr39
  %not_tmp = xor i1 %struct_field40, true
  br i1 %not_tmp, label %then41, label %else42

then41:                                           ; preds = %endif
  %struct_field_ptr44 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %input, i32 0, i32 8
  %struct_arrow45 = load i32, i32* %struct_field_ptr44
  %struct_field_ptr46 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %input, i32 0, i32 9
  %struct_arrow47 = load i32, i32* %struct_field_ptr46
  %fun_call48 = call <{ <{ float, float }>, <{ float, float }> }> @center_half_size_rect(i32 %struct_arrow45, i32 %struct_arrow47, i32 15)
  %red = load <{ float, float, float, float }>, <{ float, float, float, float }>* %struct_alloca29
  call void @draw_rectangle(<{ i8*, i32, i32, i32 }>* %render_target, <{ <{ float, float }>, <{ float, float }> }> %fun_call48, <{ float, float, float, float }> %red)
  br label %endif43

else42:                                           ; preds = %endif
  %struct_field_ptr49 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %input, i32 0, i32 8
  %struct_arrow50 = load i32, i32* %struct_field_ptr49
  %struct_field_ptr51 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %input, i32 0, i32 9
  %struct_arrow52 = load i32, i32* %struct_field_ptr51
  %fun_call53 = call <{ <{ float, float }>, <{ float, float }> }> @center_half_size_rect(i32 %struct_arrow50, i32 %struct_arrow52, i32 20)
  %red54 = load <{ float, float, float, float }>, <{ float, float, float, float }>* %struct_alloca29
  call void @draw_rectangle(<{ i8*, i32, i32, i32 }>* %render_target, <{ <{ float, float }>, <{ float, float }> }> %fun_call53, <{ float, float, float, float }> %red54)
  br label %endif43

endif43:                                          ; preds = %else42, %then41
  call void @consume_buttons(<{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %input)
  %result55 = load i1, i1* %result
  ret i1 %result55
}

; Function Attrs: nounwind
define dllexport void @game_output_sound(<{ i8*, i64, i8*, i64, i8* }>* %game_memory, <{ i16*, i32, i32, float }>* %sound_output) #0 {
vars:
  %game_state = alloca <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>*
  %wave_period = alloca float
  %sample = alloca i16*
  %arr_struct_alloca = alloca <{ i32, i8* }>
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.32, i32 0, i32 0), i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr
  %gep_arr_elem_ptr2 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 1
  store i8* getelementptr inbounds ([0 x i8], [0 x i8]* @str_arr.32, i32 0, i32 0), i8** %gep_arr_elem_ptr2
  %arr_struct_load = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca
  %arr_struct_alloca3 = alloca <{ i32, i8* }>
  %arr_elem_alloca = alloca i8, i32 93
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca, i8* getelementptr inbounds ([94 x i8], [94 x i8]* @str.33, i32 0, i32 0), i32 93, i32 0, i1 false)
  %gep_arr_elem_ptr4 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca3, i32 0, i32 0
  store i32 93, i32* %gep_arr_elem_ptr4
  %gep_arr_elem_ptr5 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca3, i32 0, i32 1
  store i8* %arr_elem_alloca, i8** %gep_arr_elem_ptr5
  %arr_struct_load6 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca3
  %t_sine = alloca float
  %phase_shift = alloca float
  %delta_t = alloca float
  %i = alloca i32
  %t = alloca float
  %v = alloca float
  %p = alloca float
  %x = alloca i16
  br label %entry

entry:                                            ; preds = %vars
  %fun_call = call <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>* @get_game_state(<{ i8*, i64, i8*, i64, i8* }>* %game_memory)
  store <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>* %fun_call, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>** %game_state
  store float 0x401921FB60000000, float* %wave_period
  %struct_field_ptr = getelementptr inbounds <{ i16*, i32, i32, float }>, <{ i16*, i32, i32, float }>* %sound_output, i32 0, i32 0
  %struct_arrow = load i16*, i16** %struct_field_ptr
  store i16* %struct_arrow, i16** %sample
  %sample1 = load i16*, i16** %sample
  %pointer_bit_cast = bitcast i16* %sample1 to i8*
  %icmp_tmp = icmp ne i8* %pointer_bit_cast, null
  call void @assert(i1 %icmp_tmp, <{ i32, i8* }> %arr_struct_load, <{ i32, i8* }> %arr_struct_load6)
  %struct_arrow_load = load <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>*, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>** %game_state
  %struct_field_ptr7 = getelementptr inbounds <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow_load, i32 0, i32 6
  %struct_field = load float, float* %struct_field_ptr7
  store float %struct_field, float* %t_sine
  %t_sine8 = load float, float* %t_sine
  %fmul_tmp = fmul float 0x401921FB60000000, %t_sine8
  %struct_arrow_load9 = load <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>*, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>** %game_state
  %struct_field_ptr10 = getelementptr inbounds <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow_load9, i32 0, i32 3
  %struct_field11 = load float, float* %struct_field_ptr10
  %struct_arrow_load12 = load <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>*, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>** %game_state
  %struct_field_ptr13 = getelementptr inbounds <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow_load12, i32 0, i32 4
  %struct_field14 = load float, float* %struct_field_ptr13
  %fsub_tmp = fsub float %struct_field11, %struct_field14
  %fmul_tmp15 = fmul float %fmul_tmp, %fsub_tmp
  %struct_arrow_load16 = load <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>*, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>** %game_state
  %struct_field_ptr17 = getelementptr inbounds <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow_load16, i32 0, i32 5
  %struct_field18 = load float, float* %struct_field_ptr17
  %fadd_tmp = fadd float %fmul_tmp15, %struct_field18
  store float %fadd_tmp, float* %phase_shift
  %struct_arrow_load19 = load <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>*, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>** %game_state
  %struct_field_ptr20 = getelementptr inbounds <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow_load19, i32 0, i32 3
  %struct_arrow_load21 = load <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>*, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>** %game_state
  %struct_field_ptr22 = getelementptr inbounds <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow_load21, i32 0, i32 4
  %struct_field23 = load float, float* %struct_field_ptr22
  store float %struct_field23, float* %struct_field_ptr20
  %struct_field_ptr24 = getelementptr inbounds <{ i16*, i32, i32, float }>, <{ i16*, i32, i32, float }>* %sound_output, i32 0, i32 2
  %struct_arrow25 = load i32, i32* %struct_field_ptr24
  %int_to_float_cast = sitofp i32 %struct_arrow25 to float
  %fdiv_tmp = fdiv float 1.000000e+00, %int_to_float_cast
  store float %fdiv_tmp, float* %delta_t
  store i32 0, i32* %i
  br label %for_cond

for_cond:                                         ; preds = %for_iter, %entry
  %i26 = load i32, i32* %i
  %struct_field_ptr27 = getelementptr inbounds <{ i16*, i32, i32, float }>, <{ i16*, i32, i32, float }>* %sound_output, i32 0, i32 1
  %struct_arrow28 = load i32, i32* %struct_field_ptr27
  %icmp_tmp29 = icmp slt i32 %i26, %struct_arrow28
  br i1 %icmp_tmp29, label %for, label %end_for

for:                                              ; preds = %for_cond
  %i30 = load i32, i32* %i
  %int_to_float_cast31 = sitofp i32 %i30 to float
  %struct_field_ptr32 = getelementptr inbounds <{ i16*, i32, i32, float }>, <{ i16*, i32, i32, float }>* %sound_output, i32 0, i32 1
  %struct_arrow33 = load i32, i32* %struct_field_ptr32
  %sub_tmp = sub i32 %struct_arrow33, 1
  %int_to_float_cast34 = sitofp i32 %sub_tmp to float
  %fdiv_tmp35 = fdiv float %int_to_float_cast31, %int_to_float_cast34
  store float %fdiv_tmp35, float* %t
  %struct_arrow_load36 = load <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>*, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>** %game_state
  %struct_field_ptr37 = getelementptr inbounds <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow_load36, i32 0, i32 1
  %struct_field38 = load float, float* %struct_field_ptr37
  %struct_arrow_load39 = load <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>*, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>** %game_state
  %struct_field_ptr40 = getelementptr inbounds <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow_load39, i32 0, i32 2
  %struct_field41 = load float, float* %struct_field_ptr40
  %t42 = load float, float* %t
  %fun_call43 = call float @lerp(float %struct_field38, float %struct_field41, float %t42)
  store float %fun_call43, float* %v
  %struct_arrow_load44 = load <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>*, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>** %game_state
  %struct_field_ptr45 = getelementptr inbounds <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow_load44, i32 0, i32 3
  %struct_field46 = load float, float* %struct_field_ptr45
  %wave_period47 = load float, float* %wave_period
  %fmul_tmp48 = fmul float %struct_field46, %wave_period47
  %t_sine49 = load float, float* %t_sine
  %fmul_tmp50 = fmul float %fmul_tmp48, %t_sine49
  store float %fmul_tmp50, float* %p
  %v51 = load float, float* %v
  %fmul_tmp52 = fmul float 3.276700e+04, %v51
  %p53 = load float, float* %p
  %phase_shift54 = load float, float* %phase_shift
  %fadd_tmp55 = fadd float %p53, %phase_shift54
  %fun_call56 = call float @sinf(float %fadd_tmp55)
  %fmul_tmp57 = fmul float %fmul_tmp52, %fun_call56
  %int_cast = fptosi float %fmul_tmp57 to i16
  store i16 %int_cast, i16* %x
  %postinc_load = load i16*, i16** %sample
  %ptr_post_inc = getelementptr i16, i16* %postinc_load, i32 1
  store i16* %ptr_post_inc, i16** %sample
  %x58 = load i16, i16* %x
  store i16 %x58, i16* %postinc_load
  %postinc_load59 = load i16*, i16** %sample
  %ptr_post_inc60 = getelementptr i16, i16* %postinc_load59, i32 1
  store i16* %ptr_post_inc60, i16** %sample
  %x61 = load i16, i16* %x
  store i16 %x61, i16* %postinc_load59
  %t_sine62 = load float, float* %t_sine
  %delta_t63 = load float, float* %delta_t
  %fadd_tmp64 = fadd float %t_sine62, %delta_t63
  store float %fadd_tmp64, float* %t_sine
  br label %for_iter

for_iter:                                         ; preds = %for
  %preinc_load = load i32, i32* %i
  %preinc = add i32 %preinc_load, 1
  store i32 %preinc, i32* %i
  br label %for_cond

end_for:                                          ; preds = %for_cond
  %struct_arrow_load65 = load <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>*, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>** %game_state
  %struct_field_ptr66 = getelementptr inbounds <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow_load65, i32 0, i32 6
  %t_sine67 = load float, float* %t_sine
  store float %t_sine67, float* %struct_field_ptr66
  %struct_arrow_load68 = load <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>*, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>** %game_state
  %struct_field_ptr69 = getelementptr inbounds <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow_load68, i32 0, i32 5
  %phase_shift70 = load float, float* %phase_shift
  store float %phase_shift70, float* %struct_field_ptr69
  %struct_arrow_load71 = load <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>*, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>** %game_state
  %struct_field_ptr72 = getelementptr inbounds <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow_load71, i32 0, i32 1
  %struct_arrow_load73 = load <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>*, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>** %game_state
  %struct_field_ptr74 = getelementptr inbounds <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow_load73, i32 0, i32 2
  %struct_field75 = load float, float* %struct_field_ptr74
  store float %struct_field75, float* %struct_field_ptr72
  ret void
}

; Function Attrs: nounwind
define internal <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>* @get_game_state(<{ i8*, i64, i8*, i64, i8* }>* %memory) #0 {
vars:
  %game_state = alloca <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>*
  br label %entry

entry:                                            ; preds = %vars
  %struct_field_ptr = getelementptr inbounds <{ i8*, i64, i8*, i64, i8* }>, <{ i8*, i64, i8*, i64, i8* }>* %memory, i32 0, i32 2
  %struct_arrow = load i8*, i8** %struct_field_ptr
  %pointer_bit_cast = bitcast i8* %struct_arrow to <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>*
  store <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>* %pointer_bit_cast, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>** %game_state
  %struct_arrow_load = load <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>*, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>** %game_state
  %struct_field_ptr1 = getelementptr inbounds <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow_load, i32 0, i32 0
  %struct_field = load i1, i1* %struct_field_ptr1
  %not_tmp = xor i1 %struct_field, true
  br i1 %not_tmp, label %then, label %endif

then:                                             ; preds = %entry
  %struct_arrow_load2 = load <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>*, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>** %game_state
  %struct_field_ptr3 = getelementptr inbounds <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow_load2, i32 0, i32 3
  store float 2.560000e+02, float* %struct_field_ptr3
  %struct_arrow_load4 = load <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>*, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>** %game_state
  %struct_field_ptr5 = getelementptr inbounds <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow_load4, i32 0, i32 4
  store float 1.280000e+02, float* %struct_field_ptr5
  %struct_arrow_load6 = load <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>*, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>** %game_state
  %struct_field_ptr7 = getelementptr inbounds <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow_load6, i32 0, i32 2
  store float 0.000000e+00, float* %struct_field_ptr7
  %struct_arrow_load8 = load <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>*, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>** %game_state
  %struct_field_ptr9 = getelementptr inbounds <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow_load8, i32 0, i32 0
  store i1 true, i1* %struct_field_ptr9
  br label %endif

endif:                                            ; preds = %then, %entry
  %game_state10 = load <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>*, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>** %game_state
  ret <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>* %game_state10
}

; Function Attrs: nounwind
define internal void @handle_player_input(<{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>* %game_state, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %game_input) #0 {
vars:
  br label %entry

entry:                                            ; preds = %vars
  %struct_field_ptr = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %game_input, i32 0, i32 11
  %struct_arrow = load i1, i1* %struct_field_ptr
  br i1 %struct_arrow, label %cand.rhs, label %cand.end

cand.rhs:                                         ; preds = %entry
  %struct_field_ptr1 = getelementptr inbounds <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>* %game_state, i32 0, i32 8
  %struct_arrow2 = load i1, i1* %struct_field_ptr1
  %not_tmp = xor i1 %struct_arrow2, true
  br label %cand.end

cand.end:                                         ; preds = %cand.rhs, %entry
  %candphi = phi i1 [ false, %entry ], [ %not_tmp, %cand.rhs ]
  br i1 %candphi, label %then, label %endif

then:                                             ; preds = %cand.end
  %struct_field_ptr3 = getelementptr inbounds <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>* %game_state, i32 0, i32 8
  store i1 true, i1* %struct_field_ptr3
  %struct_field_ptr4 = getelementptr inbounds <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>* %game_state, i32 0, i32 9
  %struct_field_ptr5 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %game_input, i32 0, i32 0
  %struct_field_ptr6 = getelementptr inbounds <{ float, float }>, <{ float, float }>* %struct_field_ptr5, i32 0, i32 1
  %struct_field = load float, float* %struct_field_ptr6
  store float %struct_field, float* %struct_field_ptr4
  %struct_field_ptr7 = getelementptr inbounds <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>* %game_state, i32 0, i32 10
  %struct_field_ptr8 = getelementptr inbounds <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, i32, i1, float, float, float }>* %game_state, i32 0, i32 1
  %struct_arrow9 = load float, float* %struct_field_ptr8
  store float %struct_arrow9, float* %struct_field_ptr7
  br label %endif

endif:                                            ; preds = %then, %cand.end
  ret void
}

; Function Attrs: nounwind
define internal void @consume_buttons(<{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %input) #0 {
vars:
  br label %entry

entry:                                            ; preds = %vars
  %struct_field_ptr = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %input, i32 0, i32 1
  call void @consume_button(<{ i1, i1, i1 }>* %struct_field_ptr)
  %struct_field_ptr1 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %input, i32 0, i32 4
  call void @consume_button(<{ i1, i1, i1 }>* %struct_field_ptr1)
  %struct_field_ptr2 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %input, i32 0, i32 2
  call void @consume_button(<{ i1, i1, i1 }>* %struct_field_ptr2)
  %struct_field_ptr3 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %input, i32 0, i32 3
  call void @consume_button(<{ i1, i1, i1 }>* %struct_field_ptr3)
  %struct_field_ptr4 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %input, i32 0, i32 5
  call void @consume_button(<{ i1, i1, i1 }>* %struct_field_ptr4)
  %struct_field_ptr5 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %input, i32 0, i32 6
  call void @consume_button(<{ i1, i1, i1 }>* %struct_field_ptr5)
  %struct_field_ptr6 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %input, i32 0, i32 7
  call void @consume_button(<{ i1, i1, i1 }>* %struct_field_ptr6)
  ret void
}

; Function Attrs: nounwind
define internal void @consume_button(<{ i1, i1, i1 }>* %button) #0 {
vars:
  br label %entry

entry:                                            ; preds = %vars
  %struct_field_ptr = getelementptr inbounds <{ i1, i1, i1 }>, <{ i1, i1, i1 }>* %button, i32 0, i32 1
  store i1 false, i1* %struct_field_ptr
  %struct_field_ptr1 = getelementptr inbounds <{ i1, i1, i1 }>, <{ i1, i1, i1 }>* %button, i32 0, i32 2
  store i1 false, i1* %struct_field_ptr1
  ret void
}

; Function Attrs: nounwind
define internal void @draw_rectangle(<{ i8*, i32, i32, i32 }>* %buffer, <{ <{ float, float }>, <{ float, float }> }> %rect, <{ float, float, float, float }> %color) #0 {
vars:
  %min_x_p = alloca i32
  %min_y_p = alloca i32
  %max_x_p = alloca i32
  %max_y_p = alloca i32
  %r = alloca i32
  %g = alloca i32
  %b = alloca i32
  %c = alloca i32
  %row = alloca i8*
  %y = alloca i32
  %pixel = alloca i32*
  %x = alloca i32
  br label %entry

entry:                                            ; preds = %vars
  %struct_field_extract = extractvalue <{ <{ float, float }>, <{ float, float }> }> %rect, 0
  %struct_field_extract1 = extractvalue <{ float, float }> %struct_field_extract, 0
  %fun_call = call float @llvm.round.f32(float %struct_field_extract1)
  %int_cast = fptosi float %fun_call to i32
  store i32 %int_cast, i32* %min_x_p
  %struct_field_extract2 = extractvalue <{ <{ float, float }>, <{ float, float }> }> %rect, 0
  %struct_field_extract3 = extractvalue <{ float, float }> %struct_field_extract2, 1
  %fun_call4 = call float @llvm.round.f32(float %struct_field_extract3)
  %int_cast5 = fptosi float %fun_call4 to i32
  store i32 %int_cast5, i32* %min_y_p
  %struct_field_extract6 = extractvalue <{ <{ float, float }>, <{ float, float }> }> %rect, 0
  %struct_field_extract7 = extractvalue <{ float, float }> %struct_field_extract6, 0
  %struct_field_extract8 = extractvalue <{ <{ float, float }>, <{ float, float }> }> %rect, 1
  %struct_field_extract9 = extractvalue <{ float, float }> %struct_field_extract8, 0
  %fadd_tmp = fadd float %struct_field_extract7, %struct_field_extract9
  %fun_call10 = call float @llvm.round.f32(float %fadd_tmp)
  %int_cast11 = fptosi float %fun_call10 to i32
  store i32 %int_cast11, i32* %max_x_p
  %struct_field_extract12 = extractvalue <{ <{ float, float }>, <{ float, float }> }> %rect, 0
  %struct_field_extract13 = extractvalue <{ float, float }> %struct_field_extract12, 1
  %struct_field_extract14 = extractvalue <{ <{ float, float }>, <{ float, float }> }> %rect, 1
  %struct_field_extract15 = extractvalue <{ float, float }> %struct_field_extract14, 1
  %fadd_tmp16 = fadd float %struct_field_extract13, %struct_field_extract15
  %fun_call17 = call float @llvm.round.f32(float %fadd_tmp16)
  %int_cast18 = fptosi float %fun_call17 to i32
  store i32 %int_cast18, i32* %max_y_p
  %min_x_p19 = load i32, i32* %min_x_p
  %icmp_tmp = icmp slt i32 %min_x_p19, 0
  br i1 %icmp_tmp, label %then, label %endif

then:                                             ; preds = %entry
  store i32 0, i32* %min_x_p
  br label %endif

endif:                                            ; preds = %then, %entry
  %min_y_p20 = load i32, i32* %min_y_p
  %icmp_tmp21 = icmp slt i32 %min_y_p20, 0
  br i1 %icmp_tmp21, label %then22, label %endif23

then22:                                           ; preds = %endif
  store i32 0, i32* %min_y_p
  br label %endif23

endif23:                                          ; preds = %then22, %endif
  %max_x_p24 = load i32, i32* %max_x_p
  %struct_field_ptr = getelementptr inbounds <{ i8*, i32, i32, i32 }>, <{ i8*, i32, i32, i32 }>* %buffer, i32 0, i32 1
  %struct_arrow = load i32, i32* %struct_field_ptr
  %icmp_tmp25 = icmp sgt i32 %max_x_p24, %struct_arrow
  br i1 %icmp_tmp25, label %then26, label %endif27

then26:                                           ; preds = %endif23
  %struct_field_ptr28 = getelementptr inbounds <{ i8*, i32, i32, i32 }>, <{ i8*, i32, i32, i32 }>* %buffer, i32 0, i32 1
  %struct_arrow29 = load i32, i32* %struct_field_ptr28
  store i32 %struct_arrow29, i32* %max_x_p
  br label %endif27

endif27:                                          ; preds = %then26, %endif23
  %max_y_p30 = load i32, i32* %max_y_p
  %struct_field_ptr31 = getelementptr inbounds <{ i8*, i32, i32, i32 }>, <{ i8*, i32, i32, i32 }>* %buffer, i32 0, i32 2
  %struct_arrow32 = load i32, i32* %struct_field_ptr31
  %icmp_tmp33 = icmp sgt i32 %max_y_p30, %struct_arrow32
  br i1 %icmp_tmp33, label %then34, label %endif35

then34:                                           ; preds = %endif27
  %struct_field_ptr36 = getelementptr inbounds <{ i8*, i32, i32, i32 }>, <{ i8*, i32, i32, i32 }>* %buffer, i32 0, i32 2
  %struct_arrow37 = load i32, i32* %struct_field_ptr36
  store i32 %struct_arrow37, i32* %max_y_p
  br label %endif35

endif35:                                          ; preds = %then34, %endif27
  %struct_field_extract38 = extractvalue <{ float, float, float, float }> %color, 0
  %fmul_tmp = fmul float %struct_field_extract38, 2.550000e+02
  %fun_call39 = call float @llvm.round.f32(float %fmul_tmp)
  %int_cast40 = fptosi float %fun_call39 to i32
  store i32 %int_cast40, i32* %r
  %r41 = load i32, i32* %r
  %fun_call42 = call i32 @clamp_i32(i32 %r41, i32 0, i32 255)
  store i32 %fun_call42, i32* %r
  %struct_field_extract43 = extractvalue <{ float, float, float, float }> %color, 1
  %fmul_tmp44 = fmul float %struct_field_extract43, 2.550000e+02
  %fun_call45 = call float @llvm.round.f32(float %fmul_tmp44)
  %int_cast46 = fptosi float %fun_call45 to i32
  store i32 %int_cast46, i32* %g
  %g47 = load i32, i32* %g
  %fun_call48 = call i32 @clamp_i32(i32 %g47, i32 0, i32 255)
  store i32 %fun_call48, i32* %g
  %struct_field_extract49 = extractvalue <{ float, float, float, float }> %color, 2
  %fmul_tmp50 = fmul float %struct_field_extract49, 2.550000e+02
  %fun_call51 = call float @llvm.round.f32(float %fmul_tmp50)
  %int_cast52 = fptosi float %fun_call51 to i32
  store i32 %int_cast52, i32* %b
  %b53 = load i32, i32* %b
  %fun_call54 = call i32 @clamp_i32(i32 %b53, i32 0, i32 255)
  store i32 %fun_call54, i32* %b
  %r55 = load i32, i32* %r
  %shl_tmp = shl i32 %r55, 16
  %g56 = load i32, i32* %g
  %shl_tmp57 = shl i32 %g56, 8
  %or_tmp = or i32 %shl_tmp, %shl_tmp57
  %b58 = load i32, i32* %b
  %shl_tmp59 = shl i32 %b58, 0
  %or_tmp60 = or i32 %or_tmp, %shl_tmp59
  store i32 %or_tmp60, i32* %c
  %struct_field_ptr61 = getelementptr inbounds <{ i8*, i32, i32, i32 }>, <{ i8*, i32, i32, i32 }>* %buffer, i32 0, i32 0
  %struct_arrow62 = load i8*, i8** %struct_field_ptr61
  store i8* %struct_arrow62, i8** %row
  %row63 = load i8*, i8** %row
  %min_y_p64 = load i32, i32* %min_y_p
  %struct_field_ptr65 = getelementptr inbounds <{ i8*, i32, i32, i32 }>, <{ i8*, i32, i32, i32 }>* %buffer, i32 0, i32 3
  %struct_arrow66 = load i32, i32* %struct_field_ptr65
  %mul_tmp = mul i32 %min_y_p64, %struct_arrow66
  %ptr_add = getelementptr i8, i8* %row63, i32 %mul_tmp
  store i8* %ptr_add, i8** %row
  %row67 = load i8*, i8** %row
  %min_x_p68 = load i32, i32* %min_x_p
  %mul_tmp69 = mul i32 4, %min_x_p68
  %ptr_add70 = getelementptr i8, i8* %row67, i32 %mul_tmp69
  store i8* %ptr_add70, i8** %row
  %min_y_p71 = load i32, i32* %min_y_p
  store i32 %min_y_p71, i32* %y
  br label %for_cond

for_cond:                                         ; preds = %for_iter, %endif35
  %y72 = load i32, i32* %y
  %max_y_p73 = load i32, i32* %max_y_p
  %icmp_tmp74 = icmp slt i32 %y72, %max_y_p73
  br i1 %icmp_tmp74, label %for, label %end_for

for:                                              ; preds = %for_cond
  %row75 = load i8*, i8** %row
  %pointer_bit_cast = bitcast i8* %row75 to i32*
  store i32* %pointer_bit_cast, i32** %pixel
  %min_x_p80 = load i32, i32* %min_x_p
  store i32 %min_x_p80, i32* %x
  br label %for_cond76

for_cond76:                                       ; preds = %for_iter78, %for
  %x81 = load i32, i32* %x
  %max_x_p82 = load i32, i32* %max_x_p
  %icmp_tmp83 = icmp slt i32 %x81, %max_x_p82
  br i1 %icmp_tmp83, label %for77, label %end_for79

for77:                                            ; preds = %for_cond76
  %postinc_load = load i32*, i32** %pixel
  %ptr_post_inc = getelementptr i32, i32* %postinc_load, i32 1
  store i32* %ptr_post_inc, i32** %pixel
  %c84 = load i32, i32* %c
  store i32 %c84, i32* %postinc_load
  br label %for_iter78

for_iter78:                                       ; preds = %for77
  %preinc_load = load i32, i32* %x
  %preinc = add i32 %preinc_load, 1
  store i32 %preinc, i32* %x
  br label %for_cond76

end_for79:                                        ; preds = %for_cond76
  %row85 = load i8*, i8** %row
  %struct_field_ptr86 = getelementptr inbounds <{ i8*, i32, i32, i32 }>, <{ i8*, i32, i32, i32 }>* %buffer, i32 0, i32 3
  %struct_arrow87 = load i32, i32* %struct_field_ptr86
  %ptr_add88 = getelementptr i8, i8* %row85, i32 %struct_arrow87
  store i8* %ptr_add88, i8** %row
  br label %for_iter

for_iter:                                         ; preds = %end_for79
  %preinc_load89 = load i32, i32* %y
  %preinc90 = add i32 %preinc_load89, 1
  store i32 %preinc90, i32* %y
  br label %for_cond

end_for:                                          ; preds = %for_cond
  ret void
}

; Function Attrs: nounwind
define internal void @main() #0 {
vars:
  br label %entry

entry:                                            ; preds = %vars
  ret void
}

attributes #0 = { nounwind }
attributes #1 = { nounwind readnone }
attributes #2 = { argmemonly nounwind }
