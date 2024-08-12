using System.Diagnostics.CodeAnalysis;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;

namespace HydraScript.Application.StaticAnalysis.Exceptions;

[ExcludeFromCodeCoverage]
public class AccessBeforeInitialization(IdentifierReference variable) : SemanticException(
    variable.Segment,
    $"Cannot access '{variable.Name}' before initialization");