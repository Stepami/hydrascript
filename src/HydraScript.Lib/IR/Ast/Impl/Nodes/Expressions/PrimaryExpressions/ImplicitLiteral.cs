using HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations;

namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public partial class ImplicitLiteral(TypeValue type) : AbstractLiteral(type)
{
    public object? ComputedDefaultValue { private get; set; }

    protected override string NodeRepresentation() =>
        Type.ToString();

    public override ValueDto ToValueDto() =>
        ValueDto.ConstantDto(
            ComputedDefaultValue,
            ComputedDefaultValue is null
                ? "null"
                : ComputedDefaultValue.ToString()!);
}