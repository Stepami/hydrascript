namespace HydraScript.Domain.FrontEnd.Lexer;

public interface ITextCoordinateSystemComputer
{
    public IReadOnlyList<int> GetLines(string text);
}