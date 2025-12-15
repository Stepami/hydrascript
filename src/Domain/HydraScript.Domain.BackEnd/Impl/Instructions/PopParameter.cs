using HydraScript.Domain.BackEnd.Impl.Values;

namespace HydraScript.Domain.BackEnd.Impl.Instructions;

public class PopParameter(Name parameter, object? defaultValue) : Instruction
{
    public override IAddress? Execute(IExecuteParams executeParams)
    {
        var frame = executeParams.Frames.Peek();
        parameter.Set(
            frame,
            executeParams.Arguments.TryDequeue(out var argument)
                ? argument
                : defaultValue);
        return Address.Next;
    }

    protected override string ToStringInternal() =>
        defaultValue is null
            ? $"PopParameter {parameter}"
            : $"PopParameter {parameter} {defaultValue}";
}