namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes;

public abstract class StatementListItem :
    AbstractSyntaxTreeNode
{
    public abstract override StatementListItem Clone();
}

public abstract class Statement :
    StatementListItem
{
    public abstract override Statement Clone();
}

public abstract class Declaration :
    StatementListItem
{
    public abstract override Declaration Clone();
}