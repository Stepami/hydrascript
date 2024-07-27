namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public partial class IdentifierReference(string name) : PrimaryExpression
{
    public string Name { get; } = name;

    protected override string NodeRepresentation() => Name;

    public override ValueDto ToValueDto() =>
        ValueDto.NameDto(Name);

    public static implicit operator string(IdentifierReference identifierReference) =>
        identifierReference.Name;
}