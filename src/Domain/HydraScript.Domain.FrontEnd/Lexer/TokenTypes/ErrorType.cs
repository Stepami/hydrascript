namespace HydraScript.Domain.FrontEnd.Lexer.TokenTypes;

public record ErrorType() : TokenType("ERROR")
{
    public override bool Error() => true;
}