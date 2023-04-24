using Interpreter.Lib.IR.Ast;
using Visitor.NET.Lib.Core;

namespace Interpreter.Lib.IR.CheckSemantics.Visitors.SymbolTableInitializer.Service;

public interface ISymbolTableInitializerService
{
    Unit InitThroughParent(AbstractSyntaxTreeNode node);

    Unit InitWithNewScope(AbstractSyntaxTreeNode node);
}