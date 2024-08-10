using System.Collections;
using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.RegularExpressions;
using HydraScript.Domain.FrontEnd.Lexer.TokenTypes;

namespace HydraScript.Domain.FrontEnd.Lexer.Impl;

public class Structure<TContainer>(ITokenTypesProvider provider) : IStructure
    where TContainer : IGeneratedRegexContainer
{
    private FrozenDictionary<string, TokenType> Types { get; } = provider.GetTokenTypes()
        .Concat([new EndOfProgramType(), new ErrorType()])
        .ToFrozenDictionary(x => x.Tag);

    public Regex Regex { get; } = TContainer.GetRegex();

    public TokenType FindByTag(string tag) =>
        Types[tag];

    public override string ToString() =>
        new StringBuilder()
            .AppendJoin('\n', this)
            .ToString();

    public IEnumerator<TokenType> GetEnumerator() =>
        ((IEnumerable<TokenType>)Types.Values).GetEnumerator();

    [ExcludeFromCodeCoverage]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}