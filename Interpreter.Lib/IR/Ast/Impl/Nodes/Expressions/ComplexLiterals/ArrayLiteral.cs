using Interpreter.Lib.BackEnd;
using Interpreter.Lib.IR.Ast.Visitors;
using Interpreter.Lib.IR.CheckSemantics.Visitors;

namespace Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions.ComplexLiterals;

public class ArrayLiteral : ComplexLiteral
{
    public List<Expression> Expressions { get; }

    public ArrayLiteral(IEnumerable<Expression> expressions)
    {
        Expressions = new List<Expression>(expressions);
        Expressions.ForEach(expr => expr.Parent = this);
    }

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator() =>
        Expressions.GetEnumerator();

    protected override string NodeRepresentation() => "[]";

    public override Type Accept(SemanticChecker visitor) =>
        visitor.Visit(this);

    public override AddressedInstructions Accept(ExpressionInstructionProvider visitor) =>
        visitor.Visit(this);
}