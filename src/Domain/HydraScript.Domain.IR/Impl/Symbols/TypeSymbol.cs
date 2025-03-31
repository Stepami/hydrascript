using HydraScript.Domain.IR.Impl.SymbolIds;

namespace HydraScript.Domain.IR.Impl.Symbols;

public class TypeSymbol(Type type, string? name = null) :
    Symbol(name ?? type.ToString(), type)
{
    public override SymbolId Id { get; } = new TypeSymbolId(name ?? type.ToString());

    public override bool Equals(object? obj) =>
        obj is TypeSymbol typeSymbol &&
        Id == typeSymbol.Id && Type.Equals(typeSymbol.Type);

    public override int GetHashCode() =>
        HashCode.Combine(Id, Type);

    public override string ToString() =>
        $"type {Id} = {Type}";
}