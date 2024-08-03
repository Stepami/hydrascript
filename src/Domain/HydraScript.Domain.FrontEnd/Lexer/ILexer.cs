namespace HydraScript.Domain.FrontEnd.Lexer;

public interface ILexer
{
    public IStructure Structure { get; }

    public List<Token> GetTokens(string text);
}