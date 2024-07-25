using HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations;
using HydraScript.Lib.IR.CheckSemantics.Variables.Symbols;

namespace HydraScript.Lib.IR.CheckSemantics.Visitors.Services.Impl;

internal class TypeDeclarationsResolver : ITypeDeclarationsResolver
{
    private readonly Queue<TypeDeclaration> _declarationsToResolve = new();
    private readonly IJavaScriptTypesProvider _provider;

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
            declarationToResolve.SymbolTable.AddSymbol(
                new TypeSymbol(
                    declarationToResolve.BuildType(),
                    declarationToResolve.TypeId));
        }

        while (_declarationsToResolve.Count != 0)
        {
            var declarationToResolve = _declarationsToResolve.Dequeue();

            var typeSymbol = declarationToResolve.SymbolTable
                .FindSymbol<TypeSymbol>(declarationToResolve.TypeId)!;

            var resolvingCandidates = declarationToResolve.SymbolTable
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