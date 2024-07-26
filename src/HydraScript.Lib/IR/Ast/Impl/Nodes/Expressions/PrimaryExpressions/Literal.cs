using HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations;

namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;

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
}