using Interpreter.Lib.FrontEnd.GetTokens;
using Interpreter.Lib.Semantic.Types;

namespace Interpreter.Lib.Semantic.Exceptions
{
    public class ArrayAccessException : SemanticException
    {
        public ArrayAccessException(Segment segment, Type type) : 
            base($"{segment} Array element cannot be accessed with type {type} it must be of type number")
        {
        }
    }
}