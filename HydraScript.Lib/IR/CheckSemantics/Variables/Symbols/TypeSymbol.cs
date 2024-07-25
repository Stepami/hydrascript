namespace HydraScript.Lib.IR.CheckSemantics.Variables.Symbols;

public class TypeSymbol(Type type, string? id = null) : Symbol
{
    public override string Id { get; } = id ?? type.ToString();
    public override Type Type { get; } = type;

    public override bool Equals(object? obj)
    {
        if (obj is TypeSymbol typeSymbol)
        {
            return Id == typeSymbol.Id &&
                   Type.Equals(typeSymbol.Type);
        }

        return false;
    }

    public override int GetHashCode() =>
        HashCode.Combine(Id, Type);

    public override string ToString() =>
        $"type {Id} = {Type}";
}