using System.Diagnostics.CodeAnalysis;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;

namespace HydraScript.Lib.IR.CheckSemantics.Exceptions;

[ExcludeFromCodeCoverage]
public class ConstWithoutInitializer(IdentifierReference ident) :
    SemanticException(ident.Segment, $"'const' without initializer: {ident.Name}");