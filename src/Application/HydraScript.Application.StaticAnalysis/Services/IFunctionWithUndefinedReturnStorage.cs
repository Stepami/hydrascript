using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations.AfterTypesAreLoaded;
using HydraScript.Domain.IR.Impl.Symbols;

namespace HydraScript.Application.StaticAnalysis.Services;

public interface IFunctionWithUndefinedReturnStorage
{
    void Save(FunctionSymbol symbol, FunctionDeclaration declaration);

    FunctionDeclaration Get(FunctionSymbol symbol);

    void RemoveIfPresent(FunctionSymbol symbol);

    IEnumerable<FunctionDeclaration> Flush();
}