using HydraScript.Lib.IR.Ast;

namespace HydraScript.Lib.IR.CheckSemantics.Visitors.Services.Impl;

internal class SymbolTableInitializerService : ISymbolTableInitializerService
{
    public void InitThroughParent(AbstractSyntaxTreeNode node) =>
        node.SymbolTable = node.Parent.SymbolTable;

    public void InitWithNewScope(AbstractSyntaxTreeNode node)
    {
        node.SymbolTable = new();
        node.SymbolTable.AddOpenScope(node.Parent.SymbolTable);
    }
}