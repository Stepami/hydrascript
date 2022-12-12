using Interpreter.Lib.BackEnd.Instructions;
using Interpreter.Lib.IR.Ast.Nodes.Expressions;

namespace Interpreter.Lib.IR.Ast.Nodes.Statements;

public class ExpressionStatement : Statement
{
    private readonly Expression _expression;

    public ExpressionStatement(Expression expression)
    {
        _expression = expression;
        expression.Parent = this;
    }

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
    {
        yield return _expression;
    }

    protected override string NodeRepresentation() => nameof(ExpressionStatement);

    public override List<Instruction> ToInstructions(int start) => _expression.ToInstructions(start);
}