using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using HydraScript.Lib.FrontEnd.GetTokens.Data.TokenTypes;

namespace HydraScript.Lib.FrontEnd.GetTokens.Data;

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
    public override string ToString() => Type.ToString();
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