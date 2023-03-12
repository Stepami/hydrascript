using Interpreter.Lib.BackEnd;
using Interpreter.Lib.IR.Ast.Visitors;
using Interpreter.Lib.IR.CheckSemantics.Exceptions;
using Interpreter.Lib.IR.CheckSemantics.Types;

namespace Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions;

public class UnaryExpression : Expression
{
    public string Operator { get; }
    public Expression Expression { get; }

    public UnaryExpression(string @operator, Expression expression)
    {
        Operator = @operator;

        Expression = expression;
        Expression.Parent = this;
    }

    internal override Type NodeCheck()
    {
        var eType = Expression.NodeCheck();
        Type retType;
        if (eType.Equals(TypeUtils.JavaScriptTypes.Number) && Operator == "-")
        {
            retType = TypeUtils.JavaScriptTypes.Number;
        }
        else if (eType.Equals(TypeUtils.JavaScriptTypes.Boolean) && Operator == "!")
        {
            retType = TypeUtils.JavaScriptTypes.Boolean;
        }
        else if (eType is ArrayType && Operator == "~")
        {
            retType = TypeUtils.JavaScriptTypes.Number;
        }
        else throw new UnsupportedOperation(Segment, eType, Operator);

        return retType;
    }

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
    {
        yield return Expression;
    }

    protected override string NodeRepresentation() => Operator;

    public override AddressedInstructions Accept(ExpressionInstructionProvider visitor) =>
        visitor.Visit(this);
}