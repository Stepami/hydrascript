using HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment.ComplexData.Write;
using HydraScript.Domain.BackEnd.Impl.Values;

namespace HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment.ComplexData.Read;

public class DotRead(Name @object, Constant property) : Simple(
    leftValue: @object,
    binaryOperator: ".",
    rightValue: property), IReadFromComplexData
{
    public Simple ToAssignment(IValue value) =>
        new DotAssignment(@object, property, value);

    public IExecutableInstruction ToInstruction() => this;

    protected override string ToStringInternal() =>
        $"{Left} = {@object}.{property}";
}