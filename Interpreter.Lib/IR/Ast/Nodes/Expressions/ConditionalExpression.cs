using Interpreter.Lib.BackEnd;
using Interpreter.Lib.IR.Ast.Visitors;
using Interpreter.Lib.IR.CheckSemantics.Exceptions;
using Interpreter.Lib.IR.CheckSemantics.Types;

namespace Interpreter.Lib.IR.Ast.Nodes.Expressions;

public class ConditionalExpression : Expression
{
    public Expression Test { get; }
    public Expression Consequent { get; }
    public Expression Alternate { get; }

    public ConditionalExpression(Expression test, Expression consequent, Expression alternate)
    {
        Test = test;
        Consequent = consequent;
        Alternate = alternate;

        Test.Parent = this;
        Consequent.Parent = this;
        Alternate.Parent = this;
    }

    internal override Type NodeCheck()
    {
        var tType = Test.NodeCheck();

        if (tType.Equals(TypeUtils.JavaScriptTypes.Boolean))
        {
            var cType = Consequent.NodeCheck();
            var aType = Alternate.NodeCheck();
            if (cType.Equals(aType))
            {
                return cType;
            }

            throw new WrongConditionalTypes(Consequent.Segment, cType, Alternate.Segment, aType);
        }

        throw new NotBooleanTestExpression(Test.Segment, tType);
    }

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
    {
        yield return Test;
        yield return Consequent;
        yield return Alternate;
    }

    protected override string NodeRepresentation() => "?:";

    public override AddressedInstructions Accept(ExpressionInstructionProvider visitor) =>
        visitor.Visit(this);
}