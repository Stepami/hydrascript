namespace HydraScript.Domain.BackEnd.Impl.Instructions;

public class PopParameter(string parameter, object? defaultValue = null) : Instruction
{
    public override IAddress Execute(IExecuteParams executeParams)
    {
        var frame = executeParams.Frames.Peek();
        if (executeParams.Arguments.TryDequeue(out var argument))
            frame[parameter] = argument;
        else
            frame[parameter] = defaultValue;
        return Address.Next;
    }

    protected override string ToStringInternal() =>
        defaultValue is null
        ? $"PopParameter {parameter}"
        : $"PopParameter {parameter} {defaultValue}";
}