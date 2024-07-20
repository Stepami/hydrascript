using System.Diagnostics.CodeAnalysis;
using HydraScript.Lib.FrontEnd.GetTokens.Data;
using HydraScript.Lib.IR.CheckSemantics.Types;

namespace HydraScript.Lib.IR.CheckSemantics.Exceptions;

[ExcludeFromCodeCoverage]
public class ObjectAccessException : SemanticException
{
    public ObjectAccessException(Segment segment, ObjectType objectType, string field) :
        base(segment, $"Object type {objectType} has no field {field}") { }
}