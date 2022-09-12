using System;

namespace Interpreter.Lib.FrontEnd.GetTokens
{
    public class LexerException : Exception
    {
        public LexerException(Token token) : base($"Unknown token {token}")
        {
        }
    }
}