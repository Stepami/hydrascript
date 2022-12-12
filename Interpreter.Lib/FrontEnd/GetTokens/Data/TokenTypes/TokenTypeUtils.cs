namespace Interpreter.Lib.FrontEnd.GetTokens.Data.TokenTypes;

public static class TokenTypeUtils
{
    public static readonly TokenType End = new EndOfProgramType();
    public static readonly TokenType Error = new ErrorType();
}