namespace HydraScript.Lib.BackEnd.Impl.Instructions;

public class PushParameter(IValue value) : Instruction
{
    public override IAddress Execute(IExecuteParams executeParams)
    {
        executeParams.Arguments.Enqueue(
            value.Get(executeParams.Frames.Peek()));
        return Address.Next;
    }

    protected override string ToStringInternal() =>
        $"PushParameter {value}";
}