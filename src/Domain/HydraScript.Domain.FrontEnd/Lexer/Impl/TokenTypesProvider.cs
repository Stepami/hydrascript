using HydraScript.Domain.FrontEnd.Lexer.TokenTypes;

namespace HydraScript.Domain.FrontEnd.Lexer.Impl;

public class TokenTypesProvider : ITokenTypesProvider
{
    public IEnumerable<TokenType> GetTokenTypes() =>
        Constants.TokenTypes.Stream
            .OrderBy(x => x.Priority)
            .Select(x => x.CanIgnore
                ? new IgnorableType(x.Tag)
                : new TokenType(x.Tag));
}