namespace HydraScript.Domain.IR.Types.Operators;

internal struct EqualityOperator : IOperator
{
    public IReadOnlyList<string> Values => ["==", "!="];

    public bool TryGetResultType(OperationDescriptor operation, out Type resultType)
    {
        resultType = "undefined";
        if (operation.OperandTypes is not [var left, var right])
            return false;

        if (!default(CommutativeTypeEqualityComparer).Equals(left, right))
            return false;

        resultType = BooleanType.Instance;
        return true;
    }
}