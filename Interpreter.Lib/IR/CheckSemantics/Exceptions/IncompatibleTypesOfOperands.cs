using Interpreter.Lib.FrontEnd.GetTokens.Data;

namespace Interpreter.Lib.IR.CheckSemantics.Exceptions;

public class IncompatibleTypesOfOperands : SemanticException
{
    public IncompatibleTypesOfOperands(Segment segment, Type left, Type right) :
        base(segment, $"Incompatible types of operands: {left} and {right}") { }
}