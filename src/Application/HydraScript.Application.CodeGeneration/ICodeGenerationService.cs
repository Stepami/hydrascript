using HydraScript.Domain.BackEnd;
using HydraScript.Domain.FrontEnd.Parser;

namespace HydraScript.Application.CodeGeneration;

public interface ICodeGenerationService
{
    public AddressedInstructions GetInstructions(IAbstractSyntaxTree ast);
}