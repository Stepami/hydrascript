namespace HydraScript.Lib.BackEnd;

public interface IExecuteParams
{
    public Stack<Call> CallStack { get; }
    public Stack<Frame> Frames { get; }
    public Queue<object?> Arguments { get; }
    public TextWriter Writer { get; }
}