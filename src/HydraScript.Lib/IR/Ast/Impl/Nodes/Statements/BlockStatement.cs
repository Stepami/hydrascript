namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Statements;

[AutoVisitable<AbstractSyntaxTreeNode>]
public partial class BlockStatement : Statement
{
    public List<StatementListItem> StatementList { get; }

    public BlockStatement(IEnumerable<StatementListItem> statementList)
    {
        StatementList = new List<StatementListItem>(statementList);
        StatementList.ForEach(item => item.Parent = this);
    }

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator() =>
        StatementList.GetEnumerator();

    protected override string NodeRepresentation() => "{}";
}