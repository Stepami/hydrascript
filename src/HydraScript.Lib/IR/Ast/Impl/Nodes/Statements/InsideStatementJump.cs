namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Statements;

[AutoVisitable<AbstractSyntaxTreeNode>]
public partial class InsideStatementJump(string keyword) : Statement
{
    public const string Break = "break";
    public const string Continue = "continue";

    public string Keyword { get; } = keyword;

    protected override string NodeRepresentation() => Keyword;
}