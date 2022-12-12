using Interpreter.Lib.FrontEnd.GetTokens.Data;

namespace Interpreter.Lib.IR.CheckSemantics.Exceptions
{
    public class WrongConditionalTypes : SemanticException
    {
        public WrongConditionalTypes(Segment cSegment, Type cType, Segment aSegment, Type aType) :
            base(cSegment + aSegment, $"Different types in conditional:  {cSegment} consequent - {cType}, {aSegment} alternate {aType}") { }
    }
}