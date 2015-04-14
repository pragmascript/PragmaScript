# PragmaScript

## Syntax

```
let x = 5.0; // constant
var y = 3.0; // mutable variable
y = 2.0; 

// function
let add = (x: int32, y: int32) -> { return x + y; }
let add1 = (x: int32) -> { return x + 1; }

// function pointer (delegate)
var foo = (x: int32) -> { return x + 2; }

// allows reassignment
foo = (x: int32) -> { return x + 3; }

// higher order function
var apply = (x: int, f: (int32) -> (int32)) { return f(x); }

apply(12, add1);
```
