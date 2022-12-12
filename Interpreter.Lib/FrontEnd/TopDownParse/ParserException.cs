using Interpreter.Lib.FrontEnd.GetTokens.Data;

namespace Interpreter.Lib.FrontEnd.TopDownParse;

[Serializable]
public class ParserException : Exception
{
    public ParserException() { }
        
    protected ParserException(string message) : base(message) { }
        
    protected ParserException(string message, Exception inner) : base(message, inner) { }
        
    public ParserException(Segment segment, string expected, Token actual) : 
        base($"Wrong syntax: {segment} expected {expected}; actual = ({actual.Type.Tag}, {actual.Value})")
    {
    }
}