using Interpreter.Lib.BackEnd.Addresses;

namespace Interpreter.Lib.BackEnd.Instructions;

public class Halt : Instruction
{
    public override bool End() => true;

    public override IAddress Execute(VirtualMachine vm)
    {
        vm.Frames.Pop();
        return new HashAddress(seed: 0);
    }

    protected override string ToStringInternal() => "End";
}