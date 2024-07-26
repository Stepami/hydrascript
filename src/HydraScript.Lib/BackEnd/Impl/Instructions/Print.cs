namespace HydraScript.Lib.BackEnd.Impl.Instructions;

public class Print(IValue value) : Instruction
{
    public override IAddress Execute(VirtualMachine vm)
    {
        vm.Writer.WriteLine(value.Get(vm.Frames.Peek()));
        return Address.Next;
    }

    protected override string ToStringInternal() =>
        $"Print {value}";
}