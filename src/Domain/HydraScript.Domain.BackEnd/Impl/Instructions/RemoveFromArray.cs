using HydraScript.Domain.BackEnd.Impl.Values;

namespace HydraScript.Domain.BackEnd.Impl.Instructions;

public class RemoveFromArray(Name id, IValue index) : Instruction
{
    public override IAddress? Execute(IExecuteParams executeParams)
    {
        var frame = executeParams.Frames.Peek();
        if (id.Get(frame) is List<object> list)
        {
            list.RemoveAt(Convert.ToInt32(index.Get(frame)));
        }
        return Address.Next;
    }

    protected override string ToStringInternal() =>
        $"RemoveFrom {id} at {index}";
}