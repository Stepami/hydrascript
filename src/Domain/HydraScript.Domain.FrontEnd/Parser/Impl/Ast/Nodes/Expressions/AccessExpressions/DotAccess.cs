using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;

namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.AccessExpressions;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public partial class DotAccess : AccessExpression
{
    protected override IReadOnlyList<IAbstractSyntaxTreeNode> Children =>
        Next is { } next ? [Property, next] : [Property];

    public IdentifierReference Property { get; }

    public DotAccess(IdentifierReference property, AccessExpression? prev = null) : base(prev)
    {
        Property = property;
        Property.Parent = this;
    }

    protected override string NodeRepresentation() => ".";

    public override DotAccess Clone() => new(Property.Clone(), Prev?.Clone());
}