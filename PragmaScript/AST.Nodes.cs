using System;
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
            public Scope scope;
            public Node(Token t, Scope s)
            {
                token = t;
                scope = s;
            }

            public virtual IEnumerable<Node> GetChilds()
            {
                yield break;
            }

            public abstract Node DeepCloneTree();
        }

        public interface ICanReturnPointer
        {
            bool returnPointer { get; set; }
            bool CanReturnPointer();
        }

        public class AnnotatedNode : Node
        {
            Node node;
            public string annotation;
            public AnnotatedNode(Node n, Scope s, string annotation)
                : base(n.token, s)
            {
                node = n;
                this.annotation = annotation;
            }
            public override Node DeepCloneTree()
            {
                throw new InvalidCodePath();
            }
            public override IEnumerable<Node> GetChilds()
            {
                foreach (var n in node.GetChilds())
                {
                    yield return n;
                }
            }
            public override string ToString()
            {
                return node.ToString();
            }
        }


        public class ProgramRoot : Node
        {
            public List<FileRoot> files = new List<FileRoot>();
            public ProgramRoot(Token t, Scope s) : base(t, s)
            {
            }
            public override Node DeepCloneTree()
            {
                var result = new ProgramRoot(token, scope);
                foreach (var f in files)
                {
                    result.files.Add(f.DeepCloneTree() as FileRoot);
                }
                return result;
            }

            public override IEnumerable<Node> GetChilds()
            {
                foreach (var f in files)
                    yield return f;
            }
            public override string ToString()
            {
                return "ProgramRoot";
            }
        }

        public class FileRoot : Node
        {
            public List<Node> declarations = new List<Node>();
            public FileRoot(Token t, Scope s)
                : base(t, s)
            {
            }
            public override Node DeepCloneTree()
            {
                var result = new FileRoot(token, scope);
                foreach (var d in declarations)
                {
                    result.declarations.Add(d.DeepCloneTree());
                }
                return result;
            }
            public override IEnumerable<Node> GetChilds()
            {
                foreach (var s in declarations)
                    yield return s;
            }
            public override string ToString()
            {
                return "FileRoot";
            }
        }

        public class Block : Node
        {
            public List<Node> statements = new List<Node>();
            public Block(Token t, Scope s)
                : base(t, s)
            {
            }
            public override Node DeepCloneTree()
            {
                var result = new Block(token, scope);
                foreach (var n in statements)
                {
                    result.statements.Add(n.DeepCloneTree());
                }
                return result;
            }
            public override IEnumerable<Node> GetChilds()
            {
                foreach (var s in statements)
                    yield return s;
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

            public Elif(Token t, Scope s)
                : base(t, s)
            {
            }
            public override Node DeepCloneTree()
            {
                var result = new Elif(token, scope);
                result.condition = condition.DeepCloneTree();
                result.thenBlock = thenBlock.DeepCloneTree();
                return result;
            }
            public override IEnumerable<Node> GetChilds()
            {
                yield return new AnnotatedNode(condition, scope, "condition");
                yield return thenBlock;
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
            public IfCondition(Token t, Scope s)
                : base(t, s)
            {
            }
            public override Node DeepCloneTree()
            {
                var result = new IfCondition(token, scope);
                result.condition = condition.DeepCloneTree();
                result.thenBlock = thenBlock.DeepCloneTree();
                foreach (var n in elifs)
                {
                    result.elifs.Add(n.DeepCloneTree());
                }
                result.elseBlock = elseBlock.DeepCloneTree();
                return result;
            }
            public override IEnumerable<Node> GetChilds()
            {
                yield return new AnnotatedNode(condition, scope, "condition");
                yield return new AnnotatedNode(thenBlock, scope, "then");
                foreach (var elif in elifs)
                {
                    yield return new AnnotatedNode(elif, scope, "elif");
                }
                if (elseBlock != null)
                {
                    yield return new AnnotatedNode(elseBlock, scope, "else");
                }
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

            public ForLoop(Token t, Scope s)
                : base(t, s)
            {
            }
            public override Node DeepCloneTree()
            {
                var result = new ForLoop(token, scope);
                foreach (var n in initializer)
                {
                    result.initializer.Add(n.DeepCloneTree());
                }
                result.condition = condition.DeepCloneTree();
                foreach (var n in iterator)
                {
                    result.iterator.Add(n.DeepCloneTree());
                }
                result.loopBody = loopBody.DeepCloneTree();
                return result;
            }
            public override IEnumerable<Node> GetChilds()
            {
                int idx = 1;
                foreach (var init in initializer)
                {
                    yield return new AnnotatedNode(init, scope, "init_" + idx);
                    idx++;
                }

                yield return new AnnotatedNode(condition, scope, "condition");

                idx = 1;
                foreach (var it in iterator)
                {
                    yield return new AnnotatedNode(it, scope, "iter_" + idx);
                    idx++;
                }

                yield return new AnnotatedNode(loopBody, scope, "body");
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

            public WhileLoop(Token t, Scope s)
                : base(t, s)
            {
            }
            public override Node DeepCloneTree()
            {
                var result = new WhileLoop(token, scope);
                result.condition = condition.DeepCloneTree();
                result.loopBody = loopBody.DeepCloneTree();
                return result;
            }
            public override IEnumerable<Node> GetChilds()
            {
                yield return new AnnotatedNode(condition, scope, "condition");
                yield return new AnnotatedNode(loopBody, scope, "body");
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

            public VariableDefinition(Token t, Scope s)
                : base(t, s)
            {
            }
            public override Node DeepCloneTree()
            {
                throw new NotImplementedException();
            }
            public override IEnumerable<Node> GetChilds()
            {
                yield return expression;
            }
            public override string ToString()
            {
                return (variable.isConstant ? "var " : "let ")
                    + variable.name + " = ";

            }
        }

        public class FunctionDefinition : Node
        {
            public struct FunctionParameter
            {
                public string name;
                public TypeString typeString;
            }
            public Node body;
            public string funName;
            public List<FunctionParameter> parameters = new List<FunctionParameter>();
            public TypeString returnType;

            public bool external;

            public FunctionDefinition(Token t, Scope s)
                : base(t, s)
            {
            }
            public override Node DeepCloneTree()
            {
                throw new NotImplementedException();
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
                foreach (var p in parameters)
                {
                    yield return new AnnotatedNode(p.typeString, scope, p.name);
                }
                if (returnType != null)
                {
                    yield return new AnnotatedNode(returnType, scope, "return");
                }
                else
                {
                    Console.WriteLine("fun no return: " + funName);
                }
            }
            public override string ToString()
            {
                string result = (external ? "extern " : "") + funName + "(...)";
                return result;
            }
        }

        public class StructConstructor : Node
        {
            public string structName;
            public List<Node> argumentList = new List<Node>();

            public StructConstructor(Token t, Scope s)
                : base(t, s)
            {
            }
            public override Node DeepCloneTree()
            {
                var result = new StructConstructor(token, scope);
                result.structName = structName;
                foreach (var arg in argumentList)
                {
                    result.argumentList.Add(arg.DeepCloneTree());
                }
                return result;
            }
            public override IEnumerable<Node> GetChilds()
            {
                foreach (var a in argumentList)
                {
                    yield return a;
                }
            }
            public override string ToString()
            {
                return structName + "{ }";
            }
        }

        public class StructDefinition : Node
        {
            public string name;
            public struct StructField
            {
                public string name;
                public TypeString typeString;
            }
            public List<StructField> fields = new List<StructField>();

            public StructDefinition(Token t, Scope s)
                : base(t, s)
            {

            }
            public override Node DeepCloneTree()
            {
                throw new NotImplementedException();
            }
            public override IEnumerable<Node> GetChilds()
            {
                foreach (var f in fields)
                {
                    yield return new AnnotatedNode(f.typeString, scope, f.name);
                }
            }
            public override string ToString()
            {
                return name + " = struct { }";
            }

        }

        public class FunctionCall : Node
        {
            public string functionName;
            public List<Node> argumentList = new List<Node>();

            public FunctionCall(Token t, Scope s)
                : base(t, s)
            {
            }
            public override Node DeepCloneTree()
            {
                var result = new FunctionCall(token, scope);
                result.functionName = functionName;
                foreach (var arg in argumentList)
                {
                    result.argumentList.Add(arg.DeepCloneTree());
                }
                return result;
            }
            public override IEnumerable<Node> GetChilds()
            {
                foreach (var exp in argumentList)
                {
                    yield return exp;
                }
            }
            public override string ToString()
            {
                return functionName + "()";
            }
        }

        public class VariableReference : Node, ICanReturnPointer
        {
            public string variableName;
            public Scope.VariableDefinition vd;

            // HACK: returnPointer is a HACK remove this?????
            public bool returnPointer { get; set; }
            public bool CanReturnPointer()
            {
                return true;
            }

            public VariableReference(Token t, Scope s)
                : base(t, s)
            {
            }
            public override Node DeepCloneTree()
            {
                // TODO: this wont work if we "link" to the variable definition
                // via the scope because the scope pointer wont get updated
                // and thus fail in the type checking phase.
                Debug.Assert(vd != null);
                var result = new VariableReference(token, scope);
                result.vd = new Scope.VariableDefinition();
                result.vd.isConstant = vd.isConstant;
                result.vd.isFunctionParameter = vd.isFunctionParameter;
                result.vd.parameterIdx = vd.parameterIdx;
                result.vd.name = vd.name;
                result.vd.node = vd.node;
                result.vd.type = vd.type;
                result.returnPointer = returnPointer;
                return result;
            }
            public override string ToString()
            {
                string name = variableName;
                if (name == null)
                {
                    name = vd.name;
                }
                return name + (returnPointer ? " (p)" : "");
            }

        }

        public class Assignment : Node, ICanReturnPointer
        {
            public Node left;
            public Node right;

            public Assignment(Token t, Scope s)
                : base(t, s)
            {
            }
            public override Node DeepCloneTree()
            {
                var result = new Assignment(token, scope);
                result.left = left.DeepCloneTree();
                result.right = right.DeepCloneTree();
                return result;
            }
            public bool returnPointer { get; set; }
            public bool CanReturnPointer()
            {
                return true;
            }

            public override IEnumerable<Node> GetChilds()
            {
                yield return left;
                yield return right;
            }
            public override string ToString()
            {
                return " = " + (returnPointer ? " (p)" : "");
            }
        }

        public class ConstInt : Node
        {
            public int number;

            public ConstInt(Token t, Scope s)
                : base(t, s)
            {
            }
            public override Node DeepCloneTree()
            {
                var result = new ConstInt(token, scope);
                result.number = number;
                return result;
            }
            public override string ToString()
            {
                return number.ToString();
            }
        }

        public class ConstFloat : Node
        {
            public double number;
            public ConstFloat(Token t, Scope s)
                : base(t, s)
            {
            }
            public override Node DeepCloneTree()
            {
                var result = new ConstFloat(token, scope);
                result.number = number;
                return result;
            }
            public override string ToString()
            {
                return number.ToString("F2", CultureInfo.InvariantCulture);
            }
        }

        public class ConstBool : Node
        {
            public bool value;
            public ConstBool(Token t, Scope s)
                : base(t, s)
            {
            }

            public ConstBool(Token t, Scope s, bool b)
                : base(t, s)
            {
                // TODO: Complete member initialization
                this.value = b;
            }
            public override Node DeepCloneTree()
            {
                var result = new ConstBool(token, scope);
                result.value = value;
                return result;
            }
            public override string ToString()
            {
                return value.ToString();
            }
        }

        public class ConstString : Node
        {
            public string s;

            public ConstString(Token t, Scope s)
                : base(t, s)
            {
            }
            public override Node DeepCloneTree()
            {
                var result = new ConstString(token, scope);
                result.s = s;
                return result;
            }
            public override string ToString()
            {
                return s;
            }
        }

        public class ArrayConstructor : Node
        {
            public List<Node> elements = new List<Node>();

            public ArrayConstructor(Token t, Scope s)
                : base(t, s)
            {

            }
            public override Node DeepCloneTree()
            {
                var result = new ArrayConstructor(token, scope);
                foreach (var e in elements)
                {
                    result.elements.Add(e.DeepCloneTree());
                }
                return result;
            }
            public override IEnumerable<Node> GetChilds()
            {
                var idx = 0;
                foreach (var x in elements)
                {
                    yield return new AnnotatedNode(x, scope, "elem_" + idx++);
                }
            }
            public override string ToString()
            {
                return "[]";
            }
        }

        //public class UninitializedArray : Node
        //{
        //    // public Node length;
        //    //TODO: change this to work without compiletime constants
        //    public int length;
        //    public TypeString typeString;

        //    public UninitializedArray(Token t, Scope s)
        //        : base(t, s)
        //    {

        //    }
        //    public override Node DeepCloneTree()
        //    {
        //        throw new NotImplementedException();
        //    }
        //    public override IEnumerable<Node> GetChilds()
        //    {
        //        yield return typeString;
        //    }
        //    public override string ToString()
        //    {
        //        return $"[{length}]";
        //    }
        //}

        public class StructFieldAccess : Node, ICanReturnPointer
        {
            public Node left;
            public string fieldName;
            public bool IsArrow = false;

            public bool returnPointer { get; set; }
            public bool CanReturnPointer()
            {
                return true;
            }

            public StructFieldAccess(Token t, Scope s) :
                base(t, s)
            {

            }
            public override Node DeepCloneTree()
            {
                var result = new StructFieldAccess(token, scope);
                result.left = left.DeepCloneTree();
                result.fieldName = fieldName;
                result.IsArrow = IsArrow;
                result.returnPointer = returnPointer;
                return result;
            }
            public override IEnumerable<Node> GetChilds()
            {
                yield return left;

            }
            public override string ToString()
            {
                return (IsArrow ? "->" : ".") + fieldName + (returnPointer ? " (p)" : "");
            }
        }

        public class ArrayElementAccess : Node, ICanReturnPointer
        {
            public Node left;
            public Node index;

            public bool returnPointer { get; set; }
            public bool CanReturnPointer()
            {
                return true;
            }
            public ArrayElementAccess(Token t, Scope s)
                : base(t, s)
            {
            }
            public override Node DeepCloneTree()
            {
                var result = new ArrayElementAccess(token, scope);
                result.left = left.DeepCloneTree();
                result.index = index.DeepCloneTree();
                result.returnPointer = returnPointer;
                return result;
            }
            public override IEnumerable<Node> GetChilds()
            {
                yield return new AnnotatedNode(left, scope, "array");
                yield return new AnnotatedNode(index, scope, "index");
            }
            public override string ToString()
            {
                return "[]" + (returnPointer ? " (p)" : "");
            }
        }

        public class BreakLoop : Node
        {
            public BreakLoop(Token t, Scope s)
                : base(t, s)
            {
            }
            public override Node DeepCloneTree()
            {
                var result = new BreakLoop(token, scope);
                return result;
            }
            public override string ToString()
            {
                return "break";
            }
        }

        public class ContinueLoop : Node
        {
            public ContinueLoop(Token t, Scope s)
                : base(t, s)
            {
            }
            public override Node DeepCloneTree()
            {
                var result = new ContinueLoop(token, scope);
                return result;
            }
            public override string ToString()
            {
                return "continue";
            }
        }

        public class ReturnFunction : Node
        {
            public Node expression;

            public ReturnFunction(Token t, Scope s)
                : base(t, s)
            {
            }
            public override Node DeepCloneTree()
            {
                var result = new ReturnFunction(token, scope);
                result.expression = expression.DeepCloneTree();
                return result;
            }
            public override IEnumerable<Node> GetChilds()
            {
                if (expression != null)
                {
                    yield return expression;
                }
            }
            public override string ToString()
            {
                return "return";
            }
        }

        public class BinOp : Node
        {
            public enum BinOpType { Add, Subract, Multiply, Divide, ConditionalOR, ConditionaAND, LogicalOR, LogicalXOR, LogicalAND, Equal, NotEqual, Greater, Less, GreaterEqual, LessEqual, LeftShift, RightShift, Remainder }
            // public enum AssignmentType { None, Assignment, MultiplyEquals, DivideEquals, RemainderEquals, PlusEquals, MinusEquals, LeftShiftEquals, RightShiftEquals, AndEquals, NotEquals, OrEquals }

            public BinOpType type;

            public Node left;
            public Node right;

            public BinOp(Token t, Scope s)
                : base(t, s)
            {
            }
            public override Node DeepCloneTree()
            {
                var result = new BinOp(token, scope);
                result.type = type;
                result.left = left.DeepCloneTree();
                result.right = right.DeepCloneTree();
                return result;
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
                    case Token.TokenType.NotEquals:
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

            internal bool isEither(params BinOpType[] types)
            {
                for (int i = 0; i < types.Length; ++i)
                {
                    if (type == types[i])
                        return true;
                }

                return false;
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

        public class UnaryOp : Node, ICanReturnPointer
        {

            public enum UnaryOpType
            {
                Add, Subract, LogicalNot, Complement, AddressOf, Dereference,
                PreInc, PreDec, PostInc, PostDec
            }
            public UnaryOpType type;

            public Node expression;

            public bool returnPointer { get; set; }
            public bool CanReturnPointer()
            {
                return type == UnaryOpType.PreInc || type == UnaryOpType.PreDec;
            }
            public UnaryOp(Token t, Scope s)
                : base(t, s)
            {
            }
            public override Node DeepCloneTree()
            {
                var result = new UnaryOp(token, scope);
                result.type = type;
                result.expression = expression.DeepCloneTree();
                result.returnPointer = returnPointer;
                return result;
            }
            public static bool IsUnaryExpression(Node node)
            {
                if (!(node is UnaryOp))
                {
                    return false;
                }
                var ut = (node as UnaryOp).type;
                switch (ut)
                {
                    case UnaryOpType.PreInc:
                        return true;
                    case UnaryOpType.PreDec:
                        return true;
                    case UnaryOpType.PostInc:
                        return true;
                    case UnaryOpType.PostDec:
                        return true;
                    default:
                        return false;
                }
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
                    case Token.TokenType.Increment:
                        return true;
                    case Token.TokenType.Decrement:
                        return true;
                    default:
                        return false;
                }
            }
            public void SetTypeFromToken(Token next, bool prefix)
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
                        type = UnaryOpType.LogicalNot;
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
                    case Token.TokenType.Increment:
                        if (prefix)
                            type = UnaryOpType.PreInc;
                        else
                            type = UnaryOpType.PostInc;
                        break;
                    case Token.TokenType.Decrement:
                        if (prefix)
                            type = UnaryOpType.PreDec;
                        else
                            type = UnaryOpType.PostDec;
                        break;

                    default:
                        throw new ParserError("Invalid token type for unary operator", next);
                }
            }
            public override IEnumerable<Node> GetChilds()
            {
                yield return expression;
            }
            public override string ToString()
            {
                string result = null;
                switch (type)
                {
                    case UnaryOpType.Add:
                        result = "unary +";
                        break;
                    case UnaryOpType.Subract:
                        result = "unary -";
                        break;
                    case UnaryOpType.LogicalNot:
                        result = "!";
                        break;
                    case UnaryOpType.Complement:
                        result = "~";
                        break;
                    case UnaryOpType.AddressOf:
                        result = "address of &";
                        break;
                    case UnaryOpType.Dereference:
                        result = "dereference *";
                        break;
                    case UnaryOpType.PreInc:
                        result = "++unary";
                        break;
                    case UnaryOpType.PreDec:
                        result = "--unary";
                        break;
                    case UnaryOpType.PostInc:
                        result = "unary++";
                        break;
                    case UnaryOpType.PostDec:
                        result = "unary--";
                        break;
                    default:
                        throw new InvalidCodePath();
                }
                if (returnPointer)
                {
                    Debug.Assert(CanReturnPointer());
                    result += " (p)";
                }
                return result;
            }
        }

        public class TypeCastOp : Node
        {
            public Node expression;
            public TypeString typeString;

            public TypeCastOp(Token t, Scope s)
                : base(t, s)
            {
            }
            public override Node DeepCloneTree()
            {
                var result = new TypeCastOp(token, scope);
                result.expression = expression.DeepCloneTree();
                result.typeString = typeString.DeepCloneTree() as TypeString;
                return result;
            }
            public override IEnumerable<Node> GetChilds()
            {
                yield return expression;
                yield return typeString;
            }
            public override string ToString()
            {
                return "(T)";
            }
        }

        public class TypeString : Node
        {
            public string typeString;
            public bool isArrayType = false;
            public bool isPointerType = false;
            public int pointerLevel = 0;
            public TypeString(Token t, Scope s) : base(t, s)
            {
            }
            public override Node DeepCloneTree()
            {
                var result = new TypeString(token, scope);
                result.typeString = typeString;
                result.isArrayType = isArrayType;
                result.isPointerType = isPointerType;
                result.pointerLevel = pointerLevel;
                return result;
            }
            public override string ToString()
            {
                var result = typeString;
                if (isArrayType)
                    result += "[]";
                if (isPointerType)
                {
                    for (int i = 0; i < pointerLevel; ++i)
                    {
                        result += "*";
                    }
                }
                return result;
            }
        }
    }
}
