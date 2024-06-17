using System.Diagnostics.CodeAnalysis;
using HydraScript.Lib.FrontEnd.GetTokens.Data;

namespace HydraScript.Lib.IR.CheckSemantics.Exceptions;

[ExcludeFromCodeCoverage]
public class UnsupportedOperation : SemanticException
{
    public UnsupportedOperation(Segment segment, Type type, string @operator) :
        base(segment, $"Type {type} does not support operation {@operator}") { }
}