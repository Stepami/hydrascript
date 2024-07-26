using System.Diagnostics.CodeAnalysis;

namespace HydraScript.Lib.IR.CheckSemantics.Exceptions;

[ExcludeFromCodeCoverage]
public class NonAccessibleType(Type type) :
    SemanticException($"Type '{type}' is not array-like or object-like");