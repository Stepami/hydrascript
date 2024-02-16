using Interpreter.Lib.IR.Ast.Impl.Nodes.Declarations.AfterTypesAreLoaded;
using Interpreter.Lib.IR.CheckSemantics.Variables.Symbols;

namespace Interpreter.Lib.IR.CheckSemantics.Visitors.Services.Impl;

internal class FunctionWithUndefinedReturnStorage : IFunctionWithUndefinedReturnStorage
{
    private readonly Dictionary<string, FunctionDeclaration> _declarations = new();
    private readonly Dictionary<string, int> _keysWithOrder = new();

    public void Save(FunctionSymbol symbol, FunctionDeclaration declaration)
    {
        _declarations[symbol.Id] = declaration;
        _keysWithOrder[symbol.Id] = _declarations.Count;
    }

    public FunctionDeclaration Get(FunctionSymbol symbol)
    {
        if (!_declarations.Remove(symbol.Id, out var declaration))
            throw new InvalidOperationException(message: "Cannot get function that has not been saved");

        _keysWithOrder.Remove(symbol.Id);
        return declaration;
    }

    public void RemoveIfPresent(FunctionSymbol symbol)
    {
        _declarations.Remove(symbol.Id);
        _keysWithOrder.Remove(symbol.Id);
    }

    public IEnumerable<FunctionDeclaration> Flush() => _declarations
        .OrderBy(kvp => _keysWithOrder[kvp.Key])
        .Select(x =>
        {
            _declarations.Remove(x.Key);
            _keysWithOrder.Remove(x.Key);
            return x.Value;
        });
}