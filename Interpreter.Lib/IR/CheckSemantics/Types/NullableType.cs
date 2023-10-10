using Visitor.NET;

namespace Interpreter.Lib.IR.CheckSemantics.Types;

public class NullableType : Type
{
    public Type Type { get; set; }
        
    public NullableType(Type type) : base($"{type}?")
    {
        Type = type;
    }

    protected NullableType()
    {
    }
        
    public override Unit Accept(ReferenceResolver visitor) =>
        visitor.Visit(this);

    public override bool Equals(object obj)
    {
        if (obj is NullableType that)
        {
            return Type.Equals(that.Type);
        }
        return obj is NullType;
    }

    public override int GetHashCode() =>
        // ReSharper disable once NonReadonlyMemberInGetHashCode
        Type.GetHashCode();
}