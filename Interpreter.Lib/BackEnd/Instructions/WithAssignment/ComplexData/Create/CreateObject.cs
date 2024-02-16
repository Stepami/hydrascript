using Interpreter.Lib.BackEnd.Addresses;

namespace Interpreter.Lib.BackEnd.Instructions.WithAssignment.ComplexData.Create;

public class CreateObject : Simple
{
    private readonly string _id;
        
    public CreateObject(string id) : base(id) =>
        _id = id;

    public override IAddress Execute(VirtualMachine vm)
    {
        var frame = vm.Frames.Peek();
        frame[_id] = new Dictionary<string, object>();
        return Address.Next;
    }

    protected override string ToStringInternal() =>
        $"object {_id} = {{}}";
}