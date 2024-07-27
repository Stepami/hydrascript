using HydraScript.Lib.IR.Ast;
using HydraScript.Lib.IR.CheckSemantics.Variables;

namespace HydraScript.Lib.IR.CheckSemantics.Visitors.Services;

public interface ISymbolTableStorage
{
    public ISymbolTable this[Scope scope] { get; }

    public void Init(Scope scope, ISymbolTable symbolTable);

    public void InitWithOpenScope(Scope scope);
}