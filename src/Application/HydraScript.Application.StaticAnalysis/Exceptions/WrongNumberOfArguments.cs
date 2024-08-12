using System.Diagnostics.CodeAnalysis;

namespace HydraScript.Application.StaticAnalysis.Exceptions;

[ExcludeFromCodeCoverage]
public class WrongNumberOfArguments(string segment, int expected, int actual) :
    SemanticException(
        segment,
        $"Wrong number of arguments: expected {expected}, actual {actual}");