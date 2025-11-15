using HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment.ComplexData.Write;
using HydraScript.Domain.BackEnd.Impl.Values;

namespace HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment.ComplexData.Read;

public class DotRead(Name @object, IValue property) : Simple(
    leftValue: @object,
    binaryOperator: ".",
    rightValue: property), IReadFromComplexData
{
    private readonly IValue _property = property;

    public Simple ToAssignment(IValue value) =>
        new DotAssignment(@object.ToString(), _property, value);

    public IExecutableInstruction ToInstruction() => this;

    protected override string ToStringInternal() =>
        $"{Left} = {@object}.{_property}";
}