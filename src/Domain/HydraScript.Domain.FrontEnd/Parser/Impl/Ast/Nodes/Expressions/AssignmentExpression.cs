using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations;

namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public partial class AssignmentExpression : Expression
{
    protected override IReadOnlyList<IAbstractSyntaxTreeNode> Children { get; }

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

        Children = [Destination, Source];
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