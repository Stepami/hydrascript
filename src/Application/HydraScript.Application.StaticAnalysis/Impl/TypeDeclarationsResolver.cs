using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations;
using HydraScript.Domain.IR.Impl.Symbols;
using HydraScript.Domain.IR.Impl.Symbols.Ids;

namespace HydraScript.Application.StaticAnalysis.Impl;

internal class TypeDeclarationsResolver(
    IJavaScriptTypesProvider provider,
    ISymbolTableStorage symbolTables,
    IVisitor<TypeValue, Type> typeBuilder) : ITypeDeclarationsResolver
{
    private readonly Queue<TypeDeclaration> _declarationsToResolve = [];

    public void Store(TypeDeclaration declaration) =>
        _declarationsToResolve.Enqueue(declaration);

    public void Resolve()
    {
        var defaults = provider.GetDefaultTypes()
            .Select(x => new TypeSymbol(x))
            .ToList();

        foreach (var declarationToResolve in _declarationsToResolve)
        {
            symbolTables[declarationToResolve.Scope].AddSymbol(
                new TypeSymbol(
                    declarationToResolve.TypeValue.Accept(typeBuilder),
                    declarationToResolve.TypeId));
        }

        while (_declarationsToResolve.Count != 0)
        {
            var declarationToResolve = _declarationsToResolve.Dequeue();

            var typeSymbol = symbolTables[declarationToResolve.Scope]
                .FindSymbol(new TypeSymbolId(declarationToResolve.TypeId))!;

            var resolvingCandidates = symbolTables[declarationToResolve.Scope]
                .GetAvailableSymbols()
                .OfType<TypeSymbol>()
                .Except(defaults);

            foreach (var referenceSymbol in resolvingCandidates)
            {
                typeSymbol.Type.ResolveReference(
                    referenceSymbol.Type,
                    referenceSymbol.Name);
            }
        }
    }
}