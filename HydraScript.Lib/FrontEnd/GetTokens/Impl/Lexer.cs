using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using HydraScript.Lib.FrontEnd.GetTokens.Data;

namespace HydraScript.Lib.FrontEnd.GetTokens.Impl;

public class Lexer(Structure structure) : ILexer, IEnumerable<Token>
{
    private readonly List<int> _lines = [];
    private string _text = "";

    public Structure Structure { get; } = structure;

    public List<Token> GetTokens(string text)
    {
        _text = text;

        _lines.Clear();
        if (!string.IsNullOrEmpty(text))
        {
            var lineMatches =
                new Regex(@"(?<NEWLINE>\n)").Matches(text[^1] == '\n'
                    ? text
                    : new string(text.Append('\n').ToArray()));
            foreach (Match match in lineMatches)
                _lines.Add(match.Groups["NEWLINE"].Index);
        }

        return this.Where(t => !t.Type.CanIgnore()).ToList();
    }

    public IEnumerator<Token> GetEnumerator()
    {
        var matches = Structure.Regex.Matches(_text);
        foreach (Match match in matches)
        {
            foreach (var type in Structure)
            {
                var group = match.Groups[type.Tag];

                if (!group.Success) continue;

                var value = group.Value;
                var segment = new Segment(
                    new Coordinates(group.Index, _lines),
                    new Coordinates(group.Index + group.Length, _lines)
                );
                var token = new Token(type, segment, value);

                if (type.Error()) throw new LexerException(token);

                yield return token;
            }
        }

        yield return new EndToken();
    }

    [ExcludeFromCodeCoverage]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public override string ToString() =>
        string.Join('\n', this);
}