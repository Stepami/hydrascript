using System.Diagnostics.CodeAnalysis;

namespace HydraScript.Application.StaticAnalysis.Exceptions;

[ExcludeFromCodeCoverage]
public class SymbolIsNotCallable(string symbol, string segment) :
    SemanticException(segment, $"Symbol is not callable: {symbol}");