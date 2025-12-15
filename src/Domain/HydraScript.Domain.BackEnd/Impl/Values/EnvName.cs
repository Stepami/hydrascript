using HydraScript.Domain.BackEnd.Impl.Frames;

namespace HydraScript.Domain.BackEnd.Impl.Values;

public class EnvName(string id, EnvFrame frame) : Name(id, frame)
{
    public override string ToString() => $"${Id}";
}