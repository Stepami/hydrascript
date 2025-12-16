using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations.AfterTypesAreLoaded;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Statements;

namespace HydraScript.UnitTests.Domain.FrontEnd;

public class AstNodeTests
{
    [Fact]
    public void ChildOf_Precedence_Success()
    {
        var lexicalDecl = new LexicalDeclaration(false);
        List<StatementListItem> stmtItemList = [lexicalDecl];

        var func = new FunctionDeclaration(
            name: new IdentifierReference(name: Guid.NewGuid().ToString()),
            new TypeIdentValue(
                TypeId: new IdentifierReference(
                    name: Guid.NewGuid().ToString())),
            arguments: [],
            new BlockStatement(stmtItemList),
            indexOfFirstDefaultArgument: int.MaxValue);

        _ = new ScriptBody([func]);

        Assert.True(lexicalDecl.ChildOf<FunctionDeclaration>());
        Assert.False(lexicalDecl.ChildOf<Literal>());
    }

    [Fact]
    public void IfStatement_ThenIsNotBlockAndElseIsNull_NotEmpty()
    {
        var ifStatement = new IfStatement(Literal.Boolean(true), new InsideStatementJump("break"));
        ifStatement.Empty.Should().BeFalse();
    }
}