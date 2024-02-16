namespace Interpreter.Lib.IR.CheckSemantics.Types;

public class NullableType : Type
{
    public Type Type { get; private set; }
        
    public NullableType(Type type) :
        base($"{type}?") =>
        Type = type;

    protected NullableType()
    {
    }

    public override void ResolveReference(
        Type reference,
        string refId,
        ISet<Type> visited = null)
    {
        if (Type == refId)
            Type = reference;
        else
            Type.ResolveReference(reference, refId, visited);
    }

    public override bool Equals(object obj)
    {
        if (obj is NullableType that)
            return Equals(Type, that.Type);

        return obj is NullType or Any;
    }

    public override int GetHashCode() =>
        // ReSharper disable once NonReadonlyMemberInGetHashCode
        Type.GetHashCode();
}