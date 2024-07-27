namespace HydraScript.Lib.BackEnd.Impl;

public class VirtualMachine(TextWriter writer) : IVirtualMachine
{
    public IExecuteParams ExecuteParams { get; } = new ExecuteParams(writer);

    public void Run(AddressedInstructions instructions)
    {
        ExecuteParams.Frames.Push(new Frame(instructions.Start));

        var address = instructions.Start;
        while (!instructions[address].End)
        {
            var instruction = instructions[address];
            var jump = instruction.Execute(ExecuteParams);
            address = jump;
        }

        instructions[address].Execute(ExecuteParams);
    }
}