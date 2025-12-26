using HydraScript.Domain.IR.Types.Operators;

namespace HydraScript.Domain.IR.Types;

public sealed class BooleanType() : Type("boolean", [default(ConditionalOperator)])
{
    public static readonly BooleanType Instance = new();
}