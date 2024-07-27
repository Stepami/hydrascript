namespace HydraScript.Lib.IR.CheckSemantics.Variables.Impl;

public class SymbolTable : ISymbolTable
{
    private readonly Dictionary<string, ISymbol> _symbols = [];
    private ISymbolTable? _openScope;

    /// <inheritdoc cref="ISymbolTable.AddOpenScope"/>
    public void AddOpenScope(ISymbolTable table) => _openScope = table;

    /// <inheritdoc cref="ISymbolTable.GetAvailableSymbols"/>
    public IEnumerable<ISymbol> GetAvailableSymbols() =>
        _symbols.Values.Concat(_openScope?.GetAvailableSymbols() ?? []);

    /// <inheritdoc cref="ISymbolTable.AddSymbol"/>
    public void AddSymbol(ISymbol symbol) =>
        _symbols[symbol.Id] = symbol;

    /// <inheritdoc cref="ISymbolTable.FindSymbol{TSymbol}"/>
    public TSymbol? FindSymbol<TSymbol>(string id)
        where TSymbol : class, ISymbol
    {
        var hasInsideTheScope = _symbols.TryGetValue(id, out var symbol);
        return !hasInsideTheScope
            ? _openScope?.FindSymbol<TSymbol>(id)
            : symbol as TSymbol;
    }

    /// <inheritdoc cref="ISymbolTable.ContainsSymbol"/>
    public bool ContainsSymbol(string id) =>
        _symbols.ContainsKey(id);
}