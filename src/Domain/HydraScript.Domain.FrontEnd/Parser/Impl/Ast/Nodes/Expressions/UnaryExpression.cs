namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public partial class UnaryExpression : Expression
{
    protected override IReadOnlyList<IAbstractSyntaxTreeNode> Children { get; }

    public string Operator { get; }
    public Expression Expression { get; }

    public UnaryExpression(string @operator, Expression expression)
    {
        Operator = @operator;

        Expression = expression;
        Expression.Parent = this;

        Children = [Expression];
    }

    protected override string NodeRepresentation() => Operator;
}