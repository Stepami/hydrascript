using HydraScript.Lib.IR.CheckSemantics.Variables;
using HydraScript.Lib.IR.CheckSemantics.Variables.Impl;
using Moq;
using Xunit;

namespace HydraScript.Tests.Unit.IR;

public class SymbolTableTests
{
    [Fact]
    public void FindSymbolTest()
    {
        const string id = "ident";
        var type = new Mock<Type>(id);

        var symbol = new Mock<ISymbol>();
        symbol.Setup(s => s.Id).Returns(id);
        symbol.Setup(s => s.Type).Returns(type.Object);

        var outerScope = new SymbolTable();
        var innerScope = new SymbolTable();
        outerScope.AddSymbol(symbol.Object);
        innerScope.AddOpenScope(outerScope);

        Assert.NotNull(innerScope.FindSymbol<ISymbol>(id));
        Assert.True(outerScope.ContainsSymbol(id));
    }
}