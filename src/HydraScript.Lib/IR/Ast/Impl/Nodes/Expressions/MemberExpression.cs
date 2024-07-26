using HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.AccessExpressions;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;

namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public partial class MemberExpression : LeftHandSideExpression
{
    private readonly IdentifierReference _identifierReference;

    protected override IReadOnlyList<IAbstractSyntaxTreeNode> Children =>
        AccessChain is not null ? [Id, AccessChain] : [Id];

    public AccessExpression? AccessChain { get; }
    public AccessExpression? Tail { get; }

    public Type ComputedIdType { get; set; } = default!;

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
        if (AccessChain is not null)
        {
            AccessChain.Parent = this;
        }

        Tail = tail;
    }

    public override IdentifierReference Id => _identifierReference;

    public override bool Empty() => AccessChain is null;

    protected override string NodeRepresentation() => nameof(MemberExpression);
}