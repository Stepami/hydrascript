namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Statements;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public partial class ExpressionStatement : Statement
{
    protected override IReadOnlyList<IAbstractSyntaxTreeNode> Children =>
        [Expression];

    public Expression Expression { get; }

    public ExpressionStatement(Expression expression)
    {
        Expression = expression;
        Expression.Parent = this;
    }

    protected override string NodeRepresentation() => nameof(ExpressionStatement);
}