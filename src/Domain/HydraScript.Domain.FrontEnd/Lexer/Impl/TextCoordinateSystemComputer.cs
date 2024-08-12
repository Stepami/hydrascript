using System.Buffers;

namespace HydraScript.Domain.FrontEnd.Lexer.Impl;

public class TextCoordinateSystemComputer : ITextCoordinateSystemComputer
{
    private readonly SearchValues<char> _sv = SearchValues.Create(['\n']);

    public IReadOnlyList<int> GetLines(string text)
    {
        var newText = text.EndsWith(Environment.NewLine)
            ? text
            : text + Environment.NewLine;
        var textLength = newText.Length;

        LinkedList<int> indices = [];
        while (true)
        {
            var start = indices.Last != null ? indices.Last.Value + 1 : 0;
            if (start == textLength)
                break;
            var textAsSpan = newText.AsSpan(
                start,
                length: textLength - start);
            var index = textAsSpan.IndexOfAny(_sv);
            indices.AddLast(start + index);
        }

        return indices.ToList();
    }
}