using System;

namespace Interpreter.Lib.FrontEnd.Lex
{
    public class LexerException : Exception
    {
        public LexerException(Token token) : base($"Unknown token {token}")
        {
        }
    }
}