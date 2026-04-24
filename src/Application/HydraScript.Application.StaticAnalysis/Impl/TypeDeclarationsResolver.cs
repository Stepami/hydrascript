using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations;
using HydraScript.Domain.IR.Impl.Symbols;
using HydraScript.Domain.IR.Impl.Symbols.Ids;
using ZLinq;

namespace HydraScript.Application.StaticAnalysis.Impl;

internal class TypeDeclarationsResolver(
    IHydraScriptTypesService typesService,
    ISymbolTableStorage symbolTables,
    IVisitor<TypeValue, Type> typeBuilder) : ITypeDeclarationsResolver
{
    private readonly List<TypeDeclaration> _declarationsToResolve = [];

    public void Store(TypeDeclaration declaration) =>
        _declarationsToResolve.Add(declaration);

    public void Resolve()
    {
        // build phase
        for (var i = 0; i < _declarationsToResolve.Count; i++)
        {
            var typeSymbol = new TypeSymbol(
                _declarationsToResolve[i].TypeValue.Accept(typeBuilder),
                _declarationsToResolve[i].TypeId);
            symbolTables[_declarationsToResolve[i].Scope].AddSymbol(typeSymbol);
        }

        var defaults = TypesService.GetDefaultTypes()
            .AsValueEnumerable()
            .Select(x => new TypeSymbol(x))
            .ToList();

        // resolve phase
        for (var i = 0; i < _declarationsToResolve.Count; i++)
        {
            var symbolTable = symbolTables[_declarationsToResolve[i].Scope];
            var typeSymbol = symbolTable.FindSymbol(new TypeSymbolId(_declarationsToResolve[i].TypeId))!;
            var resolvingCandidates = symbolTable.GetAvailableSymbols()
                .AsValueEnumerable()
                .OfType<TypeSymbol>()
                .Except(defaults);

            foreach (var referenceSymbol in resolvingCandidates)
            {
                typeSymbol.Resolve(referenceSymbol);
            }
        }

        _declarationsToResolve.Clear();
    }

    public IHydraScriptTypesService TypesService { get; } = typesService;
}