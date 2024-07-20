using HydraScript.Lib.BackEnd;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations;
using HydraScript.Lib.IR.Ast.Visitors;
using HydraScript.Lib.IR.CheckSemantics.Visitors;

namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions;

public class AssignmentExpression : Expression
{
    public LeftHandSideExpression Destination { get; }
    public Expression Source { get; }
    public TypeValue DestinationType { get; }

    public AssignmentExpression(
        LeftHandSideExpression lhs,
        Expression source,
        TypeValue destinationType = null)
    {
        Destination = lhs;
        lhs.Parent = this;

        Source = source;
        source.Parent = this;

        DestinationType = destinationType;
    }

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
    {
        yield return Destination;
        yield return Source;
    }

    protected override string NodeRepresentation() => "=";

    public override Type Accept(SemanticChecker visitor) =>
        visitor.Visit(this);

    public override AddressedInstructions Accept(ExpressionInstructionProvider visitor) =>
        visitor.Visit(this);
}