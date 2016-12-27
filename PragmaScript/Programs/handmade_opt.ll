; ModuleID = 'handmade.ll'
source_filename = "handmade.ll"
target datalayout = "e-m:w-i64:64-f80:128-n8:16:32:64-S128"
target triple = "x86_64-pc-windows-msvc"

@str = private unnamed_addr constant [11 x i8] c"0123456789\00"
@str_arr = private global [10 x i8] zeroinitializer
@decimal_digits = internal unnamed_addr global <{ i32, i8* }> zeroinitializer
@console_output_handle = internal unnamed_addr global i64 0
@str.1 = private unnamed_addr constant [65 x i8] c"                                                                \00"
@str.7 = private unnamed_addr constant [7 x i8] c"t_sine\00"
@str.8 = private unnamed_addr constant [11 x i8] c"fumm fumm\0A\00"

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
define i32 @__init(i64, i32, i8* nocapture readnone) #0 {
vars:
  %arr_elem_alloca1.i = alloca [0 x i8], align 1
  %arr_elem_alloca1.i1 = alloca [10 x i8], align 1
  tail call void @llvm.memcpy.p0i8.p0i8.i32(i8* getelementptr inbounds ([10 x i8], [10 x i8]* @str_arr, i64 0, i64 0), i8* getelementptr inbounds ([11 x i8], [11 x i8]* @str, i64 0, i64 0), i32 10, i32 1, i1 false)
  store i32 10, i32* getelementptr inbounds (<{ i32, i8* }>, <{ i32, i8* }>* @decimal_digits, i64 0, i32 0), align 8
  store i8* getelementptr inbounds ([10 x i8], [10 x i8]* @str_arr, i64 0, i64 0), i8** getelementptr inbounds (<{ i32, i8* }>, <{ i32, i8* }>* @decimal_digits, i64 0, i32 1), align 4
  %3 = getelementptr inbounds [0 x i8], [0 x i8]* %arr_elem_alloca1.i, i64 0, i64 0
  call void @llvm.lifetime.start(i64 0, i8* %3)
  br i1 select (i1 select (i1 select (i1 icmp eq (i64 ptrtoint (i8* (i8*, i32, i64)* @memset to i64), i64 1234), i1 true, i1 icmp eq (i64 ptrtoint (void ()* @__chkstk to i64), i64 1234)), i1 true, i1 icmp eq (i64 ptrtoint (float (float)* @cosf to i64), i64 1234)), i1 true, i1 icmp eq (i64 ptrtoint (float (float)* @sinf to i64), i64 1234)), label %then.i, label %endif.i

then.i:                                           ; preds = %vars
  %console_output_handle.i.i = load i64, i64* @console_output_handle, align 8
  %fun_call.i.i = call i32 @WriteFile(i64 %console_output_handle.i.i, i8* %3, i32 0, i32* null, i8* null) #5
  call void @llvm.lifetime.end(i64 0, i8* %3)
  br label %__hack_reserve_intrinsics.exit

endif.i:                                          ; preds = %vars
  call void @llvm.lifetime.end(i64 0, i8* %3)
  br label %__hack_reserve_intrinsics.exit

__hack_reserve_intrinsics.exit:                   ; preds = %endif.i, %then.i
  %fun_call2 = tail call i64 @GetStdHandle(i32 -11)
  store i64 %fun_call2, i64* @console_output_handle, align 8
  %fun_call3 = tail call i64 @GetStdHandle(i32 -10)
  %fun_call4 = tail call i64 @GetStdHandle(i32 -12)
  %4 = getelementptr inbounds [10 x i8], [10 x i8]* %arr_elem_alloca1.i1, i64 0, i64 0
  call void @llvm.lifetime.start(i64 10, i8* %4)
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %4, i8* getelementptr inbounds ([11 x i8], [11 x i8]* @str.8, i64 0, i64 0), i32 10, i32 1, i1 false) #5
  %console_output_handle.i.i3 = load i64, i64* @console_output_handle, align 8
  %fun_call.i.i4 = call i32 @WriteFile(i64 %console_output_handle.i.i3, i8* %4, i32 10, i32* null, i8* null) #5
  call void @llvm.lifetime.end(i64 10, i8* %4)
  ret i32 1
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
  br i1 %niter.ncmp.7, label %middle.block.unr-lcssa.loopexit, label %vector.body, !llvm.loop !0

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
  br i1 %epil.iter.cmp, label %middle.block.epilog-lcssa, label %vector.body.epil, !llvm.loop !3

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
  br i1 %icmp_tmp, label %while_end.loopexit, label %while, !llvm.loop !5

while_end.loopexit:                               ; preds = %while
  br label %while_end

while_end:                                        ; preds = %while_end.loopexit, %middle.block, %vars
  ret i8* %dest
}

; Function Attrs: nounwind
declare i64 @GetStdHandle(i32) #0

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
define dllexport i1 @game_update_and_render(<{ i8*, i64, i8*, i64, i8* }>* nocapture readonly %memory, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %input, <{ i8*, i32, i32, i32 }>* nocapture readonly %render_target) #0 {
vars:
  %struct_field_ptr.i = getelementptr inbounds <{ i8*, i64, i8*, i64, i8* }>, <{ i8*, i64, i8*, i64, i8* }>* %memory, i64 0, i32 2
  %0 = bitcast i8** %struct_field_ptr.i to <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>**
  %struct_arrow1.i = load <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>*, <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>** %0, align 8
  %struct_field_ptr1.i = getelementptr inbounds <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow1.i, i64 0, i32 0
  %struct_field.i = load i1, i1* %struct_field_ptr1.i, align 1
  br i1 %struct_field.i, label %get_game_state.exit, label %then.i

then.i:                                           ; preds = %vars
  %struct_field_ptr3.i = getelementptr inbounds <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow1.i, i64 0, i32 7
  store float 2.560000e+02, float* %struct_field_ptr3.i, align 4
  %struct_field_ptr5.i = getelementptr inbounds <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow1.i, i64 0, i32 8
  store float 1.280000e+02, float* %struct_field_ptr5.i, align 4
  %struct_field_ptr7.i = getelementptr inbounds <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow1.i, i64 0, i32 6
  store float 0x3F947AE140000000, float* %struct_field_ptr7.i, align 4
  store i1 true, i1* %struct_field_ptr1.i, align 1
  %struct_field_ptr11.i = getelementptr inbounds <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow1.i, i64 0, i32 3
  store float 2.000000e+02, float* %struct_field_ptr11.i, align 4
  %struct_field_ptr13.i = getelementptr inbounds <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow1.i, i64 0, i32 4
  store float 2.000000e+02, float* %struct_field_ptr13.i, align 4
  br label %get_game_state.exit

get_game_state.exit:                              ; preds = %vars, %then.i
  %struct_field_ptr1.i16 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %input, i64 0, i32 0, i32 0
  %struct_field.i17 = load float, float* %struct_field_ptr1.i16, align 4
  %struct_field_ptr2.i = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %input, i64 0, i32 11
  %struct_arrow.i18 = load i1, i1* %struct_field_ptr2.i, align 1
  %struct_field_ptr3.i19 = getelementptr inbounds <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow1.i, i64 0, i32 12
  %struct_arrow4.i = load i1, i1* %struct_field_ptr3.i19, align 1
  %struct_arrow.i18.not = xor i1 %struct_arrow.i18, true
  %brmerge = or i1 %struct_arrow4.i, %struct_arrow.i18.not
  %struct_arrow4.i.mux = or i1 %struct_arrow4.i, %struct_arrow.i18
  br i1 %brmerge, label %handle_player_input.exit, label %then.i23

then.i23:                                         ; preds = %get_game_state.exit
  store i1 true, i1* %struct_field_ptr3.i19, align 1
  %struct_field_ptr6.i20 = getelementptr inbounds <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow1.i, i64 0, i32 13
  %struct_field_ptr8.i21 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %input, i64 0, i32 0, i32 1
  %1 = bitcast float* %struct_field_ptr8.i21 to i32*
  %struct_field91.i = load i32, i32* %1, align 4
  %2 = bitcast float* %struct_field_ptr6.i20 to i32*
  store i32 %struct_field91.i, i32* %2, align 4
  %struct_field_ptr10.i = getelementptr inbounds <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow1.i, i64 0, i32 14
  %struct_field_ptr11.i22 = getelementptr inbounds <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow1.i, i64 0, i32 5
  %3 = bitcast float* %struct_field_ptr11.i22 to i32*
  %struct_arrow122.i = load i32, i32* %3, align 4
  %4 = bitcast float* %struct_field_ptr10.i to i32*
  store i32 %struct_arrow122.i, i32* %4, align 4
  br label %handle_player_input.exit

handle_player_input.exit:                         ; preds = %get_game_state.exit, %then.i23
  %struct_field = phi i1 [ true, %then.i23 ], [ %struct_arrow4.i.mux, %get_game_state.exit ]
  %struct_field_ptr14.i24 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %input, i64 0, i32 2, i32 0
  %struct_field15.i25 = load i1, i1* %struct_field_ptr14.i24, align 1
  %..i = select i1 %struct_field15.i25, float -1.000000e+00, float 0.000000e+00
  %struct_field_ptr20.i = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %input, i64 0, i32 3, i32 0
  %struct_field21.i = load i1, i1* %struct_field_ptr20.i, align 1
  %fadd_tmp25.i = fadd float %..i, 1.000000e+00
  %delta_x.1.i = select i1 %struct_field21.i, float %fadd_tmp25.i, float %..i
  %struct_field_ptr27.i = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %input, i64 0, i32 1, i32 0
  %struct_field28.i = load i1, i1* %struct_field_ptr27.i, align 1
  %.3.i = select i1 %struct_field28.i, float 1.000000e+00, float 0.000000e+00
  %struct_field_ptr34.i = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %input, i64 0, i32 4, i32 0
  %struct_field35.i = load i1, i1* %struct_field_ptr34.i, align 1
  %fsub_tmp.i26 = fadd float %.3.i, -1.000000e+00
  %delta_y.1.i = select i1 %struct_field35.i, float %fsub_tmp.i26, float %.3.i
  %struct_field_ptr39.i = getelementptr inbounds <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow1.i, i64 0, i32 3
  %struct_arrow41.i = load float, float* %struct_field_ptr39.i, align 4
  %fmul_tmp.i27 = fmul float %struct_field.i17, %delta_x.1.i
  %fmul_tmp44.i = fmul float %fmul_tmp.i27, 2.000000e+02
  %fadd_tmp45.i = fadd float %struct_arrow41.i, %fmul_tmp44.i
  store float %fadd_tmp45.i, float* %struct_field_ptr39.i, align 4
  %struct_field_ptr46.i = getelementptr inbounds <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow1.i, i64 0, i32 4
  %struct_arrow48.i = load float, float* %struct_field_ptr46.i, align 4
  %fmul_tmp51.i = fmul float %struct_field.i17, %delta_y.1.i
  %fmul_tmp52.i = fmul float %fmul_tmp51.i, 2.000000e+02
  %fsub_tmp53.i = fsub float %struct_arrow48.i, %fmul_tmp52.i
  store float %fsub_tmp53.i, float* %struct_field_ptr46.i, align 4
  br i1 %struct_field, label %then, label %endif

then:                                             ; preds = %handle_player_input.exit
  %struct_field_ptr3 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %input, i64 0, i32 0, i32 1
  %struct_field4 = load float, float* %struct_field_ptr3, align 4
  %struct_field_ptr6 = getelementptr inbounds <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow1.i, i64 0, i32 13
  %struct_field7 = load float, float* %struct_field_ptr6, align 4
  %fsub_tmp = fsub float %struct_field4, %struct_field7
  %fcmp_tmp = fcmp ogt float %fsub_tmp, 2.500000e-01
  br i1 %fcmp_tmp, label %then8, label %else

then8:                                            ; preds = %then
  %struct_field_ptr11 = getelementptr inbounds <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow1.i, i64 0, i32 6
  store float 0.000000e+00, float* %struct_field_ptr11, align 4
  br label %endif

else:                                             ; preds = %then
  %fdiv_tmp = fmul float %fsub_tmp, 4.000000e+00
  %struct_field_ptr20 = getelementptr inbounds <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow1.i, i64 0, i32 15
  store float %fdiv_tmp, float* %struct_field_ptr20, align 4
  %struct_field_ptr23 = getelementptr inbounds <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow1.i, i64 0, i32 6
  %struct_field_ptr25 = getelementptr inbounds <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow1.i, i64 0, i32 14
  %struct_field26 = load float, float* %struct_field_ptr25, align 4
  %fsub_tmp.i14 = fsub float 0.000000e+00, %struct_field26
  %fmul_tmp.i15 = fmul float %fdiv_tmp, %fsub_tmp.i14
  %fadd_tmp.i = fadd float %struct_field26, %fmul_tmp.i15
  store float %fadd_tmp.i, float* %struct_field_ptr23, align 4
  br label %endif

endif:                                            ; preds = %then8, %else, %handle_player_input.exit
  %result.0 = phi i1 [ false, %then8 ], [ true, %else ], [ true, %handle_player_input.exit ]
  %fun_call.idx = getelementptr <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow1.i, i64 0, i32 1
  %fun_call.idx.val = load float, float* %fun_call.idx, align 4
  %fun_call.idx1 = getelementptr <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow1.i, i64 0, i32 2
  %fun_call.idx1.val = load float, float* %fun_call.idx1, align 4
  %fun_call.idx2 = getelementptr <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow1.i, i64 0, i32 15
  %fun_call.idx2.val = load float, float* %fun_call.idx2, align 4
  %int_cast.i7 = fptosi float %fun_call.idx.val to i32
  %int_cast3.i = fptosi float %fun_call.idx1.val to i32
  %struct_field_ptr4.i = getelementptr inbounds <{ i8*, i32, i32, i32 }>, <{ i8*, i32, i32, i32 }>* %render_target, i64 0, i32 1
  %struct_arrow5.i = load i32, i32* %struct_field_ptr4.i, align 4
  %struct_field_ptr6.i = getelementptr inbounds <{ i8*, i32, i32, i32 }>, <{ i8*, i32, i32, i32 }>* %render_target, i64 0, i32 2
  %struct_arrow7.i = load i32, i32* %struct_field_ptr6.i, align 4
  %fsub_tmp.i = fsub float 1.000000e+00, %fun_call.idx2.val
  %fmul_tmp.i8 = fmul float %fsub_tmp.i, 2.500000e-01
  %icmp_tmp4.i = icmp sgt i32 %struct_arrow7.i, 0
  br i1 %icmp_tmp4.i, label %for.lr.ph.i, label %render_weird_gradient.exit

for.lr.ph.i:                                      ; preds = %endif
  %struct_field_ptr8.i9 = getelementptr inbounds <{ i8*, i32, i32, i32 }>, <{ i8*, i32, i32, i32 }>* %render_target, i64 0, i32 3
  %struct_arrow9.i = load i32, i32* %struct_field_ptr8.i9, align 4
  %icmp_tmp231.i = icmp sgt i32 %struct_arrow5.i, 0
  %5 = sext i32 %struct_arrow9.i to i64
  br i1 %icmp_tmp231.i, label %for.us.preheader.i10, label %render_weird_gradient.exit

for.us.preheader.i10:                             ; preds = %for.lr.ph.i
  %struct_field_ptr12.i = getelementptr inbounds <{ i8*, i32, i32, i32 }>, <{ i8*, i32, i32, i32 }>* %render_target, i64 0, i32 0
  %struct_arrow13.i = load i8*, i8** %struct_field_ptr12.i, align 8
  %6 = add i32 %struct_arrow5.i, -1
  %7 = zext i32 %6 to i64
  %8 = add nuw nsw i64 %7, 1
  %min.iters.check = icmp ult i64 %8, 4
  %n.vec = and i64 %8, 8589934588
  %cmp.zero = icmp eq i64 %n.vec, 0
  %cast.crd = trunc i64 %n.vec to i32
  %broadcast.splatinsert42 = insertelement <4 x i32> undef, i32 %int_cast.i7, i32 0
  %broadcast.splat43 = shufflevector <4 x i32> %broadcast.splatinsert42, <4 x i32> undef, <4 x i32> zeroinitializer
  %broadcast.splatinsert46 = insertelement <4 x float> undef, float %fmul_tmp.i8, i32 0
  %broadcast.splat47 = shufflevector <4 x float> %broadcast.splatinsert46, <4 x float> undef, <4 x i32> zeroinitializer
  %cmp.n = icmp eq i64 %8, %n.vec
  br label %for.us.i12

for.us.i12:                                       ; preds = %for_cond17.end_for20_crit_edge.us.i, %for.us.preheader.i10
  %row.06.us.i = phi i8* [ %ptr_add.us.i, %for_cond17.end_for20_crit_edge.us.i ], [ %struct_arrow13.i, %for.us.preheader.i10 ]
  %j.05.us.i = phi i32 [ %preinc67.us.i, %for_cond17.end_for20_crit_edge.us.i ], [ 0, %for.us.preheader.i10 ]
  %pointer_bit_cast.us.i11 = bitcast i8* %row.06.us.i to i32*
  %add_tmp28.us.i = add i32 %j.05.us.i, %int_cast3.i
  %int_trunc36.us.i = trunc i32 %add_tmp28.us.i to i8
  %int_to_float_cast46.us.i = uitofp i8 %int_trunc36.us.i to float
  %fmul_tmp48.us.i = fmul float %fmul_tmp.i8, %int_to_float_cast46.us.i
  %int_cast49.us.i = fptosi float %fmul_tmp48.us.i to i8
  %int_cast59.us.i = zext i8 %int_cast49.us.i to i32
  %shl_tmp60.us.i = shl nuw nsw i32 %int_cast59.us.i, 8
  br i1 %min.iters.check, label %for18.us.i.preheader, label %min.iters.checked

min.iters.checked:                                ; preds = %for.us.i12
  %ind.end = getelementptr i32, i32* %pointer_bit_cast.us.i11, i64 %n.vec
  br i1 %cmp.zero, label %for18.us.i.preheader, label %vector.ph

vector.ph:                                        ; preds = %min.iters.checked
  %broadcast.splatinsert44 = insertelement <4 x i32> undef, i32 %add_tmp28.us.i, i32 0
  %broadcast.splat45 = shufflevector <4 x i32> %broadcast.splatinsert44, <4 x i32> undef, <4 x i32> zeroinitializer
  %broadcast.splatinsert48 = insertelement <4 x i32> undef, i32 %shl_tmp60.us.i, i32 0
  %broadcast.splat49 = shufflevector <4 x i32> %broadcast.splatinsert48, <4 x i32> undef, <4 x i32> zeroinitializer
  br label %vector.body

vector.body:                                      ; preds = %vector.body, %vector.ph
  %index = phi i64 [ 0, %vector.ph ], [ %index.next, %vector.body ]
  %next.gep = getelementptr i32, i32* %pointer_bit_cast.us.i11, i64 %index
  %9 = trunc i64 %index to i32
  %broadcast.splatinsert = insertelement <4 x i32> undef, i32 %9, i32 0
  %broadcast.splat = shufflevector <4 x i32> %broadcast.splatinsert, <4 x i32> undef, <4 x i32> zeroinitializer
  %induction = add <4 x i32> %broadcast.splat, <i32 0, i32 1, i32 2, i32 3>
  %10 = add <4 x i32> %induction, %broadcast.splat43
  %11 = xor <4 x i32> %10, %broadcast.splat45
  %12 = trunc <4 x i32> %11 to <4 x i8>
  %13 = trunc <4 x i32> %10 to <4 x i8>
  %14 = uitofp <4 x i8> %12 to <4 x float>
  %15 = fmul <4 x float> %broadcast.splat47, %14
  %16 = fptosi <4 x float> %15 to <4 x i8>
  %17 = uitofp <4 x i8> %13 to <4 x float>
  %18 = fmul <4 x float> %broadcast.splat47, %17
  %19 = fptosi <4 x float> %18 to <4 x i8>
  %20 = zext <4 x i8> %16 to <4 x i32>
  %21 = shl nuw nsw <4 x i32> %20, <i32 16, i32 16, i32 16, i32 16>
  %22 = zext <4 x i8> %19 to <4 x i32>
  %23 = or <4 x i32> %22, %broadcast.splat49
  %24 = or <4 x i32> %23, %21
  %25 = bitcast i32* %next.gep to <4 x i32>*
  store <4 x i32> %24, <4 x i32>* %25, align 4
  %index.next = add i64 %index, 4
  %26 = icmp eq i64 %index.next, %n.vec
  br i1 %26, label %middle.block, label %vector.body, !llvm.loop !7

middle.block:                                     ; preds = %vector.body
  br i1 %cmp.n, label %for_cond17.end_for20_crit_edge.us.i, label %for18.us.i.preheader

for18.us.i.preheader:                             ; preds = %middle.block, %min.iters.checked, %for.us.i12
  %pixel.03.us.i.ph = phi i32* [ %pointer_bit_cast.us.i11, %min.iters.checked ], [ %pointer_bit_cast.us.i11, %for.us.i12 ], [ %ind.end, %middle.block ]
  %i.02.us.i.ph = phi i32 [ 0, %min.iters.checked ], [ 0, %for.us.i12 ], [ %cast.crd, %middle.block ]
  br label %for18.us.i

for18.us.i:                                       ; preds = %for18.us.i.preheader, %for18.us.i
  %pixel.03.us.i = phi i32* [ %ptr_post_inc.us.i, %for18.us.i ], [ %pixel.03.us.i.ph, %for18.us.i.preheader ]
  %i.02.us.i = phi i32 [ %preinc.us.i, %for18.us.i ], [ %i.02.us.i.ph, %for18.us.i.preheader ]
  %add_tmp.us.i = add i32 %i.02.us.i, %int_cast.i7
  %xor_tmp.us.i = xor i32 %add_tmp.us.i, %add_tmp28.us.i
  %int_trunc.us.i = trunc i32 %xor_tmp.us.i to i8
  %int_trunc38.us.i = trunc i32 %add_tmp.us.i to i8
  %int_to_float_cast.us.i = uitofp i8 %int_trunc.us.i to float
  %fmul_tmp43.us.i = fmul float %fmul_tmp.i8, %int_to_float_cast.us.i
  %int_cast44.us.i = fptosi float %fmul_tmp43.us.i to i8
  %int_to_float_cast51.us.i = uitofp i8 %int_trunc38.us.i to float
  %fmul_tmp53.us.i = fmul float %fmul_tmp.i8, %int_to_float_cast51.us.i
  %int_cast54.us.i = fptosi float %fmul_tmp53.us.i to i8
  %int_cast57.us.i = zext i8 %int_cast44.us.i to i32
  %shl_tmp.us.i = shl nuw nsw i32 %int_cast57.us.i, 16
  %int_cast62.us.i = zext i8 %int_cast54.us.i to i32
  %or_tmp.us.i = or i32 %int_cast62.us.i, %shl_tmp60.us.i
  %or_tmp63.us.i = or i32 %or_tmp.us.i, %shl_tmp.us.i
  store i32 %or_tmp63.us.i, i32* %pixel.03.us.i, align 4
  %ptr_post_inc.us.i = getelementptr i32, i32* %pixel.03.us.i, i64 1
  %preinc.us.i = add nuw nsw i32 %i.02.us.i, 1
  %exitcond.i13 = icmp eq i32 %preinc.us.i, %struct_arrow5.i
  br i1 %exitcond.i13, label %for_cond17.end_for20_crit_edge.us.i.loopexit, label %for18.us.i, !llvm.loop !8

for_cond17.end_for20_crit_edge.us.i.loopexit:     ; preds = %for18.us.i
  br label %for_cond17.end_for20_crit_edge.us.i

for_cond17.end_for20_crit_edge.us.i:              ; preds = %for_cond17.end_for20_crit_edge.us.i.loopexit, %middle.block
  %ptr_add.us.i = getelementptr i8, i8* %row.06.us.i, i64 %5
  %preinc67.us.i = add nuw nsw i32 %j.05.us.i, 1
  %exitcond8.i = icmp eq i32 %preinc67.us.i, %struct_arrow7.i
  br i1 %exitcond8.i, label %render_weird_gradient.exit.loopexit, label %for.us.i12

render_weird_gradient.exit.loopexit:              ; preds = %for_cond17.end_for20_crit_edge.us.i
  br label %render_weird_gradient.exit

render_weird_gradient.exit:                       ; preds = %render_weird_gradient.exit.loopexit, %endif, %for.lr.ph.i
  %struct_field_ptr30 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %input, i64 0, i32 8
  %struct_arrow = load i32, i32* %struct_field_ptr30, align 4
  %struct_field_ptr31 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %input, i64 0, i32 9
  %struct_arrow32 = load i32, i32* %struct_field_ptr31, align 4
  %struct_field_ptr1.i3 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %input, i64 0, i32 5, i32 1
  %struct_field.i4 = load i1, i1* %struct_field_ptr1.i3, align 1
  br i1 %struct_field.i4, label %then.i6, label %cor.rhs.i

cor.rhs.i:                                        ; preds = %render_weird_gradient.exit
  %struct_field_ptr3.i5 = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %input, i64 0, i32 5, i32 2
  %struct_field4.i = load i1, i1* %struct_field_ptr3.i5, align 1
  br i1 %struct_field4.i, label %then.i6, label %cor.rhs5.i

cor.rhs5.i:                                       ; preds = %cor.rhs.i
  %struct_field_ptr8.i = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %input, i64 0, i32 6, i32 1
  %struct_field9.i = load i1, i1* %struct_field_ptr8.i, align 1
  br i1 %struct_field9.i, label %then.i6, label %cor.rhs11.i

cor.rhs11.i:                                      ; preds = %cor.rhs5.i
  %struct_field_ptr14.i = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %input, i64 0, i32 6, i32 2
  %struct_field15.i = load i1, i1* %struct_field_ptr14.i, align 1
  br i1 %struct_field15.i, label %then.i6, label %endif.i

then.i6:                                          ; preds = %cor.rhs11.i, %cor.rhs5.i, %cor.rhs.i, %render_weird_gradient.exit
  br label %endif.i

endif.i:                                          ; preds = %then.i6, %cor.rhs11.i
  %color.0.i = phi i32 [ 16777215, %then.i6 ], [ 16711680, %cor.rhs11.i ]
  %struct_field_ptr18.i = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %input, i64 0, i32 5, i32 0
  %struct_field19.i = load i1, i1* %struct_field_ptr18.i, align 1
  %.color.0.i = select i1 %struct_field19.i, i32 65535, i32 %color.0.i
  %struct_field_ptr23.i = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %input, i64 0, i32 6, i32 0
  %struct_field24.i = load i1, i1* %struct_field_ptr23.i, align 1
  %color.2.i = select i1 %struct_field24.i, i32 16711935, i32 %.color.0.i
  %27 = or i1 %struct_field19.i, %struct_field24.i
  %size.1.i = select i1 %27, i32 24, i32 12
  %struct_field_ptr29.i = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %input, i64 0, i32 0, i32 1
  %struct_field30.i = load float, float* %struct_field_ptr29.i, align 4
  %fmul_tmp.i = fmul float %struct_field30.i, 2.000000e+00
  %fmul_tmp31.i = fmul float %fmul_tmp.i, 0x400921FB60000000
  %fmul_tmp32.i = fmul float %fmul_tmp31.i, 3.000000e+00
  %fdiv_tmp2.i.i = fdiv float %fmul_tmp32.i, 0x401921FB60000000
  %fadd_tmp.i.i = fadd float %fdiv_tmp2.i.i, 5.000000e-01
  %fun_call5.i.i = tail call float @llvm.floor.f32(float %fadd_tmp.i.i) #5
  %fsub_tmp.i.i = fsub float %fdiv_tmp2.i.i, %fun_call5.i.i
  %fmul_tmp7.i.i = fmul float %fsub_tmp.i.i, 0x401E5B9D00000000
  %fun_call9.i.i = tail call float @llvm.fabs.f32(float %fsub_tmp.i.i) #5
  %fsub_tmp10.i.i = fsub float 5.000000e-01, %fun_call9.i.i
  %fmul_tmp11.i.i = fmul float %fmul_tmp7.i.i, %fsub_tmp10.i.i
  %fun_call14.i.i = tail call float @llvm.fabs.f32(float %fmul_tmp11.i.i) #5
  %fadd_tmp15.i.i = fadd float %fun_call14.i.i, 0x3FFA243900000000
  %fmul_tmp16.i.i = fmul float %fmul_tmp11.i.i, %fadd_tmp15.i.i
  %fmul_tmp33.i = fmul float %fmul_tmp16.i.i, 2.000000e+00
  %int_cast.i = fptosi float %fmul_tmp33.i to i32
  %add_tmp.i = add i32 %int_cast.i, %size.1.i
  %sub_tmp.i = sub i32 %struct_arrow32, %add_tmp.i
  %struct_arrow.i = load i32, i32* %struct_field_ptr6.i, align 4
  %sub_tmp37.i = add i32 %struct_arrow.i, -1
  %icmp_tmp.i33.i = icmp slt i32 %sub_tmp.i, 0
  %icmp_tmp3.i34.i = icmp sgt i32 %sub_tmp.i, %sub_tmp37.i
  %max.value.i35.i = select i1 %icmp_tmp3.i34.i, i32 %sub_tmp37.i, i32 %sub_tmp.i
  %fun_call3839.i = select i1 %icmp_tmp.i33.i, i32 0, i32 %max.value.i35.i
  %add_tmp40.i = add i32 %struct_arrow32, 1
  %add_tmp41.i = add i32 %add_tmp40.i, %add_tmp.i
  %icmp_tmp.i26.i = icmp slt i32 %add_tmp41.i, 0
  %icmp_tmp3.i27.i = icmp sgt i32 %add_tmp41.i, %sub_tmp37.i
  %max.value.i28.i = select i1 %icmp_tmp3.i27.i, i32 %sub_tmp37.i, i32 %add_tmp41.i
  %fun_call4632.i = select i1 %icmp_tmp.i26.i, i32 0, i32 %max.value.i28.i
  %sub_tmp48.i = sub i32 %struct_arrow, %add_tmp.i
  %struct_arrow51.i = load i32, i32* %struct_field_ptr4.i, align 4
  %sub_tmp52.i = add i32 %struct_arrow51.i, -1
  %icmp_tmp.i19.i = icmp slt i32 %sub_tmp48.i, 0
  %icmp_tmp3.i20.i = icmp sgt i32 %sub_tmp48.i, %sub_tmp52.i
  %max.value.i21.i = select i1 %icmp_tmp3.i20.i, i32 %sub_tmp52.i, i32 %sub_tmp48.i
  %fun_call5325.i = select i1 %icmp_tmp.i19.i, i32 0, i32 %max.value.i21.i
  %add_tmp55.i = add i32 %struct_arrow, 1
  %add_tmp56.i = add i32 %add_tmp55.i, %add_tmp.i
  %icmp_tmp.i.i = icmp slt i32 %add_tmp56.i, 0
  %icmp_tmp3.i.i = icmp sgt i32 %add_tmp56.i, %sub_tmp52.i
  %max.value.i.i = select i1 %icmp_tmp3.i.i, i32 %sub_tmp52.i, i32 %add_tmp56.i
  %fun_call6118.i = select i1 %icmp_tmp.i.i, i32 0, i32 %max.value.i.i
  %struct_field_ptr66.i = getelementptr inbounds <{ i8*, i32, i32, i32 }>, <{ i8*, i32, i32, i32 }>* %render_target, i64 0, i32 3
  %icmp_tmp43.i = icmp slt i32 %fun_call3839.i, %fun_call4632.i
  %icmp_tmp8340.i = icmp slt i32 %fun_call5325.i, %fun_call6118.i
  %or.cond.i = and i1 %icmp_tmp43.i, %icmp_tmp8340.i
  br i1 %or.cond.i, label %for.us.preheader.i, label %render_player.exit

for.us.preheader.i:                               ; preds = %endif.i
  %struct_field_ptr62.i = getelementptr inbounds <{ i8*, i32, i32, i32 }>, <{ i8*, i32, i32, i32 }>* %render_target, i64 0, i32 0
  %struct_arrow63.i = load i8*, i8** %struct_field_ptr62.i, align 8
  %struct_arrow67.i = load i32, i32* %struct_field_ptr66.i, align 4
  %mul_tmp.i = mul i32 %struct_arrow67.i, %fun_call3839.i
  %28 = sext i32 %mul_tmp.i to i64
  %ptr_add.i = getelementptr i8, i8* %struct_arrow63.i, i64 %28
  %mul_tmp70.i = shl i32 %fun_call5325.i, 2
  %29 = sext i32 %mul_tmp70.i to i64
  %ptr_add71.i = getelementptr i8, i8* %ptr_add.i, i64 %29
  %30 = add i32 %fun_call6118.i, -1
  %31 = sub i32 %30, %fun_call5325.i
  %32 = zext i32 %31 to i64
  %33 = add nuw nsw i64 %32, 1
  %34 = and i64 %33, 8589934584
  %35 = add nsw i64 %34, -8
  %36 = lshr exact i64 %35, 3
  %37 = add nuw nsw i64 %36, 1
  %38 = and i64 %33, 8589934588
  %39 = add nsw i64 %38, -4
  %40 = lshr exact i64 %39, 2
  %41 = add nuw nsw i64 %40, 1
  %min.iters.check55 = icmp ult i64 %33, 4
  %n.vec58 = and i64 %33, 8589934588
  %cmp.zero59 = icmp eq i64 %n.vec58, 0
  %cast.crd64 = trunc i64 %n.vec58 to i32
  %ind.end65 = add i32 %fun_call5325.i, %cast.crd64
  %broadcast.splatinsert81 = insertelement <4 x i32> undef, i32 %struct_arrow, i32 0
  %broadcast.splat82 = shufflevector <4 x i32> %broadcast.splatinsert81, <4 x i32> undef, <4 x i32> zeroinitializer
  %broadcast.splatinsert83 = insertelement <4 x i32> undef, i32 %color.2.i, i32 0
  %broadcast.splat84 = shufflevector <4 x i32> %broadcast.splatinsert83, <4 x i32> undef, <4 x i32> zeroinitializer
  %xtraiter132 = and i64 %41, 1
  %42 = icmp eq i64 %40, 0
  %lcmp.mod133 = icmp eq i64 %xtraiter132, 0
  %cmp.n68 = icmp eq i64 %33, %n.vec58
  %unroll_iter134 = sub nsw i64 %41, %xtraiter132
  %min.iters.check90 = icmp ult i64 %33, 8
  %n.vec93 = and i64 %33, 8589934584
  %cmp.zero94 = icmp eq i64 %n.vec93, 0
  %cast.crd99 = trunc i64 %n.vec93 to i32
  %ind.end100 = add i32 %fun_call5325.i, %cast.crd99
  %broadcast.splatinsert125 = insertelement <4 x i32> undef, i32 %color.2.i, i32 0
  %broadcast.splat126 = shufflevector <4 x i32> %broadcast.splatinsert125, <4 x i32> undef, <4 x i32> zeroinitializer
  %xtraiter = and i64 %37, 7
  %43 = icmp ult i64 %35, 56
  %lcmp.mod = icmp eq i64 %xtraiter, 0
  %cmp.n103 = icmp eq i64 %33, %n.vec93
  %unroll_iter = sub nsw i64 %37, %xtraiter
  br label %for.us.i

for.us.i:                                         ; preds = %for_cond76.end_for79_crit_edge.us.i, %for.us.preheader.i
  %y.045.us.i = phi i32 [ %preinc98.us.i, %for_cond76.end_for79_crit_edge.us.i ], [ %fun_call3839.i, %for.us.preheader.i ]
  %row.044.us.i = phi i8* [ %ptr_add96.us.i, %for_cond76.end_for79_crit_edge.us.i ], [ %ptr_add71.i, %for.us.preheader.i ]
  %pointer_bit_cast.us.i = bitcast i8* %row.044.us.i to i32*
  %icmp_tmp87.us.i = icmp eq i32 %y.045.us.i, %struct_arrow32
  br i1 %icmp_tmp87.us.i, label %for77.us.us.i.preheader, label %for77.us46.i.preheader

for77.us46.i.preheader:                           ; preds = %for.us.i
  br i1 %min.iters.check90, label %for77.us46.i.preheader130, label %min.iters.checked91

for77.us46.i.preheader130:                        ; preds = %middle.block88, %min.iters.checked91, %for77.us46.i.preheader
  %x.042.us47.i.ph = phi i32 [ %fun_call5325.i, %min.iters.checked91 ], [ %fun_call5325.i, %for77.us46.i.preheader ], [ %ind.end100, %middle.block88 ]
  %pixel.041.us48.i.ph = phi i32* [ %pointer_bit_cast.us.i, %min.iters.checked91 ], [ %pointer_bit_cast.us.i, %for77.us46.i.preheader ], [ %ind.end102, %middle.block88 ]
  br label %for77.us46.i

min.iters.checked91:                              ; preds = %for77.us46.i.preheader
  %ind.end102 = getelementptr i32, i32* %pointer_bit_cast.us.i, i64 %n.vec93
  br i1 %cmp.zero94, label %for77.us46.i.preheader130, label %vector.ph95

vector.ph95:                                      ; preds = %min.iters.checked91
  br i1 %43, label %middle.block88.unr-lcssa, label %vector.ph95.new

vector.ph95.new:                                  ; preds = %vector.ph95
  br label %vector.body87

vector.body87:                                    ; preds = %vector.body87, %vector.ph95.new
  %index96 = phi i64 [ 0, %vector.ph95.new ], [ %index.next97.7, %vector.body87 ]
  %niter = phi i64 [ %unroll_iter, %vector.ph95.new ], [ %niter.nsub.7, %vector.body87 ]
  %next.gep109 = getelementptr i32, i32* %pointer_bit_cast.us.i, i64 %index96
  %44 = bitcast i32* %next.gep109 to <4 x i32>*
  store <4 x i32> %broadcast.splat126, <4 x i32>* %44, align 4
  %45 = getelementptr i32, i32* %next.gep109, i64 4
  %46 = bitcast i32* %45 to <4 x i32>*
  store <4 x i32> %broadcast.splat126, <4 x i32>* %46, align 4
  %index.next97 = or i64 %index96, 8
  %next.gep109.1 = getelementptr i32, i32* %pointer_bit_cast.us.i, i64 %index.next97
  %47 = bitcast i32* %next.gep109.1 to <4 x i32>*
  store <4 x i32> %broadcast.splat126, <4 x i32>* %47, align 4
  %48 = getelementptr i32, i32* %next.gep109.1, i64 4
  %49 = bitcast i32* %48 to <4 x i32>*
  store <4 x i32> %broadcast.splat126, <4 x i32>* %49, align 4
  %index.next97.1 = or i64 %index96, 16
  %next.gep109.2 = getelementptr i32, i32* %pointer_bit_cast.us.i, i64 %index.next97.1
  %50 = bitcast i32* %next.gep109.2 to <4 x i32>*
  store <4 x i32> %broadcast.splat126, <4 x i32>* %50, align 4
  %51 = getelementptr i32, i32* %next.gep109.2, i64 4
  %52 = bitcast i32* %51 to <4 x i32>*
  store <4 x i32> %broadcast.splat126, <4 x i32>* %52, align 4
  %index.next97.2 = or i64 %index96, 24
  %next.gep109.3 = getelementptr i32, i32* %pointer_bit_cast.us.i, i64 %index.next97.2
  %53 = bitcast i32* %next.gep109.3 to <4 x i32>*
  store <4 x i32> %broadcast.splat126, <4 x i32>* %53, align 4
  %54 = getelementptr i32, i32* %next.gep109.3, i64 4
  %55 = bitcast i32* %54 to <4 x i32>*
  store <4 x i32> %broadcast.splat126, <4 x i32>* %55, align 4
  %index.next97.3 = or i64 %index96, 32
  %next.gep109.4 = getelementptr i32, i32* %pointer_bit_cast.us.i, i64 %index.next97.3
  %56 = bitcast i32* %next.gep109.4 to <4 x i32>*
  store <4 x i32> %broadcast.splat126, <4 x i32>* %56, align 4
  %57 = getelementptr i32, i32* %next.gep109.4, i64 4
  %58 = bitcast i32* %57 to <4 x i32>*
  store <4 x i32> %broadcast.splat126, <4 x i32>* %58, align 4
  %index.next97.4 = or i64 %index96, 40
  %next.gep109.5 = getelementptr i32, i32* %pointer_bit_cast.us.i, i64 %index.next97.4
  %59 = bitcast i32* %next.gep109.5 to <4 x i32>*
  store <4 x i32> %broadcast.splat126, <4 x i32>* %59, align 4
  %60 = getelementptr i32, i32* %next.gep109.5, i64 4
  %61 = bitcast i32* %60 to <4 x i32>*
  store <4 x i32> %broadcast.splat126, <4 x i32>* %61, align 4
  %index.next97.5 = or i64 %index96, 48
  %next.gep109.6 = getelementptr i32, i32* %pointer_bit_cast.us.i, i64 %index.next97.5
  %62 = bitcast i32* %next.gep109.6 to <4 x i32>*
  store <4 x i32> %broadcast.splat126, <4 x i32>* %62, align 4
  %63 = getelementptr i32, i32* %next.gep109.6, i64 4
  %64 = bitcast i32* %63 to <4 x i32>*
  store <4 x i32> %broadcast.splat126, <4 x i32>* %64, align 4
  %index.next97.6 = or i64 %index96, 56
  %next.gep109.7 = getelementptr i32, i32* %pointer_bit_cast.us.i, i64 %index.next97.6
  %65 = bitcast i32* %next.gep109.7 to <4 x i32>*
  store <4 x i32> %broadcast.splat126, <4 x i32>* %65, align 4
  %66 = getelementptr i32, i32* %next.gep109.7, i64 4
  %67 = bitcast i32* %66 to <4 x i32>*
  store <4 x i32> %broadcast.splat126, <4 x i32>* %67, align 4
  %index.next97.7 = add i64 %index96, 64
  %niter.nsub.7 = add i64 %niter, -8
  %niter.ncmp.7 = icmp eq i64 %niter.nsub.7, 0
  br i1 %niter.ncmp.7, label %middle.block88.unr-lcssa.loopexit, label %vector.body87, !llvm.loop !9

middle.block88.unr-lcssa.loopexit:                ; preds = %vector.body87
  %index.next97.7.lcssa = phi i64 [ %index.next97.7, %vector.body87 ]
  br label %middle.block88.unr-lcssa

middle.block88.unr-lcssa:                         ; preds = %middle.block88.unr-lcssa.loopexit, %vector.ph95
  %index96.unr = phi i64 [ 0, %vector.ph95 ], [ %index.next97.7.lcssa, %middle.block88.unr-lcssa.loopexit ]
  br i1 %lcmp.mod, label %middle.block88, label %vector.body87.epil.preheader

vector.body87.epil.preheader:                     ; preds = %middle.block88.unr-lcssa
  br label %vector.body87.epil

vector.body87.epil:                               ; preds = %vector.body87.epil, %vector.body87.epil.preheader
  %index96.epil = phi i64 [ %index96.unr, %vector.body87.epil.preheader ], [ %index.next97.epil, %vector.body87.epil ]
  %epil.iter = phi i64 [ %xtraiter, %vector.body87.epil.preheader ], [ %epil.iter.sub, %vector.body87.epil ]
  %next.gep109.epil = getelementptr i32, i32* %pointer_bit_cast.us.i, i64 %index96.epil
  %68 = bitcast i32* %next.gep109.epil to <4 x i32>*
  store <4 x i32> %broadcast.splat126, <4 x i32>* %68, align 4
  %69 = getelementptr i32, i32* %next.gep109.epil, i64 4
  %70 = bitcast i32* %69 to <4 x i32>*
  store <4 x i32> %broadcast.splat126, <4 x i32>* %70, align 4
  %index.next97.epil = add i64 %index96.epil, 8
  %epil.iter.sub = add i64 %epil.iter, -1
  %epil.iter.cmp = icmp eq i64 %epil.iter.sub, 0
  br i1 %epil.iter.cmp, label %middle.block88.epilog-lcssa, label %vector.body87.epil, !llvm.loop !10

middle.block88.epilog-lcssa:                      ; preds = %vector.body87.epil
  br label %middle.block88

middle.block88:                                   ; preds = %middle.block88.unr-lcssa, %middle.block88.epilog-lcssa
  br i1 %cmp.n103, label %for_cond76.end_for79_crit_edge.us.i, label %for77.us46.i.preheader130

for77.us.us.i.preheader:                          ; preds = %for.us.i
  br i1 %min.iters.check55, label %for77.us.us.i.preheader129, label %min.iters.checked56

for77.us.us.i.preheader129:                       ; preds = %middle.block53, %min.iters.checked56, %for77.us.us.i.preheader
  %x.042.us.us.i.ph = phi i32 [ %fun_call5325.i, %min.iters.checked56 ], [ %fun_call5325.i, %for77.us.us.i.preheader ], [ %ind.end65, %middle.block53 ]
  %pixel.041.us.us.i.ph = phi i32* [ %pointer_bit_cast.us.i, %min.iters.checked56 ], [ %pointer_bit_cast.us.i, %for77.us.us.i.preheader ], [ %ind.end67, %middle.block53 ]
  br label %for77.us.us.i

min.iters.checked56:                              ; preds = %for77.us.us.i.preheader
  %ind.end67 = getelementptr i32, i32* %pointer_bit_cast.us.i, i64 %n.vec58
  br i1 %cmp.zero59, label %for77.us.us.i.preheader129, label %vector.ph60

vector.ph60:                                      ; preds = %min.iters.checked56
  br i1 %42, label %middle.block53.unr-lcssa, label %vector.ph60.new

vector.ph60.new:                                  ; preds = %vector.ph60
  br label %vector.body52

vector.body52:                                    ; preds = %vector.body52, %vector.ph60.new
  %index61 = phi i64 [ 0, %vector.ph60.new ], [ %index.next62.1, %vector.body52 ]
  %niter135 = phi i64 [ %unroll_iter134, %vector.ph60.new ], [ %niter135.nsub.1, %vector.body52 ]
  %71 = trunc i64 %index61 to i32
  %offset.idx69 = add i32 %fun_call5325.i, %71
  %broadcast.splatinsert70 = insertelement <4 x i32> undef, i32 %offset.idx69, i32 0
  %broadcast.splat71 = shufflevector <4 x i32> %broadcast.splatinsert70, <4 x i32> undef, <4 x i32> zeroinitializer
  %induction72 = add <4 x i32> %broadcast.splat71, <i32 0, i32 1, i32 2, i32 3>
  %next.gep73 = getelementptr i32, i32* %pointer_bit_cast.us.i, i64 %index61
  %72 = icmp eq <4 x i32> %induction72, %broadcast.splat82
  %73 = select <4 x i1> %72, <4 x i32> <i32 16777215, i32 16777215, i32 16777215, i32 16777215>, <4 x i32> %broadcast.splat84
  %74 = bitcast i32* %next.gep73 to <4 x i32>*
  store <4 x i32> %73, <4 x i32>* %74, align 4
  %index.next62 = or i64 %index61, 4
  %75 = trunc i64 %index.next62 to i32
  %offset.idx69.1 = add i32 %fun_call5325.i, %75
  %broadcast.splatinsert70.1 = insertelement <4 x i32> undef, i32 %offset.idx69.1, i32 0
  %broadcast.splat71.1 = shufflevector <4 x i32> %broadcast.splatinsert70.1, <4 x i32> undef, <4 x i32> zeroinitializer
  %induction72.1 = add <4 x i32> %broadcast.splat71.1, <i32 0, i32 1, i32 2, i32 3>
  %next.gep73.1 = getelementptr i32, i32* %pointer_bit_cast.us.i, i64 %index.next62
  %76 = icmp eq <4 x i32> %induction72.1, %broadcast.splat82
  %77 = select <4 x i1> %76, <4 x i32> <i32 16777215, i32 16777215, i32 16777215, i32 16777215>, <4 x i32> %broadcast.splat84
  %78 = bitcast i32* %next.gep73.1 to <4 x i32>*
  store <4 x i32> %77, <4 x i32>* %78, align 4
  %index.next62.1 = add i64 %index61, 8
  %niter135.nsub.1 = add i64 %niter135, -2
  %niter135.ncmp.1 = icmp eq i64 %niter135.nsub.1, 0
  br i1 %niter135.ncmp.1, label %middle.block53.unr-lcssa.loopexit, label %vector.body52, !llvm.loop !11

middle.block53.unr-lcssa.loopexit:                ; preds = %vector.body52
  %index.next62.1.lcssa = phi i64 [ %index.next62.1, %vector.body52 ]
  br label %middle.block53.unr-lcssa

middle.block53.unr-lcssa:                         ; preds = %middle.block53.unr-lcssa.loopexit, %vector.ph60
  %index61.unr = phi i64 [ 0, %vector.ph60 ], [ %index.next62.1.lcssa, %middle.block53.unr-lcssa.loopexit ]
  br i1 %lcmp.mod133, label %middle.block53, label %vector.body52.epil.preheader

vector.body52.epil.preheader:                     ; preds = %middle.block53.unr-lcssa
  br label %vector.body52.epil

vector.body52.epil:                               ; preds = %vector.body52.epil.preheader
  %79 = trunc i64 %index61.unr to i32
  %offset.idx69.epil = add i32 %fun_call5325.i, %79
  %broadcast.splatinsert70.epil = insertelement <4 x i32> undef, i32 %offset.idx69.epil, i32 0
  %broadcast.splat71.epil = shufflevector <4 x i32> %broadcast.splatinsert70.epil, <4 x i32> undef, <4 x i32> zeroinitializer
  %induction72.epil = add <4 x i32> %broadcast.splat71.epil, <i32 0, i32 1, i32 2, i32 3>
  %next.gep73.epil = getelementptr i32, i32* %pointer_bit_cast.us.i, i64 %index61.unr
  %80 = icmp eq <4 x i32> %induction72.epil, %broadcast.splat82
  %81 = select <4 x i1> %80, <4 x i32> <i32 16777215, i32 16777215, i32 16777215, i32 16777215>, <4 x i32> %broadcast.splat84
  %82 = bitcast i32* %next.gep73.epil to <4 x i32>*
  store <4 x i32> %81, <4 x i32>* %82, align 4
  br label %middle.block53.epilog-lcssa

middle.block53.epilog-lcssa:                      ; preds = %vector.body52.epil
  br label %middle.block53

middle.block53:                                   ; preds = %middle.block53.unr-lcssa, %middle.block53.epilog-lcssa
  br i1 %cmp.n68, label %for_cond76.end_for79_crit_edge.us.i, label %for77.us.us.i.preheader129

for77.us46.i:                                     ; preds = %for77.us46.i.preheader130, %for77.us46.i
  %x.042.us47.i = phi i32 [ %preinc.us50.i, %for77.us46.i ], [ %x.042.us47.i.ph, %for77.us46.i.preheader130 ]
  %pixel.041.us48.i = phi i32* [ %ptr_post_inc.us49.i, %for77.us46.i ], [ %pixel.041.us48.i.ph, %for77.us46.i.preheader130 ]
  %ptr_post_inc.us49.i = getelementptr i32, i32* %pixel.041.us48.i, i64 1
  store i32 %color.2.i, i32* %pixel.041.us48.i, align 4
  %preinc.us50.i = add nsw i32 %x.042.us47.i, 1
  %exitcond.i = icmp eq i32 %preinc.us50.i, %fun_call6118.i
  br i1 %exitcond.i, label %for_cond76.end_for79_crit_edge.us.i.loopexit131, label %for77.us46.i, !llvm.loop !12

for_cond76.end_for79_crit_edge.us.i.loopexit:     ; preds = %for77.us.us.i
  br label %for_cond76.end_for79_crit_edge.us.i

for_cond76.end_for79_crit_edge.us.i.loopexit131:  ; preds = %for77.us46.i
  br label %for_cond76.end_for79_crit_edge.us.i

for_cond76.end_for79_crit_edge.us.i:              ; preds = %for_cond76.end_for79_crit_edge.us.i.loopexit131, %for_cond76.end_for79_crit_edge.us.i.loopexit, %middle.block88, %middle.block53
  %struct_arrow95.us.i = load i32, i32* %struct_field_ptr66.i, align 4
  %83 = sext i32 %struct_arrow95.us.i to i64
  %ptr_add96.us.i = getelementptr i8, i8* %row.044.us.i, i64 %83
  %preinc98.us.i = add nsw i32 %y.045.us.i, 1
  %exitcond57.i = icmp eq i32 %preinc98.us.i, %fun_call4632.i
  br i1 %exitcond57.i, label %render_player.exit.loopexit, label %for.us.i

for77.us.us.i:                                    ; preds = %for77.us.us.i.preheader129, %for77.us.us.i
  %x.042.us.us.i = phi i32 [ %preinc.us.us.i, %for77.us.us.i ], [ %x.042.us.us.i.ph, %for77.us.us.i.preheader129 ]
  %pixel.041.us.us.i = phi i32* [ %ptr_post_inc.us.us.i, %for77.us.us.i ], [ %pixel.041.us.us.i.ph, %for77.us.us.i.preheader129 ]
  %icmp_tmp85.us.us.i = icmp eq i32 %x.042.us.us.i, %struct_arrow
  %ptr_post_inc.us.us.i = getelementptr i32, i32* %pixel.041.us.us.i, i64 1
  %.color.2.us.us.i = select i1 %icmp_tmp85.us.us.i, i32 16777215, i32 %color.2.i
  store i32 %.color.2.us.us.i, i32* %pixel.041.us.us.i, align 4
  %preinc.us.us.i = add nsw i32 %x.042.us.us.i, 1
  %exitcond56.i = icmp eq i32 %preinc.us.us.i, %fun_call6118.i
  br i1 %exitcond56.i, label %for_cond76.end_for79_crit_edge.us.i.loopexit, label %for77.us.us.i, !llvm.loop !13

render_player.exit.loopexit:                      ; preds = %for_cond76.end_for79_crit_edge.us.i
  br label %render_player.exit

render_player.exit:                               ; preds = %render_player.exit.loopexit, %endif.i
  %struct_field_ptr.i.i = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %input, i64 0, i32 1, i32 1
  store i1 false, i1* %struct_field_ptr.i.i, align 1
  %struct_field_ptr1.i.i = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %input, i64 0, i32 1, i32 2
  store i1 false, i1* %struct_field_ptr1.i.i, align 1
  %struct_field_ptr.i11.i = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %input, i64 0, i32 4, i32 1
  store i1 false, i1* %struct_field_ptr.i11.i, align 1
  %struct_field_ptr1.i12.i = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %input, i64 0, i32 4, i32 2
  store i1 false, i1* %struct_field_ptr1.i12.i, align 1
  %struct_field_ptr.i9.i = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %input, i64 0, i32 2, i32 1
  store i1 false, i1* %struct_field_ptr.i9.i, align 1
  %struct_field_ptr1.i10.i = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %input, i64 0, i32 2, i32 2
  store i1 false, i1* %struct_field_ptr1.i10.i, align 1
  %struct_field_ptr.i7.i = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %input, i64 0, i32 3, i32 1
  store i1 false, i1* %struct_field_ptr.i7.i, align 1
  %struct_field_ptr1.i8.i = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %input, i64 0, i32 3, i32 2
  store i1 false, i1* %struct_field_ptr1.i8.i, align 1
  store i1 false, i1* %struct_field_ptr1.i3, align 1
  %struct_field_ptr1.i6.i = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %input, i64 0, i32 5, i32 2
  store i1 false, i1* %struct_field_ptr1.i6.i, align 1
  %struct_field_ptr.i3.i = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %input, i64 0, i32 6, i32 1
  store i1 false, i1* %struct_field_ptr.i3.i, align 1
  %struct_field_ptr1.i4.i = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %input, i64 0, i32 6, i32 2
  store i1 false, i1* %struct_field_ptr1.i4.i, align 1
  %struct_field_ptr.i1.i = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %input, i64 0, i32 7, i32 1
  store i1 false, i1* %struct_field_ptr.i1.i, align 1
  %struct_field_ptr1.i2.i = getelementptr inbounds <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>, <{ <{ float, float }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, <{ i1, i1, i1 }>, i32, i32, i32, i1 }>* %input, i64 0, i32 7, i32 2
  store i1 false, i1* %struct_field_ptr1.i2.i, align 1
  ret i1 %result.0
}

; Function Attrs: nounwind
define dllexport void @game_output_sound(<{ i8*, i64, i8*, i64, i8* }>* nocapture readonly %game_memory, <{ i16*, i32, i32, float }>* nocapture readonly %sound_output) #0 {
vars:
  %arr_elem_alloca1.i.i.i = alloca [64 x i8], align 1
  %arr_elem_alloca1.i = alloca i16, align 2
  %arr_elem_alloca3.i = alloca i8, align 1
  %arr_elem_alloca4 = alloca [6 x i8], align 1
  %arr_elem_alloca4.sub = getelementptr inbounds [6 x i8], [6 x i8]* %arr_elem_alloca4, i64 0, i64 0
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %arr_elem_alloca4.sub, i8* getelementptr inbounds ([7 x i8], [7 x i8]* @str.7, i64 0, i64 0), i32 6, i32 1, i1 false)
  %struct_field_ptr.i = getelementptr inbounds <{ i8*, i64, i8*, i64, i8* }>, <{ i8*, i64, i8*, i64, i8* }>* %game_memory, i64 0, i32 2
  %0 = bitcast i8** %struct_field_ptr.i to <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>**
  %struct_arrow1.i = load <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>*, <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>** %0, align 8
  %struct_field_ptr1.i = getelementptr inbounds <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow1.i, i64 0, i32 0
  %struct_field.i = load i1, i1* %struct_field_ptr1.i, align 1
  %struct_field_ptr4.phi.trans.insert = getelementptr inbounds <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow1.i, i64 0, i32 7
  br i1 %struct_field.i, label %vars.get_game_state.exit_crit_edge, label %then.i

vars.get_game_state.exit_crit_edge:               ; preds = %vars
  %struct_field5.pre = load float, float* %struct_field_ptr4.phi.trans.insert, align 4
  %struct_field_ptr7.phi.trans.insert = getelementptr inbounds <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow1.i, i64 0, i32 8
  %struct_field8.pre = load float, float* %struct_field_ptr7.phi.trans.insert, align 4
  br label %get_game_state.exit

then.i:                                           ; preds = %vars
  store float 2.560000e+02, float* %struct_field_ptr4.phi.trans.insert, align 4
  %struct_field_ptr5.i = getelementptr inbounds <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow1.i, i64 0, i32 8
  store float 1.280000e+02, float* %struct_field_ptr5.i, align 4
  %struct_field_ptr7.i = getelementptr inbounds <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow1.i, i64 0, i32 6
  store float 0x3F947AE140000000, float* %struct_field_ptr7.i, align 4
  store i1 true, i1* %struct_field_ptr1.i, align 1
  %struct_field_ptr11.i = getelementptr inbounds <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow1.i, i64 0, i32 3
  store float 2.000000e+02, float* %struct_field_ptr11.i, align 4
  %struct_field_ptr13.i = getelementptr inbounds <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow1.i, i64 0, i32 4
  store float 2.000000e+02, float* %struct_field_ptr13.i, align 4
  br label %get_game_state.exit

get_game_state.exit:                              ; preds = %vars.get_game_state.exit_crit_edge, %then.i
  %struct_field8 = phi float [ %struct_field8.pre, %vars.get_game_state.exit_crit_edge ], [ 1.280000e+02, %then.i ]
  %struct_field5 = phi float [ %struct_field5.pre, %vars.get_game_state.exit_crit_edge ], [ 2.560000e+02, %then.i ]
  %struct_field_ptr = getelementptr inbounds <{ i16*, i32, i32, float }>, <{ i16*, i32, i32, float }>* %sound_output, i64 0, i32 0
  %struct_arrow = load i16*, i16** %struct_field_ptr, align 8
  %struct_field_ptr1 = getelementptr inbounds <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow1.i, i64 0, i32 10
  %struct_field = load float, float* %struct_field_ptr1, align 4
  %fmul_tmp = fmul float %struct_field, 0x401921FB60000000
  %fsub_tmp = fsub float %struct_field5, %struct_field8
  %fmul_tmp9 = fmul float %fmul_tmp, %fsub_tmp
  %struct_field_ptr11 = getelementptr inbounds <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow1.i, i64 0, i32 9
  %struct_field12 = load float, float* %struct_field_ptr11, align 4
  %fadd_tmp = fadd float %struct_field12, %fmul_tmp9
  store float %struct_field8, float* %struct_field_ptr4.phi.trans.insert, align 4
  %struct_field_ptr18 = getelementptr inbounds <{ i16*, i32, i32, float }>, <{ i16*, i32, i32, float }>* %sound_output, i64 0, i32 2
  %struct_arrow19 = load i32, i32* %struct_field_ptr18, align 4
  %int_to_float_cast = sitofp i32 %struct_arrow19 to float
  %fdiv_tmp = fdiv float 1.000000e+00, %int_to_float_cast
  %struct_field_ptr21 = getelementptr inbounds <{ i16*, i32, i32, float }>, <{ i16*, i32, i32, float }>* %sound_output, i64 0, i32 1
  %struct_arrow228 = load i32, i32* %struct_field_ptr21, align 4
  %icmp_tmp9 = icmp sgt i32 %struct_arrow228, 0
  br i1 %icmp_tmp9, label %for.lr.ph, label %end_for

for.lr.ph:                                        ; preds = %get_game_state.exit
  %struct_field_ptr30 = getelementptr inbounds <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow1.i, i64 0, i32 5
  %struct_field_ptr33 = getelementptr inbounds <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow1.i, i64 0, i32 6
  br label %for

for:                                              ; preds = %for.for_crit_edge, %for.lr.ph
  %struct_field39 = phi float [ %struct_field8, %for.lr.ph ], [ %struct_field39.pre, %for.for_crit_edge ]
  %struct_arrow2213 = phi i32 [ %struct_arrow228, %for.lr.ph ], [ %struct_arrow22, %for.for_crit_edge ]
  %sample.012 = phi i16* [ %struct_arrow, %for.lr.ph ], [ %ptr_post_inc53, %for.for_crit_edge ]
  %t_sine.011 = phi float [ %struct_field, %for.lr.ph ], [ %fadd_tmp57, %for.for_crit_edge ]
  %i.010 = phi i32 [ 0, %for.lr.ph ], [ %preinc, %for.for_crit_edge ]
  %int_to_float_cast24 = sitofp i32 %i.010 to float
  %sub_tmp = add i32 %struct_arrow2213, -1
  %int_to_float_cast27 = sitofp i32 %sub_tmp to float
  %fdiv_tmp28 = fdiv float %int_to_float_cast24, %int_to_float_cast27
  %struct_field31 = load float, float* %struct_field_ptr30, align 4
  %struct_field34 = load float, float* %struct_field_ptr33, align 4
  %fsub_tmp.i = fsub float %struct_field34, %struct_field31
  %fmul_tmp.i = fmul float %fdiv_tmp28, %fsub_tmp.i
  %fadd_tmp.i = fadd float %struct_field31, %fmul_tmp.i
  %fmul_tmp41 = fmul float %struct_field39, 0x401921FB60000000
  %fmul_tmp43 = fmul float %t_sine.011, %fmul_tmp41
  %fmul_tmp45 = fmul float %fadd_tmp.i, 3.276700e+04
  %fadd_tmp48 = fadd float %fadd_tmp, %fmul_tmp43
  %fdiv_tmp2.i = fdiv float %fadd_tmp48, 0x401921FB60000000
  %fadd_tmp.i6 = fadd float %fdiv_tmp2.i, 5.000000e-01
  %fun_call5.i = tail call float @llvm.floor.f32(float %fadd_tmp.i6) #5
  %fsub_tmp.i7 = fsub float %fdiv_tmp2.i, %fun_call5.i
  %fmul_tmp7.i = fmul float %fsub_tmp.i7, 0x401E5B9D00000000
  %fun_call9.i = tail call float @llvm.fabs.f32(float %fsub_tmp.i7) #5
  %fsub_tmp10.i = fsub float 5.000000e-01, %fun_call9.i
  %fmul_tmp11.i = fmul float %fmul_tmp7.i, %fsub_tmp10.i
  %fun_call14.i = tail call float @llvm.fabs.f32(float %fmul_tmp11.i) #5
  %fadd_tmp15.i = fadd float %fun_call14.i, 0x3FFA243900000000
  %fmul_tmp16.i = fmul float %fmul_tmp11.i, %fadd_tmp15.i
  %fmul_tmp50 = fmul float %fmul_tmp45, %fmul_tmp16.i
  %int_cast = fptosi float %fmul_tmp50 to i16
  %ptr_post_inc = getelementptr i16, i16* %sample.012, i64 1
  store i16 %int_cast, i16* %sample.012, align 2
  store i16 %int_cast, i16* %ptr_post_inc, align 2
  %fadd_tmp57 = fadd float %fdiv_tmp, %t_sine.011
  %preinc = add i32 %i.010, 1
  %struct_arrow22 = load i32, i32* %struct_field_ptr21, align 4
  %icmp_tmp = icmp slt i32 %preinc, %struct_arrow22
  br i1 %icmp_tmp, label %for.for_crit_edge, label %end_for.loopexit

for.for_crit_edge:                                ; preds = %for
  %ptr_post_inc53 = getelementptr i16, i16* %sample.012, i64 2
  %struct_field39.pre = load float, float* %struct_field_ptr4.phi.trans.insert, align 4
  br label %for

end_for.loopexit:                                 ; preds = %for
  %fadd_tmp57.lcssa = phi float [ %fadd_tmp57, %for ]
  br label %end_for

end_for:                                          ; preds = %end_for.loopexit, %get_game_state.exit
  %t_sine.0.lcssa = phi float [ %struct_field, %get_game_state.exit ], [ %fadd_tmp57.lcssa, %end_for.loopexit ]
  %fcmp_tmp = fcmp ogt float %t_sine.0.lcssa, 1.200000e+02
  br i1 %fcmp_tmp, label %then, label %endif

then:                                             ; preds = %end_for
  %struct_field62 = load float, float* %struct_field_ptr1, align 4
  %1 = bitcast i16* %arr_elem_alloca1.i to i8*
  call void @llvm.lifetime.start(i64 2, i8* %1)
  call void @llvm.lifetime.start(i64 1, i8* nonnull %arr_elem_alloca3.i)
  store i16 8250, i16* %arr_elem_alloca1.i, align 2
  store i8 10, i8* %arr_elem_alloca3.i, align 1
  %console_output_handle.i.i = load i64, i64* @console_output_handle, align 8
  %fun_call.i.i = call i32 @WriteFile(i64 %console_output_handle.i.i, i8* %arr_elem_alloca4.sub, i32 6, i32* null, i8* null) #5
  %console_output_handle.i2.i = load i64, i64* @console_output_handle, align 8
  %fun_call.i4.i = call i32 @WriteFile(i64 %console_output_handle.i2.i, i8* %1, i32 2, i32* null, i8* null) #5
  %fp_to_fp_cast.i.i = fpext float %struct_field62 to double
  %2 = getelementptr inbounds [64 x i8], [64 x i8]* %arr_elem_alloca1.i.i.i, i64 0, i64 0
  call void @llvm.lifetime.start(i64 64, i8* %2) #5
  call void @llvm.memcpy.p0i8.p0i8.i32(i8* %2, i8* getelementptr inbounds ([65 x i8], [65 x i8]* @str.1, i64 0, i64 0), i32 64, i32 1, i1 false) #5
  %fcmp_tmp.i.i.i = fcmp olt float %struct_field62, 0.000000e+00
  br i1 %fcmp_tmp.i.i.i, label %then.i.i.i, label %endif.i.i.i

then.i.i.i:                                       ; preds = %then
  %fneg_tmp.i.i.i = fsub double -0.000000e+00, %fp_to_fp_cast.i.i
  store i8 45, i8* %2, align 1
  br label %endif.i.i.i

endif.i.i.i:                                      ; preds = %then.i.i.i, %then
  %pd.sroa.10.0.i.i.i = phi i32 [ 1, %then.i.i.i ], [ 0, %then ]
  %v.0.i.i.i = phi double [ %fneg_tmp.i.i.i, %then.i.i.i ], [ %fp_to_fp_cast.i.i, %then ]
  %int_cast.i.i.i = fptoui double %v.0.i.i.i to i64
  %3 = zext i32 %pd.sroa.10.0.i.i.i to i64
  %arr_elem_ptr4.i.i.i.i = load i8*, i8** getelementptr inbounds (<{ i32, i8* }>, <{ i32, i8* }>* @decimal_digits, i64 0, i32 1), align 8
  %urem_tmp.i27.i.i.i = urem i64 %int_cast.i.i.i, 10
  %gep_arr_elem5.i28.i.i.i = getelementptr i8, i8* %arr_elem_ptr4.i.i.i.i, i64 %urem_tmp.i27.i.i.i
  %arr_elem.i29.i.i.i = load i8, i8* %gep_arr_elem5.i28.i.i.i, align 1
  %postinc.i.i30.i.i.i = add nuw nsw i32 %pd.sroa.10.0.i.i.i, 1
  %gep_arr_elem.i.i31.i.i.i = getelementptr [64 x i8], [64 x i8]* %arr_elem_alloca1.i.i.i, i64 0, i64 %3
  store i8 %arr_elem.i29.i.i.i, i8* %gep_arr_elem.i.i31.i.i.i, align 1
  %4 = icmp ult i64 %int_cast.i.i.i, 10
  br i1 %4, label %while_end.i.i.i.i, label %while_cond.while_cond_crit_edge.i.i.i.i.preheader

while_cond.while_cond_crit_edge.i.i.i.i.preheader: ; preds = %endif.i.i.i
  br label %while_cond.while_cond_crit_edge.i.i.i.i

while_cond.while_cond_crit_edge.i.i.i.i:          ; preds = %while_cond.while_cond_crit_edge.i.i.i.i.preheader, %while_cond.while_cond_crit_edge.i.i.i.i
  %postinc.i.i33.i.i.i = phi i32 [ %postinc.i.i.i.i.i, %while_cond.while_cond_crit_edge.i.i.i.i ], [ %postinc.i.i30.i.i.i, %while_cond.while_cond_crit_edge.i.i.i.i.preheader ]
  %v.0.i32.i.i.i = phi i64 [ %div_tmp.i.i.i.i, %while_cond.while_cond_crit_edge.i.i.i.i ], [ %int_cast.i.i.i, %while_cond.while_cond_crit_edge.i.i.i.i.preheader ]
  %div_tmp.i.i.i.i = udiv i64 %v.0.i32.i.i.i, 10
  %urem_tmp.i.i.i.i = urem i64 %div_tmp.i.i.i.i, 10
  %gep_arr_elem5.i.i.i.i = getelementptr i8, i8* %arr_elem_ptr4.i.i.i.i, i64 %urem_tmp.i.i.i.i
  %arr_elem.i.i.i.i = load i8, i8* %gep_arr_elem5.i.i.i.i, align 1
  %postinc.i.i.i.i.i = add i32 %postinc.i.i33.i.i.i, 1
  %5 = sext i32 %postinc.i.i33.i.i.i to i64
  %gep_arr_elem.i.i.i.i.i = getelementptr [64 x i8], [64 x i8]* %arr_elem_alloca1.i.i.i, i64 0, i64 %5
  store i8 %arr_elem.i.i.i.i, i8* %gep_arr_elem.i.i.i.i.i, align 1
  %6 = icmp ult i64 %v.0.i32.i.i.i, 100
  br i1 %6, label %while_end.i.i.i.i.loopexit, label %while_cond.while_cond_crit_edge.i.i.i.i

while_end.i.i.i.i.loopexit:                       ; preds = %while_cond.while_cond_crit_edge.i.i.i.i
  %postinc.i.i.i.i.i.lcssa = phi i32 [ %postinc.i.i.i.i.i, %while_cond.while_cond_crit_edge.i.i.i.i ]
  %postinc.i.i33.i.i.i.lcssa = phi i32 [ %postinc.i.i33.i.i.i, %while_cond.while_cond_crit_edge.i.i.i.i ]
  br label %while_end.i.i.i.i

while_end.i.i.i.i:                                ; preds = %while_end.i.i.i.i.loopexit, %endif.i.i.i
  %postinc.i.i.lcssa.i.i.i = phi i32 [ %postinc.i.i30.i.i.i, %endif.i.i.i ], [ %postinc.i.i.i.i.i.lcssa, %while_end.i.i.i.i.loopexit ]
  %postinc_load.i.i.lcssa.i.i.i = phi i32 [ %pd.sroa.10.0.i.i.i, %endif.i.i.i ], [ %postinc.i.i33.i.i.i.lcssa, %while_end.i.i.i.i.loopexit ]
  %7 = sext i32 %postinc.i.i.lcssa.i.i.i to i64
  %gep_arr_elem14.i.i.i.i = getelementptr [64 x i8], [64 x i8]* %arr_elem_alloca1.i.i.i, i64 0, i64 %7
  %icmp_tmp203.i.i.i.i = icmp ult i8* %gep_arr_elem.i.i31.i.i.i, %gep_arr_elem14.i.i.i.i
  br i1 %icmp_tmp203.i.i.i.i, label %while16.i.i.i.i.preheader, label %debug_print_f32.exit

while16.i.i.i.i.preheader:                        ; preds = %while_end.i.i.i.i
  br label %while16.i.i.i.i

while16.i.i.i.i:                                  ; preds = %while16.i.i.i.i.preheader, %while16.i.i.i.i
  %end.05.i.i.i.i = phi i8* [ %ptr_pre_dec.i.i.i.i, %while16.i.i.i.i ], [ %gep_arr_elem14.i.i.i.i, %while16.i.i.i.i.preheader ]
  %start.04.i.i.i.i = phi i8* [ %ptr_pre_inc.i.i.i.i, %while16.i.i.i.i ], [ %gep_arr_elem.i.i31.i.i.i, %while16.i.i.i.i.preheader ]
  %ptr_pre_dec.i.i.i.i = getelementptr i8, i8* %end.05.i.i.i.i, i64 -1
  %deref.i.i.i.i = load i8, i8* %ptr_pre_dec.i.i.i.i, align 1
  %deref24.i.i.i.i = load i8, i8* %start.04.i.i.i.i, align 1
  store i8 %deref24.i.i.i.i, i8* %ptr_pre_dec.i.i.i.i, align 1
  store i8 %deref.i.i.i.i, i8* %start.04.i.i.i.i, align 1
  %ptr_pre_inc.i.i.i.i = getelementptr i8, i8* %start.04.i.i.i.i, i64 1
  %icmp_tmp20.i.i.i.i = icmp ult i8* %ptr_pre_inc.i.i.i.i, %ptr_pre_dec.i.i.i.i
  br i1 %icmp_tmp20.i.i.i.i, label %while16.i.i.i.i, label %u64_to_ascii.exit.loopexit.i.i.i

u64_to_ascii.exit.loopexit.i.i.i:                 ; preds = %while16.i.i.i.i
  %arr_elem_ptr.pre.i.i.i = load i8*, i8** getelementptr inbounds (<{ i32, i8* }>, <{ i32, i8* }>* @decimal_digits, i64 0, i32 1), align 8
  br label %debug_print_f32.exit

debug_print_f32.exit:                             ; preds = %while_end.i.i.i.i, %u64_to_ascii.exit.loopexit.i.i.i
  %arr_elem_ptr.i.i.i = phi i8* [ %arr_elem_ptr.pre.i.i.i, %u64_to_ascii.exit.loopexit.i.i.i ], [ %arr_elem_ptr4.i.i.i.i, %while_end.i.i.i.i ]
  %int_to_float_cast.i.i.i = sitofp i64 %int_cast.i.i.i to double
  %fsub_tmp.i.i.i = fsub double %v.0.i.i.i, %int_to_float_cast.i.i.i
  %postinc.i14.i.i.i = add i32 %postinc_load.i.i.lcssa.i.i.i, 2
  store i8 46, i8* %gep_arr_elem14.i.i.i.i, align 1
  %8 = add i32 %postinc_load.i.i.lcssa.i.i.i, 6
  %fmul_tmp.i.i.i = fmul double %fsub_tmp.i.i.i, 1.000000e+01
  %int_cast15.i.i.i = fptosi double %fmul_tmp.i.i.i to i32
  %int_to_float_cast19.i.i.i = sitofp i32 %int_cast15.i.i.i to double
  %fsub_tmp20.i.i.i = fsub double %fmul_tmp.i.i.i, %int_to_float_cast19.i.i.i
  %9 = sext i32 %int_cast15.i.i.i to i64
  %gep_arr_elem.i.i.i = getelementptr i8, i8* %arr_elem_ptr.i.i.i, i64 %9
  %arr_elem.i.i.i = load i8, i8* %gep_arr_elem.i.i.i, align 1
  %postinc.i8.i.i.i = add i32 %postinc_load.i.i.lcssa.i.i.i, 3
  %10 = sext i32 %postinc.i14.i.i.i to i64
  %gep_arr_elem.i11.i.i.i = getelementptr [64 x i8], [64 x i8]* %arr_elem_alloca1.i.i.i, i64 0, i64 %10
  store i8 %arr_elem.i.i.i, i8* %gep_arr_elem.i11.i.i.i, align 1
  %fmul_tmp.1.i.i.i = fmul double %fsub_tmp20.i.i.i, 1.000000e+01
  %int_cast15.1.i.i.i = fptosi double %fmul_tmp.1.i.i.i to i32
  %int_to_float_cast19.1.i.i.i = sitofp i32 %int_cast15.1.i.i.i to double
  %fsub_tmp20.1.i.i.i = fsub double %fmul_tmp.1.i.i.i, %int_to_float_cast19.1.i.i.i
  %11 = sext i32 %int_cast15.1.i.i.i to i64
  %gep_arr_elem.1.i.i.i = getelementptr i8, i8* %arr_elem_ptr.i.i.i, i64 %11
  %arr_elem.1.i.i.i = load i8, i8* %gep_arr_elem.1.i.i.i, align 1
  %postinc.i8.1.i.i.i = add i32 %postinc_load.i.i.lcssa.i.i.i, 4
  %12 = sext i32 %postinc.i8.i.i.i to i64
  %gep_arr_elem.i11.1.i.i.i = getelementptr [64 x i8], [64 x i8]* %arr_elem_alloca1.i.i.i, i64 0, i64 %12
  store i8 %arr_elem.1.i.i.i, i8* %gep_arr_elem.i11.1.i.i.i, align 1
  %fmul_tmp.2.i.i.i = fmul double %fsub_tmp20.1.i.i.i, 1.000000e+01
  %int_cast15.2.i.i.i = fptosi double %fmul_tmp.2.i.i.i to i32
  %int_to_float_cast19.2.i.i.i = sitofp i32 %int_cast15.2.i.i.i to double
  %fsub_tmp20.2.i.i.i = fsub double %fmul_tmp.2.i.i.i, %int_to_float_cast19.2.i.i.i
  %13 = sext i32 %int_cast15.2.i.i.i to i64
  %gep_arr_elem.2.i.i.i = getelementptr i8, i8* %arr_elem_ptr.i.i.i, i64 %13
  %arr_elem.2.i.i.i = load i8, i8* %gep_arr_elem.2.i.i.i, align 1
  %postinc.i8.2.i.i.i = add i32 %postinc_load.i.i.lcssa.i.i.i, 5
  %14 = sext i32 %postinc.i8.1.i.i.i to i64
  %gep_arr_elem.i11.2.i.i.i = getelementptr [64 x i8], [64 x i8]* %arr_elem_alloca1.i.i.i, i64 0, i64 %14
  store i8 %arr_elem.2.i.i.i, i8* %gep_arr_elem.i11.2.i.i.i, align 1
  %fmul_tmp.3.i.i.i = fmul double %fsub_tmp20.2.i.i.i, 1.000000e+01
  %int_cast15.3.i.i.i = fptosi double %fmul_tmp.3.i.i.i to i32
  %15 = sext i32 %int_cast15.3.i.i.i to i64
  %gep_arr_elem.3.i.i.i = getelementptr i8, i8* %arr_elem_ptr.i.i.i, i64 %15
  %arr_elem.3.i.i.i = load i8, i8* %gep_arr_elem.3.i.i.i, align 1
  %16 = sext i32 %postinc.i8.2.i.i.i to i64
  %gep_arr_elem.i11.3.i.i.i = getelementptr [64 x i8], [64 x i8]* %arr_elem_alloca1.i.i.i, i64 0, i64 %16
  store i8 %arr_elem.3.i.i.i, i8* %gep_arr_elem.i11.3.i.i.i, align 1
  %console_output_handle.i.i.i.i = load i64, i64* @console_output_handle, align 8
  %fun_call.i.i.i.i = call i32 @WriteFile(i64 %console_output_handle.i.i.i.i, i8* nonnull %2, i32 %8, i32* null, i8* null) #5
  call void @llvm.lifetime.end(i64 64, i8* %2) #5
  %console_output_handle.i5.i = load i64, i64* @console_output_handle, align 8
  %fun_call.i7.i = call i32 @WriteFile(i64 %console_output_handle.i5.i, i8* nonnull %arr_elem_alloca3.i, i32 1, i32* null, i8* null) #5
  call void @llvm.lifetime.end(i64 2, i8* %1)
  call void @llvm.lifetime.end(i64 1, i8* nonnull %arr_elem_alloca3.i)
  br label %endif

endif:                                            ; preds = %debug_print_f32.exit, %end_for
  store float %t_sine.0.lcssa, float* %struct_field_ptr1, align 4
  store float %fadd_tmp, float* %struct_field_ptr11, align 4
  %struct_field_ptr70 = getelementptr inbounds <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow1.i, i64 0, i32 5
  %struct_field_ptr72 = getelementptr inbounds <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>, <{ i1, float, float, float, float, float, float, float, float, float, float, i32, i1, float, float, float }>* %struct_arrow1.i, i64 0, i32 6
  %17 = bitcast float* %struct_field_ptr72 to i32*
  %struct_field735 = load i32, i32* %17, align 4
  %18 = bitcast float* %struct_field_ptr70 to i32*
  store i32 %struct_field735, i32* %18, align 4
  ret void
}

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

!0 = distinct !{!0, !1, !2}
!1 = !{!"llvm.loop.vectorize.width", i32 1}
!2 = !{!"llvm.loop.interleave.count", i32 1}
!3 = distinct !{!3, !4}
!4 = !{!"llvm.loop.unroll.disable"}
!5 = distinct !{!5, !6, !1, !2}
!6 = !{!"llvm.loop.unroll.runtime.disable"}
!7 = distinct !{!7, !1, !2}
!8 = distinct !{!8, !6, !1, !2}
!9 = distinct !{!9, !1, !2}
!10 = distinct !{!10, !4}
!11 = distinct !{!11, !1, !2}
!12 = distinct !{!12, !6, !1, !2}
!13 = distinct !{!13, !6, !1, !2}
