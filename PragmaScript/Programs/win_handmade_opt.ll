; ModuleID = 'win_handmade.ll'
source_filename = "win_handmade.ll"
target datalayout = "e-m:w-i64:64-f80:128-n8:16:32:64-S128"
target triple = "x86_64-pc-windows-msvc"

@str = private unnamed_addr constant [11 x i8] c"0123456789\00"
@str_arr = private global [10 x i8] zeroinitializer
@decimal_digits = internal unnamed_addr global <{ i32, i8* }> zeroinitializer
@console_output_handle = internal unnamed_addr global i64 0
@str.1 = private unnamed_addr constant [65 x i8] c"                                                                \00"
@backbuffer = internal global <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }> zeroinitializer
@window_requests_quit = internal unnamed_addr global i1 false
@perf_count_freq = internal global i64 0
@str.9 = private unnamed_addr constant [13 x i8] c"handmade.dll\00"
@str.10 = private unnamed_addr constant [18 x i8] c"handmade_temp.dll\00"
@str.14 = private unnamed_addr constant [23 x i8] c"game_update_and_render\00"
@str.16 = private unnamed_addr constant [18 x i8] c"game_output_sound\00"
@str.18 = private unnamed_addr constant [14 x i8] c"load library\0A\00"
@str.19 = private unnamed_addr constant [16 x i8] c"unload library\0A\00"
@str.22 = private unnamed_addr constant [11 x i8] c"dsound.dll\00"
@str.24 = private unnamed_addr constant [18 x i8] c"DirectSoundCreate\00"
@str.29 = private unnamed_addr constant [24 x i8] c"PragmaScriptWindowClass\00"
@str.31 = private unnamed_addr constant [22 x i8] c"handmade-pragmascript\00"
@str.33 = private unnamed_addr constant [8 x i8] c"ms/f   \00"
@str.34 = private unnamed_addr constant [7 x i8] c"fps   \00"
@str.35 = private unnamed_addr constant [11 x i8] c"mcycles   \00"
@str.37 = private unnamed_addr constant [8 x i8] c"exit...\00"
@window.0 = internal unnamed_addr global i64 0
@window.1 = internal unnamed_addr global i64 0

; Function Attrs: nounwind
declare align 64 i8* @VirtualAlloc(i8* nocapture, i64, i32, i32) #0

; Function Attrs: nounwind
define i64 @_rdtsc() #0 {
  %1 = tail call { i32, i32 } asm sideeffect "rdtsc", "={ax},={dx},~{dirflag},~{fpsr},~{flags}"() #5
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
  tail call void asm sideeffect "push   %rcx \09\0Apush   %rax \09\0Acmp    $$0x1000,%rax \09\0Alea    24(%rsp),%rcx \09\0Ajb     1f \09\0A2: \09\0Asub    $$0x1000,%rcx \09\0Aorl    $$0,(%rcx) \09\0Asub    $$0x1000,%rax \09\0Acmp    $$0x1000,%rax \09\0Aja     2b \09\0A1: \09\0Asub    %rax,%rcx \09\0Aorl    $$0,(%rcx) \09\0Apop    %rax \09\0Apop    %rcx \09\0Aret \09\0A", "~{dirflag},~{fpsr},~{flags}"() #5
  ret void
}

; Function Attrs: nounwind readnone
declare float @llvm.fabs.f32(float) #1

; Function Attrs: nounwind readnone
declare float @llvm.floor.f32(float) #1

; Function Attrs: argmemonly nounwind
declare void @llvm.memcpy.p0i8.p0i8.i32(i8* nocapture, i8* nocapture readonly, i32, i32, i1) #2

; Function Attrs: nounwind
declare i32 @WriteFile(i64, i8* nocapture, i32, i32* nocapture, i8*) #0

; Function Attrs: nounwind
declare i32 @ReadFile(i64, i8* nocapture, i32, i32* nocapture, i8*) #0

; Function Attrs: nounwind
define void @__init() #0 {
vars:
  %arr_elem_alloca1.i = alloca [0 x i8], align 1
  %data.i.i.i.i = alloca <{ i32, i64, i64, i64, i64 }>, align 8
  %arr_elem_alloca1.i.i.i = alloca [15 x i8], align 1
  %data.i.i244.i = alloca <{ i32, i64, i64, i64, i64 }>, align 8
  %arr_elem_alloca1.i240.i = alloca i16, align 2
  %arr_elem_alloca3.i.i = alloca i8, align 1
  %result.i219.i = alloca i64, align 8
  %result.i211.i = alloca i64, align 8
  %region1.i170.i = alloca i8*, align 8
  %region1_size.i171.i = alloca i32, align 4
  %region2.i172.i = alloca i8*, align 8
  %region2_size.i173.i = alloca i32, align 4
  %region1.i149.i = alloca i8*, align 8
  %region1_size.i150.i = alloca i32, align 4
  %region2.i151.i = alloca i8*, align 8
  %region2_size.i152.i = alloca i32, align 4
  %result.i33.i.i = alloca i64, align 8
  %play_cursor.i.i = alloca i32, align 4
  %write_cursor.i.i = alloca i32, align 4
  %time.i.i = alloca i64, align 8
  %cursor_pos.i.i = alloca i64, align 8
  %msg.i.i = alloca <{ i64, i32, i64, i64, i32, <{ i32, i32 }>, i64 }>, align 8
  %result.i54.i = alloca i64, align 8
  %result.i51.i = alloca i64, align 8
  %result.i.i = alloca i64, align 8
  %region1.i.i = alloca i8*, align 8
  %region1_size.i.i = alloca i32, align 4
  %region2.i.i = alloca i8*, align 8
  %region2_size.i.i = alloca i32, align 4
  %data.i.i.i = alloca <{ i32, i64, i64, i64, i64 }>, align 8
  %arr_elem_alloca32.i.i.i = alloca [13 x i8], align 1
  %arr_elem_alloca5.i.i = alloca [13 x i8], align 1
  %arr_elem_alloca3510.i.i = alloca [23 x i8], align 1
  %arr_elem_alloca5012.i.i = alloca [18 x i8], align 1
  %arr_elem_alloca6514.i.i = alloca [13 x i8], align 1
  %arr_elem_alloca1.i.i = alloca [11 x i8], align 1
  %arr_elem_alloca103.i.i = alloca [18 x i8], align 1
  %dsound_obj.i.i = alloca <{ i8* }>*, align 8
  %struct_alloca.i.i = alloca <{ i32, i32, i32, i32, i8* }>, align 8
  %primary_buffer.i.i = alloca <{ i8* }>*, align 8
  %struct_alloca43.i.i = alloca <{ i16, i16, i32, i32, i16, i16, i16 }>, align 8
  %struct_alloca74.i.i = alloca <{ i32, i32, i32, i32, i8* }>, align 16
  %secondary_buffer.i.i = alloca <{ i8* }>*, align 8
  %rect.i.i = alloca <{ i32, i32, i32, i32 }>, align 8
  %arr_elem_alloca32.i = alloca [24 x i8], align 1
  %arr_elem_alloca1034.i = alloca [22 x i8], align 1
  %struct_alloca.i = alloca <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>, align 8
  %struct_alloca62.i = alloca <{ i1 (<{ i32, i8* }>, <{ i32, i8* }>)*, <{ i32, i8* }> (<{ i32, i8* }>)*, void (i8*)* }>, align 8
  %struct_alloca69.i = alloca <{ i8*, i64, i8*, i64, i8* }>, align 8
  %struct_alloca111.i = alloca <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, align 8
  %struct_alloca124.i = alloca <{ i16*, i32, i32, float }>, align 8
  %struct_alloca144.i = alloca <{ i8*, i32, i32, i32 }>, align 8
  %arr_elem_alloca238.i = alloca i8, align 1
  %arr_elem_alloca24436.i = alloca [7 x i8], align 1
  %arr_elem_alloca25537.i = alloca [6 x i8], align 1
  %arr_elem_alloca26338.i = alloca [10 x i8], align 1
  %arr_elem_alloca26839.i = alloca i16, align 2
  %arr_elem_alloca27740.i = alloca [7 x i8], align 1
  tail call void @llvm.memcpy.p0i8.p0i8.i32(i8* getelementptr inbounds ([10 x i8], [10 x i8]* @str_arr, i64 0, i64 0), i8* getelementptr inbounds ([11 x i8], [11 x i8]* @str, i64 0, i64 0), i32 10, i32 1, i1 false)
  store i32 10, i32* getelementptr inbounds (<{ i32, i8* }>, <{ i32, i8* }>* @decimal_digits, i64 0, i32 0), align 8
  store i8* getelementptr inbounds ([10 x i8], [10 x i8]* @str_arr, i64 0, i64 0), i8** getelementptr inbounds (<{ i32, i8* }>, <{ i32, i8* }>* @decimal_digits, i64 0, i32 1), align 4
  %0 = getelementptr inbounds [0 x i8], [0 x i8]* %arr_elem_alloca1.i, i64 0, i64 0
  call void @llvm.lifetime.start(i64 0, i8* %0)
  br i1 select (i1 select (i1 select (i1 icmp eq (i64 ptrtoint (i8* (i8*, i32, i64)* @memset to i64), i64 1234), i1 true, i1 icmp eq (i64 ptrtoint (void ()* @__chkstk to i64), i64 1234)), i1 true, i1 icmp eq (i64 ptrtoint (float (float)* @cosf to i64), i64 1234)), i1 true, i1 icmp eq (i64 ptrtoint (float (float)* @sinf to i64), i64 1234)), label %then.i, label %endif.i

then.i:                                           ; preds = %vars
  %console_output_handle.i.i = load i64, i64* @console_output_handle, align 8
  %fun_call.i.i = call i32 @WriteFile(i64 %console_output_handle.i.i, i8* %0, i32 0, i32* null, i8* null) #5
  call void @llvm.lifetime.end(i64 0, i8* %0)
  br label %__hack_reserve_intrinsics.exit

endif.i:                                          ; preds = %vars
  call void @llvm.lifetime.end(i64 0, i8* %0)
  br label %__hack_reserve_intrinsics.exit

__hack_reserve_intrinsics.exit:                   ; preds = %endif.i, %then.i
  %fun_call2 = tail call i64 @GetStdHandle(i32 -11)
  store i64 %fun_call2, i64* @console_output_handle, align 8
  %fun_call3 = tail call i64 @GetStdHandle(i32 -10)
  %fun_call4 = tail call i64 @GetStdHandle(i32 -12)
  %1 = getelementptr inbounds [24 x i8], [24 x i8]* %arr_elem_alloca32.i, i64 0, i64 0
  call void @llvm.lifetime.start(i64 24, i8* %1)
  %2 = getelementptr inbounds [22 x i8], [22 x i8]* %arr_elem_alloca1034.i, i64 0, i64 0
  call void @llvm.lifetime.start(i64 22, i8* %2)
  %3 = bitcast <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>* %struct_alloca.i to i8*
  call void @llvm.lifetime.start(i64 80, i8* %3)
  %4 = bitcast <{ i1 (<{ i32, i8* }>, <{ i32, i8* }>)*, <{ i32, i8* }> (<{ i32, i8* }>)*, void (i8*)* }>* %struct_alloca62.i to i8*
  call void @llvm.lifetime.start(i64 24, i8* %4)
  %5 = bitcast <{ i8*, i64, i8*, i64, i8* }>* %struct_alloca69.i to i8*
  call void @llvm.lifetime.start(i64 40, i8* %5)
  %6 = bitcast <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca111.i to i8*
  call void @llvm.lifetime.start(i64 42, i8* %6)
  %7 = bitcast <{ i16*, i32, i32, float }>* %struct_alloca124.i to i8*
  call void @llvm.lifetime.start(i64 20, i8* %7)
  %8 = bitcast <{ i8*, i32, i32, i32 }>* %struct_alloca144.i to i8*
  call void @llvm.lifetime.start(i64 20, i8* %8)
  call void @llvm.lifetime.start(i64 1, i8* nonnull %arr_elem_alloca238.i)
  %9 = getelementptr inbounds [7 x i8], [7 x i8]* %arr_elem_alloca24436.i, i64 0, i64 0
  call void @llvm.lifetime.start(i64 7, i8* %9)
  %10 = getelementptr inbounds [6 x i8], [6 x i8]* %arr_elem_alloca25537.i, i64 0, i64 0
  call void @llvm.lifetime.start(i64 6, i8* %10)
  %11 = getelementptr inbounds [10 x i8], [10 x i8]* %arr_elem_alloca26338.i, i64 0, i64 0
  call void @llvm.lifetime.start(i64 10, i8* %11)
  %12 = bitcast i16* %arr_elem_alloca26839.i to i8*
  call void @llvm.lifetime.start(i64 2, i8* %12)
  %13 = getelementptr inbounds [7 x i8], [7 x i8]* %arr_elem_alloca27740.i, i64 0, i64 0
  call void @llvm.lifetime.start(i64 7, i8* %13)
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %1, i8* getelementptr inbounds ([24 x i8], [24 x i8]* @str.29, i64 0, i64 0), i32 24, i32 1, i1 false) #5
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %2, i8* getelementptr inbounds ([22 x i8], [22 x i8]* @str.31, i64 0, i64 0), i32 22, i32 1, i1 false) #5
  store i8 10, i8* %arr_elem_alloca238.i, align 1
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %9, i8* getelementptr inbounds ([8 x i8], [8 x i8]* @str.33, i64 0, i64 0), i32 7, i32 1, i1 false) #5
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %10, i8* getelementptr inbounds ([7 x i8], [7 x i8]* @str.34, i64 0, i64 0), i32 6, i32 1, i1 false) #5
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %11, i8* getelementptr inbounds ([11 x i8], [11 x i8]* @str.35, i64 0, i64 0), i32 10, i32 1, i1 false) #5
  store i16 29796, i16* %arr_elem_alloca26839.i, align 2
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %13, i8* getelementptr inbounds ([8 x i8], [8 x i8]* @str.37, i64 0, i64 0), i32 7, i32 1, i1 false) #5
  %fun_call.i = tail call i32 @QueryPerformanceFrequency(i64* nonnull @perf_count_freq) #5
  %fun_call1.i = tail call i32 @timeBeginPeriod(i32 1) #5
  store i32 1244, i32* getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i64 0, i32 2), align 4
  store i32 705, i32* getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i64 0, i32 3), align 8
  store i32 40, i32* getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i64 0, i32 0, i32 0, i32 0), align 16
  store i32 1244, i32* getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i64 0, i32 0, i32 0, i32 1), align 4
  store i32 -705, i32* getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i64 0, i32 0, i32 0, i32 2), align 8
  store i16 1, i16* getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i64 0, i32 0, i32 0, i32 3), align 4
  store i16 32, i16* getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i64 0, i32 0, i32 0, i32 4), align 2
  store i32 0, i32* getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i64 0, i32 0, i32 0, i32 5), align 16
  %struct_arrow29.i.i = load i8*, i8** getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i64 0, i32 1), align 8
  %icmp_tmp.i.i = icmp eq i8* %struct_arrow29.i.i, null
  br i1 %icmp_tmp.i.i, label %create_backbuffer.exit.i, label %then.i.i

then.i.i:                                         ; preds = %__hack_reserve_intrinsics.exit
  %fun_call.i.i1 = tail call i32 @VirtualFree(i8* nonnull %struct_arrow29.i.i, i64 0, i32 32768) #5
  br label %create_backbuffer.exit.i

create_backbuffer.exit.i:                         ; preds = %then.i.i, %__hack_reserve_intrinsics.exit
  %fun_call34.i.i = tail call i8* @VirtualAlloc(i8* null, i64 3508080, i32 4096, i32 4) #5
  store i8* %fun_call34.i.i, i8** getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i64 0, i32 1), align 8
  %struct_arrow37.i.i = load i32, i32* getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i64 0, i32 2), align 4
  %mul_tmp38.i.i = shl i32 %struct_arrow37.i.i, 2
  store i32 %mul_tmp38.i.i, i32* getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i64 0, i32 4), align 4
  %fun_call20.i = tail call i64 @GetModuleHandleA(i64 0) #5
  %struct_arg_0.i = getelementptr inbounds <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>, <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>* %struct_alloca.i, i64 0, i32 0
  %struct_arg_1.i = getelementptr inbounds <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>, <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>* %struct_alloca.i, i64 0, i32 1
  %struct_arg_2.i = getelementptr inbounds <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>, <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>* %struct_alloca.i, i64 0, i32 2
  %struct_arg_5.i = getelementptr inbounds <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>, <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>* %struct_alloca.i, i64 0, i32 5
  %struct_arg_10.i = getelementptr inbounds <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>, <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>* %struct_alloca.i, i64 0, i32 10
  call void @llvm.memset.p0i8.i64(i8* %3, i8 0, i64 80, i32 8, i1 false) #5
  store i32 80, i32* %struct_arg_0.i, align 8
  store i32 3, i32* %struct_arg_1.i, align 4
  store i8* bitcast (i64 (i64, i32, i64, i64)* @main_window_callback to i8*), i8** %struct_arg_2.i, align 8
  store i64 %fun_call20.i, i64* %struct_arg_5.i, align 8
  store i8* %1, i8** %struct_arg_10.i, align 8
  %fun_call27.i = call i16 @RegisterClassExA(<{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>* nonnull %struct_alloca.i) #5
  %fun_call31.i = call i64 @CreateWindowExA(i32 0, i8* %1, i8* %2, i32 282001408, i32 -2147483648, i32 -2147483648, i32 -2147483648, i32 -2147483648, i64 0, i64 0, i64 %fun_call20.i, i64 0) #5
  store i64 %fun_call31.i, i64* @window.0, align 8
  %fun_call32.i = call i64 @GetDC(i64 %fun_call31.i) #5
  store i64 %fun_call32.i, i64* @window.1, align 8
  %14 = bitcast <{ i32, i32, i32, i32 }>* %rect.i.i to i8*
  call void @llvm.lifetime.start(i64 16, i8* %14) #5
  %struct_field.i.i = load i64, i64* @window.0, align 8
  %fun_call.i47.i = call i32 @GetClientRect(i64 %struct_field.i.i, <{ i32, i32, i32, i32 }>* nonnull %rect.i.i) #5
  call void @llvm.lifetime.end(i64 16, i8* %14) #5
  %struct_field50.i = load i64, i64* @window.0, align 8
  %15 = getelementptr inbounds [11 x i8], [11 x i8]* %arr_elem_alloca1.i.i, i64 0, i64 0
  call void @llvm.lifetime.start(i64 11, i8* %15) #5
  %16 = getelementptr inbounds [18 x i8], [18 x i8]* %arr_elem_alloca103.i.i, i64 0, i64 0
  call void @llvm.lifetime.start(i64 18, i8* %16) #5
  %17 = bitcast <{ i8* }>** %dsound_obj.i.i to i8*
  call void @llvm.lifetime.start(i64 8, i8* %17) #5
  %18 = bitcast <{ i32, i32, i32, i32, i8* }>* %struct_alloca.i.i to i8*
  call void @llvm.lifetime.start(i64 24, i8* %18) #5
  %19 = bitcast <{ i8* }>** %primary_buffer.i.i to i8*
  call void @llvm.lifetime.start(i64 8, i8* %19) #5
  %20 = bitcast <{ i16, i16, i32, i32, i16, i16, i16 }>* %struct_alloca43.i.i to i8*
  call void @llvm.lifetime.start(i64 18, i8* %20) #5
  %21 = bitcast <{ i32, i32, i32, i32, i8* }>* %struct_alloca74.i.i to i8*
  call void @llvm.lifetime.start(i64 24, i8* %21) #5
  %22 = bitcast <{ i8* }>** %secondary_buffer.i.i to i8*
  call void @llvm.lifetime.start(i64 8, i8* %22) #5
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %15, i8* getelementptr inbounds ([11 x i8], [11 x i8]* @str.22, i64 0, i64 0), i32 11, i32 1, i1 false) #5
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %16, i8* getelementptr inbounds ([18 x i8], [18 x i8]* @str.24, i64 0, i64 0), i32 18, i32 1, i1 false) #5
  %fun_call7.i.i = call i64 @LoadLibraryA(i8* %15) #5
  %fun_call20.i.i = call i8* @GetProcAddress(i64 %fun_call7.i.i, i8* %16) #5
  %23 = bitcast i8* %fun_call20.i.i to i32 (i8*, <{ i8* }>**, i8*)*
  %fun_call21.i.i = call i32 %23(i8* null, <{ i8* }>** nonnull %dsound_obj.i.i, i8* null) #5
  %struct_arrow_load.i.i = load <{ i8* }>*, <{ i8* }>** %dsound_obj.i.i, align 8
  %24 = bitcast <{ i8* }>* %struct_arrow_load.i.i to <{ i8*, i8*, i8*, i32 (i8*, i8*, i8**, i8*)*, i8*, i8*, i32 (i8*, i64, i32)*, i8*, i8*, i8*, i8* }>**
  %struct_arrow_load225.i.i = load <{ i8*, i8*, i8*, i32 (i8*, i8*, i8**, i8*)*, i8*, i8*, i32 (i8*, i64, i32)*, i8*, i8*, i8*, i8* }>*, <{ i8*, i8*, i8*, i32 (i8*, i8*, i8**, i8*)*, i8*, i8*, i32 (i8*, i64, i32)*, i8*, i8*, i8*, i8* }>** %24, align 8
  %struct_field_ptr23.i.i = getelementptr inbounds <{ i8*, i8*, i8*, i32 (i8*, i8*, i8**, i8*)*, i8*, i8*, i32 (i8*, i64, i32)*, i8*, i8*, i8*, i8* }>, <{ i8*, i8*, i8*, i32 (i8*, i8*, i8**, i8*)*, i8*, i8*, i32 (i8*, i64, i32)*, i8*, i8*, i8*, i8* }>* %struct_arrow_load225.i.i, i64 0, i32 6
  %fun_ptr_load24.i.i = load i32 (i8*, i64, i32)*, i32 (i8*, i64, i32)** %struct_field_ptr23.i.i, align 8
  %fun_param_hack.i.i = bitcast <{ i8* }>* %struct_arrow_load.i.i to i8*
  %fun_call26.i.i = call i32 %fun_ptr_load24.i.i(i8* %fun_param_hack.i.i, i64 %struct_field50.i, i32 2) #5
  %struct_arg_0.i.i = getelementptr inbounds <{ i32, i32, i32, i32, i8* }>, <{ i32, i32, i32, i32, i8* }>* %struct_alloca.i.i, i64 0, i32 0
  %struct_arg_1.i.i = getelementptr inbounds <{ i32, i32, i32, i32, i8* }>, <{ i32, i32, i32, i32, i8* }>* %struct_alloca.i.i, i64 0, i32 1
  %struct_arg_2.i.i = getelementptr inbounds <{ i32, i32, i32, i32, i8* }>, <{ i32, i32, i32, i32, i8* }>* %struct_alloca.i.i, i64 0, i32 2
  %25 = bitcast <{ i32, i32, i32, i32, i8* }>* %struct_alloca.i.i to i64*
  store i64 0, i64* %25, align 8
  store i32 1, i32* %struct_arg_1.i.i, align 4
  store i32 24, i32* %struct_arg_0.i.i, align 8
  %26 = bitcast i32* %struct_arg_2.i.i to i8*
  call void @llvm.memset.p0i8.i64(i8* %26, i8 0, i64 16, i32 8, i1 false) #5
  %struct_arrow_load32.i.i = load <{ i8* }>*, <{ i8* }>** %dsound_obj.i.i, align 8
  %27 = bitcast <{ i8* }>* %struct_arrow_load32.i.i to <{ i8*, i8*, i8*, i32 (i8*, i8*, i8**, i8*)*, i8*, i8*, i32 (i8*, i64, i32)*, i8*, i8*, i8*, i8* }>**
  %struct_arrow_load346.i.i = load <{ i8*, i8*, i8*, i32 (i8*, i8*, i8**, i8*)*, i8*, i8*, i32 (i8*, i64, i32)*, i8*, i8*, i8*, i8* }>*, <{ i8*, i8*, i8*, i32 (i8*, i8*, i8**, i8*)*, i8*, i8*, i32 (i8*, i64, i32)*, i8*, i8*, i8*, i8* }>** %27, align 8
  %struct_field_ptr36.i.i = getelementptr inbounds <{ i8*, i8*, i8*, i32 (i8*, i8*, i8**, i8*)*, i8*, i8*, i32 (i8*, i64, i32)*, i8*, i8*, i8*, i8* }>, <{ i8*, i8*, i8*, i32 (i8*, i8*, i8**, i8*)*, i8*, i8*, i32 (i8*, i64, i32)*, i8*, i8*, i8*, i8* }>* %struct_arrow_load346.i.i, i64 0, i32 3
  %fun_ptr_load37.i.i = load i32 (i8*, i8*, i8**, i8*)*, i32 (i8*, i8*, i8**, i8*)** %struct_field_ptr36.i.i, align 8
  %fun_param_hack39.i.i = bitcast <{ i8* }>* %struct_arrow_load32.i.i to i8*
  %fun_param_hack41.i.i = bitcast <{ i8* }>** %primary_buffer.i.i to i8**
  %fun_call42.i.i = call i32 %fun_ptr_load37.i.i(i8* %fun_param_hack39.i.i, i8* %18, i8** %fun_param_hack41.i.i, i8* null) #5
  %struct_arg_044.i.i = getelementptr inbounds <{ i16, i16, i32, i32, i16, i16, i16 }>, <{ i16, i16, i32, i32, i16, i16, i16 }>* %struct_alloca43.i.i, i64 0, i32 0
  %struct_arg_145.i.i = getelementptr inbounds <{ i16, i16, i32, i32, i16, i16, i16 }>, <{ i16, i16, i32, i32, i16, i16, i16 }>* %struct_alloca43.i.i, i64 0, i32 1
  %struct_arg_246.i.i = getelementptr inbounds <{ i16, i16, i32, i32, i16, i16, i16 }>, <{ i16, i16, i32, i32, i16, i16, i16 }>* %struct_alloca43.i.i, i64 0, i32 2
  %struct_arg_347.i.i = getelementptr inbounds <{ i16, i16, i32, i32, i16, i16, i16 }>, <{ i16, i16, i32, i32, i16, i16, i16 }>* %struct_alloca43.i.i, i64 0, i32 3
  %struct_arg_448.i.i = getelementptr inbounds <{ i16, i16, i32, i32, i16, i16, i16 }>, <{ i16, i16, i32, i32, i16, i16, i16 }>* %struct_alloca43.i.i, i64 0, i32 4
  %struct_arg_5.i.i = getelementptr inbounds <{ i16, i16, i32, i32, i16, i16, i16 }>, <{ i16, i16, i32, i32, i16, i16, i16 }>* %struct_alloca43.i.i, i64 0, i32 5
  %struct_arg_6.i.i = getelementptr inbounds <{ i16, i16, i32, i32, i16, i16, i16 }>, <{ i16, i16, i32, i32, i16, i16, i16 }>* %struct_alloca43.i.i, i64 0, i32 6
  call void @llvm.memset.p0i8.i64(i8* %20, i8 0, i64 16, i32 8, i1 false) #5
  store i16 1, i16* %struct_arg_044.i.i, align 8
  store i16 2, i16* %struct_arg_145.i.i, align 2
  store i16 16, i16* %struct_arg_5.i.i, align 2
  store i32 48000, i32* %struct_arg_246.i.i, align 4
  store i16 4, i16* %struct_arg_448.i.i, align 4
  store i32 192000, i32* %struct_arg_347.i.i, align 8
  store i16 0, i16* %struct_arg_6.i.i, align 8
  %struct_arrow_load64.i.i = load <{ i8* }>*, <{ i8* }>** %primary_buffer.i.i, align 8
  %28 = bitcast <{ i8* }>* %struct_arrow_load64.i.i to <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>**
  %struct_arrow_load667.i.i = load <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>*, <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>** %28, align 8
  %struct_field_ptr68.i.i = getelementptr inbounds <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>, <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>* %struct_arrow_load667.i.i, i64 0, i32 14
  %fun_ptr_load69.i.i = load i32 (i8*, i8*)*, i32 (i8*, i8*)** %struct_field_ptr68.i.i, align 8
  %fun_param_hack71.i.i = bitcast <{ i8* }>* %struct_arrow_load64.i.i to i8*
  %fun_call73.i.i = call i32 %fun_ptr_load69.i.i(i8* %fun_param_hack71.i.i, i8* %20) #5
  %struct_arg_479.i.i = getelementptr inbounds <{ i32, i32, i32, i32, i8* }>, <{ i32, i32, i32, i32, i8* }>* %struct_alloca74.i.i, i64 0, i32 4
  call void @llvm.memset.p0i8.i64(i8* %21, i8 0, i64 16, i32 16, i1 false) #5
  %29 = bitcast i8** %struct_arg_479.i.i to <{ i16, i16, i32, i32, i16, i16, i16 }>**
  store <{ i16, i16, i32, i32, i16, i16, i16 }>* %struct_alloca43.i.i, <{ i16, i16, i32, i32, i16, i16, i16 }>** %29, align 16
  %30 = bitcast <{ i32, i32, i32, i32, i8* }>* %struct_alloca74.i.i to <4 x i32>*
  store <4 x i32> <i32 24, i32 65536, i32 192000, i32 0>, <4 x i32>* %30, align 16
  %struct_arrow_load85.i.i = load <{ i8* }>*, <{ i8* }>** %dsound_obj.i.i, align 8
  %31 = bitcast <{ i8* }>* %struct_arrow_load85.i.i to <{ i8*, i8*, i8*, i32 (i8*, i8*, i8**, i8*)*, i8*, i8*, i32 (i8*, i64, i32)*, i8*, i8*, i8*, i8* }>**
  %struct_arrow_load878.i.i = load <{ i8*, i8*, i8*, i32 (i8*, i8*, i8**, i8*)*, i8*, i8*, i32 (i8*, i64, i32)*, i8*, i8*, i8*, i8* }>*, <{ i8*, i8*, i8*, i32 (i8*, i8*, i8**, i8*)*, i8*, i8*, i32 (i8*, i64, i32)*, i8*, i8*, i8*, i8* }>** %31, align 8
  %struct_field_ptr89.i.i = getelementptr inbounds <{ i8*, i8*, i8*, i32 (i8*, i8*, i8**, i8*)*, i8*, i8*, i32 (i8*, i64, i32)*, i8*, i8*, i8*, i8* }>, <{ i8*, i8*, i8*, i32 (i8*, i8*, i8**, i8*)*, i8*, i8*, i32 (i8*, i64, i32)*, i8*, i8*, i8*, i8* }>* %struct_arrow_load878.i.i, i64 0, i32 3
  %fun_ptr_load90.i.i = load i32 (i8*, i8*, i8**, i8*)*, i32 (i8*, i8*, i8**, i8*)** %struct_field_ptr89.i.i, align 8
  %fun_param_hack92.i.i = bitcast <{ i8* }>* %struct_arrow_load85.i.i to i8*
  %fun_param_hack94.i.i = bitcast <{ i8* }>** %secondary_buffer.i.i to i8**
  %fun_call95.i.i = call i32 %fun_ptr_load90.i.i(i8* %fun_param_hack92.i.i, i8* %21, i8** %fun_param_hack94.i.i, i8* null) #5
  %secondary_buffer96.i.i = load <{ i8* }>*, <{ i8* }>** %secondary_buffer.i.i, align 8
  call void @llvm.lifetime.end(i64 11, i8* %15) #5
  call void @llvm.lifetime.end(i64 18, i8* %16) #5
  call void @llvm.lifetime.end(i64 8, i8* %17) #5
  call void @llvm.lifetime.end(i64 24, i8* %18) #5
  call void @llvm.lifetime.end(i64 8, i8* %19) #5
  call void @llvm.lifetime.end(i64 18, i8* %20) #5
  call void @llvm.lifetime.end(i64 24, i8* %21) #5
  call void @llvm.lifetime.end(i64 8, i8* %22) #5
  %hmpf.i = bitcast <{ i8* }>* %secondary_buffer96.i.i to i8*
  %fun_call60.i = call i8* @VirtualAlloc(i8* null, i64 192000, i32 4096, i32 4) #5
  %struct_arg_063.i = getelementptr inbounds <{ i1 (<{ i32, i8* }>, <{ i32, i8* }>)*, <{ i32, i8* }> (<{ i32, i8* }>)*, void (i8*)* }>, <{ i1 (<{ i32, i8* }>, <{ i32, i8* }>)*, <{ i32, i8* }> (<{ i32, i8* }>)*, void (i8*)* }>* %struct_alloca62.i, i64 0, i32 0
  %struct_arg_164.i = getelementptr inbounds <{ i1 (<{ i32, i8* }>, <{ i32, i8* }>)*, <{ i32, i8* }> (<{ i32, i8* }>)*, void (i8*)* }>, <{ i1 (<{ i32, i8* }>, <{ i32, i8* }>)*, <{ i32, i8* }> (<{ i32, i8* }>)*, void (i8*)* }>* %struct_alloca62.i, i64 0, i32 1
  %struct_arg_265.i = getelementptr inbounds <{ i1 (<{ i32, i8* }>, <{ i32, i8* }>)*, <{ i32, i8* }> (<{ i32, i8* }>)*, void (i8*)* }>, <{ i1 (<{ i32, i8* }>, <{ i32, i8* }>)*, <{ i32, i8* }> (<{ i32, i8* }>)*, void (i8*)* }>* %struct_alloca62.i, i64 0, i32 2
  store i1 (<{ i32, i8* }>, <{ i32, i8* }>)* @platform_write_file, i1 (<{ i32, i8* }>, <{ i32, i8* }>)** %struct_arg_063.i, align 8
  store <{ i32, i8* }> (<{ i32, i8* }>)* @platform_read_file, <{ i32, i8* }> (<{ i32, i8* }>)** %struct_arg_164.i, align 8
  store void (i8*)* @platform_free_file_memory, void (i8*)** %struct_arg_265.i, align 8
  %struct_arg_171.i = getelementptr inbounds <{ i8*, i64, i8*, i64, i8* }>, <{ i8*, i64, i8*, i64, i8* }>* %struct_alloca69.i, i64 0, i32 1
  %struct_arg_272.i = getelementptr inbounds <{ i8*, i64, i8*, i64, i8* }>, <{ i8*, i64, i8*, i64, i8* }>* %struct_alloca69.i, i64 0, i32 2
  %struct_arg_373.i = getelementptr inbounds <{ i8*, i64, i8*, i64, i8* }>, <{ i8*, i64, i8*, i64, i8* }>* %struct_alloca69.i, i64 0, i32 3
  %struct_arg_474.i = getelementptr inbounds <{ i8*, i64, i8*, i64, i8* }>, <{ i8*, i64, i8*, i64, i8* }>* %struct_alloca69.i, i64 0, i32 4
  call void @llvm.memset.p0i8.i64(i8* %5, i8 0, i64 40, i32 8, i1 false) #5
  store i64 1048576, i64* %struct_arg_171.i, align 8
  store i64 0, i64* %struct_arg_373.i, align 8
  %fun_call85.i = call i8* @VirtualAlloc(i8* inttoptr (i64 2199023255552 to i8*), i64 1048576, i32 12288, i32 4) #5
  store i8* %fun_call85.i, i8** %struct_arg_272.i, align 8
  %struct_field90.i = load i64, i64* %struct_arg_171.i, align 8
  %ptr_add.i = getelementptr i8, i8* %fun_call85.i, i64 %struct_field90.i
  store i8* %ptr_add.i, i8** %struct_arg_474.i, align 8
  %32 = bitcast <{ i8*, i64, i8*, i64, i8* }>* %struct_alloca69.i to <{ i1 (<{ i32, i8* }>, <{ i32, i8* }>)*, <{ i32, i8* }> (<{ i32, i8* }>)*, void (i8*)* }>**
  store <{ i1 (<{ i32, i8* }>, <{ i32, i8* }>)*, <{ i32, i8* }> (<{ i32, i8* }>)*, void (i8*)* }>* %struct_alloca62.i, <{ i1 (<{ i32, i8* }>, <{ i32, i8* }>)*, <{ i32, i8* }> (<{ i32, i8* }>)*, void (i8*)* }>** %32, align 8
  %33 = getelementptr inbounds [13 x i8], [13 x i8]* %arr_elem_alloca5.i.i, i64 0, i64 0
  call void @llvm.lifetime.start(i64 13, i8* %33) #5
  call void @llvm.lifetime.start(i64 18, i8* %16) #5
  %34 = getelementptr inbounds [23 x i8], [23 x i8]* %arr_elem_alloca3510.i.i, i64 0, i64 0
  call void @llvm.lifetime.start(i64 23, i8* %34) #5
  %35 = getelementptr inbounds [18 x i8], [18 x i8]* %arr_elem_alloca5012.i.i, i64 0, i64 0
  call void @llvm.lifetime.start(i64 18, i8* %35) #5
  %36 = getelementptr inbounds [13 x i8], [13 x i8]* %arr_elem_alloca6514.i.i, i64 0, i64 0
  call void @llvm.lifetime.start(i64 13, i8* %36) #5
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %33, i8* getelementptr inbounds ([13 x i8], [13 x i8]* @str.9, i64 0, i64 0), i32 13, i32 1, i1 false) #5
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %16, i8* getelementptr inbounds ([18 x i8], [18 x i8]* @str.10, i64 0, i64 0), i32 18, i32 1, i1 false) #5
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %34, i8* getelementptr inbounds ([23 x i8], [23 x i8]* @str.14, i64 0, i64 0), i32 23, i32 1, i1 false) #5
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %35, i8* getelementptr inbounds ([18 x i8], [18 x i8]* @str.16, i64 0, i64 0), i32 18, i32 1, i1 false) #5
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %36, i8* getelementptr inbounds ([14 x i8], [14 x i8]* @str.18, i64 0, i64 0), i32 13, i32 1, i1 false) #5
  %37 = bitcast <{ i32, i64, i64, i64, i64 }>* %data.i.i.i to i8*
  call void @llvm.lifetime.start(i64 36, i8* %37) #5
  %38 = getelementptr inbounds [13 x i8], [13 x i8]* %arr_elem_alloca32.i.i.i, i64 0, i64 0
  call void @llvm.lifetime.start(i64 13, i8* %38) #5
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %38, i8* getelementptr inbounds ([13 x i8], [13 x i8]* @str.9, i64 0, i64 0), i32 13, i32 1, i1 false) #5
  %fun_call13.i.i.i = call i32 @GetFileAttributesExA(i8* %38, i32 0, <{ i32, i64, i64, i64, i64 }>* nonnull %data.i.i.i) #5
  %struct_field_ptr.i.i.i = getelementptr inbounds <{ i32, i64, i64, i64, i64 }>, <{ i32, i64, i64, i64, i64 }>* %data.i.i.i, i64 0, i32 3
  %struct_field.i.i.i = load i64, i64* %struct_field_ptr.i.i.i, align 8
  call void @llvm.lifetime.end(i64 36, i8* %37) #5
  call void @llvm.lifetime.end(i64 13, i8* %38) #5
  %fun_call22.i.i = call i32 @CopyFileA(i8* %33, i8* %16, i32 0) #5
  %fun_call31.i.i = call i64 @LoadLibraryA(i8* %16) #5
  %fun_call45.i.i = call i8* @GetProcAddress(i64 %fun_call31.i.i, i8* %34) #5
  %fun_call60.i.i = call i8* @GetProcAddress(i64 %fun_call31.i.i, i8* %35) #5
  %console_output_handle.i.i.i = load i64, i64* @console_output_handle, align 8
  %fun_call.i.i.i = call i32 @WriteFile(i64 %console_output_handle.i.i.i, i8* %36, i32 13, i32* null, i8* null) #5
  call void @llvm.lifetime.end(i64 13, i8* %33) #5
  call void @llvm.lifetime.end(i64 18, i8* %16) #5
  call void @llvm.lifetime.end(i64 23, i8* %34) #5
  call void @llvm.lifetime.end(i64 18, i8* %35) #5
  call void @llvm.lifetime.end(i64 13, i8* %36) #5
  %39 = bitcast i8** %region1.i.i to i8*
  call void @llvm.lifetime.start(i64 8, i8* %39) #5
  %40 = bitcast i32* %region1_size.i.i to i8*
  call void @llvm.lifetime.start(i64 4, i8* %40) #5
  %41 = bitcast i8** %region2.i.i to i8*
  call void @llvm.lifetime.start(i64 8, i8* %41) #5
  %42 = bitcast i32* %region2_size.i.i to i8*
  call void @llvm.lifetime.start(i64 4, i8* %42) #5
  %43 = bitcast <{ i8* }>* %secondary_buffer96.i.i to <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>**
  %struct_arrow_load22.i.i = load <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>*, <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>** %43, align 8
  %struct_field_ptr4.i.i = getelementptr inbounds <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>, <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>* %struct_arrow_load22.i.i, i64 0, i32 11
  %fun_ptr_load.i.i = load i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)** %struct_field_ptr4.i.i, align 8
  %fun_call.i49.i = call i32 %fun_ptr_load.i.i(i8* %hmpf.i, i32 0, i32 192000, i8** nonnull %region1.i.i, i32* nonnull %region1_size.i.i, i8** nonnull %region2.i.i, i32* nonnull %region2_size.i.i, i32 0) #5
  %44 = bitcast i8** %region1.i.i to i32**
  %region183.i.i = load i32*, i32** %44, align 8
  %region1_size9.i.i = load i32, i32* %region1_size.i.i, align 4
  %icmp_tmp6.i.i = icmp sgt i32 %region1_size9.i.i, 3
  br i1 %icmp_tmp6.i.i, label %for.preheader.i.i, label %vars.end_for_crit_edge.i.i

vars.end_for_crit_edge.i.i:                       ; preds = %create_backbuffer.exit.i
  %45 = ptrtoint i32* %region183.i.i to i64
  %46 = bitcast i32* %region183.i.i to i8*
  br label %clear_sound_buffer.exit.i

for.preheader.i.i:                                ; preds = %create_backbuffer.exit.i
  %div_tmp.i307.i = lshr i32 %region1_size9.i.i, 2
  %region1839.i.i = bitcast i32* %region183.i.i to i8*
  %47 = add nsw i32 %div_tmp.i307.i, -1
  %48 = zext i32 %47 to i64
  %49 = shl nuw nsw i64 %48, 2
  %50 = add nuw nsw i64 %49, 4
  call void @llvm.memset.p0i8.i64(i8* %region1839.i.i, i8 0, i64 %50, i32 4, i1 false) #5
  %.phi.trans.insert.i.i = bitcast i8** %region1.i.i to i64*
  %region1154.pre.i.i = load i64, i64* %.phi.trans.insert.i.i, align 8
  %51 = inttoptr i64 %region1154.pre.i.i to i8*
  %region1_size27.pre.i.i = load i32, i32* %region1_size.i.i, align 4
  br label %clear_sound_buffer.exit.i

clear_sound_buffer.exit.i:                        ; preds = %for.preheader.i.i, %vars.end_for_crit_edge.i.i
  %region1_size27.i.i = phi i32 [ %region1_size27.pre.i.i, %for.preheader.i.i ], [ %region1_size9.i.i, %vars.end_for_crit_edge.i.i ]
  %region126.i.i = phi i8* [ %51, %for.preheader.i.i ], [ %46, %vars.end_for_crit_edge.i.i ]
  %region1154.i.i = phi i64 [ %region1154.pre.i.i, %for.preheader.i.i ], [ %45, %vars.end_for_crit_edge.i.i ]
  %struct_arrow_load205.i.i = load <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>*, <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>** %43, align 8
  %struct_field_ptr22.i.i = getelementptr inbounds <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>, <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>* %struct_arrow_load205.i.i, i64 0, i32 19
  %fun_ptr_load23.i.i = load i32 (i8*, i8*, i32, i8*, i32)*, i32 (i8*, i8*, i32, i8*, i32)** %struct_field_ptr22.i.i, align 8
  %region228.i.i = load i8*, i8** %region2.i.i, align 8
  %region2_size29.i.i = load i32, i32* %region2_size.i.i, align 4
  %fun_call30.i.i = call i32 %fun_ptr_load23.i.i(i8* %hmpf.i, i8* %region126.i.i, i32 %region1_size27.i.i, i8* %region228.i.i, i32 %region2_size29.i.i) #5
  call void @llvm.lifetime.end(i64 8, i8* %39) #5
  call void @llvm.lifetime.end(i64 4, i8* %40) #5
  call void @llvm.lifetime.end(i64 8, i8* %41) #5
  call void @llvm.lifetime.end(i64 4, i8* %42) #5
  %struct_arrow_load10541.i = load <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>*, <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>** %43, align 8
  %struct_field_ptr106.i = getelementptr inbounds <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>, <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>* %struct_arrow_load10541.i, i64 0, i32 12
  %fun_ptr_load.i = load i32 (i8*, i32, i32, i32)*, i32 (i8*, i32, i32, i32)** %struct_field_ptr106.i, align 8
  %fun_call108.i = call i32 %fun_ptr_load.i(i8* %hmpf.i, i32 0, i32 0, i32 1) #5
  %52 = call { i32, i32 } asm sideeffect "rdtsc", "={ax},={dx},~{dirflag},~{fpsr},~{flags}"() #5
  %53 = extractvalue { i32, i32 } %52, 0
  %54 = extractvalue { i32, i32 } %52, 1
  %55 = zext i32 %54 to i64
  %56 = shl nuw i64 %55, 32
  %57 = zext i32 %53 to i64
  %58 = or i64 %56, %57
  %59 = bitcast i64* %result.i.i to i8*
  call void @llvm.lifetime.start(i64 8, i8* %59) #5
  %fun_call.i50.i = call i32 @QueryPerformanceCounter(i64* nonnull %result.i.i) #5
  %result1.i.i = load i64, i64* %result.i.i, align 8
  call void @llvm.lifetime.end(i64 8, i8* %59) #5
  %.fca.0.gep.i = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca111.i, i64 0, i32 0, i32 0
  %.fca.1.gep.i = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca111.i, i64 0, i32 0, i32 1
  %.fca.0.gep9.i = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca111.i, i64 0, i32 1, i32 0
  %.fca.1.gep10.i = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca111.i, i64 0, i32 1, i32 1
  %.fca.2.gep.i = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca111.i, i64 0, i32 1, i32 2
  %.fca.0.gep11.i = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca111.i, i64 0, i32 2, i32 0
  %.fca.1.gep12.i = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca111.i, i64 0, i32 2, i32 1
  %.fca.2.gep13.i = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca111.i, i64 0, i32 2, i32 2
  %.fca.0.gep14.i = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca111.i, i64 0, i32 3, i32 0
  %.fca.1.gep15.i = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca111.i, i64 0, i32 3, i32 1
  %.fca.2.gep16.i = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca111.i, i64 0, i32 3, i32 2
  %.fca.0.gep17.i = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca111.i, i64 0, i32 4, i32 0
  %.fca.1.gep18.i = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca111.i, i64 0, i32 4, i32 1
  %.fca.2.gep19.i = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca111.i, i64 0, i32 4, i32 2
  %.fca.0.gep20.i = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca111.i, i64 0, i32 5, i32 0
  %.fca.1.gep21.i = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca111.i, i64 0, i32 5, i32 1
  %.fca.2.gep22.i = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca111.i, i64 0, i32 5, i32 2
  %.fca.0.gep23.i = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca111.i, i64 0, i32 6, i32 0
  %.fca.1.gep24.i = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca111.i, i64 0, i32 6, i32 1
  %.fca.2.gep25.i = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca111.i, i64 0, i32 6, i32 2
  %.fca.0.gep26.i = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca111.i, i64 0, i32 7, i32 0
  %.fca.1.gep27.i = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca111.i, i64 0, i32 7, i32 1
  %.fca.2.gep28.i = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca111.i, i64 0, i32 7, i32 2
  %struct_arg_8120.i = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca111.i, i64 0, i32 8
  %struct_arg_9121.i = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca111.i, i64 0, i32 9
  %struct_arg_11123.i = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %struct_alloca111.i, i64 0, i32 11
  %struct_arg_0125.i = getelementptr inbounds <{ i16*, i32, i32, float }>, <{ i16*, i32, i32, float }>* %struct_alloca124.i, i64 0, i32 0
  call void @llvm.memset.p0i8.i64(i8* %6, i8 0, i64 42, i32 8, i1 false) #5
  %struct_arg_1126.i = getelementptr inbounds <{ i16*, i32, i32, float }>, <{ i16*, i32, i32, float }>* %struct_alloca124.i, i64 0, i32 1
  %struct_arg_2127.i = getelementptr inbounds <{ i16*, i32, i32, float }>, <{ i16*, i32, i32, float }>* %struct_alloca124.i, i64 0, i32 2
  %60 = bitcast <{ i16*, i32, i32, float }>* %struct_alloca124.i to i8**
  %61 = bitcast i32* %struct_arg_1126.i to i8*
  call void @llvm.memset.p0i8.i64(i8* %61, i8 0, i64 12, i32 8, i1 false) #5
  store i8* %fun_call60.i, i8** %60, align 8
  store i32 48000, i32* %struct_arg_2127.i, align 4
  %62 = bitcast i64* %result.i51.i to i8*
  call void @llvm.lifetime.start(i64 8, i8* %62) #5
  %fun_call.i52.i = call i32 @QueryPerformanceCounter(i64* nonnull %result.i51.i) #5
  %result1.i53.i = load i64, i64* %result.i51.i, align 8
  call void @llvm.lifetime.end(i64 8, i8* %62) #5
  %perf_count_freq.i = load i64, i64* @perf_count_freq, align 8
  %int_to_float_cast139.i = sitofp i64 %perf_count_freq.i to float
  %fmul_tmp.i = fmul float %int_to_float_cast139.i, 0x3F91111120000000
  %int_cast140.i = fptosi float %fmul_tmp.i to i64
  %sub_tmp.i = sub i64 %result1.i53.i, %int_cast140.i
  %63 = bitcast i64* %result.i54.i to i8*
  %64 = bitcast <{ i64, i32, i64, i64, i32, <{ i32, i32 }>, i64 }>* %msg.i.i to i8*
  %struct_arg_1146.i = getelementptr inbounds <{ i8*, i32, i32, i32 }>, <{ i8*, i32, i32, i32 }>* %struct_alloca144.i, i64 0, i32 1
  %struct_arg_2147.i = getelementptr inbounds <{ i8*, i32, i32, i32 }>, <{ i8*, i32, i32, i32 }>* %struct_alloca144.i, i64 0, i32 2
  %struct_arg_3148.i = getelementptr inbounds <{ i8*, i32, i32, i32 }>, <{ i8*, i32, i32, i32 }>* %struct_alloca144.i, i64 0, i32 3
  %65 = bitcast <{ i8*, i32, i32, i32 }>* %struct_alloca144.i to i64*
  %66 = bitcast i64* %cursor_pos.i.i to i8*
  %tmpcast.i.i = bitcast i64* %cursor_pos.i.i to <{ i32, i32 }>*
  %67 = bitcast i32* %play_cursor.i.i to i8*
  %68 = bitcast i32* %write_cursor.i.i to i8*
  %69 = bitcast i64* %time.i.i to i8*
  %70 = bitcast i8** %region1.i149.i to i8*
  %71 = bitcast i32* %region1_size.i150.i to i8*
  %72 = bitcast i8** %region2.i151.i to i8*
  %73 = bitcast i32* %region2_size.i152.i to i8*
  %74 = bitcast i8** %region1.i149.i to i16**
  %75 = bitcast i8** %region2.i151.i to i16**
  %76 = bitcast i8** %region1.i149.i to i32**
  %77 = bitcast i8** %region2.i151.i to i32**
  %78 = bitcast i64* %result.i211.i to i8*
  %79 = bitcast i64* %result.i219.i to i8*
  %80 = bitcast i8** %region1.i170.i to i8*
  %81 = bitcast i32* %region1_size.i171.i to i8*
  %82 = bitcast i8** %region2.i172.i to i8*
  %83 = bitcast i32* %region2_size.i173.i to i8*
  %84 = bitcast i8** %region1.i170.i to i32**
  %.phi.trans.insert.i190.i = bitcast i8** %region1.i170.i to i64*
  %85 = bitcast i64* %result.i33.i.i to i8*
  %struct_field_ptr.i58.i = getelementptr inbounds <{ i64, i32, i64, i64, i32, <{ i32, i32 }>, i64 }>, <{ i64, i32, i64, i64, i32, <{ i32, i32 }>, i64 }>* %msg.i.i, i64 0, i32 1
  %struct_field_ptr18.i.i = getelementptr inbounds <{ i64, i32, i64, i64, i32, <{ i32, i32 }>, i64 }>, <{ i64, i32, i64, i64, i32, <{ i32, i32 }>, i64 }>* %msg.i.i, i64 0, i32 2
  %struct_field_ptr20.i.i = getelementptr inbounds <{ i64, i32, i64, i64, i32, <{ i32, i32 }>, i64 }>, <{ i64, i32, i64, i64, i32, <{ i32, i32 }>, i64 }>* %msg.i.i, i64 0, i32 3
  %86 = bitcast i16* %arr_elem_alloca1.i240.i to i8*
  %87 = bitcast <{ i32, i64, i64, i64, i64 }>* %data.i.i244.i to i8*
  %struct_field_ptr.i.i247.i = getelementptr inbounds <{ i32, i64, i64, i64, i64 }>, <{ i32, i64, i64, i64, i64 }>* %data.i.i244.i, i64 0, i32 3
  %88 = getelementptr inbounds [15 x i8], [15 x i8]* %arr_elem_alloca1.i.i.i, i64 0, i64 0
  %89 = bitcast <{ i32, i64, i64, i64, i64 }>* %data.i.i.i.i to i8*
  %struct_field_ptr.i.i.i.i = getelementptr inbounds <{ i32, i64, i64, i64, i64 }>, <{ i32, i64, i64, i64, i64 }>* %data.i.i.i.i, i64 0, i32 3
  %90 = bitcast i32* %struct_arg_1146.i to i64*
  br label %while_cond.outer.i

while_cond.outer.i:                               ; preds = %unload_game_dll.exit.i.i, %clear_sound_buffer.exit.i
  %sound_output.sroa.29.0.ph.i = phi i64 [ %sound_output.sroa.29.1.i.lcssa.lcssa, %unload_game_dll.exit.i.i ], [ %region1154.i.i, %clear_sound_buffer.exit.i ]
  %sound_output.sroa.33.0.ph.i = phi i32 [ %sound_output.sroa.33.3.i.lcssa.lcssa, %unload_game_dll.exit.i.i ], [ undef, %clear_sound_buffer.exit.i ]
  %sound_output.sroa.37.0.ph.i = phi i32 [ %sound_output.sroa.37.2.i.lcssa.lcssa, %unload_game_dll.exit.i.i ], [ undef, %clear_sound_buffer.exit.i ]
  %sound_output.sroa.40.0.ph.i = phi i64 [ %sound_output.sroa.40.3.i.lcssa.lcssa, %unload_game_dll.exit.i.i ], [ undef, %clear_sound_buffer.exit.i ]
  %sound_output.sroa.43.0.ph.i = phi i32 [ %sound_output.sroa.43.1.i.lcssa.lcssa, %unload_game_dll.exit.i.i ], [ undef, %clear_sound_buffer.exit.i ]
  %sound_output.sroa.45.0.ph.i = phi i32 [ %sound_output.sroa.45.1.i.lcssa.lcssa, %unload_game_dll.exit.i.i ], [ undef, %clear_sound_buffer.exit.i ]
  %game.sroa.12.0.ph.i = phi i64 [ %struct_field.i.i.i.i, %unload_game_dll.exit.i.i ], [ %struct_field.i.i.i, %clear_sound_buffer.exit.i ]
  %game.sroa.7.0.ph.in.i = phi i8* [ %fun_call60.i.i.i, %unload_game_dll.exit.i.i ], [ %fun_call60.i.i, %clear_sound_buffer.exit.i ]
  %game.sroa.4.0.ph.in.i = phi i8* [ %fun_call45.i.i.i, %unload_game_dll.exit.i.i ], [ %fun_call45.i.i, %clear_sound_buffer.exit.i ]
  %game.sroa.0.0.ph.i = phi i64 [ %fun_call31.i.i.i, %unload_game_dll.exit.i.i ], [ %fun_call31.i.i, %clear_sound_buffer.exit.i ]
  %frame_start_counter.0.ph.i = phi i64 [ %result1.i56.i.lcssa.lcssa, %unload_game_dll.exit.i.i ], [ %sub_tmp.i, %clear_sound_buffer.exit.i ]
  %last_cycle_count.0.ph.i = phi i64 [ %.lcssa.lcssa, %unload_game_dll.exit.i.i ], [ %58, %clear_sound_buffer.exit.i ]
  %last_counter_debug_stats.0.ph.i = phi i64 [ %frame_end_counter.0.lcssa.i.lcssa.lcssa, %unload_game_dll.exit.i.i ], [ 0, %clear_sound_buffer.exit.i ]
  %running.0.ph.i = phi i1 [ %fun_call175.i.lcssa.lcssa, %unload_game_dll.exit.i.i ], [ true, %clear_sound_buffer.exit.i ]
  %game.sroa.4.0.ph.i = bitcast i8* %game.sroa.4.0.ph.in.i to i1 (i8*, i8*, i8*)*
  %game.sroa.7.0.ph.i = bitcast i8* %game.sroa.7.0.ph.in.i to void (i8*, i8*)*
  br label %while_cond.outer284.i

while_cond.outer284.i:                            ; preds = %then227.i, %while_cond.outer.i
  %sound_output.sroa.29.0.ph285.i = phi i64 [ %sound_output.sroa.29.0.ph.i, %while_cond.outer.i ], [ %sound_output.sroa.29.1.i.lcssa, %then227.i ]
  %sound_output.sroa.33.0.ph286.i = phi i32 [ %sound_output.sroa.33.0.ph.i, %while_cond.outer.i ], [ %sound_output.sroa.33.3.i.lcssa, %then227.i ]
  %sound_output.sroa.37.0.ph287.i = phi i32 [ %sound_output.sroa.37.0.ph.i, %while_cond.outer.i ], [ %sound_output.sroa.37.2.i.lcssa, %then227.i ]
  %sound_output.sroa.40.0.ph288.i = phi i64 [ %sound_output.sroa.40.0.ph.i, %while_cond.outer.i ], [ %sound_output.sroa.40.3.i.lcssa, %then227.i ]
  %sound_output.sroa.43.0.ph289.i = phi i32 [ %sound_output.sroa.43.0.ph.i, %while_cond.outer.i ], [ %sound_output.sroa.43.1.i.lcssa, %then227.i ]
  %sound_output.sroa.45.0.ph296.i = phi i32 [ %sound_output.sroa.45.0.ph.i, %while_cond.outer.i ], [ %sound_output.sroa.45.1.i.lcssa, %then227.i ]
  %frame_start_counter.0.ph297.i = phi i64 [ %frame_start_counter.0.ph.i, %while_cond.outer.i ], [ %result1.i56.i.lcssa, %then227.i ]
  %last_cycle_count.0.ph298.i = phi i64 [ %last_cycle_count.0.ph.i, %while_cond.outer.i ], [ %.lcssa, %then227.i ]
  %last_counter_debug_stats.0.ph299.i = phi i64 [ %last_counter_debug_stats.0.ph.i, %while_cond.outer.i ], [ %frame_end_counter.0.lcssa.i.lcssa, %then227.i ]
  %running.0.ph301.i = phi i1 [ %running.0.ph.i, %while_cond.outer.i ], [ %fun_call175.i.lcssa, %then227.i ]
  br label %while_cond.i

while_cond.i:                                     ; preds = %while_end205.i, %while_cond.outer284.i
  %sound_output.sroa.29.0.i = phi i64 [ %sound_output.sroa.29.1.i, %while_end205.i ], [ %sound_output.sroa.29.0.ph285.i, %while_cond.outer284.i ]
  %sound_output.sroa.33.0.i = phi i32 [ %sound_output.sroa.33.3.i, %while_end205.i ], [ %sound_output.sroa.33.0.ph286.i, %while_cond.outer284.i ]
  %sound_output.sroa.37.0.i = phi i32 [ %sound_output.sroa.37.2.i, %while_end205.i ], [ %sound_output.sroa.37.0.ph287.i, %while_cond.outer284.i ]
  %sound_output.sroa.40.0.i = phi i64 [ %sound_output.sroa.40.3.i, %while_end205.i ], [ %sound_output.sroa.40.0.ph288.i, %while_cond.outer284.i ]
  %sound_output.sroa.43.0.i = phi i32 [ %sound_output.sroa.43.1.i, %while_end205.i ], [ %sound_output.sroa.43.0.ph289.i, %while_cond.outer284.i ]
  %sound_output.sroa.45.0.i = phi i32 [ %sound_output.sroa.45.1.i, %while_end205.i ], [ %sound_output.sroa.45.0.ph296.i, %while_cond.outer284.i ]
  %frame_start_counter.0.i = phi i64 [ %result1.i56.i, %while_end205.i ], [ %frame_start_counter.0.ph297.i, %while_cond.outer284.i ]
  %last_cycle_count.0.i = phi i64 [ %121, %while_end205.i ], [ %last_cycle_count.0.ph298.i, %while_cond.outer284.i ]
  %running.0.i = phi i1 [ %fun_call175.i, %while_end205.i ], [ %running.0.ph301.i, %while_cond.outer284.i ]
  br i1 %running.0.i, label %while.i, label %main.exit

while.i:                                          ; preds = %while_cond.i
  call void @llvm.lifetime.start(i64 8, i8* %63) #5
  %fun_call.i55.i = call i32 @QueryPerformanceCounter(i64* nonnull %result.i54.i) #5
  %result1.i56.i = load i64, i64* %result.i54.i, align 8
  call void @llvm.lifetime.end(i64 8, i8* %63) #5
  call void @llvm.lifetime.start(i64 48, i8* %64) #5
  %fun_call30.i57.i = call i32 @PeekMessageA(<{ i64, i32, i64, i64, i32, <{ i32, i32 }>, i64 }>* nonnull %msg.i.i, i64 0, i32 0, i32 0, i32 1) #5
  %icmp_tmp31.i.i = icmp eq i32 %fun_call30.i57.i, 0
  br i1 %icmp_tmp31.i.i, label %process_pending_messages.exit.i, label %while.i.i.preheader

while.i.i.preheader:                              ; preds = %while.i
  br label %while.i.i

while.i.i:                                        ; preds = %while.i.i.preheader, %while_cond.backedge.i.i
  %struct_field.i60.i = load i32, i32* %struct_field_ptr.i58.i, align 8
  %icmp_tmp2.i.i = icmp eq i32 %struct_field.i60.i, 18
  br i1 %icmp_tmp2.i.i, label %then.i61.i, label %elif_0.i.i

then.i61.i:                                       ; preds = %while.i.i
  store i1 true, i1* %struct_arg_11123.i, align 1
  br label %while_cond.backedge.i.i

elif_0.i.i:                                       ; preds = %while.i.i
  %91 = or i32 %struct_field.i60.i, 1
  switch i32 %91, label %else.i.i [
    i32 261, label %elif_0_then.i.i
    i32 257, label %elif_0_then.i.i
  ]

elif_0_then.i.i:                                  ; preds = %elif_0.i.i, %elif_0.i.i
  %struct_field19.i.i = load i64, i64* %struct_field_ptr18.i.i, align 8
  %shr_tmp1.i.i = lshr i64 %struct_field19.i.i, 32
  %int_trunc.i.i = trunc i64 %shr_tmp1.i.i to i32
  %struct_field21.i.i = load i64, i64* %struct_field_ptr20.i.i, align 8
  %shr_tmp222.i.i = lshr i64 %struct_field21.i.i, 32
  %int_trunc23.i.i = trunc i64 %shr_tmp222.i.i to i32
  %icmp_tmp29.i.i = icmp sgt i32 %int_trunc23.i.i, -1
  switch i32 %int_trunc.i.i, label %while_cond.backedge.i.i [
    i32 27, label %then40.i.i
    i32 37, label %elif_0_then44.i.i
    i32 39, label %elif_1_then.i.i
    i32 38, label %elif_2_then.i.i
    i32 40, label %elif_3_then.i.i
  ]

then40.i.i:                                       ; preds = %elif_0_then.i.i
  store i1 true, i1* %struct_arg_11123.i, align 1
  br label %while_cond.backedge.i.i

while_cond.backedge.i.i:                          ; preds = %else.i.i, %update_game_button.exit11.i.i, %update_game_button.exit20.i.i, %update_game_button.exit29.i.i, %update_game_button.exit.i.i, %then40.i.i, %elif_0_then.i.i, %then.i61.i
  %fun_call.i62.i = call i32 @PeekMessageA(<{ i64, i32, i64, i64, i32, <{ i32, i32 }>, i64 }>* nonnull %msg.i.i, i64 0, i32 0, i32 0, i32 1) #5
  %icmp_tmp.i63.i = icmp eq i32 %fun_call.i62.i, 0
  br i1 %icmp_tmp.i63.i, label %process_pending_messages.exit.i.loopexit, label %while.i.i

elif_0_then44.i.i:                                ; preds = %elif_0_then.i.i
  %struct_arrow.i.i.i = load i1, i1* %.fca.0.gep11.i, align 1
  br i1 %icmp_tmp29.i.i, label %cand.rhs.i.i.i, label %cand.rhs3.i.i.i

cand.rhs.i.i.i:                                   ; preds = %elif_0_then44.i.i
  br i1 %struct_arrow.i.i.i, label %update_game_button.exit.i.i, label %then.i.i.i

then.i.i.i:                                       ; preds = %cand.rhs.i.i.i
  store i1 true, i1* %.fca.1.gep12.i, align 1
  br label %update_game_button.exit.i.i

cand.rhs3.i.i.i:                                  ; preds = %elif_0_then44.i.i
  br i1 %struct_arrow.i.i.i, label %then8.i.i.i, label %update_game_button.exit.i.i

then8.i.i.i:                                      ; preds = %cand.rhs3.i.i.i
  store i1 true, i1* %.fca.2.gep13.i, align 1
  br label %update_game_button.exit.i.i

update_game_button.exit.i.i:                      ; preds = %then8.i.i.i, %cand.rhs3.i.i.i, %then.i.i.i, %cand.rhs.i.i.i
  store i1 %icmp_tmp29.i.i, i1* %.fca.0.gep11.i, align 1
  br label %while_cond.backedge.i.i

elif_1_then.i.i:                                  ; preds = %elif_0_then.i.i
  %struct_arrow.i22.i.i = load i1, i1* %.fca.0.gep14.i, align 2
  br i1 %icmp_tmp29.i.i, label %cand.rhs.i23.i.i, label %cand.rhs3.i26.i.i

cand.rhs.i23.i.i:                                 ; preds = %elif_1_then.i.i
  br i1 %struct_arrow.i22.i.i, label %update_game_button.exit29.i.i, label %then.i25.i.i

then.i25.i.i:                                     ; preds = %cand.rhs.i23.i.i
  store i1 true, i1* %.fca.1.gep15.i, align 1
  br label %update_game_button.exit29.i.i

cand.rhs3.i26.i.i:                                ; preds = %elif_1_then.i.i
  br i1 %struct_arrow.i22.i.i, label %then8.i28.i.i, label %update_game_button.exit29.i.i

then8.i28.i.i:                                    ; preds = %cand.rhs3.i26.i.i
  store i1 true, i1* %.fca.2.gep16.i, align 2
  br label %update_game_button.exit29.i.i

update_game_button.exit29.i.i:                    ; preds = %then8.i28.i.i, %cand.rhs3.i26.i.i, %then.i25.i.i, %cand.rhs.i23.i.i
  store i1 %icmp_tmp29.i.i, i1* %.fca.0.gep14.i, align 2
  br label %while_cond.backedge.i.i

elif_2_then.i.i:                                  ; preds = %elif_0_then.i.i
  %struct_arrow.i13.i.i = load i1, i1* %.fca.0.gep9.i, align 8
  br i1 %icmp_tmp29.i.i, label %cand.rhs.i14.i.i, label %cand.rhs3.i17.i.i

cand.rhs.i14.i.i:                                 ; preds = %elif_2_then.i.i
  br i1 %struct_arrow.i13.i.i, label %update_game_button.exit20.i.i, label %then.i16.i.i

then.i16.i.i:                                     ; preds = %cand.rhs.i14.i.i
  store i1 true, i1* %.fca.1.gep10.i, align 1
  br label %update_game_button.exit20.i.i

cand.rhs3.i17.i.i:                                ; preds = %elif_2_then.i.i
  br i1 %struct_arrow.i13.i.i, label %then8.i19.i.i, label %update_game_button.exit20.i.i

then8.i19.i.i:                                    ; preds = %cand.rhs3.i17.i.i
  store i1 true, i1* %.fca.2.gep.i, align 2
  br label %update_game_button.exit20.i.i

update_game_button.exit20.i.i:                    ; preds = %then8.i19.i.i, %cand.rhs3.i17.i.i, %then.i16.i.i, %cand.rhs.i14.i.i
  store i1 %icmp_tmp29.i.i, i1* %.fca.0.gep9.i, align 8
  br label %while_cond.backedge.i.i

elif_3_then.i.i:                                  ; preds = %elif_0_then.i.i
  %struct_arrow.i4.i.i = load i1, i1* %.fca.0.gep17.i, align 1
  br i1 %icmp_tmp29.i.i, label %cand.rhs.i5.i.i, label %cand.rhs3.i8.i.i

cand.rhs.i5.i.i:                                  ; preds = %elif_3_then.i.i
  br i1 %struct_arrow.i4.i.i, label %update_game_button.exit11.i.i, label %then.i7.i.i

then.i7.i.i:                                      ; preds = %cand.rhs.i5.i.i
  store i1 true, i1* %.fca.1.gep18.i, align 1
  br label %update_game_button.exit11.i.i

cand.rhs3.i8.i.i:                                 ; preds = %elif_3_then.i.i
  br i1 %struct_arrow.i4.i.i, label %then8.i10.i.i, label %update_game_button.exit11.i.i

then8.i10.i.i:                                    ; preds = %cand.rhs3.i8.i.i
  store i1 true, i1* %.fca.2.gep19.i, align 1
  br label %update_game_button.exit11.i.i

update_game_button.exit11.i.i:                    ; preds = %then8.i10.i.i, %cand.rhs3.i8.i.i, %then.i7.i.i, %cand.rhs.i5.i.i
  store i1 %icmp_tmp29.i.i, i1* %.fca.0.gep17.i, align 1
  br label %while_cond.backedge.i.i

else.i.i:                                         ; preds = %elif_0.i.i
  %fun_call61.i.i = call i32 @TranslateMessage(<{ i64, i32, i64, i64, i32, <{ i32, i32 }>, i64 }>* nonnull %msg.i.i) #5
  %fun_call62.i.i = call i64 @DispatchMessageA(<{ i64, i32, i64, i64, i32, <{ i32, i32 }>, i64 }>* nonnull %msg.i.i) #5
  br label %while_cond.backedge.i.i

process_pending_messages.exit.i.loopexit:         ; preds = %while_cond.backedge.i.i
  br label %process_pending_messages.exit.i

process_pending_messages.exit.i:                  ; preds = %process_pending_messages.exit.i.loopexit, %while.i
  call void @llvm.lifetime.end(i64 48, i8* %64) #5
  store i64 0, i64* %90, align 8
  %struct_field15042.i = load i64, i64* bitcast (i8** getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i64 0, i32 1) to i64*), align 8
  store i64 %struct_field15042.i, i64* %65, align 8
  %struct_field152.i = load i32, i32* getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i64 0, i32 2), align 4
  store i32 %struct_field152.i, i32* %struct_arg_1146.i, align 8
  %struct_field154.i = load i64, i64* bitcast (i32* getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i64 0, i32 3) to i64*), align 8
  %92 = trunc i64 %struct_field154.i to i32
  store i32 %92, i32* %struct_arg_2147.i, align 4
  %93 = lshr i64 %struct_field154.i, 32
  %94 = trunc i64 %93 to i32
  store i32 %94, i32* %struct_arg_3148.i, align 8
  %sub_tmp.i.i = sub i64 %result1.i56.i, %result1.i.i
  %int_to_float_cast.i.i = sitofp i64 %sub_tmp.i.i to float
  %perf_count_freq.i.i = load i64, i64* @perf_count_freq, align 8
  %int_to_float_cast1.i.i = sitofp i64 %perf_count_freq.i.i to float
  %fdiv_tmp.i.i = fdiv float %int_to_float_cast.i.i, %int_to_float_cast1.i.i
  %sub_tmp.i64.i = sub i64 %result1.i56.i, %frame_start_counter.0.i
  %int_to_float_cast.i65.i = sitofp i64 %sub_tmp.i64.i to float
  %fdiv_tmp.i68.i = fdiv float %int_to_float_cast.i65.i, %int_to_float_cast1.i.i
  store float %fdiv_tmp.i68.i, float* %.fca.0.gep.i, align 8
  store float %fdiv_tmp.i.i, float* %.fca.1.gep.i, align 4
  %struct_field169.i = load i1, i1* %struct_arg_11123.i, align 1
  %window_requests_quit.i = load i1, i1* @window_requests_quit, align 1
  %or_tmp.i = or i1 %struct_field169.i, %window_requests_quit.i
  store i1 %or_tmp.i, i1* %struct_arg_11123.i, align 1
  call void @llvm.lifetime.start(i64 8, i8* %66) #5
  %fun_call.i69.i = call i32 @GetCursorPos(<{ i32, i32 }>* nonnull %tmpcast.i.i) #5
  %struct_field.i70.i = load i64, i64* @window.0, align 8
  %fun_call1.i.i = call i32 @ScreenToClient(i64 %struct_field.i70.i, <{ i32, i32 }>* nonnull %tmpcast.i.i) #5
  %struct_field3.i.i = load i64, i64* %cursor_pos.i.i, align 8
  %95 = trunc i64 %struct_field3.i.i to i32
  store i32 %95, i32* %struct_arg_8120.i, align 4
  %96 = lshr i64 %struct_field3.i.i, 32
  %97 = trunc i64 %96 to i32
  store i32 %97, i32* %struct_arg_9121.i, align 4
  %fun_call8.i.i = call i16 @GetKeyState(i32 1) #5
  %icmp_tmp.i73.i = icmp slt i16 %fun_call8.i.i, 0
  %struct_arrow.i.i75.i = load i1, i1* %.fca.0.gep20.i, align 4
  br i1 %icmp_tmp.i73.i, label %cand.rhs.i.i76.i, label %cand.rhs3.i.i79.i

cand.rhs.i.i76.i:                                 ; preds = %process_pending_messages.exit.i
  br i1 %struct_arrow.i.i75.i, label %update_game_button.exit.i82.i, label %then.i.i78.i

then.i.i78.i:                                     ; preds = %cand.rhs.i.i76.i
  store i1 true, i1* %.fca.1.gep21.i, align 1
  br label %update_game_button.exit.i82.i

cand.rhs3.i.i79.i:                                ; preds = %process_pending_messages.exit.i
  br i1 %struct_arrow.i.i75.i, label %then8.i.i81.i, label %update_game_button.exit.i82.i

then8.i.i81.i:                                    ; preds = %cand.rhs3.i.i79.i
  store i1 true, i1* %.fca.2.gep22.i, align 2
  br label %update_game_button.exit.i82.i

update_game_button.exit.i82.i:                    ; preds = %then8.i.i81.i, %cand.rhs3.i.i79.i, %then.i.i78.i, %cand.rhs.i.i76.i
  store i1 %icmp_tmp.i73.i, i1* %.fca.0.gep20.i, align 4
  %fun_call10.i.i = call i16 @GetKeyState(i32 2) #5
  %icmp_tmp12.i.i = icmp slt i16 %fun_call10.i.i, 0
  %struct_arrow.i11.i.i = load i1, i1* %.fca.0.gep23.i, align 1
  br i1 %icmp_tmp12.i.i, label %cand.rhs.i12.i.i, label %cand.rhs3.i15.i.i

cand.rhs.i12.i.i:                                 ; preds = %update_game_button.exit.i82.i
  br i1 %struct_arrow.i11.i.i, label %update_game_button.exit18.i.i, label %then.i14.i.i

then.i14.i.i:                                     ; preds = %cand.rhs.i12.i.i
  store i1 true, i1* %.fca.1.gep24.i, align 1
  br label %update_game_button.exit18.i.i

cand.rhs3.i15.i.i:                                ; preds = %update_game_button.exit.i82.i
  br i1 %struct_arrow.i11.i.i, label %then8.i17.i.i, label %update_game_button.exit18.i.i

then8.i17.i.i:                                    ; preds = %cand.rhs3.i15.i.i
  store i1 true, i1* %.fca.2.gep25.i, align 1
  br label %update_game_button.exit18.i.i

update_game_button.exit18.i.i:                    ; preds = %then8.i17.i.i, %cand.rhs3.i15.i.i, %then.i14.i.i, %cand.rhs.i12.i.i
  store i1 %icmp_tmp12.i.i, i1* %.fca.0.gep23.i, align 1
  %fun_call14.i.i = call i16 @GetKeyState(i32 4) #5
  %icmp_tmp16.i.i = icmp slt i16 %fun_call14.i.i, 0
  %struct_arrow.i2.i.i = load i1, i1* %.fca.0.gep26.i, align 2
  br i1 %icmp_tmp16.i.i, label %cand.rhs.i3.i.i, label %cand.rhs3.i6.i.i

cand.rhs.i3.i.i:                                  ; preds = %update_game_button.exit18.i.i
  br i1 %struct_arrow.i2.i.i, label %replay_input.exit.i, label %then.i5.i.i

then.i5.i.i:                                      ; preds = %cand.rhs.i3.i.i
  store i1 true, i1* %.fca.1.gep27.i, align 1
  br label %replay_input.exit.i

cand.rhs3.i6.i.i:                                 ; preds = %update_game_button.exit18.i.i
  br i1 %struct_arrow.i2.i.i, label %then8.i8.i.i, label %replay_input.exit.i

then8.i8.i.i:                                     ; preds = %cand.rhs3.i6.i.i
  store i1 true, i1* %.fca.2.gep28.i, align 2
  br label %replay_input.exit.i

replay_input.exit.i:                              ; preds = %then8.i8.i.i, %cand.rhs3.i6.i.i, %then.i5.i.i, %cand.rhs.i3.i.i
  store i1 %icmp_tmp16.i.i, i1* %.fca.0.gep26.i, align 2
  call void @llvm.lifetime.end(i64 8, i8* nonnull %66) #5
  %fun_call175.i = call i1 %game.sroa.4.0.ph.i(i8* %5, i8* nonnull %6, i8* nonnull %8) #5
  call void @llvm.lifetime.start(i64 4, i8* %67) #5
  call void @llvm.lifetime.start(i64 4, i8* %68) #5
  call void @llvm.lifetime.start(i64 8, i8* %69) #5
  %struct_arrow_load225.i131.i = load <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>*, <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>** %43, align 8
  %struct_field_ptr4.i132.i = getelementptr inbounds <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>, <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>* %struct_arrow_load225.i131.i, i64 0, i32 4
  %fun_ptr_load.i133.i = load i32 (i8*, i32*, i32*)*, i32 (i8*, i32*, i32*)** %struct_field_ptr4.i132.i, align 8
  %fun_call.i134.i = call i32 %fun_ptr_load.i133.i(i8* %hmpf.i, i32* nonnull %play_cursor.i.i, i32* nonnull %write_cursor.i.i) #5
  %play_cursor6.i.i = load i32, i32* %play_cursor.i.i, align 4
  %write_cursor7.i.i = load i32, i32* %write_cursor.i.i, align 4
  %icmp_tmp.i135.i = icmp eq i32 %play_cursor6.i.i, %write_cursor7.i.i
  br i1 %icmp_tmp.i135.i, label %update_next_sound_write_positon.exit.i, label %endif11.i.i

endif11.i.i:                                      ; preds = %replay_input.exit.i
  %icmp_tmp19.i.i = icmp eq i32 %play_cursor6.i.i, %sound_output.sroa.37.0.i
  br i1 %icmp_tmp19.i.i, label %then20.i.i, label %else.i146.i

then20.i.i:                                       ; preds = %endif11.i.i
  %fun_call22.i140.i = call i32 @QueryPerformanceCounter(i64* nonnull %time.i.i) #5
  %time23.i.i = load i64, i64* %time.i.i, align 8
  %sub_tmp.i141.i = sub i64 %time23.i.i, %sound_output.sroa.40.0.i
  %int_to_float_cast.i142.i = sitofp i64 %sub_tmp.i141.i to double
  %perf_count_freq.i143.i = load i64, i64* @perf_count_freq, align 8
  %int_to_float_cast26.i.i = sitofp i64 %perf_count_freq.i143.i to double
  %fdiv_tmp.i144.i = fdiv double %int_to_float_cast.i142.i, %int_to_float_cast26.i.i
  %fmul_tmp.i.i = fmul double %fdiv_tmp.i144.i, 4.800000e+04
  %int_cast.i.i = fptosi double %fmul_tmp.i.i to i32
  %mul_tmp.i.i = shl i32 %int_cast.i.i, 2
  %play_cursor33.i.i = load i32, i32* %play_cursor.i.i, align 4
  %add_tmp.i145.i = add i32 %mul_tmp.i.i, %play_cursor33.i.i
  %write_cursor35.i.i = load i32, i32* %write_cursor.i.i, align 4
  %add_tmp37.i.i = add i32 %mul_tmp.i.i, %write_cursor35.i.i
  %urem_tmp.i.i = urem i32 %add_tmp.i145.i, 192000
  store i32 %urem_tmp.i.i, i32* %play_cursor.i.i, align 4
  %urem_tmp44.i.i = urem i32 %add_tmp37.i.i, 192000
  store i32 %urem_tmp44.i.i, i32* %write_cursor.i.i, align 4
  br label %endif21.i.i

else.i146.i:                                      ; preds = %endif11.i.i
  call void @llvm.lifetime.start(i64 8, i8* %85) #5
  %fun_call.i34.i.i = call i32 @QueryPerformanceCounter(i64* nonnull %result.i33.i.i) #5
  %result1.i35.i.i = load i64, i64* %result.i33.i.i, align 8
  call void @llvm.lifetime.end(i64 8, i8* %85) #5
  %play_cursor51.pre.i.i = load i32, i32* %play_cursor.i.i, align 4
  %write_cursor68.pre.i.i = load i32, i32* %write_cursor.i.i, align 4
  br label %endif21.i.i

endif21.i.i:                                      ; preds = %else.i146.i, %then20.i.i
  %sound_output.sroa.37.1.i = phi i32 [ %sound_output.sroa.37.0.i, %then20.i.i ], [ %play_cursor6.i.i, %else.i146.i ]
  %sound_output.sroa.40.2.i = phi i64 [ %sound_output.sroa.40.0.i, %then20.i.i ], [ %result1.i35.i.i, %else.i146.i ]
  %write_cursor134.i.i = phi i32 [ %urem_tmp44.i.i, %then20.i.i ], [ %write_cursor68.pre.i.i, %else.i146.i ]
  %play_cursor156.i.i = phi i32 [ %urem_tmp.i.i, %then20.i.i ], [ %play_cursor51.pre.i.i, %else.i146.i ]
  %icmp_tmp54.i.i = icmp sle i32 %play_cursor156.i.i, %sound_output.sroa.37.1.i
  %icmp_tmp61.i.i = icmp slt i32 %sound_output.sroa.33.0.i, %sound_output.sroa.37.1.i
  %or.cond26.i.i = or i1 %icmp_tmp61.i.i, %icmp_tmp54.i.i
  %icmp_tmp65.i.i = icmp sgt i32 %sound_output.sroa.33.0.i, %play_cursor156.i.i
  %or.cond27.i.i = or i1 %icmp_tmp65.i.i, %or.cond26.i.i
  %position_to_write.0.i.i = select i1 %or.cond27.i.i, i32 %sound_output.sroa.33.0.i, i32 %write_cursor134.i.i
  %icmp_tmp72.i.i = icmp slt i32 %play_cursor156.i.i, %sound_output.sroa.37.1.i
  %or.cond28.i.i = and i1 %icmp_tmp61.i.i, %icmp_tmp65.i.i
  %position_to_write.0.write_cursor86.i.i = select i1 %or.cond28.i.i, i32 %position_to_write.0.i.i, i32 %write_cursor134.i.i
  %position_to_write.1.i.i = select i1 %icmp_tmp72.i.i, i32 %position_to_write.0.write_cursor86.i.i, i32 %position_to_write.0.i.i
  %icmp_tmp89.i.i = icmp sgt i32 %write_cursor134.i.i, %play_cursor156.i.i
  br i1 %icmp_tmp89.i.i, label %then90.i.i, label %endif91.i.i

then90.i.i:                                       ; preds = %endif21.i.i
  %icmp_tmp94.i.i = icmp sge i32 %position_to_write.1.i.i, %play_cursor156.i.i
  %icmp_tmp99.i.i = icmp slt i32 %position_to_write.1.i.i, %write_cursor134.i.i
  %or.cond29.i.i = and i1 %icmp_tmp94.i.i, %icmp_tmp99.i.i
  %position_to_write.2.i.i = select i1 %or.cond29.i.i, i32 %write_cursor134.i.i, i32 %position_to_write.1.i.i
  %sub_tmp106.i.i = sub i32 %write_cursor134.i.i, %play_cursor156.i.i
  br label %endif91.i.i

endif91.i.i:                                      ; preds = %then90.i.i, %endif21.i.i
  %delta_cursor.0.i.i = phi i32 [ %sub_tmp106.i.i, %then90.i.i ], [ undef, %endif21.i.i ]
  %position_to_write.3.i.i = phi i32 [ %position_to_write.2.i.i, %then90.i.i ], [ %position_to_write.1.i.i, %endif21.i.i ]
  %icmp_tmp109.i.i = icmp slt i32 %write_cursor134.i.i, %play_cursor156.i.i
  br i1 %icmp_tmp109.i.i, label %then110.i.i, label %endif111.i.i

then110.i.i:                                      ; preds = %endif91.i.i
  %icmp_tmp114.i.i = icmp sge i32 %position_to_write.3.i.i, %play_cursor156.i.i
  %icmp_tmp119.i.i = icmp slt i32 %position_to_write.3.i.i, %write_cursor134.i.i
  %or.cond30.i.i = or i1 %icmp_tmp114.i.i, %icmp_tmp119.i.i
  %position_to_write.4.i.i = select i1 %or.cond30.i.i, i32 %write_cursor134.i.i, i32 %position_to_write.3.i.i
  %sub_tmp127.i.i = add i32 %write_cursor134.i.i, 192000
  %add_tmp129.i.i = sub i32 %sub_tmp127.i.i, %play_cursor156.i.i
  br label %endif111.i.i

endif111.i.i:                                     ; preds = %then110.i.i, %endif91.i.i
  %delta_cursor.1.i.i = phi i32 [ %add_tmp129.i.i, %then110.i.i ], [ %delta_cursor.0.i.i, %endif91.i.i ]
  %position_to_write.5.i.i = phi i32 [ %position_to_write.4.i.i, %then110.i.i ], [ %position_to_write.3.i.i, %endif91.i.i ]
  %add_tmp13637.i.i = add i32 %delta_cursor.1.i.i, %write_cursor134.i.i
  %icmp_tmp13738.i.i = icmp sgt i32 %write_cursor134.i.i, %add_tmp13637.i.i
  br i1 %icmp_tmp13738.i.i, label %while_end.i.i, label %while.i148.i.preheader

while.i148.i.preheader:                           ; preds = %endif111.i.i
  br label %while.i148.i

while.i148.i:                                     ; preds = %while.i148.i.preheader, %while.i148.i
  %target_cursor.039.i.i = phi i32 [ %add_tmp141.i.i, %while.i148.i ], [ %write_cursor134.i.i, %while.i148.i.preheader ]
  %add_tmp141.i.i = add i32 %target_cursor.039.i.i, 3200
  %icmp_tmp137.i.i = icmp sgt i32 %add_tmp141.i.i, %add_tmp13637.i.i
  br i1 %icmp_tmp137.i.i, label %while_end.i.i.loopexit, label %while.i148.i

while_end.i.i.loopexit:                           ; preds = %while.i148.i
  %add_tmp141.i.i.lcssa = phi i32 [ %add_tmp141.i.i, %while.i148.i ]
  br label %while_end.i.i

while_end.i.i:                                    ; preds = %while_end.i.i.loopexit, %endif111.i.i
  %target_cursor.0.lcssa.i.i = phi i32 [ %write_cursor134.i.i, %endif111.i.i ], [ %add_tmp141.i.i.lcssa, %while_end.i.i.loopexit ]
  %add_tmp144.i.i = add i32 %target_cursor.0.lcssa.i.i, 1600
  %icmp_tmp148.i.i = icmp sgt i32 %add_tmp144.i.i, 192000
  %sub_tmp154.i.i = select i1 %icmp_tmp148.i.i, i32 192000, i32 0
  %sub_tmp154.add_tmp144.i.i = sub i32 %add_tmp144.i.i, %sub_tmp154.i.i
  %icmp_tmp157.i.i = icmp sgt i32 %sub_tmp154.add_tmp144.i.i, %play_cursor156.i.i
  %icmp_tmp162.i.i = icmp sgt i32 %sub_tmp154.add_tmp144.i.i, %position_to_write.5.i.i
  %or.cond31.i.i = and i1 %icmp_tmp157.i.i, %icmp_tmp162.i.i
  %icmp_tmp167.i.i = icmp sgt i32 %position_to_write.5.i.i, %play_cursor156.i.i
  %or.cond32.i.i = and i1 %icmp_tmp167.i.i, %or.cond31.i.i
  %sub_tmp173.i.i = sub i32 %sub_tmp154.add_tmp144.i.i, %position_to_write.5.i.i
  %bytes_to_write.0.i.i = select i1 %or.cond32.i.i, i32 %sub_tmp173.i.i, i32 0
  %icmp_tmp176.i.i = icmp slt i32 %sub_tmp154.add_tmp144.i.i, %play_cursor156.i.i
  br i1 %icmp_tmp176.i.i, label %then177.i.i, label %update_next_sound_write_positon.exit.i

then177.i.i:                                      ; preds = %while_end.i.i
  %sub_tmp187.i.i = sub i32 192000, %position_to_write.5.i.i
  %add_tmp190.i.i = add i32 %sub_tmp187.i.i, %sub_tmp154.add_tmp144.i.i
  %bytes_to_write.1.i.i = select i1 %icmp_tmp167.i.i, i32 %add_tmp190.i.i, i32 %bytes_to_write.0.i.i
  %sub_tmp198.bytes_to_write.1.i.i = select i1 %icmp_tmp162.i.i, i32 %sub_tmp173.i.i, i32 %bytes_to_write.1.i.i
  br label %update_next_sound_write_positon.exit.i

update_next_sound_write_positon.exit.i:           ; preds = %then177.i.i, %while_end.i.i, %replay_input.exit.i
  %sound_output.sroa.37.2.i = phi i32 [ %sound_output.sroa.37.0.i, %replay_input.exit.i ], [ %sound_output.sroa.37.1.i, %then177.i.i ], [ %sound_output.sroa.37.1.i, %while_end.i.i ]
  %sound_output.sroa.40.3.i = phi i64 [ %sound_output.sroa.40.0.i, %replay_input.exit.i ], [ %sound_output.sroa.40.2.i, %then177.i.i ], [ %sound_output.sroa.40.2.i, %while_end.i.i ]
  %sound_output.sroa.43.1.i = phi i32 [ %sound_output.sroa.43.0.i, %replay_input.exit.i ], [ %position_to_write.5.i.i, %then177.i.i ], [ %position_to_write.5.i.i, %while_end.i.i ]
  %sound_output.sroa.45.1.i = phi i32 [ %sound_output.sroa.45.0.i, %replay_input.exit.i ], [ %sub_tmp198.bytes_to_write.1.i.i, %then177.i.i ], [ %bytes_to_write.0.i.i, %while_end.i.i ]
  call void @llvm.lifetime.end(i64 4, i8* %67) #5
  call void @llvm.lifetime.end(i64 4, i8* %68) #5
  call void @llvm.lifetime.end(i64 8, i8* %69) #5
  %div_tmp181.i = sdiv i32 %sound_output.sroa.45.1.i, 4
  store i32 %div_tmp181.i, i32* %struct_arg_1126.i, align 8
  call void %game.sroa.7.0.ph.i(i8* %5, i8* %7) #5
  call void @llvm.lifetime.start(i64 8, i8* %70) #5
  call void @llvm.lifetime.start(i64 4, i8* %71) #5
  call void @llvm.lifetime.start(i64 8, i8* %72) #5
  call void @llvm.lifetime.start(i64 4, i8* %73) #5
  %struct_arrow_load57.i.i = load <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>*, <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>** %43, align 8
  %struct_field_ptr7.i.i = getelementptr inbounds <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>, <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>* %struct_arrow_load57.i.i, i64 0, i32 11
  %fun_ptr_load.i159.i = load i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)** %struct_field_ptr7.i.i, align 8
  %fun_call.i160.i = call i32 %fun_ptr_load.i159.i(i8* %hmpf.i, i32 %sound_output.sroa.43.1.i, i32 %sound_output.sroa.45.1.i, i8** nonnull %region1.i149.i, i32* nonnull %region1_size.i150.i, i8** nonnull %region2.i151.i, i32* nonnull %region2_size.i152.i, i32 0) #5
  %region1_size12.i.i = load i32, i32* %region1_size.i150.i, align 4
  %div_tmp.i161.i = sdiv i32 %region1_size12.i.i, 4
  %region2_size15.i.i = load i32, i32* %region2_size.i152.i, align 4
  %div_tmp18.i.i = sdiv i32 %region2_size15.i.i, 4
  %icmp_tmp.i162.i = icmp eq i32 %fun_call.i160.i, 0
  br i1 %icmp_tmp.i162.i, label %then.i164.i, label %fill_sound_buffer.exit.i

then.i164.i:                                      ; preds = %update_next_sound_write_positon.exit.i
  %struct_arrow21.i.i = load i16*, i16** %struct_arg_0125.i, align 8
  %icmp_tmp2519.i.i = icmp sgt i32 %region1_size12.i.i, 3
  br i1 %icmp_tmp2519.i.i, label %for.preheader.i165.i, label %end_for.i.i

for.preheader.i165.i:                             ; preds = %then.i164.i
  %region1228.i.i = load i16*, i16** %74, align 8
  %98 = add nsw i32 %div_tmp.i161.i, -1
  %99 = zext i32 %98 to i64
  %xtraiter = and i32 %div_tmp.i161.i, 3
  %100 = icmp ult i32 %98, 3
  br i1 %100, label %end_for.loopexit.i.i.unr-lcssa, label %for.preheader.i165.i.new

for.preheader.i165.i.new:                         ; preds = %for.preheader.i165.i
  %unroll_iter = sub nsw i32 %div_tmp.i161.i, %xtraiter
  br label %for.i.i

for.i.i:                                          ; preds = %for.i.i, %for.preheader.i165.i.new
  %source_sample.022.i.i = phi i16* [ %struct_arrow21.i.i, %for.preheader.i165.i.new ], [ %ptr_post_inc31.i.i.3, %for.i.i ]
  %dest_sample.020.i.i = phi i16* [ %region1228.i.i, %for.preheader.i165.i.new ], [ %ptr_post_inc29.i.i.3, %for.i.i ]
  %niter = phi i32 [ %unroll_iter, %for.preheader.i165.i.new ], [ %niter.nsub.3, %for.i.i ]
  %ptr_post_inc.i.i = getelementptr i16, i16* %dest_sample.020.i.i, i64 1
  %ptr_post_inc27.i.i = getelementptr i16, i16* %source_sample.022.i.i, i64 1
  %deref.i.i = load i16, i16* %source_sample.022.i.i, align 2
  store i16 %deref.i.i, i16* %dest_sample.020.i.i, align 2
  %ptr_post_inc29.i.i = getelementptr i16, i16* %dest_sample.020.i.i, i64 2
  %ptr_post_inc31.i.i = getelementptr i16, i16* %source_sample.022.i.i, i64 2
  %deref32.i.i = load i16, i16* %ptr_post_inc27.i.i, align 2
  store i16 %deref32.i.i, i16* %ptr_post_inc.i.i, align 2
  %ptr_post_inc.i.i.1 = getelementptr i16, i16* %dest_sample.020.i.i, i64 3
  %ptr_post_inc27.i.i.1 = getelementptr i16, i16* %source_sample.022.i.i, i64 3
  %deref.i.i.1 = load i16, i16* %ptr_post_inc31.i.i, align 2
  store i16 %deref.i.i.1, i16* %ptr_post_inc29.i.i, align 2
  %ptr_post_inc29.i.i.1 = getelementptr i16, i16* %dest_sample.020.i.i, i64 4
  %ptr_post_inc31.i.i.1 = getelementptr i16, i16* %source_sample.022.i.i, i64 4
  %deref32.i.i.1 = load i16, i16* %ptr_post_inc27.i.i.1, align 2
  store i16 %deref32.i.i.1, i16* %ptr_post_inc.i.i.1, align 2
  %ptr_post_inc.i.i.2 = getelementptr i16, i16* %dest_sample.020.i.i, i64 5
  %ptr_post_inc27.i.i.2 = getelementptr i16, i16* %source_sample.022.i.i, i64 5
  %deref.i.i.2 = load i16, i16* %ptr_post_inc31.i.i.1, align 2
  store i16 %deref.i.i.2, i16* %ptr_post_inc29.i.i.1, align 2
  %ptr_post_inc29.i.i.2 = getelementptr i16, i16* %dest_sample.020.i.i, i64 6
  %ptr_post_inc31.i.i.2 = getelementptr i16, i16* %source_sample.022.i.i, i64 6
  %deref32.i.i.2 = load i16, i16* %ptr_post_inc27.i.i.2, align 2
  store i16 %deref32.i.i.2, i16* %ptr_post_inc.i.i.2, align 2
  %ptr_post_inc.i.i.3 = getelementptr i16, i16* %dest_sample.020.i.i, i64 7
  %ptr_post_inc27.i.i.3 = getelementptr i16, i16* %source_sample.022.i.i, i64 7
  %deref.i.i.3 = load i16, i16* %ptr_post_inc31.i.i.2, align 2
  store i16 %deref.i.i.3, i16* %ptr_post_inc29.i.i.2, align 2
  %ptr_post_inc29.i.i.3 = getelementptr i16, i16* %dest_sample.020.i.i, i64 8
  %ptr_post_inc31.i.i.3 = getelementptr i16, i16* %source_sample.022.i.i, i64 8
  %deref32.i.i.3 = load i16, i16* %ptr_post_inc27.i.i.3, align 2
  store i16 %deref32.i.i.3, i16* %ptr_post_inc.i.i.3, align 2
  %niter.nsub.3 = add i32 %niter, -4
  %niter.ncmp.3 = icmp eq i32 %niter.nsub.3, 0
  br i1 %niter.ncmp.3, label %end_for.loopexit.i.i.unr-lcssa.loopexit, label %for.i.i

end_for.loopexit.i.i.unr-lcssa.loopexit:          ; preds = %for.i.i
  %ptr_post_inc31.i.i.3.lcssa = phi i16* [ %ptr_post_inc31.i.i.3, %for.i.i ]
  %ptr_post_inc29.i.i.3.lcssa = phi i16* [ %ptr_post_inc29.i.i.3, %for.i.i ]
  br label %end_for.loopexit.i.i.unr-lcssa

end_for.loopexit.i.i.unr-lcssa:                   ; preds = %end_for.loopexit.i.i.unr-lcssa.loopexit, %for.preheader.i165.i
  %source_sample.022.i.i.unr = phi i16* [ %struct_arrow21.i.i, %for.preheader.i165.i ], [ %ptr_post_inc31.i.i.3.lcssa, %end_for.loopexit.i.i.unr-lcssa.loopexit ]
  %dest_sample.020.i.i.unr = phi i16* [ %region1228.i.i, %for.preheader.i165.i ], [ %ptr_post_inc29.i.i.3.lcssa, %end_for.loopexit.i.i.unr-lcssa.loopexit ]
  %lcmp.mod = icmp eq i32 %xtraiter, 0
  br i1 %lcmp.mod, label %end_for.loopexit.i.i, label %for.i.i.epil.preheader

for.i.i.epil.preheader:                           ; preds = %end_for.loopexit.i.i.unr-lcssa
  br label %for.i.i.epil

for.i.i.epil:                                     ; preds = %for.i.i.epil, %for.i.i.epil.preheader
  %source_sample.022.i.i.epil = phi i16* [ %ptr_post_inc31.i.i.epil, %for.i.i.epil ], [ %source_sample.022.i.i.unr, %for.i.i.epil.preheader ]
  %dest_sample.020.i.i.epil = phi i16* [ %ptr_post_inc29.i.i.epil, %for.i.i.epil ], [ %dest_sample.020.i.i.unr, %for.i.i.epil.preheader ]
  %epil.iter = phi i32 [ %epil.iter.sub, %for.i.i.epil ], [ %xtraiter, %for.i.i.epil.preheader ]
  %ptr_post_inc.i.i.epil = getelementptr i16, i16* %dest_sample.020.i.i.epil, i64 1
  %ptr_post_inc27.i.i.epil = getelementptr i16, i16* %source_sample.022.i.i.epil, i64 1
  %deref.i.i.epil = load i16, i16* %source_sample.022.i.i.epil, align 2
  store i16 %deref.i.i.epil, i16* %dest_sample.020.i.i.epil, align 2
  %ptr_post_inc29.i.i.epil = getelementptr i16, i16* %dest_sample.020.i.i.epil, i64 2
  %ptr_post_inc31.i.i.epil = getelementptr i16, i16* %source_sample.022.i.i.epil, i64 2
  %deref32.i.i.epil = load i16, i16* %ptr_post_inc27.i.i.epil, align 2
  store i16 %deref32.i.i.epil, i16* %ptr_post_inc.i.i.epil, align 2
  %epil.iter.sub = add i32 %epil.iter, -1
  %epil.iter.cmp = icmp eq i32 %epil.iter.sub, 0
  br i1 %epil.iter.cmp, label %end_for.loopexit.i.i.epilog-lcssa, label %for.i.i.epil, !llvm.loop !0

end_for.loopexit.i.i.epilog-lcssa:                ; preds = %for.i.i.epil
  br label %end_for.loopexit.i.i

end_for.loopexit.i.i:                             ; preds = %end_for.loopexit.i.i.unr-lcssa, %end_for.loopexit.i.i.epilog-lcssa
  %101 = shl nuw nsw i64 %99, 1
  %102 = add nuw nsw i64 %101, 2
  %scevgep.i.i = getelementptr i16, i16* %struct_arrow21.i.i, i64 %102
  br label %end_for.i.i

end_for.i.i:                                      ; preds = %end_for.loopexit.i.i, %then.i164.i
  %source_sample.0.lcssa.i.i = phi i16* [ %struct_arrow21.i.i, %then.i164.i ], [ %scevgep.i.i, %end_for.loopexit.i.i ]
  %region2339.i.i = load i16*, i16** %75, align 8
  %icmp_tmp4215.i.i = icmp sgt i32 %region2_size15.i.i, 3
  br i1 %icmp_tmp4215.i.i, label %for36.i.i.preheader, label %elif_0.i167.i

for36.i.i.preheader:                              ; preds = %end_for.i.i
  %103 = add nsw i32 %div_tmp18.i.i, -1
  %xtraiter8 = and i32 %div_tmp18.i.i, 3
  %104 = icmp ult i32 %103, 3
  br i1 %104, label %then72.i.i.unr-lcssa, label %for36.i.i.preheader.new

for36.i.i.preheader.new:                          ; preds = %for36.i.i.preheader
  %unroll_iter11 = sub nsw i32 %div_tmp18.i.i, %xtraiter8
  br label %for36.i.i

for36.i.i:                                        ; preds = %for36.i.i, %for36.i.i.preheader.new
  %source_sample.118.i.i = phi i16* [ %source_sample.0.lcssa.i.i, %for36.i.i.preheader.new ], [ %ptr_post_inc51.i.i.3, %for36.i.i ]
  %dest_sample.116.i.i = phi i16* [ %region2339.i.i, %for36.i.i.preheader.new ], [ %ptr_post_inc49.i.i.3, %for36.i.i ]
  %niter12 = phi i32 [ %unroll_iter11, %for36.i.i.preheader.new ], [ %niter12.nsub.3, %for36.i.i ]
  %ptr_post_inc44.i.i = getelementptr i16, i16* %dest_sample.116.i.i, i64 1
  %ptr_post_inc46.i.i = getelementptr i16, i16* %source_sample.118.i.i, i64 1
  %deref47.i.i = load i16, i16* %source_sample.118.i.i, align 2
  store i16 %deref47.i.i, i16* %dest_sample.116.i.i, align 2
  %ptr_post_inc49.i.i = getelementptr i16, i16* %dest_sample.116.i.i, i64 2
  %ptr_post_inc51.i.i = getelementptr i16, i16* %source_sample.118.i.i, i64 2
  %deref52.i.i = load i16, i16* %ptr_post_inc46.i.i, align 2
  store i16 %deref52.i.i, i16* %ptr_post_inc44.i.i, align 2
  %ptr_post_inc44.i.i.1 = getelementptr i16, i16* %dest_sample.116.i.i, i64 3
  %ptr_post_inc46.i.i.1 = getelementptr i16, i16* %source_sample.118.i.i, i64 3
  %deref47.i.i.1 = load i16, i16* %ptr_post_inc51.i.i, align 2
  store i16 %deref47.i.i.1, i16* %ptr_post_inc49.i.i, align 2
  %ptr_post_inc49.i.i.1 = getelementptr i16, i16* %dest_sample.116.i.i, i64 4
  %ptr_post_inc51.i.i.1 = getelementptr i16, i16* %source_sample.118.i.i, i64 4
  %deref52.i.i.1 = load i16, i16* %ptr_post_inc46.i.i.1, align 2
  store i16 %deref52.i.i.1, i16* %ptr_post_inc44.i.i.1, align 2
  %ptr_post_inc44.i.i.2 = getelementptr i16, i16* %dest_sample.116.i.i, i64 5
  %ptr_post_inc46.i.i.2 = getelementptr i16, i16* %source_sample.118.i.i, i64 5
  %deref47.i.i.2 = load i16, i16* %ptr_post_inc51.i.i.1, align 2
  store i16 %deref47.i.i.2, i16* %ptr_post_inc49.i.i.1, align 2
  %ptr_post_inc49.i.i.2 = getelementptr i16, i16* %dest_sample.116.i.i, i64 6
  %ptr_post_inc51.i.i.2 = getelementptr i16, i16* %source_sample.118.i.i, i64 6
  %deref52.i.i.2 = load i16, i16* %ptr_post_inc46.i.i.2, align 2
  store i16 %deref52.i.i.2, i16* %ptr_post_inc44.i.i.2, align 2
  %ptr_post_inc44.i.i.3 = getelementptr i16, i16* %dest_sample.116.i.i, i64 7
  %ptr_post_inc46.i.i.3 = getelementptr i16, i16* %source_sample.118.i.i, i64 7
  %deref47.i.i.3 = load i16, i16* %ptr_post_inc51.i.i.2, align 2
  store i16 %deref47.i.i.3, i16* %ptr_post_inc49.i.i.2, align 2
  %ptr_post_inc49.i.i.3 = getelementptr i16, i16* %dest_sample.116.i.i, i64 8
  %ptr_post_inc51.i.i.3 = getelementptr i16, i16* %source_sample.118.i.i, i64 8
  %deref52.i.i.3 = load i16, i16* %ptr_post_inc46.i.i.3, align 2
  store i16 %deref52.i.i.3, i16* %ptr_post_inc44.i.i.3, align 2
  %niter12.nsub.3 = add i32 %niter12, -4
  %niter12.ncmp.3 = icmp eq i32 %niter12.nsub.3, 0
  br i1 %niter12.ncmp.3, label %then72.i.i.unr-lcssa.loopexit, label %for36.i.i

then72.i.i.unr-lcssa.loopexit:                    ; preds = %for36.i.i
  %ptr_post_inc51.i.i.3.lcssa = phi i16* [ %ptr_post_inc51.i.i.3, %for36.i.i ]
  %ptr_post_inc49.i.i.3.lcssa = phi i16* [ %ptr_post_inc49.i.i.3, %for36.i.i ]
  br label %then72.i.i.unr-lcssa

then72.i.i.unr-lcssa:                             ; preds = %then72.i.i.unr-lcssa.loopexit, %for36.i.i.preheader
  %source_sample.118.i.i.unr = phi i16* [ %source_sample.0.lcssa.i.i, %for36.i.i.preheader ], [ %ptr_post_inc51.i.i.3.lcssa, %then72.i.i.unr-lcssa.loopexit ]
  %dest_sample.116.i.i.unr = phi i16* [ %region2339.i.i, %for36.i.i.preheader ], [ %ptr_post_inc49.i.i.3.lcssa, %then72.i.i.unr-lcssa.loopexit ]
  %lcmp.mod10 = icmp eq i32 %xtraiter8, 0
  br i1 %lcmp.mod10, label %then72.i.i, label %for36.i.i.epil.preheader

for36.i.i.epil.preheader:                         ; preds = %then72.i.i.unr-lcssa
  br label %for36.i.i.epil

for36.i.i.epil:                                   ; preds = %for36.i.i.epil, %for36.i.i.epil.preheader
  %source_sample.118.i.i.epil = phi i16* [ %ptr_post_inc51.i.i.epil, %for36.i.i.epil ], [ %source_sample.118.i.i.unr, %for36.i.i.epil.preheader ]
  %dest_sample.116.i.i.epil = phi i16* [ %ptr_post_inc49.i.i.epil, %for36.i.i.epil ], [ %dest_sample.116.i.i.unr, %for36.i.i.epil.preheader ]
  %epil.iter9 = phi i32 [ %epil.iter9.sub, %for36.i.i.epil ], [ %xtraiter8, %for36.i.i.epil.preheader ]
  %ptr_post_inc44.i.i.epil = getelementptr i16, i16* %dest_sample.116.i.i.epil, i64 1
  %ptr_post_inc46.i.i.epil = getelementptr i16, i16* %source_sample.118.i.i.epil, i64 1
  %deref47.i.i.epil = load i16, i16* %source_sample.118.i.i.epil, align 2
  store i16 %deref47.i.i.epil, i16* %dest_sample.116.i.i.epil, align 2
  %ptr_post_inc49.i.i.epil = getelementptr i16, i16* %dest_sample.116.i.i.epil, i64 2
  %ptr_post_inc51.i.i.epil = getelementptr i16, i16* %source_sample.118.i.i.epil, i64 2
  %deref52.i.i.epil = load i16, i16* %ptr_post_inc46.i.i.epil, align 2
  store i16 %deref52.i.i.epil, i16* %ptr_post_inc44.i.i.epil, align 2
  %epil.iter9.sub = add i32 %epil.iter9, -1
  %epil.iter9.cmp = icmp eq i32 %epil.iter9.sub, 0
  br i1 %epil.iter9.cmp, label %then72.i.i.epilog-lcssa, label %for36.i.i.epil, !llvm.loop !2

then72.i.i.epilog-lcssa:                          ; preds = %for36.i.i.epil
  br label %then72.i.i

then72.i.i:                                       ; preds = %then72.i.i.unr-lcssa, %then72.i.i.epilog-lcssa
  %region267.pre.i.i = load i8*, i8** %region2.i151.i, align 8
  %struct_arrow_load5910.i.i = load <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>*, <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>** %43, align 8
  %struct_field_ptr61.i.i = getelementptr inbounds <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>, <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>* %struct_arrow_load5910.i.i, i64 0, i32 19
  %fun_ptr_load62.i.i = load i32 (i8*, i8*, i32, i8*, i32)*, i32 (i8*, i8*, i32, i8*, i32)** %struct_field_ptr61.i.i, align 8
  %region165.i.i = load i8*, i8** %region1.i149.i, align 8
  %region1_size66.i.i = load i32, i32* %region1_size.i150.i, align 4
  %region2_size68.i.i = load i32, i32* %region2_size.i152.i, align 4
  %fun_call69.i.i = call i32 %fun_ptr_load62.i.i(i8* %hmpf.i, i8* %region165.i.i, i32 %region1_size66.i.i, i8* %region267.pre.i.i, i32 %region2_size68.i.i) #5
  %region27413.i.i = load i32*, i32** %77, align 8
  %105 = sext i32 %div_tmp18.i.i to i64
  %ptr_add.i.i = getelementptr i32, i32* %region27413.i.i, i64 %105
  %ptr_to_int.i.i = ptrtoint i32* %ptr_add.i.i to i64
  %sub.i.i = sub i64 %ptr_to_int.i.i, %sound_output.sroa.29.0.i
  %int_trunc.i166.i = trunc i64 %sub.i.i to i32
  br label %fill_sound_buffer.exit.i

elif_0.i167.i:                                    ; preds = %end_for.i.i
  %106 = bitcast i16* %region2339.i.i to i8*
  %struct_arrow_load5910.c.i.i = load <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>*, <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>** %43, align 8
  %struct_field_ptr61.c.i.i = getelementptr inbounds <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>, <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>* %struct_arrow_load5910.c.i.i, i64 0, i32 19
  %fun_ptr_load62.c.i.i = load i32 (i8*, i8*, i32, i8*, i32)*, i32 (i8*, i8*, i32, i8*, i32)** %struct_field_ptr61.c.i.i, align 8
  %region165.c.i.i = load i8*, i8** %region1.i149.i, align 8
  %region1_size66.c.i.i = load i32, i32* %region1_size.i150.i, align 4
  %region2_size68.c.i.i = load i32, i32* %region2_size.i152.i, align 4
  %fun_call69.c.i.i = call i32 %fun_ptr_load62.c.i.i(i8* %hmpf.i, i8* %region165.c.i.i, i32 %region1_size66.c.i.i, i8* %106, i32 %region2_size68.c.i.i) #5
  br i1 %icmp_tmp2519.i.i, label %elif_0_then.i168.i, label %fill_sound_buffer.exit.i

elif_0_then.i168.i:                               ; preds = %elif_0.i167.i
  %region18511.i.i = load i32*, i32** %76, align 8
  %107 = sext i32 %div_tmp.i161.i to i64
  %ptr_add88.i.i = getelementptr i32, i32* %region18511.i.i, i64 %107
  %ptr_to_int95.i.i = ptrtoint i32* %ptr_add88.i.i to i64
  %sub97.i.i = sub i64 %ptr_to_int95.i.i, %sound_output.sroa.29.0.i
  %int_trunc99.i.i = trunc i64 %sub97.i.i to i32
  br label %fill_sound_buffer.exit.i

fill_sound_buffer.exit.i:                         ; preds = %elif_0_then.i168.i, %elif_0.i167.i, %then72.i.i, %update_next_sound_write_positon.exit.i
  %sound_output.sroa.33.3.i = phi i32 [ %int_trunc.i166.i, %then72.i.i ], [ %int_trunc99.i.i, %elif_0_then.i168.i ], [ %sound_output.sroa.33.0.i, %elif_0.i167.i ], [ %sound_output.sroa.33.0.i, %update_next_sound_write_positon.exit.i ]
  call void @llvm.lifetime.end(i64 8, i8* %70) #5
  call void @llvm.lifetime.end(i64 4, i8* nonnull %71) #5
  call void @llvm.lifetime.end(i64 8, i8* %72) #5
  call void @llvm.lifetime.end(i64 4, i8* nonnull %73) #5
  br i1 %fun_call175.i, label %endif188.i, label %then187.i

then187.i:                                        ; preds = %fill_sound_buffer.exit.i
  call void @llvm.lifetime.start(i64 8, i8* %80) #5
  call void @llvm.lifetime.start(i64 4, i8* %81) #5
  call void @llvm.lifetime.start(i64 8, i8* %82) #5
  call void @llvm.lifetime.start(i64 4, i8* %83) #5
  %struct_arrow_load22.i176.i = load <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>*, <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>** %43, align 8
  %struct_field_ptr4.i177.i = getelementptr inbounds <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>, <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>* %struct_arrow_load22.i176.i, i64 0, i32 11
  %fun_ptr_load.i178.i = load i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)** %struct_field_ptr4.i177.i, align 8
  %fun_call.i181.i = call i32 %fun_ptr_load.i178.i(i8* nonnull %hmpf.i, i32 0, i32 192000, i8** nonnull %region1.i170.i, i32* nonnull %region1_size.i171.i, i8** nonnull %region2.i172.i, i32* nonnull %region2_size.i173.i, i32 0) #5
  %region183.i182.i = load i32*, i32** %84, align 8
  %region1_size9.i183.i = load i32, i32* %region1_size.i171.i, align 4
  %icmp_tmp6.i187.i = icmp sgt i32 %region1_size9.i183.i, 3
  br i1 %icmp_tmp6.i187.i, label %for.preheader.i193.i, label %vars.end_for_crit_edge.i188.i

vars.end_for_crit_edge.i188.i:                    ; preds = %then187.i
  %108 = ptrtoint i32* %region183.i182.i to i64
  %109 = bitcast i32* %region183.i182.i to i8*
  br label %clear_sound_buffer.exit206.i

for.preheader.i193.i:                             ; preds = %then187.i
  %div_tmp.i186308.i = lshr i32 %region1_size9.i183.i, 2
  %region1839.i189.i = bitcast i32* %region183.i182.i to i8*
  %110 = add nsw i32 %div_tmp.i186308.i, -1
  %111 = zext i32 %110 to i64
  %112 = shl nuw nsw i64 %111, 2
  %113 = add nuw nsw i64 %112, 4
  call void @llvm.memset.p0i8.i64(i8* %region1839.i189.i, i8 0, i64 %113, i32 4, i1 false) #5
  %region1154.pre.i191.i = load i64, i64* %.phi.trans.insert.i190.i, align 8
  %114 = inttoptr i64 %region1154.pre.i191.i to i8*
  %region1_size27.pre.i192.i = load i32, i32* %region1_size.i171.i, align 4
  br label %clear_sound_buffer.exit206.i

clear_sound_buffer.exit206.i:                     ; preds = %for.preheader.i193.i, %vars.end_for_crit_edge.i188.i
  %region1_size27.i194.i = phi i32 [ %region1_size27.pre.i192.i, %for.preheader.i193.i ], [ %region1_size9.i183.i, %vars.end_for_crit_edge.i188.i ]
  %region126.i195.i = phi i8* [ %114, %for.preheader.i193.i ], [ %109, %vars.end_for_crit_edge.i188.i ]
  %region1154.i196.i = phi i64 [ %region1154.pre.i191.i, %for.preheader.i193.i ], [ %108, %vars.end_for_crit_edge.i188.i ]
  %struct_arrow_load205.i199.i = load <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>*, <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>** %43, align 8
  %struct_field_ptr22.i200.i = getelementptr inbounds <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>, <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>* %struct_arrow_load205.i199.i, i64 0, i32 19
  %fun_ptr_load23.i201.i = load i32 (i8*, i8*, i32, i8*, i32)*, i32 (i8*, i8*, i32, i8*, i32)** %struct_field_ptr22.i200.i, align 8
  %region228.i202.i = load i8*, i8** %region2.i172.i, align 8
  %region2_size29.i203.i = load i32, i32* %region2_size.i173.i, align 4
  %fun_call30.i204.i = call i32 %fun_ptr_load23.i201.i(i8* nonnull %hmpf.i, i8* %region126.i195.i, i32 %region1_size27.i194.i, i8* %region228.i202.i, i32 %region2_size29.i203.i) #5
  call void @llvm.lifetime.end(i64 8, i8* nonnull %80) #5
  call void @llvm.lifetime.end(i64 4, i8* nonnull %81) #5
  call void @llvm.lifetime.end(i64 8, i8* %82) #5
  call void @llvm.lifetime.end(i64 4, i8* %83) #5
  %struct_arrow_load19344.i = load <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>*, <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>** %43, align 8
  %struct_field_ptr195.i = getelementptr inbounds <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>, <{ i8*, i8*, i8*, i8*, i32 (i8*, i32*, i32*)*, i8*, i8*, i8*, i8*, i8*, i8*, i32 (i8*, i32, i32, i8**, i32*, i8**, i32*, i32)*, i32 (i8*, i32, i32, i32)*, i8*, i32 (i8*, i8*)*, i8*, i8*, i8*, i32 ()*, i32 (i8*, i8*, i32, i8*, i32)*, i8* }>* %struct_arrow_load19344.i, i64 0, i32 18
  %fun_ptr_load196.i = load i32 ()*, i32 ()** %struct_field_ptr195.i, align 8
  %fun_call197.i = call i32 %fun_ptr_load196.i() #5
  br label %endif188.i

endif188.i:                                       ; preds = %clear_sound_buffer.exit206.i, %fill_sound_buffer.exit.i
  %sound_output.sroa.29.1.i = phi i64 [ %sound_output.sroa.29.0.i, %fill_sound_buffer.exit.i ], [ %region1154.i196.i, %clear_sound_buffer.exit206.i ]
  %struct_field.i207.i = load i64, i64* @window.1, align 8
  %struct_arrow.i208.i = load i32, i32* getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i64 0, i32 2), align 4
  %struct_arrow2.i209.i = load i32, i32* getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i64 0, i32 3), align 8
  %struct_arrow8.i.i = load i8*, i8** getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i64 0, i32 1), align 8
  %fun_call.i210.i = call i32 @StretchDIBits(i64 %struct_field.i207.i, i32 0, i32 0, i32 %struct_arrow.i208.i, i32 %struct_arrow2.i209.i, i32 0, i32 0, i32 %struct_arrow.i208.i, i32 %struct_arrow2.i209.i, i8* %struct_arrow8.i.i, <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>* getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i64 0, i32 0), i32 0, i32 13369376) #5
  %115 = call { i32, i32 } asm sideeffect "rdtsc", "={ax},={dx},~{dirflag},~{fpsr},~{flags}"() #5
  %116 = extractvalue { i32, i32 } %115, 0
  %117 = extractvalue { i32, i32 } %115, 1
  %118 = zext i32 %117 to i64
  %119 = shl nuw i64 %118, 32
  %120 = zext i32 %116 to i64
  %121 = or i64 %119, %120
  call void @llvm.lifetime.start(i64 8, i8* %78) #5
  %fun_call.i212.i = call i32 @QueryPerformanceCounter(i64* nonnull %result.i211.i) #5
  %result1.i213.i = load i64, i64* %result.i211.i, align 8
  call void @llvm.lifetime.end(i64 8, i8* %78) #5
  %sub_tmp.i214.i = sub i64 %result1.i213.i, %result1.i56.i
  %int_to_float_cast.i215.i = sitofp i64 %sub_tmp.i214.i to float
  %perf_count_freq.i216.i = load i64, i64* @perf_count_freq, align 8
  %int_to_float_cast1.i217.i = sitofp i64 %perf_count_freq.i216.i to float
  %fdiv_tmp.i218.i = fdiv float %int_to_float_cast.i215.i, %int_to_float_cast1.i217.i
  %fcmp_tmp303.i = fcmp olt float %fdiv_tmp.i218.i, 0x3F91111120000000
  br i1 %fcmp_tmp303.i, label %while204.i.preheader, label %while_end205.i

while204.i.preheader:                             ; preds = %endif188.i
  br label %while204.i

while204.i:                                       ; preds = %while204.i.preheader, %endif214.i
  %wait_time.0304.i = phi float [ %fdiv_tmp.i226.i, %endif214.i ], [ %fdiv_tmp.i218.i, %while204.i.preheader ]
  %fsub_tmp.i = fsub float 0x3F91111120000000, %wait_time.0304.i
  %fmul_tmp210.i = fmul float %fsub_tmp.i, 1.000000e+03
  %int_cast211.i = fptosi float %fmul_tmp210.i to i32
  %icmp_tmp.i = icmp sgt i32 %int_cast211.i, 0
  br i1 %icmp_tmp.i, label %then213.i, label %endif214.i

then213.i:                                        ; preds = %while204.i
  call void @Sleep(i32 %int_cast211.i) #5
  br label %endif214.i

endif214.i:                                       ; preds = %then213.i, %while204.i
  call void @llvm.lifetime.start(i64 8, i8* %79) #5
  %fun_call.i220.i = call i32 @QueryPerformanceCounter(i64* nonnull %result.i219.i) #5
  %result1.i221.i = load i64, i64* %result.i219.i, align 8
  call void @llvm.lifetime.end(i64 8, i8* %79) #5
  %sub_tmp.i222.i = sub i64 %result1.i221.i, %result1.i56.i
  %int_to_float_cast.i223.i = sitofp i64 %sub_tmp.i222.i to float
  %perf_count_freq.i224.i = load i64, i64* @perf_count_freq, align 8
  %int_to_float_cast1.i225.i = sitofp i64 %perf_count_freq.i224.i to float
  %fdiv_tmp.i226.i = fdiv float %int_to_float_cast.i223.i, %int_to_float_cast1.i225.i
  %fcmp_tmp.i = fcmp olt float %fdiv_tmp.i226.i, 0x3F91111120000000
  br i1 %fcmp_tmp.i, label %while204.i, label %while_end205.i.loopexit

while_end205.i.loopexit:                          ; preds = %endif214.i
  %perf_count_freq.i224.i.lcssa = phi i64 [ %perf_count_freq.i224.i, %endif214.i ]
  %result1.i221.i.lcssa = phi i64 [ %result1.i221.i, %endif214.i ]
  br label %while_end205.i

while_end205.i:                                   ; preds = %while_end205.i.loopexit, %endif188.i
  %perf_count_freq223.i = phi i64 [ %perf_count_freq.i216.i, %endif188.i ], [ %perf_count_freq.i224.i.lcssa, %while_end205.i.loopexit ]
  %frame_end_counter.0.lcssa.i = phi i64 [ %result1.i213.i, %endif188.i ], [ %result1.i221.i.lcssa, %while_end205.i.loopexit ]
  %sub_tmp222.i = sub i64 %frame_end_counter.0.lcssa.i, %last_counter_debug_stats.0.ph299.i
  %icmp_tmp224.i = icmp sgt i64 %sub_tmp222.i, %perf_count_freq223.i
  br i1 %icmp_tmp224.i, label %then227.i, label %while_cond.i

then227.i:                                        ; preds = %while_end205.i
  %frame_end_counter.0.lcssa.i.lcssa = phi i64 [ %frame_end_counter.0.lcssa.i, %while_end205.i ]
  %.lcssa = phi i64 [ %121, %while_end205.i ]
  %sound_output.sroa.29.1.i.lcssa = phi i64 [ %sound_output.sroa.29.1.i, %while_end205.i ]
  %sound_output.sroa.33.3.i.lcssa = phi i32 [ %sound_output.sroa.33.3.i, %while_end205.i ]
  %sound_output.sroa.45.1.i.lcssa = phi i32 [ %sound_output.sroa.45.1.i, %while_end205.i ]
  %sound_output.sroa.43.1.i.lcssa = phi i32 [ %sound_output.sroa.43.1.i, %while_end205.i ]
  %sound_output.sroa.40.3.i.lcssa = phi i64 [ %sound_output.sroa.40.3.i, %while_end205.i ]
  %sound_output.sroa.37.2.i.lcssa = phi i32 [ %sound_output.sroa.37.2.i, %while_end205.i ]
  %fun_call175.i.lcssa = phi i1 [ %fun_call175.i, %while_end205.i ]
  %result1.i56.i.lcssa = phi i64 [ %result1.i56.i, %while_end205.i ]
  %last_cycle_count.0.i.lcssa13 = phi i64 [ %last_cycle_count.0.i, %while_end205.i ]
  %sub_tmp232.i = sub i64 %.lcssa, %last_cycle_count.0.i.lcssa13
  %struct_field235.i = load float, float* %.fca.0.gep.i, align 8
  %fmul_tmp236.i = fmul float %struct_field235.i, 1.000000e+03
  %console_output_handle.i227.i = load i64, i64* @console_output_handle, align 8
  %fun_call.i229.i = call i32 @WriteFile(i64 %console_output_handle.i227.i, i8* nonnull %arr_elem_alloca238.i, i32 1, i32* null, i8* null) #5
  %fp_to_fp_cast.i.i = fpext float %fmul_tmp236.i to double
  call fastcc void @print_f64(double %fp_to_fp_cast.i.i) #5
  %console_output_handle.i230.i = load i64, i64* @console_output_handle, align 8
  %fun_call.i232.i = call i32 @WriteFile(i64 %console_output_handle.i230.i, i8* %9, i32 7, i32* null, i8* null) #5
  %fdiv_tmp249.i = fdiv float 1.000000e+03, %fmul_tmp236.i
  %fp_to_fp_cast.i233.i = fpext float %fdiv_tmp249.i to double
  call fastcc void @print_f64(double %fp_to_fp_cast.i233.i) #5
  %console_output_handle.i234.i = load i64, i64* @console_output_handle, align 8
  %fun_call.i236.i = call i32 @WriteFile(i64 %console_output_handle.i234.i, i8* %10, i32 6, i32* null, i8* null) #5
  %int_to_float_cast260.i = sitofp i64 %sub_tmp232.i to double
  %fdiv_tmp261.i = fdiv double %int_to_float_cast260.i, 1.000000e+06
  call fastcc void @print_f64(double %fdiv_tmp261.i) #5
  %console_output_handle.i237.i = load i64, i64* @console_output_handle, align 8
  %fun_call.i239.i = call i32 @WriteFile(i64 %console_output_handle.i237.i, i8* %11, i32 10, i32* null, i8* null) #5
  %struct_field274.i = load float, float* %.fca.0.gep.i, align 8
  call void @llvm.lifetime.start(i64 2, i8* %86) #5
  call void @llvm.lifetime.start(i64 1, i8* nonnull %arr_elem_alloca3.i.i) #5
  store i16 8250, i16* %arr_elem_alloca1.i240.i, align 2
  store i8 10, i8* %arr_elem_alloca3.i.i, align 1
  %console_output_handle.i.i242.i = load i64, i64* @console_output_handle, align 8
  %fun_call.i.i243.i = call i32 @WriteFile(i64 %console_output_handle.i.i242.i, i8* %12, i32 2, i32* null, i8* null) #5
  %console_output_handle.i2.i.i = load i64, i64* @console_output_handle, align 8
  %fun_call.i4.i.i = call i32 @WriteFile(i64 %console_output_handle.i2.i.i, i8* %86, i32 2, i32* null, i8* null) #5
  %fp_to_fp_cast.i.i.i = fpext float %struct_field274.i to double
  call fastcc void @print_f64(double %fp_to_fp_cast.i.i.i) #5
  %console_output_handle.i5.i.i = load i64, i64* @console_output_handle, align 8
  %fun_call.i7.i.i = call i32 @WriteFile(i64 %console_output_handle.i5.i.i, i8* nonnull %arr_elem_alloca3.i.i, i32 1, i32* null, i8* null) #5
  call void @llvm.lifetime.end(i64 2, i8* %86) #5
  call void @llvm.lifetime.end(i64 1, i8* nonnull %arr_elem_alloca3.i.i) #5
  call void @llvm.lifetime.start(i64 36, i8* %87) #5
  call void @llvm.lifetime.start(i64 13, i8* %36) #5
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %36, i8* getelementptr inbounds ([13 x i8], [13 x i8]* @str.9, i64 0, i64 0), i32 13, i32 1, i1 false) #5
  %fun_call13.i.i246.i = call i32 @GetFileAttributesExA(i8* %36, i32 0, <{ i32, i64, i64, i64, i64 }>* nonnull %data.i.i244.i) #5
  %struct_field.i.i248.i = load i64, i64* %struct_field_ptr.i.i247.i, align 8
  call void @llvm.lifetime.end(i64 36, i8* %87) #5
  call void @llvm.lifetime.end(i64 13, i8* %36) #5
  %icmp_tmp.i251.i = icmp eq i64 %struct_field.i.i248.i, %game.sroa.12.0.ph.i
  br i1 %icmp_tmp.i251.i, label %while_cond.outer284.i, label %then.i254.i

then.i254.i:                                      ; preds = %then227.i
  %frame_end_counter.0.lcssa.i.lcssa.lcssa = phi i64 [ %frame_end_counter.0.lcssa.i.lcssa, %then227.i ]
  %.lcssa.lcssa = phi i64 [ %.lcssa, %then227.i ]
  %sound_output.sroa.29.1.i.lcssa.lcssa = phi i64 [ %sound_output.sroa.29.1.i.lcssa, %then227.i ]
  %sound_output.sroa.33.3.i.lcssa.lcssa = phi i32 [ %sound_output.sroa.33.3.i.lcssa, %then227.i ]
  %sound_output.sroa.45.1.i.lcssa.lcssa = phi i32 [ %sound_output.sroa.45.1.i.lcssa, %then227.i ]
  %sound_output.sroa.43.1.i.lcssa.lcssa = phi i32 [ %sound_output.sroa.43.1.i.lcssa, %then227.i ]
  %sound_output.sroa.40.3.i.lcssa.lcssa = phi i64 [ %sound_output.sroa.40.3.i.lcssa, %then227.i ]
  %sound_output.sroa.37.2.i.lcssa.lcssa = phi i32 [ %sound_output.sroa.37.2.i.lcssa, %then227.i ]
  %fun_call175.i.lcssa.lcssa = phi i1 [ %fun_call175.i.lcssa, %then227.i ]
  %result1.i56.i.lcssa.lcssa = phi i64 [ %result1.i56.i.lcssa, %then227.i ]
  call void @llvm.lifetime.start(i64 15, i8* %88) #5
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %88, i8* getelementptr inbounds ([16 x i8], [16 x i8]* @str.19, i64 0, i64 0), i32 15, i32 1, i1 false) #5
  %icmp_tmp.i.i.i = icmp eq i64 %game.sroa.0.0.ph.i, 0
  br i1 %icmp_tmp.i.i.i, label %unload_game_dll.exit.i.i, label %then.i.i256.i

then.i.i256.i:                                    ; preds = %then.i254.i
  %fun_call.i.i255.i = call i32 @FreeLibrary(i64 %game.sroa.0.0.ph.i) #5
  br label %unload_game_dll.exit.i.i

unload_game_dll.exit.i.i:                         ; preds = %then.i.i256.i, %then.i254.i
  %console_output_handle.i.i.i.i = load i64, i64* @console_output_handle, align 8
  %fun_call.i.i.i.i = call i32 @WriteFile(i64 %console_output_handle.i.i.i.i, i8* nonnull %88, i32 15, i32* null, i8* null) #5
  call void @llvm.lifetime.end(i64 15, i8* nonnull %88) #5
  call void @llvm.lifetime.start(i64 13, i8* %38) #5
  call void @llvm.lifetime.start(i64 18, i8* %16) #5
  call void @llvm.lifetime.start(i64 23, i8* %34) #5
  call void @llvm.lifetime.start(i64 18, i8* %35) #5
  call void @llvm.lifetime.start(i64 13, i8* %33) #5
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %38, i8* getelementptr inbounds ([13 x i8], [13 x i8]* @str.9, i64 0, i64 0), i32 13, i32 1, i1 false) #5
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %16, i8* getelementptr inbounds ([18 x i8], [18 x i8]* @str.10, i64 0, i64 0), i32 18, i32 1, i1 false) #5
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %34, i8* getelementptr inbounds ([23 x i8], [23 x i8]* @str.14, i64 0, i64 0), i32 23, i32 1, i1 false) #5
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %35, i8* getelementptr inbounds ([18 x i8], [18 x i8]* @str.16, i64 0, i64 0), i32 18, i32 1, i1 false) #5
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %33, i8* getelementptr inbounds ([14 x i8], [14 x i8]* @str.18, i64 0, i64 0), i32 13, i32 1, i1 false) #5
  call void @llvm.lifetime.start(i64 36, i8* %89) #5
  call void @llvm.lifetime.start(i64 13, i8* nonnull %36) #5
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* nonnull %36, i8* getelementptr inbounds ([13 x i8], [13 x i8]* @str.9, i64 0, i64 0), i32 13, i32 1, i1 false) #5
  %fun_call13.i.i.i.i = call i32 @GetFileAttributesExA(i8* nonnull %36, i32 0, <{ i32, i64, i64, i64, i64 }>* nonnull %data.i.i.i.i) #5
  %struct_field.i.i.i.i = load i64, i64* %struct_field_ptr.i.i.i.i, align 8
  call void @llvm.lifetime.end(i64 36, i8* %89) #5
  call void @llvm.lifetime.end(i64 13, i8* nonnull %36) #5
  %fun_call22.i.i.i = call i32 @CopyFileA(i8* %38, i8* %16, i32 0) #5
  %fun_call31.i.i.i = call i64 @LoadLibraryA(i8* %16) #5
  %fun_call45.i.i.i = call i8* @GetProcAddress(i64 %fun_call31.i.i.i, i8* %34) #5
  %fun_call60.i.i.i = call i8* @GetProcAddress(i64 %fun_call31.i.i.i, i8* %35) #5
  %console_output_handle.i.i11.i.i = load i64, i64* @console_output_handle, align 8
  %fun_call.i.i12.i.i = call i32 @WriteFile(i64 %console_output_handle.i.i11.i.i, i8* %33, i32 13, i32* null, i8* null) #5
  call void @llvm.lifetime.end(i64 13, i8* %38) #5
  call void @llvm.lifetime.end(i64 18, i8* %16) #5
  call void @llvm.lifetime.end(i64 23, i8* %34) #5
  call void @llvm.lifetime.end(i64 18, i8* %35) #5
  call void @llvm.lifetime.end(i64 13, i8* %33) #5
  br label %while_cond.outer.i

main.exit:                                        ; preds = %while_cond.i
  %console_output_handle.i.i2 = load i64, i64* @console_output_handle, align 8
  %fun_call.i46.i = call i32 @WriteFile(i64 %console_output_handle.i.i2, i8* %13, i32 7, i32* null, i8* null) #5
  call void @ExitProcess(i32 0) #5
  call void @llvm.lifetime.end(i64 24, i8* %1)
  call void @llvm.lifetime.end(i64 22, i8* %2)
  call void @llvm.lifetime.end(i64 80, i8* %3)
  call void @llvm.lifetime.end(i64 24, i8* %4)
  call void @llvm.lifetime.end(i64 40, i8* %5)
  call void @llvm.lifetime.end(i64 42, i8* %6)
  call void @llvm.lifetime.end(i64 20, i8* %7)
  call void @llvm.lifetime.end(i64 20, i8* %8)
  call void @llvm.lifetime.end(i64 1, i8* nonnull %arr_elem_alloca238.i)
  call void @llvm.lifetime.end(i64 7, i8* %9)
  call void @llvm.lifetime.end(i64 6, i8* %10)
  call void @llvm.lifetime.end(i64 10, i8* %11)
  call void @llvm.lifetime.end(i64 2, i8* %12)
  call void @llvm.lifetime.end(i64 7, i8* %13)
  ret void
}

; Function Attrs: norecurse nounwind
define internal i8* @memset(i8* %dest, i32 %value, i64 %count) #3 {
vars:
  %ptr_add = getelementptr i8, i8* %dest, i64 %count
  %icmp_tmp1 = icmp eq i8* %ptr_add, %dest
  br i1 %icmp_tmp1, label %while_end, label %while.lr.ph

while.lr.ph:                                      ; preds = %vars
  %int_trunc = trunc i32 %value to i8
  %min.iters.check = icmp ult i64 %count, 16
  br i1 %min.iters.check, label %while.preheader, label %min.iters.checked

min.iters.checked:                                ; preds = %while.lr.ph
  %n.vec = and i64 %count, -16
  %cmp.zero = icmp eq i64 %n.vec, 0
  %ind.end = getelementptr i8, i8* %dest, i64 %n.vec
  br i1 %cmp.zero, label %while.preheader, label %vector.ph

vector.ph:                                        ; preds = %min.iters.checked
  %broadcast.splatinsert = insertelement <16 x i8> undef, i8 %int_trunc, i32 0
  %broadcast.splat = shufflevector <16 x i8> %broadcast.splatinsert, <16 x i8> undef, <16 x i32> zeroinitializer
  %0 = add i64 %n.vec, -16
  %1 = lshr exact i64 %0, 4
  %2 = add nuw nsw i64 %1, 1
  %xtraiter = and i64 %2, 7
  %3 = icmp ult i64 %0, 112
  br i1 %3, label %middle.block.unr-lcssa, label %vector.ph.new

vector.ph.new:                                    ; preds = %vector.ph
  %unroll_iter = sub nsw i64 %2, %xtraiter
  br label %vector.body

vector.body:                                      ; preds = %vector.body, %vector.ph.new
  %index = phi i64 [ 0, %vector.ph.new ], [ %index.next.7, %vector.body ]
  %niter = phi i64 [ %unroll_iter, %vector.ph.new ], [ %niter.nsub.7, %vector.body ]
  %next.gep = getelementptr i8, i8* %dest, i64 %index
  %4 = bitcast i8* %next.gep to <16 x i8>*
  store <16 x i8> %broadcast.splat, <16 x i8>* %4, align 1
  %index.next = or i64 %index, 16
  %next.gep.1 = getelementptr i8, i8* %dest, i64 %index.next
  %5 = bitcast i8* %next.gep.1 to <16 x i8>*
  store <16 x i8> %broadcast.splat, <16 x i8>* %5, align 1
  %index.next.1 = or i64 %index, 32
  %next.gep.2 = getelementptr i8, i8* %dest, i64 %index.next.1
  %6 = bitcast i8* %next.gep.2 to <16 x i8>*
  store <16 x i8> %broadcast.splat, <16 x i8>* %6, align 1
  %index.next.2 = or i64 %index, 48
  %next.gep.3 = getelementptr i8, i8* %dest, i64 %index.next.2
  %7 = bitcast i8* %next.gep.3 to <16 x i8>*
  store <16 x i8> %broadcast.splat, <16 x i8>* %7, align 1
  %index.next.3 = or i64 %index, 64
  %next.gep.4 = getelementptr i8, i8* %dest, i64 %index.next.3
  %8 = bitcast i8* %next.gep.4 to <16 x i8>*
  store <16 x i8> %broadcast.splat, <16 x i8>* %8, align 1
  %index.next.4 = or i64 %index, 80
  %next.gep.5 = getelementptr i8, i8* %dest, i64 %index.next.4
  %9 = bitcast i8* %next.gep.5 to <16 x i8>*
  store <16 x i8> %broadcast.splat, <16 x i8>* %9, align 1
  %index.next.5 = or i64 %index, 96
  %next.gep.6 = getelementptr i8, i8* %dest, i64 %index.next.5
  %10 = bitcast i8* %next.gep.6 to <16 x i8>*
  store <16 x i8> %broadcast.splat, <16 x i8>* %10, align 1
  %index.next.6 = or i64 %index, 112
  %next.gep.7 = getelementptr i8, i8* %dest, i64 %index.next.6
  %11 = bitcast i8* %next.gep.7 to <16 x i8>*
  store <16 x i8> %broadcast.splat, <16 x i8>* %11, align 1
  %index.next.7 = add i64 %index, 128
  %niter.nsub.7 = add i64 %niter, -8
  %niter.ncmp.7 = icmp eq i64 %niter.nsub.7, 0
  br i1 %niter.ncmp.7, label %middle.block.unr-lcssa.loopexit, label %vector.body, !llvm.loop !3

middle.block.unr-lcssa.loopexit:                  ; preds = %vector.body
  %index.next.7.lcssa = phi i64 [ %index.next.7, %vector.body ]
  br label %middle.block.unr-lcssa

middle.block.unr-lcssa:                           ; preds = %middle.block.unr-lcssa.loopexit, %vector.ph
  %index.unr = phi i64 [ 0, %vector.ph ], [ %index.next.7.lcssa, %middle.block.unr-lcssa.loopexit ]
  %lcmp.mod = icmp eq i64 %xtraiter, 0
  br i1 %lcmp.mod, label %middle.block, label %vector.body.epil.preheader

vector.body.epil.preheader:                       ; preds = %middle.block.unr-lcssa
  br label %vector.body.epil

vector.body.epil:                                 ; preds = %vector.body.epil, %vector.body.epil.preheader
  %index.epil = phi i64 [ %index.unr, %vector.body.epil.preheader ], [ %index.next.epil, %vector.body.epil ]
  %epil.iter = phi i64 [ %xtraiter, %vector.body.epil.preheader ], [ %epil.iter.sub, %vector.body.epil ]
  %next.gep.epil = getelementptr i8, i8* %dest, i64 %index.epil
  %12 = bitcast i8* %next.gep.epil to <16 x i8>*
  store <16 x i8> %broadcast.splat, <16 x i8>* %12, align 1
  %index.next.epil = add i64 %index.epil, 16
  %epil.iter.sub = add i64 %epil.iter, -1
  %epil.iter.cmp = icmp eq i64 %epil.iter.sub, 0
  br i1 %epil.iter.cmp, label %middle.block.epilog-lcssa, label %vector.body.epil, !llvm.loop !6

middle.block.epilog-lcssa:                        ; preds = %vector.body.epil
  br label %middle.block

middle.block:                                     ; preds = %middle.block.unr-lcssa, %middle.block.epilog-lcssa
  %cmp.n = icmp eq i64 %n.vec, %count
  br i1 %cmp.n, label %while_end, label %while.preheader

while.preheader:                                  ; preds = %middle.block, %min.iters.checked, %while.lr.ph
  %data.02.ph = phi i8* [ %dest, %min.iters.checked ], [ %dest, %while.lr.ph ], [ %ind.end, %middle.block ]
  br label %while

while:                                            ; preds = %while.preheader, %while
  %data.02 = phi i8* [ %ptr_post_inc, %while ], [ %data.02.ph, %while.preheader ]
  %ptr_post_inc = getelementptr i8, i8* %data.02, i64 1
  store i8 %int_trunc, i8* %data.02, align 1
  %icmp_tmp = icmp eq i8* %ptr_post_inc, %ptr_add
  br i1 %icmp_tmp, label %while_end.loopexit, label %while, !llvm.loop !7

while_end.loopexit:                               ; preds = %while
  br label %while_end

while_end:                                        ; preds = %while_end.loopexit, %middle.block, %vars
  ret i8* %dest
}

; Function Attrs: nounwind
define internal fastcc void @print_f64(double %value) unnamed_addr #0 {
vars:
  %arr_elem_alloca1 = alloca [64 x i8], align 1
  %arr_elem_alloca1.sub = getelementptr inbounds [64 x i8], [64 x i8]* %arr_elem_alloca1, i64 0, i64 0
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca1.sub, i8* getelementptr inbounds ([65 x i8], [65 x i8]* @str.1, i64 0, i64 0), i32 64, i32 1, i1 false)
  %fcmp_tmp = fcmp olt double %value, 0.000000e+00
  br i1 %fcmp_tmp, label %then, label %endif

then:                                             ; preds = %vars
  %fneg_tmp = fsub double -0.000000e+00, %value
  store i8 45, i8* %arr_elem_alloca1.sub, align 1
  br label %endif

endif:                                            ; preds = %then, %vars
  %pd.sroa.10.0 = phi i32 [ 1, %then ], [ 0, %vars ]
  %v.0 = phi double [ %fneg_tmp, %then ], [ %value, %vars ]
  %int_cast = fptoui double %v.0 to i64
  %0 = zext i32 %pd.sroa.10.0 to i64
  %arr_elem_ptr4.i = load i8*, i8** getelementptr inbounds (<{ i32, i8* }>, <{ i32, i8* }>* @decimal_digits, i64 0, i32 1), align 8
  %urem_tmp.i27 = urem i64 %int_cast, 10
  %gep_arr_elem5.i28 = getelementptr i8, i8* %arr_elem_ptr4.i, i64 %urem_tmp.i27
  %arr_elem.i29 = load i8, i8* %gep_arr_elem5.i28, align 1
  %postinc.i.i30 = add nuw nsw i32 %pd.sroa.10.0, 1
  %gep_arr_elem.i.i31 = getelementptr [64 x i8], [64 x i8]* %arr_elem_alloca1, i64 0, i64 %0
  store i8 %arr_elem.i29, i8* %gep_arr_elem.i.i31, align 1
  %1 = icmp ult i64 %int_cast, 10
  br i1 %1, label %while_end.i, label %while_cond.while_cond_crit_edge.i.preheader

while_cond.while_cond_crit_edge.i.preheader:      ; preds = %endif
  br label %while_cond.while_cond_crit_edge.i

while_cond.while_cond_crit_edge.i:                ; preds = %while_cond.while_cond_crit_edge.i.preheader, %while_cond.while_cond_crit_edge.i
  %postinc.i.i33 = phi i32 [ %postinc.i.i, %while_cond.while_cond_crit_edge.i ], [ %postinc.i.i30, %while_cond.while_cond_crit_edge.i.preheader ]
  %v.0.i32 = phi i64 [ %div_tmp.i, %while_cond.while_cond_crit_edge.i ], [ %int_cast, %while_cond.while_cond_crit_edge.i.preheader ]
  %div_tmp.i = udiv i64 %v.0.i32, 10
  %urem_tmp.i = urem i64 %div_tmp.i, 10
  %gep_arr_elem5.i = getelementptr i8, i8* %arr_elem_ptr4.i, i64 %urem_tmp.i
  %arr_elem.i = load i8, i8* %gep_arr_elem5.i, align 1
  %postinc.i.i = add i32 %postinc.i.i33, 1
  %2 = sext i32 %postinc.i.i33 to i64
  %gep_arr_elem.i.i = getelementptr [64 x i8], [64 x i8]* %arr_elem_alloca1, i64 0, i64 %2
  store i8 %arr_elem.i, i8* %gep_arr_elem.i.i, align 1
  %3 = icmp ult i64 %v.0.i32, 100
  br i1 %3, label %while_end.i.loopexit, label %while_cond.while_cond_crit_edge.i

while_end.i.loopexit:                             ; preds = %while_cond.while_cond_crit_edge.i
  %postinc.i.i.lcssa40 = phi i32 [ %postinc.i.i, %while_cond.while_cond_crit_edge.i ]
  %postinc.i.i33.lcssa = phi i32 [ %postinc.i.i33, %while_cond.while_cond_crit_edge.i ]
  br label %while_end.i

while_end.i:                                      ; preds = %while_end.i.loopexit, %endif
  %postinc.i.i.lcssa = phi i32 [ %postinc.i.i30, %endif ], [ %postinc.i.i.lcssa40, %while_end.i.loopexit ]
  %postinc_load.i.i.lcssa = phi i32 [ %pd.sroa.10.0, %endif ], [ %postinc.i.i33.lcssa, %while_end.i.loopexit ]
  %4 = sext i32 %postinc.i.i.lcssa to i64
  %gep_arr_elem14.i = getelementptr [64 x i8], [64 x i8]* %arr_elem_alloca1, i64 0, i64 %4
  %icmp_tmp203.i = icmp ult i8* %gep_arr_elem.i.i31, %gep_arr_elem14.i
  br i1 %icmp_tmp203.i, label %while16.i.preheader, label %u64_to_ascii.exit

while16.i.preheader:                              ; preds = %while_end.i
  br label %while16.i

while16.i:                                        ; preds = %while16.i.preheader, %while16.i
  %end.05.i = phi i8* [ %ptr_pre_dec.i, %while16.i ], [ %gep_arr_elem14.i, %while16.i.preheader ]
  %start.04.i = phi i8* [ %ptr_pre_inc.i, %while16.i ], [ %gep_arr_elem.i.i31, %while16.i.preheader ]
  %ptr_pre_dec.i = getelementptr i8, i8* %end.05.i, i64 -1
  %deref.i = load i8, i8* %ptr_pre_dec.i, align 1
  %deref24.i = load i8, i8* %start.04.i, align 1
  store i8 %deref24.i, i8* %ptr_pre_dec.i, align 1
  store i8 %deref.i, i8* %start.04.i, align 1
  %ptr_pre_inc.i = getelementptr i8, i8* %start.04.i, i64 1
  %icmp_tmp20.i = icmp ult i8* %ptr_pre_inc.i, %ptr_pre_dec.i
  br i1 %icmp_tmp20.i, label %while16.i, label %u64_to_ascii.exit.loopexit

u64_to_ascii.exit.loopexit:                       ; preds = %while16.i
  %arr_elem_ptr.pre = load i8*, i8** getelementptr inbounds (<{ i32, i8* }>, <{ i32, i8* }>* @decimal_digits, i64 0, i32 1), align 8
  br label %u64_to_ascii.exit

u64_to_ascii.exit:                                ; preds = %u64_to_ascii.exit.loopexit, %while_end.i
  %arr_elem_ptr = phi i8* [ %arr_elem_ptr.pre, %u64_to_ascii.exit.loopexit ], [ %arr_elem_ptr4.i, %while_end.i ]
  %int_to_float_cast = sitofp i64 %int_cast to double
  %fsub_tmp = fsub double %v.0, %int_to_float_cast
  %postinc.i14 = add i32 %postinc_load.i.i.lcssa, 2
  store i8 46, i8* %gep_arr_elem14.i, align 1
  %5 = add i32 %postinc_load.i.i.lcssa, 6
  %fmul_tmp = fmul double %fsub_tmp, 1.000000e+01
  %int_cast15 = fptosi double %fmul_tmp to i32
  %int_to_float_cast19 = sitofp i32 %int_cast15 to double
  %fsub_tmp20 = fsub double %fmul_tmp, %int_to_float_cast19
  %6 = sext i32 %int_cast15 to i64
  %gep_arr_elem = getelementptr i8, i8* %arr_elem_ptr, i64 %6
  %arr_elem = load i8, i8* %gep_arr_elem, align 1
  %postinc.i8 = add i32 %postinc_load.i.i.lcssa, 3
  %7 = sext i32 %postinc.i14 to i64
  %gep_arr_elem.i11 = getelementptr [64 x i8], [64 x i8]* %arr_elem_alloca1, i64 0, i64 %7
  store i8 %arr_elem, i8* %gep_arr_elem.i11, align 1
  %fmul_tmp.1 = fmul double %fsub_tmp20, 1.000000e+01
  %int_cast15.1 = fptosi double %fmul_tmp.1 to i32
  %int_to_float_cast19.1 = sitofp i32 %int_cast15.1 to double
  %fsub_tmp20.1 = fsub double %fmul_tmp.1, %int_to_float_cast19.1
  %8 = sext i32 %int_cast15.1 to i64
  %gep_arr_elem.1 = getelementptr i8, i8* %arr_elem_ptr, i64 %8
  %arr_elem.1 = load i8, i8* %gep_arr_elem.1, align 1
  %postinc.i8.1 = add i32 %postinc_load.i.i.lcssa, 4
  %9 = sext i32 %postinc.i8 to i64
  %gep_arr_elem.i11.1 = getelementptr [64 x i8], [64 x i8]* %arr_elem_alloca1, i64 0, i64 %9
  store i8 %arr_elem.1, i8* %gep_arr_elem.i11.1, align 1
  %fmul_tmp.2 = fmul double %fsub_tmp20.1, 1.000000e+01
  %int_cast15.2 = fptosi double %fmul_tmp.2 to i32
  %int_to_float_cast19.2 = sitofp i32 %int_cast15.2 to double
  %fsub_tmp20.2 = fsub double %fmul_tmp.2, %int_to_float_cast19.2
  %10 = sext i32 %int_cast15.2 to i64
  %gep_arr_elem.2 = getelementptr i8, i8* %arr_elem_ptr, i64 %10
  %arr_elem.2 = load i8, i8* %gep_arr_elem.2, align 1
  %postinc.i8.2 = add i32 %postinc_load.i.i.lcssa, 5
  %11 = sext i32 %postinc.i8.1 to i64
  %gep_arr_elem.i11.2 = getelementptr [64 x i8], [64 x i8]* %arr_elem_alloca1, i64 0, i64 %11
  store i8 %arr_elem.2, i8* %gep_arr_elem.i11.2, align 1
  %fmul_tmp.3 = fmul double %fsub_tmp20.2, 1.000000e+01
  %int_cast15.3 = fptosi double %fmul_tmp.3 to i32
  %12 = sext i32 %int_cast15.3 to i64
  %gep_arr_elem.3 = getelementptr i8, i8* %arr_elem_ptr, i64 %12
  %arr_elem.3 = load i8, i8* %gep_arr_elem.3, align 1
  %13 = sext i32 %postinc.i8.2 to i64
  %gep_arr_elem.i11.3 = getelementptr [64 x i8], [64 x i8]* %arr_elem_alloca1, i64 0, i64 %13
  store i8 %arr_elem.3, i8* %gep_arr_elem.i11.3, align 1
  %console_output_handle.i = load i64, i64* @console_output_handle, align 8
  %fun_call.i = call i32 @WriteFile(i64 %console_output_handle.i, i8* nonnull %arr_elem_alloca1.sub, i32 %5, i32* null, i8* null) #5
  ret void
}

; Function Attrs: nounwind
declare i64 @CreateFileA(i8*, i32, i32, i8*, i32, i32, i64) #0

; Function Attrs: nounwind
declare i32 @CopyFileA(i8*, i8*, i32) #0

; Function Attrs: nounwind
declare i32 @GetFileAttributesExA(i8*, i32, <{ i32, i64, i64, i64, i64 }>*) #0

; Function Attrs: nounwind
declare i32 @GetFileSizeEx(i64, i64*) #0

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
declare i32 @GetClientRect(i64, <{ i32, i32, i32, i32 }>*) #0

; Function Attrs: nounwind
declare i32 @StretchDIBits(i64, i32, i32, i32, i32, i32, i32, i32, i32, i8*, <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>*, i32, i32) #0

; Function Attrs: nounwind
declare i32 @CloseHandle(i64) #0

; Function Attrs: nounwind
declare i64 @GetDC(i64) #0

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
declare i32 @GetCursorPos(<{ i32, i32 }>*) #0

; Function Attrs: nounwind
declare i16 @GetKeyState(i32) #0

; Function Attrs: nounwind
declare i32 @ScreenToClient(i64, <{ i32, i32 }>*) #0

; Function Attrs: nounwind readnone
define internal float @sinf(float %x) #1 {
vars:
  %fdiv_tmp2 = fdiv float %x, 0x401921FB60000000
  %fadd_tmp = fadd float %fdiv_tmp2, 5.000000e-01
  %fun_call5 = tail call float @llvm.floor.f32(float %fadd_tmp)
  %fsub_tmp = fsub float %fdiv_tmp2, %fun_call5
  %fmul_tmp7 = fmul float %fsub_tmp, 0x401E5B9D00000000
  %fun_call9 = tail call float @llvm.fabs.f32(float %fsub_tmp)
  %fsub_tmp10 = fsub float 5.000000e-01, %fun_call9
  %fmul_tmp11 = fmul float %fmul_tmp7, %fsub_tmp10
  %fun_call14 = tail call float @llvm.fabs.f32(float %fmul_tmp11)
  %fadd_tmp15 = fadd float %fun_call14, 0x3FFA243900000000
  %fmul_tmp16 = fmul float %fmul_tmp11, %fadd_tmp15
  ret float %fmul_tmp16
}

; Function Attrs: nounwind readnone
define internal float @cosf(float %value) #1 {
vars:
  %fsub_tmp = fsub float 0x3FF921FB60000000, %value
  %fdiv_tmp3 = fdiv float %fsub_tmp, 0x401921FB60000000
  %fadd_tmp = fadd float %fdiv_tmp3, 5.000000e-01
  %fun_call6 = tail call float @llvm.floor.f32(float %fadd_tmp)
  %fsub_tmp7 = fsub float %fdiv_tmp3, %fun_call6
  %fmul_tmp9 = fmul float %fsub_tmp7, 0x401E5B9D00000000
  %fun_call11 = tail call float @llvm.fabs.f32(float %fsub_tmp7)
  %fsub_tmp12 = fsub float 5.000000e-01, %fun_call11
  %fmul_tmp13 = fmul float %fmul_tmp9, %fsub_tmp12
  %fun_call16 = tail call float @llvm.fabs.f32(float %fmul_tmp13)
  %fadd_tmp17 = fadd float %fun_call16, 0x3FFA243900000000
  %fmul_tmp18 = fmul float %fmul_tmp13, %fadd_tmp17
  ret float %fmul_tmp18
}

; Function Attrs: nounwind
define internal i1 @platform_write_file(<{ i32, i8* }> %name, <{ i32, i8* }> %buffer) #0 {
vars:
  %bytes_written = alloca i32, align 4
  %struct_field_extract.i = extractvalue <{ i32, i8* }> %name, 1
  %fun_call2 = tail call i64 @CreateFileA(i8* %struct_field_extract.i, i32 1073741824, i32 0, i8* null, i32 2, i32 0, i64 0)
  %icmp_tmp = icmp eq i64 %fun_call2, 0
  br i1 %icmp_tmp, label %then, label %endif

then:                                             ; preds = %vars
  %fun_call5 = tail call i32 @CloseHandle(i64 0)
  ret i1 false

endif:                                            ; preds = %vars
  store i32 0, i32* %bytes_written, align 4
  %struct_field_extract = extractvalue <{ i32, i8* }> %buffer, 1
  %struct_field_extract7 = extractvalue <{ i32, i8* }> %buffer, 0
  %fun_call8 = call i32 @WriteFile(i64 %fun_call2, i8* %struct_field_extract, i32 %struct_field_extract7, i32* nonnull %bytes_written, i8* null)
  %icmp_tmp10 = icmp ne i32 %fun_call8, 0
  %fun_call14 = tail call i32 @CloseHandle(i64 %fun_call2)
  ret i1 %icmp_tmp10
}

; Function Attrs: nounwind
define internal <{ i32, i8* }> @platform_read_file(<{ i32, i8* }> %name) #0 {
vars:
  %size = alloca i64, align 8
  %bytes_read = alloca i32, align 4
  %struct_field_extract.i = extractvalue <{ i32, i8* }> %name, 1
  %fun_call3 = tail call i64 @CreateFileA(i8* %struct_field_extract.i, i32 -2147483648, i32 1, i8* null, i32 3, i32 0, i64 0)
  %icmp_tmp = icmp eq i64 %fun_call3, 0
  br i1 %icmp_tmp, label %then, label %endif

then:                                             ; preds = %vars
  ret <{ i32, i8* }> zeroinitializer

endif:                                            ; preds = %vars
  store i64 0, i64* %size, align 8
  %fun_call7 = call i32 @GetFileSizeEx(i64 %fun_call3, i64* nonnull %size)
  %icmp_tmp9 = icmp eq i32 %fun_call7, 0
  %size10 = load i64, i64* %size, align 8
  %icmp_tmp11 = icmp eq i64 %size10, 0
  %corphi = or i1 %icmp_tmp9, %icmp_tmp11
  br i1 %corphi, label %then12, label %endif13

then12:                                           ; preds = %endif
  %fun_call15 = call i32 @CloseHandle(i64 %fun_call3)
  ret <{ i32, i8* }> zeroinitializer

endif13:                                          ; preds = %endif
  %fun_call18 = call i8* @VirtualAlloc(i8* null, i64 %size10, i32 4096, i32 4)
  %icmp_tmp20 = icmp eq i8* %fun_call18, null
  br i1 %icmp_tmp20, label %then21, label %endif22

then21:                                           ; preds = %endif13
  %fun_call24 = call i32 @CloseHandle(i64 %fun_call3)
  ret <{ i32, i8* }> zeroinitializer

endif22:                                          ; preds = %endif13
  %size26 = load i64, i64* %size, align 8
  %int_trunc = trunc i64 %size26 to i32
  %fun_call30 = call i32 @ReadFile(i64 %fun_call3, i8* nonnull %fun_call18, i32 %int_trunc, i32* nonnull %bytes_read, i8* null)
  %icmp_tmp32 = icmp eq i32 %fun_call30, 0
  br i1 %icmp_tmp32, label %then33, label %endif34

then33:                                           ; preds = %endif22
  %fun_call36 = call i32 @VirtualFree(i8* nonnull %fun_call18, i64 0, i32 32768)
  %fun_call38 = call i32 @CloseHandle(i64 %fun_call3)
  ret <{ i32, i8* }> zeroinitializer

endif34:                                          ; preds = %endif22
  %fun_call45 = call i32 @CloseHandle(i64 %fun_call3)
  %result46.fca.0.insert = insertvalue <{ i32, i8* }> undef, i32 %int_trunc, 0
  %result46.fca.1.insert = insertvalue <{ i32, i8* }> %result46.fca.0.insert, i8* %fun_call18, 1
  ret <{ i32, i8* }> %result46.fca.1.insert
}

; Function Attrs: nounwind
define internal void @platform_free_file_memory(i8* %mem) #0 {
vars:
  %fun_call = tail call i32 @VirtualFree(i8* %mem, i64 0, i32 32768)
  ret void
}

; Function Attrs: nounwind
define internal i64 @main_window_callback(i64 %window_handle, i32 %message, i64 %w_param, i64 %l_param) #0 {
vars:
  %rect.i = alloca <{ i32, i32, i32, i32 }>, align 8
  %paint = alloca <{ i64, i32, <{ i32, i32, i32, i32 }>, i32, i32, i64, i64, i64, i64 }>, align 8
  switch i32 %message, label %else [
    i32 5, label %then
    i32 16, label %elif_0_then
    i32 2, label %elif_1_then
    i32 15, label %elif_2_then
  ]

then:                                             ; preds = %vars
  %0 = bitcast <{ i32, i32, i32, i32 }>* %rect.i to i8*
  call void @llvm.lifetime.start(i64 16, i8* %0)
  %struct_field.i = load i64, i64* @window.0, align 8
  %fun_call.i = call i32 @GetClientRect(i64 %struct_field.i, <{ i32, i32, i32, i32 }>* nonnull %rect.i) #5
  call void @llvm.lifetime.end(i64 16, i8* %0)
  br label %endif

elif_0_then:                                      ; preds = %vars
  store i1 true, i1* @window_requests_quit, align 1
  br label %endif

elif_1_then:                                      ; preds = %vars
  store i1 true, i1* @window_requests_quit, align 1
  br label %endif

elif_2_then:                                      ; preds = %vars
  %fun_call = call i64 @BeginPaint(i64 %window_handle, <{ i64, i32, <{ i32, i32, i32, i32 }>, i32, i32, i64, i64, i64, i64 }>* nonnull %paint)
  %struct_field.i1 = load i64, i64* @window.1, align 8
  %struct_arrow.i = load i32, i32* getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i64 0, i32 2), align 4
  %struct_arrow2.i = load i32, i32* getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i64 0, i32 3), align 8
  %struct_arrow8.i = load i8*, i8** getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i64 0, i32 1), align 8
  %fun_call.i2 = call i32 @StretchDIBits(i64 %struct_field.i1, i32 0, i32 0, i32 %struct_arrow.i, i32 %struct_arrow2.i, i32 0, i32 0, i32 %struct_arrow.i, i32 %struct_arrow2.i, i8* %struct_arrow8.i, <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>* getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i64 0, i32 0), i32 0, i32 13369376) #5
  %fun_call4 = call i32 @EndPaint(i64 %window_handle, <{ i64, i32, <{ i32, i32, i32, i32 }>, i32, i32, i64, i64, i64, i64 }>* nonnull %paint)
  br label %endif

else:                                             ; preds = %vars
  %fun_call5 = tail call i64 @DefWindowProcA(i64 %window_handle, i32 %message, i64 %w_param, i64 %l_param)
  br label %endif

endif:                                            ; preds = %else, %elif_2_then, %elif_1_then, %elif_0_then, %then
  %result.0 = phi i64 [ %fun_call5, %else ], [ 0, %elif_2_then ], [ 0, %elif_1_then ], [ 0, %elif_0_then ], [ 0, %then ]
  ret i64 %result.0
}

; Function Attrs: argmemonly nounwind
declare void @llvm.memset.p0i8.i64(i8* nocapture, i8, i64, i32, i1) #4

; Function Attrs: argmemonly nounwind
declare void @llvm.lifetime.start(i64, i8* nocapture) #4

; Function Attrs: argmemonly nounwind
declare void @llvm.lifetime.end(i64, i8* nocapture) #4

attributes #0 = { nounwind "target-cpu"="nehalem" "target-features"="+sse2,+cx16,-tbm,-avx512ifma,-avx512dq,-fma4,-prfchw,-bmi2,-xsavec,-fsgsbase,+popcnt,-aes,-pcommit,-xsaves,-avx512er,-clwb,-avx512f,-pku,-smap,+mmx,-xop,-rdseed,-hle,-sse4a,-avx512bw,-clflushopt,-xsave,-avx512vl,-invpcid,-avx512cd,-avx,-rtm,-fma,-bmi,-rdrnd,+sse4.1,+sse4.2,-avx2,+sse,-lzcnt,-pclmul,-prefetchwt1,-f16c,+ssse3,-sgx,+cmov,-avx512vbmi,-movbe,-xsaveopt,-sha,-adx,-avx512pf,+sse3" }
attributes #1 = { nounwind readnone "target-cpu"="nehalem" "target-features"="+sse2,+cx16,-tbm,-avx512ifma,-avx512dq,-fma4,-prfchw,-bmi2,-xsavec,-fsgsbase,+popcnt,-aes,-pcommit,-xsaves,-avx512er,-clwb,-avx512f,-pku,-smap,+mmx,-xop,-rdseed,-hle,-sse4a,-avx512bw,-clflushopt,-xsave,-avx512vl,-invpcid,-avx512cd,-avx,-rtm,-fma,-bmi,-rdrnd,+sse4.1,+sse4.2,-avx2,+sse,-lzcnt,-pclmul,-prefetchwt1,-f16c,+ssse3,-sgx,+cmov,-avx512vbmi,-movbe,-xsaveopt,-sha,-adx,-avx512pf,+sse3" }
attributes #2 = { argmemonly nounwind "target-cpu"="nehalem" "target-features"="+sse2,+cx16,-tbm,-avx512ifma,-avx512dq,-fma4,-prfchw,-bmi2,-xsavec,-fsgsbase,+popcnt,-aes,-pcommit,-xsaves,-avx512er,-clwb,-avx512f,-pku,-smap,+mmx,-xop,-rdseed,-hle,-sse4a,-avx512bw,-clflushopt,-xsave,-avx512vl,-invpcid,-avx512cd,-avx,-rtm,-fma,-bmi,-rdrnd,+sse4.1,+sse4.2,-avx2,+sse,-lzcnt,-pclmul,-prefetchwt1,-f16c,+ssse3,-sgx,+cmov,-avx512vbmi,-movbe,-xsaveopt,-sha,-adx,-avx512pf,+sse3" }
attributes #3 = { norecurse nounwind "target-cpu"="nehalem" "target-features"="+sse2,+cx16,-tbm,-avx512ifma,-avx512dq,-fma4,-prfchw,-bmi2,-xsavec,-fsgsbase,+popcnt,-aes,-pcommit,-xsaves,-avx512er,-clwb,-avx512f,-pku,-smap,+mmx,-xop,-rdseed,-hle,-sse4a,-avx512bw,-clflushopt,-xsave,-avx512vl,-invpcid,-avx512cd,-avx,-rtm,-fma,-bmi,-rdrnd,+sse4.1,+sse4.2,-avx2,+sse,-lzcnt,-pclmul,-prefetchwt1,-f16c,+ssse3,-sgx,+cmov,-avx512vbmi,-movbe,-xsaveopt,-sha,-adx,-avx512pf,+sse3" }
attributes #4 = { argmemonly nounwind }
attributes #5 = { nounwind }

!0 = distinct !{!0, !1}
!1 = !{!"llvm.loop.unroll.disable"}
!2 = distinct !{!2, !1}
!3 = distinct !{!3, !4, !5}
!4 = !{!"llvm.loop.vectorize.width", i32 1}
!5 = !{!"llvm.loop.interleave.count", i32 1}
!6 = distinct !{!6, !1}
!7 = distinct !{!7, !8, !4, !5}
!8 = !{!"llvm.loop.unroll.runtime.disable"}
