using Interpreter.Lib.FrontEnd.GetTokens.Data;

namespace Interpreter.Services.Providers.StructureProvider;

public interface IStructureProvider
{
    Structure CreateStructure();
}