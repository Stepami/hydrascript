namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Statements;

[AutoVisitable<AbstractSyntaxTreeNode>]
public partial class IfStatement : Statement
{
    protected override IReadOnlyList<AbstractSyntaxTreeNode> Children =>
        Else is not null ? [Test, Then, Else] : [Test, Then];

    public Expression Test { get; }
    public Statement Then { get; }
    public Statement? Else { get; }

    public IfStatement(Expression test, Statement then, Statement? @else = null)
    {
        Test = test;
        Test.Parent = this;

        Then = then;
        Then.Parent = this;

        if (@else is not null)
        {
            Else = @else;
            Else.Parent = this;
        }
    }

    public bool Empty() => this is
    {
        Then.Count: 0,
        Else: null or { Count: 0 }
    };

    public bool HasElseBlock() => Else is { Count: > 0 };

    protected override string NodeRepresentation() => "if";
}