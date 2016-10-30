; ModuleID = 'output.ll'
source_filename = "output.ll"
target datalayout = "e-m:w-i64:64-f80:128-n8:16:32:64-S128"
target triple = "x86_64-pc-windows-msvc"

@running = internal unnamed_addr global i1 false
@backbuffer = internal global <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }> zeroinitializer
@window.0 = internal unnamed_addr global i64 0
@window.1 = internal unnamed_addr global i64 0
@window.2 = internal unnamed_addr global i32 0, align 8
@window.3 = internal unnamed_addr global i32 0

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
declare i64 @GetDC(i64) #0

; Function Attrs: nounwind
declare i8* @VirtualAlloc(i8*, i64, i32, i32) #0

; Function Attrs: nounwind
declare i32 @VirtualFree(i8*, i64, i32) #0

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
  %GetClientRect.i = call i32 @GetClientRect(i64 %struct_field.i, <{ i32, i32, i32, i32 }>* nonnull %rect.i) #2
  %struct_field_ptr.i = getelementptr inbounds <{ i32, i32, i32, i32 }>, <{ i32, i32, i32, i32 }>* %rect.i, i64 0, i32 2
  %1 = bitcast i32* %struct_field_ptr.i to i64*
  %struct_field1.i = load i64, i64* %1, align 8
  %2 = trunc i64 %struct_field1.i to i32
  %3 = bitcast <{ i32, i32, i32, i32 }>* %rect.i to i64*
  %struct_field3.i = load i64, i64* %3, align 8
  %4 = trunc i64 %struct_field3.i to i32
  %sub_tmp.i = sub i32 %2, %4
  store i32 %sub_tmp.i, i32* @window.2, align 8
  %5 = lshr i64 %struct_field1.i, 32
  %6 = trunc i64 %5 to i32
  %7 = lshr i64 %struct_field3.i, 32
  %8 = trunc i64 %7 to i32
  %sub_tmp8.i = sub i32 %6, %8
  store i32 %sub_tmp8.i, i32* @window.3, align 4
  call void @llvm.lifetime.end(i64 16, i8* %0)
  br label %endif

elif_0_then:                                      ; preds = %vars
  store i1 false, i1* @running, align 1
  br label %endif

elif_1_then:                                      ; preds = %vars
  store i1 false, i1* @running, align 1
  br label %endif

elif_2_then:                                      ; preds = %vars
  %BeginPaint = call i64 @BeginPaint(i64 %window_handle, <{ i64, i32, <{ i32, i32, i32, i32 }>, i32, i32, i64, i64, i64, i64 }>* nonnull %paint)
  %struct_field.i1 = load i64, i64* @window.1, align 8
  %struct_field1.i2 = load i32, i32* @window.2, align 8
  %struct_field2.i = load i32, i32* @window.3, align 4
  %struct_arrow.i = load i32, i32* getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i64 0, i32 2), align 4
  %struct_arrow4.i = load i32, i32* getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i64 0, i32 3), align 8
  %struct_arrow6.i = load i8*, i8** getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i64 0, i32 1), align 8
  %StretchDIBits.i = call i32 @StretchDIBits(i64 %struct_field.i1, i32 0, i32 0, i32 %struct_field1.i2, i32 %struct_field2.i, i32 0, i32 0, i32 %struct_arrow.i, i32 %struct_arrow4.i, i8* %struct_arrow6.i, <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>* getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i64 0, i32 0), i32 0, i32 13369376) #2
  %EndPaint = call i32 @EndPaint(i64 %window_handle, <{ i64, i32, <{ i32, i32, i32, i32 }>, i32, i32, i64, i64, i64, i64 }>* nonnull %paint)
  br label %endif

else:                                             ; preds = %vars
  %DefWindowProcA = tail call i64 @DefWindowProcA(i64 %window_handle, i32 %message, i64 %w_param, i64 %l_param)
  br label %endif

endif:                                            ; preds = %else, %elif_2_then, %elif_1_then, %elif_0_then, %then
  %result.0 = phi i64 [ %DefWindowProcA, %else ], [ 0, %elif_2_then ], [ 0, %elif_1_then ], [ 0, %elif_0_then ], [ 0, %then ]
  ret i64 %result.0
}

; Function Attrs: nounwind
define void @__init() #0 {
vars:
  %rect.i.i = alloca <{ i32, i32, i32, i32 }>, align 8
  %arr_elem_alloca5.i = alloca [24 x i8], align 16
  %arr_elem_alloca36.i = alloca [22 x i8], align 16
  %window_class.i = alloca <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>, align 8
  %msg.i = alloca <{ i64, i32, i64, i64, i32, <{ i32, i32 }>, i64 }>, align 8
  %GetStdHandle = tail call i64 @GetStdHandle(i32 -11)
  %GetStdHandle1 = tail call i64 @GetStdHandle(i32 -10)
  %0 = getelementptr inbounds [24 x i8], [24 x i8]* %arr_elem_alloca5.i, i64 0, i64 0
  call void @llvm.lifetime.start(i64 24, i8* %0)
  %1 = getelementptr inbounds [22 x i8], [22 x i8]* %arr_elem_alloca36.i, i64 0, i64 0
  call void @llvm.lifetime.start(i64 22, i8* %1)
  %2 = bitcast <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>* %window_class.i to i8*
  call void @llvm.lifetime.start(i64 80, i8* %2)
  %3 = bitcast <{ i64, i32, i64, i64, i32, <{ i32, i32 }>, i64 }>* %msg.i to i8*
  call void @llvm.lifetime.start(i64 48, i8* %3)
  %4 = bitcast [24 x i8]* %arr_elem_alloca5.i to <16 x i8>*
  store <16 x i8> <i8 80, i8 114, i8 97, i8 103, i8 109, i8 97, i8 83, i8 99, i8 114, i8 105, i8 112, i8 116, i8 87, i8 105, i8 110, i8 100>, <16 x i8>* %4, align 16
  %array_elem_16.i = getelementptr [24 x i8], [24 x i8]* %arr_elem_alloca5.i, i64 0, i64 16
  store i8 111, i8* %array_elem_16.i, align 16
  %array_elem_17.i = getelementptr [24 x i8], [24 x i8]* %arr_elem_alloca5.i, i64 0, i64 17
  store i8 119, i8* %array_elem_17.i, align 1
  %array_elem_18.i = getelementptr [24 x i8], [24 x i8]* %arr_elem_alloca5.i, i64 0, i64 18
  store i8 67, i8* %array_elem_18.i, align 2
  %array_elem_19.i = getelementptr [24 x i8], [24 x i8]* %arr_elem_alloca5.i, i64 0, i64 19
  store i8 108, i8* %array_elem_19.i, align 1
  %array_elem_20.i = getelementptr [24 x i8], [24 x i8]* %arr_elem_alloca5.i, i64 0, i64 20
  store i8 97, i8* %array_elem_20.i, align 4
  %array_elem_21.i = getelementptr [24 x i8], [24 x i8]* %arr_elem_alloca5.i, i64 0, i64 21
  store i8 115, i8* %array_elem_21.i, align 1
  %array_elem_22.i = getelementptr [24 x i8], [24 x i8]* %arr_elem_alloca5.i, i64 0, i64 22
  store i8 115, i8* %array_elem_22.i, align 2
  %array_elem_23.i = getelementptr [24 x i8], [24 x i8]* %arr_elem_alloca5.i, i64 0, i64 23
  store i8 0, i8* %array_elem_23.i, align 1
  %5 = bitcast [22 x i8]* %arr_elem_alloca36.i to <16 x i8>*
  store <16 x i8> <i8 104, i8 97, i8 110, i8 100, i8 109, i8 97, i8 100, i8 101, i8 45, i8 112, i8 114, i8 97, i8 103, i8 109, i8 97, i8 115>, <16 x i8>* %5, align 16
  %array_elem_1625.i = getelementptr [22 x i8], [22 x i8]* %arr_elem_alloca36.i, i64 0, i64 16
  store i8 99, i8* %array_elem_1625.i, align 16
  %array_elem_1726.i = getelementptr [22 x i8], [22 x i8]* %arr_elem_alloca36.i, i64 0, i64 17
  store i8 114, i8* %array_elem_1726.i, align 1
  %array_elem_1827.i = getelementptr [22 x i8], [22 x i8]* %arr_elem_alloca36.i, i64 0, i64 18
  store i8 105, i8* %array_elem_1827.i, align 2
  %array_elem_1928.i = getelementptr [22 x i8], [22 x i8]* %arr_elem_alloca36.i, i64 0, i64 19
  store i8 112, i8* %array_elem_1928.i, align 1
  %array_elem_2029.i = getelementptr [22 x i8], [22 x i8]* %arr_elem_alloca36.i, i64 0, i64 20
  store i8 116, i8* %array_elem_2029.i, align 4
  %array_elem_2130.i = getelementptr [22 x i8], [22 x i8]* %arr_elem_alloca36.i, i64 0, i64 21
  store i8 0, i8* %array_elem_2130.i, align 1
  store i32 1244, i32* getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i64 0, i32 2), align 4
  store i32 705, i32* getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i64 0, i32 3), align 8
  store i32 40, i32* getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i64 0, i32 0, i32 0, i32 0), align 16
  store i32 1244, i32* getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i64 0, i32 0, i32 0, i32 1), align 4
  store i32 -705, i32* getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i64 0, i32 0, i32 0, i32 2), align 8
  store i16 1, i16* getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i64 0, i32 0, i32 0, i32 3), align 4
  store i16 32, i16* getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i64 0, i32 0, i32 0, i32 4), align 2
  store i32 0, i32* getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i64 0, i32 0, i32 0, i32 5), align 16
  %struct_arrow.i.i = load i8*, i8** getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i64 0, i32 1), align 8
  %icmp_tmp.i.i = icmp eq i8* %struct_arrow.i.i, null
  br i1 %icmp_tmp.i.i, label %create_backbuffer.exit.i, label %then.i.i

then.i.i:                                         ; preds = %vars
  %VirtualFree.i.i = tail call i32 @VirtualFree(i8* nonnull %struct_arrow.i.i, i64 0, i32 32768) #2
  br label %create_backbuffer.exit.i

create_backbuffer.exit.i:                         ; preds = %then.i.i, %vars
  %VirtualAlloc.i.i = tail call i8* @VirtualAlloc(i8* null, i64 3508080, i32 4096, i32 4) #2
  store i8* %VirtualAlloc.i.i, i8** getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i64 0, i32 1), align 8
  %struct_arrow28.i.i = load i32, i32* getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i64 0, i32 2), align 4
  %mul_tmp29.i.i = shl i32 %struct_arrow28.i.i, 2
  store i32 %mul_tmp29.i.i, i32* getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i64 0, i32 4), align 4
  %GetModuleHandleA.i = tail call i64 @GetModuleHandleA(i64 0) #2
  %struct_field_ptr.i = getelementptr inbounds <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>, <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>* %window_class.i, i64 0, i32 0
  store i32 80, i32* %struct_field_ptr.i, align 8
  %struct_field_ptr32.i = getelementptr inbounds <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>, <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>* %window_class.i, i64 0, i32 1
  store i32 3, i32* %struct_field_ptr32.i, align 4
  %struct_field_ptr33.i = getelementptr inbounds <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>, <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>* %window_class.i, i64 0, i32 2
  store i8* bitcast (i64 (i64, i32, i64, i64)* @main_window_callback to i8*), i8** %struct_field_ptr33.i, align 8
  %struct_field_ptr34.i = getelementptr inbounds <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>, <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>* %window_class.i, i64 0, i32 3
  store i32 0, i32* %struct_field_ptr34.i, align 8
  %struct_field_ptr35.i = getelementptr inbounds <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>, <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>* %window_class.i, i64 0, i32 4
  store i32 0, i32* %struct_field_ptr35.i, align 4
  %struct_field_ptr37.i = getelementptr inbounds <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>, <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>* %window_class.i, i64 0, i32 5
  store i64 %GetModuleHandleA.i, i64* %struct_field_ptr37.i, align 8
  %struct_field_ptr38.i = getelementptr inbounds <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>, <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>* %window_class.i, i64 0, i32 6
  %struct_field_ptr43.i = getelementptr inbounds <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>, <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>* %window_class.i, i64 0, i32 10
  %6 = bitcast i64* %struct_field_ptr38.i to i8*
  call void @llvm.memset.p0i8.i64(i8* %6, i8 0, i64 32, i32 8, i1 false) #2
  store i8* %0, i8** %struct_field_ptr43.i, align 8
  %struct_field_ptr44.i = getelementptr inbounds <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>, <{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>* %window_class.i, i64 0, i32 11
  store i64 0, i64* %struct_field_ptr44.i, align 8
  %RegisterClassExA.i = call i16 @RegisterClassExA(<{ i32, i32, i8*, i32, i32, i64, i64, i64, i64, i8*, i8*, i64 }>* nonnull %window_class.i) #2
  %CreateWindowExA.i = call i64 @CreateWindowExA(i32 0, i8* %0, i8* %1, i32 282001408, i32 -2147483648, i32 -2147483648, i32 -2147483648, i32 -2147483648, i64 0, i64 0, i64 %GetModuleHandleA.i, i64 0) #2
  store i64 %CreateWindowExA.i, i64* @window.0, align 8
  %GetDC.i = call i64 @GetDC(i64 %CreateWindowExA.i) #2
  store i64 %GetDC.i, i64* @window.1, align 8
  %7 = bitcast <{ i32, i32, i32, i32 }>* %rect.i.i to i8*
  call void @llvm.lifetime.start(i64 16, i8* %7) #2
  %struct_field.i.i = load i64, i64* @window.0, align 8
  %GetClientRect.i.i = call i32 @GetClientRect(i64 %struct_field.i.i, <{ i32, i32, i32, i32 }>* nonnull %rect.i.i) #2
  %struct_field_ptr.i.i = getelementptr inbounds <{ i32, i32, i32, i32 }>, <{ i32, i32, i32, i32 }>* %rect.i.i, i64 0, i32 2
  %8 = bitcast i32* %struct_field_ptr.i.i to i64*
  %struct_field1.i.i = load i64, i64* %8, align 8
  %9 = trunc i64 %struct_field1.i.i to i32
  %10 = bitcast <{ i32, i32, i32, i32 }>* %rect.i.i to i64*
  %struct_field3.i.i = load i64, i64* %10, align 8
  %11 = trunc i64 %struct_field3.i.i to i32
  %sub_tmp.i.i = sub i32 %9, %11
  store i32 %sub_tmp.i.i, i32* @window.2, align 8
  %12 = lshr i64 %struct_field1.i.i, 32
  %13 = trunc i64 %12 to i32
  %14 = lshr i64 %struct_field3.i.i, 32
  %15 = trunc i64 %14 to i32
  %sub_tmp8.i.i = sub i32 %13, %15
  store i32 %sub_tmp8.i.i, i32* @window.3, align 4
  call void @llvm.lifetime.end(i64 16, i8* %7) #2
  store i1 true, i1* @running, align 1
  %struct_field_ptr54.i = getelementptr inbounds <{ i64, i32, i64, i64, i32, <{ i32, i32 }>, i64 }>, <{ i64, i32, i64, i64, i32, <{ i32, i32 }>, i64 }>* %msg.i, i64 0, i32 1
  br label %while_cond51.preheader.i

while_cond51.preheader.i:                         ; preds = %render_weird_gradient.exit.i, %create_backbuffer.exit.i
  %z_offset.018.i = phi i32 [ 0, %create_backbuffer.exit.i ], [ %add_tmp64.i, %render_weird_gradient.exit.i ]
  %y_offset.016.i = phi i32 [ 0, %create_backbuffer.exit.i ], [ %add_tmp62.i, %render_weird_gradient.exit.i ]
  %PeekMessageA12.i = call i32 @PeekMessageA(<{ i64, i32, i64, i64, i32, <{ i32, i32 }>, i64 }>* nonnull %msg.i, i64 0, i32 0, i32 0, i32 1) #2
  %icmp_tmp13.i = icmp eq i32 %PeekMessageA12.i, 0
  br i1 %icmp_tmp13.i, label %while_end53.i, label %while52.i.preheader

while52.i.preheader:                              ; preds = %while_cond51.preheader.i
  br label %while52.i

while52.i:                                        ; preds = %while52.i.preheader, %endif.i
  %struct_field55.i = load i32, i32* %struct_field_ptr54.i, align 8
  %icmp_tmp56.i = icmp eq i32 %struct_field55.i, 18
  br i1 %icmp_tmp56.i, label %then.i, label %endif.i

then.i:                                           ; preds = %while52.i
  store i1 false, i1* @running, align 1
  br label %endif.i

endif.i:                                          ; preds = %then.i, %while52.i
  %TranslateMessage.i = call i32 @TranslateMessage(<{ i64, i32, i64, i64, i32, <{ i32, i32 }>, i64 }>* nonnull %msg.i) #2
  %DispatchMessageA.i = call i64 @DispatchMessageA(<{ i64, i32, i64, i64, i32, <{ i32, i32 }>, i64 }>* nonnull %msg.i) #2
  %PeekMessageA.i = call i32 @PeekMessageA(<{ i64, i32, i64, i64, i32, <{ i32, i32 }>, i64 }>* nonnull %msg.i, i64 0, i32 0, i32 0, i32 1) #2
  %icmp_tmp.i = icmp eq i32 %PeekMessageA.i, 0
  br i1 %icmp_tmp.i, label %while_end53.i.loopexit, label %while52.i

while_end53.i.loopexit:                           ; preds = %endif.i
  br label %while_end53.i

while_end53.i:                                    ; preds = %while_end53.i.loopexit, %while_cond51.preheader.i
  %struct_arrow.i7.i = load i32, i32* getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i64 0, i32 2), align 4
  %struct_arrow2.i.i = load i64, i64* bitcast (i32* getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i64 0, i32 3) to i64*), align 8
  %16 = trunc i64 %struct_arrow2.i.i to i32
  %icmp_tmp4.i.i = icmp sgt i32 %16, 0
  br i1 %icmp_tmp4.i.i, label %for.lr.ph.i.i, label %render_weird_gradient.exit.i

for.lr.ph.i.i:                                    ; preds = %while_end53.i
  %icmp_tmp161.i.i = icmp sgt i32 %struct_arrow.i7.i, 0
  %17 = ashr i64 %struct_arrow2.i.i, 32
  br i1 %icmp_tmp161.i.i, label %for.us.preheader.i.i, label %render_weird_gradient.exit.i

for.us.preheader.i.i:                             ; preds = %for.lr.ph.i.i
  %struct_arrow6.i.i = load i8*, i8** getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i64 0, i32 1), align 8
  %18 = add i32 %struct_arrow.i7.i, -1
  %19 = zext i32 %18 to i64
  %20 = add nuw nsw i64 %19, 1
  %min.iters.check = icmp ult i64 %20, 8
  %n.vec = and i64 %20, 8589934584
  %cmp.zero = icmp eq i64 %n.vec, 0
  %cast.crd = trunc i64 %n.vec to i32
  %broadcast.splatinsert18 = insertelement <4 x i32> undef, i32 %z_offset.018.i, i32 0
  %broadcast.splat19 = shufflevector <4 x i32> %broadcast.splatinsert18, <4 x i32> undef, <4 x i32> zeroinitializer
  %cmp.n = icmp eq i64 %20, %n.vec
  br label %for.us.i.i

for.us.i.i:                                       ; preds = %for_cond10.end_for13_crit_edge.us.i.i, %for.us.preheader.i.i
  %row.06.us.i.i = phi i8* [ %ptr_add.us.i.i, %for_cond10.end_for13_crit_edge.us.i.i ], [ %struct_arrow6.i.i, %for.us.preheader.i.i ]
  %j.05.us.i.i = phi i32 [ %preinc37.us.i.i, %for_cond10.end_for13_crit_edge.us.i.i ], [ 0, %for.us.preheader.i.i ]
  %pointer_bit_cast.us.i.i = bitcast i8* %row.06.us.i.i to i32*
  %add_tmp19.us.i.i = add i32 %j.05.us.i.i, %y_offset.016.i
  %xor_tmp.us.i.i = xor i32 %add_tmp19.us.i.i, %z_offset.018.i
  %int_cast29.us.i.i = shl i32 %add_tmp19.us.i.i, 8
  %shl_tmp30.us.i.i = and i32 %int_cast29.us.i.i, 65280
  br i1 %min.iters.check, label %for11.us.i.i.preheader, label %min.iters.checked

min.iters.checked:                                ; preds = %for.us.i.i
  %ind.end = getelementptr i32, i32* %pointer_bit_cast.us.i.i, i64 %n.vec
  br i1 %cmp.zero, label %for11.us.i.i.preheader, label %vector.ph

vector.ph:                                        ; preds = %min.iters.checked
  %broadcast.splatinsert20 = insertelement <4 x i32> undef, i32 %xor_tmp.us.i.i, i32 0
  %broadcast.splat21 = shufflevector <4 x i32> %broadcast.splatinsert20, <4 x i32> undef, <4 x i32> zeroinitializer
  %broadcast.splatinsert22 = insertelement <4 x i32> undef, i32 %shl_tmp30.us.i.i, i32 0
  %broadcast.splat23 = shufflevector <4 x i32> %broadcast.splatinsert22, <4 x i32> undef, <4 x i32> zeroinitializer
  br label %vector.body

vector.body:                                      ; preds = %vector.body, %vector.ph
  %index = phi i64 [ 0, %vector.ph ], [ %index.next, %vector.body ]
  %next.gep = getelementptr i32, i32* %pointer_bit_cast.us.i.i, i64 %index
  %21 = trunc i64 %index to i32
  %broadcast.splatinsert = insertelement <4 x i32> undef, i32 %21, i32 0
  %broadcast.splat = shufflevector <4 x i32> %broadcast.splatinsert, <4 x i32> undef, <4 x i32> zeroinitializer
  %induction = add <4 x i32> %broadcast.splat, <i32 0, i32 1, i32 2, i32 3>
  %induction17 = add <4 x i32> %broadcast.splat, <i32 4, i32 5, i32 6, i32 7>
  %22 = add <4 x i32> %induction, %broadcast.splat19
  %23 = add <4 x i32> %induction17, %broadcast.splat19
  %24 = xor <4 x i32> %broadcast.splat21, %22
  %25 = xor <4 x i32> %broadcast.splat21, %23
  %26 = shl <4 x i32> %24, <i32 16, i32 16, i32 16, i32 16>
  %27 = shl <4 x i32> %25, <i32 16, i32 16, i32 16, i32 16>
  %28 = and <4 x i32> %26, <i32 16711680, i32 16711680, i32 16711680, i32 16711680>
  %29 = and <4 x i32> %27, <i32 16711680, i32 16711680, i32 16711680, i32 16711680>
  %30 = and <4 x i32> %22, <i32 255, i32 255, i32 255, i32 255>
  %31 = and <4 x i32> %23, <i32 255, i32 255, i32 255, i32 255>
  %32 = or <4 x i32> %30, %broadcast.splat23
  %33 = or <4 x i32> %31, %broadcast.splat23
  %34 = or <4 x i32> %32, %28
  %35 = or <4 x i32> %33, %29
  %36 = bitcast i32* %next.gep to <4 x i32>*
  store <4 x i32> %34, <4 x i32>* %36, align 4
  %37 = getelementptr i32, i32* %next.gep, i64 4
  %38 = bitcast i32* %37 to <4 x i32>*
  store <4 x i32> %35, <4 x i32>* %38, align 4
  %index.next = add i64 %index, 8
  %39 = icmp eq i64 %index.next, %n.vec
  br i1 %39, label %middle.block, label %vector.body, !llvm.loop !0

middle.block:                                     ; preds = %vector.body
  br i1 %cmp.n, label %for_cond10.end_for13_crit_edge.us.i.i, label %for11.us.i.i.preheader

for11.us.i.i.preheader:                           ; preds = %middle.block, %min.iters.checked, %for.us.i.i
  %pixel.03.us.i.i.ph = phi i32* [ %pointer_bit_cast.us.i.i, %min.iters.checked ], [ %pointer_bit_cast.us.i.i, %for.us.i.i ], [ %ind.end, %middle.block ]
  %i.02.us.i.i.ph = phi i32 [ 0, %min.iters.checked ], [ 0, %for.us.i.i ], [ %cast.crd, %middle.block ]
  br label %for11.us.i.i

for11.us.i.i:                                     ; preds = %for11.us.i.i.preheader, %for11.us.i.i
  %pixel.03.us.i.i = phi i32* [ %ptr_post_inc.us.i.i, %for11.us.i.i ], [ %pixel.03.us.i.i.ph, %for11.us.i.i.preheader ]
  %i.02.us.i.i = phi i32 [ %preinc.us.i.i, %for11.us.i.i ], [ %i.02.us.i.i.ph, %for11.us.i.i.preheader ]
  %add_tmp.us.i.i = add i32 %i.02.us.i.i, %z_offset.018.i
  %xor_tmp22.us.i.i = xor i32 %xor_tmp.us.i.i, %add_tmp.us.i.i
  %int_cast.us.i.i = shl i32 %xor_tmp22.us.i.i, 16
  %shl_tmp.us.i.i = and i32 %int_cast.us.i.i, 16711680
  %int_cast32.us.i.i = and i32 %add_tmp.us.i.i, 255
  %or_tmp.us.i.i = or i32 %int_cast32.us.i.i, %shl_tmp30.us.i.i
  %or_tmp33.us.i.i = or i32 %or_tmp.us.i.i, %shl_tmp.us.i.i
  store i32 %or_tmp33.us.i.i, i32* %pixel.03.us.i.i, align 4
  %ptr_post_inc.us.i.i = getelementptr i32, i32* %pixel.03.us.i.i, i64 1
  %preinc.us.i.i = add nuw nsw i32 %i.02.us.i.i, 1
  %exitcond.i.i = icmp eq i32 %preinc.us.i.i, %struct_arrow.i7.i
  br i1 %exitcond.i.i, label %for_cond10.end_for13_crit_edge.us.i.i.loopexit, label %for11.us.i.i, !llvm.loop !3

for_cond10.end_for13_crit_edge.us.i.i.loopexit:   ; preds = %for11.us.i.i
  br label %for_cond10.end_for13_crit_edge.us.i.i

for_cond10.end_for13_crit_edge.us.i.i:            ; preds = %for_cond10.end_for13_crit_edge.us.i.i.loopexit, %middle.block
  %ptr_add.us.i.i = getelementptr i8, i8* %row.06.us.i.i, i64 %17
  %preinc37.us.i.i = add nuw nsw i32 %j.05.us.i.i, 1
  %exitcond8.i.i = icmp eq i32 %preinc37.us.i.i, %16
  br i1 %exitcond8.i.i, label %render_weird_gradient.exit.loopexit.i, label %for.us.i.i

render_weird_gradient.exit.loopexit.i:            ; preds = %for_cond10.end_for13_crit_edge.us.i.i
  %struct_arrow.i10.pre.i = load i32, i32* getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i64 0, i32 2), align 4
  %struct_arrow4.i.pre.i = load i32, i32* getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i64 0, i32 3), align 8
  br label %render_weird_gradient.exit.i

render_weird_gradient.exit.i:                     ; preds = %render_weird_gradient.exit.loopexit.i, %for.lr.ph.i.i, %while_end53.i
  %struct_arrow4.i.i = phi i32 [ %struct_arrow4.i.pre.i, %render_weird_gradient.exit.loopexit.i ], [ %16, %while_end53.i ], [ %16, %for.lr.ph.i.i ]
  %struct_arrow.i10.i = phi i32 [ %struct_arrow.i10.pre.i, %render_weird_gradient.exit.loopexit.i ], [ %struct_arrow.i7.i, %while_end53.i ], [ %struct_arrow.i7.i, %for.lr.ph.i.i ]
  %struct_field.i8.i = load i64, i64* @window.1, align 8
  %struct_field1.i9.i = load i32, i32* @window.2, align 8
  %struct_field2.i.i = load i32, i32* @window.3, align 4
  %struct_arrow6.i11.i = load i8*, i8** getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i64 0, i32 1), align 8
  %StretchDIBits.i.i = call i32 @StretchDIBits(i64 %struct_field.i8.i, i32 0, i32 0, i32 %struct_field1.i9.i, i32 %struct_field2.i.i, i32 0, i32 0, i32 %struct_arrow.i10.i, i32 %struct_arrow4.i.i, i8* %struct_arrow6.i11.i, <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>* getelementptr inbounds (<{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>, <{ <{ <{ i32, i32, i32, i16, i16, i32, i32, i32, i32, i32, i32 }>, <{ i8, i8, i8, i8 }> }>, i8*, i32, i32, i32 }>* @backbuffer, i64 0, i32 0), i32 0, i32 13369376) #2
  %add_tmp64.i = add i32 %z_offset.018.i, 1
  %add_tmp62.i = add i32 %y_offset.016.i, 2
  %running.pr.i = load i1, i1* @running, align 1
  br i1 %running.pr.i, label %while_cond51.preheader.i, label %main.exit

main.exit:                                        ; preds = %render_weird_gradient.exit.i
  call void @ExitProcess(i32 0) #2
  call void @llvm.lifetime.end(i64 24, i8* %0)
  call void @llvm.lifetime.end(i64 22, i8* %1)
  call void @llvm.lifetime.end(i64 80, i8* %2)
  call void @llvm.lifetime.end(i64 48, i8* %3)
  ret void
}

; Function Attrs: argmemonly nounwind
declare void @llvm.lifetime.start(i64, i8* nocapture) #1

; Function Attrs: argmemonly nounwind
declare void @llvm.lifetime.end(i64, i8* nocapture) #1

; Function Attrs: argmemonly nounwind
declare void @llvm.memset.p0i8.i64(i8* nocapture, i8, i64, i32, i1) #1

attributes #0 = { nounwind "target-cpu"="nehalem" }
attributes #1 = { argmemonly nounwind }
attributes #2 = { nounwind }

!0 = distinct !{!0, !1, !2}
!1 = !{!"llvm.loop.vectorize.width", i32 1}
!2 = !{!"llvm.loop.interleave.count", i32 1}
!3 = distinct !{!3, !4, !1, !2}
!4 = !{!"llvm.loop.unroll.runtime.disable"}
