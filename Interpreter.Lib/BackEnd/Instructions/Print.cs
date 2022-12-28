using Interpreter.Lib.BackEnd.Addresses;
using Interpreter.Lib.BackEnd.Values;

namespace Interpreter.Lib.BackEnd.Instructions;

public class Print : Instruction
{
    private readonly IValue _value;
        
    public Print(IValue value) =>
        _value = value;

    public override IAddress Execute(VirtualMachine vm)
    {
        vm.Writer.WriteLine(_value.Get(vm.Frames.Peek()));
        return Address.Next;
    }

    protected override string ToStringInternal() =>
        $"Print {_value}";
}