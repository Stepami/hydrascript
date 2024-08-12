namespace HydraScript.Domain.FrontEnd.Lexer.TokenTypes;

public record IgnorableType(string Tag) : TokenType(Tag)
{
    public override bool CanIgnore() => true;
}