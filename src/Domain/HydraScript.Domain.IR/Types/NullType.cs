namespace HydraScript.Domain.IR.Types;

public sealed class NullType() : Type("null")
{
    public static readonly NullType Instance = new();

    public override bool Equals(Type? obj) =>
        obj is ObjectType or NullableType or NullType or Any;

    public override int GetHashCode() =>
        "null".GetHashCode();
}