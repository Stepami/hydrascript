using HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations;
using HydraScript.Lib.IR.CheckSemantics.Variables;

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
    public override void InitScope(ISymbolTable? scope = null)
    {
        base.InitScope(scope);
        if (DestinationType is not null)
            DestinationType.SymbolTable = SymbolTable;
    }

    protected override string NodeRepresentation() => "=";
}