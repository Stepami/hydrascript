using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations;

namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public partial class Literal : AbstractLiteral
{
    private readonly object? _value;
    private readonly string _label;

    public Literal(
        TypeValue type,
        object? value,
        string segment,
        string? label = null) : base(type)
    {
        _label = (label ?? value?.ToString())!;
        _value = value;
        Segment = segment;
    }

    protected override string NodeRepresentation() => _label;

    public override ValueDto ToValueDto() =>
        ValueDto.ConstantDto(_value, _label);

    public static Literal String(string value, string? segment = null, string? label = null) =>
        new(new TypeIdentValue(new IdentifierReference(name: "string")), value, segment ?? "(1, 1)-(1, 1)", label);

    public static Literal Number(double value, string? segment = null) =>
        new(new TypeIdentValue(new IdentifierReference(name: "number")), value, segment ?? "(1, 1)-(1, 1)");

    public static Literal Boolean(bool value, string? segment = null) =>
        new(new TypeIdentValue(new IdentifierReference(name: "boolean")), value, segment ?? "(1, 1)-(1, 1)");

    public static Literal Null(string? segment = null) =>
        new(
            new TypeIdentValue(new IdentifierReference(name: "null")),
            value: null,
            segment ?? "(1, 1)-(1, 1)",
            label: "null");
}