namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Statements;

[AutoVisitable<AbstractSyntaxTreeNode>]
public partial class ReturnStatement : Statement
{
    protected override IReadOnlyList<AbstractSyntaxTreeNode> Children =>
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