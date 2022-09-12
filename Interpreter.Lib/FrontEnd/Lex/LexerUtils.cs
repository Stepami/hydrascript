using Interpreter.Lib.FrontEnd.Lex.TokenTypes;

namespace Interpreter.Lib.FrontEnd.Lex
{
    public static class LexerUtils
    {
        public static readonly TokenType End = new EndOfProgramType();
        public static readonly TokenType Error = new ErrorType();
    }
}