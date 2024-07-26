namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Statements;

[AutoVisitable<AbstractSyntaxTreeNode>]
public partial class InsideStatementJump(string keyword) : Statement
{
    public const string Break = "break";
    public const string Continue = "continue";

    public string Keyword { get; } = keyword;

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
    {
        yield break;
    }

    protected override string NodeRepresentation() => Keyword;
}