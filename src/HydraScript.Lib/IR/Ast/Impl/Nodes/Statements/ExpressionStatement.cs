namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Statements;

[AutoVisitable<AbstractSyntaxTreeNode>]
public partial class ExpressionStatement : Statement
{
    protected override IReadOnlyList<AbstractSyntaxTreeNode> Children =>
        [Expression];

    public Expression Expression { get; }

    public ExpressionStatement(Expression expression)
    {
        Expression = expression;
        Expression.Parent = this;
    }

    protected override string NodeRepresentation() => nameof(ExpressionStatement);
}