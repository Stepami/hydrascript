using System.Diagnostics.CodeAnalysis;

namespace HydraScript.Lib.IR.CheckSemantics.Exceptions;

[ExcludeFromCodeCoverage]
public class SymbolIsNotCallable(string symbol, string segment) :
    SemanticException(segment, $"Symbol is not callable: {symbol}");