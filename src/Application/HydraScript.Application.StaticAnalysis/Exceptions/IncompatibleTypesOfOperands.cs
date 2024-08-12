using System.Diagnostics.CodeAnalysis;

namespace HydraScript.Application.StaticAnalysis.Exceptions;

[ExcludeFromCodeCoverage]
public class IncompatibleTypesOfOperands(string segment, Type left, Type right) :
    SemanticException(segment, $"Incompatible types of operands: {left} and {right}");