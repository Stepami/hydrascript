using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace HydraScript.UnitTests.Domain.FrontEnd;

public record LexerInput([property:MinLength(10), MaxLength(25)] TokenInput[] TokenInputs) : IReadOnlyList<string>
{
    public IEnumerator<string> GetEnumerator() =>
        TokenInputs.Select(x => x.Value).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public int Count => TokenInputs.Length;

    public string this[int index] => TokenInputs[index].Value;

    public override string ToString() =>
        TokenInputs.Aggregate(
            TokenInput.AdditiveIdentity,
            (x, y) => x + y).Value;
}

public record TokenInput(
    [property: RegularExpression(TokenInput.Pattern)]
    string Value) :
    IAdditiveIdentity<TokenInput, TokenInput>,
    IAdditionOperators<TokenInput, TokenInput, TokenInput>
{
    [StringSyntax(StringSyntaxAttribute.Regex)]
    public const string Pattern = "[a-zA-Z]+|[0-9]+|[+]{2}";

    public static TokenInput operator +(TokenInput left, TokenInput right) =>
        new(left.Value + " " + right.Value);

    public static TokenInput AdditiveIdentity { get; } = new(string.Empty);
}