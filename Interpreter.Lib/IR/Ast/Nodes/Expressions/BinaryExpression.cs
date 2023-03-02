using Interpreter.Lib.BackEnd;
using Interpreter.Lib.BackEnd.Addresses;
using Interpreter.Lib.IR.Ast.Visitors;
using Interpreter.Lib.IR.CheckSemantics.Exceptions;
using Interpreter.Lib.IR.CheckSemantics.Types;

namespace Interpreter.Lib.IR.Ast.Nodes.Expressions;

public class BinaryExpression : Expression
{
    public Expression Left { get; }
    public string Operator { get; }
    public Expression Right { get; }

    public BinaryExpression(Expression left, string @operator, Expression right)
    {
        Left = left;
        Left.Parent = this;

        Operator = @operator;

        Right = right;
        Right.Parent = this;
    }

    internal override Type NodeCheck()
    {
        var lType = Left.NodeCheck();
        var rType = Right.NodeCheck();
        Type retType = null;
        if (Operator != "::" && !lType.Equals(rType))
        {
            throw new IncompatibleTypesOfOperands(Segment, lType, rType);
        }

        switch (Operator)
        {
            case "+":
                if (lType.Equals(TypeUtils.JavaScriptTypes.Number))
                {
                    retType = TypeUtils.JavaScriptTypes.Number;
                }
                else if (lType.Equals(TypeUtils.JavaScriptTypes.String))
                {
                    retType = TypeUtils.JavaScriptTypes.String;
                }
                else throw new UnsupportedOperation(Segment, lType, Operator);

                break;
            case "-":
            case "*":
            case "/":
            case "%":
                if (lType.Equals(TypeUtils.JavaScriptTypes.Number))
                {
                    retType = TypeUtils.JavaScriptTypes.Number;
                }
                else throw new UnsupportedOperation(Segment, lType, Operator);

                break;
            case "||":
            case "&&":
                if (lType.Equals(TypeUtils.JavaScriptTypes.Boolean))
                {
                    retType = TypeUtils.JavaScriptTypes.Boolean;
                }
                else throw new UnsupportedOperation(Segment, lType, Operator);

                break;
            case "==":
            case "!=":
                retType = TypeUtils.JavaScriptTypes.Boolean;
                break;
            case ">":
            case ">=":
            case "<":
            case "<=":
                if (lType.Equals(TypeUtils.JavaScriptTypes.Number))
                {
                    retType = TypeUtils.JavaScriptTypes.Boolean;
                }
                else throw new UnsupportedOperation(Segment, lType, Operator);

                break;
            case "++":
                if (lType is ArrayType && rType is ArrayType)
                {
                    retType = lType;
                }
                else throw new UnsupportedOperation(Segment, lType, Operator);

                break;
            case "::":
                if (!(lType is ArrayType))
                {
                    throw new UnsupportedOperation(Segment, lType, Operator);
                }
                if (rType.Equals(TypeUtils.JavaScriptTypes.Number))
                {
                    retType = TypeUtils.JavaScriptTypes.Void;
                }
                else throw new ArrayAccessException(Segment, rType);

                break;
        }

        return retType;
    }

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
    {
        yield return Left;
        yield return Right;
    }

    protected override string NodeRepresentation() => Operator;

    public override AddressedInstructions Accept(ExpressionInstructionProvider visitor) =>
        visitor.Visit(this);
}