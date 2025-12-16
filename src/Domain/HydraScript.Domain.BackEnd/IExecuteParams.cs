namespace HydraScript.Domain.BackEnd;

public interface IExecuteParams
{
    public Stack<Call> CallStack { get; }

    public Queue<object?> Arguments { get; }

    public IOutputWriter Writer { get; }

    public IFrameContext FrameContext { get; }
}