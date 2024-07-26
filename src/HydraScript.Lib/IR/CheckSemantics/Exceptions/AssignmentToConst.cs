using System.Diagnostics.CodeAnalysis;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;

namespace HydraScript.Lib.IR.CheckSemantics.Exceptions;

[ExcludeFromCodeCoverage]
public class AssignmentToConst(IdentifierReference ident)
    : SemanticException(ident.Segment, $"Cannot assign to const: {ident.Name}");