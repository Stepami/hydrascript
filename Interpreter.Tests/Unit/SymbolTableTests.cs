using System;
using System.Collections.Generic;
using System.Linq;
using Interpreter.Lib.IR.Ast.Nodes;
using Interpreter.Lib.IR.CheckSemantics.Variables;
using Interpreter.Lib.IR.CheckSemantics.Variables.Symbols;
using Moq;
using Xunit;

namespace Interpreter.Tests.Unit
{
    public class SymbolTableTests
    {
        [Fact]
        public void FindSymbolTest()
        {
            const string id = "ident";

            var symbol = new Mock<Symbol>(id);
            symbol.Setup(s => s.Id).Returns(id);

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

            var symbol = new Mock<Symbol>(id);
            symbol.Setup(s => s.Id).Returns(id);

            script.SymbolTable.AddSymbol(symbol.Object);

            Assert.All(
                script.ToList(),
                stmtListItem =>
                    Assert.True(stmtListItem.SymbolTable.ContainsSymbol(id))
            );
        }
    }
}