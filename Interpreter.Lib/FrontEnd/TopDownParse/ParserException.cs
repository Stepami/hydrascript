using System;
using Interpreter.Lib.FrontEnd.GetTokens.Data;

namespace Interpreter.Lib.FrontEnd.TopDownParse
{
    public class ParserException : Exception
    {
        public ParserException(Segment segment, string expected, Token actual) : 
            base($"Wrong syntax: {segment} expected {expected}; actual = ({actual.Type.Tag}, {actual.Value})")
        {
        }
    }
}