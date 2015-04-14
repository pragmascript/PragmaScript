# PragmaScript

## Syntax

```
let x = 5.0; // constant (assignment not allowed)
var y = 5.0; // variable (assignment allowed)

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
