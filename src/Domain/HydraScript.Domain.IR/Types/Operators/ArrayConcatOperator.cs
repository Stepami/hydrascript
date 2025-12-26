using ZLinq;

namespace HydraScript.Domain.IR.Types.Operators;

internal struct ArrayConcatOperator : IOperator
{
    public IReadOnlyList<string> Values => ["++"];

    public bool TryGetResultType(OperationDescriptor operation, out Type resultType)
    {
        resultType = "undefined";
        if (operation.OperandTypes is not [ArrayType left, ArrayType right])
            return false;

        if (default(CommutativeTypeEqualityComparer).Equals(left.Type, right.Type))
            resultType = operation.OperandTypes.AsValueEnumerable()
                .FirstOrDefault(x => x is ArrayType { Type: not Any }) ?? "undefined";

        return true;
    }
}