# PragmaScript

## Syntax Example (everything is still in flux)

```
let print_int32_array = (a: int32[]) => 
{
	for (var i = 0; i < a.length; ++i)
	{
		print_i32(a[i]);
		if (i < a.length - 1)
		{
			print(", ");
		}
	}
}

var x = [1,2,3,4,12];
print_int32_array(x);

```
