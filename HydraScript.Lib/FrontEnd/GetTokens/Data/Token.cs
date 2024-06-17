using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using HydraScript.Lib.FrontEnd.GetTokens.Data.TokenTypes;

namespace HydraScript.Lib.FrontEnd.GetTokens.Data;

[ExcludeFromCodeCoverage]
public record Token(TokenType Type)
{
    public Segment Segment { get; }

    public string Value { get; }

    public Token(TokenType type, Segment segment, string value) :
        this(type) =>
        (Segment, Value) = (segment, value);

    public override string ToString()
    {
        var displayValue = Value;
        if (displayValue != null) displayValue = Regex.Replace(Value, "\"", "\\\"");
        if (Type.CanIgnore()) displayValue = "";
        return $"{Type} {Segment}: {displayValue}";
    }
}
    
[ExcludeFromCodeCoverage]
public record Segment(Coordinates Start, Coordinates End)
{
    public override string ToString() => $"{Start}-{End}";

    public static Segment operator +(Segment left, Segment right) => 
        new(left.Start, right.End);
}
    
[ExcludeFromCodeCoverage]
public record Coordinates(int Line, int Column)
{
    public Coordinates(int absolutePos, IReadOnlyList<int> system) :
        this(0, 0)
    {
        for (var i = 0; i < system.Count; i++)
            if (absolutePos <= system[i])
            {
                var offset = i == 0 ? -1 : system[i - 1];
                (Line, Column) = (i + 1, absolutePos - offset);
                break;
            }

        if (Line == 0)
        {
            (Line, Column) = (system.Count + 1, 1);
        }
    }

    public override string ToString() => $"({Line}, {Column})";
}