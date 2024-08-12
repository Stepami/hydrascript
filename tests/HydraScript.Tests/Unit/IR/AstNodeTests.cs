using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes;
using Xunit;
using BlockStatement = HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Statements.BlockStatement;
using FunctionDeclaration = HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations.AfterTypesAreLoaded.FunctionDeclaration;
using IdentifierReference = HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions.IdentifierReference;
using LexicalDeclaration = HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations.AfterTypesAreLoaded.LexicalDeclaration;
using Literal = HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions.Literal;
using ScriptBody = HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.ScriptBody;
using TypeIdentValue = HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations.TypeIdentValue;

namespace HydraScript.Tests.Unit.IR;

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