namespace HydraScript.Domain.FrontEnd.Lexer.TokenTypes;

internal record EndOfProgramType() : TokenType(EopTag)
{
    public const string EopTag = "EOP";
}