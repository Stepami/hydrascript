using Interpreter.Lib.FrontEnd.GetTokens;
using Interpreter.Lib.Semantic.Types;

namespace Interpreter.Lib.Semantic.Exceptions
{
    public class NotBooleanTestExpression : SemanticException
    {
        public NotBooleanTestExpression(Segment segment, Type type) :
            base(
                $"{segment} Type of expression is {type} but expected boolean"
            )
        {
        }
    }
}