using System.Diagnostics.CodeAnalysis;
using HydraScript.Lib.FrontEnd.GetTokens.Data;

namespace HydraScript.Lib.IR.CheckSemantics.Exceptions;

[ExcludeFromCodeCoverage]
public class NotBooleanTestExpression : SemanticException
{
    public NotBooleanTestExpression(Segment segment, Type type) :
        base(segment, $"Type of expression is {type} but expected boolean") { }
}