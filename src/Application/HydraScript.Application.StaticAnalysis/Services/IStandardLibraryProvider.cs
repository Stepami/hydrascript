using HydraScript.Domain.IR;

namespace HydraScript.Application.StaticAnalysis.Services;

public interface IStandardLibraryProvider
{
    ISymbolTable GetStandardLibrary();
}