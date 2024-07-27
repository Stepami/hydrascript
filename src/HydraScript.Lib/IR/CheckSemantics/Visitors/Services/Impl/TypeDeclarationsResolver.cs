using HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations;
using HydraScript.Lib.IR.CheckSemantics.Variables.Impl.Symbols;

namespace HydraScript.Lib.IR.CheckSemantics.Visitors.Services.Impl;

public class TypeDeclarationsResolver : ITypeDeclarationsResolver
{
    private readonly Queue<TypeDeclaration> _declarationsToResolve = new();
    private readonly IJavaScriptTypesProvider _provider;
    private readonly IVisitor<TypeValue, Type> _typeBuilder = new TypeBuilder();

    public TypeDeclarationsResolver(IJavaScriptTypesProvider provider) =>
        _provider = provider;

    public void Store(TypeDeclaration declaration) =>
        _declarationsToResolve.Enqueue(declaration);

    public void Resolve()
    {
        var defaults = _provider.GetDefaultTypes()
            .Select(x => new TypeSymbol(x))
            .ToList();

        foreach (var declarationToResolve in _declarationsToResolve)
        {
            declarationToResolve.Scope.AddSymbol(
                new TypeSymbol(
                    declarationToResolve.TypeValue.Accept(_typeBuilder),
                    declarationToResolve.TypeId));
        }

        while (_declarationsToResolve.Count != 0)
        {
            var declarationToResolve = _declarationsToResolve.Dequeue();

            var typeSymbol = declarationToResolve.Scope
                .FindSymbol<TypeSymbol>(declarationToResolve.TypeId)!;

            var resolvingCandidates = declarationToResolve.Scope
                .GetAvailableSymbols()
                .OfType<TypeSymbol>()
                .Except(defaults);

            foreach (var referenceSymbol in resolvingCandidates)
            {
                typeSymbol.Type.ResolveReference(
                    referenceSymbol.Type,
                    referenceSymbol.Id);
            }
        }
    }
}