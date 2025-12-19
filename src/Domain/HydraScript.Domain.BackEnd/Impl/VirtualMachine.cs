namespace HydraScript.Domain.BackEnd.Impl;

public class VirtualMachine(
    IConsole console,
    IFrameContext frameContext) : IVirtualMachine
{
    public IExecuteParams ExecuteParams { get; } = new ExecuteParams(console, frameContext);

    public void Run(AddressedInstructions instructions)
    {
        frameContext.StepIn();

        var address = instructions.Start;
        while (address is not null)
        {
            var instruction = instructions[address];
            var jump = instruction.Execute(ExecuteParams);
            address = jump;
        }
    }
}