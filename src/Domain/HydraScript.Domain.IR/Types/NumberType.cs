using HydraScript.Domain.IR.Types.Operators;

namespace HydraScript.Domain.IR.Types;

public sealed class NumberType() :
    Type(
        "number",
        [
            default(ArithmeticOperator),
            default(ComparisonOperator)
        ])
{
    public static readonly NumberType Instance = new();
}