using HydraScript.Lib.IR.Ast;

namespace HydraScript.Lib.IR.CheckSemantics.Visitors.Services.Impl;

internal class SymbolTableInitializerService : ISymbolTableInitializerService
{
    public void InitThroughParent(IAbstractSyntaxTreeNode node) =>
        node.SymbolTable = node.Parent.SymbolTable;

    public void InitWithNewScope(IAbstractSyntaxTreeNode node)
    {
        node.SymbolTable = new();
        node.SymbolTable.AddOpenScope(node.Parent.SymbolTable);
    }
}