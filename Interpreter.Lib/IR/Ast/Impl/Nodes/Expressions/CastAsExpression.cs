using Interpreter.Lib.BackEnd;
using Interpreter.Lib.IR.Ast.Visitors;
using Interpreter.Lib.IR.CheckSemantics.Types;

namespace Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions;

public class CastAsExpression : Expression
{
    public Expression Expression { get; }
    private readonly Type _cast;

    public CastAsExpression(Expression expression, Type cast)
    {
        Expression = expression;
        Expression.Parent = this;

        _cast = cast;
    }

    internal override Type NodeCheck() => 
        TypeUtils.JavaScriptTypes.String;

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
    {
        yield return Expression;
    }

    protected override string NodeRepresentation() => $"as {_cast}";

    public override AddressedInstructions Accept(ExpressionInstructionProvider visitor) =>
        visitor.Visit(this);
}