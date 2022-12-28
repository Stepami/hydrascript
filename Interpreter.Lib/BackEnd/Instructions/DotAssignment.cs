using Interpreter.Lib.BackEnd.Values;

namespace Interpreter.Lib.BackEnd.Instructions;

public class DotAssignment : Simple
{
    public DotAssignment(string left, (IValue left, IValue right) right) : 
        base(left, right, ".") { }

    public override int Execute(VirtualMachine vm)
    {
        var frame = vm.Frames.Peek();
        var obj = (Dictionary<string, object>) frame[Left];
        var field = (string) right.left.Get(frame) ?? string.Empty;
        obj[field] = right.right.Get(frame);
        return 0 + 1;
    }

    protected override string ToStringInternal() =>
        $"{Left}{@operator}{right.left} = {right.right}";
}