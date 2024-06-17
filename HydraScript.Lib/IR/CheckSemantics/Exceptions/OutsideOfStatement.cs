using System.Diagnostics.CodeAnalysis;
using HydraScript.Lib.FrontEnd.GetTokens.Data;

namespace HydraScript.Lib.IR.CheckSemantics.Exceptions;

[ExcludeFromCodeCoverage]
public class OutsideOfStatement : SemanticException
{
    public OutsideOfStatement(Segment segment, string keyword, string statement) :
        base(segment, $"Jump \"{keyword}\" outside of statement \"{statement}\"") { }
}