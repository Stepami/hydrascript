using System.Collections;
using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Cysharp.Text;
using HydraScript.Domain.FrontEnd.Lexer.TokenTypes;

namespace HydraScript.Domain.FrontEnd.Lexer.Impl;

public class Structure<TContainer>(ITokenTypesProvider provider) : IStructure
    where TContainer : IGeneratedRegexContainer
{
    private FrozenDictionary<string, TokenType> Types { get; } = provider.GetTokenTypes()
        .Concat([new EndOfProgramType(), new ErrorType()])
        .ToFrozenDictionary(x => x.Tag);

    public Regex Regex { get; } = TContainer.Regex;

    public TokenType FindByTag(string tag) =>
        Types[tag];

    public override string ToString() => ZString.Join<TokenType>('\n', this);

    // ReSharper disable once NotDisposedResourceIsReturned
    public IEnumerator<TokenType> GetEnumerator() =>
        Types.Values.AsEnumerable().GetEnumerator();

    [ExcludeFromCodeCoverage]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}