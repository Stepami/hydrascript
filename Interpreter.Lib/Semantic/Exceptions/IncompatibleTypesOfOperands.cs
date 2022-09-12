using Interpreter.Lib.FrontEnd.GetTokens.Impl;
using Interpreter.Lib.Semantic.Types;

namespace Interpreter.Lib.Semantic.Exceptions
{
    public class IncompatibleTypesOfOperands : SemanticException
    {
        public IncompatibleTypesOfOperands(Segment segment, Type left, Type right) :
            base(
                $"{segment} Incompatible types of operands: {left} and {right}"
            )
        {
        }
    }
}