namespace Interpreter.Lib.IR.Ast.Nodes.Declarations;

public abstract class Declaration : StatementListItem
{
    public override bool IsStatement() => false;
}