using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using HydraScript.Domain.FrontEnd.Lexer.TokenTypes;
using ZLinq;

namespace HydraScript.Domain.FrontEnd.Lexer;

[ExcludeFromCodeCoverage]
public partial record Token(TokenType Type, Segment Segment, string Value)
{
    public override string ToString()
    {
        var displayValue = Quotes().Replace(Value, "\\\"");

        if (Type.CanIgnore()) displayValue = "";
        return $"{Type} {Segment}: {displayValue}";
    }

    [GeneratedRegex("\"")]
    private static partial Regex Quotes();
}

public record EndToken() : Token(new EndOfProgramType(), null!, null!)
{
    public override string ToString() => Type.Tag;
}
    
[ExcludeFromCodeCoverage]
public record Segment(Coordinates Start, Coordinates End)
{
    public override string ToString() => $"{Start}-{End}";

    public static Segment operator +(Segment left, Segment right) => 
        new(left.Start, right.End);

    public static implicit operator string(Segment segment) =>
        segment.ToString();

    public static implicit operator Segment(string segment)
    {
        var coords = segment.Split("-").AsValueEnumerable()
            .Select(x => x[1..^1].Replace(" ", string.Empty))
            .Select(x => x.Split(',').AsValueEnumerable().Select(int.Parse).ToArray())
            .ToArray();
        return new Segment(
            new Coordinates(coords[0][0], coords[0][1]),
            new Coordinates(coords[1][0], coords[1][1]));
    }
}
    
[ExcludeFromCodeCoverage]
public record Coordinates(int Line, int Column)
{
    public Coordinates() : this(Line: 1, Column: 1)
    {
    }

    public override string ToString() => $"({Line}, {Column})";
}