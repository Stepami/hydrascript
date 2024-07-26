using HydraScript.Lib.BackEnd.Values;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations;

namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;

[AutoVisitable<AbstractSyntaxTreeNode>]
public partial class ImplicitLiteral(TypeValue typeValue) : PrimaryExpression
{
    public TypeValue TypeValue { get; } = typeValue;
    public object? ComputedDefaultValue { private get; set; }

    protected override string NodeRepresentation() =>
        TypeValue.ToString();

    public override IValue ToValue() =>
        new Constant(
            ComputedDefaultValue,
            ComputedDefaultValue is null
                ? "null"
                : ComputedDefaultValue.ToString()!);
}