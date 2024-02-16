namespace Interpreter.Lib.IR.CheckSemantics.Types;

public class NullType : Type
{
    public NullType() : base("null")
    {
    }

    public override bool Equals(object obj) =>
        obj is NullableType or NullType or Any;

    public override int GetHashCode() =>
        "null".GetHashCode();
}