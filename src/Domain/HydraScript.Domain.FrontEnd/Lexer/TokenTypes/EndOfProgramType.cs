namespace HydraScript.Domain.FrontEnd.Lexer.TokenTypes;

internal record EndOfProgramType() : TokenType("EOP", "", int.MaxValue - 1)
{
    public override bool EndOfProgram() => true;
}