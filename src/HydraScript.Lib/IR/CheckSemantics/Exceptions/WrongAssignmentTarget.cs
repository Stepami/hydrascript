using System.Diagnostics.CodeAnalysis;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions;

namespace HydraScript.Lib.IR.CheckSemantics.Exceptions;

[ExcludeFromCodeCoverage]
public class WrongAssignmentTarget(LeftHandSideExpression lhs) :
    SemanticException(
        lhs.Segment,
        $"Assignment target must be variable, property or indexer. '{lhs.Id.Name}' is {lhs.GetType().Name}");