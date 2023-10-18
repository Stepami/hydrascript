using Interpreter.Lib.IR.CheckSemantics.Types;
using Interpreter.Lib.IR.CheckSemantics.Variables;
using Interpreter.Lib.IR.CheckSemantics.Variables.Symbols;

namespace Interpreter.Lib.IR.CheckSemantics.Visitors.SymbolTableInitializer.Service.Impl;

internal class StandardLibraryProvider : IStandardLibraryProvider
{
    public SymbolTable GetStandardLibrary()
    {
        var library = new SymbolTable();
            
        library.AddSymbol(new TypeSymbol("number"));
        library.AddSymbol(new TypeSymbol("boolean"));
        library.AddSymbol(new TypeSymbol("string"));
        library.AddSymbol(new TypeSymbol(new NullType()));
        library.AddSymbol(new TypeSymbol("void"));
        library.AddSymbol(new TypeSymbol("undefined"));

        var print = new FunctionSymbol(
            "print",
            new List<Symbol>
            {
                new VariableSymbol("str", "string")
            },
            new FunctionType(
                "void",
                new Type[] {"string"})
        );

        library.AddSymbol(print);

        var symbolTable = new SymbolTable();
        symbolTable.AddOpenScope(library);
        return symbolTable;
    }
}