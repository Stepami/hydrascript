using System.Collections.Generic;
using Interpreter.Lib.IR.Ast.Nodes;
using Interpreter.Lib.IR.Ast.Nodes.Declarations;
using Interpreter.Lib.IR.Ast.Nodes.Statements;
using Interpreter.Lib.IR.CheckSemantics.Types;
using Interpreter.Lib.IR.CheckSemantics.Variables.Symbols;
using Moq;
using Xunit;

namespace Interpreter.Tests.Unit
{
    public class AstNodeTests
    {
        [Fact]
        public void PrecedenceTest()
        {
            var fType = new Mock<FunctionType>(new Mock<Type>("").Object, new List<Type>());
            var funcSymbol = new FunctionSymbol("f", new List<Symbol>(), fType.Object);

            var lexicalDecl = new LexicalDeclaration(false);
            var stmtItemList = new List<StatementListItem>
            {
                lexicalDecl
            };
            // ReSharper disable once UnusedVariable
            var func = new FunctionDeclaration(funcSymbol, new BlockStatement(stmtItemList));

            Assert.True(lexicalDecl.ChildOf<FunctionDeclaration>());
        }
    }
}