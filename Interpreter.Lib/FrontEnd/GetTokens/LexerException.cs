using System;
using Interpreter.Lib.FrontEnd.GetTokens.Impl;

namespace Interpreter.Lib.FrontEnd.GetTokens
{
    public class LexerException : Exception
    {
        public LexerException(Token token) : base($"Unknown token {token}")
        {
        }
    }
}