namespace Interpreter.Lib.IR.Ast.Nodes
{
    public class ScriptBody : AbstractSyntaxTreeNode
    {
        private readonly List<StatementListItem> _statementList;

        public ScriptBody(IEnumerable<StatementListItem> statementList)
        {
            _statementList = new List<StatementListItem>(statementList);
            _statementList.ForEach(item => item.Parent = this);
        }

        public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator() => _statementList.GetEnumerator();

        protected override string NodeRepresentation() => "Script";
    }
}