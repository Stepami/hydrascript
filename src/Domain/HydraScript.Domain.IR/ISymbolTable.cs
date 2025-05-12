namespace HydraScript.Domain.IR;

public interface ISymbolTable
{
    /// <summary>
    /// Добавление области видимости
    /// </summary>
    /// <param name="table">Доступная область видимости</param>
    public void AddOpenScope(ISymbolTable table);

    /// <summary>
    /// Символы доступные в области видимости таблицы
    /// </summary>
    public IEnumerable<ISymbol> GetAvailableSymbols();

    /// <summary>
    /// Добавление собственного символа в область видимости
    /// </summary>
    /// <param name="symbol">Собственный символ</param>
    public void AddSymbol(ISymbol symbol);

    /// <summary>
    /// Добавление собственного символа в область видимости по заданному ключу
    /// </summary>
    /// <param name="symbol">Собственный символ</param>
    /// <param name="symbolId">Заданный ключ</param>
    public void AddSymbol<TSymbol>(TSymbol symbol, ISymbolId<TSymbol> symbolId)
        where TSymbol : class, ISymbol;

    /// <summary>
    /// Поиск эффективного символа
    /// </summary>
    /// <param name="id">Идентификатор символа</param>
    public TSymbol? FindSymbol<TSymbol>(ISymbolId<TSymbol> id)
        where TSymbol : class, ISymbol;

    /// <summary>
    /// Проверка наличия собственного символа
    /// </summary>
    /// <param name="id">Идентификатор символа</param>
    public bool ContainsSymbol(ISymbolId<ISymbol> id);
}