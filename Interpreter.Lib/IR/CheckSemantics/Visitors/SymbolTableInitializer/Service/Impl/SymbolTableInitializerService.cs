using Interpreter.Lib.IR.Ast;

namespace Interpreter.Lib.IR.CheckSemantics.Visitors.SymbolTableInitializer.Service.Impl;

public class SymbolTableInitializerService : ISymbolTableInitializerService
{
    public void InitThroughParent(AbstractSyntaxTreeNode node) =>
        node.SymbolTable = node.Parent.SymbolTable;

    public void InitWithNewScope(AbstractSyntaxTreeNode node)
    {
        node.SymbolTable = new();
        node.SymbolTable.AddOpenScope(node.Parent.SymbolTable);
    }
}