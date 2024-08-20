namespace HydraScript.Domain.FrontEnd.Lexer;

public interface ITextCoordinateSystemComputer
{
    /// <summary>
    /// Возвращает список индексов переноса строки внутри фрагмента исходного кода<br/>
    /// Всегда начинается с -1
    /// </summary>
    public IReadOnlyList<int> GetLines(string text);

    /// <summary>
    /// Строит координату в формате (Строка, Столбец)
    /// </summary>
    /// <param name="absoluteIndex">Индекс символа от начала строки, в диапазоне [0, ДлинаСтроки)</param>
    /// <param name="newLineList">Список индексов переноса строки <see cref="GetLines"/></param>
    /// <returns></returns>
    public Coordinates GetCoordinates(
        int absoluteIndex,
        IReadOnlyList<int> newLineList);
}