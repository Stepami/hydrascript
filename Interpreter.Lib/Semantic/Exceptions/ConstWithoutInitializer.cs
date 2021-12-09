using Interpreter.Lib.Semantic.Nodes.Expressions.PrimaryExpressions;

namespace Interpreter.Lib.Semantic.Exceptions
{
    public class ConstWithoutInitializer : SemanticException
    {
        public ConstWithoutInitializer(IdentifierReference ident) : base($"{ident.Segment} Const without initializer: {ident.Id}")
        {
        }
    }
}