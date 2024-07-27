namespace HydraScript.Domain.FrontEnd.Lexer.TokenTypes;

internal record ErrorType() : TokenType("ERROR", @"\S+", int.MaxValue)
{
    public override bool Error() => true;
}