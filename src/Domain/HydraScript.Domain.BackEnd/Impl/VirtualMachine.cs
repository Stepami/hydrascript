namespace HydraScript.Domain.BackEnd.Impl;

public class VirtualMachine(
    IOutputWriter writer,
    IFrameContext frameContext) : IVirtualMachine
{
    public IExecuteParams ExecuteParams { get; } = new ExecuteParams(writer, frameContext);

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