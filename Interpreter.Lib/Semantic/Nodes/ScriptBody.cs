using System.Collections.Generic;

namespace Interpreter.Lib.Semantic.Nodes
{
    public class ScriptBody : AbstractSyntaxTreeNode
    {
        private readonly List<StatementListItem> statementList;

        public ScriptBody(IEnumerable<StatementListItem> statementList)
        {
            this.statementList = new List<StatementListItem>(statementList);
            this.statementList.ForEach(item => item.Parent = this);
        }

        public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator() => statementList.GetEnumerator();

        protected override string NodeRepresentation() => "Script";
    }
}