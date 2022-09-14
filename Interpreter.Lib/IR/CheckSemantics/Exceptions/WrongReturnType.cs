using Interpreter.Lib.FrontEnd.GetTokens.Impl;
using Interpreter.Lib.IR.CheckSemantics.Types;

namespace Interpreter.Lib.IR.CheckSemantics.Exceptions
{
    public class WrongReturnType : SemanticException
    {
        public WrongReturnType(Segment segment, Type expected, Type actual) :
            base(segment, $"Wrong return type: expected {expected}, actual {actual}") { }
    }
}