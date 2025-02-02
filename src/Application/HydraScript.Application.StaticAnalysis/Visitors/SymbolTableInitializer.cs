using HydraScript.Domain.FrontEnd.Parser;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations.AfterTypesAreLoaded;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.ComplexLiterals;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Statements;
using HydraScript.Domain.IR.Impl;

namespace HydraScript.Application.StaticAnalysis.Visitors;

internal class SymbolTableInitializer : VisitorNoReturnBase<IAbstractSyntaxTreeNode>,
    IVisitor<ScriptBody>,
    IVisitor<FunctionDeclaration>,
    IVisitor<BlockStatement>,
    IVisitor<ObjectLiteral>
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

    public VisitUnit Visit(ObjectLiteral visitable)
    {
        visitable.InitScope(scope: new Scope());
        _symbolTables.InitWithOpenScope(visitable.Scope);
        for (var i = 0; i < visitable.Count; i++)
            visitable[i].Accept(This);
        return default;
    }
}