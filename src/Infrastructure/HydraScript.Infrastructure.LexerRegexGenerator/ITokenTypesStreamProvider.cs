using HydraScript.Domain.Constants;

namespace HydraScript.Infrastructure.LexerRegexGenerator;

public interface ITokenTypesStreamProvider
{
    public IEnumerable<TokenTypes.Dto> TokenTypesStream { get; }
}