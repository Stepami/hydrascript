using HydraScript.Domain.BackEnd.Impl.Values;

namespace HydraScript.Domain.BackEnd.Impl.Instructions;

public class RemoveFromArray(Name id, IValue index) : Instruction
{
    public override IAddress? Execute(IExecuteParams executeParams)
    {
        if (id.Get() is List<object> list)
        {
            list.RemoveAt(Convert.ToInt32(index.Get()));
        }
        return Address.Next;
    }

    protected override string ToStringInternal() =>
        $"RemoveFrom {id} at {index}";
}