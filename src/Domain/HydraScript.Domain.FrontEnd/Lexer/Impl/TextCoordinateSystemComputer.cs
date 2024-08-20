using System.Buffers;

namespace HydraScript.Domain.FrontEnd.Lexer.Impl;

public class TextCoordinateSystemComputer : ITextCoordinateSystemComputer
{
    private readonly SearchValues<char> _sv = SearchValues.Create(['\n']);

    /// <inheritdoc/>
    public IReadOnlyList<int> GetLines(string text)
    {
        var newText = text.EndsWith(Environment.NewLine)
            ? text
            : text + Environment.NewLine;
        var textLength = newText.Length;

        var indices = new List<int>(capacity: 128) { -1 };
        var textAsSpan = newText.AsSpan();
        while (true)
        {
            var start = indices[^1] + 1;
            if (start == textLength)
                break;
            var index = textAsSpan.Slice(
                start,
                length: textLength - start).IndexOfAny(_sv);
            indices.Add(start + index);
        }

        return indices.ToList();
    }
}