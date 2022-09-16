namespace Interpreter.Lib.IR.Ast.Nodes.Statements
{
    public abstract class Statement : StatementListItem
    {
        public override bool IsStatement() => true;
    }
}