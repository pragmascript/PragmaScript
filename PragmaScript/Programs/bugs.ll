; ModuleID = 'd:\Projects\Dotnet\PragmaScript\PragmaScript\bin\x64\Release\Programs\ll\preamble.ll'
target datalayout = "e-m:w-i64:64-f80:128-n8:16:32:64-S128"
target triple = "x86_64-pc-windows-msvc"

@str = private unnamed_addr constant [11 x i8] c"0123456789\00"
@decimal_digits = internal global <{ i32, i8* }> zeroinitializer
@__intrinsics = internal global i1 false
@console_output_handle = internal global i64 0
@console_input_handle = internal global i64 0
@console_error_handle = internal global i64 0
@XInputGetState = internal global i32 (i32, <{ i32, <{ i16, i8, i8, i16, i16, i16, i16 }> }>*)* null
@str.1 = private unnamed_addr constant [1 x i8] zeroinitializer
@str.2 = private unnamed_addr constant [93 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cpreamble.prag\22, line 27, pos 8)\00"
@str.3 = private unnamed_addr constant [93 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cpreamble.prag\22, line 32, pos 8)\00"
@str.4 = private unnamed_addr constant [11 x i8] c"Assertion \00"
@str.5 = private unnamed_addr constant [2 x i8] c"\22\00"
@str.6 = private unnamed_addr constant [3 x i8] c"\22 \00"
@str.7 = private unnamed_addr constant [12 x i8] c"failed at: \00"
@str.8 = private unnamed_addr constant [2 x i8] c"\0A\00"
@str.9 = private unnamed_addr constant [29 x i8] c"c-string not null terminated\00"
@str.10 = private unnamed_addr constant [93 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cpreamble.prag\22, line 74, pos 8)\00"
@str.11 = private unnamed_addr constant [94 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cpreamble.prag\22, line 190, pos 9)\00"
@str.12 = private unnamed_addr constant [65 x i8] c"                                                                \00"
@str.13 = private unnamed_addr constant [94 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cpreamble.prag\22, line 216, pos 8)\00"
@str.14 = private unnamed_addr constant [94 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cpreamble.prag\22, line 240, pos 8)\00"
@str.15 = private unnamed_addr constant [94 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cpreamble.prag\22, line 253, pos 9)\00"
@str.16 = private unnamed_addr constant [94 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cpreamble.prag\22, line 278, pos 8)\00"
@str.17 = private unnamed_addr constant [94 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cpreamble.prag\22, line 299, pos 9)\00"
@str.18 = private unnamed_addr constant [94 x i8] c"(file \22d:\5CProjects\5CDotnet\5CPragmaScript\5CPragmaScript\5CPrograms\5Cpreamble.prag\22, line 305, pos 8)\00"
@str.19 = private unnamed_addr constant [3 x i8] c": \00"
@str.20 = private unnamed_addr constant [5 x i8] c"true\00"
@str.21 = private unnamed_addr constant [6 x i8] c"false\00"

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
  %arr_elem_alloca = alloca i8, i32 10
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca, i8* getelementptr inbounds ([11 x i8], [11 x i8]* @str, i32 0, i32 0), i32 10, i32 0, i1 false)
  %gep_arr_elem_ptr = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 0
  store i32 10, i32* %gep_arr_elem_ptr
  %gep_arr_elem_ptr1 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 1
  store i8* %arr_elem_alloca, i8** %gep_arr_elem_ptr1
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
  %arr_elem_alloca = alloca i8, i32 0
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca, i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr
  %gep_arr_elem_ptr1 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 1
  store i8* %arr_elem_alloca, i8** %gep_arr_elem_ptr1
  %arr_struct_load = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca
  %arr_struct_alloca2 = alloca <{ i32, i8* }>
  %arr_elem_alloca3 = alloca i8, i32 92
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca3, i8* getelementptr inbounds ([93 x i8], [93 x i8]* @str.2, i32 0, i32 0), i32 92, i32 0, i1 false)
  %gep_arr_elem_ptr4 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2, i32 0, i32 0
  store i32 92, i32* %gep_arr_elem_ptr4
  %gep_arr_elem_ptr5 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2, i32 0, i32 1
  store i8* %arr_elem_alloca3, i8** %gep_arr_elem_ptr5
  %arr_struct_load6 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2
  br label %entry

entry:                                            ; preds = %vars
  %console_output_handle = load i64, i64* @console_output_handle
  %icmp_tmp = icmp sgt i64 %console_output_handle, 0
  call void @assert(i1 %icmp_tmp, <{ i32, i8* }> %arr_struct_load, <{ i32, i8* }> %arr_struct_load6)
  %console_output_handle7 = load i64, i64* @console_output_handle
  %struct_field_extract = extractvalue <{ i32, i8* }> %s, 1
  %struct_field_extract8 = extractvalue <{ i32, i8* }> %s, 0
  %fun_call = call i32 @WriteFile(i64 %console_output_handle7, i8* %struct_field_extract, i32 %struct_field_extract8, i32* null, i8* null)
  ret void
}

; Function Attrs: nounwind
define internal void @print_error(<{ i32, i8* }> %s) #0 {
vars:
  %arr_struct_alloca = alloca <{ i32, i8* }>
  %arr_elem_alloca = alloca i8, i32 0
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca, i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr
  %gep_arr_elem_ptr1 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 1
  store i8* %arr_elem_alloca, i8** %gep_arr_elem_ptr1
  %arr_struct_load = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca
  %arr_struct_alloca2 = alloca <{ i32, i8* }>
  %arr_elem_alloca3 = alloca i8, i32 92
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca3, i8* getelementptr inbounds ([93 x i8], [93 x i8]* @str.3, i32 0, i32 0), i32 92, i32 0, i1 false)
  %gep_arr_elem_ptr4 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2, i32 0, i32 0
  store i32 92, i32* %gep_arr_elem_ptr4
  %gep_arr_elem_ptr5 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2, i32 0, i32 1
  store i8* %arr_elem_alloca3, i8** %gep_arr_elem_ptr5
  %arr_struct_load6 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2
  br label %entry

entry:                                            ; preds = %vars
  %console_error_handle = load i64, i64* @console_error_handle
  %icmp_tmp = icmp sgt i64 %console_error_handle, 0
  call void @assert(i1 %icmp_tmp, <{ i32, i8* }> %arr_struct_load, <{ i32, i8* }> %arr_struct_load6)
  %console_error_handle7 = load i64, i64* @console_error_handle
  %struct_field_extract = extractvalue <{ i32, i8* }> %s, 1
  %struct_field_extract8 = extractvalue <{ i32, i8* }> %s, 0
  %fun_call = call i32 @WriteFile(i64 %console_error_handle7, i8* %struct_field_extract, i32 %struct_field_extract8, i32* null, i8* null)
  ret void
}

; Function Attrs: nounwind
define internal void @assert(i1 %value, <{ i32, i8* }> %msg, <{ i32, i8* }> %filepos) #0 {
vars:
  %arr_struct_alloca = alloca <{ i32, i8* }>
  %arr_elem_alloca = alloca i8, i32 10
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca, i8* getelementptr inbounds ([11 x i8], [11 x i8]* @str.4, i32 0, i32 0), i32 10, i32 0, i1 false)
  %gep_arr_elem_ptr = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 0
  store i32 10, i32* %gep_arr_elem_ptr
  %gep_arr_elem_ptr1 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 1
  store i8* %arr_elem_alloca, i8** %gep_arr_elem_ptr1
  %arr_struct_load = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca
  %arr_struct_alloca4 = alloca <{ i32, i8* }>
  %arr_elem_alloca5 = alloca i8
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca5, i8* getelementptr inbounds ([2 x i8], [2 x i8]* @str.5, i32 0, i32 0), i32 1, i32 0, i1 false)
  %gep_arr_elem_ptr6 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca4, i32 0, i32 0
  store i32 1, i32* %gep_arr_elem_ptr6
  %gep_arr_elem_ptr7 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca4, i32 0, i32 1
  store i8* %arr_elem_alloca5, i8** %gep_arr_elem_ptr7
  %arr_struct_load8 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca4
  %arr_struct_alloca9 = alloca <{ i32, i8* }>
  %arr_elem_alloca10 = alloca i8, i32 2
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca10, i8* getelementptr inbounds ([3 x i8], [3 x i8]* @str.6, i32 0, i32 0), i32 2, i32 0, i1 false)
  %gep_arr_elem_ptr11 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca9, i32 0, i32 0
  store i32 2, i32* %gep_arr_elem_ptr11
  %gep_arr_elem_ptr12 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca9, i32 0, i32 1
  store i8* %arr_elem_alloca10, i8** %gep_arr_elem_ptr12
  %arr_struct_load13 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca9
  %arr_struct_alloca14 = alloca <{ i32, i8* }>
  %arr_elem_alloca15 = alloca i8, i32 11
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca15, i8* getelementptr inbounds ([12 x i8], [12 x i8]* @str.7, i32 0, i32 0), i32 11, i32 0, i1 false)
  %gep_arr_elem_ptr16 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca14, i32 0, i32 0
  store i32 11, i32* %gep_arr_elem_ptr16
  %gep_arr_elem_ptr17 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca14, i32 0, i32 1
  store i8* %arr_elem_alloca15, i8** %gep_arr_elem_ptr17
  %arr_struct_load18 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca14
  %arr_struct_alloca19 = alloca <{ i32, i8* }>
  %arr_elem_alloca20 = alloca i8
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca20, i8* getelementptr inbounds ([2 x i8], [2 x i8]* @str.8, i32 0, i32 0), i32 1, i32 0, i1 false)
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
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca, i8* getelementptr inbounds ([29 x i8], [29 x i8]* @str.9, i32 0, i32 0), i32 28, i32 0, i1 false)
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
  %arr_elem_alloca = alloca i8, i32 0
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca, i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr
  %gep_arr_elem_ptr1 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 1
  store i8* %arr_elem_alloca, i8** %gep_arr_elem_ptr1
  %arr_struct_load = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca
  %arr_struct_alloca2 = alloca <{ i32, i8* }>
  %arr_elem_alloca3 = alloca i8, i32 92
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca3, i8* getelementptr inbounds ([93 x i8], [93 x i8]* @str.10, i32 0, i32 0), i32 92, i32 0, i1 false)
  %gep_arr_elem_ptr4 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2, i32 0, i32 0
  store i32 92, i32* %gep_arr_elem_ptr4
  %gep_arr_elem_ptr5 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2, i32 0, i32 1
  store i8* %arr_elem_alloca3, i8** %gep_arr_elem_ptr5
  %arr_struct_load6 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2
  br label %entry

entry:                                            ; preds = %vars
  %struct_field_extract = extractvalue <{ i32, i8* }> %value, 0
  %icmp_tmp = icmp eq i32 %struct_field_extract, 1
  call void @assert(i1 %icmp_tmp, <{ i32, i8* }> %arr_struct_load, <{ i32, i8* }> %arr_struct_load6)
  %gep_arr_elem_ptr7 = extractvalue <{ i32, i8* }> %value, 1
  %gep_arr_elem = getelementptr i8, i8* %gep_arr_elem_ptr7, i32 0
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
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca, i8* getelementptr inbounds ([65 x i8], [65 x i8]* @str.12, i32 0, i32 0), i32 64, i32 0, i1 false)
  %gep_arr_elem_ptr = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 0
  store i32 64, i32* %gep_arr_elem_ptr
  %gep_arr_elem_ptr2 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 1
  store i8* %arr_elem_alloca, i8** %gep_arr_elem_ptr2
  %arr_struct_load = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca
  %arr_struct_alloca5 = alloca <{ i32, i8* }>
  %arr_elem_alloca6 = alloca i8, i32 0
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca6, i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr7 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca5, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr7
  %gep_arr_elem_ptr8 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca5, i32 0, i32 1
  store i8* %arr_elem_alloca6, i8** %gep_arr_elem_ptr8
  %arr_struct_load9 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca5
  %arr_struct_alloca10 = alloca <{ i32, i8* }>
  %arr_elem_alloca11 = alloca i8, i32 93
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca11, i8* getelementptr inbounds ([94 x i8], [94 x i8]* @str.13, i32 0, i32 0), i32 93, i32 0, i1 false)
  %gep_arr_elem_ptr12 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca10, i32 0, i32 0
  store i32 93, i32* %gep_arr_elem_ptr12
  %gep_arr_elem_ptr13 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca10, i32 0, i32 1
  store i8* %arr_elem_alloca11, i8** %gep_arr_elem_ptr13
  %arr_struct_load14 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca10
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
  call void @assert(i1 %icmp_tmp, <{ i32, i8* }> %arr_struct_load9, <{ i32, i8* }> %arr_struct_load14)
  br i1 %signed, label %cand.rhs, label %cand.end

cand.rhs:                                         ; preds = %entry
  %v15 = load i64, i64* %v
  %icmp_tmp16 = icmp slt i64 %v15, 0
  br label %cand.end

cand.end:                                         ; preds = %cand.rhs, %entry
  %candphi = phi i1 [ false, %entry ], [ %icmp_tmp16, %cand.rhs ]
  br i1 %candphi, label %then, label %endif

then:                                             ; preds = %cand.end
  %v17 = load i64, i64* %v
  %neg_tmp = sub i64 0, %v17
  store i64 %neg_tmp, i64* %v
  call void @out_char(<{ <{ i32, i8* }>, i32 }>* %pd, i8 45)
  br label %endif

endif:                                            ; preds = %then, %cand.end
  %v18 = load i64, i64* %v
  call void @u64_to_ascii(<{ <{ i32, i8* }>, i32 }>* %pd, i64 %v18)
  %struct_field_ptr19 = getelementptr inbounds <{ <{ i32, i8* }>, i32 }>, <{ <{ i32, i8* }>, i32 }>* %pd, i32 0, i32 0
  %struct_field_ptr20 = getelementptr inbounds <{ i32, i8* }>, <{ i32, i8* }>* %struct_field_ptr19, i32 0, i32 0
  %struct_field_ptr21 = getelementptr inbounds <{ <{ i32, i8* }>, i32 }>, <{ <{ i32, i8* }>, i32 }>* %pd, i32 0, i32 1
  %struct_field22 = load i32, i32* %struct_field_ptr21
  store i32 %struct_field22, i32* %struct_field_ptr20
  %struct_field_ptr23 = getelementptr inbounds <{ <{ i32, i8* }>, i32 }>, <{ <{ i32, i8* }>, i32 }>* %pd, i32 0, i32 0
  %struct_field24 = load <{ i32, i8* }>, <{ i32, i8* }>* %struct_field_ptr23
  call void @print_string(<{ i32, i8* }> %struct_field24)
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
  %arr_elem_alloca = alloca i8, i32 0
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca, i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr
  %gep_arr_elem_ptr6 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 1
  store i8* %arr_elem_alloca, i8** %gep_arr_elem_ptr6
  %arr_struct_load = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca
  %arr_struct_alloca7 = alloca <{ i32, i8* }>
  %arr_elem_alloca8 = alloca i8, i32 93
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca8, i8* getelementptr inbounds ([94 x i8], [94 x i8]* @str.14, i32 0, i32 0), i32 93, i32 0, i1 false)
  %gep_arr_elem_ptr9 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca7, i32 0, i32 0
  store i32 93, i32* %gep_arr_elem_ptr9
  %gep_arr_elem_ptr10 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca7, i32 0, i32 1
  store i8* %arr_elem_alloca8, i8** %gep_arr_elem_ptr10
  %arr_struct_load11 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca7
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
  call void @assert(i1 %candphi, <{ i32, i8* }> %arr_struct_load, <{ i32, i8* }> %arr_struct_load11)
  %struct_field_ptr12 = getelementptr inbounds <{ <{ i32, i8* }>, i32 }>, <{ <{ i32, i8* }>, i32 }>* %dest, i32 0, i32 0
  %struct_field_ptr13 = getelementptr inbounds <{ <{ i32, i8* }>, i32 }>, <{ <{ i32, i8* }>, i32 }>* %dest, i32 0, i32 1
  %postinc_load = load i32, i32* %struct_field_ptr13
  %postinc = add i32 %postinc_load, 1
  store i32 %postinc, i32* %struct_field_ptr13
  %gep_arr_elem_ptr14 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %struct_field_ptr12, i32 0, i32 1
  %arr_elem_ptr = load i8*, i8** %gep_arr_elem_ptr14
  %gep_arr_elem = getelementptr i8, i8* %arr_elem_ptr, i32 %postinc_load
  store i8 %char, i8* %gep_arr_elem
  ret void
}

; Function Attrs: nounwind
define internal void @u64_to_ascii(<{ <{ i32, i8* }>, i32 }>* %dest, i64 %value) #0 {
vars:
  %v = alloca i64
  %start = alloca i8*
  %index = alloca i64
  %arr_struct_alloca = alloca <{ i32, i8* }>
  %arr_elem_alloca = alloca i8, i32 0
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca, i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr6 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr6
  %gep_arr_elem_ptr7 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 1
  store i8* %arr_elem_alloca, i8** %gep_arr_elem_ptr7
  %arr_struct_load = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca
  %arr_struct_alloca8 = alloca <{ i32, i8* }>
  %arr_elem_alloca9 = alloca i8, i32 93
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca9, i8* getelementptr inbounds ([94 x i8], [94 x i8]* @str.15, i32 0, i32 0), i32 93, i32 0, i1 false)
  %gep_arr_elem_ptr10 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca8, i32 0, i32 0
  store i32 93, i32* %gep_arr_elem_ptr10
  %gep_arr_elem_ptr11 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca8, i32 0, i32 1
  store i8* %arr_elem_alloca9, i8** %gep_arr_elem_ptr11
  %arr_struct_load12 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca8
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
  store i64 %urem_tmp, i64* %index
  %index3 = load i64, i64* %index
  %icmp_tmp = icmp sge i64 %index3, 0
  br i1 %icmp_tmp, label %cand.rhs, label %cand.end

cand.rhs:                                         ; preds = %while
  %index4 = load i64, i64* %index
  %icmp_tmp5 = icmp slt i64 %index4, 10
  br label %cand.end

cand.end:                                         ; preds = %cand.rhs, %while
  %candphi = phi i1 [ false, %while ], [ %icmp_tmp5, %cand.rhs ]
  call void @assert(i1 %candphi, <{ i32, i8* }> %arr_struct_load, <{ i32, i8* }> %arr_struct_load12)
  %index13 = load i64, i64* %index
  %int_trunc = trunc i64 %index13 to i32
  %arr_elem_ptr14 = load i8*, i8** getelementptr inbounds (<{ i32, i8* }>, <{ i32, i8* }>* @decimal_digits, i32 0, i32 1)
  %gep_arr_elem15 = getelementptr i8, i8* %arr_elem_ptr14, i32 %int_trunc
  %arr_elem = load i8, i8* %gep_arr_elem15
  store i8 %arr_elem, i8* %digit
  %digit16 = load i8, i8* %digit
  call void @out_char(<{ <{ i32, i8* }>, i32 }>* %dest, i8 %digit16)
  %v17 = load i64, i64* %v
  %div_tmp = udiv i64 %v17, 10
  store i64 %div_tmp, i64* %v
  %v18 = load i64, i64* %v
  %icmp_tmp19 = icmp eq i64 %v18, 0
  br i1 %icmp_tmp19, label %then, label %endif

then:                                             ; preds = %cand.end
  br label %while_end

endif:                                            ; preds = %cand.end
  br label %while_cond

while_end:                                        ; preds = %then, %while_cond
  %struct_field_ptr20 = getelementptr inbounds <{ <{ i32, i8* }>, i32 }>, <{ <{ i32, i8* }>, i32 }>* %dest, i32 0, i32 0
  %struct_field_ptr21 = getelementptr inbounds <{ <{ i32, i8* }>, i32 }>, <{ <{ i32, i8* }>, i32 }>* %dest, i32 0, i32 1
  %struct_arrow22 = load i32, i32* %struct_field_ptr21
  %gep_arr_elem_ptr23 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %struct_field_ptr20, i32 0, i32 1
  %arr_elem_ptr24 = load i8*, i8** %gep_arr_elem_ptr23
  %gep_arr_elem25 = getelementptr i8, i8* %arr_elem_ptr24, i32 %struct_arrow22
  store i8* %gep_arr_elem25, i8** %end
  br label %while_cond26

while_cond26:                                     ; preds = %while27, %while_end
  %start29 = load i8*, i8** %start
  %end30 = load i8*, i8** %end
  %icmp_tmp31 = icmp ult i8* %start29, %end30
  br i1 %icmp_tmp31, label %while27, label %while_end28

while27:                                          ; preds = %while_cond26
  %predec_load = load i8*, i8** %end
  %ptr_pre_dec = getelementptr i8, i8* %predec_load, i32 -1
  store i8* %ptr_pre_dec, i8** %end
  %end32 = load i8*, i8** %end
  %deref = load i8, i8* %end32
  store i8 %deref, i8* %temp
  %end33 = load i8*, i8** %end
  %start34 = load i8*, i8** %start
  %deref35 = load i8, i8* %start34
  store i8 %deref35, i8* %end33
  %start36 = load i8*, i8** %start
  %temp37 = load i8, i8* %temp
  store i8 %temp37, i8* %start36
  %preinc_load = load i8*, i8** %start
  %ptr_pre_inc = getelementptr i8, i8* %preinc_load, i32 1
  store i8* %ptr_pre_inc, i8** %start
  br label %while_cond26

while_end28:                                      ; preds = %while_cond26
  ret void
}

; Function Attrs: nounwind
define internal void @print_f64(double %value, i32 %precision) #0 {
vars:
  %v = alloca double
  %pd = alloca <{ <{ i32, i8* }>, i32 }>
  %arr_struct_alloca = alloca <{ i32, i8* }>
  %arr_elem_alloca = alloca i8, i32 64
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca, i8* getelementptr inbounds ([65 x i8], [65 x i8]* @str.12, i32 0, i32 0), i32 64, i32 0, i1 false)
  %gep_arr_elem_ptr = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 0
  store i32 64, i32* %gep_arr_elem_ptr
  %gep_arr_elem_ptr2 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 1
  store i8* %arr_elem_alloca, i8** %gep_arr_elem_ptr2
  %arr_struct_load = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca
  %arr_struct_alloca5 = alloca <{ i32, i8* }>
  %arr_elem_alloca6 = alloca i8, i32 0
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca6, i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr7 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca5, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr7
  %gep_arr_elem_ptr8 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca5, i32 0, i32 1
  store i8* %arr_elem_alloca6, i8** %gep_arr_elem_ptr8
  %arr_struct_load9 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca5
  %arr_struct_alloca10 = alloca <{ i32, i8* }>
  %arr_elem_alloca11 = alloca i8, i32 93
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca11, i8* getelementptr inbounds ([94 x i8], [94 x i8]* @str.16, i32 0, i32 0), i32 93, i32 0, i1 false)
  %gep_arr_elem_ptr12 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca10, i32 0, i32 0
  store i32 93, i32* %gep_arr_elem_ptr12
  %gep_arr_elem_ptr13 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca10, i32 0, i32 1
  store i8* %arr_elem_alloca11, i8** %gep_arr_elem_ptr13
  %arr_struct_load14 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca10
  %int_part = alloca i64
  %first_fraction_char = alloca i32
  %last_non_zero = alloca i32
  %i = alloca i32
  %int_part30 = alloca i32
  %arr_struct_alloca49 = alloca <{ i32, i8* }>
  %arr_elem_alloca50 = alloca i8, i32 0
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca50, i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr51 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca49, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr51
  %gep_arr_elem_ptr52 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca49, i32 0, i32 1
  store i8* %arr_elem_alloca50, i8** %gep_arr_elem_ptr52
  %arr_struct_load53 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca49
  %arr_struct_alloca54 = alloca <{ i32, i8* }>
  %arr_elem_alloca55 = alloca i8, i32 93
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca55, i8* getelementptr inbounds ([94 x i8], [94 x i8]* @str.17, i32 0, i32 0), i32 93, i32 0, i1 false)
  %gep_arr_elem_ptr56 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca54, i32 0, i32 0
  store i32 93, i32* %gep_arr_elem_ptr56
  %gep_arr_elem_ptr57 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca54, i32 0, i32 1
  store i8* %arr_elem_alloca55, i8** %gep_arr_elem_ptr57
  %arr_struct_load58 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca54
  %arr_struct_alloca72 = alloca <{ i32, i8* }>
  %arr_elem_alloca73 = alloca i8, i32 0
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca73, i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr74 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca72, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr74
  %gep_arr_elem_ptr75 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca72, i32 0, i32 1
  store i8* %arr_elem_alloca73, i8** %gep_arr_elem_ptr75
  %arr_struct_load76 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca72
  %arr_struct_alloca77 = alloca <{ i32, i8* }>
  %arr_elem_alloca78 = alloca i8, i32 93
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca78, i8* getelementptr inbounds ([94 x i8], [94 x i8]* @str.18, i32 0, i32 0), i32 93, i32 0, i1 false)
  %gep_arr_elem_ptr79 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca77, i32 0, i32 0
  store i32 93, i32* %gep_arr_elem_ptr79
  %gep_arr_elem_ptr80 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca77, i32 0, i32 1
  store i8* %arr_elem_alloca78, i8** %gep_arr_elem_ptr80
  %arr_struct_load81 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca77
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
  call void @assert(i1 %icmp_tmp, <{ i32, i8* }> %arr_struct_load9, <{ i32, i8* }> %arr_struct_load14)
  %v15 = load double, double* %v
  %fcmp_tmp = fcmp olt double %v15, 0.000000e+00
  br i1 %fcmp_tmp, label %then, label %endif

then:                                             ; preds = %entry
  %v16 = load double, double* %v
  %fneg_tmp = fsub double -0.000000e+00, %v16
  store double %fneg_tmp, double* %v
  call void @out_char(<{ <{ i32, i8* }>, i32 }>* %pd, i8 45)
  br label %endif

endif:                                            ; preds = %then, %entry
  %v17 = load double, double* %v
  %int_cast = fptoui double %v17 to i64
  store i64 %int_cast, i64* %int_part
  %int_part18 = load i64, i64* %int_part
  call void @u64_to_ascii(<{ <{ i32, i8* }>, i32 }>* %pd, i64 %int_part18)
  %v19 = load double, double* %v
  %int_part20 = load i64, i64* %int_part
  %int_to_float_cast = sitofp i64 %int_part20 to double
  %fsub_tmp = fsub double %v19, %int_to_float_cast
  store double %fsub_tmp, double* %v
  call void @out_char(<{ <{ i32, i8* }>, i32 }>* %pd, i8 46)
  %struct_field_ptr21 = getelementptr inbounds <{ <{ i32, i8* }>, i32 }>, <{ <{ i32, i8* }>, i32 }>* %pd, i32 0, i32 1
  %struct_field22 = load i32, i32* %struct_field_ptr21
  store i32 %struct_field22, i32* %first_fraction_char
  %struct_field_ptr23 = getelementptr inbounds <{ <{ i32, i8* }>, i32 }>, <{ <{ i32, i8* }>, i32 }>* %pd, i32 0, i32 1
  %struct_field24 = load i32, i32* %struct_field_ptr23
  store i32 %struct_field24, i32* %last_non_zero
  store i32 0, i32* %i
  br label %for_cond

for_cond:                                         ; preds = %for_iter, %endif
  %i25 = load i32, i32* %i
  %icmp_tmp26 = icmp slt i32 %i25, %precision
  br i1 %icmp_tmp26, label %for, label %end_for

for:                                              ; preds = %for_cond
  %v27 = load double, double* %v
  %fmul_tmp = fmul double %v27, 1.000000e+01
  store double %fmul_tmp, double* %v
  %v28 = load double, double* %v
  %int_cast29 = fptosi double %v28 to i32
  store i32 %int_cast29, i32* %int_part30
  %v31 = load double, double* %v
  %int_part32 = load i32, i32* %int_part30
  %int_to_float_cast33 = sitofp i32 %int_part32 to double
  %fsub_tmp34 = fsub double %v31, %int_to_float_cast33
  store double %fsub_tmp34, double* %v
  %int_part35 = load i32, i32* %int_part30
  %icmp_tmp36 = icmp slt i32 %int_part35, 10
  br i1 %icmp_tmp36, label %cand.rhs, label %cand.end

cand.rhs:                                         ; preds = %for
  %int_part37 = load i32, i32* %int_part30
  %icmp_tmp38 = icmp sge i32 %int_part37, 0
  br label %cand.end

cand.end:                                         ; preds = %cand.rhs, %for
  %candphi = phi i1 [ false, %for ], [ %icmp_tmp38, %cand.rhs ]
  %not_tmp = xor i1 %candphi, true
  br i1 %not_tmp, label %then39, label %endif40

then39:                                           ; preds = %cand.end
  %int_part41 = load i32, i32* %int_part30
  call void @print_i32(i32 %int_part41, i1 true)
  br label %endif40

endif40:                                          ; preds = %then39, %cand.end
  %int_part42 = load i32, i32* %int_part30
  %icmp_tmp43 = icmp slt i32 %int_part42, 10
  br i1 %icmp_tmp43, label %cand.rhs44, label %cand.end45

cand.rhs44:                                       ; preds = %endif40
  %int_part46 = load i32, i32* %int_part30
  %icmp_tmp47 = icmp sge i32 %int_part46, 0
  br label %cand.end45

cand.end45:                                       ; preds = %cand.rhs44, %endif40
  %candphi48 = phi i1 [ false, %endif40 ], [ %icmp_tmp47, %cand.rhs44 ]
  call void @assert(i1 %candphi48, <{ i32, i8* }> %arr_struct_load53, <{ i32, i8* }> %arr_struct_load58)
  %int_part59 = load i32, i32* %int_part30
  %arr_elem_ptr = load i8*, i8** getelementptr inbounds (<{ i32, i8* }>, <{ i32, i8* }>* @decimal_digits, i32 0, i32 1)
  %gep_arr_elem = getelementptr i8, i8* %arr_elem_ptr, i32 %int_part59
  %arr_elem = load i8, i8* %gep_arr_elem
  call void @out_char(<{ <{ i32, i8* }>, i32 }>* %pd, i8 %arr_elem)
  %int_part60 = load i32, i32* %int_part30
  %icmp_tmp61 = icmp ne i32 %int_part60, 0
  br i1 %icmp_tmp61, label %then62, label %endif63

then62:                                           ; preds = %cand.end45
  %struct_field_ptr64 = getelementptr inbounds <{ <{ i32, i8* }>, i32 }>, <{ <{ i32, i8* }>, i32 }>* %pd, i32 0, i32 1
  %struct_field65 = load i32, i32* %struct_field_ptr64
  store i32 %struct_field65, i32* %last_non_zero
  br label %endif63

endif63:                                          ; preds = %then62, %cand.end45
  br label %for_iter

for_iter:                                         ; preds = %endif63
  %preinc_load = load i32, i32* %i
  %preinc = add i32 %preinc_load, 1
  store i32 %preinc, i32* %i
  br label %for_cond

end_for:                                          ; preds = %for_cond
  %struct_field_ptr66 = getelementptr inbounds <{ <{ i32, i8* }>, i32 }>, <{ <{ i32, i8* }>, i32 }>* %pd, i32 0, i32 1
  %struct_field67 = load i32, i32* %struct_field_ptr66
  %struct_field_ptr68 = getelementptr inbounds <{ <{ i32, i8* }>, i32 }>, <{ <{ i32, i8* }>, i32 }>* %pd, i32 0, i32 0
  %struct_field_ptr69 = getelementptr inbounds <{ i32, i8* }>, <{ i32, i8* }>* %struct_field_ptr68, i32 0, i32 0
  %struct_field70 = load i32, i32* %struct_field_ptr69
  %icmp_tmp71 = icmp sle i32 %struct_field67, %struct_field70
  call void @assert(i1 %icmp_tmp71, <{ i32, i8* }> %arr_struct_load76, <{ i32, i8* }> %arr_struct_load81)
  %struct_field_ptr82 = getelementptr inbounds <{ <{ i32, i8* }>, i32 }>, <{ <{ i32, i8* }>, i32 }>* %pd, i32 0, i32 0
  %struct_field_ptr83 = getelementptr inbounds <{ i32, i8* }>, <{ i32, i8* }>* %struct_field_ptr82, i32 0, i32 0
  %struct_field_ptr84 = getelementptr inbounds <{ <{ i32, i8* }>, i32 }>, <{ <{ i32, i8* }>, i32 }>* %pd, i32 0, i32 1
  %struct_field85 = load i32, i32* %struct_field_ptr84
  store i32 %struct_field85, i32* %struct_field_ptr83
  %struct_field_ptr86 = getelementptr inbounds <{ <{ i32, i8* }>, i32 }>, <{ <{ i32, i8* }>, i32 }>* %pd, i32 0, i32 0
  %struct_field87 = load <{ i32, i8* }>, <{ i32, i8* }>* %struct_field_ptr86
  call void @print_string(<{ i32, i8* }> %struct_field87)
  ret void
}

; Function Attrs: nounwind
define internal void @debug_print_i64(<{ i32, i8* }> %name, i64 %value, i1 %signed) #0 {
vars:
  %arr_struct_alloca = alloca <{ i32, i8* }>
  %arr_elem_alloca = alloca i8, i32 2
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca, i8* getelementptr inbounds ([3 x i8], [3 x i8]* @str.19, i32 0, i32 0), i32 2, i32 0, i1 false)
  %gep_arr_elem_ptr = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 0
  store i32 2, i32* %gep_arr_elem_ptr
  %gep_arr_elem_ptr1 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 1
  store i8* %arr_elem_alloca, i8** %gep_arr_elem_ptr1
  %arr_struct_load = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca
  %arr_struct_alloca2 = alloca <{ i32, i8* }>
  %arr_elem_alloca3 = alloca i8
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca3, i8* getelementptr inbounds ([2 x i8], [2 x i8]* @str.8, i32 0, i32 0), i32 1, i32 0, i1 false)
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
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca, i8* getelementptr inbounds ([3 x i8], [3 x i8]* @str.19, i32 0, i32 0), i32 2, i32 0, i1 false)
  %gep_arr_elem_ptr = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 0
  store i32 2, i32* %gep_arr_elem_ptr
  %gep_arr_elem_ptr1 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 1
  store i8* %arr_elem_alloca, i8** %gep_arr_elem_ptr1
  %arr_struct_load = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca
  %arr_struct_alloca2 = alloca <{ i32, i8* }>
  %arr_elem_alloca3 = alloca i8
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca3, i8* getelementptr inbounds ([2 x i8], [2 x i8]* @str.8, i32 0, i32 0), i32 1, i32 0, i1 false)
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
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca, i8* getelementptr inbounds ([3 x i8], [3 x i8]* @str.19, i32 0, i32 0), i32 2, i32 0, i1 false)
  %gep_arr_elem_ptr = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 0
  store i32 2, i32* %gep_arr_elem_ptr
  %gep_arr_elem_ptr1 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 1
  store i8* %arr_elem_alloca, i8** %gep_arr_elem_ptr1
  %arr_struct_load = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca
  %arr_struct_alloca2 = alloca <{ i32, i8* }>
  %arr_elem_alloca3 = alloca i8
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca3, i8* getelementptr inbounds ([2 x i8], [2 x i8]* @str.8, i32 0, i32 0), i32 1, i32 0, i1 false)
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
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca, i8* getelementptr inbounds ([3 x i8], [3 x i8]* @str.19, i32 0, i32 0), i32 2, i32 0, i1 false)
  %gep_arr_elem_ptr = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 0
  store i32 2, i32* %gep_arr_elem_ptr
  %gep_arr_elem_ptr1 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 1
  store i8* %arr_elem_alloca, i8** %gep_arr_elem_ptr1
  %arr_struct_load = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca
  %arr_struct_alloca2 = alloca <{ i32, i8* }>
  %arr_elem_alloca3 = alloca i8
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca3, i8* getelementptr inbounds ([2 x i8], [2 x i8]* @str.8, i32 0, i32 0), i32 1, i32 0, i1 false)
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
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca, i8* getelementptr inbounds ([3 x i8], [3 x i8]* @str.19, i32 0, i32 0), i32 2, i32 0, i1 false)
  %gep_arr_elem_ptr = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 0
  store i32 2, i32* %gep_arr_elem_ptr
  %gep_arr_elem_ptr1 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 1
  store i8* %arr_elem_alloca, i8** %gep_arr_elem_ptr1
  %arr_struct_load = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca
  %arr_struct_alloca2 = alloca <{ i32, i8* }>
  %arr_elem_alloca3 = alloca i8, i32 4
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca3, i8* getelementptr inbounds ([5 x i8], [5 x i8]* @str.20, i32 0, i32 0), i32 4, i32 0, i1 false)
  %gep_arr_elem_ptr4 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2, i32 0, i32 0
  store i32 4, i32* %gep_arr_elem_ptr4
  %gep_arr_elem_ptr5 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2, i32 0, i32 1
  store i8* %arr_elem_alloca3, i8** %gep_arr_elem_ptr5
  %arr_struct_load6 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2
  %arr_struct_alloca7 = alloca <{ i32, i8* }>
  %arr_elem_alloca8 = alloca i8, i32 5
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca8, i8* getelementptr inbounds ([6 x i8], [6 x i8]* @str.21, i32 0, i32 0), i32 5, i32 0, i1 false)
  %gep_arr_elem_ptr9 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca7, i32 0, i32 0
  store i32 5, i32* %gep_arr_elem_ptr9
  %gep_arr_elem_ptr10 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca7, i32 0, i32 1
  store i8* %arr_elem_alloca8, i8** %gep_arr_elem_ptr10
  %arr_struct_load11 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca7
  %arr_struct_alloca12 = alloca <{ i32, i8* }>
  %arr_elem_alloca13 = alloca i8
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca13, i8* getelementptr inbounds ([2 x i8], [2 x i8]* @str.8, i32 0, i32 0), i32 1, i32 0, i1 false)
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
  %gep_arr_elem_ptr7 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 1
  store i8* %arr_elem_alloca, i8** %gep_arr_elem_ptr7
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
  br i1 %corphi6, label %then, label %endif

then:                                             ; preds = %cor.end5
  call void @print_string(<{ i32, i8* }> %arr_struct_load)
  ret i1 true

endif:                                            ; preds = %cor.end5
  ret i1 false
}

; Function Attrs: nounwind
define internal void @main() #0 {
vars:
  %arr_struct_alloca = alloca <{ i32, i8* }>
  %arr_elem_alloca = alloca i8
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca, i8* getelementptr inbounds ([2 x i8], [2 x i8]* @str.8, i32 0, i32 0), i32 1, i32 0, i1 false)
  %gep_arr_elem_ptr = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 0
  store i32 1, i32* %gep_arr_elem_ptr
  %gep_arr_elem_ptr1 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 1
  store i8* %arr_elem_alloca, i8** %gep_arr_elem_ptr1
  %arr_struct_load = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca
  br label %entry

entry:                                            ; preds = %vars
  call void @print_i64(i64 -1, i1 false)
  call void @print_string(<{ i32, i8* }> %arr_struct_load)
  call void @print_i64(i64 -1, i1 false)
  ret void
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
  %arr_elem_alloca = alloca i8, i32 0
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca, i8* getelementptr inbounds ([1 x i8], [1 x i8]* @str.1, i32 0, i32 0), i32 0, i32 0, i1 false)
  %gep_arr_elem_ptr = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 0
  store i32 0, i32* %gep_arr_elem_ptr
  %gep_arr_elem_ptr1 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca, i32 0, i32 1
  store i8* %arr_elem_alloca, i8** %gep_arr_elem_ptr1
  %arr_struct_load = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca
  %arr_struct_alloca2 = alloca <{ i32, i8* }>
  %arr_elem_alloca3 = alloca i8, i32 93
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca3, i8* getelementptr inbounds ([94 x i8], [94 x i8]* @str.11, i32 0, i32 0), i32 93, i32 0, i1 false)
  %gep_arr_elem_ptr4 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2, i32 0, i32 0
  store i32 93, i32* %gep_arr_elem_ptr4
  %gep_arr_elem_ptr5 = getelementptr <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2, i32 0, i32 1
  store i8* %arr_elem_alloca3, i8** %gep_arr_elem_ptr5
  %arr_struct_load6 = load <{ i32, i8* }>, <{ i32, i8* }>* %arr_struct_alloca2
  %result = alloca i32
  br label %entry

entry:                                            ; preds = %vars
  %icmp_tmp = icmp sle i32 %min, %max
  call void @assert(i1 %icmp_tmp, <{ i32, i8* }> %arr_struct_load, <{ i32, i8* }> %arr_struct_load6)
  store i32 %value, i32* %result
  %result7 = load i32, i32* %result
  %icmp_tmp8 = icmp slt i32 %result7, %min
  br i1 %icmp_tmp8, label %then, label %elif_0

then:                                             ; preds = %entry
  store i32 %min, i32* %result
  br label %endif

elif_0:                                           ; preds = %entry
  %result9 = load i32, i32* %result
  %icmp_tmp10 = icmp sgt i32 %result9, %max
  br i1 %icmp_tmp10, label %elif_0_then, label %endif

elif_0_then:                                      ; preds = %elif_0
  store i32 %max, i32* %result
  br label %endif

endif:                                            ; preds = %elif_0_then, %elif_0, %then
  %result11 = load i32, i32* %result
  ret i32 %result11
}

attributes #0 = { nounwind }
attributes #1 = { nounwind readnone }
attributes #2 = { argmemonly nounwind }
