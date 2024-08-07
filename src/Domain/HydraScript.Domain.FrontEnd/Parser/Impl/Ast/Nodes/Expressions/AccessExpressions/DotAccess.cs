using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;

namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.AccessExpressions;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public partial class DotAccess : AccessExpression
{
    protected override IReadOnlyList<IAbstractSyntaxTreeNode> Children =>
        HasNext() ? [Property, Next!] : [Property];

    public IdentifierReference Property { get; }

    public DotAccess(IdentifierReference property, AccessExpression? prev = null) : base(prev)
    {
        Property = property;
        Property.Parent = this;
    }

    protected override string NodeRepresentation() => ".";
}