using Interpreter.Lib.FrontEnd.Lex;
using Interpreter.Lib.Semantic.Types;

namespace Interpreter.Lib.Semantic.Exceptions
{
    public class WrongTypeOfArgument : SemanticException
    {
        public WrongTypeOfArgument(Segment segment, Type expected, Type actual) :
            base(
                $"{segment} Wrong type of argument: expected {expected}, actual {actual}"
            )
        {
        }
    }
}