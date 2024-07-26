namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Statements;

[AutoVisitable<AbstractSyntaxTreeNode>]
public partial class BlockStatement : Statement
{
    private readonly List<StatementListItem> _statementList;

    protected override IReadOnlyList<AbstractSyntaxTreeNode> Children =>
        _statementList;

    public IReadOnlyList<StatementListItem> StatementList => _statementList;

    public BlockStatement(IEnumerable<StatementListItem> statementList)
    {
        _statementList = new List<StatementListItem>(statementList);
        _statementList.ForEach(item => item.Parent = this);
    }

    protected override string NodeRepresentation() => "{}";
}