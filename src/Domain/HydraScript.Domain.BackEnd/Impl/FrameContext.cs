using HydraScript.Domain.BackEnd.Impl.Frames;

namespace HydraScript.Domain.BackEnd.Impl;

internal sealed class FrameContext : IFrameContext
{
    private readonly Stack<IFrame> _frames = [];

    public IFrame Current => _frames.Peek();

    public void StepIn() => _frames.Push(
        _frames.TryPeek(out var frame)
            ? new Frame(frame)
            : new Frame());

    public IFrame StepOut() => _frames.Pop();
}