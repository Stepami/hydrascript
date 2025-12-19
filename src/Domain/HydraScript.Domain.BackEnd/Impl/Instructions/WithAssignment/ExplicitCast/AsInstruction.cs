namespace HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment.ExplicitCast;

public abstract class AsInstruction<T>(IValue value) : Simple(value)
{
    protected override void Assign()
    {
        var value = Right.right!.Get();
        var valueAsType = value is T ? value : Convert(value);
        Left?.Set(valueAsType);
    }

    protected abstract T Convert(object? value);

    protected override string ToStringInternal() =>
        $"{Left} = {Right.right} as {typeof(T).Name}";
}