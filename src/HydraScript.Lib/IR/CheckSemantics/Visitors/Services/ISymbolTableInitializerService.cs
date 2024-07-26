using HydraScript.Lib.IR.Ast;

namespace HydraScript.Lib.IR.CheckSemantics.Visitors.Services;

public interface ISymbolTableInitializerService
{
    void InitThroughParent(IAbstractSyntaxTreeNode node);

    void InitWithNewScope(IAbstractSyntaxTreeNode node);
}