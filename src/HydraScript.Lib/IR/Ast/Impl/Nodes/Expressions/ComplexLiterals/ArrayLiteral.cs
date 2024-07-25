using HydraScript.Lib.BackEnd;
using HydraScript.Lib.IR.Ast.Visitors;
using HydraScript.Lib.IR.CheckSemantics.Visitors;

namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.ComplexLiterals;

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