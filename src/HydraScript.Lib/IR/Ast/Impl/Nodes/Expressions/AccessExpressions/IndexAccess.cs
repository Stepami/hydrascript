namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.AccessExpressions;

[AutoVisitable<AbstractSyntaxTreeNode>]
public partial class IndexAccess : AccessExpression
{
    protected override IReadOnlyList<AbstractSyntaxTreeNode> Children =>
        HasNext() ? [Index, Next!] : [Index];

    public Expression Index { get; }

    public IndexAccess(Expression index, AccessExpression? prev = null) : base(prev)
    {
        Index = index;
        Index.Parent = this;
    }

    protected override string NodeRepresentation() => "[]";
}