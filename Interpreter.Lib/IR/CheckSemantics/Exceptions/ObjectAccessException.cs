using Interpreter.Lib.FrontEnd.GetTokens.Impl;
using Interpreter.Lib.IR.CheckSemantics.Types;

namespace Interpreter.Lib.IR.CheckSemantics.Exceptions
{
    public class ObjectAccessException : SemanticException
    {
        public ObjectAccessException(Segment segment, ObjectType objectType, string field) :
            base($"{segment} Object type {objectType} has no field {field}")
        {
        }
    }
}