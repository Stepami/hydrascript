using System.Diagnostics.CodeAnalysis;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;
using HydraScript.Domain.IR.Impl.Symbols.Ids;

namespace HydraScript.Application.StaticAnalysis.Exceptions;

[ExcludeFromCodeCoverage]
public class OverloadAlreadyExists(IdentifierReference ident, FunctionSymbolId overload) :
    SemanticException(ident.Segment, $"Overload already exists: {overload}");