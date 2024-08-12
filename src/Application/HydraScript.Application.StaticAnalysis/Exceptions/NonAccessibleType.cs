using System.Diagnostics.CodeAnalysis;

namespace HydraScript.Application.StaticAnalysis.Exceptions;

[ExcludeFromCodeCoverage]
public class NonAccessibleType(Type type) :
    SemanticException($"Type '{type}' is not array-like or object-like");