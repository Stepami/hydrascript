using HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;

namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.AccessExpressions;

[AutoVisitable<AbstractSyntaxTreeNode>]
public partial class DotAccess : AccessExpression
{
    protected override IReadOnlyList<AbstractSyntaxTreeNode> Children =>
        HasNext() ? [Property, Next!] : [Property];

    public IdentifierReference Property { get; }

    public DotAccess(IdentifierReference property, AccessExpression? prev = null) : base(prev)
    {
        Property = property;
        Property.Parent = this;
    }

    protected override string NodeRepresentation() => ".";
}