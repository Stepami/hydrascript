namespace HydraScript.Domain.FrontEnd.Lexer;

public interface ILexer
{
    public IStructure Structure { get; }

    public IEnumerable<Token> GetTokens(string text);
}