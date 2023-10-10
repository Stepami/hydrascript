using Interpreter.Lib.IR.CheckSemantics.Variables.Symbols;

namespace Interpreter.Lib.IR.CheckSemantics.Variables;

public class SymbolTable
{
    private readonly Dictionary<string, Symbol> _symbols = new();

    private SymbolTable _openScope;

    public void AddOpenScope(SymbolTable table)
    {
        _openScope = table;
    }

    /// <summary>
    /// Символы доступные в области видимости таблицы
    /// </summary>
    public IEnumerable<Symbol> GetAvailableSymbols() =>
        _symbols.Values.Concat(_openScope?.GetAvailableSymbols() ?? Array.Empty<Symbol>());

    public void AddSymbol(Symbol symbol) =>
        _symbols[symbol.Id] = symbol;

    /// <summary>
    /// Поиск эффективного символа
    /// </summary>
    public TSymbol FindSymbol<TSymbol>(string id) where TSymbol : Symbol
    {
        var hasInsideTheScope = _symbols.TryGetValue(id, out var symbol);
        return !hasInsideTheScope
            ? _openScope?.FindSymbol<TSymbol>(id)
            : symbol as TSymbol;
    }

    /// <summary>
    /// Проверяет наличие собственного символа
    /// </summary>
    public bool ContainsSymbol(string id) =>
        _symbols.ContainsKey(id);
}