using HydraScript.Domain.IR.Types.Operators;

namespace HydraScript.Domain.IR.Types;

public sealed class StringType() :
    Type(
        "string",
        [
            default(StringConcatOperator),
            default(LengthOperator),
            default(IndexOperator)
        ])
{
    public static readonly StringType Instance = new();
}