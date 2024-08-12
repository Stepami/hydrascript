using HydraScript.Domain.IR;

namespace HydraScript.Application.StaticAnalysis;

public interface IStandardLibraryProvider
{
    public ISymbolTable GetStandardLibrary();
}