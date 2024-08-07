namespace HydraScript.Domain.FrontEnd.Parser;

public interface IAbstractSyntaxTreeNode :
    IReadOnlyList<IAbstractSyntaxTreeNode>,
    IVisitable<IAbstractSyntaxTreeNode>
{
    public IAbstractSyntaxTreeNode Parent { get; }
    public Scope Scope { get; }
    public void InitScope(Scope? scope = null);
    public IReadOnlyList<IAbstractSyntaxTreeNode> GetAllNodes();
}