using Interpreter.Lib.IR.CheckSemantics.Types;
using Interpreter.Lib.IR.CheckSemantics.Variables;
using Interpreter.Lib.IR.CheckSemantics.Variables.Symbols;

namespace Interpreter.Lib.IR.CheckSemantics.Visitors.Services.Impl;

internal class StandardLibraryProvider : IStandardLibraryProvider
{
    private readonly IJavaScriptTypesProvider _provider;

    public StandardLibraryProvider(IJavaScriptTypesProvider provider) =>
        _provider = provider;

    public SymbolTable GetStandardLibrary()
    {
        var library = new SymbolTable();

        foreach (var type in _provider.GetDefaultTypes())
        {
            library.AddSymbol(new TypeSymbol(type));
        }

        var print = new FunctionSymbol(
            "print",
            new List<Symbol>
            {
                new VariableSymbol("str", "string")
            },
            new FunctionType(
                "void",
                new Type[] { "string" }),
            isEmpty: false
        );

        library.AddSymbol(print);

        var symbolTable = new SymbolTable();
        symbolTable.AddOpenScope(library);
        return symbolTable;
    }
}