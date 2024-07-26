namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Statements;

[AutoVisitable<AbstractSyntaxTreeNode>]
public partial class ReturnStatement : Statement
{
    public Expression? Expression { get; }

    public ReturnStatement(Expression? expression = null)
    {
        Expression = expression;
        if (Expression is not null)
        {
            Expression.Parent = this;
        }
    }

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
    {
        if (Expression is null)
        {
            yield break;
        }

        yield return Expression;
    }

    protected override string NodeRepresentation() => "return";
}