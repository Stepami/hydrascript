using Interpreter.Lib.BackEnd.Values;

namespace Interpreter.Lib.BackEnd.Instructions;

public class PushParameter : Instruction
{
    private readonly string _parameter;
    private readonly IValue _value;

    public PushParameter(string parameter, IValue value) =>
        (_parameter, _value) = (parameter, value);

    public override int Execute(VirtualMachine vm)
    {
        vm.Arguments.Push((_parameter, _value.Get(vm.Frames.Peek())));
        return 0 + 1;
    }

    protected override string ToStringInternal() =>
        $"PushParameter {_parameter} = {_value}";
}