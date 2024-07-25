using System.Diagnostics.CodeAnalysis;
using HydraScript.Lib.FrontEnd.GetTokens.Data;

namespace HydraScript.Lib.IR.CheckSemantics.Exceptions;

[ExcludeFromCodeCoverage]
public class CannotDefineType : SemanticException
{
    public CannotDefineType(Segment segment) :
        base(segment, "Cannot define type") { }
}