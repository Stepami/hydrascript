namespace HydraScript.Domain.IR.Types.Operators;

internal struct ArithmeticOperator : IOperator
{
    public IReadOnlyList<string> Values => ["+", "-", "*", "/", "%"];

    public bool TryGetResultType(OperationDescriptor operation, out Type resultType)
    {
        if (operation is
            { Operator: "-", OperandTypes: [NumberType or NullableType { Type: NumberType }] } or
            {
                OperandTypes:
                [
                    NumberType or NullableType { Type: NumberType },
                    NumberType or NullableType { Type: NumberType }
                ]
            })
        {
            resultType = NumberType.Instance;
            return true;
        }

        resultType = "undefined";
        return false;
    }
}