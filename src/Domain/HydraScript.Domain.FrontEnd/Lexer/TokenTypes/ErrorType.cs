namespace HydraScript.Domain.FrontEnd.Lexer.TokenTypes;

internal record ErrorType() : TokenType("ERROR")
{
    public override bool Error() => true;
}