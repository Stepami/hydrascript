namespace HydraScript.Lib.BackEnd.Impl.Instructions;

public class PushParameter(string parameter, IValue value) : Instruction
{
    public override IAddress Execute(IExecuteParams executeParams)
    {
        executeParams.Arguments.Push(new CallArgument(
            parameter,
            value.Get(executeParams.Frames.Peek())));
        return Address.Next;
    }

    protected override string ToStringInternal() =>
        $"PushParameter {parameter} = {value}";
}