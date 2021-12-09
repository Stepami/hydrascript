using Interpreter.Lib.Semantic.Nodes.Expressions.PrimaryExpressions;

namespace Interpreter.Lib.Semantic.Exceptions
{
    public class UnknownIdentifierReference : SemanticException
    {
        public UnknownIdentifierReference(IdentifierReference ident) :
            base(
                $"{ident.Segment} Unknown identifier reference: {ident.Id}"
            )
        {
        }
    }
}