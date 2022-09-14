using Interpreter.Lib.IR.Ast.Nodes.Expressions.PrimaryExpressions;

namespace Interpreter.Lib.IR.CheckSemantics.Exceptions
{
    public class AssignmentToConst : SemanticException
    {
        public AssignmentToConst(IdentifierReference ident) :
            base(ident.Segment,$"Cannot assign to const: {ident.Id}") { }
    }
}