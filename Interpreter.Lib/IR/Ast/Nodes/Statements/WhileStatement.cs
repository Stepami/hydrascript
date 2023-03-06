using Interpreter.Lib.BackEnd.Instructions;
using Interpreter.Lib.BackEnd.Values;
using Interpreter.Lib.IR.Ast.Nodes.Expressions.PrimaryExpressions;
using Interpreter.Lib.IR.CheckSemantics.Exceptions;
using Interpreter.Lib.IR.CheckSemantics.Types;

namespace Interpreter.Lib.IR.Ast.Nodes.Statements;

public class WhileStatement : Statement
{
    private readonly Expression _condition;
    private readonly Statement _statement;

    public WhileStatement(Expression condition, Statement statement)
    {
        _condition = condition;
        _condition.Parent = this;

        _statement = statement;
        _statement.Parent = this;

        CanEvaluate = true;
    }

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
    {
        yield return _condition;
        yield return _statement;
    }

    internal override Type NodeCheck()
    {
        var condType = _condition.NodeCheck();
        if (!condType.Equals(TypeUtils.JavaScriptTypes.Boolean))
        {
            throw new NotBooleanTestExpression(Segment, condType);
        }

        return condType;
    }

    protected override string NodeRepresentation() => "while";

    public override List<Instruction> ToInstructions(int start)
    {
        var instructions = new List<Instruction>();
        IValue ifNotTest;
        if (!_condition.Primary())
        {
            var conditionInstructions = _condition.ToInstructions(start, "_t");
            ifNotTest = new Name(conditionInstructions.OfType<Simple>().Last().Left);
            instructions.AddRange(conditionInstructions);
        }
        else
        {
            ifNotTest = ((PrimaryExpression) _condition).ToValue();
        }

        var cOffset = start + instructions.Count + 1;
        var loopBody = _statement.ToInstructions(cOffset);
        if (loopBody.Any())
        {
            instructions.Add(new IfNotGoto(ifNotTest, loopBody.Last().Number + 2, cOffset - 1));
            instructions.AddRange(loopBody);
            instructions.Add(new Goto(start, loopBody.Last().Number + 1));

            loopBody
                .OfType<Goto>()
                .Where(g => g.JumpType is not null)
                .ToList()
                .ForEach(j =>
                {
                    if (j.JumpType == InsideLoopStatementType.Break)
                    {
                        j.SetJump(loopBody.Last().Number + 2);
                    }
                    else if (j.JumpType == InsideLoopStatementType.Continue)
                    {
                        j.SetJump(start);
                    }
                });
        }
        else
        {
            instructions.Clear();
        }

        return instructions;
    }
}