using Interpreter.Lib.IR.Ast.Impl.Nodes;
using Interpreter.Lib.IR.CheckSemantics.Variables;
using Interpreter.Lib.IR.CheckSemantics.Variables.Symbols;
using Moq;
using Xunit;

namespace Interpreter.Tests.Unit.IR;

public class SymbolTableTests
{
    [Fact]
    public void FindSymbolTest()
    {
        const string id = "ident";
        var type = new Mock<Type>(id);

        var symbol = new Mock<Symbol>();
        symbol.Setup(s => s.Id).Returns(id);
        symbol.Setup(s => s.Type).Returns(type.Object);

        var outerScope = new SymbolTable();
        var innerScope = new SymbolTable();
        outerScope.AddSymbol(symbol.Object);
        innerScope.AddOpenScope(outerScope);

        Assert.NotNull(innerScope.FindSymbol<Symbol>(id));
        Assert.True(outerScope.ContainsSymbol(id));
    }

    [Fact]
    public void FlatteningScopeTest()
    {
        var table = new SymbolTable();
        var stmtList = new List<StatementListItem>(
            Enumerable.Repeat(
                new Mock<StatementListItem>().Object,
                new Random().Next(10)
            )
        );
        var script = new ScriptBody(stmtList)
        {
            SymbolTable = table
        };
        script.ToList().ForEach(node => node.SymbolTable = table);

        const string id = "ident";
        var type = new Mock<Type>(id);

        var symbol = new Mock<Symbol>();
        symbol.Setup(s => s.Id).Returns(id);
        symbol.Setup(s => s.Type).Returns(type.Object);

        script.SymbolTable.AddSymbol(symbol.Object);

        Assert.All(
            script.ToList(),
            stmtListItem =>
                Assert.True(stmtListItem.SymbolTable.ContainsSymbol(id))
        );
    }
}