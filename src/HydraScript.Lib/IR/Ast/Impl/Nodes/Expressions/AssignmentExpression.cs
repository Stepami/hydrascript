using HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations;

namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public partial class AssignmentExpression : Expression
{
    protected override IReadOnlyList<IAbstractSyntaxTreeNode> Children =>
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

    /// <inheritdoc cref="AbstractSyntaxTreeNode.InitScope"/>
    public override void InitScope(Scope? scope = null)
    {
        base.InitScope(scope);
        if (DestinationType is not null)
            DestinationType.Scope = Scope;
    }

    protected override string NodeRepresentation() => "=";
}