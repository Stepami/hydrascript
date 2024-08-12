using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations;

namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public partial class ImplicitLiteral(TypeValue type) : AbstractLiteral(type)
{
    public object? ComputedDefaultValue { private get; set; }

    protected override string NodeRepresentation() =>
        $"Implicit {Type}";

    public override ValueDto ToValueDto() =>
        ValueDto.ConstantDto(
            ComputedDefaultValue,
            ComputedDefaultValue is null
                ? "null"
                : ComputedDefaultValue.ToString()!);
}