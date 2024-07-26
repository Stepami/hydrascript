using HydraScript.Lib.IR.Ast;
using HydraScript.Lib.IR.Ast.Impl.Nodes;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations;
using HydraScript.Lib.IR.CheckSemantics.Exceptions;
using HydraScript.Lib.IR.CheckSemantics.Variables.Symbols;
using HydraScript.Lib.IR.CheckSemantics.Visitors.Services;

namespace HydraScript.Lib.IR.CheckSemantics.Visitors;

public class TypeSystemLoader : VisitorNoReturnBase<AbstractSyntaxTreeNode>,
    IVisitor<ScriptBody>,
    IVisitor<TypeDeclaration>
{
    private readonly ITypeDeclarationsResolver _resolver;
    private readonly ISet<Type> _defaultTypes;

    public TypeSystemLoader(
        ITypeDeclarationsResolver resolver,
        IJavaScriptTypesProvider provider)
    {
        _resolver = resolver;
        _defaultTypes = provider.GetDefaultTypes().ToHashSet();
    }

    public VisitUnit Visit(ScriptBody visitable)
    {
        for (var i = 0; i < visitable.Count; i++)
            visitable[i].Accept(This);
        _resolver.Resolve();
        return default;
    }

    public override VisitUnit Visit(AbstractSyntaxTreeNode visitable)
    {
        for (var i = 0; i < visitable.Count; i++)
            visitable[i].Accept(This);

        return default;
    }

    public VisitUnit Visit(TypeDeclaration visitable)
    {
        if (visitable.SymbolTable.ContainsSymbol(visitable.TypeId) ||
            _defaultTypes.Contains(visitable.TypeId.Name))
            throw new DeclarationAlreadyExists(visitable.TypeId);

        visitable.SymbolTable.AddSymbol(
            new TypeSymbol(
                visitable.TypeId.Name,
                visitable.TypeId));

        _resolver.Store(visitable);
        return default;
    }
}