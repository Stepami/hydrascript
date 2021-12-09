using Interpreter.Lib.Semantic.Nodes.Expressions.PrimaryExpressions;

namespace Interpreter.Lib.Semantic.Exceptions
{
    public class DeclarationAlreadyExists : SemanticException
    {
        public DeclarationAlreadyExists(IdentifierReference ident) : base($"{ident.Segment} Declaration already exists: {ident.Id}")
        {
        }
    }
}