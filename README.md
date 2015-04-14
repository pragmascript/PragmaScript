# PragmaScript

## Syntax

```
let x = 5.0;
var y = 5.0;

let add = (x: int32, y: int32) => (int32) { return x + y; }
let add1 = (x: int32) -> (int32) { return x + 1; }
var add2 = (x: int32) -> (int32) { return x + 2; }
var apply = (x: int, f: (int32) -> (int32, float32)) -> (int)
{
	return f(x);	
}

apply(12, add1);
```