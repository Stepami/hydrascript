using System;

namespace Interpreter.Lib.RBNF.Analysis.Exceptions
{
    public class ParserException : Exception
    {
        public ParserException(string message) : base($"Wrong syntax: {message}")
        {
        }
    }
}