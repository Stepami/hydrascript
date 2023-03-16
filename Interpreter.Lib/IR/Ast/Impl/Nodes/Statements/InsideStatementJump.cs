using Interpreter.Lib.BackEnd;
using Interpreter.Lib.IR.Ast.Visitors;
using Interpreter.Lib.IR.CheckSemantics.Exceptions;

namespace Interpreter.Lib.IR.Ast.Impl.Nodes.Statements;

public class InsideStatementJump : Statement
{
    public const string Break = "break";
    public const string Continue = "continue";

    public string Keyword { get; }

    public InsideStatementJump(string keyword)
    {
        CanEvaluate = true;
        Keyword = keyword;
    }

    internal override Type NodeCheck()
    {
        if (!ChildOf<WhileStatement>())
        {
            throw new OutsideOfLoop(Segment, keyword: NodeRepresentation());
        }
        return null;
    }

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
    {
        yield break;
    }

    protected override string NodeRepresentation() => Keyword;

    public override AddressedInstructions Accept(InstructionProvider visitor) =>
        visitor.Visit(this);
}