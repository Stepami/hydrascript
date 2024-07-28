using System.Diagnostics.CodeAnalysis;

namespace HydraScript.Application.StaticAnalysis.Exceptions;

[ExcludeFromCodeCoverage]
public class ArrayAccessException(string segment, Type type) : SemanticException(
    segment,
    $"Array element cannot be accessed with type {type} it must be of type number");