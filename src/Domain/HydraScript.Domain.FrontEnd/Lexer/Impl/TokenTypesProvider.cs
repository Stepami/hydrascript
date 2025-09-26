using System.Collections.Frozen;
using HydraScript.Domain.FrontEnd.Lexer.TokenTypes;
using ZLinq;

namespace HydraScript.Domain.FrontEnd.Lexer.Impl;

public class TokenTypesProvider : ITokenTypesProvider
{
    public FrozenDictionary<string, TokenType> GetTokenTypes() =>
        Constants.TokenTypes.Stream.AsValueEnumerable()
            .OrderBy(x => x.Priority)
            .Select(x => x.CanIgnore
                ? new IgnorableType(x.Tag)
                : new TokenType(x.Tag))
            .Concat([new EndOfProgramType(), new ErrorType()])
            .ToFrozenDictionary(x => x.Tag);
}