using System.Diagnostics.CodeAnalysis;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;

namespace HydraScript.Application.StaticAnalysis.Exceptions;

[ExcludeFromCodeCoverage]
public class UnknownIdentifierReference(IdentifierReference ident) :
    SemanticException(ident.Segment, $"Unknown identifier reference: {ident.Name}");