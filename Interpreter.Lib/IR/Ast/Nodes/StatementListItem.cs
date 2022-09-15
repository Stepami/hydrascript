namespace Interpreter.Lib.IR.Ast.Nodes
{
    public abstract class StatementListItem : AbstractSyntaxTreeNode
    {
        public virtual bool IsStatement() => false;

        public virtual bool IsDeclaration() => false;
    }
}