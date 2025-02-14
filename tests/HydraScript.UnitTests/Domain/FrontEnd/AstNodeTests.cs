using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations.AfterTypesAreLoaded;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Statements;

namespace HydraScript.UnitTests.Domain.FrontEnd;

public class AstNodeTests
{
    [Fact]
    public void PrecedenceTest()
    {
        var lexicalDecl = new LexicalDeclaration(false);
        List<StatementListItem> stmtItemList = [lexicalDecl];
        // ReSharper disable once UnusedVariable
        var func = new FunctionDeclaration(
            name: new IdentifierReference(name: Guid.NewGuid().ToString()),
            new TypeIdentValue(
                TypeId: new IdentifierReference(
                    name: Guid.NewGuid().ToString())),
            arguments: [],
            new BlockStatement(stmtItemList));

        _ = new ScriptBody([func]);

        Assert.True(lexicalDecl.ChildOf<FunctionDeclaration>());
        Assert.False(lexicalDecl.ChildOf<Literal>());
    }
}