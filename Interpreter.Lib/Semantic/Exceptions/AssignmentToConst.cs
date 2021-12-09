using Interpreter.Lib.Semantic.Nodes.Expressions.PrimaryExpressions;

namespace Interpreter.Lib.Semantic.Exceptions
{
    public class AssignmentToConst : SemanticException
    {
        public AssignmentToConst(IdentifierReference ident) :
            base(
                $"{ident.Segment} Cannot assign to const: {ident.Id}"
            )
        {
        }
    }
}