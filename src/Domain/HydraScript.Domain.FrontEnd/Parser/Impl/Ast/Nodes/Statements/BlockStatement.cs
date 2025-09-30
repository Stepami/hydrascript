namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Statements;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public partial class BlockStatement : Statement
{
    private readonly List<StatementListItem> _statementList;

    protected override IReadOnlyList<IAbstractSyntaxTreeNode> Children =>
        _statementList;

    public BlockStatement(List<StatementListItem> statementList)
    {
        _statementList = statementList;
        _statementList.ForEach(item => item.Parent = this);
    }

    /// <summary>Стратегия "блока" - углубление скоупа</summary>
    /// <param name="scope">Новый скоуп</param>
    public override void InitScope(Scope? scope = null)
    {
        ArgumentNullException.ThrowIfNull(scope);
        Scope = scope;
        Scope.AddOpenScope(Parent.Scope);
    }

    protected override string NodeRepresentation() => "{}";
}