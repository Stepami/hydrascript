using System.IO.Abstractions;
using HydraScript.Domain.BackEnd;
using Microsoft.Extensions.Options;

namespace HydraScript.Infrastructure.Logging;

internal class LoggingVirtualMachine(
    IVirtualMachine virtualMachine,
    IFileSystem fileSystem,
    IOptions<InputFile> inputFile) : IVirtualMachine
{
    private readonly InputFile _inputFile = inputFile.Value;

    public IExecuteParams ExecuteParams => virtualMachine.ExecuteParams;

    public void Run(AddressedInstructions instructions)
    {
        var fileName = _inputFile.Info.Name.Split(".js")[0];
        fileSystem.File.WriteAllLines(
            $"{fileName}.tac",
            instructions.Select(i => i.ToString()!));

        virtualMachine.Run(instructions);
    }
}