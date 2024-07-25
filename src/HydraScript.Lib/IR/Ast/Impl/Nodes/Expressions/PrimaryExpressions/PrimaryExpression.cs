using HydraScript.Lib.BackEnd;
using HydraScript.Lib.BackEnd.Values;
using HydraScript.Lib.IR.Ast.Visitors;

namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;

public abstract class PrimaryExpression : Expression
{
    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
    {
        yield break;
    }

    public abstract IValue ToValue();

    public override AddressedInstructions Accept(ExpressionInstructionProvider visitor) =>
        visitor.Visit(this);
}