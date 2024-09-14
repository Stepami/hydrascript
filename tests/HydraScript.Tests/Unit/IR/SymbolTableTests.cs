using HydraScript.Domain.IR;
using HydraScript.Domain.IR.Impl;
using Xunit;

namespace HydraScript.Tests.Unit.IR;

public class SymbolTableTests
{
    [Fact]
    public void FindSymbolTest()
    {
        const string id = "ident";
        var type = new Type(id);

        var symbol = Substitute.For<ISymbol>();
        symbol.Id.Returns(id);
        symbol.Type.Returns(type);

        var outerScope = new SymbolTable();
        var innerScope = new SymbolTable();
        outerScope.AddSymbol(symbol);
        innerScope.AddOpenScope(outerScope);

        Assert.NotNull(innerScope.FindSymbol<ISymbol>(id));
        Assert.True(outerScope.ContainsSymbol(id));
    }
}