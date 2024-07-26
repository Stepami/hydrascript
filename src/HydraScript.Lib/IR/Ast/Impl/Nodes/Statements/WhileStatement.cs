namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Statements;

[AutoVisitable<AbstractSyntaxTreeNode>]
public partial class WhileStatement : Statement
{
    public Expression Condition { get; }
    public Statement Statement { get; }

    public WhileStatement(Expression condition, Statement statement)
    {
        Condition = condition;
        Condition.Parent = this;

        Statement = statement;
        Statement.Parent = this;
    }

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
    {
        yield return Condition;
        yield return Statement;
    }

    protected override string NodeRepresentation() => "while";
}