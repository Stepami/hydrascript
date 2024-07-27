namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Statements;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public partial class InsideStatementJump(string keyword) : Statement
{
    public const string Break = "break";
    public const string Continue = "continue";

    public string Keyword { get; } = keyword;

    protected override string NodeRepresentation() => Keyword;
}