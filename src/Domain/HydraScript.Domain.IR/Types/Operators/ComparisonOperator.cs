namespace HydraScript.Domain.IR.Types.Operators;

internal struct ComparisonOperator : IOperator
{
    public IReadOnlyList<string> Values => [">", ">=", "<", "<="];

    public bool TryGetResultType(OperationDescriptor operation, out Type resultType)
    {
        resultType = "undefined";
        if (operation.OperandTypes is not
            [
                NumberType or NullableType { Type: NumberType },
                NumberType or NullableType { Type: NumberType }
            ])
            return false;

        resultType = BooleanType.Instance;
        return true;
    }
}