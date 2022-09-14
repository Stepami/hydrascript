using System;
using Interpreter.Lib.FrontEnd.GetTokens.Impl;

namespace Interpreter.Lib.IR.CheckSemantics.Exceptions
{
    public abstract class SemanticException : Exception
    {
        protected SemanticException(Segment segment, string message) :
            base($"{segment} {message}") { }
    }
}