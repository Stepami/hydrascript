using HydraScript.Domain.FrontEnd.Lexer;
using HydraScript.Domain.FrontEnd.Lexer.Data;
using HydraScript.Lib.FrontEnd.GetTokens.Data;

namespace HydraScript.Services.Providers.StructureProvider;

public interface IStructureProvider
{
    Structure CreateStructure();
}