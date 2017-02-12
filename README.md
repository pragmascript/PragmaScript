# PragmaScript

## Syntax Example (everything is still in flux)

```csharp
import "preamble.prag"

[
	"compile.output": "hello_world.exe",
	"compile.entry" : "true",
	"compile.debug" : "true",
 	"compile.opt"   : "0",
 	"compile.run"   : "true",
 	"compile.libs"  : "kernel32.lib",
	"compile.path"  : "C:\Program Files (x86)\Windows Kits\10\Lib\10.0.14393.0\um\x64"
]
let main = fun () => void 
{
	print_string("hello, world!\n");
	for (var i = 0; i < 12; ++i) {
		print_i32(i + 1);
		if (i != 11) {
			print_string(", ");
		}
	}
	print_string("\n");
}

```
