using System.Diagnostics.CodeAnalysis;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;

namespace HydraScript.Application.StaticAnalysis.Exceptions;

[ExcludeFromCodeCoverage]
public class AssignmentToConst(IdentifierReference ident)
    : SemanticException(ident.Segment, $"Cannot assign to const: {ident.Name}");