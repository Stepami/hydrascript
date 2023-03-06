using Interpreter.Lib.BackEnd.Addresses;

namespace Interpreter.Lib.BackEnd;

public record VirtualMachine(
    Stack<Call> CallStack, Stack<Frame> Frames,
    Stack<(string Id, object Value)> Arguments,
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
    List<(string Id, object Value)> Parameters,
    string Where = null)
{
    public override string ToString() =>
        $"{From} => {To.Start}: {To.Id}({string.Join(", ", Parameters.Select(x => $"{x.Id}: {x.Value}"))})";
}
    
public record FunctionInfo(string Id, string MethodOf = null)
{
    public Label Start =>
        new($"Start_{this}");

    public Label End =>
        new($"End_{this}");

    public string MethodOf { get; set; } = MethodOf;

    private string CallId() =>
        MethodOf == null
            ? Id
            : $"{MethodOf}.{Id}";

    public override string ToString() => CallId();
}
    
public class Frame
{
    private readonly Dictionary<string, object> _variables = new();
    private readonly Frame _parentFrame;

    public IAddress ReturnAddress { get; }

    public Frame(IAddress returnAddress, Frame parentFrame = null) =>
        (ReturnAddress, _parentFrame) = (returnAddress, parentFrame);

    public object this[string id]
    {
        get => _variables.TryGetValue(id, out var value)
            ? value
            : _parentFrame?[id];
        set => _variables[id] = value;
    }
}