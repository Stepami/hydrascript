namespace HydraScript.Domain.IR.Impl;

public class SymbolTable : ISymbolTable
{
    private readonly Dictionary<ISymbolId<ISymbol>, ISymbol> _symbols = [];
    private ISymbolTable? _openScope;

    /// <inheritdoc cref="ISymbolTable.AddOpenScope"/>
    public void AddOpenScope(ISymbolTable table) => _openScope = table;

    /// <inheritdoc cref="ISymbolTable.GetAvailableSymbols"/>
    public IEnumerable<ISymbol> GetAvailableSymbols() =>
        _symbols.Values.Concat(_openScope?.GetAvailableSymbols() ?? []);

    /// <inheritdoc cref="ISymbolTable.AddSymbol(ISymbol)"/>
    public void AddSymbol(ISymbol symbol) =>
        _symbols[symbol.Id] = symbol;

    /// <inheritdoc cref="ISymbolTable.AddSymbol{TSymbol}(TSymbol, ISymbolId{TSymbol})"/>
    public void AddSymbol<TSymbol>(TSymbol symbol, ISymbolId<TSymbol> symbolId)
        where TSymbol : class, ISymbol =>
        _symbols[symbolId] = symbol;

    /// <inheritdoc cref="ISymbolTable.FindSymbol{TSymbol}"/>
    public TSymbol? FindSymbol<TSymbol>(ISymbolId<TSymbol> id)
        where TSymbol : class, ISymbol
    {
        var hasInsideTheScope = _symbols.TryGetValue(id, out var symbol);
        return !hasInsideTheScope
            ? _openScope?.FindSymbol(id)
            : symbol as TSymbol;
    }

    /// <inheritdoc cref="ISymbolTable.ContainsSymbol"/>
    public bool ContainsSymbol(ISymbolId<ISymbol> id) =>
        _symbols.ContainsKey(id);

    /// <inheritdoc cref="ISymbolTable.Clear"/>
    public void Clear()
    {
        _symbols.Clear();
        _openScope?.Clear();
    }
}