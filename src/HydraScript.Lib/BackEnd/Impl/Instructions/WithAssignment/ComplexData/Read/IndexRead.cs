using HydraScript.Lib.BackEnd.Impl.Instructions.WithAssignment.ComplexData.Write;
using HydraScript.Lib.BackEnd.Impl.Values;

namespace HydraScript.Lib.BackEnd.Impl.Instructions.WithAssignment.ComplexData.Read;

public class IndexRead(Name array, IValue index) : Simple(
    leftValue: array,
    binaryOperator: "[]",
    rightValue: index), IReadFromComplexData
{
    private readonly IValue _index = index;

    public Simple ToAssignment(IValue value) =>
        new IndexAssignment(array.ToString(), _index, value);

    public Instruction ToInstruction() => this;

    protected override string ToStringInternal() =>
        $"{Left} = {array}[{_index}]";
}