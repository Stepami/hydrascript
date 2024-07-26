namespace HydraScript.Lib.BackEnd.Impl.Instructions.WithAssignment.ComplexData.Create;

public class CreateArray(string id, int size) : Simple(id)
{
    private readonly string _id = id;

    public override IAddress Execute(VirtualMachine vm)
    {
        var frame = vm.Frames.Peek();
        frame[_id] = new object[size].ToList();
        return Address.Next;
    }

    protected override string ToStringInternal() =>
        $"array {_id} = [{size}]";
}