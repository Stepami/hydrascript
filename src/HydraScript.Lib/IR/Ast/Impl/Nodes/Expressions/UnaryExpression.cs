using HydraScript.Lib.BackEnd;
using HydraScript.Lib.IR.Ast.Visitors;
using HydraScript.Lib.IR.CheckSemantics.Visitors;

namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions;

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

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
    {
        yield return Expression;
    }

    protected override string NodeRepresentation() => Operator;

    public override Type Accept(SemanticChecker visitor) =>
        visitor.Visit(this);

    public override AddressedInstructions Accept(ExpressionInstructionProvider visitor) =>
        visitor.Visit(this);
}