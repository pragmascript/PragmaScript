; Function Attrs: nounwind
declare align 64 i8* @VirtualAlloc(i8* nocapture, i64, i32, i32) #0 

; Function Attrs: nounwind uwtable
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


attributes #0 = { nounwind }
