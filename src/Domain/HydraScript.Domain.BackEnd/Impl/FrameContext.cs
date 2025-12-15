using HydraScript.Domain.BackEnd.Impl.Frames;

namespace HydraScript.Domain.BackEnd.Impl;

public sealed class FrameContext : IFrameContext
{
    private readonly Stack<IFrame> _frames = [];

    public IFrame Current => _frames.Peek();

    public void StepIn() => _frames.Push(
        _frames.TryPeek(out var frame)
            ? new Frame(frame)
            : new Frame());

    public void StepOut() => _frames.Pop();
}