using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Cysharp.Text;

namespace HydraScript.Domain.FrontEnd.Lexer.Impl;

public class RegexLexer(IStructure structure, ITextCoordinateSystemComputer computer) : ILexer, IEnumerable<Token>
{
    private IReadOnlyList<int> _lines = [];
    private string _text = "";

    public IStructure Structure { get; } = structure;

    public List<Token> GetTokens(string text)
    {
        _text = text;
        _lines = computer.GetLines(_text);

        return this.Where(t => !t.Type.CanIgnore()).ToList();
    }

    public IEnumerator<Token> GetEnumerator()
    {
        foreach (Match match in Structure.Regex.Matches(_text))
        {
            foreach (var type in Structure)
            {
                var group = match.Groups[type.Tag];

                if (!group.Success) continue;

                var value = group.Value;
                var segment = new Segment(
                    computer.GetCoordinates(group.Index, _lines),
                    computer.GetCoordinates(absoluteIndex: group.Index + group.Length, _lines));
                var token = new Token(type, segment, value);

                if (type.Error()) throw new LexerException(token);

                yield return token;
            }
        }

        yield return new EndToken();
    }

    [ExcludeFromCodeCoverage]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public override string ToString() => ZString.Join<Token>('\n', this);
}