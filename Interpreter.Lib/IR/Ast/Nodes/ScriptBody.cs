using Interpreter.Lib.BackEnd.Instructions;
using Interpreter.Lib.IR.Ast.Visitors;

namespace Interpreter.Lib.IR.Ast.Nodes;

public class ScriptBody : AbstractSyntaxTreeNode
{
    public List<StatementListItem> StatementList { get; }

    public ScriptBody(IEnumerable<StatementListItem> statementList)
    {
        StatementList = new List<StatementListItem>(statementList);
        StatementList.ForEach(item => item.Parent = this);
    }

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator() =>
        StatementList.GetEnumerator();

    protected override string NodeRepresentation() => "Script";

    public override List<Instruction> Accept(InstructionProvider visitor) =>
        visitor.Visit(this);
}