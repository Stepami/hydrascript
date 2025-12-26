namespace HydraScript.Domain.IR.Types.Operators;

internal struct IndexOperator : IOperator
{
    public IReadOnlyList<string> Values => ["[]"];

    public bool TryGetResultType(OperationDescriptor operation, out Type resultType)
    {
        resultType = "undefined";
        if (operation.OperandTypes is not
            [
                StringType or ArrayType,
                NumberType or NullableType { Type: NumberType }
            ])
            return false;

        resultType = operation.OperandTypes[0] switch
        {
            StringType => StringType.Instance,
            ArrayType arrayType => arrayType.Type,
            _ => "undefined"
        };
        return true;
    }
}