using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations.AfterTypesAreLoaded;
using HydraScript.Domain.IR.Impl.Symbols;

namespace HydraScript.Application.StaticAnalysis;

public interface IFunctionWithUndefinedReturnStorage
{
    public void Save(FunctionSymbol symbol, FunctionDeclaration declaration);

    public FunctionDeclaration Get(FunctionSymbol symbol);

    public void RemoveIfPresent(FunctionSymbol symbol);

    public IEnumerable<FunctionDeclaration> Flush();
}