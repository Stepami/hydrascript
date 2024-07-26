using System.Diagnostics.CodeAnalysis;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;

namespace HydraScript.Lib.IR.CheckSemantics.Exceptions;

[ExcludeFromCodeCoverage]
public class UnknownIdentifierReference(IdentifierReference ident) :
    SemanticException(ident.Segment, $"Unknown identifier reference: {ident.Name}");