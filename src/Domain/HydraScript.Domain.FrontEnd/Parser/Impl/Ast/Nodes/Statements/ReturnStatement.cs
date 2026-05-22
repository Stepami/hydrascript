namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Statements;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public partial class ReturnStatement : Statement
{
    protected override IReadOnlyList<IAbstractSyntaxTreeNode> Children { get; }

    public Expression? Expression { get; }

    public ReturnStatement(Expression? expression = null)
    {
        Expression = expression;
        Expression?.Parent = this;

        Children = Expression is not null ? [Expression] : [];
    }

    protected override string NodeRepresentation() => "return";
}