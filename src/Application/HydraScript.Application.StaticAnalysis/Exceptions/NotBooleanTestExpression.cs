using System.Diagnostics.CodeAnalysis;

namespace HydraScript.Application.StaticAnalysis.Exceptions;

[ExcludeFromCodeCoverage]
public class NotBooleanTestExpression(string segment, Type type) :
    SemanticException(segment, $"Type of expression is {type} but expected boolean");