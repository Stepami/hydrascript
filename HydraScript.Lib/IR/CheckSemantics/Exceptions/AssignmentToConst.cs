using System.Diagnostics.CodeAnalysis;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;

namespace HydraScript.Lib.IR.CheckSemantics.Exceptions;

[ExcludeFromCodeCoverage]
public class AssignmentToConst : SemanticException
{
    public AssignmentToConst(IdentifierReference ident) :
        base(ident.Segment,$"Cannot assign to const: {ident.Name}") { }
}