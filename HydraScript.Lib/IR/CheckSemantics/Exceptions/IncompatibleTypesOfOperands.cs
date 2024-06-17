using System.Diagnostics.CodeAnalysis;
using HydraScript.Lib.FrontEnd.GetTokens.Data;

namespace HydraScript.Lib.IR.CheckSemantics.Exceptions;

[ExcludeFromCodeCoverage]
public class IncompatibleTypesOfOperands : SemanticException
{
    public IncompatibleTypesOfOperands(Segment segment, Type left, Type right) :
        base(segment, $"Incompatible types of operands: {left} and {right}") { }
}