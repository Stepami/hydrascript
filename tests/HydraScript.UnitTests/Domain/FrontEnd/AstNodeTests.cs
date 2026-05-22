using System.Text.RegularExpressions;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations.AfterTypesAreLoaded;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.AccessExpressions;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Statements;

namespace HydraScript.UnitTests.Domain.FrontEnd;

public partial class AstNodeTests
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
        var ifStatement = new IfStatement(
            Literal.Boolean(true),
            new InsideStatementJump("break"));
        ifStatement.Empty.Should().BeFalse();
    }

    [Fact]
    public void Clone_MemberExpressionWithChain_ReturnsDeepCopy()
    {
        // obj.arr[0].x
        var id = new IdentifierReference("obj");
        var dotArr = new DotAccess(new IdentifierReference("arr"));
        var arrIndex = new IndexAccess(Literal.Number(0), dotArr);
        var dotX = new DotAccess(new IdentifierReference("x"), arrIndex);

        var accessChain = new LinkedList<AccessExpression>(
        [
            dotArr,
            arrIndex,
            dotX
        ]);
        var member = new MemberExpression(id, accessChain);

        var clone = member.Clone();

        Assert.NotSame(member, clone);
        Assert.NotSame(member.Id, clone.Id);
        Assert.NotSame(member.AccessChain, clone.AccessChain);

        var memberAst = new AbstractSyntaxTree(member).ToString();
        var cloneAst = new AbstractSyntaxTree(clone).ToString();

        Assert.NotEqual(memberAst, cloneAst);
        Assert.Equal(
            RemoveHashCodeDigits.Replace(memberAst, string.Empty),
            RemoveHashCodeDigits.Replace(cloneAst, string.Empty));
    }

    [GeneratedRegex("[0-9]+")]
    private static partial Regex RemoveHashCodeDigits { get; }
}