using Interpreter.Lib.FrontEnd.GetTokens.Impl;

namespace Interpreter.Lib.IR.CheckSemantics.Exceptions
{
    public class WrongNumberOfArguments : SemanticException
    {
        public WrongNumberOfArguments(Segment segment, int expected, int actual) :
            base(segment, $"Wrong number of arguments: expected {expected}, actual {actual}") { }
    }
}