namespace HydraScript.Domain.FrontEnd.Lexer.TokenTypes;

public record TokenType(string Tag, string Pattern, int Priority)
{
    public virtual bool CanIgnore() => false;

    public virtual bool EndOfProgram() => false;

    public virtual bool Error() => false;

    public string GetNamedRegex() => $"(?<{Tag}>{Pattern})";

    public override string ToString() => Tag;
}