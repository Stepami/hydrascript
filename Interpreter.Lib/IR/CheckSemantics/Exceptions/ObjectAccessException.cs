using System.Diagnostics.CodeAnalysis;
using Interpreter.Lib.FrontEnd.GetTokens.Data;
using Interpreter.Lib.IR.CheckSemantics.Types;

namespace Interpreter.Lib.IR.CheckSemantics.Exceptions;

[ExcludeFromCodeCoverage]
public class ObjectAccessException : SemanticException
{
    public ObjectAccessException(Segment segment, ObjectType objectType, string field) :
        base(segment, $"Object type {objectType} has no field {field}") { }
}