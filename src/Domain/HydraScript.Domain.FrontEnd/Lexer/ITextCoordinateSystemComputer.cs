namespace HydraScript.Domain.FrontEnd.Lexer;

public interface ITextCoordinateSystemComputer
{
    IReadOnlyList<int> GetLines(string text);
}