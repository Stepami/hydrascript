namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Statements;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public partial class ReturnStatement : Statement
{
    protected override IReadOnlyList<IAbstractSyntaxTreeNode> Children =>
        Expression is not null ? [Expression] : [];

    public Expression? Expression { get; }

    public ReturnStatement(Expression? expression = null)
    {
        Expression = expression;
        if (Expression is not null)
        {
            Expression.Parent = this;
        }
    }

    protected override string NodeRepresentation() => "return";
}