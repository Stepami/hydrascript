using System;

namespace Interpreter.Lib.IR.CheckSemantics.Exceptions
{
    public abstract class SemanticException : Exception
    {
        protected SemanticException(string message) : base(message)
        {
        }
    }
}