using System.Diagnostics.CodeAnalysis;
using HydraScript.Lib.FrontEnd.GetTokens.Data;

namespace HydraScript.Lib.IR.CheckSemantics.Exceptions;

[ExcludeFromCodeCoverage]
public class FunctionWithoutReturnStatement : SemanticException
{
    public FunctionWithoutReturnStatement(Segment segment) :
        base(segment, "function with non-void return type must have a return statement") { }
}