namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions;

public abstract class Expression : AbstractSyntaxTreeNode
{
    public abstract override TReturn Accept<TReturn>(
        IVisitor<AbstractSyntaxTreeNode, TReturn> visitor);
}