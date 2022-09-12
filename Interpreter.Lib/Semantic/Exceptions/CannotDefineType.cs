using Interpreter.Lib.FrontEnd.GetTokens;

namespace Interpreter.Lib.Semantic.Exceptions
{
    public class CannotDefineType : SemanticException
    {
        public CannotDefineType(Segment segment) :
            base($"{segment} Cannot define type")
        {
        }
    }
}