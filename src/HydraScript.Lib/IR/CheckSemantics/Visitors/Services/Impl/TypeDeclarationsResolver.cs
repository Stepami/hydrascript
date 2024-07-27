using HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations;
using HydraScript.Lib.IR.CheckSemantics.Variables.Impl.Symbols;

namespace HydraScript.Lib.IR.CheckSemantics.Visitors.Services.Impl;

public class TypeDeclarationsResolver : ITypeDeclarationsResolver
{
    private readonly Queue<TypeDeclaration> _declarationsToResolve = new();
    private readonly IJavaScriptTypesProvider _provider;
    private readonly ISymbolTableStorage _symbolTables;
    private readonly IVisitor<TypeValue, Type> _typeBuilder;

    public TypeDeclarationsResolver(
        IJavaScriptTypesProvider provider,
        ISymbolTableStorage symbolTables)
    {
        _provider = provider;
        _symbolTables = symbolTables;
        _typeBuilder = new TypeBuilder(_symbolTables);
    }

    public void Store(TypeDeclaration declaration) =>
        _declarationsToResolve.Enqueue(declaration);

    public void Resolve()
    {
        var defaults = _provider.GetDefaultTypes()
            .Select(x => new TypeSymbol(x))
            .ToList();

        foreach (var declarationToResolve in _declarationsToResolve)
        {
            _symbolTables[declarationToResolve.Scope].AddSymbol(
                new TypeSymbol(
                    declarationToResolve.TypeValue.Accept(_typeBuilder),
                    declarationToResolve.TypeId));
        }

        while (_declarationsToResolve.Count != 0)
        {
            var declarationToResolve = _declarationsToResolve.Dequeue();

            var typeSymbol = _symbolTables[declarationToResolve.Scope]
                .FindSymbol<TypeSymbol>(declarationToResolve.TypeId)!;

            var resolvingCandidates = _symbolTables[declarationToResolve.Scope]
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