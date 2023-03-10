using Interpreter.Lib.BackEnd.Instructions.WithAssignment.ComplexData.Write;
using Interpreter.Lib.BackEnd.Values;

namespace Interpreter.Lib.BackEnd.Instructions.WithAssignment.ComplexData.Read;

public class DotRead : Simple, IReadFromComplexData
{
    private readonly Name _objectName;
    private readonly IValue _property;

    public DotRead(Name @object, IValue property) : base(
        leftValue: @object,
        binaryOperator: ".",
        rightValue: property)
    {
        _objectName = @object;
        _property = property;
    }

    public Simple ToAssignment(IValue value) =>
        new DotAssignment(_objectName.ToString(), _property, value);

    public Instruction ToInstruction() => this;

    protected override string ToStringInternal() =>
        $"{Left} = {_objectName}.{_property}";
}