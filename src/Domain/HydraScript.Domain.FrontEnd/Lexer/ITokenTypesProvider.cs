using System.Collections.Frozen;
using HydraScript.Domain.FrontEnd.Lexer.TokenTypes;

namespace HydraScript.Domain.FrontEnd.Lexer;

public interface ITokenTypesProvider
{
    FrozenDictionary<string, TokenType> GetTokenTypes();
}