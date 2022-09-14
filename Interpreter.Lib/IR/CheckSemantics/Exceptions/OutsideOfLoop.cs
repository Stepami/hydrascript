using Interpreter.Lib.FrontEnd.GetTokens.Impl;

namespace Interpreter.Lib.IR.CheckSemantics.Exceptions
{
    public class OutsideOfLoop : SemanticException
    {
        public OutsideOfLoop(Segment segment, string keyword) :
            base(
                $"{segment} \"{keyword}\" outside of loop"
            )
        {
        }
    }
}