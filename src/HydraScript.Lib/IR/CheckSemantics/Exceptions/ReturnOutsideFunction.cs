using System.Diagnostics.CodeAnalysis;

namespace HydraScript.Lib.IR.CheckSemantics.Exceptions;

[ExcludeFromCodeCoverage]
public class ReturnOutsideFunction(string segment) :
    SemanticException(segment, "\"return\" outside function");