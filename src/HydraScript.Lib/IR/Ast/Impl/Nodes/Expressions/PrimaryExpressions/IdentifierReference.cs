using HydraScript.Lib.BackEnd.Values;

namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;

[AutoVisitable<AbstractSyntaxTreeNode>]
public partial class IdentifierReference(string name) : PrimaryExpression
{
    public string Name { get; } = name;

    protected override string NodeRepresentation() => Name;

    public override IValue ToValue() => new Name(Name);

    public static implicit operator string(IdentifierReference identifierReference) =>
        identifierReference.Name;
}