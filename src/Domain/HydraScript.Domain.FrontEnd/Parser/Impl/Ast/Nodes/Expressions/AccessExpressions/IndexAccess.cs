namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.AccessExpressions;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public partial class IndexAccess : AccessExpression
{
    protected override IReadOnlyList<IAbstractSyntaxTreeNode> Children =>
        Next is { } next ? [Index, next] : [Index];

    public Expression Index { get; }

    public IndexAccess(Expression index, AccessExpression? prev = null) : base(prev)
    {
        Index = index;
        Index.Parent = this;
    }

    protected override string NodeRepresentation() => "[]";

    public override IndexAccess Clone() => new(Index.Clone(), Prev?.Clone());
}