using Interpreter.Lib.IR.Ast.Nodes.Expressions.PrimaryExpressions;

namespace Interpreter.Lib.IR.CheckSemantics.Exceptions
{
    public class ConstWithoutInitializer : SemanticException
    {
        public ConstWithoutInitializer(IdentifierReference ident) : base($"{ident.Segment} Const without initializer: {ident.Id}")
        {
        }
    }
}