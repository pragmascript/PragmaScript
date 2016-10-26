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

            public Node(Token t)
            {
                token = t;
            }

            public virtual IEnumerable<Node> GetChilds()
            {
                yield break;
            }

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
                foreach (var p in parameters)
                {
                    yield return new AnnotatedNode(p.typeString, p.name);
                }
                if (returnType != null)
                {
                    yield return new AnnotatedNode(returnType, "return");
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

            public StructDefinition(Token t)
                : base(t)
            {

            }
            public override IEnumerable<Node> GetChilds()
            {
                foreach (var f in fields)
                {
                    yield return new AnnotatedNode(f.typeString, f.name);
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

            public Scope.VariableDefinition vd;

            // HACK: returnPointer is a HACK remove this?????
            public bool returnPointer { get; set; }
            public VariableReference(Token t)
                : base(t)
            {
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

            public Assignment(Token t)
                : base(t)
            {
            }
            public override IEnumerable<Node> GetChilds()
            {
                yield return new AnnotatedNode(target, "target");
                yield return new AnnotatedNode(expression, "expression");
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
            public override string ToString()
            {
                return s;
            }
        }

        public class ArrayConstructor : Node
        {
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
            public TypeString typeString;

            public UninitializedArray(Token t)
                : base(t)
            {

            }

            public override IEnumerable<Node> GetChilds()
            {
                yield return typeString;
            }
            public override string ToString()
            {
                return $"[{length}]";
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
            public TypeString typeString;

            public TypeCastOp(Token t)
                : base(t)
            {
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
            public TypeString(Token t) : base(t)
            {
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
