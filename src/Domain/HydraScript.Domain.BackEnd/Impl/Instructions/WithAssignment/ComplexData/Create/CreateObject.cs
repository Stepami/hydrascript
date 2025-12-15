using HydraScript.Domain.BackEnd.Impl.Values;

namespace HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment.ComplexData.Create;

public class CreateObject(Name id) : Simple(id)
{
    private readonly Name _id = id;

    public override IAddress? Execute(IExecuteParams executeParams)
    {
        var frame = executeParams.Frames.Peek();
        _id.Set(frame, new Dictionary<string, object>());
        return Address.Next;
    }

    protected override string ToStringInternal() =>
        $"object {_id} = {{}}";
}