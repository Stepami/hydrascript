using System.Diagnostics.CodeAnalysis;
using HydraScript.Lib.FrontEnd.GetTokens.Data;

namespace HydraScript.Lib.IR.CheckSemantics.Exceptions;

[ExcludeFromCodeCoverage]
public class SymbolIsNotCallable : SemanticException
{
    public SymbolIsNotCallable(string symbol, Segment segment) : 
        base(segment, $"Symbol is not callable: {symbol}") { }
}