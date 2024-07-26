namespace HydraScript.Lib.BackEnd.Instructions;

public class RemoveFromArray(string id, IValue index) : Instruction
{
    public override IAddress Execute(VirtualMachine vm)
    {
        var frame = vm.Frames.Peek();
        if (frame[id] is List<object> list)
        {
            list.RemoveAt(Convert.ToInt32(index.Get(frame)));
        }
        return Address.Next;
    }

    protected override string ToStringInternal() =>
        $"RemoveFrom {id} at {index}";
}