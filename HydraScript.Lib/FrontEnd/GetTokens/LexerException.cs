using System.Diagnostics.CodeAnalysis;
using HydraScript.Lib.FrontEnd.GetTokens.Data;

namespace HydraScript.Lib.FrontEnd.GetTokens;

[Serializable, ExcludeFromCodeCoverage]
public class LexerException : Exception
{
    public LexerException() { }
        
    protected LexerException(string message) : base(message) { }
        
    protected LexerException(string message, Exception inner) : base(message, inner) { }
        
    public LexerException(Token token) :
        base($"Unknown token {token}") { }
}