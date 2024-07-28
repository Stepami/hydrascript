using HydraScript.Domain.IR;
using HydraScript.Domain.IR.Impl;
using HydraScript.Domain.IR.Impl.Symbols;

namespace HydraScript.Application.StaticAnalysis.Impl;

internal class StandardLibraryProvider : IStandardLibraryProvider
{
    private readonly IJavaScriptTypesProvider _provider;

    public StandardLibraryProvider(IJavaScriptTypesProvider provider) =>
        _provider = provider;

    public ISymbolTable GetStandardLibrary()
    {
        var library = new SymbolTable();

        foreach (var type in _provider.GetDefaultTypes())
            library.AddSymbol(new TypeSymbol(type));

        var print = new FunctionSymbol(
            "print",
            [new VariableSymbol("str", "string")],
            "void",
            isEmpty: false);

        library.AddSymbol(print);

        var symbolTable = new SymbolTable();
        symbolTable.AddOpenScope(library);
        return symbolTable;
    }
}