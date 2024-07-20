using System.Diagnostics.CodeAnalysis;
using HydraScript.Lib.FrontEnd.GetTokens.Data;

namespace HydraScript.Lib.IR.CheckSemantics.Exceptions;

[ExcludeFromCodeCoverage]
public class WrongReturnType : SemanticException
{
    public WrongReturnType(Segment segment, Type expected, Type actual) :
        base(segment, $"Wrong return type: expected {expected}, actual {actual}") { }
}