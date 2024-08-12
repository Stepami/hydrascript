namespace HydraScript.Domain.IR.Types;

public class Any() : Type("any")
{
    public override bool Equals(object? obj) => true;

    public override int GetHashCode() => "any".GetHashCode();
}