using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations;

namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public partial class ImplicitLiteral(TypeValue type) : AbstractLiteral(type)
{
    private object? _defaultValue = type switch
    {
        TypeIdentValue { TypeId.Name: "string" } => string.Empty,
        TypeIdentValue { TypeId.Name: "number" } => 0,
        TypeIdentValue { TypeId.Name: "boolean" } => false,
        TypeIdentValue { TypeId.Name: "null" } or NullableTypeValue => null,
        ArrayTypeValue => new List<object>(),
        _ => default(Undefined)
    };

    protected override string NodeRepresentation() =>
        $"Implicit {Type}";

    public override ImplicitLiteral Clone() => new(Type.DeepClone());

    public void SetValue(object? value) => _defaultValue = value;

    public bool IsDefined => _defaultValue is not Undefined;

    public override ValueDto ToValueDto() =>
        ValueDto.ConstantDto(
            _defaultValue,
            _defaultValue is null
                ? "null"
                : _defaultValue.ToString()!);

    private readonly struct Undefined;
}