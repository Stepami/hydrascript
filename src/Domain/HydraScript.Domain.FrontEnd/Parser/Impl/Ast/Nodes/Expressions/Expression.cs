namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions;

public abstract class Expression : AbstractSyntaxTreeNode
{
    public abstract Expression Clone();

    public abstract override TReturn Accept<TReturn>(
        IVisitor<IAbstractSyntaxTreeNode, TReturn> visitor);
}