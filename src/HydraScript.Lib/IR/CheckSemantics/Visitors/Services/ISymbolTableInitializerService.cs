using HydraScript.Lib.IR.Ast;

namespace HydraScript.Lib.IR.CheckSemantics.Visitors.Services;

public interface ISymbolTableInitializerService
{
    void InitThroughParent(AbstractSyntaxTreeNode node);

    void InitWithNewScope(AbstractSyntaxTreeNode node);
}