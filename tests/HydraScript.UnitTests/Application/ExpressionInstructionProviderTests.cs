using HydraScript.Application.CodeGeneration.Impl;
using HydraScript.Application.CodeGeneration.Visitors;
using HydraScript.Domain.BackEnd;
using HydraScript.Domain.FrontEnd.Parser;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions;

namespace HydraScript.UnitTests.Application;

public class ExpressionInstructionProviderTests(ITestOutputHelper testOutputHelper)
{
    private readonly ExpressionInstructionProvider _expressionInstructionProvider = new(new ValueDtoConverter());

    [Theory, ClassData(typeof(WithExpressionData))]
    public void Visit_WithExpression_ExpectedInstructions(
        IAbstractSyntaxTree ast,
        AddressedInstructions expected)
    {
        var withExpression = ast.Root.GetAllNodes().OfType<WithExpression>().Single();
        var result = _expressionInstructionProvider.Visit(withExpression);
        result.ToString().Should().Be(expected.ToString());
        testOutputHelper.WriteLine(expected.ToString());
    }
}