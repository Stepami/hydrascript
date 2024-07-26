using System.Diagnostics.CodeAnalysis;

namespace HydraScript.Lib.IR.CheckSemantics.Exceptions;

[ExcludeFromCodeCoverage]
public class FunctionWithoutReturnStatement(string segment) :
    SemanticException(segment, "function with non-void return type must have a return statement");