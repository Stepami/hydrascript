using Interpreter.Lib.BackEnd;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Declarations;
using Interpreter.Lib.IR.Ast.Visitors;
using Interpreter.Lib.IR.CheckSemantics.Visitors.SemanticChecker;

namespace Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions;

public class CastAsExpression : Expression
{
    public Expression Expression { get; }
    private readonly TypeValue _cast;

    public CastAsExpression(Expression expression, TypeValue cast)
    {
        Expression = expression;
        Expression.Parent = this;

        _cast = cast;
    }

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
    {
        yield return Expression;
    }

    protected override string NodeRepresentation() => $"as {_cast}";

    public override AddressedInstructions Accept(ExpressionInstructionProvider visitor) =>
        visitor.Visit(this);

    public override Type Accept(SemanticChecker visitor) =>
        _cast.BuildType(SymbolTable) == "string"
            ? "string"
            : throw new NotSupportedException("Other types but 'string' have not been supported for casting yet");
}