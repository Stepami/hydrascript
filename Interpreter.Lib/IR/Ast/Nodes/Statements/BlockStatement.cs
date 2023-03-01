using Interpreter.Lib.BackEnd.Addresses;
using Interpreter.Lib.IR.Ast.Visitors;

namespace Interpreter.Lib.IR.Ast.Nodes.Statements;

public class BlockStatement : Statement
{
    public List<StatementListItem> StatementList { get; }

    public BlockStatement(IEnumerable<StatementListItem> statementList)
    {
        StatementList = new List<StatementListItem>(statementList);
        StatementList.ForEach(item => item.Parent = this);
    }

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator() =>
        StatementList.GetEnumerator();

    protected override string NodeRepresentation() => "{}";

    public override AddressedInstructions Accept(InstructionProvider visitor) =>
        visitor.Visit(this);
}