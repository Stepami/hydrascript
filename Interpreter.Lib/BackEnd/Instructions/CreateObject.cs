using Interpreter.Lib.BackEnd.Addresses;

namespace Interpreter.Lib.BackEnd.Instructions;

public class CreateObject : Instruction
{
    private readonly string _id;
        
    public CreateObject(string id) =>
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