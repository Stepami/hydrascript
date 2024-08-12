using System.Diagnostics.CodeAnalysis;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;

namespace HydraScript.Application.StaticAnalysis.Exceptions;

[ExcludeFromCodeCoverage]
public class ConstWithoutInitializer(IdentifierReference ident) :
    SemanticException(ident.Segment, $"'const' without initializer: {ident.Name}");