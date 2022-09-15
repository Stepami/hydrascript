using System;
using Interpreter.Lib.FrontEnd.GetTokens.Data;

namespace Interpreter.Lib.FrontEnd.GetTokens
{
    [Serializable]
    public class LexerException : Exception
    {
        protected LexerException() { }
        
        protected LexerException(string message) : base(message) { }
        
        protected LexerException(string message, Exception inner) : base(message, inner) { }
        
        public LexerException(Token token) :
            base($"Unknown token {token}") { }
    }
}