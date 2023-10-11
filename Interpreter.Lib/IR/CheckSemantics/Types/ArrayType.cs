namespace Interpreter.Lib.IR.CheckSemantics.Types;

public class ArrayType : Type
{
    public Type Type { get; private set; }

    public ArrayType(Type type) :
        base($"{type}[]") =>
        Type = type;

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
        if (ReferenceEquals(this, obj)) return true;
        if (obj == null || GetType() != obj.GetType()) return false;
        var that = (ArrayType) obj;
        return Equals(Type, that.Type);
    }
        
    public override int GetHashCode() => 
        // ReSharper disable once NonReadonlyMemberInGetHashCode
        Type.GetHashCode();
}