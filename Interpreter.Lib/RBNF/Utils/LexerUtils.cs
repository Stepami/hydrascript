using Interpreter.Lib.RBNF.Analysis.Lexical.TokenTypes;

namespace Interpreter.Lib.RBNF.Utils
{
    public static class LexerUtils
    {
        public static readonly TokenType End = new EndOfProgramType();
        public static readonly TokenType Error = new ErrorType();
    }
}