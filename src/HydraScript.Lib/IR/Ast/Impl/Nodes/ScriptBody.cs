namespace HydraScript.Lib.IR.Ast.Impl.Nodes;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public partial class ScriptBody : AbstractSyntaxTreeNode
{
    private readonly List<StatementListItem> _statementList;

    protected override IReadOnlyList<IAbstractSyntaxTreeNode> Children =>
        _statementList;

    public IReadOnlyList<StatementListItem> StatementList => _statementList;

    public ScriptBody(IEnumerable<StatementListItem> statementList)
    {
        _statementList = new List<StatementListItem>(statementList);
        _statementList.ForEach(item => item.Parent = this);
    }

    protected override string NodeRepresentation() => "Script";
}