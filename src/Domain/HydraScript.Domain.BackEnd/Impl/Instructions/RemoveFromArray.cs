namespace HydraScript.Domain.BackEnd.Impl.Instructions;

public class RemoveFromArray(string id, IValue index) : Instruction
{
    public override IAddress? Execute(IExecuteParams executeParams)
    {
        var frame = executeParams.Frames.Peek();
        if (frame[id] is List<object> list)
        {
            list.RemoveAt(Convert.ToInt32(index.Get(frame)));
        }
        return Address.Next;
    }

    protected override string ToStringInternal() =>
        $"RemoveFrom {id} at {index}";
}