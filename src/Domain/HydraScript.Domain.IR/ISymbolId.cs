namespace HydraScript.Domain.IR;

// ReSharper disable once UnusedTypeParameter
public interface ISymbolId<out TSymbol> : IEquatable<ISymbolId<ISymbol>>
    where TSymbol : class, ISymbol;