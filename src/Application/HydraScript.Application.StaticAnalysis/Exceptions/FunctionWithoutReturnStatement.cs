using System.Diagnostics.CodeAnalysis;

namespace HydraScript.Application.StaticAnalysis.Exceptions;

[ExcludeFromCodeCoverage]
public class FunctionWithoutReturnStatement(string segment) :
    SemanticException(segment, "function with non-void return type must have a return statement");