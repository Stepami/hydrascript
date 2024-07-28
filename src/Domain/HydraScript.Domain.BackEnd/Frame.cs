namespace HydraScript.Domain.BackEnd;

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