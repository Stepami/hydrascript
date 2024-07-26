using HydraScript.Lib.IR.Ast;
using HydraScript.Lib.IR.Ast.Impl.Nodes;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations.AfterTypesAreLoaded;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Statements;
using HydraScript.Lib.IR.CheckSemantics.Variables;
using HydraScript.Lib.IR.CheckSemantics.Visitors.Services;

namespace HydraScript.Lib.IR.CheckSemantics.Visitors;

public class SymbolTableInitializer : VisitorNoReturnBase<IAbstractSyntaxTreeNode>,
    IVisitor<ScriptBody>,
    IVisitor<FunctionDeclaration>,
    IVisitor<BlockStatement>
{
    private readonly IStandardLibraryProvider _provider;

    public SymbolTableInitializer(IStandardLibraryProvider provider)
    {
        _provider = provider;
    }

    public override VisitUnit Visit(IAbstractSyntaxTreeNode visitable)
    {
        visitable.InitScope();
        for (var i = 0; i < visitable.Count; i++)
            visitable[i].Accept(This);

        return default;
    }

    public VisitUnit Visit(ScriptBody visitable)
    {
        var scope = _provider.GetStandardLibrary();
        visitable.InitScope(scope);
        for (var i = 0; i < visitable.Count; i++)
            visitable[i].Accept(This);
        return default;
    }

    public VisitUnit Visit(FunctionDeclaration visitable)
    {
        visitable.InitScope(scope: new SymbolTable());
        visitable.Statements.Accept(This);
        return default;
    }

    public VisitUnit Visit(BlockStatement visitable)
    {
        visitable.InitScope(scope: new SymbolTable());
        for (var i = 0; i < visitable.Count; i++)
            visitable[i].Accept(This);
        return default;
    }
}