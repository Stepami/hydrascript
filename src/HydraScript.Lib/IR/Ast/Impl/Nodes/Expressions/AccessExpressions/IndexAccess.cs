namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.AccessExpressions;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public partial class IndexAccess : AccessExpression
{
    protected override IReadOnlyList<IAbstractSyntaxTreeNode> Children =>
        HasNext() ? [Index, Next!] : [Index];

    public Expression Index { get; }

    public IndexAccess(Expression index, AccessExpression? prev = null) : base(prev)
    {
        Index = index;
        Index.Parent = this;
    }

    protected override string NodeRepresentation() => "[]";
}