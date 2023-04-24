using Interpreter.Lib.IR.Ast;
using Visitor.NET.Lib.Core;

namespace Interpreter.Lib.IR.CheckSemantics.Visitors.SymbolTableInitializer.Service.Impl;

public class SymbolTableInitializerService : ISymbolTableInitializerService
{
    public Unit InitThroughParent(AbstractSyntaxTreeNode node)
    {
        node.SymbolTable = node.Parent.SymbolTable;
        return default;
    }

    public Unit InitWithNewScope(AbstractSyntaxTreeNode node)
    {
        node.SymbolTable = new();
        node.SymbolTable.AddOpenScope(node.Parent.SymbolTable);
        return default;
    }
}