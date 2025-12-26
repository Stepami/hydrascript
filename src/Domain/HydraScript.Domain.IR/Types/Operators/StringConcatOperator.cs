namespace HydraScript.Domain.IR.Types.Operators;

internal struct StringConcatOperator : IOperator
{
    public IReadOnlyList<string> Values => ["+", "++"];

    public bool TryGetResultType(OperationDescriptor operation, out Type resultType)
    {
        resultType = "undefined";
        if (operation.OperandTypes is not [StringType, StringType])
            return false;

        resultType = StringType.Instance;
        return true;
    }
}