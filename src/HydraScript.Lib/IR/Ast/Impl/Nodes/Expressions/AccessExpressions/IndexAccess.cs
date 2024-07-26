namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.AccessExpressions;

[AutoVisitable<AbstractSyntaxTreeNode>]
public partial class IndexAccess : AccessExpression
{
    public Expression Index { get; }

    public IndexAccess(Expression index, AccessExpression? prev = null) : base(prev)
    {
        Index = index;
        Index.Parent = this;
    }

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
    {
        yield return Index;
        if (HasNext())
        {
            yield return Next!;
        }
    }

    protected override string NodeRepresentation() => "[]";
}