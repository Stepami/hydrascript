namespace HydraScript.Domain.BackEnd.Impl.Frames;

public sealed class EnvFrame(IEnvironmentVariableProvider provider) : IFrame
{
    public object? this[string id]
    {
        get => provider.GetEnvironmentVariable(id);
        set => provider.SetEnvironmentVariable(id, value?.ToString());
    }
}