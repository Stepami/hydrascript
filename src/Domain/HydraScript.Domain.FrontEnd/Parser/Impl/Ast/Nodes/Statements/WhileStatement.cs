namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Statements;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public partial class WhileStatement : Statement
{
    protected override IReadOnlyList<IAbstractSyntaxTreeNode> Children { get; }

    public Expression Condition { get; }
    public Statement Statement { get; }

    public WhileStatement(Expression condition, Statement statement)
    {
        Condition = condition;
        Condition.Parent = this;

        Statement = statement;
        Statement.Parent = this;

        Children = [Condition, Statement];
    }

    protected override string NodeRepresentation() => "while";
}