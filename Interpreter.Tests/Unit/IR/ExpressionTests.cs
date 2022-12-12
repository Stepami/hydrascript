using Interpreter.Lib.IR.Ast.Nodes.Expressions;
using Interpreter.Lib.IR.Ast.Nodes.Expressions.PrimaryExpressions;
using Xunit;

namespace Interpreter.Tests.Unit.IR;

public class ExpressionTests
{
    [Fact]
    public void BinaryExpressionTest()
    {
        var number = new Type("number");
            
        var left = new Literal(number, 0);
        var right = new Literal(number, 1);

        var binExpr = new BinaryExpression(left, "-", right);

        var ex = Record.Exception(() => binExpr.SemanticCheck());
        Assert.Null(ex);
    }
}