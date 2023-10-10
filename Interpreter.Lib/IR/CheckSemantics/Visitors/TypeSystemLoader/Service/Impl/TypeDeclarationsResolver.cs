using Interpreter.Lib.IR.Ast.Impl.Nodes.Declarations;
using Interpreter.Lib.IR.CheckSemantics.Types;
using Interpreter.Lib.IR.CheckSemantics.Variables.Symbols;

namespace Interpreter.Lib.IR.CheckSemantics.Visitors.TypeSystemLoader.Service.Impl;

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

        while (_declarationsToResolve.Any())
        {
            var declarationToResolve = _declarationsToResolve.Dequeue();

            var type = declarationToResolve.BuildType();
            declarationToResolve.SymbolTable.AddSymbol(
                new TypeSymbol(
                    type,
                    declarationToResolve.TypeId));

            var resolvingCandidates = declarationToResolve.SymbolTable
                .GetAvailableSymbols()
                .OfType<TypeSymbol>()
                .Except(defaults);

            foreach (var referenceSymbol in resolvingCandidates)
            {
                type.Accept(new ReferenceResolver(
                    referenceSymbol.Type,
                    referenceSymbol.Id));
            }
        }
    }
}