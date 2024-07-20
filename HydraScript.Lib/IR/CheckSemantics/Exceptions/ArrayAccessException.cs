using System.Diagnostics.CodeAnalysis;
using HydraScript.Lib.FrontEnd.GetTokens.Data;

namespace HydraScript.Lib.IR.CheckSemantics.Exceptions;

[ExcludeFromCodeCoverage]
public class ArrayAccessException : SemanticException
{
    public ArrayAccessException(Segment segment, Type type) :
        base(segment, $"Array element cannot be accessed with type {type} it must be of type number") { }
}