using HydraScript.Lib.IR.CheckSemantics.Variables;
using HydraScript.Lib.IR.CheckSemantics.Variables.Impl;
using HydraScript.Lib.IR.CheckSemantics.Variables.Impl.Symbols;

namespace HydraScript.Lib.IR.CheckSemantics.Visitors.Services.Impl;

public class StandardLibraryProvider : IStandardLibraryProvider
{
    private readonly IJavaScriptTypesProvider _provider;

    public StandardLibraryProvider(IJavaScriptTypesProvider provider) =>
        _provider = provider;

    public ISymbolTable GetStandardLibrary()
    {
        var library = new SymbolTable();

        foreach (var type in _provider.GetDefaultTypes())
        {
            library.AddSymbol(new TypeSymbol(type));
        }

        var print = new FunctionSymbol(
            "print",
            [new VariableSymbol("str", "string")],
            "void",
            isEmpty: false
        );

        library.AddSymbol(print);

        var symbolTable = new SymbolTable();
        symbolTable.AddOpenScope(library);
        return symbolTable;
    }
}