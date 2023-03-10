using Interpreter.Lib.BackEnd.Instructions.WithAssignment.ComplexData.Write;
using Interpreter.Lib.BackEnd.Values;

namespace Interpreter.Lib.BackEnd.Instructions.WithAssignment.ComplexData.Read;

public class IndexRead : Simple, IReadFromComplexData
{
    private readonly Name _arrayName;
    private readonly IValue _index;

    public IndexRead(Name array, IValue index) : base(
        leftValue: array,
        binaryOperator: "[]",
        rightValue: index)
    {
        _arrayName = array;
        _index = index;
    }

    public Simple ToAssignment(IValue value) =>
        new IndexAssignment(_arrayName.ToString(), _index, value);

    public Instruction ToInstruction() => this;

    protected override string ToStringInternal() =>
        $"{Left} = {_arrayName}[{_index}]";
}