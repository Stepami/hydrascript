namespace HydraScript.Domain.IR.Impl.Symbols;

public class TypeSymbol(Type type, string? id = null) :
    Symbol(id ?? type.ToString(), type)
{
    public override bool Equals(object? obj) =>
        obj is TypeSymbol typeSymbol &&
        Id == typeSymbol.Id && Type.Equals(typeSymbol.Type);

    public override int GetHashCode() =>
        HashCode.Combine(Id, Type);

    public override string ToString() =>
        $"type {Id} = {Type}";
}