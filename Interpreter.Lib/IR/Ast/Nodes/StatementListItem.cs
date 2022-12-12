namespace Interpreter.Lib.IR.Ast.Nodes;

public abstract class StatementListItem : AbstractSyntaxTreeNode
{
    public abstract bool IsStatement();
}