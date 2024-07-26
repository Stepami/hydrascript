using System.Diagnostics.CodeAnalysis;

namespace HydraScript.Lib.IR.CheckSemantics.Exceptions;

[Serializable, ExcludeFromCodeCoverage]
public abstract class SemanticException : Exception
{
    protected SemanticException()
    {
    }

    protected SemanticException(string message) : base(message)
    {
    }

    protected SemanticException(string message, Exception inner) : base(message, inner)
    {
    }

    protected SemanticException(string segment, string message) :
        base($"{segment} {message}")
    {
    }
}