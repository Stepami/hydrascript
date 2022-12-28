using Interpreter.Lib.BackEnd.Values;

namespace Interpreter.Lib.BackEnd.Instructions;

public class RemoveFromArray : Instruction
{
    private readonly string _id;
    private readonly IValue _index;

    public RemoveFromArray(string id, IValue index) =>
        (_id, _index) = (id, index);

    public override int Execute(VirtualMachine vm)
    {
        var frame = vm.Frames.Peek();
        var list = (List<object>) frame[_id];
        list.RemoveAt(Convert.ToInt32(_index.Get(frame)));
        return 0 + 1;
    }

    protected override string ToStringInternal() =>
        $"RemoveFrom {_id} at {_index}";
}