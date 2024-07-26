using System.Diagnostics.CodeAnalysis;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;

namespace HydraScript.Lib.IR.CheckSemantics.Exceptions;

[ExcludeFromCodeCoverage]
public class DeclarationAlreadyExists(IdentifierReference ident) :
    SemanticException(ident.Segment, $"Declaration already exists: {ident.Name}");