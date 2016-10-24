using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace PragmaScript
{
    partial class AST
    {

        public abstract class Node
        {
            public Token token;

            public Node(Token t)
            {
                token = t;
            }

            public virtual IEnumerable<Node> GetChilds()
            {
                yield break;
            }

            public abstract Task<FrontendType> CheckType(Scope scope);
        }

        public interface ICanReturnPointer
        {
            bool returnPointer { get; set; }
        }

        public class AnnotatedNode : Node
        {
            Node node;
            public string annotation;
            public AnnotatedNode(Node n, string annotation)
                : base(n.token)
            {
                node = n;
                this.annotation = annotation;
            }

            public override IEnumerable<Node> GetChilds()
            {
                foreach (var n in node.GetChilds())
                {
                    yield return n;
                }
            }

            public override async Task<FrontendType> CheckType(Scope scope)
            {
                return await node.CheckType(scope);
            }

            public override string ToString()
            {
                return node.ToString();
            }
        }


        public class Root : Node
        {
            public Scope scope;
            public List<Node> declarations = new List<Node>();
            public Root(Token t)
                : base(t)
            {
            }
            public override IEnumerable<Node> GetChilds()
            {
                foreach (var s in declarations)
                    yield return s;
            }
            public override async Task<FrontendType> CheckType(Scope scope)
            {
                var types = await Task.WhenAll(declarations.Select(s => s.CheckType(this.scope)));
                return null;
            }

            public override string ToString()
            {
                return "Root";
            }
        }

        public class Block : Node
        {
            public Scope scope;
            public List<Node> statements = new List<Node>();
            public Block(Token t)
                : base(t)
            {
            }
            public override IEnumerable<Node> GetChilds()
            {
                foreach (var s in statements)
                    yield return s;
            }
            public override async Task<FrontendType> CheckType(Scope scope)
            {

                var types = await Task.WhenAll(statements.Select(s => s.CheckType(this.scope)));
                return null;
            }

            public override string ToString()
            {
                return "Block";
            }
        }

        public class Elif : Node
        {
            public Node condition;
            public Node thenBlock;

            public Elif(Token t)
                : base(t)
            {
            }

            public override IEnumerable<Node> GetChilds()
            {
                yield return new AnnotatedNode(condition, "condition");
                yield return thenBlock;
            }

            public override async Task<FrontendType> CheckType(Scope scope)
            {
                var ct = await condition.CheckType(scope);
                if (!ct.Equals(FrontendType.bool_))
                    throw new ParserExpectedType(FrontendType.bool_, ct, condition.token);

                await thenBlock.CheckType(scope);

                return null;
            }

            public override string ToString()
            {
                return "elif";
            }
        }

        public class IfCondition : Node
        {
            public Node condition;
            public Node thenBlock;
            public List<Node> elifs = new List<Node>();
            public Node elseBlock;
            public IfCondition(Token t)
                : base(t)
            {
            }
            public override IEnumerable<Node> GetChilds()
            {
                yield return new AnnotatedNode(condition, "condition");
                yield return new AnnotatedNode(thenBlock, "then"); 
                foreach (var elif in elifs)
                {
                    yield return new AnnotatedNode(elif, "elif");
                }
                if (elseBlock != null)
                {
                    yield return new AnnotatedNode(elseBlock, "else");
                }
            }
            public override async Task<FrontendType> CheckType(Scope scope)
            {
                var ct = await condition.CheckType(scope);
                if (!ct.Equals(FrontendType.bool_))
                    throw new ParserExpectedType(FrontendType.bool_, ct, condition.token);

                await thenBlock.CheckType(scope);

                await Task.WhenAll(elifs.Select(e => e.CheckType(scope)));

                //foreach (var elif in elifs)
                //{
                //    elif.CheckType(scope);
                //}

                if (elseBlock != null)
                    await elseBlock.CheckType(scope);

                return null;
            }

            public override string ToString()
            {
                return "if";
            }
        }

        public class ForLoop : Node
        {
            public List<Node> initializer;
            public Node condition;
            public List<Node> iterator;

            public Node loopBody;

            public ForLoop(Token t)
                : base(t)
            {
            }
            public override IEnumerable<Node> GetChilds()
            {
                int idx = 1;
                foreach (var init in initializer)
                {
                    yield return new AnnotatedNode(init, "init_" + idx);
                    idx++;
                }
                
                yield return new AnnotatedNode(condition, "condition");

                idx = 1;
                foreach (var it in iterator)
                {
                    yield return new AnnotatedNode(it, "iter_" + idx);
                    idx++;
                }
                
                yield return new AnnotatedNode(loopBody, "body");
            }
            public override async Task<FrontendType> CheckType(Scope scope)
            {
                var loopBodyScope = (loopBody as Block).scope;

                if (initializer.Count > 0)
                {
                    await Task.WhenAll(initializer.Select(init => init.CheckType(loopBodyScope)));
                }
                
                var ct = await condition.CheckType(loopBodyScope);
                if (!ct.Equals(FrontendType.bool_))
                    throw new ParserExpectedType(FrontendType.bool_, ct, condition.token);

                if (iterator.Count > 0)
                {
                    await Task.WhenAll(iterator.Select(iter => iter.CheckType(loopBodyScope)));
                }
                
                await loopBody.CheckType(scope);
                return null;
            }
            public override string ToString()
            {
                return "for";
            }
        }

        public class WhileLoop : Node
        {
            public Node condition;
            public Node loopBody;

            public WhileLoop(Token t)
                : base(t)
            {
            }
            public override IEnumerable<Node> GetChilds()
            {
                yield return new AnnotatedNode(condition, "condition");
                yield return new AnnotatedNode(loopBody, "body");
            }
            public override async Task<FrontendType> CheckType(Scope scope)
            {
                var loopBodyScope = (loopBody as Block).scope;

                var ct = await condition.CheckType(loopBodyScope);
                if (!ct.Equals(FrontendType.bool_))
                    throw new ParserExpectedType(FrontendType.bool_, ct, condition.token);
                await loopBody.CheckType(scope);
                return null;
            }
            public override string ToString()
            {
                return "while";
            }
        }


      
        public class VariableDefinition : Node
        {
            public Scope.VariableDefinition variable;
            public Node expression;

            public VariableDefinition(Token t)
                : base(t)
            {
            }

            public override IEnumerable<Node> GetChilds()
            {
                yield return expression;
            }
            public override async Task<FrontendType> CheckType(Scope scope)
            {
                var type = await expression.CheckType(scope);
                variable.type = type;
                return type;
            }
            public override string ToString()
            {
                return (variable.isConstant ? "var " : "let ")
                    + variable.name + " = ";

            }
        }

        public class FunctionDefinition : Node
        {
            public Node body;
            public FrontendFunctionType fun;
            public string funName;
            public bool external;

            public FunctionDefinition(Token t)
                : base(t)
            {
            }

            public override IEnumerable<Node> GetChilds()
            {
                if (!external)
                {
                    yield return body;
                }
                else
                {
                    yield break;
                }
                
            }
            public override async Task<FrontendType> CheckType(Scope scope)
            {
                if (external)
                {
                    return await Task.FromResult<FrontendType>(null);
                }
                await body.CheckType(scope);
                if (fun.returnType == null)
                {
                    fun.returnType = FrontendType.void_;
                }
                return await Task.FromResult<FrontendType>(null);
            }
            public override string ToString()
            {
                string result = (external ? "extern " : "") + funName + fun.ToString();
                return result;
            }
        }

        public class StructConstructor : Node
        {
            public string structName;
            public List<Node> argumentList = new List<Node>();
            public FrontendStructType structType;

            public StructConstructor(Token t)
                : base(t)
            {
            }

            public override IEnumerable<Node> GetChilds()
            {
                foreach (var a in argumentList)
                {
                    yield return a;
                }
            }

            public override async Task<FrontendType> CheckType(Scope scope)
            {
                var _type = scope.GetType(structName);
                
                if (!(_type is FrontendStructType))
                {
                    throw new ParserErrorExpected("struct type", _type.name, token);
                }

                structType = _type as FrontendStructType;

                int idx = 0;
                var args = await Task.WhenAll(argumentList.Select(arg => arg.CheckType(scope)));

                foreach (var targ in args)
                {
                    var fieldType = structType.fields[idx++].type;
                    if (!targ.Equals(fieldType))
                    {
                        throw new ParserExpectedArgumentType(fieldType, targ, idx + 1, token);
                    }
                }
                
                return structType;
            }

            public override string ToString()
            {
                return structName + "{ }";
            }
        }

        public class StructDefinition : Node
        {
            public FrontendStructType type;
            public StructDefinition(Token t)
                : base(t)
            {

            }

            public override async Task<FrontendType> CheckType(Scope scope)
            {
                return await Task.FromResult(type);
            }

            public override string ToString()
            {
                var fields = string.Join(", ", type.fields.Select(f => f.name + ": " + f.type));
                return type.name + " = struct { " + fields + " }";
            }

        }

        public class FunctionCall : Node
        {
            public string functionName;
            public List<Node> argumentList = new List<Node>();
            public FrontendType returnType;

            public FunctionCall(Token t)
                : base(t)
            {
            }
            public override IEnumerable<Node> GetChilds()
            {
                foreach (var exp in argumentList)
                {
                    yield return exp;
                }
            }
            public override async Task<FrontendType> CheckType(Scope scope)
            {
                var fun_var = scope.GetVar(functionName);
                if (fun_var == null)
                {
                    throw new ParserError($"Unknown function of name: \"{functionName}\"", token);
                }

                var fun = fun_var.type as FrontendFunctionType;
                if (fun == null)
                {
                    throw new ParserError($"Variable \"{fun_var.name}\" is not a function and cannot be called.", token);
                }

                var args = await Task.WhenAll(argumentList.Select(arg => arg.CheckType(scope)));
                if (args.Length != fun.parameters.Count)
                {
                    throw new ParserError($"Function argument count mismatch! Got {args.Length} expected {fun.parameters.Count}.", token);
                }

                for (int i = 0; i < fun.parameters.Count; ++i)
                {
                    var arg = args[i];

                    if (!arg.Equals(fun.parameters[i].type))
                    {
                        throw new ParserExpectedArgumentType(fun.parameters[i].type, arg, i + 1, token);
                    }
                }
                returnType = fun.returnType;
                return returnType;
            }
            public override string ToString()
            {
                return functionName + "()";
            }
        }

        public class VariableReference : Node, ICanReturnPointer
        {
            public enum Incrementor { None, preIncrement, preDecrement, postIncrement, postDecrement }
            public Incrementor inc;
            public string variableName;
            public Scope.VariableDefinition varDefinition;

            // HACK: returnPointer is a HACK remove this?????
            public bool returnPointer { get; set; }
            public VariableReference(Token t)
                : base(t)
            {
            }
            public override async Task<FrontendType> CheckType(Scope scope)
            {
                var v = scope.GetVar(variableName);
                Debug.Assert(v != null);
                varDefinition = v;

                while(v.type == null)
                {
                    // TODO: use TaskCompletionSource instead
                    // http://stackoverflow.com/questions/15122936/write-an-async-method-that-will-await-a-bool
                    await Task.Yield();
                }
                return v.type;
            }
            public override string ToString()
            {
                switch (inc)
                {
                    case Incrementor.None:
                        return variableName + (returnPointer ? " (p)" : "");
                    case Incrementor.preIncrement:
                        return "++" + variableName;
                    case Incrementor.preDecrement:
                        return "--" + variableName;
                    case Incrementor.postIncrement:
                        return variableName + "++";
                    case Incrementor.postDecrement:
                        return variableName + "--";
                    default:
                        throw new InvalidCodePath();
                }
            }
        }

        public class Assignment : Node
        {
            public Node target;
            public Node expression;
            public Node index;

            public Assignment(Token t)
                : base(t)
            {
            }
            public override IEnumerable<Node> GetChilds()
            {
                yield return new AnnotatedNode(target, "target");
                yield return new AnnotatedNode(expression, "expression");
            }
            public override async Task<FrontendType> CheckType(Scope scope)
            {
                var et = await expression.CheckType(scope);
                var tt = await target.CheckType(scope);
                if (!et.Equals(tt))
                {
                    throw new ParserVariableTypeMismatch(tt, et, token);
                }
                return tt;
            }
            public override string ToString()
            {
                return " = ";
            }
        }

        public class ConstInt : Node
        {
            public int number;

            public ConstInt(Token t)
                : base(t)
            {
            }
            public override async Task<FrontendType> CheckType(Scope scope)
            {
                return await Task.FromResult(FrontendType.i32);
            }
            public override string ToString()
            {
                return number.ToString();
            }
        }

        public class ConstFloat : Node
        {
            public double number;
            public ConstFloat(Token t)
                : base(t)
            {
            }
            public override async Task<FrontendType> CheckType(Scope scope)
            {
                return await Task.FromResult(FrontendType.f32);
            }
            public override string ToString()
            {
                return number.ToString("F2", CultureInfo.InvariantCulture);
            }
        }

        public class ConstBool : Node
        {
            public bool value;
            public ConstBool(Token t)
                : base(t)
            {
            }

            public ConstBool(Token t, bool b)
                : base(t)
            {
                // TODO: Complete member initialization
                this.value = b;
            }
            public override async Task<FrontendType> CheckType(Scope scope)
            {
                return await Task.FromResult(FrontendType.bool_);
            }
            public override string ToString()
            {
                return value.ToString();
            }
        }

        public class ConstString : Node
        {
            public string s;

            public ConstString(Token t)
                : base(t)
            {
            }
            public override async Task<FrontendType> CheckType(Scope scope)
            {
                return await Task.FromResult(FrontendType.string_);
            }
            public override string ToString()
            {
                return s;
            }
        }

        public class ArrayConstructor : Node
        {
            public FrontendArrayType type;
            public List<Node> elements = new List<Node>();

            public ArrayConstructor(Token t)
                : base(t)
            {

            }
            public override IEnumerable<Node> GetChilds()
            {
                var idx = 0;
                foreach (var x in elements)
                {
                    yield return new AnnotatedNode(x, "elem_" + idx++);
                }
            }

            public override async Task<FrontendType> CheckType(Scope scope)
            {
                if (elements.Count == 0)
                {
                    throw new ParserError("zero sized array detected", token);
                }
                var ets = await Task.WhenAll(elements.Select(e => e.CheckType(scope)));

                var firstType = ets.First();
                if (!ets.All(e => e.Equals(firstType)))
                {
                    throw new ParserError("all elements in an array must have the same type", token);
                }

                var elementType = firstType;
                type = new FrontendArrayType(elementType);

                return type;

            }
            public override string ToString()
            {
                return "[]";
            }
        }

        public class UninitializedArray : Node
        {
            // public Node length;
            //TODO: change this to work without compiletime constants
            public int length;
            public string elementTypeName;
            public FrontendType elementType;

            public UninitializedArray(Token t)
                : base(t)
            {

            }

            //public override IEnumerable<Node> GetChilds()
            //{
            //    yield return length;
            //}

            public override async Task<FrontendType> CheckType(Scope scope)
            {
                elementType = scope.GetType(elementTypeName);

                //var ct = await length.CheckType(scope);
                //if (ct != FrontendType.int32)
                //{
                //    throw new ParserExpectedType(FrontendType.int32, ct, length.token);
                //}
                
                // TODO: wait for type definition?
                if (elementType == null)
                {
                    throw new UndefinedType(elementTypeName, token);
                }

                return await Task.FromResult(new FrontendArrayType(elementType));
            }

            public override string ToString()
            {
                return elementTypeName + $"[{length}]";
            }
        }

        public class StructFieldAccess : Node, ICanReturnPointer
        {
            public Node left;
            public string fieldName;

            public bool returnPointer { get; set; }

            public FrontendStructType structType;

            public StructFieldAccess(Token t) :
                base(t)
            {

            }

            public override async Task<FrontendType> CheckType(Scope scope)
            {

                var t = await (left.CheckType(scope));
                var st = t as FrontendStructType;
                if (t == null)
                {
                    throw new ParserError("left side is not a struct type", token);
                }
                structType = st;
                var field = st.GetField(fieldName);
                if (field == null)
                {
                    throw new ParserError(
                        string.Format("struct does not contain field \"{0}\"", fieldName), token);
                }
                return field;
            }

            public override IEnumerable<Node> GetChilds()
            {
                yield return left;

            }

            public override string ToString()
            {
                return  "." + fieldName + (returnPointer ? " (p)": "");
            }
        }

        public class ArrayElementAccess : Node, ICanReturnPointer
        {
            public Node left;
            public Node index;

            public bool returnPointer { get; set; }

            public ArrayElementAccess(Token t)
                : base(t)
            {
            }

            public override IEnumerable<Node> GetChilds()
            {
                yield return new AnnotatedNode(left, "array");
                yield return new AnnotatedNode(index, "index");
            }

            public override async Task<FrontendType> CheckType(Scope scope)
            {
                var vt = await (left.CheckType(scope));

                if (!(vt is FrontendArrayType))
                {
                    throw new ParserError("variable is not an array type", token);
                }

                var idxType = await index.CheckType(scope);
                if (!idxType.Equals(FrontendType.i32))
                {
                    throw new ParserExpectedType(FrontendType.i32, idxType, index.token);
                }

                var atype = vt as FrontendArrayType;
                return atype.elementType;
            }
            public override string ToString()
            {
                return "[]" + (returnPointer ? " (p)" : "");
            }
        }

        public class BreakLoop : Node
        {
            public BreakLoop(Token t)
                : base(t)
            {
            }

            public override async Task<FrontendType> CheckType(Scope scope)
            {
                return await Task.FromResult<FrontendType>(null);
            }
            public override string ToString()
            {
                return "break";
            }
        }

        public class ContinueLoop : Node
        {
            public ContinueLoop(Token t)
                : base(t)
            {
            }

            public override async Task<FrontendType> CheckType(Scope scope)
            {
                return await Task.FromResult<FrontendType>(null);
            }

            public override string ToString()
            {
                return "continue";
            }
        }

        public class ReturnFunction : Node
        {
            public Node expression;

            public ReturnFunction(Token t)
                : base(t)
            {
            }
            public override IEnumerable<Node> GetChilds()
            {
                if (expression != null)
                {
                    yield return expression;
                }
            }

            public override async Task<FrontendType> CheckType(Scope scope)
            {
                var result = default(FrontendType);
                if (expression != null)
                {
                    result = await expression.CheckType(scope);
                }
                else
                {
                    result = FrontendType.void_;
                }

                if (scope.function.returnType != null)
                {
                    if (!result.Equals(scope.function.returnType))
                    {
                        throw new ParserError("return statement returns different types in one block", token);
                    }
                }
                else
                {
                    scope.function.returnType = result;
                }
                return result;
            }
            public override string ToString()
            {
                return "return";
            }

        }

        public class BinOp : Node
        {
            public enum BinOpType { Add, Subract, Multiply, Divide, ConditionalOR, ConditionaAND, LogicalOR, LogicalXOR, LogicalAND, Equal, NotEqual, Greater, Less, GreaterEqual, LessEqual, LeftShift, RightShift, Remainder }
            public BinOpType type;

            public Node left;
            public Node right;

            public BinOp(Token t)
                : base(t)
            {
            }
            public void SetTypeFromToken(Token next)
            {
                switch (next.type)
                {
                    case Token.TokenType.Add:
                        type = BinOpType.Add;
                        break;
                    case Token.TokenType.Subtract:
                        type = BinOpType.Subract;
                        break;
                    case Token.TokenType.Multiply:
                        type = BinOpType.Multiply;
                        break;
                    case Token.TokenType.Divide:
                        type = BinOpType.Divide;
                        break;
                    case Token.TokenType.Remainder:
                        type = BinOpType.Remainder;
                        break;
                    case Token.TokenType.LeftShift:
                        type = BinOpType.LeftShift;
                        break;
                    case Token.TokenType.RightShift:
                        type = BinOpType.RightShift;
                        break;
                    case Token.TokenType.ConditionalOR:
                        type = BinOpType.ConditionalOR;
                        break;
                    case Token.TokenType.ConditionalAND:
                        type = BinOpType.ConditionaAND;
                        break;
                    case Token.TokenType.LogicalOR:
                        type = BinOpType.LogicalOR;
                        break;
                    case Token.TokenType.LogicalXOR:
                        type = BinOpType.LogicalXOR;
                        break;
                    case Token.TokenType.LogicalAND:
                        type = BinOpType.LogicalAND;
                        break;
                    case Token.TokenType.Equal:
                        type = BinOpType.Equal;
                        break;
                    case Token.TokenType.NotEqual:
                        type = BinOpType.NotEqual;
                        break;
                    case Token.TokenType.Greater:
                        type = BinOpType.Greater;
                        break;
                    case Token.TokenType.Less:
                        type = BinOpType.Less;
                        break;
                    case Token.TokenType.GreaterEqual:
                        type = BinOpType.GreaterEqual;
                        break;
                    case Token.TokenType.LessEqual:
                        type = BinOpType.LessEqual;
                        break;
                    default:
                        throw new ParserError("Invalid token type for binary operation", next);
                }
            }

            bool isEither(params BinOpType[] types)
            {
                for (int i = 0; i < types.Length; ++i)
                {
                    if (type == types[i])
                        return true;
                }

                return false;
            }

            public override async Task<FrontendType> CheckType(Scope scope)
            {
                
                // System.Console.WriteLine($"W: binop: {this.type}, {this.token}");
                var opTypes = await Task.WhenAll(left.CheckType(scope), right.CheckType(scope));
                // System.Console.WriteLine($"D: binop: {this.type}, {this.token}");
                var lType = opTypes[0];
                var rType = opTypes[1];

                if (isEither(BinOpType.LeftShift, BinOpType.RightShift))
                {
                    // TODO: suppport all integer types here.
                    if (!lType.Equals(FrontendType.i32) || !rType.Equals(FrontendType.i32))
                    {
                        throw new ParserErrorExpected("two integer types", string.Format("{0} and {1}", lType, rType), token);
                    }
                }

                if (lType is FrontendPointerType)
                {
                    if (!isEither(BinOpType.Add, BinOpType.Subract))
                    {
                        throw new ParserError("Only add and subtract are valid pointer arithmetic operations.", token);
                    }
                    if (!(rType.Equals(FrontendType.i32) || rType.Equals(FrontendType.i64) || rType.Equals(FrontendType.i8)))
                    {
                        throw new ParserError($"Right side of pointer arithmetic operation must be of integer type not \"{rType}\".", token);
                    }
                }
                else if (!lType.Equals(rType))
                {
                    throw new ParserTypeMismatch(lType, rType, token);
                }

                if (isEither(BinOpType.Less, BinOpType.LessEqual, BinOpType.Greater, BinOpType.GreaterEqual,
                    BinOpType.Equal, BinOpType.NotEqual))
                {
                    return FrontendType.bool_;
                }
                else
                {
                    return lType;
                }
            }

            public override IEnumerable<Node> GetChilds()
            {
                yield return left;
                yield return right;
            }

            public override string ToString()
            {
                switch (type)
                {
                    case BinOpType.Add:
                        return "+";
                    case BinOpType.Subract:
                        return "-";
                    case BinOpType.Multiply:
                        return "*";
                    case BinOpType.Divide:
                        return "/";
                    case BinOpType.ConditionalOR:
                        return "||";
                    case BinOpType.ConditionaAND:
                        return "&&";
                    case BinOpType.LogicalOR:
                        return "|";
                    case BinOpType.LogicalXOR:
                        return "^";
                    case BinOpType.LogicalAND:
                        return "&";
                    case BinOpType.Equal:
                        return "==";
                    case BinOpType.NotEqual:
                        return "!=";
                    case BinOpType.Greater:
                        return ">";
                    case BinOpType.Less:
                        return "<";
                    case BinOpType.GreaterEqual:
                        return ">=";
                    case BinOpType.LessEqual:
                        return "<=";
                    case BinOpType.LeftShift:
                        return "<<";
                    case BinOpType.RightShift:
                        return ">>";
                    case BinOpType.Remainder:
                        return "%";
                    default:
                        throw new InvalidCodePath();
                }


            }
        }

        public class UnaryOp : Node
        {
            public enum UnaryOpType { Add, Subract, LogicalNOT, Complement, AddressOf, Dereference }
            public UnaryOpType type;

            public Node expression;

            public UnaryOp(Token t)
                : base(t)
            {
            }

            public static bool IsUnaryToken(Token t)
            {
                switch (t.type)
                {
                    case Token.TokenType.Add:
                        return true;
                    case Token.TokenType.Subtract:
                        return true;
                    case Token.TokenType.LogicalNOT:
                        return true;
                    case Token.TokenType.Complement:
                        return true;
                    case Token.TokenType.LogicalAND:
                        return true;
                    case Token.TokenType.Multiply:
                        return true;
                    default:
                        return false;
                }
            }

            public void SetTypeFromToken(Token next)
            {
                switch (next.type)
                {
                    case Token.TokenType.Add:
                        type = UnaryOpType.Add;
                        break;
                    case Token.TokenType.Subtract:
                        type = UnaryOpType.Subract;
                        break;
                    case Token.TokenType.LogicalNOT:
                        type = UnaryOpType.LogicalNOT;
                        break;
                    case Token.TokenType.Complement:
                        type = UnaryOpType.Complement;
                        break;
                    case Token.TokenType.LogicalAND:
                        type = UnaryOpType.AddressOf;
                        break;
                    case Token.TokenType.Multiply:
                        type = UnaryOpType.Dereference;
                        break;
                    default:
                        throw new ParserError("Invalid token type for unary operator", next);
                }
            }

            public override async Task<FrontendType> CheckType(Scope scope)
            {
                switch (type)
                {
                    case UnaryOpType.AddressOf:
                        {
                            var et = await expression.CheckType(scope);
                            return new FrontendPointerType(et);
                        }
                    case UnaryOpType.Dereference:
                        {
                            var et = await expression.CheckType(scope);
                            var pet = et as FrontendPointerType;
                            if (pet == null)
                            {
                                throw new ParserErrorExpected("Pointer type", et.ToString(), this.token);
                            }
                            return pet.elementType;
                        }
                    default:
                        return await expression.CheckType(scope);
                }
            }

            public override IEnumerable<Node> GetChilds()
            {
                yield return expression;
            }

            public override string ToString()
            {
                switch (type)
                {
                    case UnaryOpType.Add:
                        return "unary +";
                    case UnaryOpType.Subract:
                        return "unary -";
                    case UnaryOpType.LogicalNOT:
                        return "!";
                    case UnaryOpType.Complement:
                        return "~";
                    case UnaryOpType.AddressOf:
                        return "address of &";
                    case UnaryOpType.Dereference:
                        return "dereference *";
                    default:
                        throw new InvalidCodePath();
                }
            }
        }


        
        public class TypeCastOp : Node
        {
            public Node expression;
            public FrontendType type;

            public TypeCastOp(Token t)
                : base(t)
            {
            }

            public override IEnumerable<Node> GetChilds()
            {
                yield return expression;
            }

            // TODO: handle types that are not resolved yet!
            public override async Task<FrontendType> CheckType(Scope scope)
            {
                await expression.CheckType(scope);
                return await Task.FromResult(type);

            }
            public override string ToString()
            {
                return "(" + type.ToString() + ")";
            }
        }
    }
}
