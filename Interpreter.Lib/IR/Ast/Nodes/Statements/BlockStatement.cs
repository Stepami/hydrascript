using Interpreter.Lib.BackEnd.Instructions;

namespace Interpreter.Lib.IR.Ast.Nodes.Statements
{
    public class BlockStatement : Statement
    {
        private readonly List<StatementListItem> _statementList;

        public BlockStatement(IEnumerable<StatementListItem> statementList)
        {
            _statementList = new List<StatementListItem>(statementList);
            _statementList.ForEach(item => item.Parent = this);
        }

        public bool HasReturnStatement()
        {
            var has = _statementList.Any(item => item is ReturnStatement);
            if (!has)
            {
                has = _statementList
                    .Where(item => item.IsStatement())
                    .OfType<IfStatement>()
                    .Any(ifStmt => ifStmt.HasReturnStatement());
            }

            return has;
        }

        public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator() => _statementList.GetEnumerator();

        protected override string NodeRepresentation() => "{}";

        public override List<Instruction> ToInstructions(int start)
        {
            var blockInstructions = new List<Instruction>();
            var offset = start;
            foreach (var item in _statementList)
            {
                var itemInstructions = item.ToInstructions(offset);
                blockInstructions.AddRange(itemInstructions);
                if (item is ReturnStatement)
                {
                    break;
                }

                offset += itemInstructions.Count;
            }

            return blockInstructions;
        }
    }
}