namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions;

[AutoVisitable<AbstractSyntaxTreeNode>]
public partial class UnaryExpression : Expression
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
}