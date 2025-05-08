using System.Diagnostics.CodeAnalysis;
using HydraScript.Domain.IR.Impl.Symbols.Ids;

namespace HydraScript.Domain.IR.Impl.Symbols;

public class TypeSymbol(Type type, string? name = null) :
    Symbol(name ?? type.ToString(), type)
{
    public override TypeSymbolId Id { get; } = new(name ?? type.ToString());

    public override bool Equals(object? obj) =>
        obj is TypeSymbol typeSymbol &&
        Name == typeSymbol.Name && Type.Equals(typeSymbol.Type);

    public override int GetHashCode() =>
        HashCode.Combine(Name, Type);

    [ExcludeFromCodeCoverage]
    public override string ToString() =>
        $"type {Name} = {Type}";
}