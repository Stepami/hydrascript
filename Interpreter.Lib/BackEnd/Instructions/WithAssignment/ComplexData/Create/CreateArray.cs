using Interpreter.Lib.BackEnd.Addresses;

namespace Interpreter.Lib.BackEnd.Instructions.WithAssignment.ComplexData.Create;

public class CreateArray : Simple
{
    private readonly string _id;
    private readonly int _size;

    public CreateArray(string id, int size) : base(id) =>
        (_id, _size) = (id, size);

    public override IAddress Execute(VirtualMachine vm)
    {
        var frame = vm.Frames.Peek();
        frame[_id] = new object[_size].ToList();
        return Address.Next;
    }

    protected override string ToStringInternal() =>
        $"array {_id} = [{_size}]";
}