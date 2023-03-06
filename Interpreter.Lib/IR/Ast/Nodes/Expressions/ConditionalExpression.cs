using Interpreter.Lib.BackEnd.Instructions;
using Interpreter.Lib.BackEnd.Values;
using Interpreter.Lib.IR.Ast.Nodes.Expressions.PrimaryExpressions;
using Interpreter.Lib.IR.CheckSemantics.Exceptions;
using Interpreter.Lib.IR.CheckSemantics.Types;

namespace Interpreter.Lib.IR.Ast.Nodes.Expressions;

public class ConditionalExpression : Expression
{
    private readonly Expression _test, _consequent, _alternate;

    public ConditionalExpression(Expression test, Expression consequent, Expression alternate)
    {
        _test = test;
        _consequent = consequent;
        _alternate = alternate;

        _test.Parent = this;
        _consequent.Parent = this;
        _alternate.Parent = this;
    }

    internal override Type NodeCheck()
    {
        var tType = _test.NodeCheck();

        if (tType.Equals(TypeUtils.JavaScriptTypes.Boolean))
        {
            var cType = _consequent.NodeCheck();
            var aType = _alternate.NodeCheck();
            if (cType.Equals(aType))
            {
                return cType;
            }

            throw new WrongConditionalTypes(_consequent.Segment, cType, _alternate.Segment, aType);
        }

        throw new NotBooleanTestExpression(_test.Segment, tType);
    }

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
    {
        yield return _test;
        yield return _consequent;
        yield return _alternate;
    }

    protected override string NodeRepresentation() => "?:";

    public List<Instruction> ToInstructions(int start, string temp)
    {
        var instructions = new List<Instruction>();
        IValue ifNotTest;
        if (!_test.Primary())
        {
            var testInstructions = _test.ToInstructions(start, "_t");
            ifNotTest = new Name(testInstructions.OfType<Simple>().Last().Left);
            instructions.AddRange(testInstructions);
        }
        else
        {
            ifNotTest = ((PrimaryExpression) _test).ToValue();
        }

        var cOffset = start + instructions.Count + 1;
        var consequentInstructions = _consequent.ToInstructions(cOffset, temp);

        var aOffset = consequentInstructions.Last().Number + 2;
        var alternateInstructions = _alternate.ToInstructions(aOffset, temp);

        instructions.Add(
            new IfNotGoto(
                ifNotTest, alternateInstructions.First().Number, cOffset - 1
            )
        );
        instructions.AddRange(consequentInstructions);
        instructions.OfType<Simple>().Last().Left = temp;

        instructions.Add(
            new Goto(alternateInstructions.Last().Number + 1, aOffset - 1)
        );
        instructions.AddRange(alternateInstructions);

        return instructions;
    }
}