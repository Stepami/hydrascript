namespace HydraScript.Domain.IR.Types;

public class NullableType(Type type) : Type($"{type}?")
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

    public override bool Equals(Type? obj)
    {
        if (obj is NullableType that)
            return Equals(Type, that.Type);

        return obj is NullType or Any || (obj is not null && obj.Equals(Type));
    }

    public override int GetHashCode() =>
        // ReSharper disable once NonReadonlyMemberInGetHashCode
        Type.GetHashCode();
}