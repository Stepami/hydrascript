using System.Diagnostics.CodeAnalysis;
using HydraScript.Lib.FrontEnd.GetTokens.Data;

namespace HydraScript.Lib.IR.CheckSemantics.Exceptions;

[Serializable, ExcludeFromCodeCoverage]
public abstract class SemanticException : Exception
{
    protected SemanticException() { }
        
    protected SemanticException(string message) : base(message) { }
        
    protected SemanticException(string message, Exception inner) : base(message, inner) { }
        
    protected SemanticException(Segment segment, string message) :
        base($"{segment} {message}") { }
}