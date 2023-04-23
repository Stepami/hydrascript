using Interpreter.Lib.IR.Ast;
using Visitor.NET.Lib.Core;

namespace Interpreter.Lib.IR.CheckSemantics.Initializer.Impl;

public class SymbolTableInitializer : ISymbolTableInitializer
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