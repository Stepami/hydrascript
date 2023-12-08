using Interpreter.Lib.IR.Ast;

namespace Interpreter.Lib.IR.CheckSemantics.Visitors.Services;

public interface ISymbolTableInitializerService
{
    void InitThroughParent(AbstractSyntaxTreeNode node);

    void InitWithNewScope(AbstractSyntaxTreeNode node);
}