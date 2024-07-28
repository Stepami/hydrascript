using System.Diagnostics.CodeAnalysis;

namespace HydraScript.Application.StaticAnalysis.Exceptions;

[ExcludeFromCodeCoverage]
public class WrongTypeOfArgument(string segment, Type expected, Type actual) :
    SemanticException(
        segment,
        $"Wrong type of argument: expected {expected}, actual {actual}");