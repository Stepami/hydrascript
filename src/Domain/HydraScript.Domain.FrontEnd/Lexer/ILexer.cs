namespace HydraScript.Domain.FrontEnd.Lexer;

public interface ILexer
{
    Structure Structure { get; }

    List<Token> GetTokens(string text);
}