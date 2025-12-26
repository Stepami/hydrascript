namespace HydraScript.Domain.IR.Types.Operators;

internal struct RemoveFromArrayOperator : IOperator
{
    public IReadOnlyList<string> Values => ["::"];

    public bool TryGetResultType(OperationDescriptor operation, out Type resultType)
    {
        resultType = "undefined";
        if (operation.OperandTypes is not
            [ArrayType, NumberType or NullableType { Type: NumberType }])
            return false;

        resultType = "void";
        return true;
    }
}