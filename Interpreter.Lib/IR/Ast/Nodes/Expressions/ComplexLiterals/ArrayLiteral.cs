using Interpreter.Lib.BackEnd;
using Interpreter.Lib.IR.Ast.Visitors;
using Interpreter.Lib.IR.CheckSemantics.Exceptions;
using Interpreter.Lib.IR.CheckSemantics.Types;

namespace Interpreter.Lib.IR.Ast.Nodes.Expressions.ComplexLiterals;

public class ArrayLiteral : ComplexLiteral
{
    public List<Expression> Expressions { get; }

    public ArrayLiteral(IEnumerable<Expression> expressions)
    {
        Expressions = new List<Expression>(expressions);
        Expressions.ForEach(expr => expr.Parent = this);
    }

    internal override Type NodeCheck()
    {
        if (Expressions.Any())
        {
            var type = Expressions.First().NodeCheck();
            if (Expressions.All(e => e.NodeCheck().Equals(type)))
            {
                return new ArrayType(type);
            }

            throw new WrongArrayLiteralDeclaration(Segment, type);
        }

        return TypeUtils.JavaScriptTypes.Undefined;
    }

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator() =>
        Expressions.GetEnumerator();

    protected override string NodeRepresentation() => "[]";

    public override AddressedInstructions Accept(ExpressionInstructionProvider visitor) =>
        visitor.Visit(this);
}