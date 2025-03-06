using HydraScript.Domain.Constants;

namespace HydraScript.Infrastructure.LexerRegexGenerator;

internal class DefaultTokenTypesStreamProvider :
    ITokenTypesStreamProvider
{
    public IEnumerable<TokenTypes.Dto> TokenTypesStream { get; } = TokenTypes.Stream;
}