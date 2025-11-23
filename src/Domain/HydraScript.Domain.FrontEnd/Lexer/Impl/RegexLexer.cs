using System.Text.RegularExpressions;

namespace HydraScript.Domain.FrontEnd.Lexer.Impl;

public class RegexLexer(IStructure structure, ITextCoordinateSystemComputer computer) : ILexer
{
    public IStructure Structure { get; } = structure;

    public IEnumerable<Token> GetTokens(string text)
    {
        var lines = computer.GetLines(text);

        foreach (Match match in Structure.Regex.Matches(text))
        {
            for (var i = 0; i < Structure.Count; i++)
            {
                var type = Structure[i];
                var group = match.Groups[type.Tag];

                if (!group.Success || type.CanIgnore()) continue;

                var value = group.Value;
                var segment = new Segment(
                    computer.GetCoordinates(group.Index, lines),
                    computer.GetCoordinates(absoluteIndex: group.Index + group.Length, lines));
                var token = new Token(type, segment, value);

                if (type.Error()) throw new LexerException(token);

                yield return token;
            }
        }

        yield return new EndToken();
    }
}