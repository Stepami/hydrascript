using System.Diagnostics.CodeAnalysis;
using Interpreter.Lib.FrontEnd.GetTokens.Data;

namespace Interpreter.Lib.FrontEnd.GetTokens;

[Serializable, ExcludeFromCodeCoverage]
public class LexerException : Exception
{
    public LexerException() { }
        
    protected LexerException(string message) : base(message) { }
        
    protected LexerException(string message, Exception inner) : base(message, inner) { }
        
    public LexerException(Token token) :
        base($"Unknown token {token}") { }
}