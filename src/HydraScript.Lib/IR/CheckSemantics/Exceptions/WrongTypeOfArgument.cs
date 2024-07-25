using System.Diagnostics.CodeAnalysis;
using HydraScript.Lib.FrontEnd.GetTokens.Data;

namespace HydraScript.Lib.IR.CheckSemantics.Exceptions;

[ExcludeFromCodeCoverage]
public class WrongTypeOfArgument : SemanticException
{
    public WrongTypeOfArgument(Segment segment, Type expected, Type actual) :
        base(segment,$"Wrong type of argument: expected {expected}, actual {actual}") { }
}