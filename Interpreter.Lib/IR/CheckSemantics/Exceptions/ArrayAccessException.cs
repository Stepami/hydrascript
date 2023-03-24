using System.Diagnostics.CodeAnalysis;
using Interpreter.Lib.FrontEnd.GetTokens.Data;

namespace Interpreter.Lib.IR.CheckSemantics.Exceptions;

[ExcludeFromCodeCoverage]
public class ArrayAccessException : SemanticException
{
    public ArrayAccessException(Segment segment, Type type) :
        base(segment, $"Array element cannot be accessed with type {type} it must be of type number") { }
}