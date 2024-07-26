namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Statements;

[AutoVisitable<AbstractSyntaxTreeNode>]
public partial class WhileStatement : Statement
{
    protected override IReadOnlyList<AbstractSyntaxTreeNode> Children =>
        [Condition, Statement];

    public Expression Condition { get; }
    public Statement Statement { get; }

    public WhileStatement(Expression condition, Statement statement)
    {
        Condition = condition;
        Condition.Parent = this;

        Statement = statement;
        Statement.Parent = this;
    }

    protected override string NodeRepresentation() => "while";
}