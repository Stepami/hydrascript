using System.Diagnostics.CodeAnalysis;
using HydraScript.Lib.FrontEnd.GetTokens.Data;

namespace HydraScript.Lib.IR.CheckSemantics.Exceptions;

[ExcludeFromCodeCoverage]
public class ReturnOutsideFunction : SemanticException
{
    public ReturnOutsideFunction(Segment segment) :
        base(segment, "\"return\" outside function") { }
}