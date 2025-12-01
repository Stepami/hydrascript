using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations;

namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public partial class ImplicitLiteral(TypeValue type) : AbstractLiteral(type)
{
    private readonly object? _defaultValue = type switch
    {
        TypeIdentValue { TypeId.Name: "string" } => string.Empty,
        TypeIdentValue { TypeId.Name: "number" } => 0,
        TypeIdentValue { TypeId.Name: "boolean" } => false,
        TypeIdentValue { TypeId.Name: "null" } or NullableTypeValue => null,
        ArrayTypeValue => new List<object>(),
        _ => new()
    };

    protected override string NodeRepresentation() =>
        $"Implicit {Type}";

    public override ValueDto ToValueDto() =>
        ValueDto.ConstantDto(
            _defaultValue,
            _defaultValue is null
                ? "null"
                : _defaultValue.ToString()!);
}