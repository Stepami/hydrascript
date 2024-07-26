using HydraScript.Lib.BackEnd.Addresses;
using HydraScript.Lib.BackEnd.Values;

namespace HydraScript.Lib.BackEnd.Instructions;

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