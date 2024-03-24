using Interpreter.Lib.BackEnd;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions.AccessExpressions;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;
using Interpreter.Lib.IR.Ast.Visitors;
using Interpreter.Lib.IR.CheckSemantics.Visitors;

namespace Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions;

public class MemberExpression : LeftHandSideExpression
{
    private readonly IdentifierReference _identifierReference;

    public AccessExpression AccessChain { get; }
    public AccessExpression Tail { get; }

    public Type ComputedIdType { get; set; }

    public MemberExpression(
        IdentifierReference identifierReference,
        AccessExpression accessChain = null,
        AccessExpression tail = null)
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