using System.Diagnostics.CodeAnalysis;

namespace HydraScript.Application.StaticAnalysis.Exceptions;

[ExcludeFromCodeCoverage]
public class OutsideOfStatement(string segment, string keyword, string statement) :
    SemanticException(segment, $"Jump \"{keyword}\" outside of statement \"{statement}\"");