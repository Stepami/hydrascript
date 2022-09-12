using Interpreter.Lib.FrontEnd.GetTokens.Impl.TokenTypes;

namespace Interpreter.Lib.FrontEnd.GetTokens
{
    public static class LexerUtils
    {
        public static readonly TokenType End = new EndOfProgramType();
        public static readonly TokenType Error = new ErrorType();
    }
}