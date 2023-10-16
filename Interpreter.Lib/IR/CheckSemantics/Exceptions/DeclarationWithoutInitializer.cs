using System.Diagnostics.CodeAnalysis;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;

namespace Interpreter.Lib.IR.CheckSemantics.Exceptions;

[ExcludeFromCodeCoverage]
public class DeclarationWithoutInitializer : SemanticException
{
    public DeclarationWithoutInitializer(IdentifierReference ident, bool readOnly) :
        base(ident.Segment, $"'{(readOnly ? "const" : "let")}' without initializer: {ident.Name}") { }
}