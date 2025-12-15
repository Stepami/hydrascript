namespace HydraScript.Domain.BackEnd.Impl.Frames;

internal sealed class Frame(IFrame? parentFrame = null) : IFrame
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