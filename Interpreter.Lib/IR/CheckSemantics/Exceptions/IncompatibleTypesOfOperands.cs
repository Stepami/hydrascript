using System.Diagnostics.CodeAnalysis;
using Interpreter.Lib.FrontEnd.GetTokens.Data;

namespace Interpreter.Lib.IR.CheckSemantics.Exceptions;

[ExcludeFromCodeCoverage]
public class IncompatibleTypesOfOperands : SemanticException
{
    public IncompatibleTypesOfOperands(Segment segment, Type left, Type right) :
        base(segment, $"Incompatible types of operands: {left} and {right}") { }
}