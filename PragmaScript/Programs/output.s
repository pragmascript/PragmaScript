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

	.def	 __chkstk;
	.scl	2;
	.type	32;
	.endef
	.globl	__chkstk
	.p2align	4, 0x90
__chkstk:                               # @__chkstk
# BB#0:
	#APP
	pushq	%rcx
	pushq	%rax
	cmpq	$4096, %rax             # imm = 0x1000
	leaq	24(%rsp), %rcx
	jb	.Ltmp0
.Ltmp1:
	subq	$4096, %rcx             # imm = 0x1000
	orl	$0, (%rcx)
	subq	$4096, %rax             # imm = 0x1000
	cmpq	$4096, %rax             # imm = 0x1000
	ja	.Ltmp1
.Ltmp0:
	subq	%rax, %rcx
	orl	$0, (%rcx)
	popq	%rax
	popq	%rcx
	retq

	#NO_APP
	retq

	.def	 __init;
	.scl	2;
	.type	32;
	.endef
	.globl	__init
	.p2align	4, 0x90
__init:                                 # @__init
.Ltmp2:
.seh_proc __init
# BB#0:                                 # %vars
	subq	$40, %rsp
.Ltmp3:
	.seh_stackalloc 40
.Ltmp4:
	.seh_endprologue
	callq	main
	nop
	addq	$40, %rsp
	retq
	.seh_handlerdata
	.text
.Ltmp5:
	.seh_endproc

	.def	 main;
	.scl	3;
	.type	32;
	.endef
	.p2align	4, 0x90
main:                                   # @main
# BB#0:                                 # %entry
	subq	$16, %rsp
	movl	$12, 8(%rsp)
	movl	$0, 12(%rsp)
	movb	$1, %al
	cmpl	$0, 8(%rsp)
	je	.LBB3_3
# BB#1:                                 # %cor.rhs
	cmpl	$0, 8(%rsp)
	jne	.LBB3_3
# BB#2:                                 # %cor.rhs4
	cmpl	$0, 12(%rsp)
	setne	%al
.LBB3_3:                                # %cor.end
	movb	%al, 7(%rsp)
	addq	$16, %rsp
	retq


