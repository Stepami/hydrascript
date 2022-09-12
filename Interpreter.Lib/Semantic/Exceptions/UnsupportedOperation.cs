using Interpreter.Lib.FrontEnd.Lex;
using Interpreter.Lib.Semantic.Types;

namespace Interpreter.Lib.Semantic.Exceptions
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