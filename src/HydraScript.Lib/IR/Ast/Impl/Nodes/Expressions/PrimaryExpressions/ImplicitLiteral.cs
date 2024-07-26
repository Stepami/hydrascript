using HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations;

namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public partial class ImplicitLiteral(TypeValue typeValue) : PrimaryExpression
{
    public TypeValue TypeValue { get; } = typeValue;
    public object? ComputedDefaultValue { private get; set; }

    protected override string NodeRepresentation() =>
        TypeValue.ToString();

    public override ValueDto ToValueDto() =>
        ValueDto.ConstantDto(
            ComputedDefaultValue,
            ComputedDefaultValue is null
                ? "null"
                : ComputedDefaultValue.ToString()!);
}