using System.Diagnostics.CodeAnalysis;

namespace HydraScript.Lib.IR.CheckSemantics.Exceptions;

[ExcludeFromCodeCoverage]
public class IncompatibleTypesOfOperands(string segment, Type left, Type right) :
    SemanticException(segment, $"Incompatible types of operands: {left} and {right}");