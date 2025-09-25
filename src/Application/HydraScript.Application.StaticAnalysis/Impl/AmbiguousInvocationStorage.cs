using HydraScript.Application.StaticAnalysis.Exceptions;
using HydraScript.Domain.IR.Impl.Symbols.Ids;

namespace HydraScript.Application.StaticAnalysis.Impl;

internal class AmbiguousInvocationStorage : IAmbiguousInvocationStorage
{
    private readonly Dictionary<FunctionSymbolId, HashSet<FunctionSymbolId>> _invocations = [];

    public void WriteCandidate(FunctionSymbolId invocation, FunctionSymbolId candidate)
    {
        if (!_invocations.ContainsKey(invocation))
            _invocations[invocation] = [];
        _invocations[invocation].Add(candidate);
    }

    public void CheckCandidatesAndThrow(string segment, FunctionSymbolId invocation)
    {
        var candidates = _invocations.GetValueOrDefault(invocation, []);
        if (candidates.Count > 0)
            throw new AmbiguousInvocation(segment, candidates);
    }

    public void Clear(FunctionSymbolId invocation) => _invocations.Remove(invocation);

    public void Clear() => _invocations.Clear();
}