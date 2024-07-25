using HydraScript.Lib.BackEnd.Addresses;

namespace HydraScript.Lib.BackEnd;

public record VirtualMachine(
    Stack<Call> CallStack, Stack<Frame> Frames,
    Stack<(string Id, object? Value)> Arguments,
    TextWriter Writer
)
{
    public VirtualMachine() :
        this(new(), new(), new(), Console.Out) { }

    public void Run(AddressedInstructions instructions)
    {
        Frames.Push(new Frame(instructions.Start));

        var address = instructions.Start;
        while (!instructions[address].End())
        {
            var instruction = instructions[address];
            var jump = instruction.Execute(this);
            address = jump;
        }

        instructions[address].Execute(this);
    }
}

public record Call(
    IAddress From, FunctionInfo To, 
    List<(string Id, object? Value)> Parameters,
    string? Where = null)
{
    public override string ToString() =>
        $"{From} => {To.Start}: {To.Id}({string.Join(", ", Parameters.Select(x => $"{x.Id}: {x.Value}"))})";
}

public record FunctionInfo(string Id)
{
    public Label Start =>
        new($"Start_{this}");

    public Label End =>
        new($"End_{this}");

    public override string ToString() => Id;
}

public class Frame(IAddress returnAddress, Frame? parentFrame = null)
{
    private readonly Dictionary<string, object?> _variables = new();

    public IAddress ReturnAddress { get; } = returnAddress;

    public object? this[string id]
    {
        get => _variables.TryGetValue(id, out var value)
            ? value
            : parentFrame?[id];
        set => _variables[id] = value;
    }
}