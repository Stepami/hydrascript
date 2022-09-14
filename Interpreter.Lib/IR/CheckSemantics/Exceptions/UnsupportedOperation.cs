using Interpreter.Lib.FrontEnd.GetTokens.Impl;
using Interpreter.Lib.IR.CheckSemantics.Types;

namespace Interpreter.Lib.IR.CheckSemantics.Exceptions
{
    public class UnsupportedOperation : SemanticException
    {
        public UnsupportedOperation(Segment segment, Type type, string @operator) :
            base(
                $"{segment} Type {type} does not support operation {@operator}"
            )
        {
        }
    }
}