using System.Diagnostics.CodeAnalysis;

namespace HydraScript.Lib.IR.CheckSemantics.Exceptions;

[ExcludeFromCodeCoverage]
public class UnsupportedOperation(string segment, Type type, string @operator) :
    SemanticException(segment, $"Type {type} does not support operation {@operator}");