using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.RegularExpressions;
using HydraScript.Domain.FrontEnd.Lexer.TokenTypes;

namespace HydraScript.Domain.FrontEnd.Lexer.Impl;

public class Structure : IStructure
{
    public Structure(ITokenTypesProvider provider)
    {
        Types = provider.GetTokenTypes()
            .Concat([new EndOfProgramType(), new ErrorType()])
            .OrderBy(t => t.Priority)
            .ToDictionary(x => x.Tag);

        Regex = new Regex(
            string.Join(
                '|',
                this.Where(t => !t.EndOfProgram())
                    .Select(t => t.GetNamedRegex())
                    .ToList()));
    }

    private Dictionary<string, TokenType> Types { get; }

    public Regex Regex { get; }

    public TokenType FindByTag(string tag) =>
        Types[tag];

    public override string ToString() =>
        new StringBuilder()
            .AppendJoin('\n',
                Types.Select(x => $"{x.Key} {x.Value.Pattern}"))
            .ToString();

    public IEnumerator<TokenType> GetEnumerator() =>
        Types.Values.GetEnumerator();

    [ExcludeFromCodeCoverage]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}