namespace HydraScript.Domain.FrontEnd.Lexer;

public interface ITextCoordinateSystemComputer
{
    /// <summary>
    /// Возвращает список индексов переноса строки внутри фрагмента исходного кода<br/>
    /// Всегда начинается с -1
    /// </summary>
    public IReadOnlyList<int> GetLines(string text);
}