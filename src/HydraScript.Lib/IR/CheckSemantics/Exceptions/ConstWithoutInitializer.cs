using System.Diagnostics.CodeAnalysis;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;

namespace HydraScript.Lib.IR.CheckSemantics.Exceptions;

[ExcludeFromCodeCoverage]
public class ConstWithoutInitializer : SemanticException
{
    public ConstWithoutInitializer(IdentifierReference ident) :
        base(ident.Segment, $"'const' without initializer: {ident.Name}") { }
}