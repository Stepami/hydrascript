namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Statements;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public partial class OutputStatement : Statement
{
    protected override IReadOnlyList<IAbstractSyntaxTreeNode> Children { get; }

    public Expression Expression { get; }

    public OutputStatement(Expression expression)
    {
        Expression = expression;
        Expression.Parent = this;

        Children = [Expression];
    }

    protected override string NodeRepresentation() => "output";
}