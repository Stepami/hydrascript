using System;
using Interpreter.Lib.RBNF.Analysis.Lexical;

namespace Interpreter.Lib.RBNF.Analysis.Exceptions
{
    public class LexerException : Exception
    {
        public LexerException(Token token) : base($"Unknown token {token}")
        {
        }
    }
}