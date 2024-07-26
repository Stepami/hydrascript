namespace HydraScript.Lib.BackEnd.Impl.Instructions;

public class PushParameter(string parameter, IValue value) : Instruction
{
    public override IAddress Execute(VirtualMachine vm)
    {
        vm.Arguments.Push(new CallArgument(
            parameter,
            value.Get(vm.Frames.Peek())));
        return Address.Next;
    }

    protected override string ToStringInternal() =>
        $"PushParameter {parameter} = {value}";
}