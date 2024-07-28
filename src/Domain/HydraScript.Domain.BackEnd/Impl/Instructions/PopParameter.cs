namespace HydraScript.Domain.BackEnd.Impl.Instructions;

public class PopParameter(string parameter) : Instruction
{
    public override IAddress Execute(IExecuteParams executeParams)
    {
        var argument = executeParams.Arguments.Dequeue();
        executeParams.Frames.Peek()[parameter] = argument;
        return Address.Next;
    }

    protected override string ToStringInternal() =>
        $"PopParameter {parameter}";
}