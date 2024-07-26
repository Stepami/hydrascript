namespace HydraScript.Lib.IR.Ast.Impl.Nodes;

[AutoVisitable<AbstractSyntaxTreeNode>]
public partial class ScriptBody : AbstractSyntaxTreeNode
{
    public List<StatementListItem> StatementList { get; }

    public ScriptBody(IEnumerable<StatementListItem> statementList)
    {
        StatementList = new List<StatementListItem>(statementList);
        StatementList.ForEach(item => item.Parent = this);
    }

    protected override bool IsRoot => true;

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator() =>
        StatementList.GetEnumerator();

    protected override string NodeRepresentation() => "Script";
}