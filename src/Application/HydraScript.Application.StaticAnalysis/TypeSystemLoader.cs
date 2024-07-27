using HydraScript.Application.StaticAnalysis.Exceptions;
using HydraScript.Application.StaticAnalysis.Services;
using HydraScript.Domain.FrontEnd.Parser;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations;
using HydraScript.Domain.IR.Impl.Symbols;

namespace HydraScript.Application.StaticAnalysis;

public class TypeSystemLoader : VisitorNoReturnBase<IAbstractSyntaxTreeNode>,
    IVisitor<ScriptBody>,
    IVisitor<TypeDeclaration>
{
    private readonly ITypeDeclarationsResolver _resolver;
    private readonly IJavaScriptTypesProvider _provider;
    private readonly ISymbolTableStorage _symbolTables;

    public TypeSystemLoader(
        ITypeDeclarationsResolver resolver,
        IJavaScriptTypesProvider provider,
        ISymbolTableStorage symbolTables)
    {
        _resolver = resolver;
        _provider = provider;
        _symbolTables = symbolTables;
    }

    public VisitUnit Visit(ScriptBody visitable)
    {
        for (var i = 0; i < visitable.Count; i++)
            visitable[i].Accept(This);
        _resolver.Resolve();
        return default;
    }

    public override VisitUnit Visit(IAbstractSyntaxTreeNode visitable)
    {
        for (var i = 0; i < visitable.Count; i++)
            visitable[i].Accept(This);

        return default;
    }

    public VisitUnit Visit(TypeDeclaration visitable)
    {
        var symbolTable = _symbolTables[visitable.Scope];
        if (symbolTable.ContainsSymbol(visitable.TypeId) ||
            _provider.Contains(visitable.TypeId.Name))
            throw new DeclarationAlreadyExists(visitable.TypeId);

        symbolTable.AddSymbol(
            new TypeSymbol(
                visitable.TypeId.Name,
                visitable.TypeId));

        _resolver.Store(visitable);
        return default;
    }
}