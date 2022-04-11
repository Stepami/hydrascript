using System;
using Interpreter.Lib.RBNF.Analysis.Lexical;

namespace Interpreter.Lib.RBNF.Analysis.Exceptions
{
    public class ParserException : Exception
    {
        public ParserException(Segment segment, string expected, Token actual) : 
            base($"Wrong syntax: {segment} expected {expected}; actual = ({actual.Type.Tag}, {actual.Value})")
        {
        }
    }
}