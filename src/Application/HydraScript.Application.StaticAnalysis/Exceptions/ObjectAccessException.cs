using System.Diagnostics.CodeAnalysis;
using HydraScript.Domain.IR.Types;

namespace HydraScript.Application.StaticAnalysis.Exceptions;

[ExcludeFromCodeCoverage]
public class ObjectAccessException(string segment, ObjectType objectType, string field) :
    SemanticException(segment, $"Object type {objectType} has no field {field}");