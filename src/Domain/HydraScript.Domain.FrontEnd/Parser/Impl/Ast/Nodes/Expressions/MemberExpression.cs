using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.AccessExpressions;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;

namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public partial class MemberExpression : LeftHandSideExpression
{
    private readonly IdentifierReference _identifierReference;

    protected override IReadOnlyList<IAbstractSyntaxTreeNode> Children =>
        AccessChain.First is { Value: { } head } ? [Id, head] : [Id];

    public LinkedList<AccessExpression> AccessChain { get; }

    public MemberExpression(IdentifierReference identifierReference) :
        this(identifierReference, [])
    {
    }

    public MemberExpression(
        IdentifierReference identifierReference,
        LinkedList<AccessExpression> accessChain)
    {
        _identifierReference = identifierReference;
        _identifierReference.Parent = this;

        AccessChain = accessChain;
        AccessChain.First?.Value.Parent = this;
    }

    public override IdentifierReference Id => _identifierReference;

    public bool Empty() => AccessChain.Count == 0;

    protected override string NodeRepresentation() => nameof(MemberExpression);

    public override MemberExpression Clone()
    {
        var clonedAccessChain = new LinkedList<AccessExpression>();
        var clonedTail = AccessChain.Last?.Value.Clone();
        while (clonedTail != null)
        {
            clonedAccessChain.AddFirst(clonedTail);
            clonedTail = clonedTail.Prev;
        }
        return new MemberExpression(Id.Clone(), clonedAccessChain);
    }
}