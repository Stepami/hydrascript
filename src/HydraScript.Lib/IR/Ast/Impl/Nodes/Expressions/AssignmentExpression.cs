using HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations;

namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions;

[AutoVisitable<AbstractSyntaxTreeNode>]
public partial class AssignmentExpression : Expression
{
    protected override IReadOnlyList<AbstractSyntaxTreeNode> Children =>
        [Destination, Source];

    public LeftHandSideExpression Destination { get; }
    public Expression Source { get; }
    public TypeValue? DestinationType { get; }

    public AssignmentExpression(
        LeftHandSideExpression lhs,
        Expression source,
        TypeValue? destinationType = null)
    {
        Destination = lhs;
        lhs.Parent = this;

        Source = source;
        source.Parent = this;

        DestinationType = destinationType;
    }

    protected override string NodeRepresentation() => "=";
}