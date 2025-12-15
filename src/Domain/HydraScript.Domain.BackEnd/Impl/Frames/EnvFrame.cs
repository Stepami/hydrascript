namespace HydraScript.Domain.BackEnd.Impl.Frames;

public sealed class EnvFrame(IEnvironmentVariableProvider provider) : IFrame
{
    public object? this[string id]
    {
        get => provider.GetEnvironmentVariable(id[1..]);
        set => provider.SetEnvironmentVariable(id[1..], value?.ToString());
    }
}