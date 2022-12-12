using Interpreter.Lib.FrontEnd.GetTokens.Data;

namespace Interpreter.Lib.IR.CheckSemantics.Exceptions;

public class CannotDefineType : SemanticException
{
    public CannotDefineType(Segment segment) :
        base(segment, "Cannot define type") { }
}