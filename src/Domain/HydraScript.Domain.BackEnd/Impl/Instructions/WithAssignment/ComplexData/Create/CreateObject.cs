using HydraScript.Domain.BackEnd.Impl.Values;

namespace HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment.ComplexData.Create;

public class CreateObject(Name id) : Simple(id)
{
    private readonly Name _id = id;

    protected override void Assign() =>
        _id.Set(new Dictionary<string, object>());

    protected override string ToStringInternal() =>
        $"object {_id} = {{}}";
}