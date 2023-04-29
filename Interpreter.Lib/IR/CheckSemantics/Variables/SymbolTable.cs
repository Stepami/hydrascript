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

    public void AddSymbol(Symbol symbol) =>
        _symbols[symbol.Id] = symbol;

    /// <summary>
    /// Поиск эффективного символа
    /// </summary>
    public T FindSymbol<T>(string id) where T : Symbol
    {
        var hasInsideTheScope = _symbols.TryGetValue(id, out var symbol);
        return !hasInsideTheScope
            ? _openScope?.FindSymbol<T>(id)
            : symbol as T;
    }

    /// <summary>
    /// Проверяет наличие собственного символа
    /// </summary>
    public bool ContainsSymbol(string id) =>
        _symbols.ContainsKey(id);

    public void Clear() => _symbols.Clear();
}