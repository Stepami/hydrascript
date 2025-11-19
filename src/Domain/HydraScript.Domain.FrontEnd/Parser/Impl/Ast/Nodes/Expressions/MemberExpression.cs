using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.AccessExpressions;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;

namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public partial class MemberExpression : LeftHandSideExpression
{
    private readonly IdentifierReference _identifierReference;

    protected override IReadOnlyList<IAbstractSyntaxTreeNode> Children =>
        AccessChain is not null ? [Id, AccessChain] : [Id];

    public AccessExpression? AccessChain { get; }
    public AccessExpression? Tail { get; }

    public Guid ComputedIdTypeGuid { get; set; } = Guid.Empty;

    public MemberExpression(IdentifierReference identifierReference)
    {
        _identifierReference = identifierReference;
        _identifierReference.Parent = this;
    }

    public MemberExpression(
        IdentifierReference identifierReference,
        AccessExpression? accessChain,
        AccessExpression? tail) : this(identifierReference)
    {
        AccessChain = accessChain;
        AccessChain?.Parent = this;

        Tail = tail;
    }

    public override IdentifierReference Id => _identifierReference;

    public override bool Empty() => AccessChain is null;

    protected override string NodeRepresentation() => nameof(MemberExpression);
}