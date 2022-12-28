using Interpreter.Lib.BackEnd.Values;

namespace Interpreter.Lib.BackEnd.Instructions;

public class Print : Instruction
{
    private readonly IValue _value;
        
    public Print(IValue value) =>
        _value = value;

    public override int Execute(VirtualMachine vm)
    {
        vm.Writer.WriteLine(_value.Get(vm.Frames.Peek()));
        return 0 + 1;
    }

    protected override string ToStringInternal() =>
        $"Print {_value}";
}