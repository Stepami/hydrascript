using HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;

namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.AccessExpressions;

[AutoVisitable<AbstractSyntaxTreeNode>]
public partial class DotAccess : AccessExpression
{
    public IdentifierReference Property { get; }

    public DotAccess(IdentifierReference property, AccessExpression? prev = null) : base(prev)
    {
        Property = property;
        Property.Parent = this;
    }

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
    {
        yield return Property;
        if (HasNext())
        {
            yield return Next!;
        }
    }

    protected override string NodeRepresentation() => ".";
}