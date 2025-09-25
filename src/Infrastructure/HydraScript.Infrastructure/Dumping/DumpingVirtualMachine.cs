using System.IO.Abstractions;
using HydraScript.Domain.BackEnd;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HydraScript.Infrastructure.Dumping;

internal class DumpingVirtualMachine(
    [FromKeyedServices(DecoratorKey.Value)]
    IVirtualMachine virtualMachine,
    IFileSystem fileSystem,
    IOptions<FileInfo> inputFile) : IVirtualMachine
{
    public IExecuteParams ExecuteParams => virtualMachine.ExecuteParams;

    public void Run(AddressedInstructions instructions)
    {
        var fileName = inputFile.Value.Name.Split(".js")[0];
        fileSystem.File.WriteAllLines(
            $"{fileName}.tac",
            instructions.Select(i => i.ToString()!));

        virtualMachine.Run(instructions);
    }
}