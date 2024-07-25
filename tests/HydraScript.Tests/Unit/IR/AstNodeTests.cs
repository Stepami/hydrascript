using HydraScript.Lib.IR.Ast.Impl.Nodes;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations.AfterTypesAreLoaded;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Statements;
using Xunit;

namespace HydraScript.Tests.Unit.IR;

public class AstNodeTests
{
    [Fact]
    public void PrecedenceTest()
    {
        var lexicalDecl = new LexicalDeclaration(false);
        var stmtItemList = new List<StatementListItem>
        {
            lexicalDecl
        };
        // ReSharper disable once UnusedVariable
        var func = new FunctionDeclaration(
            name: new IdentifierReference(name: Guid.NewGuid().ToString()),
            new TypeIdentValue(
                TypeId: new IdentifierReference(
                    name: Guid.NewGuid().ToString())),
            arguments: new List<PropertyTypeValue>(),
            new BlockStatement(stmtItemList));

        Assert.True(lexicalDecl.ChildOf<FunctionDeclaration>());
    }
}