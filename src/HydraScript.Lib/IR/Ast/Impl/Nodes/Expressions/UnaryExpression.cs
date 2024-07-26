namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public partial class UnaryExpression : Expression
{
    protected override IReadOnlyList<IAbstractSyntaxTreeNode> Children =>
        [Expression];

    public string Operator { get; }
    public Expression Expression { get; }

    public UnaryExpression(string @operator, Expression expression)
    {
        Operator = @operator;

        Expression = expression;
        Expression.Parent = this;
    }

    protected override string NodeRepresentation() => Operator;
}