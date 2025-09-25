using HydraScript.Domain.IR.Impl.Symbols.Ids;

namespace HydraScript.Application.StaticAnalysis;

public interface IAmbiguousInvocationStorage
{
    void WriteCandidate(FunctionSymbolId invocation, FunctionSymbolId candidate);

    void CheckCandidatesAndThrow(string segment, FunctionSymbolId invocation);

    void Clear(FunctionSymbolId invocation);

    void Clear();
}