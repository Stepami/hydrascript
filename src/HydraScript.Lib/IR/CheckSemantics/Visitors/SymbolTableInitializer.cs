using HydraScript.Lib.IR.Ast;
using HydraScript.Lib.IR.Ast.Impl.Nodes;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations.AfterTypesAreLoaded;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Statements;
using HydraScript.Lib.IR.CheckSemantics.Variables.Impl;
using HydraScript.Lib.IR.CheckSemantics.Visitors.Services;

namespace HydraScript.Lib.IR.CheckSemantics.Visitors;

public class SymbolTableInitializer : VisitorNoReturnBase<IAbstractSyntaxTreeNode>,
    IVisitor<ScriptBody>,
    IVisitor<FunctionDeclaration>,
    IVisitor<BlockStatement>
{
    private readonly IStandardLibraryProvider _provider;
    private readonly ISymbolTableStorage _symbolTables;

    public SymbolTableInitializer(
        IStandardLibraryProvider provider,
        ISymbolTableStorage symbolTables)
    {
        _provider = provider;
        _symbolTables = symbolTables;
    }

    public override VisitUnit Visit(IAbstractSyntaxTreeNode visitable)
    {
        visitable.InitScope();
        _symbolTables.Init(visitable.Scope, new SymbolTable());
        for (var i = 0; i < visitable.Count; i++)
            visitable[i].Accept(This);

        return default;
    }

    public VisitUnit Visit(ScriptBody visitable)
    {
        visitable.InitScope(new Scope());
        var symbolTable = _provider.GetStandardLibrary();
        _symbolTables.Init(visitable.Scope, symbolTable);
        for (var i = 0; i < visitable.Count; i++)
            visitable[i].Accept(This);
        return default;
    }

    public VisitUnit Visit(FunctionDeclaration visitable)
    {
        visitable.InitScope(scope: new Scope());
        _symbolTables.InitWithOpenScope(visitable.Scope);
        visitable.Statements.Accept(This);
        return default;
    }

    public VisitUnit Visit(BlockStatement visitable)
    {
        visitable.InitScope(scope: new Scope());
        _symbolTables.InitWithOpenScope(visitable.Scope);
        for (var i = 0; i < visitable.Count; i++)
            visitable[i].Accept(This);
        return default;
    }
}