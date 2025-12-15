namespace HydraScript.Domain.BackEnd;

public class Frame(Frame? parentFrame = null) : IFrame
{
    private readonly Dictionary<string, object?> _variables = new();

    public object? this[string id]
    {
        get => _variables.TryGetValue(id, out var value)
            ? value
            : parentFrame?[id];
        set => _variables[id] = value;
    }
}