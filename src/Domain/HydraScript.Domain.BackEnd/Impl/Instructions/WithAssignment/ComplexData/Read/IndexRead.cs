using HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment.ComplexData.Write;
using HydraScript.Domain.BackEnd.Impl.Values;

namespace HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment.ComplexData.Read;

public class IndexRead(Name array, IValue index) : Simple(
    leftValue: array,
    binaryOperator: "[]",
    rightValue: index), IReadFromComplexData
{
    private readonly IValue _index = index;

    public Simple ToAssignment(IValue value) =>
        new IndexAssignment(array.ToString(), _index, value);

    public IExecutableInstruction ToInstruction() => this;

    protected override string ToStringInternal() =>
        $"{Left} = {array}[{_index}]";
}