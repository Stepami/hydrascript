namespace HydraScript.Domain.IR;

public interface ISymbolId<out TSymbol> : IEquatable<ISymbolId<ISymbol>>
    where TSymbol : class, ISymbol;