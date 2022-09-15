using Interpreter.Lib.FrontEnd.GetTokens.Data;
using Interpreter.Lib.IR.CheckSemantics.Types;

namespace Interpreter.Lib.IR.CheckSemantics.Exceptions
{
    public class NotBooleanTestExpression : SemanticException
    {
        public NotBooleanTestExpression(Segment segment, Type type) :
            base(segment, $"Type of expression is {type} but expected boolean") { }
    }
}