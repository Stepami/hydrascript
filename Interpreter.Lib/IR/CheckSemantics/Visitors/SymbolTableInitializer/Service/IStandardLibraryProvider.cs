using Interpreter.Lib.IR.CheckSemantics.Variables;

namespace Interpreter.Lib.IR.CheckSemantics.Visitors.SymbolTableInitializer.Service;

public interface IStandardLibraryProvider
{
    SymbolTable GetStandardLibrary();
}