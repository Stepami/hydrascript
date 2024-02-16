namespace Interpreter.Lib.IR.CheckSemantics.Types;

public class Type
{
    private readonly string _name;

    protected Type()
    {
    }

    public Type(string name) =>
        _name = name;

    public virtual void ResolveReference(
        Type reference,
        string refId,
        ISet<Type> visited = null)
    {
    }

    public override bool Equals(object obj) =>
        obj switch
        {
            Any => true,
            Type that => _name == that._name,
            _ => false
        };

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