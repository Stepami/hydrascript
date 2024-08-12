using HydraScript.Domain.FrontEnd.Lexer.TokenTypes;

namespace HydraScript.Domain.FrontEnd.Lexer;

public interface ITokenTypesProvider
{
    IEnumerable<TokenType> GetTokenTypes();
}