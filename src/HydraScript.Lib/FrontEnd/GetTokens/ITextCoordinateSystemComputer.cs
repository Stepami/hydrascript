namespace HydraScript.Lib.FrontEnd.GetTokens;

public interface ITextCoordinateSystemComputer
{
    IReadOnlyList<int> GetLines(string text);
}