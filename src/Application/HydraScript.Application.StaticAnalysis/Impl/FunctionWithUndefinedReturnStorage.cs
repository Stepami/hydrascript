using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations.AfterTypesAreLoaded;
using HydraScript.Domain.IR.Impl.Symbols;
using HydraScript.Domain.IR.Impl.Symbols.Ids;

namespace HydraScript.Application.StaticAnalysis.Impl;

internal class FunctionWithUndefinedReturnStorage : IFunctionWithUndefinedReturnStorage
{
    private readonly OrderedDictionary<FunctionSymbolId, FunctionDeclaration> _declarations = [];

    public void Save(FunctionSymbol symbol, FunctionDeclaration declaration)
    {
        _declarations[symbol.Id] = declaration;
    }

    public FunctionDeclaration Get(FunctionSymbol symbol)
    {
        if (!_declarations.Remove(symbol.Id, out var declaration))
            throw new InvalidOperationException(message: "Cannot get function that has not been saved");

        return declaration;
    }

    public void RemoveIfPresent(FunctionSymbol symbol)
    {
        _declarations.Remove(symbol.Id);
    }

    public IEnumerable<FunctionDeclaration> Flush() => _declarations.Keys.ToList()
        .Select(x =>
        {
            var decl = _declarations[x];
            _declarations.Remove(x);
            return decl;
        });
}