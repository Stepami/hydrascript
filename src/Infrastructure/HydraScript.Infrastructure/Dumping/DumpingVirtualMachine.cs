using HydraScript.Domain.BackEnd;
using Microsoft.Extensions.DependencyInjection;

namespace HydraScript.Infrastructure.Dumping;

internal class DumpingVirtualMachine(
    [FromKeyedServices(DecoratorKey.Value)]
    IVirtualMachine virtualMachine,
    IDumpingService dumpingService) : IVirtualMachine
{
    public IExecuteParams ExecuteParams => virtualMachine.ExecuteParams;

    public void Run(AddressedInstructions instructions)
    {
        dumpingService.Dump(instructions.ToString(), "tac");
        virtualMachine.Run(instructions);
    }
}