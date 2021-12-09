namespace Interpreter.Lib.Semantic.Nodes.Statements
{
    public abstract class Statement : StatementListItem
    {
        public override bool IsStatement() => true;
    }
}