using HydraScript.Lib.IR.Ast;
using HydraScript.Lib.IR.Ast.Impl.Nodes;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations.AfterTypesAreLoaded;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Statements;
using HydraScript.Lib.IR.CheckSemantics.Visitors.Services;

namespace HydraScript.Lib.IR.CheckSemantics.Visitors;

public class SymbolTableInitializer :
    IVisitor<AbstractSyntaxTreeNode>,
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

    public Unit Visit(AbstractSyntaxTreeNode visitable)
    {
        _initializerService.InitThroughParent(visitable);
        foreach (var child in visitable)
            child.Accept(this);
        return default;
    }

    public Unit Visit(ScriptBody visitable)
    {
        visitable.SymbolTable = _provider.GetStandardLibrary();
        visitable.StatementList.ForEach(item => item.Accept(this));
        return default;
    }

    public Unit Visit(FunctionDeclaration visitable)
    {
        _initializerService.InitWithNewScope(visitable);
        visitable.Statements.Accept(this);
        return default;
    }

    public Unit Visit(BlockStatement visitable)
    {
        _initializerService.InitWithNewScope(visitable);
        visitable.StatementList.ForEach(item => item.Accept(this));
        return default;
    }
}