using Interpreter.Lib.FrontEnd.GetTokens.Data;

namespace Interpreter.Services.Providers
{
    public interface IStructureProvider
    {
        Structure CreateStructure();
    }
}