using HydraScript.Domain.FrontEnd.Parser;
using HydraScript.Domain.IR;

namespace HydraScript.Application.StaticAnalysis.Services;

public interface ISymbolTableStorage
{
    public ISymbolTable this[Scope scope] { get; }

    public void Init(Scope scope, ISymbolTable symbolTable);

    public void InitWithOpenScope(Scope scope);
}