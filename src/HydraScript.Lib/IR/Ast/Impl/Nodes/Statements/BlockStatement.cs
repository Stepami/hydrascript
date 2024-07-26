using HydraScript.Lib.IR.CheckSemantics.Variables;

namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Statements;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public partial class BlockStatement : Statement
{
    private readonly List<StatementListItem> _statementList;

    protected override IReadOnlyList<IAbstractSyntaxTreeNode> Children =>
        _statementList;

    public IReadOnlyList<StatementListItem> StatementList => _statementList;

    public BlockStatement(IEnumerable<StatementListItem> statementList)
    {
        _statementList = new List<StatementListItem>(statementList);
        _statementList.ForEach(item => item.Parent = this);
    }

    /// <summary>Стратегия "блока" - углубление скоупа</summary>
    /// <param name="scope">Новый скоуп</param>
    public override void InitScope(SymbolTable? scope = null)
    {
        ArgumentNullException.ThrowIfNull(scope);
        SymbolTable = scope;
        SymbolTable.AddOpenScope(Parent.SymbolTable);
    }

    protected override string NodeRepresentation() => "{}";
}