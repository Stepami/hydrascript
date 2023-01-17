using Interpreter.Lib.BackEnd;
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

    public bool HasReturnStatement()
    {
        var has = StatementList.Any(item => item is ReturnStatement);
        if (!has)
        {
            has = StatementList
                .Where(item => item.IsStatement())
                .OfType<IfStatement>()
                .Any(ifStmt => ifStmt.HasReturnStatement());
        }

        return has;
    }

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator() =>
        StatementList.GetEnumerator();

    protected override string NodeRepresentation() => "{}";

    public override AddressedInstructions Accept(InstructionProvider visitor) =>
        visitor.Visit(this);
}