using System.Diagnostics.CodeAnalysis;
using HydraScript.Lib.IR.CheckSemantics.Types;

namespace HydraScript.Lib.IR.CheckSemantics.Exceptions;

[ExcludeFromCodeCoverage]
public class ObjectAccessException(string segment, ObjectType objectType, string field) :
    SemanticException(segment, $"Object type {objectType} has no field {field}");