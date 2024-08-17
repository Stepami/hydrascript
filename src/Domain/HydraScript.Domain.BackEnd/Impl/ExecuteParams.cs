namespace HydraScript.Domain.BackEnd.Impl;

public class ExecuteParams(IOutputWriter textWriter) : IExecuteParams
{
    public Stack<Call> CallStack { get; } = new();
    public Stack<Frame> Frames { get; } = new();
    public Queue<object?> Arguments { get; } = new();
    public IOutputWriter Writer { get; } = textWriter;
}