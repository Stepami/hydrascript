using Interpreter.Lib.FrontEnd.GetTokens.Data;

namespace Interpreter.Lib.IR.CheckSemantics.Exceptions;

public class UnsupportedOperation : SemanticException
{
    public UnsupportedOperation(Segment segment, Type type, string @operator) :
        base(segment, $"Type {type} does not support operation {@operator}") { }
}