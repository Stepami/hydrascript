namespace HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment.ExplicitCast;

public class AsBool(IValue value) : AsInstruction<bool>(value)
{
    protected override bool Convert(object? value) =>
        System.Convert.ToBoolean(value);
}