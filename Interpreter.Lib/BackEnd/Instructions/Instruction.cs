using Interpreter.Lib.BackEnd.Addresses;

namespace Interpreter.Lib.BackEnd.Instructions;

public abstract class Instruction
{
    public IAddress Address { get; set; }

    public virtual bool End() => false;

    public abstract int Execute(VirtualMachine vm);

    protected abstract string ToStringInternal();

    public override string ToString() =>
        $"{(Address?.IsLabel() ?? false ? $"{Address}: " : "")}{ToStringInternal()}";
}