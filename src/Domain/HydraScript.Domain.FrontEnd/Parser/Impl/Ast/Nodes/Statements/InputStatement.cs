using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;

namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Statements;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public partial class InputStatement : Statement
{
    protected override IReadOnlyList<IAbstractSyntaxTreeNode> Children { get; }

    public IdentifierReference Destination { get; }

    public InputStatement(IdentifierReference destination)
    {
        Destination = destination;
        Destination.Parent = this;

        Children = [Destination];
    }

    protected override string NodeRepresentation() => "input";
}