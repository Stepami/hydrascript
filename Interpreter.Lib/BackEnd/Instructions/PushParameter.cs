using Interpreter.Lib.BackEnd.Addresses;
using Interpreter.Lib.BackEnd.Values;

namespace Interpreter.Lib.BackEnd.Instructions;

public class PushParameter : Instruction
{
    private readonly string _parameter;
    private readonly IValue _value;

    public PushParameter(string parameter, IValue value) =>
        (_parameter, _value) = (parameter, value);

    public override IAddress Execute(VirtualMachine vm)
    {
        vm.Arguments.Push((_parameter, _value.Get(vm.Frames.Peek())));
        return Address.Next;
    }

    protected override string ToStringInternal() =>
        $"PushParameter {_parameter} = {_value}";
}