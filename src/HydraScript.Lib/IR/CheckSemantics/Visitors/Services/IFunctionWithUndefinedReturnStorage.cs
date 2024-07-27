using HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations.AfterTypesAreLoaded;
using HydraScript.Lib.IR.CheckSemantics.Variables.Impl.Symbols;

namespace HydraScript.Lib.IR.CheckSemantics.Visitors.Services;

public interface IFunctionWithUndefinedReturnStorage
{
    void Save(FunctionSymbol symbol, FunctionDeclaration declaration);

    FunctionDeclaration Get(FunctionSymbol symbol);

    void RemoveIfPresent(FunctionSymbol symbol);

    IEnumerable<FunctionDeclaration> Flush();
}