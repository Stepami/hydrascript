using System.Diagnostics.CodeAnalysis;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions;

namespace HydraScript.Application.StaticAnalysis.Exceptions;

[ExcludeFromCodeCoverage]
public class ExplicitCastNotSupported(CastAsExpression cast, Type from, Type to) :
    SemanticException(cast.Segment, $"Cast from {from} to {to} is not supported");