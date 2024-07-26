using System.Diagnostics.CodeAnalysis;

namespace HydraScript.Lib.IR.CheckSemantics.Exceptions;

[ExcludeFromCodeCoverage]
public class WrongNumberOfArguments(string segment, int expected, int actual) :
    SemanticException(
        segment,
        $"Wrong number of arguments: expected {expected}, actual {actual}");