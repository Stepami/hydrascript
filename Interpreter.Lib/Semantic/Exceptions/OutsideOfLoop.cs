using Interpreter.Lib.RBNF.Analysis.Lexical;

namespace Interpreter.Lib.Semantic.Exceptions
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