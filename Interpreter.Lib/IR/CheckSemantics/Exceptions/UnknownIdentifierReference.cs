using Interpreter.Lib.IR.Ast.Nodes.Expressions.PrimaryExpressions;

namespace Interpreter.Lib.IR.CheckSemantics.Exceptions
{
    public class UnknownIdentifierReference : SemanticException
    {
        public UnknownIdentifierReference(IdentifierReference ident) :
            base(ident.Segment, $"Unknown identifier reference: {ident.Id}") { }
    }
}