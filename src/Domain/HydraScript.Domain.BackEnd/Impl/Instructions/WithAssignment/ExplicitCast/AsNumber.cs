namespace HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment.ExplicitCast;

public class AsNumber(IValue value) : AsInstruction<double>(value)
{
    protected override double Convert(object? value) =>
        System.Convert.ToDouble(value);
}