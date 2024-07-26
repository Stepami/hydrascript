using System.Diagnostics.CodeAnalysis;

namespace HydraScript.Lib.IR.CheckSemantics.Exceptions;

[ExcludeFromCodeCoverage]
public class OutsideOfStatement(string segment, string keyword, string statement) :
    SemanticException(segment, $"Jump \"{keyword}\" outside of statement \"{statement}\"");