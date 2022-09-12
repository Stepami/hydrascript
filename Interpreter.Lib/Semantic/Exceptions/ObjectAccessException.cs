using Interpreter.Lib.FrontEnd.Lex;
using Interpreter.Lib.Semantic.Types;

namespace Interpreter.Lib.Semantic.Exceptions
{
    public class ObjectAccessException : SemanticException
    {
        public ObjectAccessException(Segment segment, ObjectType objectType, string field) :
            base($"{segment} Object type {objectType} has no field {field}")
        {
        }
    }
}