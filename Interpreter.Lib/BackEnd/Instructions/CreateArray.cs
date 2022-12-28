namespace Interpreter.Lib.BackEnd.Instructions;

public class CreateArray : Instruction
{
    private readonly string _id;
    private readonly int _size;

    public CreateArray(string id, int size) =>
        (_id, _size) = (id, size);

    public override int Execute(VirtualMachine vm)
    {
        var frame = vm.Frames.Peek();
        frame[_id] = new object[_size].ToList();
        return 0 + 1;
    }

    protected override string ToStringInternal() =>
        $"array {_id} = [{_size}]";
}