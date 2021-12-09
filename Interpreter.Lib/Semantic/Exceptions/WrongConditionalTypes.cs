using Interpreter.Lib.RBNF.Analysis.Lexical;
using Interpreter.Lib.Semantic.Types;

namespace Interpreter.Lib.Semantic.Exceptions
{
    public class WrongConditionalTypes : SemanticException
    {
        public WrongConditionalTypes(Segment cSegment, Type cType, Segment aSegment, Type aType) :
            base(
                $"Different types in conditional:  {cSegment} consequent - {cType}, {aSegment} alternate {aType}"
            )
        {
        }
    }
}