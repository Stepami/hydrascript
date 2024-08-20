using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using HydraScript.Domain.FrontEnd.Lexer.TokenTypes;

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
}
    
[ExcludeFromCodeCoverage]
public record Coordinates(int Line, int Column)
{
    public Coordinates() : this(Line: 1, Column: 1)
    {
    }

    public override string ToString() => $"({Line}, {Column})";
}