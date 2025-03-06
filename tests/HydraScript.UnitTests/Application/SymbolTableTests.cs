using HydraScript.Domain.IR;
using HydraScript.Domain.IR.Impl;

namespace HydraScript.UnitTests.Application;

public class SymbolTableTests
{
    [Theory, AutoHydraScriptData]
    public void FindSymbolTest(ISymbol symbol)
    {
        var id = symbol.Id;

        var outerScope = new SymbolTable();
        var innerScope = new SymbolTable();
        outerScope.AddSymbol(symbol);
        innerScope.AddOpenScope(outerScope);

        Assert.NotNull(innerScope.FindSymbol<ISymbol>(id));
        Assert.True(outerScope.ContainsSymbol(id));
    }
}