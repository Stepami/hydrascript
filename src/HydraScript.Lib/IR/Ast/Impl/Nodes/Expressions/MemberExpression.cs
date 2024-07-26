using HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.AccessExpressions;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;

namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions;

[AutoVisitable<AbstractSyntaxTreeNode>]
public partial class MemberExpression : LeftHandSideExpression
{
    private readonly IdentifierReference _identifierReference;

    public AccessExpression? AccessChain { get; }
    public AccessExpression? Tail { get; }

    public Type ComputedIdType { get; set; } = default!;

    public MemberExpression(IdentifierReference identifierReference) :
        this(identifierReference, accessChain: null, tail: null)
    {
    }

    public MemberExpression(
        IdentifierReference identifierReference,
        AccessExpression? accessChain,
        AccessExpression? tail)
    {
        _identifierReference = identifierReference;
        _identifierReference.Parent = this;

        AccessChain = accessChain;
        if (AccessChain is not null)
        {
            AccessChain.Parent = this;
        }

        Tail = tail;
    }

    public override IdentifierReference Id =>
        _identifierReference;

    public override bool Empty() =>
        AccessChain is null;

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
    {
        yield return Id;
        if (AccessChain is not null)
        {
            yield return AccessChain;
        }
    }

    protected override string NodeRepresentation() => nameof(MemberExpression);
}