using HydraScript.Lib.IR.CheckSemantics.Variables;

namespace HydraScript.Lib.IR.CheckSemantics.Visitors.Services;

public interface IStandardLibraryProvider
{
    ISymbolTable GetStandardLibrary();
}