namespace HydraScript.Domain.FrontEnd.Lexer.TokenTypes;

public record TokenType(string Tag)
{
    public virtual bool CanIgnore() => false;

    public virtual bool Error() => false;

    public sealed override string ToString() => Tag;
}