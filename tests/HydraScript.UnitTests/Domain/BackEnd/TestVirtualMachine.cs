using HydraScript.Domain.BackEnd;
using HydraScript.Domain.BackEnd.Impl;
using HydraScript.Domain.BackEnd.Impl.Frames;

namespace HydraScript.UnitTests.Domain.BackEnd;

public class TestVirtualMachine(IConsole console, IFrameContext frameContext) :
    VirtualMachine(console, frameContext)
{
    private readonly IFrameContext _frameContext = frameContext;

    public IFrame Frame => new CurrentFrame(_frameContext);
}