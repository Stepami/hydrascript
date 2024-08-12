using System.Diagnostics.CodeAnalysis;

namespace HydraScript.Application.StaticAnalysis.Exceptions;

[ExcludeFromCodeCoverage]
public class UnsupportedOperation(string segment, Type type, string @operator) :
    SemanticException(segment, $"Type {type} does not support operation {@operator}");