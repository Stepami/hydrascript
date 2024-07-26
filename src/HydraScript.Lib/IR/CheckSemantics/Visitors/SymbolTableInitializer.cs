using HydraScript.Lib.IR.Ast;
using HydraScript.Lib.IR.Ast.Impl.Nodes;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations.AfterTypesAreLoaded;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Statements;
using HydraScript.Lib.IR.CheckSemantics.Visitors.Services;

namespace HydraScript.Lib.IR.CheckSemantics.Visitors;

public class SymbolTableInitializer : VisitorNoReturnBase<AbstractSyntaxTreeNode>,
    IVisitor<ScriptBody>,
    IVisitor<FunctionDeclaration>,
    IVisitor<BlockStatement>
{
    private readonly ISymbolTableInitializerService _initializerService;
    private readonly IStandardLibraryProvider _provider;

    public SymbolTableInitializer(
        ISymbolTableInitializerService initializerService,
        IStandardLibraryProvider provider)
    {
        _initializerService = initializerService;
        _provider = provider;
    }

    public override VisitUnit Visit(AbstractSyntaxTreeNode visitable)
    {
        _initializerService.InitThroughParent(visitable);
        foreach (var child in visitable)
            child.Accept(This);
        return default;
    }

    public VisitUnit Visit(ScriptBody visitable)
    {
        visitable.SymbolTable = _provider.GetStandardLibrary();
        visitable.StatementList.ForEach(item => item.Accept(This));
        return default;
    }

    public VisitUnit Visit(FunctionDeclaration visitable)
    {
        _initializerService.InitWithNewScope(visitable);
        visitable.Statements.Accept(This);
        return default;
    }

    public VisitUnit Visit(BlockStatement visitable)
    {
        _initializerService.InitWithNewScope(visitable);
        visitable.StatementList.ForEach(item => item.Accept(This));
        return default;
    }
}