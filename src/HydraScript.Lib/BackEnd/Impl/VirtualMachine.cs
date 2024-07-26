namespace HydraScript.Lib.BackEnd.Impl;

public record VirtualMachine(
    Stack<Call> CallStack, Stack<Frame> Frames,
    Stack<CallArgument> Arguments,
    TextWriter Writer)
{
    public VirtualMachine() :
        this(new(), new(), new(), Console.Out) { }

    public void Run(AddressedInstructions instructions)
    {
        Frames.Push(new Frame(instructions.Start));

        var address = instructions.Start;
        while (!instructions[address].End)
        {
            var instruction = instructions[address];
            var jump = instruction.Execute(this);
            address = jump;
        }

        instructions[address].Execute(this);
    }
}