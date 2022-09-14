using Interpreter.Lib.FrontEnd.GetTokens.Impl;
using Interpreter.Lib.IR.CheckSemantics.Types;

namespace Interpreter.Lib.IR.CheckSemantics.Exceptions
{
    public class WrongConditionalTypes : SemanticException
    {
        public WrongConditionalTypes(Segment cSegment, Type cType, Segment aSegment, Type aType) :
            base(cSegment + aSegment, $"Different types in conditional:  {cSegment} consequent - {cType}, {aSegment} alternate {aType}") { }
    }
}