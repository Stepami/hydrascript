using System.Diagnostics.CodeAnalysis;

namespace Interpreter.Lib.IR.CheckSemantics.Exceptions;

[ExcludeFromCodeCoverage]
public class NonAccessibleType : SemanticException
{
    public NonAccessibleType(Type type) :
        base($"Type '{type}' is not array-like or object-like") { }
}