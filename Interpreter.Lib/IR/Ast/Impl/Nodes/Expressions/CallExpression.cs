using Interpreter.Lib.BackEnd;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;
using Interpreter.Lib.IR.Ast.Visitors;
using Interpreter.Lib.IR.CheckSemantics.Visitors;

namespace Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions;

public class CallExpression : LeftHandSideExpression
{
    public MemberExpression Member { get; }
    public List<Expression> Parameters { get; }

    public CallExpression(MemberExpression member, IEnumerable<Expression> expressions)
    {
        Member = member;
        Member.Parent = this;

        Parameters = new List<Expression>(expressions);
        Parameters.ForEach(expr => expr.Parent = this);
    }

    public override IdentifierReference Id =>
        Member.Id;

    public override bool Empty() =>
        Member.Empty();

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
    {
        var nodes = new List<AbstractSyntaxTreeNode>
        {
            Member
        };
        nodes.AddRange(Parameters);
        return nodes.GetEnumerator();
    }

    protected override string NodeRepresentation() => "()";

    public override Type Accept(SemanticChecker visitor) =>
        visitor.Visit(this);

    public override AddressedInstructions Accept(ExpressionInstructionProvider visitor) =>
        visitor.Visit(this);
}