namespace HydraScript.Domain.IR.Types.Operators;

internal struct LengthOperator : IOperator
{
    public IReadOnlyList<string> Values => ["~"];

    public bool TryGetResultType(OperationDescriptor operation, out Type resultType)
    {
        resultType = "undefined";
        if (operation.OperandTypes is not [StringType or ArrayType])
            return false;

        resultType = NumberType.Instance;
        return true;
    }
}