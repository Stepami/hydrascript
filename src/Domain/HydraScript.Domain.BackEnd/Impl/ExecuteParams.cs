namespace HydraScript.Domain.BackEnd.Impl;

public class ExecuteParams(
    IOutputWriter textWriter,
    IFrameContext frameContext) : IExecuteParams
{
    public Stack<Call> CallStack { get; } = [];

    public Queue<object?> Arguments { get; } = [];

    public IOutputWriter Writer { get; } = textWriter;

    public IFrameContext FrameContext { get; } = frameContext;
}