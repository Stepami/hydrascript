namespace HydraScript.Domain.IR.Types;

public class ArrayType(Type type) : Type($"{type}[]")
{
    public Type Type { get; private set; } = type;

    public override void ResolveReference(
        Type reference,
        string refId,
        ISet<Type>? visited = null)
    {
        if (Type == refId)
            Type = reference;
        else
            Type.ResolveReference(reference, refId, visited);
    }

    public override bool Equals(object? obj)
    {
        if (obj is ArrayType that)
            return Equals(Type, that.Type);
        return obj is Any;
    }

    public override int GetHashCode() =>
        // ReSharper disable once NonReadonlyMemberInGetHashCode
        Type.GetHashCode();
}