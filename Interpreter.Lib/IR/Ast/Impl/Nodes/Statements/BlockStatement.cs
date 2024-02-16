using Interpreter.Lib.BackEnd;
using Interpreter.Lib.IR.Ast.Visitors;
using Interpreter.Lib.IR.CheckSemantics.Visitors;

namespace Interpreter.Lib.IR.Ast.Impl.Nodes.Statements;

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

    public override Unit Accept(SymbolTableInitializer visitor) =>
        visitor.Visit(this);

    public override Type Accept(SemanticChecker visitor) =>
        visitor.Visit(this);

    public override AddressedInstructions Accept(InstructionProvider visitor) =>
        visitor.Visit(this);
}