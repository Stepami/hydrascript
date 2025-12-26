using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations;
using HydraScript.Domain.IR.Impl.Symbols;
using ZLinq;

namespace HydraScript.Application.StaticAnalysis.Impl;

internal class TypeDeclarationsResolver(
    IHydraScriptTypesService typesService,
    ISymbolTableStorage symbolTables,
    IVisitor<TypeValue, Type> typeBuilder) : ITypeDeclarationsResolver
{
    private readonly Queue<TypeDeclaration> _declarationsToResolve = [];

    public void Store(TypeDeclaration declaration) =>
        _declarationsToResolve.Enqueue(declaration);

    public void Resolve()
    {
        var defaults = TypesService.GetDefaultTypes()
            .AsValueEnumerable()
            .Select(x => new TypeSymbol(x))
            .ToList();

        while (_declarationsToResolve.Count != 0)
        {
            var declarationToResolve = _declarationsToResolve.Dequeue();
            var typeSymbol = new TypeSymbol(
                declarationToResolve.TypeValue.Accept(typeBuilder),
                declarationToResolve.TypeId);
            symbolTables[declarationToResolve.Scope].AddSymbol(typeSymbol);

            var resolvingCandidates = symbolTables[declarationToResolve.Scope]
                .GetAvailableSymbols()
                .AsValueEnumerable()
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

    public IHydraScriptTypesService TypesService { get; } = typesService;
}