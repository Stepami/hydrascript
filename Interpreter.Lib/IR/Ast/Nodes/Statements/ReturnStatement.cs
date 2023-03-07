using Interpreter.Lib.BackEnd;
using Interpreter.Lib.IR.Ast.Nodes.Declarations;
using Interpreter.Lib.IR.Ast.Visitors;
using Interpreter.Lib.IR.CheckSemantics.Exceptions;
using Interpreter.Lib.IR.CheckSemantics.Types;

namespace Interpreter.Lib.IR.Ast.Nodes.Statements;

public class ReturnStatement : Statement
{
    public Expression Expression { get; }

    public ReturnStatement(Expression expression = null)
    {
        Expression = expression;
        CanEvaluate = true;
        if (expression is not null)
        {
            Expression.Parent = this;
        }
    }

    internal override Type NodeCheck()
    {
        if (!ChildOf<FunctionDeclaration>())
        {
            throw new ReturnOutsideFunction(Segment);
        }

        return Expression?.NodeCheck() ?? TypeUtils.JavaScriptTypes.Void;
    }

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
    {
        if (Expression is null)
        {
            yield break;
        }

        yield return Expression;
    }

    protected override string NodeRepresentation() => "return";

    public override AddressedInstructions Accept(InstructionProvider visitor) =>
        visitor.Visit(this);
}