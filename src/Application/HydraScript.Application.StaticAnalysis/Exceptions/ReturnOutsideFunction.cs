using System.Diagnostics.CodeAnalysis;

namespace HydraScript.Application.StaticAnalysis.Exceptions;

[ExcludeFromCodeCoverage]
public class ReturnOutsideFunction(string segment) :
    SemanticException(segment, "\"return\" outside function");