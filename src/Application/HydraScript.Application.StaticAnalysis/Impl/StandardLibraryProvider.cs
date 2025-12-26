using HydraScript.Domain.IR;
using HydraScript.Domain.IR.Impl;
using HydraScript.Domain.IR.Impl.Symbols;

namespace HydraScript.Application.StaticAnalysis.Impl;

internal class StandardLibraryProvider(IHydraScriptTypesService typesService) : IStandardLibraryProvider
{
    public ISymbolTable GetStandardLibrary()
    {
        var library = new SymbolTable();

        foreach (var type in typesService.GetDefaultTypes())
            library.AddSymbol(new TypeSymbol(type));

        var symbolTable = new SymbolTable();
        symbolTable.AddOpenScope(library);
        return symbolTable;
    }
}