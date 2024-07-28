using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations;
using HydraScript.Domain.IR.Impl.Symbols;

namespace HydraScript.Application.StaticAnalysis.Impl;

internal class TypeDeclarationsResolver : ITypeDeclarationsResolver
{
    private readonly Queue<TypeDeclaration> _declarationsToResolve = [];

    private readonly IJavaScriptTypesProvider _provider;
    private readonly ISymbolTableStorage _symbolTables;
    private readonly IVisitor<TypeValue, Type> _typeBuilder;

    public TypeDeclarationsResolver(
        IJavaScriptTypesProvider provider,
        ISymbolTableStorage symbolTables,
        IVisitor<TypeValue, Type> typeBuilder)
    {
        _provider = provider;
        _symbolTables = symbolTables;
        _typeBuilder = typeBuilder;
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