using System.Diagnostics.CodeAnalysis;
using HydraScript.Domain.IR.Impl.Symbols.Ids;

namespace HydraScript.Application.StaticAnalysis.Exceptions;

[ExcludeFromCodeCoverage]
public class AmbiguousInvocation(
    string segment,
    IReadOnlyCollection<FunctionSymbolId> candidates) :
    SemanticException(
        segment,
        $"Ambiguous Invocation - Candidates are:\n{string.Join('\n', candidates)}");