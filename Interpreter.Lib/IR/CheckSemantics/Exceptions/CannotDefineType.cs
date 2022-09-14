using Interpreter.Lib.FrontEnd.GetTokens.Impl;

namespace Interpreter.Lib.IR.CheckSemantics.Exceptions
{
    public class CannotDefineType : SemanticException
    {
        public CannotDefineType(Segment segment) :
            base(segment, "Cannot define type") { }
    }
}