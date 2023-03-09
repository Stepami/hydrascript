using Interpreter.Lib.IR.Ast.Nodes.Expressions.PrimaryExpressions;

namespace Interpreter.Lib.IR.CheckSemantics.Exceptions;

public class DeclarationAlreadyExists : SemanticException
{
    public DeclarationAlreadyExists(IdentifierReference ident) :
        base(ident.Segment, $"Declaration already exists: {ident.Name}") { }
}