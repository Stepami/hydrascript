using System.Diagnostics.CodeAnalysis;

namespace HydraScript.Application.StaticAnalysis.Exceptions;

[ExcludeFromCodeCoverage]
public class WrongReturnType(string segment, Type expected, Type actual) :
    SemanticException(
        segment,
        $"Wrong return type: expected {expected}, actual {actual}");