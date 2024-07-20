using System.Diagnostics.CodeAnalysis;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;

namespace HydraScript.Lib.IR.CheckSemantics.Exceptions;

[ExcludeFromCodeCoverage]
public class DeclarationAlreadyExists : SemanticException
{
    public DeclarationAlreadyExists(IdentifierReference ident) :
        base(ident.Segment, $"Declaration already exists: {ident.Name}") { }
}