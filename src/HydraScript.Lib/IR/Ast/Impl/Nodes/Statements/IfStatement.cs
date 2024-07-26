namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Statements;

[AutoVisitable<AbstractSyntaxTreeNode>]
public partial class IfStatement : Statement
{
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

    public bool Empty() =>
        !Then.Any() && !HasElseBlock();

    public bool HasElseBlock() =>
        Else is not null && Else.Any();

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
    {
        yield return Test;
        yield return Then;
        if (Else is not null)
        {
            yield return Else;
        }
    }

    protected override string NodeRepresentation() => "if";
}