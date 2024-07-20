using HydraScript.Lib.BackEnd;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.AccessExpressions;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;
using HydraScript.Lib.IR.Ast.Visitors;
using HydraScript.Lib.IR.CheckSemantics.Visitors;

namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions;

public class MemberExpression : LeftHandSideExpression
{
    private readonly IdentifierReference _identifierReference;

    public AccessExpression AccessChain { get; }
    public AccessExpression Tail { get; }

    public Type ComputedIdType { get; set; }

    public MemberExpression(IdentifierReference identifierReference) :
        this(identifierReference, accessChain: null, tail: null)
    {
    }

    public MemberExpression(
        IdentifierReference identifierReference,
        AccessExpression accessChain,
        AccessExpression tail)
    {
        _identifierReference = identifierReference;
        _identifierReference.Parent = this;
            
        AccessChain = accessChain;
        if (accessChain is not null)
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

    public override Type Accept(SemanticChecker visitor) =>
        visitor.Visit(this);

    public override AddressedInstructions Accept(ExpressionInstructionProvider visitor) =>
        visitor.Visit(this);
}