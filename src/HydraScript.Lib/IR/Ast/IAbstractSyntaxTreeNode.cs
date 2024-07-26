using HydraScript.Lib.IR.CheckSemantics.Variables;

namespace HydraScript.Lib.IR.Ast;

public interface IAbstractSyntaxTreeNode :
    IReadOnlyList<IAbstractSyntaxTreeNode>,
    IVisitable<IAbstractSyntaxTreeNode>
{
    public IAbstractSyntaxTreeNode Parent { get; }
    public SymbolTable SymbolTable { get; }
    public void InitScope(SymbolTable? scope = null);
    public IReadOnlyList<IAbstractSyntaxTreeNode> GetAllNodes();
}