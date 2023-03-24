using System.Diagnostics.CodeAnalysis;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;

namespace Interpreter.Lib.IR.CheckSemantics.Exceptions;

[ExcludeFromCodeCoverage]
public class DeclarationAlreadyExists : SemanticException
{
    public DeclarationAlreadyExists(IdentifierReference ident) :
        base(ident.Segment, $"Declaration already exists: {ident.Name}") { }
}