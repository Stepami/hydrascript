using System;

namespace Interpreter.Lib.Semantic.Exceptions
{
    public abstract class SemanticException : Exception
    {
        protected SemanticException(string message) : base(message)
        {
        }
    }
}