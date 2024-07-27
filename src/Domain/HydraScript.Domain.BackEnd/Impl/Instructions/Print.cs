namespace HydraScript.Domain.BackEnd.Impl.Instructions;

public class Print(IValue value) : Instruction
{
    public override IAddress Execute(IExecuteParams executeParams)
    {
        executeParams.Writer.WriteLine(value.Get(executeParams.Frames.Peek()));
        return Address.Next;
    }

    protected override string ToStringInternal() =>
        $"Print {value}";
}