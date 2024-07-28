namespace HydraScript.Domain.FrontEnd.Lexer;

public interface ILexer
{
    public Structure Structure { get; }

    public List<Token> GetTokens(string text);
}