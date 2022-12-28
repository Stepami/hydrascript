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
        Frames.Push(new Frame());

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
    int From, FunctionInfo To, 
    List<(string Id, object Value)> Parameters,
    string Where = null)
{
    public override string ToString() =>
        $"{From} => {To.Location}: {To.Id}({string.Join(", ", Parameters.Select(x => $"{x.Id}: {x.Value}"))})";
}
    
public record FunctionInfo(string Id, int Location = 0, string MethodOf = null)
{
    public int Location { get; set; } = Location;

    public string MethodOf { get; set; } = MethodOf;

    public string CallId() =>
        MethodOf == null
            ? Id
            : $"{MethodOf}.{Id}";

    public override string ToString() =>
        $"({Location}, {CallId()})";
}
    
public class Frame
{
    private readonly Dictionary<string, object> _variables = new();
    private readonly Frame _parentFrame;

    public int ReturnAddress { get; }

    public Frame(int returnAddress = 0, Frame parentFrame = null)
    {
        ReturnAddress = returnAddress;
        _parentFrame = parentFrame;
    }

    public object this[string id]
    {
        get => _variables.ContainsKey(id)
            ? _variables[id]
            : _parentFrame?[id];
        set => _variables[id] = value;
    }
}