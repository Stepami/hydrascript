using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations.AfterTypesAreLoaded;
using HydraScript.Domain.IR.Impl.Symbols;
using HydraScript.Domain.IR.Impl.Symbols.Ids;

namespace HydraScript.Application.StaticAnalysis.Impl;

internal class FunctionWithUndefinedReturnStorage : IFunctionWithUndefinedReturnStorage
{
#if NET10_0
    private readonly OrderedDictionary<FunctionSymbolId, FunctionDeclaration> _declarations = [];
#else
    private readonly Dictionary<FunctionSymbolId, FunctionDeclaration> _declarations = [];
    private readonly Dictionary<FunctionSymbolId, int> _keysWithOrder = [];
#endif

    public void Save(FunctionSymbol symbol, FunctionDeclaration declaration)
    {
        _declarations[symbol.Id] = declaration;
#if NET10_0
#else
        _keysWithOrder[symbol.Id] = _declarations.Count;
#endif
    }

    public FunctionDeclaration Get(FunctionSymbol symbol)
    {
        if (!_declarations.Remove(symbol.Id, out var declaration))
            throw new InvalidOperationException(message: $"Cannot get {symbol} that has not been saved");
#if NET10_0
#else
        _keysWithOrder.Remove(symbol.Id);
#endif
        return declaration;
    }

    public void RemoveIfPresent(FunctionSymbol symbol)
    {
        _declarations.Remove(symbol.Id);
#if NET10_0
#else
        _keysWithOrder.Remove(symbol.Id);
#endif
    }

    public IEnumerable<FunctionDeclaration> Flush()
    {
#if NET10_0
        IReadOnlyList<FunctionSymbolId> keys = _declarations.Keys;
        while (keys.Count > 0)
        {
            yield return _declarations[keys[0]];
            _declarations.Remove(keys[0]);
        }
#else
        return _declarations.OrderBy(kvp => _keysWithOrder[kvp.Key])
            .Select(x =>
            {
                _declarations.Remove(x.Key);
                _keysWithOrder.Remove(x.Key);
                return x.Value;
            });
#endif
    }
}