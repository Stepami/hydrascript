using Interpreter.Lib.IR.Ast.Impl.Nodes.Declarations.AfterTypesAreLoaded;
using Interpreter.Lib.IR.CheckSemantics.Variables.Symbols;

namespace Interpreter.Lib.IR.CheckSemantics.Visitors.Services;

public interface IFunctionWithUndefinedReturnStorage
{
    void Save(FunctionSymbol symbol, FunctionDeclaration declaration);

    FunctionDeclaration Get(FunctionSymbol symbol);

    void RemoveIfPresent(FunctionSymbol symbol);

    IEnumerable<FunctionDeclaration> Flush();
}