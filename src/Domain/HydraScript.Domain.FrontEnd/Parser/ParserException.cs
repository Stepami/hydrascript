using System.Diagnostics.CodeAnalysis;
using HydraScript.Domain.FrontEnd.Lexer;

namespace HydraScript.Domain.FrontEnd.Parser;

[Serializable, ExcludeFromCodeCoverage]
public class ParserException : Exception
{
    public ParserException() { }
        
    public ParserException(string message) : base(message) { }
        
    protected ParserException(string message, Exception inner) : base(message, inner) { }
        
    public ParserException(string? expected, Token actual) : 
        base($"Wrong syntax: {actual.Segment} expected {expected}; actual = ({actual.Type.Tag}, {actual.Value})")
    {
    }
}