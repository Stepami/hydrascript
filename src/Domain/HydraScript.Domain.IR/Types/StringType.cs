namespace HydraScript.Domain.IR.Types;

public sealed class StringType() : ArrayType("string")
{
    public override bool Equals(Type? obj)
    {
        if (obj?.Equals("string") is true)
            return true;
        return base.Equals(obj);
    }

    public override string ToString() => "string";
}