namespace HydraScript.Domain.IR.Types;

public class Type : IEquatable<Type>
{
    private readonly string _name = string.Empty;

    protected Type()
    {
    }

    public Type(string name) =>
        _name = name;

    public virtual void ResolveReference(
        Type reference,
        string refId,
        ISet<Type>? visited = null)
    {
    }

    public virtual bool Equals(Type? obj) =>
        obj switch
        {
            Any => true,
            not null => _name == obj._name,
            _ => false
        };

    public override bool Equals(object? obj) => Equals(obj as Type);

    public override int GetHashCode() =>
        _name.GetHashCode();

    public override string ToString() => _name;

    public static implicit operator Type(string alias) =>
        new(alias);

    public static bool operator ==(Type left, Type right) =>
        Equals(left, right);

    public static bool operator !=(Type left, Type right) =>
        !(left == right);
}