namespace HydraScript.Lib.IR.CheckSemantics.Types;

public class NullType() : Type("null")
{
    public override bool Equals(object? obj) =>
        obj is NullableType or NullType or Any;

    public override int GetHashCode() =>
        "null".GetHashCode();
}