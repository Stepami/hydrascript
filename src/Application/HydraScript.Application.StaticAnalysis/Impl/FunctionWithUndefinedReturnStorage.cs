using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations.AfterTypesAreLoaded;
using HydraScript.Domain.IR.Impl.Symbols;

namespace HydraScript.Application.StaticAnalysis.Impl;

internal class FunctionWithUndefinedReturnStorage : IFunctionWithUndefinedReturnStorage
{
    private readonly OrderedDictionary<string, FunctionDeclaration> _declarations = [];

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

    public IEnumerable<FunctionDeclaration> Flush() => _declarations
        .Select(x =>
        {
            _declarations.Remove(x.Key);
            return x.Value;
        });
}