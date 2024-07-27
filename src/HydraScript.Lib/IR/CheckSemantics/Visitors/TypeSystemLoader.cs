using HydraScript.Lib.IR.Ast;
using HydraScript.Lib.IR.Ast.Impl.Nodes;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations;
using HydraScript.Lib.IR.CheckSemantics.Exceptions;
using HydraScript.Lib.IR.CheckSemantics.Variables.Impl.Symbols;
using HydraScript.Lib.IR.CheckSemantics.Visitors.Services;

namespace HydraScript.Lib.IR.CheckSemantics.Visitors;

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