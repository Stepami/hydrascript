using Interpreter.Lib.FrontEnd.GetTokens.Data;

namespace Interpreter.Lib.IR.CheckSemantics.Exceptions
{
    public class WrongTypeOfArgument : SemanticException
    {
        public WrongTypeOfArgument(Segment segment, Type expected, Type actual) :
            base(segment,$"Wrong type of argument: expected {expected}, actual {actual}") { }
    }
}