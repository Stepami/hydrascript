using System.Collections.Generic;
using Interpreter.Lib.Semantic.Nodes;
using Interpreter.Lib.Semantic.Nodes.Declarations;
using Interpreter.Lib.Semantic.Nodes.Statements;
using Interpreter.Lib.Semantic.Symbols;
using Moq;
using Xunit;

namespace Interpreter.Tests.Unit
{
    public class AstNodeTests
    {
        [Fact]
        public void PrecedenceTest()
        {
            var funcSymbol = new Mock<FunctionSymbol>("f", new List<Symbol>());

            var lexicalDecl = new LexicalDeclaration(false);
            var stmtItemList = new List<StatementListItem>
            {
                lexicalDecl
            };
            // ReSharper disable once UnusedVariable
            var func = new FunctionDeclaration(funcSymbol.Object, new BlockStatement(stmtItemList));

            Assert.True(lexicalDecl.ChildOf<FunctionDeclaration>());
        }
    }
}