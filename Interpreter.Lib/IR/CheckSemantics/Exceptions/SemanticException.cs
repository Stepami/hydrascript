using System.Diagnostics.CodeAnalysis;
using Interpreter.Lib.FrontEnd.GetTokens.Data;

namespace Interpreter.Lib.IR.CheckSemantics.Exceptions;

[Serializable, ExcludeFromCodeCoverage]
public abstract class SemanticException : Exception
{
    protected SemanticException() { }
        
    protected SemanticException(string message) : base(message) { }
        
    protected SemanticException(string message, Exception inner) : base(message, inner) { }
        
    protected SemanticException(Segment segment, string message) :
        base($"{segment} {message}") { }
}