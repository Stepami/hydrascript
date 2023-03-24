using System.Diagnostics.CodeAnalysis;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;

namespace Interpreter.Lib.IR.CheckSemantics.Exceptions;

[ExcludeFromCodeCoverage]
public class UnknownIdentifierReference : SemanticException
{
    public UnknownIdentifierReference(IdentifierReference ident) :
        base(ident.Segment, $"Unknown identifier reference: {ident.Name}") { }
}