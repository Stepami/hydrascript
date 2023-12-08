using Interpreter.Lib.IR.CheckSemantics.Variables;

namespace Interpreter.Lib.IR.CheckSemantics.Visitors.Services;

public interface IStandardLibraryProvider
{
    SymbolTable GetStandardLibrary();
}