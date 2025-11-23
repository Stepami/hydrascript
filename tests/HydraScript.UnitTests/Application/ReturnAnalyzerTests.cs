using HydraScript.Application.StaticAnalysis.Visitors;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations.AfterTypesAreLoaded;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Statements;

namespace HydraScript.UnitTests.Application;

public class ReturnAnalyzerTests
{
    /// <summary>
    /// <code>
    /// function f(b: boolean) {
    ///     if (b)
    ///         return 1
    /// }
    /// </code>
    /// </summary>
    [Fact]
    public void Visit_FunctionWithMissingReturn_CodePathEndedWithReturnIsFalse()
    {
        // Arrange
        var functionDeclaration = new FunctionDeclaration(
            new IdentifierReference("f"),
            new TypeIdentValue(new IdentifierReference("undefined")),
            [new NamedArgument("b", new TypeIdentValue(new IdentifierReference("boolean")))],
            new BlockStatement([
                new IfStatement(
                    new IdentifierReference("b"),
                    new ReturnStatement(
                        new Literal(new TypeIdentValue(new IdentifierReference("number")), 1, "segment")))
            ]),
            indexOfFirstDefaultArgument: int.MaxValue);

        // Act
        var result = new ReturnAnalyzer().Visit(functionDeclaration);

        // Assert
        result.CodePathEndedWithReturn.Should().BeFalse();
        result.ReturnStatements.Count.Should().Be(1);
        result.ReturnStatements[0].Expression.Should().BeOfType<Literal>();
    }
}