using Interpreter.Lib.FrontEnd.GetTokens.Data;

namespace Interpreter.Lib.IR.CheckSemantics.Exceptions
{
    public class WrongArrayLiteralDeclaration : SemanticException
    {
        public WrongArrayLiteralDeclaration(Segment segment, Type type) : 
            base(segment, $"{segment} Wrong array literal declaration: all array elements must be of type {type}") { }
    }
}