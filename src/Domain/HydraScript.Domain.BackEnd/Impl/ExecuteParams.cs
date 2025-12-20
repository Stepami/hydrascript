namespace HydraScript.Domain.BackEnd.Impl;

public class ExecuteParams(
    IConsole console,
    IFrameContext frameContext) : IExecuteParams
{
    public Stack<Call> CallStack { get; } = [];

    public Queue<object?> Arguments { get; } = [];

    public IConsole Console { get; } = console;

    public IFrameContext FrameContext { get; } = frameContext;
}