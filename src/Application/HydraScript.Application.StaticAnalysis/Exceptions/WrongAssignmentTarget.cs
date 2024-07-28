using System.Diagnostics.CodeAnalysis;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions;

namespace HydraScript.Application.StaticAnalysis.Exceptions;

[ExcludeFromCodeCoverage]
public class WrongAssignmentTarget(LeftHandSideExpression lhs) :
    SemanticException(
        lhs.Segment,
        $"Assignment target must be variable, property or indexer. '{lhs.Id.Name}' is {lhs.GetType().Name}");