	.text
	.def	 _rdtsc;
	.scl	2;
	.type	32;
	.endef
	.globl	_rdtsc
	.p2align	4, 0x90
_rdtsc:                                 # @_rdtsc
# BB#0:
	#APP
	rdtsc
	#NO_APP
	shlq	$32, %rdx
	movl	%eax, %eax
	orq	%rdx, %rax
	retq

	.def	 sinf;
	.scl	3;
	.type	32;
	.endef
	.globl	__real@40c90fdb
	.section	.rdata,"dr",discard,__real@40c90fdb
	.p2align	2
__real@40c90fdb:
	.long	1086918619              # float 6.28318548
	.globl	__real@3f000000
	.section	.rdata,"dr",discard,__real@3f000000
	.p2align	2
__real@3f000000:
	.long	1056964608              # float 0.5
	.globl	__real@40f2dce8
	.section	.rdata,"dr",discard,__real@40f2dce8
	.p2align	2
__real@40f2dce8:
	.long	1089658088              # float 7.58946609
	.globl	__xmm@7fffffff7fffffff7fffffff7fffffff
	.section	.rdata,"dr",discard,__xmm@7fffffff7fffffff7fffffff7fffffff
	.p2align	4
__xmm@7fffffff7fffffff7fffffff7fffffff:
	.long	2147483647              # 0x7fffffff
	.long	2147483647              # 0x7fffffff
	.long	2147483647              # 0x7fffffff
	.long	2147483647              # 0x7fffffff
	.globl	__real@3fd121c8
	.section	.rdata,"dr",discard,__real@3fd121c8
	.p2align	2
__real@3fd121c8:
	.long	1070670280              # float 1.63384342
	.text
	.p2align	4, 0x90
sinf:                                   # @sinf
# BB#0:                                 # %vars
	divss	__real@40c90fdb(%rip), %xmm0
	movss	__real@3f000000(%rip), %xmm2 # xmm2 = mem[0],zero,zero,zero
	movaps	%xmm0, %xmm1
	addss	%xmm2, %xmm1
	roundss	$9, %xmm1, %xmm1
	subss	%xmm1, %xmm0
	movss	__real@40f2dce8(%rip), %xmm3 # xmm3 = mem[0],zero,zero,zero
	mulss	%xmm0, %xmm3
	movaps	__xmm@7fffffff7fffffff7fffffff7fffffff(%rip), %xmm1 # xmm1 = [2147483647,2147483647,2147483647,2147483647]
	andps	%xmm1, %xmm0
	subss	%xmm0, %xmm2
	mulss	%xmm3, %xmm2
	andps	%xmm2, %xmm1
	addss	__real@3fd121c8(%rip), %xmm1
	mulss	%xmm2, %xmm1
	movaps	%xmm1, %xmm0
	retq

	.def	 cosf;
	.scl	3;
	.type	32;
	.endef
	.globl	__real@3fc90fdb
	.section	.rdata,"dr",discard,__real@3fc90fdb
	.p2align	2
__real@3fc90fdb:
	.long	1070141403              # float 1.57079637
	.text
	.p2align	4, 0x90
cosf:                                   # @cosf
# BB#0:                                 # %vars
	movss	__real@3fc90fdb(%rip), %xmm1 # xmm1 = mem[0],zero,zero,zero
	subss	%xmm0, %xmm1
	divss	__real@40c90fdb(%rip), %xmm1
	movss	__real@3f000000(%rip), %xmm2 # xmm2 = mem[0],zero,zero,zero
	movaps	%xmm1, %xmm0
	addss	%xmm2, %xmm0
	roundss	$9, %xmm0, %xmm0
	subss	%xmm0, %xmm1
	movss	__real@40f2dce8(%rip), %xmm3 # xmm3 = mem[0],zero,zero,zero
	mulss	%xmm1, %xmm3
	movaps	__xmm@7fffffff7fffffff7fffffff7fffffff(%rip), %xmm0 # xmm0 = [2147483647,2147483647,2147483647,2147483647]
	andps	%xmm0, %xmm1
	subss	%xmm1, %xmm2
	mulss	%xmm3, %xmm2
	andps	%xmm2, %xmm0
	addss	__real@3fd121c8(%rip), %xmm0
	mulss	%xmm2, %xmm0
	retq

	.def	 fill_sound_buffer;
	.scl	3;
	.type	32;
	.endef
	.p2align	4, 0x90
fill_sound_buffer:                      # @fill_sound_buffer
# BB#0:                                 # %vars
	pushq	%rsi
	pushq	%rdi
	pushq	%rbx
	subq	$128, %rsp
	movaps	%xmm7, 112(%rsp)        # 16-byte Spill
	movaps	%xmm6, 96(%rsp)         # 16-byte Spill
	movq	%rcx, %rdi
	movq	(%rdi), %rbx
	movq	(%rbx), %rax
	leaq	72(%rsp), %rcx
	movq	%rcx, 48(%rsp)
	leaq	80(%rsp), %rcx
	movq	%rcx, 40(%rsp)
	leaq	76(%rsp), %rcx
	movq	%rcx, 32(%rsp)
	movl	$0, 56(%rsp)
	leaq	88(%rsp), %r9
	movq	%rbx, %rcx
	callq	*88(%rax)
	testl	%eax, %eax
	jne	.LBB3_11
# BB#1:                                 # %then
	movl	76(%rsp), %eax
	movl	12(%rdi), %esi
	cltd
	idivl	%esi
	testl	%eax, %eax
	jle	.LBB3_5
# BB#2:                                 # %for.lr.ph
	movq	88(%rsp), %rcx
	movss	38(%rdi), %xmm5         # xmm5 = mem[0],zero,zero,zero
	movss	__real@40c90fdb(%rip), %xmm0 # xmm0 = mem[0],zero,zero,zero
	movss	__real@3f000000(%rip), %xmm1 # xmm1 = mem[0],zero,zero,zero
	movss	__real@40f2dce8(%rip), %xmm2 # xmm2 = mem[0],zero,zero,zero
	movaps	__xmm@7fffffff7fffffff7fffffff7fffffff(%rip), %xmm3 # xmm3 = [2147483647,2147483647,2147483647,2147483647]
	movss	__real@3fd121c8(%rip), %xmm4 # xmm4 = mem[0],zero,zero,zero
	.p2align	4, 0x90
.LBB3_3:                                # %for
                                        # =>This Inner Loop Header: Depth=1
	divss	%xmm0, %xmm5
	movaps	%xmm5, %xmm6
	addss	%xmm1, %xmm6
	roundss	$9, %xmm6, %xmm6
	subss	%xmm6, %xmm5
	movaps	%xmm5, %xmm6
	mulss	%xmm2, %xmm6
	andps	%xmm3, %xmm5
	movaps	%xmm1, %xmm7
	subss	%xmm5, %xmm7
	mulss	%xmm6, %xmm7
	movaps	%xmm7, %xmm5
	andps	%xmm3, %xmm5
	addss	%xmm4, %xmm5
	mulss	%xmm7, %xmm5
	movswl	28(%rdi), %edx
	xorps	%xmm6, %xmm6
	cvtsi2ssl	%edx, %xmm6
	mulss	%xmm5, %xmm6
	cvttss2si	%xmm6, %edx
	movw	%dx, (%rcx)
	movw	%dx, 2(%rcx)
	movss	24(%rdi), %xmm5         # xmm5 = mem[0],zero,zero,zero
	mulss	34(%rdi), %xmm5
	addss	38(%rdi), %xmm5
	movss	%xmm5, 38(%rdi)
	incl	30(%rdi)
	addq	$4, %rcx
	decl	%eax
	jne	.LBB3_3
# BB#4:                                 # %end_for.loopexit
	movl	12(%rdi), %esi
.LBB3_5:                                # %end_for
	movl	72(%rsp), %ecx
	movl	%ecx, %eax
	cltd
	idivl	%esi
	testl	%eax, %eax
	jle	.LBB3_9
# BB#6:                                 # %for41.lr.ph
	movq	80(%rsp), %rcx
	movss	38(%rdi), %xmm5         # xmm5 = mem[0],zero,zero,zero
	movss	__real@40c90fdb(%rip), %xmm0 # xmm0 = mem[0],zero,zero,zero
	movss	__real@3f000000(%rip), %xmm1 # xmm1 = mem[0],zero,zero,zero
	movss	__real@40f2dce8(%rip), %xmm2 # xmm2 = mem[0],zero,zero,zero
	movaps	__xmm@7fffffff7fffffff7fffffff7fffffff(%rip), %xmm3 # xmm3 = [2147483647,2147483647,2147483647,2147483647]
	movss	__real@3fd121c8(%rip), %xmm4 # xmm4 = mem[0],zero,zero,zero
	.p2align	4, 0x90
.LBB3_7:                                # %for41
                                        # =>This Inner Loop Header: Depth=1
	divss	%xmm0, %xmm5
	movaps	%xmm5, %xmm6
	addss	%xmm1, %xmm6
	roundss	$9, %xmm6, %xmm6
	subss	%xmm6, %xmm5
	movaps	%xmm5, %xmm6
	mulss	%xmm2, %xmm6
	andps	%xmm3, %xmm5
	movaps	%xmm1, %xmm7
	subss	%xmm5, %xmm7
	mulss	%xmm6, %xmm7
	movaps	%xmm7, %xmm5
	andps	%xmm3, %xmm5
	addss	%xmm4, %xmm5
	mulss	%xmm7, %xmm5
	movswl	28(%rdi), %edx
	xorps	%xmm6, %xmm6
	cvtsi2ssl	%edx, %xmm6
	mulss	%xmm5, %xmm6
	cvttss2si	%xmm6, %edx
	movw	%dx, (%rcx)
	movw	%dx, 2(%rcx)
	movss	24(%rdi), %xmm5         # xmm5 = mem[0],zero,zero,zero
	mulss	34(%rdi), %xmm5
	addss	38(%rdi), %xmm5
	movss	%xmm5, 38(%rdi)
	incl	30(%rdi)
	addq	$4, %rcx
	decl	%eax
	jne	.LBB3_7
# BB#8:                                 # %end_for43.loopexit
	movl	72(%rsp), %ecx
.LBB3_9:                                # %end_for43
	movq	(%rdi), %rax
	movq	(%rax), %rax
	movq	88(%rsp), %rdx
	movl	76(%rsp), %r8d
	movq	80(%rsp), %r9
	movl	%ecx, 32(%rsp)
	movq	%rbx, %rcx
	callq	*152(%rax)
	testl	%eax, %eax
	jne	.LBB3_10
.LBB3_11:                               # %endif
	movaps	96(%rsp), %xmm6         # 16-byte Reload
	movaps	112(%rsp), %xmm7        # 16-byte Reload
	addq	$128, %rsp
	popq	%rbx
	popq	%rdi
	popq	%rsi
	retq
.LBB3_10:                               # %then.i
	ud2
	ud2

	.def	 main_window_callback;
	.scl	3;
	.type	32;
	.endef
	.p2align	4, 0x90
main_window_callback:                   # @main_window_callback
# BB#0:                                 # %vars
	pushq	%r14
	pushq	%rsi
	pushq	%rdi
	pushq	%rbx
	subq	$200, %rsp
	movq	%rcx, %rsi
	leal	-2(%rdx), %eax
	cmpl	$14, %eax
	ja	.LBB4_1
# BB#5:                                 # %vars
	leaq	LJTI4_0(%rip), %rcx
	movslq	(%rcx,%rax,4), %rax
	addq	%rcx, %rax
	jmpq	*%rax
.LBB4_6:                                # %elif_1_then
	movb	$0, running(%rip)
	jmp	.LBB4_15
.LBB4_1:                                # %vars
	leal	-260(%rdx), %eax
	cmpl	$2, %eax
	jb	.LBB4_3
.LBB4_2:                                # %cor.end
	movl	%edx, %eax
	orl	$1, %eax
	cmpl	$257, %eax              # imm = 0x101
	jne	.LBB4_13
.LBB4_3:                                # %elif_3_then
	addl	$-27, %r8d
	cmpl	$13, %r8d
	ja	.LBB4_15
# BB#4:                                 # %elif_3_then
	leaq	LJTI4_1(%rip), %rax
	movslq	(%rax,%r8,4), %rcx
	addq	%rax, %rcx
	jmpq	*%rcx
.LBB4_8:                                # %then28
	movb	$0, running(%rip)
	jmp	.LBB4_15
.LBB4_14:                               # %then
	movq	window.0(%rip), %rcx
	leaq	112(%rsp), %rdx
	callq	GetClientRect
	movq	112(%rsp), %rax
	movq	120(%rsp), %rcx
	movl	%ecx, %edx
	subl	%eax, %edx
	movl	%edx, window.2(%rip)
	shrq	$32, %rcx
	shrq	$32, %rax
	subl	%eax, %ecx
	movl	%ecx, window.3(%rip)
	jmp	.LBB4_15
.LBB4_7:                                # %elif_2_then
	leaq	128(%rsp), %r14
	movq	%rsi, %rcx
	movq	%r14, %rdx
	callq	BeginPaint
	movq	window.1(%rip), %rcx
	movl	window.2(%rip), %r9d
	movl	window.3(%rip), %eax
	movl	backbuffer+52(%rip), %edx
	leaq	backbuffer(%rip), %r8
	movl	backbuffer+56(%rip), %ebx
	movq	backbuffer+44(%rip), %rdi
	movq	%r8, 80(%rsp)
	movq	%rdi, 72(%rsp)
	movl	%ebx, 64(%rsp)
	movl	%edx, 56(%rsp)
	movl	%eax, 32(%rsp)
	movl	$13369376, 96(%rsp)     # imm = 0xCC0020
	movl	$0, 88(%rsp)
	movl	$0, 48(%rsp)
	movl	$0, 40(%rsp)
	xorl	%edx, %edx
	xorl	%r8d, %r8d
	callq	StretchDIBits
	movq	%rsi, %rcx
	movq	%r14, %rdx
	callq	EndPaint
	jmp	.LBB4_15
.LBB4_13:                               # %else
	movq	%rsi, %rcx
	addq	$200, %rsp
	popq	%rbx
	popq	%rdi
	popq	%rsi
	popq	%r14
	rex64 jmp	DefWindowProcA  # TAILCALL
.LBB4_9:                                # %elif_0_then34
	addl	$10, x_offset(%rip)
	jmp	.LBB4_15
.LBB4_11:                               # %elif_2_then41
	addl	$10, y_offset(%rip)
	jmp	.LBB4_15
.LBB4_10:                               # %elif_1_then37
	addl	$-10, x_offset(%rip)
	jmp	.LBB4_15
.LBB4_12:                               # %elif_3_then45
	addl	$-10, y_offset(%rip)
.LBB4_15:                               # %endif
	xorl	%eax, %eax
	addq	$200, %rsp
	popq	%rbx
	popq	%rdi
	popq	%rsi
	popq	%r14
	retq
	.p2align	2, 0x90
LJTI4_0:
	.long	.LBB4_6-LJTI4_0
	.long	.LBB4_2-LJTI4_0
	.long	.LBB4_2-LJTI4_0
	.long	.LBB4_14-LJTI4_0
	.long	.LBB4_2-LJTI4_0
	.long	.LBB4_2-LJTI4_0
	.long	.LBB4_2-LJTI4_0
	.long	.LBB4_2-LJTI4_0
	.long	.LBB4_2-LJTI4_0
	.long	.LBB4_2-LJTI4_0
	.long	.LBB4_2-LJTI4_0
	.long	.LBB4_2-LJTI4_0
	.long	.LBB4_2-LJTI4_0
	.long	.LBB4_7-LJTI4_0
	.long	.LBB4_6-LJTI4_0
LJTI4_1:
	.long	.LBB4_8-LJTI4_1
	.long	.LBB4_15-LJTI4_1
	.long	.LBB4_15-LJTI4_1
	.long	.LBB4_15-LJTI4_1
	.long	.LBB4_15-LJTI4_1
	.long	.LBB4_15-LJTI4_1
	.long	.LBB4_15-LJTI4_1
	.long	.LBB4_15-LJTI4_1
	.long	.LBB4_15-LJTI4_1
	.long	.LBB4_15-LJTI4_1
	.long	.LBB4_9-LJTI4_1
	.long	.LBB4_11-LJTI4_1
	.long	.LBB4_10-LJTI4_1
	.long	.LBB4_12-LJTI4_1

	.def	 __init;
	.scl	2;
	.type	32;
	.endef
	.globl	__xmm@646e6957747069726353616d67617250
	.section	.rdata,"dr",discard,__xmm@646e6957747069726353616d67617250
	.p2align	4
__xmm@646e6957747069726353616d67617250:
	.byte	80                      # 0x50
	.byte	114                     # 0x72
	.byte	97                      # 0x61
	.byte	103                     # 0x67
	.byte	109                     # 0x6d
	.byte	97                      # 0x61
	.byte	83                      # 0x53
	.byte	99                      # 0x63
	.byte	114                     # 0x72
	.byte	105                     # 0x69
	.byte	112                     # 0x70
	.byte	116                     # 0x74
	.byte	87                      # 0x57
	.byte	105                     # 0x69
	.byte	110                     # 0x6e
	.byte	100                     # 0x64
	.globl	__xmm@73616d676172702d6564616d646e6168
	.section	.rdata,"dr",discard,__xmm@73616d676172702d6564616d646e6168
	.p2align	4
__xmm@73616d676172702d6564616d646e6168:
	.byte	104                     # 0x68
	.byte	97                      # 0x61
	.byte	110                     # 0x6e
	.byte	100                     # 0x64
	.byte	109                     # 0x6d
	.byte	97                      # 0x61
	.byte	100                     # 0x64
	.byte	101                     # 0x65
	.byte	45                      # 0x2d
	.byte	112                     # 0x70
	.byte	114                     # 0x72
	.byte	97                      # 0x61
	.byte	103                     # 0x67
	.byte	109                     # 0x6d
	.byte	97                      # 0x61
	.byte	115                     # 0x73
	.globl	__xmm@000032000002ee00000000040000bb80
	.section	.rdata,"dr",discard,__xmm@000032000002ee00000000040000bb80
	.p2align	4
__xmm@000032000002ee00000000040000bb80:
	.long	48000                   # 0xbb80
	.long	4                       # 0x4
	.long	192000                  # 0x2ee00
	.long	12800                   # 0x3200
	.globl	__xmm@7461657243646e756f53746365726944
	.section	.rdata,"dr",discard,__xmm@7461657243646e756f53746365726944
	.p2align	4
__xmm@7461657243646e756f53746365726944:
	.byte	68                      # 0x44
	.byte	105                     # 0x69
	.byte	114                     # 0x72
	.byte	101                     # 0x65
	.byte	99                      # 0x63
	.byte	116                     # 0x74
	.byte	83                      # 0x53
	.byte	111                     # 0x6f
	.byte	117                     # 0x75
	.byte	110                     # 0x6e
	.byte	100                     # 0x64
	.byte	67                      # 0x43
	.byte	114                     # 0x72
	.byte	101                     # 0x65
	.byte	97                      # 0x61
	.byte	116                     # 0x74
	.globl	__xmm@000000000002ee000000000000000018
	.section	.rdata,"dr",discard,__xmm@000000000002ee000000000000000018
	.p2align	4
__xmm@000000000002ee000000000000000018:
	.long	24                      # 0x18
	.long	0                       # 0x0
	.long	192000                  # 0x2ee00
	.long	0                       # 0x0
	.globl	__xmm@00000003000000020000000100000000
	.section	.rdata,"dr",discard,__xmm@00000003000000020000000100000000
	.p2align	4
__xmm@00000003000000020000000100000000:
	.long	0                       # 0x0
	.long	1                       # 0x1
	.long	2                       # 0x2
	.long	3                       # 0x3
	.globl	__xmm@00000007000000060000000500000004
	.section	.rdata,"dr",discard,__xmm@00000007000000060000000500000004
	.p2align	4
__xmm@00000007000000060000000500000004:
	.long	4                       # 0x4
	.long	5                       # 0x5
	.long	6                       # 0x6
	.long	7                       # 0x7
	.globl	__xmm@000000ff000000ff000000ff000000ff
	.section	.rdata,"dr",discard,__xmm@000000ff000000ff000000ff000000ff
	.p2align	4
__xmm@000000ff000000ff000000ff000000ff:
	.byte	255                     # 0xff
	.byte	0                       # 0x0
	.byte	0                       # 0x0
	.byte	0                       # 0x0
	.byte	255                     # 0xff
	.byte	0                       # 0x0
	.byte	0                       # 0x0
	.byte	0                       # 0x0
	.byte	255                     # 0xff
	.byte	0                       # 0x0
	.byte	0                       # 0x0
	.byte	0                       # 0x0
	.byte	255                     # 0xff
	.byte	0                       # 0x0
	.byte	0                       # 0x0
	.byte	0                       # 0x0
	.globl	__real@44fa0000
	.section	.rdata,"dr",discard,__real@44fa0000
	.p2align	2
__real@44fa0000:
	.long	1157234688              # float 2000
	.text
	.globl	__init
	.p2align	4, 0x90
__init:                                 # @__init
# BB#0:                                 # %vars
	pushq	%r15
	pushq	%r14
	pushq	%r13
	pushq	%r12
	pushq	%rsi
	pushq	%rdi
	pushq	%rbp
	pushq	%rbx
	subq	$696, %rsp              # imm = 0x2B8
	movaps	%xmm10, 672(%rsp)       # 16-byte Spill
	movaps	%xmm9, 656(%rsp)        # 16-byte Spill
	movdqa	%xmm8, 640(%rsp)        # 16-byte Spill
	movdqa	%xmm7, 624(%rsp)        # 16-byte Spill
	movdqa	%xmm6, 608(%rsp)        # 16-byte Spill
	movabsq	$3978425819141910832, %rax # imm = 0x3736353433323130
	movq	%rax, 398(%rsp)
	movw	$14648, 406(%rsp)       # imm = 0x3938
	movl	$-11, %ecx
	callq	GetStdHandle
	movq	%rax, console_output_handle(%rip)
	movl	$-10, %ecx
	callq	GetStdHandle
	movl	$10, decimal_digits(%rip)
	leaq	398(%rsp), %rax
	movq	%rax, decimal_digits+4(%rip)
	movaps	__xmm@646e6957747069726353616d67617250(%rip), %xmm0 # xmm0 = [80,114,97,103,109,97,83,99,114,105,112,116,87,105,110,100]
	movaps	%xmm0, 192(%rsp)
	movb	$111, 208(%rsp)
	movb	$119, 209(%rsp)
	movb	$67, 210(%rsp)
	movb	$108, 211(%rsp)
	movb	$97, 212(%rsp)
	movb	$115, 213(%rsp)
	movb	$115, 214(%rsp)
	movb	$0, 215(%rsp)
	movdqa	__xmm@73616d676172702d6564616d646e6168(%rip), %xmm0 # xmm0 = [104,97,110,100,109,97,100,101,45,112,114,97,103,109,97,115]
	movdqa	%xmm0, 240(%rsp)
	movb	$99, 256(%rsp)
	movb	$114, 257(%rsp)
	movb	$105, 258(%rsp)
	movb	$112, 259(%rsp)
	movb	$116, 260(%rsp)
	movb	$0, 261(%rsp)
	movl	$1835102822, 171(%rsp)  # imm = 0x6D617266
	movb	$101, 175(%rsp)
	leaq	448(%rsp), %rcx
	callq	QueryPerformanceFrequency
	leaq	sinf(%rip), %rax
	cmpq	$1234, %rax             # imm = 0x4D2
	sete	%al
	leaq	cosf(%rip), %rcx
	cmpq	$1234, %rcx             # imm = 0x4D2
	sete	%cl
	orb	%al, %cl
	movzbl	%cl, %eax
	cmpl	$1, %eax
	jne	.LBB5_2
# BB#1:                                 # %then.i
	movq	console_output_handle(%rip), %rcx
	movq	$0, 32(%rsp)
	leaq	143(%rsp), %rdx
	xorl	%r8d, %r8d
	xorl	%r9d, %r9d
	callq	WriteFile
.LBB5_2:                                # %endif.i
	movb	$120, 144(%rsp)
	movb	$105, 145(%rsp)
	movb	$110, 146(%rsp)
	movb	$112, 147(%rsp)
	movb	$117, 148(%rsp)
	movb	$116, 149(%rsp)
	movb	$49, 150(%rsp)
	movb	$95, 151(%rsp)
	movb	$52, 152(%rsp)
	movb	$46, 153(%rsp)
	movb	$100, 154(%rsp)
	movb	$108, 155(%rsp)
	movb	$108, 156(%rsp)
	movb	$0, 157(%rsp)
	movb	$120, 112(%rsp)
	movb	$105, 113(%rsp)
	movb	$110, 114(%rsp)
	movb	$112, 115(%rsp)
	movb	$117, 116(%rsp)
	movb	$116, 117(%rsp)
	movb	$49, 118(%rsp)
	movb	$95, 119(%rsp)
	movb	$51, 120(%rsp)
	movb	$46, 121(%rsp)
	movb	$100, 122(%rsp)
	movb	$108, 123(%rsp)
	movb	$108, 124(%rsp)
	movb	$0, 125(%rsp)
	movabsq	$7297929768923449688, %rax # imm = 0x65477475706E4958
	movq	%rax, 288(%rsp)
	movl	$1635013492, 296(%rsp)  # imm = 0x61745374
	movw	$25972, 300(%rsp)       # imm = 0x6574
	movb	$0, 302(%rsp)
	leaq	144(%rsp), %rcx
	callq	LoadLibraryA
	testq	%rax, %rax
	jne	.LBB5_5
# BB#3:                                 # %endif.i.i
	leaq	112(%rsp), %rcx
	callq	LoadLibraryA
	testq	%rax, %rax
	je	.LBB5_6
# BB#4:                                 # %then27.i.i
	cmpb	$0, 302(%rsp)
	jne	.LBB5_52
.LBB5_5:                                # %cstr.exit13.i.i
	leaq	288(%rsp), %rdx
	movq	%rax, %rcx
	callq	GetProcAddress
	movq	%rax, XInputGetState(%rip)
	testq	%rax, %rax
	je	.LBB5_52
.LBB5_6:                                # %load_x_input.exit.i
	movl	$1244, backbuffer+52(%rip) # imm = 0x4DC
	movl	$705, backbuffer+56(%rip) # imm = 0x2C1
	movl	$40, backbuffer(%rip)
	movl	$1244, backbuffer+4(%rip) # imm = 0x4DC
	movl	$-705, backbuffer+8(%rip) # imm = 0xFFFFFFFFFFFFFD3F
	movw	$1, backbuffer+12(%rip)
	movw	$32, backbuffer+14(%rip)
	movl	$0, backbuffer+16(%rip)
	movq	backbuffer+44(%rip), %rcx
	testq	%rcx, %rcx
	je	.LBB5_8
# BB#7:                                 # %then.i.i
	xorl	%edx, %edx
	movl	$32768, %r8d            # imm = 0x8000
	callq	VirtualFree
.LBB5_8:                                # %create_backbuffer.exit.i
	xorl	%ecx, %ecx
	movl	$3508080, %edx          # imm = 0x358770
	movl	$4096, %r8d             # imm = 0x1000
	movl	$4, %r9d
	callq	VirtualAlloc
	movq	%rax, backbuffer+44(%rip)
	movl	backbuffer+52(%rip), %eax
	shll	$2, %eax
	movl	%eax, backbuffer+60(%rip)
	cmpb	$0, 215(%rsp)
	jne	.LBB5_52
# BB#9:                                 # %cstr.exit.i
	cmpb	$0, 261(%rsp)
	jne	.LBB5_52
# BB#10:                                # %cstr.exit25.i
	xorl	%ecx, %ecx
	callq	GetModuleHandleA
	movq	%rax, %rsi
	movl	$80, 480(%rsp)
	movl	$3, 484(%rsp)
	leaq	main_window_callback(%rip), %rax
	movq	%rax, 488(%rsp)
	movl	$0, 496(%rsp)
	movl	$0, 500(%rsp)
	movq	%rsi, 504(%rsp)
	pxor	%xmm0, %xmm0
	movdqu	%xmm0, 528(%rsp)
	movdqu	%xmm0, 512(%rsp)
	leaq	192(%rsp), %rdi
	movq	%rdi, 544(%rsp)
	movq	$0, 552(%rsp)
	leaq	480(%rsp), %rcx
	callq	RegisterClassExA
	movq	%rsi, 80(%rsp)
	movq	$0, 88(%rsp)
	movq	$0, 72(%rsp)
	movq	$0, 64(%rsp)
	movl	$-2147483648, 56(%rsp)  # imm = 0xFFFFFFFF80000000
	movl	$-2147483648, 48(%rsp)  # imm = 0xFFFFFFFF80000000
	movl	$-2147483648, 40(%rsp)  # imm = 0xFFFFFFFF80000000
	movl	$-2147483648, 32(%rsp)  # imm = 0xFFFFFFFF80000000
	leaq	240(%rsp), %r8
	xorl	%ecx, %ecx
	movl	$282001408, %r9d        # imm = 0x10CF0000
	movq	%rdi, %rdx
	callq	CreateWindowExA
	movq	%rax, window.0(%rip)
	movq	%rax, %rcx
	callq	GetDC
	movq	%rax, window.1(%rip)
	movq	window.0(%rip), %rcx
	leaq	288(%rsp), %rdx
	callq	GetClientRect
	movq	288(%rsp), %rax
	movq	296(%rsp), %rcx
	movl	%ecx, %edx
	subl	%eax, %edx
	movl	%edx, window.2(%rip)
	shrq	$32, %rcx
	shrq	$32, %rax
	subl	%eax, %ecx
	movl	%ecx, window.3(%rip)
	movaps	__xmm@000032000002ee00000000040000bb80(%rip), %xmm0 # xmm0 = [48000,4,192000,12800]
	movups	%xmm0, 360(%rsp)
	movl	$1132462080, 376(%rsp)  # imm = 0x43800000
	movw	$1000, 380(%rsp)        # imm = 0x3E8
	movl	$956908063, 386(%rsp)   # imm = 0x3909421F
	movl	$0, 382(%rsp)
	movl	$0, 390(%rsp)
	movq	window.0(%rip), %rsi
	movb	$100, 132(%rsp)
	movb	$115, 133(%rsp)
	movb	$111, 134(%rsp)
	movb	$117, 135(%rsp)
	movb	$110, 136(%rsp)
	movb	$100, 137(%rsp)
	movb	$46, 138(%rsp)
	movb	$100, 139(%rsp)
	movb	$108, 140(%rsp)
	movb	$108, 141(%rsp)
	movb	$0, 142(%rsp)
	movdqa	__xmm@7461657243646e756f53746365726944(%rip), %xmm0 # xmm0 = [68,105,114,101,99,116,83,111,117,110,100,67,114,101,97,116]
	movdqa	%xmm0, 112(%rsp)
	movw	$101, 128(%rsp)
	leaq	132(%rsp), %rcx
	callq	LoadLibraryA
	testq	%rax, %rax
	je	.LBB5_52
# BB#11:                                # %cstr.exit18.i.i
	leaq	112(%rsp), %rdx
	movq	%rax, %rcx
	callq	GetProcAddress
	movq	%rax, %rbp
	testq	%rbp, %rbp
	je	.LBB5_52
# BB#12:                                # %assert.exit22.i.i
	leaq	176(%rsp), %rdx
	xorl	%ecx, %ecx
	xorl	%r8d, %r8d
	callq	*%rbp
	testl	%eax, %eax
	jne	.LBB5_52
# BB#13:                                # %assert.exit26.i.i
	movq	176(%rsp), %rcx
	movq	(%rcx), %rax
	movl	$2, %r8d
	movq	%rsi, %rdx
	callq	*48(%rax)
	testl	%eax, %eax
	jne	.LBB5_52
# BB#14:                                # %assert.exit24.i.i
	movl	$1, 292(%rsp)
	movl	$24, 288(%rsp)
	pxor	%xmm0, %xmm0
	movdqu	%xmm0, 296(%rsp)
	movq	176(%rsp), %rcx
	movq	(%rcx), %rax
	leaq	288(%rsp), %rdx
	leaq	464(%rsp), %r8
	xorl	%r9d, %r9d
	callq	*24(%rax)
	testl	%eax, %eax
	jne	.LBB5_52
# BB#15:                                # %assert.exit20.i.i
	movw	$1, 216(%rsp)
	movw	$2, 218(%rsp)
	movw	$16, 230(%rsp)
	movl	$48000, 220(%rsp)       # imm = 0xBB80
	movw	$4, 228(%rsp)
	movl	$192000, 224(%rsp)      # imm = 0x2EE00
	movw	$0, 232(%rsp)
	movq	464(%rsp), %rcx
	movq	(%rcx), %rax
	leaq	216(%rsp), %rsi
	movq	%rsi, %rdx
	callq	*112(%rax)
	testl	%eax, %eax
	jne	.LBB5_52
# BB#16:                                # %assert.exit11.i.i
	movq	%rsi, 160(%rsp)
	movdqa	__xmm@000000000002ee000000000000000018(%rip), %xmm0 # xmm0 = [24,0,192000,0]
	movdqa	%xmm0, 144(%rsp)
	movq	176(%rsp), %rcx
	movq	(%rcx), %rax
	leaq	144(%rsp), %rdx
	leaq	456(%rsp), %r8
	xorl	%r9d, %r9d
	callq	*24(%rax)
	testl	%eax, %eax
	jne	.LBB5_52
# BB#17:                                # %init_direct_sound.exit.i
	movq	456(%rsp), %rsi
	movq	%rsi, 352(%rsp)
	leaq	352(%rsp), %rcx
	xorl	%edx, %edx
	movl	$12800, %r8d            # imm = 0x3200
	callq	fill_sound_buffer
	movq	(%rsi), %rax
	xorl	%edx, %edx
	xorl	%r8d, %r8d
	movl	$1, %r9d
	movq	%rsi, 408(%rsp)         # 8-byte Spill
	movq	%rsi, %rcx
	callq	*96(%rax)
	testl	%eax, %eax
	jne	.LBB5_52
# BB#18:                                # %assert.exit.i
	movb	$1, running(%rip)
	leaq	272(%rsp), %rcx
	callq	QueryPerformanceCounter
	movzbl	running(%rip), %eax
	cmpl	$1, %eax
	jne	.LBB5_51
# BB#19:                                # %while_cond90.preheader.lr.ph.i
	xorl	%r14d, %r14d
	leaq	560(%rsp), %rdi
	movdqa	__xmm@00000003000000020000000100000000(%rip), %xmm8 # xmm8 = [0,1,2,3]
	movdqa	__xmm@00000007000000060000000500000004(%rip), %xmm7 # xmm7 = [4,5,6,7]
	movdqa	__xmm@000000ff000000ff000000ff000000ff(%rip), %xmm6 # xmm6 = [255,0,0,0,255,0,0,0,255,0,0,0,255,0,0,0]
	movss	__real@40c90fdb(%rip), %xmm9 # xmm9 = mem[0],zero,zero,zero
	xorps	%xmm10, %xmm10
	jmp	.LBB5_20
	.p2align	4, 0x90
.LBB5_23:                               # %endif99.i
                                        #   in Loop: Header=BB5_20 Depth=1
	movq	%rdi, %rcx
	callq	TranslateMessage
	movq	%rdi, %rcx
	callq	DispatchMessageA
.LBB5_20:                               # %while_cond90.preheader.i
                                        # =>This Loop Header: Depth=1
                                        #     Child Loop BB5_27 Depth 2
                                        #       Child Loop BB5_31 Depth 3
                                        #       Child Loop BB5_37 Depth 3
                                        #     Child Loop BB5_45 Depth 2
                                        #     Child Loop BB5_48 Depth 2
	movl	$1, 32(%rsp)
	xorl	%edx, %edx
	xorl	%r8d, %r8d
	xorl	%r9d, %r9d
	movq	%rdi, %rcx
	callq	PeekMessageA
	testl	%eax, %eax
	je	.LBB5_24
# BB#21:                                # %while91.i
                                        #   in Loop: Header=BB5_20 Depth=1
	cmpl	$18, 568(%rsp)
	jne	.LBB5_23
# BB#22:                                # %then98.i
                                        #   in Loop: Header=BB5_20 Depth=1
	movb	$0, running(%rip)
	jmp	.LBB5_23
.LBB5_24:                               # %while_end92.i
                                        #   in Loop: Header=BB5_20 Depth=1
	movq	%r14, 416(%rsp)         # 8-byte Spill
	movq	backbuffer+56(%rip), %r8
	testl	%r8d, %r8d
	jle	.LBB5_39
# BB#25:                                # %for.lr.ph.i.i
                                        #   in Loop: Header=BB5_20 Depth=1
	movl	backbuffer+52(%rip), %ecx
	testl	%ecx, %ecx
	jle	.LBB5_39
# BB#26:                                # %for.us.preheader.i.i
                                        #   in Loop: Header=BB5_20 Depth=1
	movl	x_offset(%rip), %eax
	movl	y_offset(%rip), %edx
	movq	%r8, %r14
	sarq	$32, %r14
	movq	backbuffer+44(%rip), %r11
	leal	-1(%rcx), %r10d
	incq	%r10
	movq	%r10, %r9
	movabsq	$8589934584, %rsi       # imm = 0x1FFFFFFF8
	andq	%rsi, %r9
	leaq	-8(%r9), %rsi
	shrq	$3, %rsi
	leal	1(%rsi), %edi
	movd	%eax, %xmm0
	pshufd	$0, %xmm0, %xmm0        # xmm0 = xmm0[0,0,0,0]
	andl	$1, %edi
	movq	%rdi, 432(%rsp)         # 8-byte Spill
	decq	%rdi
	movq	%rsi, 440(%rsp)         # 8-byte Spill
	subq	%rsi, %rdi
	movq	%rdi, 424(%rsp)         # 8-byte Spill
	xorl	%r15d, %r15d
	.p2align	4, 0x90
.LBB5_27:                               # %for.us.i.i
                                        #   Parent Loop BB5_20 Depth=1
                                        # =>  This Loop Header: Depth=2
                                        #       Child Loop BB5_31 Depth 3
                                        #       Child Loop BB5_37 Depth 3
	leal	(%r15,%rdx), %edi
	shll	$8, %edi
	cmpq	$8, %r10
	movzwl	%di, %r12d
	movq	%r11, %rdi
	movl	$0, %ebx
	jb	.LBB5_36
# BB#28:                                # %min.iters.checked
                                        #   in Loop: Header=BB5_27 Depth=2
	testq	%r9, %r9
	movq	%r11, %rdi
	movl	$0, %ebx
	je	.LBB5_36
# BB#29:                                # %vector.ph
                                        #   in Loop: Header=BB5_27 Depth=2
	movq	%r14, %rsi
	cmpq	$0, 440(%rsp)           # 8-byte Folded Reload
	movd	%r12d, %xmm1
	pshufd	$0, %xmm1, %xmm1        # xmm1 = xmm1[0,0,0,0]
	movl	$0, %r14d
	je	.LBB5_32
# BB#30:                                # %vector.ph.new
                                        #   in Loop: Header=BB5_27 Depth=2
	movq	424(%rsp), %r13         # 8-byte Reload
	xorl	%r14d, %r14d
	.p2align	4, 0x90
.LBB5_31:                               # %vector.body
                                        #   Parent Loop BB5_20 Depth=1
                                        #     Parent Loop BB5_27 Depth=2
                                        # =>    This Inner Loop Header: Depth=3
	movd	%r14d, %xmm2
	pshufd	$0, %xmm2, %xmm2        # xmm2 = xmm2[0,0,0,0]
	paddd	%xmm0, %xmm2
	movdqa	%xmm2, %xmm3
	paddd	%xmm8, %xmm3
	paddd	%xmm7, %xmm2
	pand	%xmm6, %xmm3
	pand	%xmm6, %xmm2
	por	%xmm1, %xmm3
	por	%xmm1, %xmm2
	movdqu	%xmm3, (%r11,%r14,4)
	movdqu	%xmm2, 16(%r11,%r14,4)
	leal	8(%r14), %edi
	movd	%edi, %xmm2
	pshufd	$0, %xmm2, %xmm2        # xmm2 = xmm2[0,0,0,0]
	paddd	%xmm0, %xmm2
	movdqa	%xmm2, %xmm3
	paddd	%xmm8, %xmm3
	paddd	%xmm7, %xmm2
	pand	%xmm6, %xmm3
	pand	%xmm6, %xmm2
	por	%xmm1, %xmm3
	por	%xmm1, %xmm2
	movdqu	%xmm3, 32(%r11,%r14,4)
	movdqu	%xmm2, 48(%r11,%r14,4)
	addq	$16, %r14
	addq	$2, %r13
	jne	.LBB5_31
.LBB5_32:                               # %middle.block.unr-lcssa
                                        #   in Loop: Header=BB5_27 Depth=2
	cmpq	$0, 432(%rsp)           # 8-byte Folded Reload
	je	.LBB5_34
# BB#33:                                # %vector.body.epil
                                        #   in Loop: Header=BB5_27 Depth=2
	movd	%r14d, %xmm2
	pshufd	$0, %xmm2, %xmm2        # xmm2 = xmm2[0,0,0,0]
	paddd	%xmm0, %xmm2
	movdqa	%xmm2, %xmm3
	paddd	%xmm8, %xmm3
	paddd	%xmm7, %xmm2
	pand	%xmm6, %xmm3
	pand	%xmm6, %xmm2
	por	%xmm1, %xmm3
	por	%xmm1, %xmm2
	movdqu	%xmm3, (%r11,%r14,4)
	movdqu	%xmm2, 16(%r11,%r14,4)
.LBB5_34:                               # %middle.block
                                        #   in Loop: Header=BB5_27 Depth=2
	cmpq	%r9, %r10
	movq	%rsi, %r14
	je	.LBB5_38
# BB#35:                                #   in Loop: Header=BB5_27 Depth=2
	leaq	(%r11,%r9,4), %rdi
	movl	%r9d, %ebx
	.p2align	4, 0x90
.LBB5_36:                               # %for11.us.i.i.preheader
                                        #   in Loop: Header=BB5_27 Depth=2
	movl	%ecx, %ebp
	subl	%ebx, %ebp
	addl	%eax, %ebx
	.p2align	4, 0x90
.LBB5_37:                               # %for11.us.i.i
                                        #   Parent Loop BB5_20 Depth=1
                                        #     Parent Loop BB5_27 Depth=2
                                        # =>    This Inner Loop Header: Depth=3
	movzbl	%bl, %esi
	orl	%r12d, %esi
	movl	%esi, (%rdi)
	addq	$4, %rdi
	incl	%ebx
	decl	%ebp
	jne	.LBB5_37
.LBB5_38:                               # %for_cond10.end_for13_crit_edge.us.i.i
                                        #   in Loop: Header=BB5_27 Depth=2
	addq	%r14, %r11
	incl	%r15d
	cmpl	%r8d, %r15d
	jne	.LBB5_27
.LBB5_39:                               # %render_weird_gradient.exit.i
                                        #   in Loop: Header=BB5_20 Depth=1
	movq	408(%rsp), %rcx         # 8-byte Reload
	movq	(%rcx), %rax
	leaq	188(%rsp), %rdx
	leaq	476(%rsp), %r8
	callq	*32(%rax)
	testl	%eax, %eax
	jne	.LBB5_52
# BB#40:                                # %assert.exit51.i
                                        #   in Loop: Header=BB5_20 Depth=1
	movl	364(%rsp), %eax
	imull	382(%rsp), %eax
	movq	368(%rsp), %rcx
	xorl	%edx, %edx
	divl	%ecx
	movl	%edx, %ebp
	movq	%rcx, %rax
	shrq	$32, %rax
	addl	188(%rsp), %eax
	xorl	%edx, %edx
	divl	%ecx
	subl	%ebp, %edx
	movl	$0, %r8d
	cmovll	%ecx, %r8d
	addl	%edx, %r8d
	leaq	352(%rsp), %rcx
	movl	%ebp, %edx
	callq	fill_sound_buffer
	movss	390(%rsp), %xmm0        # xmm0 = mem[0],zero,zero,zero
	ucomiss	__real@44fa0000(%rip), %xmm0
	movq	416(%rsp), %r14         # 8-byte Reload
	jbe	.LBB5_42
# BB#41:                                # %then143.i
                                        #   in Loop: Header=BB5_20 Depth=1
	movaps	%xmm0, %xmm1
	divss	%xmm9, %xmm1
	roundss	$9, %xmm1, %xmm1
	mulss	%xmm9, %xmm1
	subss	%xmm1, %xmm0
	movss	%xmm0, 390(%rsp)
.LBB5_42:                               # %endif144.i
                                        #   in Loop: Header=BB5_20 Depth=1
	movq	window.1(%rip), %rcx
	movl	window.2(%rip), %r9d
	movl	window.3(%rip), %eax
	movl	backbuffer+52(%rip), %edx
	movl	backbuffer+56(%rip), %ebp
	movq	backbuffer+44(%rip), %rbx
	leaq	backbuffer(%rip), %rdi
	movq	%rdi, 80(%rsp)
	movq	%rbx, 72(%rsp)
	movl	%ebp, 64(%rsp)
	movl	%edx, 56(%rsp)
	movl	%eax, 32(%rsp)
	movl	$13369376, 96(%rsp)     # imm = 0xCC0020
	movl	$0, 88(%rsp)
	movl	$0, 48(%rsp)
	movl	$0, 40(%rsp)
	xorl	%edx, %edx
	xorl	%r8d, %r8d
	callq	StretchDIBits
	incl	z_offset(%rip)
	incl	%r14d
	leaq	280(%rsp), %rcx
	callq	QueryPerformanceCounter
	movq	%r14, %rax
	movl	$2290649225, %ecx       # imm = 0x88888889
	imulq	%rcx, %rax
	shrq	$38, %rax
	imull	$120, %eax, %eax
	cmpl	%eax, %r14d
	jne	.LBB5_50
# BB#43:                                # %then162.i
                                        #   in Loop: Header=BB5_20 Depth=1
	movq	280(%rsp), %rax
	subq	272(%rsp), %rax
	imulq	$1000, %rax, %rax       # imm = 0x3E8
	cqto
	idivq	448(%rsp)
	movq	%rax, %rbp
	movb	$58, 144(%rsp)
	movb	$32, 145(%rsp)
	movb	$10, 112(%rsp)
	movq	console_output_handle(%rip), %rcx
	movq	$0, 32(%rsp)
	movl	$5, %r8d
	xorl	%r9d, %r9d
	leaq	171(%rsp), %rdx
	callq	WriteFile
	movq	console_output_handle(%rip), %rcx
	movq	$0, 32(%rsp)
	movl	$2, %r8d
	xorl	%r9d, %r9d
	leaq	144(%rsp), %rdx
	callq	WriteFile
	movaps	%xmm10, 336(%rsp)
	movaps	%xmm10, 320(%rsp)
	movaps	%xmm10, 304(%rsp)
	movaps	%xmm10, 288(%rsp)
	movq	decimal_digits+4(%rip), %rdi
	movq	%rbp, %rax
	movabsq	$-3689348814741910323, %rsi # imm = 0xCCCCCCCCCCCCCCCD
	movq	%rsi, %r9
	mulq	%r9
	shrq	$2, %rdx
	movabsq	$4611686018427387902, %rsi # imm = 0x3FFFFFFFFFFFFFFE
	movq	%rsi, %r10
	andq	%r10, %rdx
	leaq	(%rdx,%rdx,4), %rax
	movq	%rbp, %rcx
	subq	%rax, %rcx
	movb	(%rdi,%rcx), %al
	movb	%al, 288(%rsp)
	leaq	9(%rbp), %rax
	movl	$1, %r8d
	cmpq	$19, %rax
	movabsq	$7378697629483820647, %rsi # imm = 0x6666666666666667
	jb	.LBB5_46
# BB#44:                                # %while_cond.while_cond_crit_edge.i.i.i.i.preheader
                                        #   in Loop: Header=BB5_20 Depth=1
	movl	$1, %r8d
	.p2align	4, 0x90
.LBB5_45:                               # %while_cond.while_cond_crit_edge.i.i.i.i
                                        #   Parent Loop BB5_20 Depth=1
                                        # =>  This Inner Loop Header: Depth=2
	movq	%rbp, %rax
	imulq	%rsi
	movq	%rdx, %rcx
	movq	%rcx, %rbx
	shrq	$63, %rbx
	sarq	$2, %rcx
	leaq	(%rcx,%rbx), %rbp
	movq	%rbp, %rax
	mulq	%r9
	shrq	$2, %rdx
	andq	%r10, %rdx
	leaq	(%rdx,%rdx,4), %rax
	movq	%rbp, %rdx
	subq	%rax, %rdx
	movzbl	(%rdi,%rdx), %eax
	movslq	%r8d, %rdx
	incl	%r8d
	movb	%al, 288(%rsp,%rdx)
	leaq	9(%rcx,%rbx), %rax
	cmpq	$18, %rax
	ja	.LBB5_45
.LBB5_46:                               # %while_end.i.i.i.i
                                        #   in Loop: Header=BB5_20 Depth=1
	movslq	%r8d, %rax
	leaq	288(%rsp,%rax), %rax
	leaq	288(%rsp), %rdx
	cmpq	%rax, %rdx
	jge	.LBB5_49
# BB#47:                                # %while16.i.i.i.i.preheader
                                        #   in Loop: Header=BB5_20 Depth=1
	leaq	289(%rsp), %rcx
	.p2align	4, 0x90
.LBB5_48:                               # %while16.i.i.i.i
                                        #   Parent Loop BB5_20 Depth=1
                                        # =>  This Inner Loop Header: Depth=2
	movzbl	-1(%rax), %r9d
	movzbl	-1(%rcx), %ebx
	movb	%bl, -1(%rax)
	decq	%rax
	movb	%r9b, -1(%rcx)
	cmpq	%rax, %rcx
	leaq	1(%rcx), %rcx
	jl	.LBB5_48
.LBB5_49:                               # %debug_print_u64.exit.i
                                        #   in Loop: Header=BB5_20 Depth=1
	movq	console_output_handle(%rip), %rcx
	movq	$0, 32(%rsp)
	xorl	%r9d, %r9d
	callq	WriteFile
	movq	console_output_handle(%rip), %rcx
	movq	$0, 32(%rsp)
	movl	$1, %r8d
	xorl	%r9d, %r9d
	leaq	112(%rsp), %rdx
	callq	WriteFile
.LBB5_50:                               # %endif163.i
                                        #   in Loop: Header=BB5_20 Depth=1
	movq	280(%rsp), %rax
	movq	%rax, 272(%rsp)
	movb	running(%rip), %al
	testb	%al, %al
	leaq	560(%rsp), %rdi
	jne	.LBB5_20
.LBB5_51:                               # %main.exit
	xorl	%ecx, %ecx
	callq	ExitProcess
	movaps	608(%rsp), %xmm6        # 16-byte Reload
	movaps	624(%rsp), %xmm7        # 16-byte Reload
	movaps	640(%rsp), %xmm8        # 16-byte Reload
	movaps	656(%rsp), %xmm9        # 16-byte Reload
	movaps	672(%rsp), %xmm10       # 16-byte Reload
	addq	$696, %rsp              # imm = 0x2B8
	popq	%rbx
	popq	%rbp
	popq	%rdi
	popq	%rsi
	popq	%r12
	popq	%r13
	popq	%r14
	popq	%r15
	retq
.LBB5_52:                               # %then.i49.i
	ud2
	ud2

	.lcomm	console_output_handle,8,8 # @console_output_handle
	.lcomm	XInputGetState,8,8      # @XInputGetState
	.lcomm	decimal_digits,12,8     # @decimal_digits
	.lcomm	running,1               # @running
	.lcomm	backbuffer,64,16        # @backbuffer
	.lcomm	x_offset,4,4            # @x_offset
	.lcomm	y_offset,4,4            # @y_offset
	.lcomm	z_offset,4,4            # @z_offset
	.lcomm	window.0,8,8            # @window.0
	.lcomm	window.1,8,8            # @window.1
	.lcomm	window.2,4,8            # @window.2
	.lcomm	window.3,4,4            # @window.3

