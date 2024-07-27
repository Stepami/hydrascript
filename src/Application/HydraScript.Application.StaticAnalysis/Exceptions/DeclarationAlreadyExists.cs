using System.Diagnostics.CodeAnalysis;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;

namespace HydraScript.Application.StaticAnalysis.Exceptions;

[ExcludeFromCodeCoverage]
public class DeclarationAlreadyExists(IdentifierReference ident) :
    SemanticException(ident.Segment, $"Declaration already exists: {ident.Name}");