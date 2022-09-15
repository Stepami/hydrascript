using System;
using Interpreter.Lib.FrontEnd.GetTokens.Data;

namespace Interpreter.Lib.FrontEnd.GetTokens
{
    public class LexerException : Exception
    {
        public LexerException(Token token) :
            base($"Unknown token {token}")
        {
        }
    }
}