using HydraScript.Lib.IR.CheckSemantics.Variables;

namespace HydraScript.Lib.IR.Ast;

public interface IAbstractSyntaxTreeNode :
    IReadOnlyList<IAbstractSyntaxTreeNode>,
    IVisitable<IAbstractSyntaxTreeNode>
{
    public IAbstractSyntaxTreeNode Parent { get; }
    public ISymbolTable Scope { get; }
    public void InitScope(ISymbolTable? scope = null);
    public IReadOnlyList<IAbstractSyntaxTreeNode> GetAllNodes();
}