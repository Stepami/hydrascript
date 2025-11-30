namespace HydraScript.Domain.BackEnd.Impl;

public class VirtualMachine(
    IOutputWriter writer,
    IEnvironmentVariableProvider environmentVariableProvider) : IVirtualMachine
{
    public IExecuteParams ExecuteParams { get; } = new ExecuteParams(writer, environmentVariableProvider);

    public void Run(AddressedInstructions instructions)
    {
        ExecuteParams.Frames.Push(new Frame(instructions.Start));

        var address = instructions.Start;
        while (address is not null)
        {
            var instruction = instructions[address];
            var jump = instruction.Execute(ExecuteParams);
            address = jump;
        }
    }
}