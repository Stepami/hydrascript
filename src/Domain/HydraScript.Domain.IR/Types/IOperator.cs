namespace HydraScript.Domain.IR.Types;

public interface IOperator
{
    public IReadOnlyList<string> Values { get; }

    public bool TryGetResultType(OperationDescriptor operation, out Type resultType);
}

public record struct OperationDescriptor(
    string Operator,
    IReadOnlyList<Type> OperandTypes);