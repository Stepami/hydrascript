namespace HydraScript.Domain.IR.Types.Operators;

internal struct ConditionalOperator : IOperator
{
    public IReadOnlyList<string> Values => ["!", "&&", "||"];

    public bool TryGetResultType(OperationDescriptor operation, out Type resultType)
    {
        if (operation is
            { Operator: "!", OperandTypes: [BooleanType or NullableType { Type: BooleanType }] } or
            {
                OperandTypes:
                [
                    BooleanType or NullableType { Type: BooleanType },
                    BooleanType or NullableType { Type: BooleanType }
                ]
            })
        {
            resultType = BooleanType.Instance;
            return true;
        }

        resultType = "undefined";
        return false;
    }
}