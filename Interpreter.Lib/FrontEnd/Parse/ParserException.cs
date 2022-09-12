using System;
using Interpreter.Lib.FrontEnd.Lex;

namespace Interpreter.Lib.FrontEnd.Parse
{
    public class ParserException : Exception
    {
        public ParserException(Segment segment, string expected, Token actual) : 
            base($"Wrong syntax: {segment} expected {expected}; actual = ({actual.Type.Tag}, {actual.Value})")
        {
        }
    }
}