using Interpreter.Lib.IR.Ast.Impl.Nodes.Declarations;
using Interpreter.Lib.IR.CheckSemantics.Types;
using Interpreter.Lib.IR.CheckSemantics.Variables.Symbols;

namespace Interpreter.Lib.IR.CheckSemantics.Visitors.TypeSystemLoader.Service.Impl;

internal class TypeDeclarationsResolver : ITypeDeclarationsResolver
{
    private readonly Queue<TypeDeclaration> _declarationsToResolve = new();

    public void Store(TypeDeclaration declaration) =>
        _declarationsToResolve.Enqueue(declaration);

    public void Resolve()
    {
        // todo replace with provider
        Type[] defaults =
        {
            new("number"),
            new("boolean"),
            new("string"),
            new NullType(),
            new("undefined"),
            new("void")
        };

        while (_declarationsToResolve.Any())
        {
            var declarationToResolve = _declarationsToResolve.Dequeue();

            var type = declarationToResolve.BuildType();
            declarationToResolve.SymbolTable.AddSymbol(
                new TypeSymbol(
                    type,
                    declarationToResolve.TypeId));

            var resolvingCandidates = declarationToResolve.SymbolTable
                .AvailableSymbolIds
                .Except(defaults.Select(x => x.ToString()));

            foreach (var refId in resolvingCandidates)
            {
                var referenceSymbol = declarationToResolve.SymbolTable
                    .FindSymbol<TypeSymbol>(refId);
                if (referenceSymbol is null)
                    continue;
                type.Accept(new ReferenceResolver(referenceSymbol.Type, refId));
            }
        }
    }
}