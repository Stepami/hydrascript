using System.Diagnostics.CodeAnalysis;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;

namespace Interpreter.Lib.IR.CheckSemantics.Exceptions;

[ExcludeFromCodeCoverage]
public class ConstWithoutInitializer : SemanticException
{
    public ConstWithoutInitializer(IdentifierReference ident) :
        base(ident.Segment, $"'const' without initializer: {ident.Name}") { }
}