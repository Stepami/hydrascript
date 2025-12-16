namespace HydraScript.Domain.BackEnd.Impl.Frames;

public sealed class CurrentFrame(IFrameContext frameContext) : IFrame
{
    public object? this[string id]
    {
        get => frameContext.Current[id];
        set => frameContext.Current[id] =  value;
    }
}