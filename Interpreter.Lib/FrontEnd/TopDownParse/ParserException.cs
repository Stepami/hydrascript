using System.Diagnostics.CodeAnalysis;
using Interpreter.Lib.FrontEnd.GetTokens.Data;

namespace Interpreter.Lib.FrontEnd.TopDownParse;

[Serializable, ExcludeFromCodeCoverage]
public class ParserException : Exception
{
    public ParserException() { }
        
    public ParserException(string message) : base(message) { }
        
    protected ParserException(string message, Exception inner) : base(message, inner) { }
        
    public ParserException(Segment segment, string expected, Token actual) : 
        base($"Wrong syntax: {segment} expected {expected}; actual = ({actual.Type.Tag}, {actual.Value})")
    {
    }
}